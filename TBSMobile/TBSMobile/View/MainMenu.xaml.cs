using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using System.Net;
using System.Net.NetworkInformation;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;
        string synccount;

        public MainMenu(string host, string database, string contact, string ipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            
            CheckConnectionContinuously();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        if (CrossConnectivity.Current.IsConnected)
                        {
                            Ping ping = new Ping();
                             PingReply pingresult = ping.Send(ipaddress, 200);

                            if (pingresult.Status.ToString() == "Success")
                            {
                                var db = DependencyService.Get<ISQLiteDB>();
                                var conn = db.GetConnection();

                                var contactchangessql = "SELECT * FROM tblContacts WHERE Supervisor = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getcontactschanges = conn.QueryAsync<ContactsTable>(contactchangessql);
                                var contactchangesresultCount = getcontactschanges.Result.Count;

                                var retaileroutletchangessql = "SELECT * FROM tblRetailerGroup WHERE Supervisor = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getretaileroutletchanges = conn.QueryAsync<RetailerGroupTable>(retaileroutletchangessql);
                                var retaileroutletchangesresultCount = getretaileroutletchanges.Result.Count;

                                var cafchangessql = "SELECT * FROM tblCaf WHERE EmployeeID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getcafchanges = conn.QueryAsync<CAFTable>(cafchangessql);
                                var cafchangesresultCount = getcafchanges.Result.Count;

                                var actchangessql = "SELECT * FROM tblActivity WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getactchanges = conn.QueryAsync<ActivityTable>(actchangessql);
                                var actchangesresultCount = getactchanges.Result.Count;

                                var emailchangessql = "SELECT * FROM tblUserEmail WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getemailchanges = conn.QueryAsync<UserEmailTable>(emailchangessql);
                                var emailchangesresultCount = getemailchanges.Result.Count;

                                if (contactchangesresultCount > 0 || retaileroutletchangesresultCount > 0 || cafchangesresultCount > 0 || emailchangesresultCount > 0)
                                {
                                    var optimalSpeed = 50;
                                    var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
                                    var speeds = CrossConnectivity.Current.Bandwidths;

                                    if (connectionTypes.Any(speed => Convert.ToInt32(speed) < optimalSpeed))
                                    {
                                        lblStatus.Text = "Initializing data sync";
                                        lblStatus.BackgroundColor = Color.FromHex("#27ae60");

                                        var confirm = await DisplayAlert("Auto-sync Connection Warning", "Slow connection detected. Do you want to sync the data?", "Yes", "No");
                                        if (confirm == true)
                                        {
                                            SyncRetailer(host, database, contact, ipaddress);
                                            btnFAF.IsEnabled = false;
                                            btnAH.IsEnabled = false;
                                            btnLogout.IsEnabled = false;
                                            btnUI.IsEnabled = false;
                                            btnPR.IsEnabled = false;
                                            btnRetailer.IsEnabled = false;
                                            btnResend.IsEnabled = false;
                                        }
                                        else
                                        {
                                            lblStatus.Text = "Online - Connected to server";
                                            lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
                                            btnFAF.IsEnabled = true;
                                            btnAH.IsEnabled = true;
                                            btnLogout.IsEnabled = true;
                                            btnUI.IsEnabled = true;
                                            btnPR.IsEnabled = true;
                                            btnRetailer.IsEnabled = true;
                                            btnResend.IsEnabled = true;
                                        }
                                    }
                                    else
                                    {
                                        SyncRetailer(host, database, contact, ipaddress);
                                        btnFAF.IsEnabled = false;
                                        btnAH.IsEnabled = false;
                                        btnLogout.IsEnabled = false;
                                        btnUI.IsEnabled = false;
                                        btnPR.IsEnabled = false;
                                        btnRetailer.IsEnabled = false;
                                        btnResend.IsEnabled = false;
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Online - Connected to server";
                                    lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
                                    btnFAF.IsEnabled = true;
                                    btnAH.IsEnabled = true;
                                    btnLogout.IsEnabled = true;
                                    btnUI.IsEnabled = true;
                                    btnPR.IsEnabled = true;
                                    btnRetailer.IsEnabled = true;
                                    btnResend.IsEnabled = true;
                                }
                            }
                            else
                            {
                                lblStatus.Text = "Online - Server unreachable. Connect to VPN";
                                lblStatus.BackgroundColor = Color.FromHex("#e67e22");
                                btnFAF.IsEnabled = true;
                                btnAH.IsEnabled = true;
                                btnLogout.IsEnabled = true;
                                btnUI.IsEnabled = true;
                                btnPR.IsEnabled = true;
                                btnRetailer.IsEnabled = true;
                                btnResend.IsEnabled = true;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "Offline - Connect to internet";
                            lblStatus.BackgroundColor = Color.FromHex("#e74c3c");
                            btnFAF.IsEnabled = true;
                            btnAH.IsEnabled = true;
                            btnLogout.IsEnabled = true;
                            btnUI.IsEnabled = true;
                            btnPR.IsEnabled = true;
                            btnRetailer.IsEnabled = true;
                            btnResend.IsEnabled = true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void btnFAF_Clicked(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
                        
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 15;

                        if (!locator.IsGeolocationAvailable)
                        {
                            await DisplayAlert("GPS Error", "GPS location not available", "Ok");
                        }
                        else if (!locator.IsGeolocationEnabled)
                        {
                            await DisplayAlert("GPS Error", "GPS location was not enabled", "Ok");
                        }
                        else
                        {
                            Analytics.TrackEvent("Opened Field Activity Form");
                            await Navigation.PushAsync(new FieldActivityForm(host, database, contact, ipaddress));
                        }
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async void btnPR_Clicked(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        Analytics.TrackEvent("Opened Add Prospect Retailer Form");
                        await Navigation.PushAsync(new ProspectRetailerList(host, database, contact, ipaddress));
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async void btnRetailer_Clicked(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        Analytics.TrackEvent("Opened Add Retailer Outlet Form");
                        await Navigation.PushAsync(new RetailerList(host, database, contact, ipaddress));
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
                        Analytics.TrackEvent("Logged Out");

                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public void CheckConnectionContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

                if (string.IsNullOrEmpty(appdate))
                {
                    Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
                }
                else
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        if (CrossConnectivity.Current.IsConnected)
                        {
                            Ping ping = new Ping();
                             PingReply pingresult = ping.Send(ipaddress, 200);

                            if (pingresult.Status.ToString() == "Success")
                            {
                                var db = DependencyService.Get<ISQLiteDB>();
                                var conn = db.GetConnection();

                                var contactchangessql = "SELECT * FROM tblContacts WHERE Supervisor = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getcontactschanges = conn.QueryAsync<ContactsTable>(contactchangessql);
                                var contactchangesresultCount = getcontactschanges.Result.Count;

                                var retaileroutletchangessql = "SELECT * FROM tblRetailerGroup WHERE Supervisor = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getretaileroutletchanges = conn.QueryAsync<RetailerGroupTable>(retaileroutletchangessql);
                                var retaileroutletchangesresultCount = getretaileroutletchanges.Result.Count;

                                var cafchangessql = "SELECT * FROM tblCaf WHERE EmployeeID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getcafchanges = conn.QueryAsync<CAFTable>(cafchangessql);
                                var cafchangesresultCount = getcafchanges.Result.Count;

                                var actchangessql = "SELECT * FROM tblActivity WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getactchanges = conn.QueryAsync<ActivityTable>(actchangessql);
                                var actchangesresultCount = getactchanges.Result.Count;

                                var emailchangessql = "SELECT * FROM tblUserEmail WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                                var getemailchanges = conn.QueryAsync<UserEmailTable>(emailchangessql);
                                var emailchangesresultCount = getemailchanges.Result.Count;

                                if (contactchangesresultCount > 0 || retaileroutletchangesresultCount > 0 || cafchangesresultCount > 0 || emailchangesresultCount > 0)
                                {
                                    var optimalSpeed = 50;
                                    var connectionTypes = CrossConnectivity.Current.ConnectionTypes;

                                    if (connectionTypes.Any(speed => Convert.ToInt32(speed) < optimalSpeed))
                                    {
                                        lblStatus.Text = "Initializing data sync";
                                        lblStatus.BackgroundColor = Color.FromHex("#27ae60");

                                        var confirm = await DisplayAlert("Auto-sync Connection Speed Warning", "Slow connection detected. Do you want to sync the data?  Please do not turn off/lock your device during the syncing process.", "Yes", "No");
                                        if (confirm == true)
                                        {
                                            SyncRetailer(host, database, contact, ipaddress);
                                            btnFAF.IsEnabled = false;
                                            btnAH.IsEnabled = false;
                                            btnLogout.IsEnabled = false;
                                            btnUI.IsEnabled = false;
                                            btnPR.IsEnabled = false;
                                            btnRetailer.IsEnabled = false;
                                            btnResend.IsEnabled = false;
                                        }
                                        else
                                        {
                                            lblStatus.Text = "Online - Connected to server";
                                            lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
                                            btnFAF.IsEnabled = true;
                                            btnAH.IsEnabled = true;
                                            btnLogout.IsEnabled = true;
                                            btnUI.IsEnabled = true;
                                            btnPR.IsEnabled = true;
                                            btnRetailer.IsEnabled = true;
                                            btnResend.IsEnabled = true;
                                        }
                                    }
                                    else
                                    {
                                        SyncRetailer(host, database, contact, ipaddress);
                                        btnFAF.IsEnabled = false;
                                        btnAH.IsEnabled = false;
                                        btnLogout.IsEnabled = false;
                                        btnUI.IsEnabled = false;
                                        btnPR.IsEnabled = false;
                                        btnRetailer.IsEnabled = false;
                                        btnResend.IsEnabled = false;
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Online - Connected to server";
                                    lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
                                    btnFAF.IsEnabled = true;
                                    btnAH.IsEnabled = true;
                                    btnLogout.IsEnabled = true;
                                    btnUI.IsEnabled = true;
                                    btnPR.IsEnabled = true;
                                    btnRetailer.IsEnabled = true;
                                    btnResend.IsEnabled = true;
                                }
                            }
                            else
                            {
                                lblStatus.Text = "Online - Server unreachable. Connect to VPN";
                                lblStatus.BackgroundColor = Color.FromHex("#e67e22");
                                btnFAF.IsEnabled = true;
                                btnAH.IsEnabled = true;
                                btnLogout.IsEnabled = true;
                                btnUI.IsEnabled = true;
                                btnPR.IsEnabled = true;
                                btnRetailer.IsEnabled = true;
                                btnResend.IsEnabled = true;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "Offline - Connect to internet";
                            lblStatus.BackgroundColor = Color.FromHex("#e74c3c");
                            btnFAF.IsEnabled = true;
                            btnAH.IsEnabled = true;
                            btnLogout.IsEnabled = true;
                            btnUI.IsEnabled = true;
                            btnPR.IsEnabled = true;
                            btnRetailer.IsEnabled = true;
                            btnResend.IsEnabled = true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                
            };
        }

        private async void btnUI_Clicked(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        await Navigation.PushAsync(new UnsyncedData(host, database, contact, ipaddress));
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async void btnAH_Clicked(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        Analytics.TrackEvent("Opened Activity History");
                        await Navigation.PushAsync(new ActivityHistoryList(host, database, contact, ipaddress));
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public class UserData
        {
            public string ContactID { get; set; }
            public string UserID { get; set; }
            public string UsrPassword { get; set; }
            public string UserTypeID { get; set; }
            public string UserStatus { get; set; }
            public string RecordLog { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class ContactsData
        {
            public string ContactID { get; set; }
            public string FileAs { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Position { get; set; }
            public string Company { get; set; }
            public string CompanyID { get; set; }
            public string RetailerType { get; set; }
            public string PresStreet { get; set; }
            public string PresBarangay { get; set; }
            public string PresDistrict { get; set; }
            public string PresTown { get; set; }
            public string PresProvince { get; set; }
            public string PresCountry { get; set; }
            public string Landmark { get; set; }
            public string CustomerRemarks { get; set; }
            public DateTime RecordDate { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Telephone1 { get; set; }
            public string Telephone2 { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Photo1 { get; set; }
            public string Photo2 { get; set; }
            public string Photo3 { get; set; }
            public string Video { get; set; }
            public string MobilePhoto1 { get; set; }
            public string MobilePhoto2 { get; set; }
            public string MobilePhoto3 { get; set; }
            public string MobileVideo { get; set; }
            public int Employee { get; set; }
            public int Customer { get; set; }
            public string Supervisor { get; set; }
            public string RecordLog { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class RetailerGroupData
        {
            public string RetailerCode { get; set; }
            public string TemporaryRetailerCode { get; set; }
            public string ContactID { get; set; }
            public string PresStreet { get; set; }
            public string PresBarangay { get; set; }
            public string PresDistrict { get; set; }
            public string PresTown { get; set; }
            public string PresProvince { get; set; }
            public string PresCountry { get; set; }
            public string Telephone1 { get; set; }
            public string Telephone2 { get; set; }
            public string Mobile { get; set; }
            public string HomePhone { get; set; }
            public string Email { get; set; }
            public string Landmark { get; set; }
            public string GPSCoordinates { get; set; }
            public string Supervisor { get; set; }
            public string RecordLog { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class CAFData
        {
            public string CAFNo { get; set; }
            public string EmployeeID { get; set; }
            public DateTime CAFDate { get; set; }
            public string CustomerID { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Photo1 { get; set; }
            public string Photo2 { get; set; }
            public string Photo3 { get; set; }
            public string Video { get; set; }
            public string MobilePhoto1 { get; set; }
            public string MobilePhoto2 { get; set; }
            public string MobilePhoto3 { get; set; }
            public string MobileVideo { get; set; }
            public string Remarks { get; set; }
            public string OtherConcern { get; set; }
            public string GPSCoordinates { get; set; }
            public string RecordLog { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class ActivityData
        {
            public string CAFNo { get; set; }
            public string ContactID { get; set; }
            public string ActivityID { get; set; }
            public string RecordLog { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class EmailData
        {
            public string ContactID { get; set; }
            public string Email { get; set; }
            public DateTime LastSync { get; set; }
            public string RecordLog { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class UserLogsData
        {
            public string ContactID { get; set; }
            public string LogType { get; set; }
            public string Log { get; set; }
            public DateTime LogDate { get; set; }
            public string DatabaseName { get; set; }
            public DateTime LastSync { get; set; }
            public DateTime LastUpdated { get; set; }
            public int Deleted { get; set; }
        }

        public class ServerMessage
        {
            public string Message { get; set; }
        }

        public async void SyncRetailer(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing retailer sync";

                    try
                    {
                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        int count = 1;

                        var getContactsChanges = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getContactsChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending retailer changes to server " + count + " out of " + changesresultCount;

                                var crresult = getContactsChanges.Result[i];
                                var crcontactID = crresult.ContactID;
                                var crfileAs = crresult.FileAs;
                                var crfirstName = crresult.FirstName;
                                var crmiddleName = crresult.MiddleName;
                                var crlastName = crresult.LastName;
                                var crposition = crresult.Position;
                                var crcompany = crresult.Company;
                                var crcompanyID = crresult.CompanyID;
                                var crretailerType = crresult.RetailerType;
                                var crpresStreet = crresult.PresStreet;
                                var crpresBarangay = crresult.PresBarangay;
                                var crpresDistrict = crresult.PresDistrict;
                                var crpresTown = crresult.PresTown;
                                var crpresProvince = crresult.PresProvince;
                                var crpresCountry = crresult.PresCountry;
                                var crlandmark = crresult.Landmark;
                                var crremarks = crresult.CustomerRemarks;
                                var crrecordDate = crresult.RecordDate;
                                var crstartTime = crresult.StartTime;
                                var crendTime = crresult.EndTime;
                                var crtelephone1 = crresult.Telephone1;
                                var crtelephone2 = crresult.Telephone2;
                                var crmobile = crresult.Mobile;
                                var cremail = crresult.Email;
                                var crphoto1 = crresult.Photo1;
                                var crphoto2 = crresult.Photo2;
                                var crphoto3 = crresult.Photo3;
                                var crvideo = crresult.Video;
                                var crmobilePhoto1 = crresult.MobilePhoto1;
                                var crmobilePhoto2 = crresult.MobilePhoto2;
                                var crmobilePhoto3 = crresult.MobilePhoto3;
                                var crmobileVideo = crresult.MobileVideo;
                                var cremployee = crresult.Employee;
                                var crcustomer = crresult.Customer;
                                var crsupervisor = crresult.Supervisor;
                                var crrecordlog = crresult.RecordLog;
                                var crdeleted = crresult.Deleted;
                                var crlastUpdated = crresult.LastUpdated;

                                var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=nLm8YE";
                                string crcontentType = "application/json";
                                JObject crjson = new JObject
                                {
                                    { "ContactID", crcontactID },
                                    { "FileAs", crfileAs },
                                    { "FirstName", crfirstName },
                                    { "MiddleName", crmiddleName },
                                    { "LastName", crlastName },
                                    { "Position", crposition },
                                    { "Company", crcompany },
                                    { "CompanyID", crcompanyID },
                                    { "RetailerType", crretailerType },
                                    { "PresStreet", crpresStreet },
                                    { "PresBarangay", crpresBarangay },
                                    { "PresDistrict", crpresDistrict },
                                    { "PresTown", crpresTown },
                                    { "PresProvince", crpresProvince },
                                    { "PresCountry", crpresCountry },
                                    { "Landmark", crlandmark },
                                    { "Remarks", crremarks },
                                    { "RecordDate", crrecordDate },
                                    { "StartTime", crstartTime },
                                    { "EndTime", crendTime },
                                    { "Telephone1", crtelephone1 },
                                    { "Telephone2", crtelephone2 },
                                    { "Mobile", crmobile },
                                    { "Email", cremail },
                                    { "MobilePhoto1", crmobilePhoto1 },
                                    { "MobilePhoto2", crmobilePhoto2 },
                                    { "MobilePhoto3", crmobilePhoto3 },
                                    { "MobileVideo", crmobileVideo },
                                    { "Employee", cremployee },
                                    { "Customer", crcustomer },
                                    { "Supervisor", crsupervisor },
                                    { "RecordLog", crrecordlog },
                                    { "Deleted", crdeleted },
                                    { "LastUpdated", crlastUpdated }
                                };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.IsSuccessStatusCode)
                                {
                                    var crcontent = await crresponse.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(crcontent))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(crcontent, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            var ph1link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=tWyd43";
                                            string ph1contentType = "application/json";

                                            JObject ph1json;
                                            bool ph1doesExist = File.Exists(crmobilePhoto1);

                                            if (!ph1doesExist || string.IsNullOrEmpty(crmobilePhoto1))
                                            {
                                                ph1json = new JObject
                                                {
                                                    {"ContactID",crcontactID},
                                                    { "Photo1",""}
                                                };
                                            }
                                            else
                                            {
                                                ph1json = new JObject
                                                {
                                                    {"ContactID",crcontactID},
                                                    {"Photo1",File.ReadAllBytes(crmobilePhoto1)}
                                                };
                                            }

                                            HttpClient ph1client = new HttpClient();
                                            var ph1response = await ph1client.PostAsync(ph1link, new StringContent(ph1json.ToString(), Encoding.UTF8, ph1contentType));

                                            if (ph1response.IsSuccessStatusCode)
                                            {
                                                var ph1content = await ph1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(ph1content))
                                                {
                                                    var ph1result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph1content, settings);

                                                    var ph1item = ph1result[0];
                                                    var ph1message = ph1item.Message;

                                                    if (ph1message.Equals("Inserted"))
                                                    {
                                                        var ph2link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=qAWS26";
                                                        string ph2contentType = "application/json";

                                                        JObject ph2json;
                                                        bool ph2doesExist = File.Exists(crmobilePhoto2);

                                                        if (!ph2doesExist || string.IsNullOrEmpty(crmobilePhoto2))
                                                        {
                                                            ph2json = new JObject
                                                            {
                                                                {"ContactID",crcontactID},
                                                                { "Photo2",""}
                                                            };
                                                        }
                                                        else
                                                        {
                                                            ph2json = new JObject
                                                            {
                                                                {"ContactID",crcontactID},
                                                                {"Photo2",File.ReadAllBytes(crmobilePhoto2)}
                                                            };
                                                        }

                                                        HttpClient ph2client = new HttpClient();
                                                        var ph2response = await ph2client.PostAsync(ph2link, new StringContent(ph2json.ToString(), Encoding.UTF8, ph2contentType));

                                                        if (ph2response.IsSuccessStatusCode)
                                                        {
                                                            var ph2content = await ph2response.Content.ReadAsStringAsync();
                                                            if (!string.IsNullOrEmpty(ph2content))
                                                            {
                                                                var ph2result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph2content, settings);

                                                                var ph2item = ph2result[0];
                                                                var ph2message = ph2item.Message;

                                                                if (ph2message.Equals("Inserted"))
                                                                {
                                                                    var ph3link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=XuY4RN";
                                                                    string ph3contentType = "application/json";

                                                                    JObject ph3json;
                                                                    bool ph3doesExist = File.Exists(crmobilePhoto3);

                                                                    if (!ph3doesExist || string.IsNullOrEmpty(crmobilePhoto3))
                                                                    {
                                                                        ph3json = new JObject
                                                                        {
                                                                            {"ContactID",crcontactID},
                                                                            { "Photo3",""}
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        ph3json = new JObject
                                                                        {
                                                                            {"ContactID",crcontactID},
                                                                            {"Photo3",File.ReadAllBytes(crmobilePhoto3)}
                                                                        };
                                                                    }

                                                                    HttpClient ph3client = new HttpClient();
                                                                    var ph3response = await ph3client.PostAsync(ph3link, new StringContent(ph3json.ToString(), Encoding.UTF8, ph3contentType));

                                                                    if (ph3response.IsSuccessStatusCode)
                                                                    {
                                                                        var ph3content = await ph3response.Content.ReadAsStringAsync();
                                                                        if (!string.IsNullOrEmpty(ph3content))
                                                                        {
                                                                            var ph3result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph3content, settings);

                                                                            var ph3item = ph3result[0];
                                                                            var ph3message = ph3item.Message;

                                                                            if (ph3message.Equals("Inserted"))
                                                                            {
                                                                                if (!string.IsNullOrEmpty(crvideo))
                                                                                {
                                                                                    var vidlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=PsxQ7v";
                                                                                    string vidcontentType = "application/json";

                                                                                    JObject vidjson;
                                                                                    bool viddoesExist = File.Exists(crmobileVideo);

                                                                                    if (viddoesExist)
                                                                                    {
                                                                                        vidjson = new JObject
                                                                                        {
                                                                                           { "ContactID", crcontactID },
                                                                                           { "Video", File.ReadAllBytes(crmobileVideo) }
                                                                                        };
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        vidjson = new JObject
                                                                                        {
                                                                                           { "ContactID", crcontactID },
                                                                                           { "Video", "" }
                                                                                        };
                                                                                    }

                                                                                    HttpClient vidclient = new HttpClient();
                                                                                    var vidresponse = await vidclient.PostAsync(vidlink, new StringContent(vidjson.ToString(), Encoding.UTF8, vidcontentType));

                                                                                    if (vidresponse.IsSuccessStatusCode)
                                                                                    {
                                                                                        var vidcontent = await vidresponse.Content.ReadAsStringAsync();
                                                                                        if (!string.IsNullOrEmpty(vidcontent))
                                                                                        {
                                                                                            var vidresult = JsonConvert.DeserializeObject<List<ServerMessage>>(vidcontent, settings);

                                                                                            var viditem = vidresult[0];
                                                                                            var vidmessage = viditem.Message;

                                                                                            if (vidmessage.Equals("Inserted"))
                                                                                            {
                                                                                                await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), crcontactID);
                                                                                                count++;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                                                        OnSyncFailed();
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), crcontactID);
                                                                                    count++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                                        OnSyncFailed();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                            OnSyncFailed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                OnSyncFailed();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced retailer: " + (count - 1) + " out of " + changesresultCount + "\n";

                        SyncRetailerOutlet(host, database, contact, ipaddress);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Retailer data failed", "Data syncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    SyncRetailer(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Retailer data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Retailer data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void SyncRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing retailer sync";

                    try
                    {
                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        int count = 1;

                        var getOutletChanges = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getOutletChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending retailer outlet changes to server " + count + " out of " + changesresultCount;

                                var crresult = getOutletChanges.Result[i];
                                var crretailerCode = crresult.RetailerCode;
                                var crcontactID = crresult.ContactID;
                                var crpresStreet = crresult.PresStreet;
                                var crpresBarangay = crresult.PresBarangay;
                                var crpresDistrict = crresult.PresDistrict;
                                var crpresTown = crresult.PresTown;
                                var crpresProvince = crresult.PresProvince;
                                var crpresCountry = crresult.PresCountry;
                                var crtelephone1 = crresult.Telephone1;
                                var crtelephone2 = crresult.Telephone2;
                                var crmobile = crresult.Mobile;
                                var cremail = crresult.Email;
                                var crlandmark = crresult.Landmark;
                                var crgpsCoordinates = crresult.GPSCoordinates;
                                var crsupervisor = crresult.Supervisor;
                                var crrecordlog = crresult.RecordLog;
                                var crdeleted = crresult.Deleted;
                                var crlastUpdated = crresult.LastUpdated;

                                var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Pb3c6A";
                                string crcontentType = "application/json";
                                JObject crjson = new JObject
                                {
                                    { "RetailerCode", crretailerCode },
                                    { "ContactID", crcontactID },
                                    { "PresStreet", crpresStreet },
                                    { "PresBarangay", crpresBarangay },
                                    { "PresDistrict", crpresDistrict },
                                    { "PresTown", crpresTown },
                                    { "PresProvince", crpresProvince },
                                    { "PresCountry", crpresCountry },
                                    { "Telephone1", crtelephone1 },
                                    { "Telephone2", crtelephone2 },
                                    { "Mobile", crmobile },
                                    { "Email", cremail },
                                    { "Landmark", crlandmark },
                                    { "GPSCoordinates", crgpsCoordinates },
                                    { "Supervisor", crsupervisor },
                                    { "RecordLog", crsupervisor },
                                    { "Deleted", crdeleted },
                                    { "LastUpdated", crlastUpdated }
                                };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.IsSuccessStatusCode)
                                {
                                    var crcontent = await crresponse.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(crcontent))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(crcontent, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE RetailerCode = ?", DateTime.Parse(current_datetime), crretailerCode);
                                            count++;
                                        }
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Retailer outlet data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced retailer outlet changes: " + (count - 1) + " out of " + changesresultCount + "\n";

                        SyncCaf(host, database, contact, ipaddress);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Retailer outlet data failed", "Data syncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    SyncRetailerOutlet(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Retailer outlet data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Retailer outlet data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Retailer outlet data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Retailer outlet data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void SyncCaf(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing field activity sync";

                    try
                    {
                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var getCAFChanges = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getCAFChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            int count = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {

                                lblStatus.Text = "Sending Supervisor activity changes to server " + count + " out of " + changesresultCount;

                                var crresult = getCAFChanges.Result[i];
                                var crcafNo = crresult.CAFNo;
                                var cremployeeID = crresult.EmployeeID;
                                var crcafDate = crresult.CAFDate;
                                var crcustomerID = crresult.CustomerID;
                                var crstartTime = crresult.StartTime;
                                var crendTime = crresult.EndTime;
                                var crphoto1 = crresult.Photo1;
                                var crphoto2 = crresult.Photo2;
                                var crphoto3 = crresult.Photo3;
                                var crvideo = crresult.Video;
                                var crmobilePhoto1 = crresult.MobilePhoto1;
                                var crmobilePhoto2 = crresult.MobilePhoto2;
                                var crmobilePhoto3 = crresult.MobilePhoto3;
                                var crmobileVideo = crresult.MobileVideo;
                                var crgpsCoordinates = crresult.GPSCoordinates;
                                var crremarks = crresult.Remarks;
                                var crotherConcern = crresult.OtherConcern;
                                var crrecordlog = crresult.RecordLog;
                                var crdeleted = crresult.Deleted;
                                var crlastUpdated = crresult.LastUpdated;

                                var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=k5N7PE";
                                string crcontentType = "application/json";
                                JObject crjson = new JObject
                                {
                                    { "CAFNo", crcafNo },
                                    { "EmployeeID", cremployeeID },
                                    { "CAFDate", crcafDate },
                                    { "CustomerID", crcustomerID },
                                    { "StartTime", crstartTime },
                                    { "EndTime", crendTime },
                                    { "MobilePhoto1", crmobilePhoto1 },
                                    { "MobilePhoto2", crmobilePhoto2 },
                                    { "MobilePhoto3", crmobilePhoto3 },
                                    { "MobileVideo", crmobileVideo },
                                    { "GPSCoordinates", crgpsCoordinates },
                                    { "Remarks", crremarks },
                                    { "OtherConcern", crotherConcern },
                                    { "RecordLog", crrecordlog },
                                    { "Deleted", crdeleted },
                                    { "LastUpdated", crlastUpdated }
                                };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.IsSuccessStatusCode)
                                {
                                    var crcontent = await crresponse.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(crcontent))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(crcontent, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            var ph1link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=N4f5GL";
                                            string ph1contentType = "application/json";

                                            JObject ph1json;
                                            bool ph1doesExist = File.Exists(crmobilePhoto1);

                                            if (!ph1doesExist || string.IsNullOrEmpty(crmobilePhoto1))
                                            {
                                                ph1json = new JObject
                                                {
                                                    { "CAFNo", crcafNo },
                                                    { "CAFDate", crcafDate },
                                                    { "Photo1", ""}
                                                };
                                            }
                                            else
                                            {
                                                ph1json = new JObject
                                                {
                                                    { "CAFNo", crcafNo },
                                                    { "CAFDate", crcafDate },
                                                    { "Photo1", File.ReadAllBytes(crmobilePhoto1)}
                                                };
                                            }

                                            HttpClient ph1client = new HttpClient();
                                            var ph1response = await ph1client.PostAsync(ph1link, new StringContent(ph1json.ToString(), Encoding.UTF8, ph1contentType));

                                            if (ph1response.IsSuccessStatusCode)
                                            {
                                                var ph1content = await ph1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(ph1content))
                                                {
                                                    var ph1result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph1content, settings);

                                                    var ph1item = ph1result[0];
                                                    var ph1message = ph1item.Message;

                                                    if (ph1message.Equals("Inserted"))
                                                    {
                                                        var ph2link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=6LqMxW";
                                                        string ph2contentType = "application/json";

                                                        JObject ph2json;
                                                        bool ph2doesExist = File.Exists(crmobilePhoto2);

                                                        if (!ph2doesExist || string.IsNullOrEmpty(crmobilePhoto2))
                                                        {
                                                            ph2json = new JObject
                                                            {
                                                                { "CAFNo", crcafNo },
                                                                { "CAFDate", crcafDate },
                                                                { "Photo2", ""}
                                                            };
                                                        }
                                                        else
                                                        {
                                                            ph2json = new JObject
                                                            {
                                                                { "CAFNo", crcafNo },
                                                                { "CAFDate", crcafDate },
                                                                { "Photo2", File.ReadAllBytes(crmobilePhoto2)}
                                                            };
                                                        }

                                                        HttpClient ph2client = new HttpClient();
                                                        var ph2response = await ph2client.PostAsync(ph2link, new StringContent(ph2json.ToString(), Encoding.UTF8, ph2contentType));

                                                        if (ph2response.IsSuccessStatusCode)
                                                        {
                                                            var ph2content = await ph2response.Content.ReadAsStringAsync();
                                                            if (!string.IsNullOrEmpty(ph2content))
                                                            {
                                                                var ph2result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph2content, settings);

                                                                var ph2item = ph2result[0];
                                                                var ph2message = ph2item.Message;

                                                                if (ph2message.Equals("Inserted"))
                                                                {
                                                                    var ph3link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Mpt2Y9";
                                                                    string ph3contentType = "application/json";

                                                                    JObject ph3json;
                                                                    bool ph3doesExist = File.Exists(crmobilePhoto3);

                                                                    if (!ph3doesExist || string.IsNullOrEmpty(crmobilePhoto3))
                                                                    {
                                                                        ph3json = new JObject
                                                                        {
                                                                            { "CAFNo", crcafNo },
                                                                            { "CAFDate", crcafDate },
                                                                            { "Photo3", ""}
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        ph3json = new JObject
                                                                        {
                                                                            { "CAFNo", crcafNo },
                                                                            { "CAFDate", crcafDate },
                                                                            { "Photo3", File.ReadAllBytes(crmobilePhoto3)}
                                                                        };
                                                                    }

                                                                    HttpClient ph3client = new HttpClient();
                                                                    var ph3response = await ph3client.PostAsync(ph3link, new StringContent(ph3json.ToString(), Encoding.UTF8, ph3contentType));

                                                                    if (ph3response.IsSuccessStatusCode)
                                                                    {
                                                                        var ph3content = await ph3response.Content.ReadAsStringAsync();
                                                                        if (!string.IsNullOrEmpty(ph3content))
                                                                        {
                                                                            var ph3result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph3content, settings);

                                                                            var ph3item = ph3result[0];
                                                                            var ph3message = ph3item.Message;

                                                                            if (ph3message.Equals("Inserted"))
                                                                            {
                                                                                if (!string.IsNullOrEmpty(crvideo))
                                                                                {
                                                                                    var vidlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Lqr9fy";
                                                                                    string vidcontentType = "application/json";

                                                                                    JObject vidjson;
                                                                                    bool viddoesExist = File.Exists(crmobileVideo);

                                                                                    if (viddoesExist)
                                                                                    {
                                                                                        vidjson = new JObject
                                                                                        {
                                                                                           { "CAFNo", crcafNo },
                                                                                           { "CAFDate", crcafDate },
                                                                                           { "Video", File.ReadAllBytes(crmobileVideo) }
                                                                                        };
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        vidjson = new JObject
                                                                                        {
                                                                                           { "CAFNo", crcafNo },
                                                                                           { "CAFDate", crcafDate },
                                                                                           { "Video", "" }
                                                                                        };
                                                                                    }

                                                                                    HttpClient vidclient = new HttpClient();
                                                                                    var vidresponse = await vidclient.PostAsync(vidlink, new StringContent(vidjson.ToString(), Encoding.UTF8, vidcontentType));

                                                                                    if (vidresponse.IsSuccessStatusCode)
                                                                                    {
                                                                                        var vidcontent = await vidresponse.Content.ReadAsStringAsync();
                                                                                        if (!string.IsNullOrEmpty(vidcontent))
                                                                                        {
                                                                                            var vidresult = JsonConvert.DeserializeObject<List<ServerMessage>>(vidcontent, settings);

                                                                                            var viditem = vidresult[0];
                                                                                            var vidmessage = viditem.Message;

                                                                                            if (vidmessage.Equals("Inserted"))
                                                                                            {
                                                                                                await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), crcafNo);
                                                                                                count++;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        lblStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                                                                        btnFAF.IsEnabled = true;
                                                                                        btnAH.IsEnabled = true;
                                                                                        btnLogout.IsEnabled = true;
                                                                                        btnUI.IsEnabled = true;
                                                                                        btnPR.IsEnabled = true;
                                                                                        btnRetailer.IsEnabled = true;
                                                                                        btnResend.IsEnabled = true;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), crcafNo);
                                                                                    count++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                                        OnSyncFailed();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                            OnSyncFailed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                                OnSyncFailed();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced field activity: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }

                        SyncActivities(host, database, contact, ipaddress);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);

                        var seedata = await DisplayAlert("Field activity data failed", "Data syncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    SyncCaf(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Field activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Field activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void SyncActivities(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing activity sync";

                    try
                    {
                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var getActivityChanges = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getActivityChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            int count = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending activity changes to server " + count + " out of " + changesresultCount;

                                var crresult = getActivityChanges.Result[i];
                                var crcafNo = crresult.CAFNo;
                                var crcontactId = crresult.ContactID;
                                var cractivityID = crresult.ActivityID;
                                var crrecordlog = crresult.RecordLog;
                                var crdeleted = crresult.Deleted;
                                var crlastUpdated = crresult.LastUpdated;

                                var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=b7Q9XU";
                                string crcontentType = "application/json";
                                JObject crjson = new JObject
                                {
                                    { "CAFNo", crcafNo },
                                    { "ContactID", crcontactId },
                                    { "ActivityID", cractivityID },
                                    { "RecordLog", crrecordlog },
                                    { "Deleted", crdeleted },
                                    { "LastUpdated", crlastUpdated }
                                };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.IsSuccessStatusCode)
                                {
                                    var crcontent = await crresponse.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(crcontent))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(crcontent, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), crcafNo);
                                            count++;
                                        }
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced activity: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }

                        SyncEmail(host, database, contact, ipaddress);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);

                        var seedata = await DisplayAlert("Activity data failed", "Data syncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    SyncActivities(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void SyncEmail(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing email recipient sync";

                    try
                    {
                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var getEmailChanges = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getEmailChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            int count = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending email changes to server " + count + " out of " + changesresultCount;

                                var crresult = getEmailChanges.Result[i];
                                var crcontactID = crresult.ContactID;
                                var cremail = crresult.Email;
                                var crrecordlog = crresult.RecordLog;
                                var crdeleted = crresult.Deleted;
                                var crlastUpdated = crresult.LastUpdated;

                                var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=kcZw9g";
                                string crcontentType = "application/json";
                                JObject crjson = new JObject
                                {
                                    { "ContactID", crcontactID },
                                    { "Email", cremail },
                                    { "RecordLog", crrecordlog },
                                    { "Deleted", crdeleted },
                                    { "LastUpdated", crlastUpdated }
                                };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.IsSuccessStatusCode)
                                {
                                    await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);
                                    count++;
                                }
                                else
                                {
                                    await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced email recipient changes: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }

                        SyncLogs(host, database, contact, ipaddress);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);

                        var seedata = await DisplayAlert("Email recipient data failed", "Data syncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    SyncEmail(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Email recipient data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Email recipient data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void SyncLogs(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    try
                    {
                        lblStatus.Text = "Initializing user logs sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getUserLogsChanges = conn.QueryAsync<UserLogsTable>("SELECT * FROM tblUserLogs WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getUserLogsChanges.Result.Count;
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending user logs to server " + clientupdate + " out of " + changesresultCount;

                                var result = getUserLogsChanges.Result[i];
                                var crcontactID = result.ContactID;
                                var crlogType = result.LogType;
                                var crlog = result.Log;
                                var crlogDate = result.LogDate;
                                var crdatabaseName = result.DatabaseName;
                                var crdeleted = result.Deleted;
                                var crlastUpdated = result.LastUpdated;

                                var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=pQ412v";
                                string crcontentType = "application/json";
                                JObject crjson = new JObject
                                    {
                                        { "ContactID", contact },
                                        { "LogType", crlogType },
                                        { "Log", crlog },
                                        { "LogDate", crlogDate },
                                        { "DatabaseName", crdatabaseName },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.IsSuccessStatusCode)
                                {
                                    var crcontent = await crresponse.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(crcontent))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(crcontent, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET LastSync = ? WHERE ContactID = ? AND LogType = ? AND Log = ?", DateTime.Parse(current_datetime), contact, crlogType, crlog);

                                            clientupdate++;
                                        }
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("User logs recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced user logs: " + (clientupdate - 1) + " out of " + changesresultCount + "\n";
                        }

                        OnSyncComplete();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);

                        var seedata = await DisplayAlert("User logs data failed", "Data syncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    SyncLogs(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Email recipient data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("User logs data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("User logs data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void ReSyncRetailer(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing retailer re-sync";

                    try
                    {
                        lblStatus.Text = "Getting retailer data from server";

                        var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=9DpndD";
                        string contentType = "application/json";
                        JObject json = new JObject
                        {
                            { "ContactID", contact }
                        };

                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            int count = 1;

                            if (!string.IsNullOrEmpty(content))
                            {
                                var contactsresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);

                                for (int i = 0; i < contactsresult.Count; i++)
                                {
                                    var item = contactsresult[i];
                                    var contactID = item.ContactID;

                                    lblStatus.Text = "Checking retailer " + count + " out of " + contactsresult.Count;

                                    var getContacts = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID = ?", contactID);
                                    var resultCount = getContacts.Result.Count;

                                    if (resultCount == 1)
                                    {
                                        await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Checked = ? WHERE ContactID = ?", 1, contactID);
                                    }

                                    count++;
                                }
                            }
                            else
                            {
                                await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE Supervisor = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE Supervisor = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncRetailerOutlet(host, database, contact, ipaddress);
                        }
                        else
                        {
                            var seedata = await DisplayAlert("Retailer data failed", "Data resyncing failed", "Retry", "Cancel");
                            if (seedata == true)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Ping ping_retry = new Ping();
                                    PingReply pingretry_result = ping_retry.Send(ipaddress);

                                    if (pingretry_result.Status.ToString() == "Success")
                                    {
                                        ReSyncRetailer(host, database, contact, ipaddress);
                                    }
                                    else
                                    {
                                        await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                        OnSyncFailed();
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Retailer data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                OnSyncFailed();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Retailer data failed", "Data resyncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    ReSyncRetailerOutlet(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Retailer data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Retailer data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Retailer data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void ReSyncRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing retailer outlet re-sync";

                    try
                    {
                        lblStatus.Text = "Getting retailer outlet data from server";

                        var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Vf6HfC";
                        string contentType = "application/json";
                        JObject json = new JObject
                        {
                            { "ContactID", contact }
                        };

                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            if (!string.IsNullOrEmpty(content))
                            {
                                int count = 1;

                                var contactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                                for (int i = 0; i < contactsresult.Count; i++)
                                {
                                    var item = contactsresult[i];
                                    var retailerCode = item.RetailerCode;

                                    lblStatus.Text = "Checking retailer outlet " + count + " out of " + contactsresult.Count;

                                    var getContacts = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode = ?", retailerCode);
                                    var resultCount = getContacts.Result.Count;

                                    if (resultCount == 1)
                                    {
                                        await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET Checked = ? WHERE RetailerCode = ?", 1, retailerCode);
                                    }

                                    count++;
                                }
                            }
                            else
                            {
                                await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Supervisor = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Supervisor = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncCaf(host, database, contact, ipaddress);
                        }
                        else
                        {
                            var seedata = await DisplayAlert("Retailer outlet data failed", "Data resyncing failed", "Retry", "Cancel");
                            if (seedata == true)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Ping ping_retry = new Ping();
                                    PingReply pingretry_result = ping_retry.Send(ipaddress);

                                    if (pingretry_result.Status.ToString() == "Success")
                                    {
                                        ReSyncRetailerOutlet(host, database, contact, ipaddress);
                                    }
                                    else
                                    {
                                        await DisplayAlert("Retailer outlet data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                        OnSyncFailed();
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Retailer outlet data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                OnSyncFailed();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Retailer outlet data failed", "Data resyncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    ReSyncRetailerOutlet(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Retailer outlet data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Retailer outlet data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Retailer outlet data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Retailer outlet data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void ReSyncCaf(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing field activity re-sync";

                    try
                    {
                        lblStatus.Text = "Getting field activity data from server";

                        var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=fqV2Vb";
                        string contentType = "application/json";
                        JObject json = new JObject
                        {
                            { "ContactID", contact }
                        };

                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            if (!string.IsNullOrEmpty(content))
                            {
                                int count = 1;

                                var cafresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                                for (int i = 0; i < cafresult.Count; i++)
                                {
                                    var item = cafresult[i];
                                    var cafNo = item.CAFNo;

                                    lblStatus.Text = "Checking field activity " + count + " out of " + cafresult.Count;

                                    var getContacts = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE CAFNo = ?", cafNo);
                                    var resultCount = getContacts.Result.Count;

                                    if (resultCount == 1)
                                    {
                                        await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET Checked = ? WHERE CAFNo = ?", 1, cafNo);
                                    }

                                    count++;
                                }
                            }
                            else
                            {
                                await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE EmployeeID = ?", DateTime.Parse(default_datetime), contact, 1);
                            }

                            await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE EmployeeID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncActivities(host, database, contact, ipaddress);
                        }
                        else
                        {
                            var seedata = await DisplayAlert("Field activity data failed", "Data resyncing failed", "Retry", "Cancel");
                            if (seedata == true)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Ping ping_retry = new Ping();
                                    PingReply pingretry_result = ping_retry.Send(ipaddress);

                                    if (pingretry_result.Status.ToString() == "Success")
                                    {
                                        ReSyncCaf(host, database, contact, ipaddress);
                                    }
                                    else
                                    {
                                        await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                        OnSyncFailed();
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Field activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                OnSyncFailed();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Field activity data failed", "Data resyncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    ReSyncCaf(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Field activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Field activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Field activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void ReSyncActivities(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing activity re-sync";

                    try
                    {
                        lblStatus.Text = "Getting activity data from server";

                        var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=M2T3w2";
                        string contentType = "application/json";
                        JObject json = new JObject
                        {
                            { "ContactID", contact }
                        };

                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            if (!string.IsNullOrEmpty(content))
                            {
                                int count = 1;

                                var actresult = JsonConvert.DeserializeObject<List<ActivityData>>(content, settings);
                                for (int i = 0; i < actresult.Count; i++)
                                {
                                    lblStatus.Text = "Checking activity " + count + " out of " + actresult.Count;

                                    var item = actresult[i];
                                    var cafNo = item.CAFNo;
                                    var contactId = item.ContactID;
                                    var activityid = item.ActivityID;

                                    var getContacts = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID = ?", cafNo, contactId, activityid);
                                    var resultCount = getContacts.Result.Count;

                                    if (resultCount == 1)
                                    {
                                        await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET Checked = ? WHERE CAFNo = ? AND ContactID = ? AND ActivityID = ?", 1, cafNo, contactId, activityid);
                                    }

                                    count++;
                                }
                            }
                            else
                            {
                                await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE ContactID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncEmail(host, database, contact, ipaddress);
                        }
                        else
                        {
                            var seedata = await DisplayAlert("Activity data failed", "Data resyncing failed", "Retry", "Cancel");
                            if (seedata == true)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Ping ping_retry = new Ping();
                                    PingReply pingretry_result = ping_retry.Send(ipaddress);

                                    if (pingretry_result.Status.ToString() == "Success")
                                    {
                                        ReSyncActivities(host, database, contact, ipaddress);
                                    }
                                    else
                                    {
                                        await DisplayAlert("Activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                        OnSyncFailed();
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                OnSyncFailed();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Activity data failed", "Data resyncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    ReSyncActivities(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Activity data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Activity data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void ReSyncEmail(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing email recipient re-sync";

                    try
                    {
                        lblStatus.Text = "Getting email recipient data from server";

                        var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=3sEW7W";
                        string chcontentType = "application/json";
                        JObject chjson = new JObject
                        {
                            { "ContactID", contact }
                        };

                        HttpClient chclient = new HttpClient();
                        var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                        if (chresponse.IsSuccessStatusCode)
                        {
                            var chcontent = await chresponse.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            if (!string.IsNullOrEmpty(chcontent))
                            {
                                int count = 1;

                                var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent, settings);
                                for (int i = 0; i < chemailresult.Count; i++)
                                {
                                    lblStatus.Text = "Checking email recipient " + count + " out of " + chemailresult.Count;

                                    var chitem = chemailresult[i];
                                    var contactID = chitem.ContactID;

                                    var getContacts = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ?", contactID);
                                    var resultCount = getContacts.Result.Count;

                                    if (resultCount == 1)
                                    {
                                        await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET Checked = ? WHERE ContactID = ?", 1, contactID);
                                    }

                                    count++;
                                }
                            }
                            else
                            {
                                await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncLogs(host, database, contact, ipaddress);
                        }
                        else
                        {
                            var seedata = await DisplayAlert("Email recipient data failed", "Data resyncing failed", "Retry", "Cancel");
                            if (seedata == true)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Ping ping_retry = new Ping();
                                    PingReply pingretry_result = ping_retry.Send(ipaddress);

                                    if (pingretry_result.Status.ToString() == "Success")
                                    {
                                        ReSyncEmail(host, database, contact, ipaddress);
                                    }
                                    else
                                    {
                                        await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                        OnSyncFailed();
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Email recipient data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                OnSyncFailed();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("Email recipient data failed", "Data resyncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    ReSyncEmail(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Email recipient data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Email recipient data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Email recipient data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public async void ReSyncLogs(string host, string database, string contact, string ipaddress)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Initializing user logs re-sync";

                    try
                    {
                        lblStatus.Text = "Getting user logs data from server";

                        var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=F23lba";
                        string chcontentType = "application/json";
                        JObject chjson = new JObject
                        {
                            { "ContactID", contact }
                        };

                        HttpClient chclient = new HttpClient();
                        var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                        if (chresponse.IsSuccessStatusCode)
                        {
                            var chcontent = await chresponse.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            if (!string.IsNullOrEmpty(chcontent))
                            {
                                int count = 1;

                                var chlogsresult = JsonConvert.DeserializeObject<List<UserLogsData>>(chcontent, settings);
                                for (int i = 0; i < chlogsresult.Count; i++)
                                {
                                    lblStatus.Text = "Checking user logs " + count + " out of " + chlogsresult.Count;

                                    var chitem = chlogsresult[i];
                                    var contactID = chitem.ContactID;
                                    var logtype = chitem.LogType;
                                    var log = chitem.Log;
                                    var logdate = chitem.LogDate;
                                    var databasename = chitem.DatabaseName;

                                    var getContacts = conn.QueryAsync<UserLogsTable>("SELECT * FROM tblUserLogs WHERE ContactID = ? AND LogType = ? AND Log = ? AND LogDate = ? AND DatabaseName = ?", contactID, logtype, log, logdate, databasename);
                                    var resultCount = getContacts.Result.Count;

                                    if (resultCount == 1)
                                    {
                                        await conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET Checked = ? WHERE ContactID = ? AND LogType = ? AND Log = ? AND LogDate = ? AND DatabaseName = ?", 1, contactID, logtype, log, logdate, databasename);
                                    }

                                    count++;
                                }
                            }
                            else
                            {
                                await conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET LastSync = ? WHERE ContactID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            SyncRetailer(host, database, contact, ipaddress);
                        }
                        else
                        {
                            var seedata = await DisplayAlert("User logs data failed", "Data resyncing failed", "Retry", "Cancel");
                            if (seedata == true)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Ping ping_retry = new Ping();
                                    PingReply pingretry_result = ping_retry.Send(ipaddress);

                                    if (pingretry_result.Status.ToString() == "Success")
                                    {
                                        ReSyncLogs(host, database, contact, ipaddress);
                                    }
                                    else
                                    {
                                        await DisplayAlert("User logs data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                        OnSyncFailed();
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("User logs data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                OnSyncFailed();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        var seedata = await DisplayAlert("User logs data failed", "Data resyncing failed", "Retry", "Cancel");
                        if (seedata == true)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Ping ping_retry = new Ping();
                                PingReply pingretry_result = ping_retry.Send(ipaddress);

                                if (pingretry_result.Status.ToString() == "Success")
                                {
                                    ReSyncLogs(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    await DisplayAlert("User logs data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                                    OnSyncFailed();
                                }
                            }
                            else
                            {
                                await DisplayAlert("User logs data failed", "No connection detected, please connect to the internet to retry", "Got it");
                                OnSyncFailed();
                            }
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("User logs data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("User logs data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }

        public void OnSyncComplete()
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (CrossConnectivity.Current.IsConnected)
            {
                if (pingresult.Status.ToString() == "Success")
                {
                    DisplayAlert("Sync Completed", "Sync Summary: \n\n" + synccount , "Got it");
                    synccount = "";
                    lblStatus.Text = "Online - Connected to server";
                    lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
                }
                else
                {
                    lblStatus.Text = "Online - Server unreachable. Connect to VPN";
                    lblStatus.BackgroundColor = Color.FromHex("#e67e22");
                }
            }
            else
            {
                lblStatus.Text = "Offline - Connect to internet";
                lblStatus.BackgroundColor = Color.FromHex("#e74c3c");
            }

            btnFAF.IsEnabled = true;
            btnAH.IsEnabled = true;
            btnLogout.IsEnabled = true;
            btnUI.IsEnabled = true;
            btnPR.IsEnabled = true;
            btnRetailer.IsEnabled = true;
            btnResend.IsEnabled = true;
        }

        public void OnSyncFailed()
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (CrossConnectivity.Current.IsConnected)
            {
                if (pingresult.Status.ToString() == "Success")
                {
                    lblStatus.Text = "Online - Connected to server";
                    lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
                }
                else
                {
                    lblStatus.Text = "Online - Server unreachable. Connect to VPN";
                    lblStatus.BackgroundColor = Color.FromHex("#e67e22");
                }
            }
            else
            {
                lblStatus.Text = "Offline - Connect to internet";
                lblStatus.BackgroundColor = Color.FromHex("#e74c3c");
            }

            btnFAF.IsEnabled = true;
            btnAH.IsEnabled = true;
            btnLogout.IsEnabled = true;
            btnUI.IsEnabled = true;
            btnPR.IsEnabled = true;
            btnRetailer.IsEnabled = true;
            btnResend.IsEnabled = true;
        }

        private async void BtnResend_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Ping ping = new Ping();
                 PingReply pingresult = ping.Send(ipaddress, 200);

                if (pingresult.Status.ToString() == "Success")
                {
                    var optimalSpeed = 50;
                    var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
                    var speeds = CrossConnectivity.Current.Bandwidths;

                    if (connectionTypes.Any(speed => Convert.ToInt32(speed) < optimalSpeed))
                    {
                        lblStatus.Text = "Initializing data sync";
                        lblStatus.BackgroundColor = Color.FromHex("#27ae60");

                        var confirm = await DisplayAlert("Re-sync Connection Warning", "Slow connection detected. Do you want to re-sync the data? Please do not turn off/lock your device during the syncing process.", "Yes", "No");
                        if (confirm == true)
                        {
                            ReSyncRetailer(host, database, contact, ipaddress);

                            btnFAF.IsEnabled = false;
                            btnAH.IsEnabled = false;
                            btnLogout.IsEnabled = false;
                            btnUI.IsEnabled = false;
                            btnPR.IsEnabled = false;
                            btnRetailer.IsEnabled = false;
                            btnResend.IsEnabled = false;
                        }
                    }
                    else
                    {
                        var resync = await DisplayAlert("Re-sync Warning", "Do you want to re-sync the data? Please do not turn off/lock your device during the syncing process.", "Yes", "No");
                        if (resync == true)
                        {
                            ReSyncRetailer(host, database, contact, ipaddress);

                            btnFAF.IsEnabled = false;
                            btnAH.IsEnabled = false;
                            btnLogout.IsEnabled = false;
                            btnUI.IsEnabled = false;
                            btnPR.IsEnabled = false;
                            btnRetailer.IsEnabled = false;
                            btnResend.IsEnabled = false;
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Resync data failed", "Server unreachable, please connect to your VPN to retry", "Got it");
                    OnSyncFailed();
                }
            }
            else
            {
                await DisplayAlert("Resync data failed", "No connection detected, please connect to the internet to retry", "Got it");
                OnSyncFailed();
            }
        }
    }
}