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
                            PingReply pingresult = ping.Send(ipaddress, 800);

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
                                            SyncContactsClientUpdate(host, database, contact, ipaddress);
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
                                        SyncContactsClientUpdate(host, database, contact, ipaddress);
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
                             PingReply pingresult = ping.Send(ipaddress, 800);

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
                                            SyncContactsClientUpdate(host, database, contact, ipaddress);
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
                                        SyncContactsClientUpdate(host, database, contact, ipaddress);
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

        public async void SyncContactsClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-contacts-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing contacts client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = datachanges.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending contacts changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var contactID = result.ContactID;
                                var fileAs = result.FileAs;
                                var firstName = result.FirstName;
                                var middleName = result.MiddleName;
                                var lastName = result.LastName;
                                var position = result.Position;
                                var company = result.Company;
                                var companyID = result.CompanyID;
                                var retailerType = result.RetailerType;
                                var presStreet = result.PresStreet;
                                var presBarangay = result.PresBarangay;
                                var presDistrict = result.PresDistrict;
                                var presTown = result.PresTown;
                                var presProvince = result.PresProvince;
                                var presCountry = result.PresCountry;
                                var landmark = result.Landmark;
                                var remarks = result.CustomerRemarks;
                                var recordDate = result.RecordDate;
                                var startTime = result.StartTime;
                                var endTime = result.EndTime;
                                var telephone1 = result.Telephone1;
                                var telephone2 = result.Telephone2;
                                var mobile = result.Mobile;
                                var email = result.Email;
                                var photo1 = result.Photo1;
                                var photo2 = result.Photo2;
                                var photo3 = result.Photo3;
                                var video = result.Video;
                                var mobilePhoto1 = result.MobilePhoto1;
                                var mobilePhoto2 = result.MobilePhoto2;
                                var mobilePhoto3 = result.MobilePhoto3;
                                var mobileVideo = result.MobileVideo;
                                var employee = result.Employee;
                                var customer = result.Customer;
                                var recordLog = result.RecordLog;
                                var supervisor = result.Supervisor;
                                var deleted = result.Deleted;
                                var lastUpdated = result.LastUpdated;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "ContactID", contactID },
                                    { "FileAs", fileAs },
                                    { "FirstName", firstName },
                                    { "MiddleName", middleName },
                                    { "LastName", lastName },
                                    { "Position", position },
                                    { "Company", company },
                                    { "CompanyID", companyID },
                                    { "RetailerType", retailerType },
                                    { "PresStreet", presStreet },
                                    { "PresBarangay", presBarangay },
                                    { "PresDistrict", presDistrict },
                                    { "PresTown", presTown },
                                    { "PresProvince", presProvince },
                                    { "PresCountry", presCountry },
                                    { "Landmark", landmark },
                                    { "Remarks", remarks },
                                    { "RecordDate", recordDate },
                                    { "StartTime", startTime },
                                    { "EndTime", endTime },
                                    { "Telephone1", telephone1 },
                                    { "Telephone2", telephone2 },
                                    { "Mobile", mobile },
                                    { "Email", email },
                                    { "MobilePhoto1", mobilePhoto1 },
                                    { "MobilePhoto2", mobilePhoto2 },
                                    { "MobilePhoto3", mobilePhoto3 },
                                    { "MobileVideo", mobileVideo },
                                    { "Employee", employee },
                                    { "Customer", customer },
                                    { "RecordLog", recordLog },
                                    { "Supervisor", supervisor },
                                    { "Deleted", deleted },
                                    { "LastUpdated", lastUpdated }
                                };

                                HttpClient client = new HttpClient();
                                var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            string path1file = "sync-contact-media-path-1-client-update-api.php";

                                            var path1link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path1file;
                                            string path1contentType = "application/json";

                                            JObject path1json;
                                            bool path1doesExist = File.Exists(mobilePhoto1);

                                            if (!path1doesExist || string.IsNullOrEmpty(mobilePhoto1))
                                            {
                                                path1json = new JObject
                                                {
                                                   { "Host", host },
                                                   { "Database", database },
                                                   { "MediaID", contactID},
                                                   { "Path", ""}
                                                };
                                            }
                                            else
                                            {
                                                path1json = new JObject
                                                {
                                                    { "Host", host },
                                                    { "Database", database },
                                                    { "MediaID", contactID},
                                                    { "Path", File.ReadAllBytes(mobilePhoto1)}
                                                };
                                            }

                                            HttpClient path1client = new HttpClient();
                                            var path1response = await path1client.PostAsync(path1link, new StringContent(path1json.ToString(), Encoding.UTF8, path1contentType));

                                            if (path1response.IsSuccessStatusCode)
                                            {
                                                var path1content = await path1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(path1content))
                                                {
                                                    var path1result = JsonConvert.DeserializeObject<List<ServerMessage>>(path1content, settings);

                                                    var path1item = path1result[0];
                                                    var path1message = path1item.Message;

                                                    if (path1message.Equals("Inserted"))
                                                    {
                                                        string path2file = "sync-contact-media-path-2-client-update-api.php";

                                                        var path2link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path2file;
                                                        string path2contentType = "application/json";

                                                        JObject path2json;
                                                        bool path2doesExist = File.Exists(mobilePhoto2);

                                                        if (!path2doesExist || string.IsNullOrEmpty(mobilePhoto2))
                                                        {
                                                            path2json = new JObject
                                                            {
                                                               { "Host", host },
                                                               { "Database", database },
                                                               { "MediaID", contactID},
                                                               { "Path", ""}
                                                            };
                                                        }
                                                        else
                                                        {
                                                            path2json = new JObject
                                                            {
                                                                { "Host", host },
                                                                { "Database", database },
                                                                { "MediaID", contactID},
                                                                { "Path", File.ReadAllBytes(mobilePhoto2)}
                                                            };
                                                        }

                                                        HttpClient path2client = new HttpClient();
                                                        var path2response = await path2client.PostAsync(path2link, new StringContent(path2json.ToString(), Encoding.UTF8, path2contentType));

                                                        if (path2response.IsSuccessStatusCode)
                                                        {
                                                            var path2content = await path2response.Content.ReadAsStringAsync();
                                                            if (!string.IsNullOrEmpty(path2content))
                                                            {
                                                                var path2result = JsonConvert.DeserializeObject<List<ServerMessage>>(path2content, settings);

                                                                var path2item = path2result[0];
                                                                var path2message = path2item.Message;

                                                                if (path2message.Equals("Inserted"))
                                                                {
                                                                    string path3file = "sync-contact-media-path-3-client-update-api.php";

                                                                    var path3link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path3file;
                                                                    string path3contentType = "application/json";

                                                                    JObject path3json;
                                                                    bool path3doesExist = File.Exists(mobilePhoto3);

                                                                    if (!path3doesExist || string.IsNullOrEmpty(mobilePhoto3))
                                                                    {
                                                                        path3json = new JObject
                                                                        {
                                                                           { "Host", host },
                                                                           { "Database", database },
                                                                           { "MediaID", contactID},
                                                                           { "Path", ""}
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        path3json = new JObject
                                                                        {
                                                                            { "Host", host },
                                                                            { "Database", database },
                                                                            { "MediaID", contactID},
                                                                            { "Path", File.ReadAllBytes(mobilePhoto3)}
                                                                        };
                                                                    }

                                                                    HttpClient path3client = new HttpClient();
                                                                    var path3response = await path3client.PostAsync(path3link, new StringContent(path3json.ToString(), Encoding.UTF8, path3contentType));

                                                                    if (path3response.IsSuccessStatusCode)
                                                                    {
                                                                        var path3content = await path3response.Content.ReadAsStringAsync();
                                                                        if (!string.IsNullOrEmpty(path3content))
                                                                        {
                                                                            var path3result = JsonConvert.DeserializeObject<List<ServerMessage>>(path3content, settings);

                                                                            var path3item = path3result[0];
                                                                            var path3message = path3item.Message;

                                                                            if (path3message.Equals("Inserted"))
                                                                            {
                                                                                string path4file = "sync-contact-media-path-4-client-update-api.php";

                                                                                var path4link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path4file;
                                                                                string path4contentType = "application/json";

                                                                                JObject path4json;
                                                                                bool path4doesExist = File.Exists(mobileVideo);

                                                                                if (!path4doesExist || string.IsNullOrEmpty(mobileVideo))
                                                                                {
                                                                                    path4json = new JObject
                                                                                    {
                                                                                       { "Host", host },
                                                                                       { "Database", database },
                                                                                       { "MediaID", contactID},
                                                                                       { "Path", ""}
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    path4json = new JObject
                                                                                    {
                                                                                        { "Host", host },
                                                                                        { "Database", database },
                                                                                        { "MediaID", contactID},
                                                                                        { "Path", File.ReadAllBytes(mobileVideo)}
                                                                                    };
                                                                                }

                                                                                HttpClient path4client = new HttpClient();
                                                                                var path4response = await path4client.PostAsync(path4link, new StringContent(path4json.ToString(), Encoding.UTF8, path4contentType));

                                                                                if (path4response.IsSuccessStatusCode)
                                                                                {
                                                                                    var path4content = await path4response.Content.ReadAsStringAsync();
                                                                                    if (!string.IsNullOrEmpty(path4content))
                                                                                    {
                                                                                        var path4result = JsonConvert.DeserializeObject<List<ServerMessage>>(path4content, settings);

                                                                                        var path4item = path4result[0];
                                                                                        var path4message = path4item.Message;

                                                                                        if (path4message.Equals("Inserted"))
                                                                                        {
                                                                                            await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contactID);

                                                                                            clientupdate++;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                                                    OnSyncFailed();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                                        OnSyncFailed();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                            OnSyncFailed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                OnSyncFailed();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced client contacts update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>Contacts</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncRetailerOutletClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-retailer-outlet-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing retailer outlet client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = datachanges.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending retailer outlet changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var retailerCode = result.RetailerCode;
                                var contactID = result.ContactID;
                                var presStreet = result.PresStreet;
                                var presBarangay = result.PresBarangay;
                                var presDistrict = result.PresDistrict;
                                var presTown = result.PresTown;
                                var presProvince = result.PresProvince;
                                var presCountry = result.PresCountry;
                                var telephone1 = result.Telephone1;
                                var telephone2 = result.Telephone2;
                                var mobile = result.Mobile;
                                var email = result.Email;
                                var landmark = result.Landmark;
                                var gpsCoordinates = result.GPSCoordinates;
                                var supervisor = result.Supervisor;
                                var recordLog = result.RecordLog;
                                var deleted = result.Deleted;
                                var lastUpdated = result.LastUpdated;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "RetailerCode", retailerCode },
                                    { "ContactID", contactID },
                                    { "PresStreet", presStreet },
                                    { "PresBarangay", presBarangay },
                                    { "PresDistrict", presDistrict },
                                    { "PresTown", presTown },
                                    { "PresProvince", presProvince },
                                    { "PresCountry", presCountry },
                                    { "Telephone1", telephone1 },
                                    { "Telephone2", telephone2 },
                                    { "Mobile", mobile },
                                    { "Email", email },
                                    { "Landmark", landmark },
                                    { "GPSCoordinates", gpsCoordinates },
                                    { "Supervisor", supervisor },
                                    { "RecordLog", recordLog },
                                    { "Deleted", deleted },
                                    { "LastUpdated", lastUpdated }
                                };

                                HttpClient client = new HttpClient();
                                var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE RetailerCode = ?", DateTime.Parse(current_datetime), retailerCode);

                                            clientupdate++;
                                        }
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced client retailer outlet update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>Retailer Outlet</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncCAFClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncCAFClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-caf-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing caf client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE EmployeeID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = datachanges.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending caf changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var cafNo = result.CAFNo;
                                var employeeID = result.EmployeeID;
                                var cafDate = result.CAFDate;
                                var customerID = result.CustomerID;
                                var startTime = result.StartTime;
                                var endTime = result.EndTime;
                                var photo1 = result.Photo1;
                                var photo2 = result.Photo2;
                                var photo3 = result.Photo3;
                                var video = result.Video;
                                var mobilePhoto1 = result.MobilePhoto1;
                                var mobilePhoto2 = result.MobilePhoto2;
                                var mobilePhoto3 = result.MobilePhoto3;
                                var mobileVideo = result.MobileVideo;
                                var gpsLocation = result.GPSCoordinates;
                                var remarks = result.Remarks;
                                var otherConcern = result.OtherConcern;
                                var recordLog = result.RecordLog;
                                var deleted = result.Deleted;
                                var lastUpdated = result.LastUpdated;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "CAFNo", cafNo },
                                    { "EmployeeID", employeeID },
                                    { "CAFDate", cafDate },
                                    { "CustomerID", customerID },
                                    { "StartTime", startTime },
                                    { "EndTime", endTime },
                                    { "MobilePhoto1", mobilePhoto1 },
                                    { "MobilePhoto2", mobilePhoto2 },
                                    { "MobilePhoto3", mobilePhoto3 },
                                    { "MobileVideo", mobileVideo },
                                    { "GPSCoordinates", gpsLocation },
                                    { "Remarks", remarks },
                                    { "OtherConcern", otherConcern },
                                    { "RecordLog", recordLog },
                                    { "Deleted", deleted },
                                    { "LastUpdated", lastUpdated }
                                };

                                HttpClient client = new HttpClient();
                                var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            string path1file = "sync-caf-media-path-1-client-update-api.php";

                                            var path1link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path1file;
                                            string path1contentType = "application/json";

                                            JObject path1json;
                                            bool path1doesExist = File.Exists(mobilePhoto1);

                                            if (!path1doesExist || string.IsNullOrEmpty(mobilePhoto1))
                                            {
                                                path1json = new JObject
                                                {
                                                   { "Host", host },
                                                   { "Database", database },
                                                   { "MediaID", cafNo},
                                                   { "Path", ""}
                                                };
                                            }
                                            else
                                            {
                                                path1json = new JObject
                                                {
                                                    { "Host", host },
                                                    { "Database", database },
                                                    { "MediaID", cafNo},
                                                    { "Path", File.ReadAllBytes(mobilePhoto1)}
                                                };
                                            }

                                            HttpClient path1client = new HttpClient();
                                            var path1response = await path1client.PostAsync(path1link, new StringContent(path1json.ToString(), Encoding.UTF8, path1contentType));

                                            if (path1response.IsSuccessStatusCode)
                                            {
                                                var path1content = await path1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(path1content))
                                                {
                                                    var path1result = JsonConvert.DeserializeObject<List<ServerMessage>>(path1content, settings);

                                                    var path1item = path1result[0];
                                                    var path1message = path1item.Message;

                                                    if (path1message.Equals("Inserted"))
                                                    {
                                                        string path2file = "sync-caf-media-path-2-client-update-api.php";

                                                        var path2link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path2file;
                                                        string path2contentType = "application/json";

                                                        JObject path2json;
                                                        bool path2doesExist = File.Exists(mobilePhoto2);

                                                        if (!path2doesExist || string.IsNullOrEmpty(mobilePhoto2))
                                                        {
                                                            path2json = new JObject
                                                            {
                                                               { "Host", host },
                                                               { "Database", database },
                                                               { "MediaID", cafNo},
                                                               { "Path", ""}
                                                            };
                                                        }
                                                        else
                                                        {
                                                            path2json = new JObject
                                                            {
                                                                { "Host", host },
                                                                { "Database", database },
                                                                { "MediaID", cafNo},
                                                                { "Path", File.ReadAllBytes(mobilePhoto2)}
                                                            };
                                                        }

                                                        HttpClient path2client = new HttpClient();
                                                        var path2response = await path2client.PostAsync(path2link, new StringContent(path2json.ToString(), Encoding.UTF8, path2contentType));

                                                        if (path2response.IsSuccessStatusCode)
                                                        {
                                                            var path2content = await path2response.Content.ReadAsStringAsync();
                                                            if (!string.IsNullOrEmpty(path2content))
                                                            {
                                                                var path2result = JsonConvert.DeserializeObject<List<ServerMessage>>(path2content, settings);

                                                                var path2item = path2result[0];
                                                                var path2message = path2item.Message;

                                                                if (path2message.Equals("Inserted"))
                                                                {
                                                                    string path3file = "sync-caf-media-path-3-client-update-api.php";

                                                                    var path3link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path3file;
                                                                    string path3contentType = "application/json";

                                                                    JObject path3json;
                                                                    bool path3doesExist = File.Exists(mobilePhoto3);

                                                                    if (!path3doesExist || string.IsNullOrEmpty(mobilePhoto3))
                                                                    {
                                                                        path3json = new JObject
                                                                        {
                                                                           { "Host", host },
                                                                           { "Database", database },
                                                                           { "MediaID", cafNo},
                                                                           { "Path", ""}
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        path3json = new JObject
                                                                        {
                                                                            { "Host", host },
                                                                            { "Database", database },
                                                                            { "MediaID", cafNo},
                                                                            { "Path", File.ReadAllBytes(mobilePhoto3)}
                                                                        };
                                                                    }

                                                                    HttpClient path3client = new HttpClient();
                                                                    var path3response = await path3client.PostAsync(path3link, new StringContent(path3json.ToString(), Encoding.UTF8, path3contentType));

                                                                    if (path3response.IsSuccessStatusCode)
                                                                    {
                                                                        var path3content = await path3response.Content.ReadAsStringAsync();
                                                                        if (!string.IsNullOrEmpty(path3content))
                                                                        {
                                                                            var path3result = JsonConvert.DeserializeObject<List<ServerMessage>>(path3content, settings);

                                                                            var path3item = path3result[0];
                                                                            var path3message = path3item.Message;

                                                                            if (path3message.Equals("Inserted"))
                                                                            {
                                                                                string path4file = "sync-caf-media-path-4-client-update-api.php";

                                                                                var path4link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path4file;
                                                                                string path4contentType = "application/json";

                                                                                JObject path4json;
                                                                                bool path4doesExist = File.Exists(mobileVideo);

                                                                                if (!path4doesExist || string.IsNullOrEmpty(mobileVideo))
                                                                                {
                                                                                    path4json = new JObject
                                                                                    {
                                                                                       { "Host", host },
                                                                                       { "Database", database },
                                                                                       { "MediaID", cafNo},
                                                                                       { "Path", ""}
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    path4json = new JObject
                                                                                    {
                                                                                        { "Host", host },
                                                                                        { "Database", database },
                                                                                        { "MediaID", cafNo},
                                                                                        { "Path", File.ReadAllBytes(mobileVideo)}
                                                                                    };
                                                                                }

                                                                                HttpClient path4client = new HttpClient();
                                                                                var path4response = await path4client.PostAsync(path4link, new StringContent(path4json.ToString(), Encoding.UTF8, path4contentType));

                                                                                if (path4response.IsSuccessStatusCode)
                                                                                {
                                                                                    var path4content = await path4response.Content.ReadAsStringAsync();
                                                                                    if (!string.IsNullOrEmpty(path4content))
                                                                                    {
                                                                                        var path4result = JsonConvert.DeserializeObject<List<ServerMessage>>(path4content, settings);

                                                                                        var path4item = path4result[0];
                                                                                        var path4message = path4item.Message;

                                                                                        if (path4message.Equals("Inserted"))
                                                                                        {
                                                                                            await conn.QueryAsync<CAFTable>("UPDATE tblCAF SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), cafNo);

                                                                                            clientupdate++;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                                                    OnSyncFailed();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                                        OnSyncFailed();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                            OnSyncFailed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblStatus.Text = "Syncing failed. Server is unreachable.";
                                                OnSyncFailed();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced client caf update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>CAF</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncCAFActivityClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-caf-activity-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing caf activity client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE LastUpdated > LastSync AND Deleted != '1'");
                        var changesresultCount = datachanges.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending caf activity changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var cafNo = result.CAFNo;
                                var activityID = result.ActivityID;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = result.LastUpdated;
                                var deleted = result.Deleted;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "CAFNo", cafNo },
                                    { "ActivityID", activityID },
                                    { "LastUpdated", lastupdated },
                                    { "Deleted", deleted }
                                };

                                HttpClient client = new HttpClient();
                                var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), cafNo);

                                            clientupdate++;
                                        }
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced client caf activity update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>CAF Activity</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncEmailRecipientClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-email-recipient-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing email recipient client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = datachanges.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending email recipient changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var contactsID = result.ContactID;
                                var email = result.Email;
                                var recordLog = result.RecordLog;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = result.LastUpdated;
                                var deleted = result.Deleted;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "ContactID", contactsID },
                                    { "Email", email },
                                    { "RecordLog", recordLog },
                                    { "LastUpdated", lastupdated },
                                    { "Deleted", deleted }
                                };

                                HttpClient client = new HttpClient();
                                var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contactsID);

                                            clientupdate++;
                                        }
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced client email recipient update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>Email Recipient</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);


                        }

                        SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncUserLogsClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-user-logs-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing user logs client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<UserLogsTable>("SELECT * FROM tblUserLogs WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = datachanges.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                lblStatus.Text = "Sending user logs changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var contactsID = result.ContactID;
                                var logtype = result.LogType;
                                var logs = result.Log;
                                var logDate = result.LogDate;
                                var databasename = result.DatabaseName;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = result.LastUpdated;
                                var deleted = result.Deleted;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "ContactID", contactsID },
                                    { "LogType", logtype },
                                    { "Log", logs },
                                    { "LogDate", logDate },
                                    { "LastUpdated", lastupdated },
                                    { "Deleted", deleted }
                                };

                                HttpClient client = new HttpClient();
                                var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET LastSync = ? WHERE ContactID = ? AND LogType = ? AND Log = ? AND LogDate = ? AND DatabaseName = ?", DateTime.Parse(current_datetime), contactsID, logtype, logs, logDate, database);

                                            clientupdate++;
                                        }
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "Syncing failed. Server is unreachable.";
                                    OnSyncFailed();
                                }
                            }

                            synccount += "Total synced client user logs update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>User Logs</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        OnSyncComplete();
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void ReSyncContacts(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-contacts-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing first-time contacts sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        lblStatus.Text = "Getting data from the server";

                        var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                        string contentType = "application/json";
                        JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database },
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
                                var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                                var datacount = dataresult.Count;

                                for (int i = 0; i < datacount; i++)
                                {
                                    lblStatus.Text = "Checking contacts " + count + " out of " + datacount;

                                    var item = dataresult[i];
                                    var contactID = item.ContactID;

                                    var getContacts = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID = ?", contactID);
                                    var counts = getContacts.Result.Count;

                                    if (counts == 1)
                                    {
                                        await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Checked = ? WHERE ContactID = ?", 1, contactID);
                                    }

                                    count++;
                                }

                                synccount += "Total checked contacts: " + (count + 1) + " out of " + datacount + "\n";

                                var logType = "App Log";
                                var log = "Initialized re-sync (<b>Contacts</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
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
                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                            OnSyncFailed();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void ReSyncRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-retailer-outlet-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing first-time retailer outlet sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        lblStatus.Text = "Getting data from the server";

                        var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                        string contentType = "application/json";
                        JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database },
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
                                var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                                var datacount = dataresult.Count;

                                for (int i = 0; i < datacount; i++)
                                {
                                    lblStatus.Text = "Checking retailer outlet " + count + " out of " + datacount;

                                    var item = dataresult[i];
                                    var retailerCode = item.RetailerCode;

                                    var getContacts = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode = ?", retailerCode);
                                    var counts = getContacts.Result.Count;

                                    if (counts == 1)
                                    {
                                        await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET Checked = ? WHERE RetailerCode = ?", 1, retailerCode);
                                    }

                                    count++;
                                }

                                synccount += "Total checked retailer outlet: " + (count + 1) + " out of " + datacount + "\n";

                                var logType = "App Log";
                                var log = "Initialized re-sync (<b>Retailer Outlet</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }
                            else
                            {
                                await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Supervisor = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Supervisor = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncCAF(host, database, contact, ipaddress);
                        }
                        else
                        {
                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                            OnSyncFailed();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void ReSyncCAF(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-caf-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing first-time caf sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        lblStatus.Text = "Getting data from the server";

                        var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                        string contentType = "application/json";
                        JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database },
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
                                var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                                var datacount = dataresult.Count;

                                for (int i = 0; i < datacount; i++)
                                {
                                    lblStatus.Text = "Checking caf " + count + " out of " + datacount;

                                    var item = dataresult[i];
                                    var cafNo = item.CAFNo;

                                    var getContacts = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE CAFNo = ?", cafNo);
                                    var counts = getContacts.Result.Count;

                                    if (counts == 1)
                                    {
                                        await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET Checked = ? WHERE CAFNo = ?", 1, cafNo);
                                    }

                                    count++;
                                }

                                synccount += "Total checked caf: " + (count + 1) + " out of " + datacount + "\n";

                                var logType = "App Log";
                                var log = "Initialized re-sync (<b>CAF</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }
                            else
                            {
                                await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE EmployeeID = ?", DateTime.Parse(default_datetime), contact, 1);
                            }

                            await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE EmployeeID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncCAFActivity(host, database, contact, ipaddress);
                        }
                        else
                        {
                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                            OnSyncFailed();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void ReSyncCAFActivity(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-caf-activity-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing first-time caf activity sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        lblStatus.Text = "Getting data from the server";

                        var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                        string contentType = "application/json";
                        JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database }
                            };

                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var default_datetime = "0001-01-01 00:00:00";

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<ActivityTable>>(content, settings);
                                var datacount = dataresult.Count;

                                for (int i = 0; i < datacount; i++)
                                {
                                    lblStatus.Text = "Checking caf activity " + count + " out of " + datacount;

                                    var item = dataresult[i];
                                    var cafNo = item.CAFNo;
                                    var contactId = item.ContactID;
                                    var activityid = item.ActivityID;

                                    var getContacts = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID = ?", cafNo, contactId, activityid);
                                    var counts = getContacts.Result.Count;

                                    if (counts == 1)
                                    {
                                        await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET Checked = ? WHERE CAFNo = ? AND ContactID = ? AND ActivityID = ?", 1, cafNo, contactId, activityid);
                                    }

                                    count++;
                                }

                                synccount += "Total checked caf activity: " + (count + 1) + " out of " + datacount + "\n";

                                var logType = "App Log";
                                var log = "Initialized re-sync (<b>CAF Activity</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }
                            else
                            {
                                await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE ContactID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            ReSyncEmailRecipient(host, database, contact, ipaddress);
                        }
                        else
                        {
                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                            OnSyncFailed();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void ReSyncEmailRecipient(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-email-recipient-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        lblStatus.Text = "Initializing first-time email recipient sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();
                        
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        lblStatus.Text = "Getting data from the server";

                        var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                        string contentType = "application/json";
                        JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database },
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
                                var dataresult = JsonConvert.DeserializeObject<List<UserEmailTable>>(content, settings);
                                var datacount = dataresult.Count;

                                for (int i = 0; i < datacount; i++)
                                {
                                    lblStatus.Text = "Syncing email recipient " + count + " out of " + datacount;

                                    var item = dataresult[i];
                                    var contactsID = item.ContactID;

                                    var getContacts = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ?", contactsID);
                                    var counts = getContacts.Result.Count;

                                    if (counts == 1)
                                    {
                                        await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET Checked = ? WHERE ContactID = ?", 1, contactsID);
                                    }

                                    count++;
                                }

                                synccount += "Total checked email recipient: " + (count + 1) + " out of " + datacount + "\n";

                                var logType = "App Log";
                                var log = "Initialized re-sync (<b>Email Recipient</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }
                            else
                            {
                                await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(default_datetime), contact);
                            }

                            await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ? AND Checked != ?", DateTime.Parse(default_datetime), contact, 1);

                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            lblStatus.Text = "Syncing failed. Server is unreachable.";
                            OnSyncFailed();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Syncing failed. Server is unreachable.";
                        OnSyncFailed();
                    }
                }
                else
                {
                    lblStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    OnSyncFailed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public void OnSyncComplete()
        {
            Ping ping = new Ping();
            PingReply pingresult = ping.Send(ipaddress, 800);

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
             PingReply pingresult = ping.Send(ipaddress, 800);

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
                PingReply pingresult = ping.Send(ipaddress, 800);

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
                            ReSyncContacts(host, database, contact, ipaddress);

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
                            ReSyncContacts(host, database, contact, ipaddress);

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

        public async void Save_Logs(string contactID, string logType, string log, string database, int deleted)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var logs_insert = new UserLogsTable
            {
                ContactID = contactID,
                LogType = logType,
                Log = log,
                LogDate = DateTime.Parse(current_datetime),
                DatabaseName = database,
                Deleted = deleted,
                LastUpdated = DateTime.Parse(current_datetime)
            };

            await conn.InsertOrReplaceAsync(logs_insert);
        }
    }
}