﻿using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
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
        string synccount = "Sync Summary: \n\n";
        HttpClient client = new HttpClient();

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

                            if (contactchangesresultCount > 0 || retaileroutletchangesresultCount > 0 || cafchangesresultCount > 0 || actchangesresultCount > 0 || emailchangesresultCount > 0)
                            {
                                lblStatus.Text = "Initializing data sync";
                                lblStatus.BackgroundColor = Color.FromHex("#27ae60");

                                var confirm = await DisplayAlert("Auto-sync Data", "Do you want to sync the data?", "Yes", "No");
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
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                        var confirm = await DisplayAlert("Logout Confirmation", "Do you want to logout?", "Yes", "No");

                        if (confirm.Equals(true))
                        {
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                                    var confirm = await DisplayAlert("Auto-sync Data", "Do you want to sync the data?", "Yes", "No");
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
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
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
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

        // ------------------------------ Auto-Sync Function ------------------------------ //

        public async void SyncContactsClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "sync-contacts-client-update-api.php";
                
                 Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                if (CrossConnectivity.Current.IsConnected)
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

                            var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
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

                            var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    try
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            string apifile1 = "sync-contact-media-path-1-client-update-api.php";

                                            var path1link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile1;

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

                                            var path1response = await Constants.client.PostAsync(path1link, new StringContent(path1json.ToString(), Encoding.UTF8, contentType));

                                            if (path1response.IsSuccessStatusCode)
                                            {
                                                var path1content = await path1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(path1content))
                                                {
                                                    try
                                                    {
                                                        var path1result = JsonConvert.DeserializeObject<List<ServerMessage>>(path1content, settings);

                                                        var path1item = path1result[0];
                                                        var path1message = path1item.Message;

                                                        if (path1message.Equals("Inserted"))
                                                        {
                                                            string apifile2 = "sync-contact-media-path-2-client-update-api.php";

                                                            var path2link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile2;

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

                                                            var path2response = await Constants.client.PostAsync(path2link, new StringContent(path2json.ToString(), Encoding.UTF8, contentType));

                                                            if (path2response.IsSuccessStatusCode)
                                                            {
                                                                var path2content = await path2response.Content.ReadAsStringAsync();
                                                                if (!string.IsNullOrEmpty(path2content))
                                                                {
                                                                    try
                                                                    {
                                                                        var path2result = JsonConvert.DeserializeObject<List<ServerMessage>>(path2content, settings);

                                                                        var path2item = path2result[0];
                                                                        var path2message = path2item.Message;

                                                                        if (path2message.Equals("Inserted"))
                                                                        {
                                                                            string apifile3 = "sync-contact-media-path-3-client-update-api.php";

                                                                            var path3link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile3;

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

                                                                            var path3response = await Constants.client.PostAsync(path3link, new StringContent(path3json.ToString(), Encoding.UTF8, contentType));

                                                                            if (path3response.IsSuccessStatusCode)
                                                                            {
                                                                                var path3content = await path3response.Content.ReadAsStringAsync();
                                                                                if (!string.IsNullOrEmpty(path3content))
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        var path3result = JsonConvert.DeserializeObject<List<ServerMessage>>(path3content, settings);

                                                                                        var path3item = path3result[0];
                                                                                        var path3message = path3item.Message;

                                                                                        if (path3message.Equals("Inserted"))
                                                                                        {
                                                                                            if (String.IsNullOrEmpty(mobileVideo))
                                                                                            {
                                                                                                await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contactID);

                                                                                                clientupdate++;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                string apifile4 = "sync-contact-media-path-4-client-update-api.php";

                                                                                                var path4link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile4;

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

                                                                                                var path4response = await Constants.client.PostAsync(path4link, new StringContent(path4json.ToString(), Encoding.UTF8, contentType));

                                                                                                if (path4response.IsSuccessStatusCode)
                                                                                                {
                                                                                                    var path4content = await path4response.Content.ReadAsStringAsync();
                                                                                                    if (!string.IsNullOrEmpty(path4content))
                                                                                                    {
                                                                                                        try
                                                                                                        {
                                                                                                            var path4result = JsonConvert.DeserializeObject<List<ServerMessage>>(path4content, settings);

                                                                                                            var path4item = path4result[0];
                                                                                                            var path4message = path4item.Message;

                                                                                                            if (path4message.Equals("Inserted"))
                                                                                                            {
                                                                                                                await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contactID);

                                                                                                                clientupdate++;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path4message + "\n\n Do you want to retry?", "Yes", "No");

                                                                                                                if (retry.Equals(true))
                                                                                                                {
                                                                                                                    SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    OnSyncFailed();
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        catch
                                                                                                        {
                                                                                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path4content + "\n\n Do you want to retry?", "Yes", "No");

                                                                                                            if (retry.Equals(true))
                                                                                                            {
                                                                                                                SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                OnSyncFailed();
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path3message + "\n\n Do you want to retry?", "Yes", "No");

                                                                                                    if (retry.Equals(true))
                                                                                                    {
                                                                                                        SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        OnSyncFailed();
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path3message + "\n\n Do you want to retry?", "Yes", "No");

                                                                                            if (retry.Equals(true))
                                                                                            {
                                                                                                SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                OnSyncFailed();
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    catch
                                                                                    {
                                                                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path3content + "\n\n Do you want to retry?", "Yes", "No");

                                                                                        if (retry.Equals(true))
                                                                                        {
                                                                                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            OnSyncFailed();
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path2message + "\n\n Do you want to retry?", "Yes", "No");

                                                                                if (retry.Equals(true))
                                                                                {
                                                                                    SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                                }
                                                                                else
                                                                                {
                                                                                    OnSyncFailed();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    catch
                                                                    {
                                                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path2content + "\n\n Do you want to retry?", "Yes", "No");

                                                                        if (retry.Equals(true))
                                                                        {
                                                                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                        }
                                                                        else
                                                                        {
                                                                            OnSyncFailed();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path1message + "\n\n Do you want to retry?", "Yes", "No");

                                                                if (retry.Equals(true))
                                                                {
                                                                    SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                                }
                                                                else
                                                                {
                                                                    OnSyncFailed();
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path1content + "\n\n Do you want to retry?", "Yes", "No");

                                                        if (retry.Equals(true))
                                                        {
                                                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                        }
                                                        else
                                                        {
                                                            OnSyncFailed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + path1response.StatusCode + " Do you want to retry?", "Yes", "No");

                                                if (retry.Equals(true))
                                                {
                                                    SyncContactsClientUpdate(host, database, contact, ipaddress);
                                                }
                                                else
                                                {
                                                    OnSyncFailed();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + datamessage + "\n\n Do you want to retry?", "Yes", "No");

                                            if (retry.Equals(true))
                                            {
                                                SyncContactsClientUpdate(host, database, contact, ipaddress);
                                            }
                                            else
                                            {
                                                OnSyncFailed();
                                            };
                                        }
                                    }
                                    catch
                                    {
                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + content + "\n\n Do you want to retry?", "Yes", "No");

                                        if (retry.Equals(true))
                                        {
                                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                                        }
                                        else
                                        {
                                            OnSyncFailed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n"+ response.StatusCode +" Do you want to retry?", "Yes", "No");

                                if (retry.Equals(true))
                                {
                                    SyncContactsClientUpdate(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced client contacts update: " + clientupdate + "\n";

                        var logType = "App Log";
                        var log = "Sent client updates to the server (<b>Contacts</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                        int logdeleted = 0;

                        Save_Logs(contact, logType, log, database, logdeleted);

                        SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        SyncContactsClientUpdate(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    SyncContactsClientUpdate(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void SyncRetailerOutletClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "sync-retailer-outlet-client-update-api.php";
                
                 Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                if (CrossConnectivity.Current.IsConnected)
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

                            var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
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

                            var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    try
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE RetailerCode = ?", DateTime.Parse(current_datetime), retailerCode);

                                            clientupdate++;
                                        }
                                        else
                                        {
                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + datamessage + "\n\n Do you want to retry?", "Yes", "No");

                                            if (retry.Equals(true))
                                            {
                                                SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                                            }
                                            else
                                            {
                                                OnSyncFailed();
                                            };
                                        }
                                    }
                                    catch
                                    {
                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + content + "\n\n Do you want to retry?", "Yes", "No");

                                        if (retry.Equals(true))
                                        {
                                            SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                                        }
                                        else
                                        {
                                            OnSyncFailed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n"+ response.StatusCode +" Do you want to retry?", "Yes", "No");

                                if (retry.Equals(true))
                                {
                                    SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced client retailer outlet update: " + clientupdate + "\n";

                        var logType = "App Log";
                        var log = "Sent client updates to the server (<b>Retailer Outlet</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                        int logdeleted = 0;

                        Save_Logs(contact, logType, log, database, logdeleted);

                        SyncCAFClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        SyncCAFClientUpdate(host, database, contact, ipaddress);
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void SyncCAFClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "sync-caf-client-update-api.php";
                
                 Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                if (CrossConnectivity.Current.IsConnected)
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

                            var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
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


                            var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(content))
                                {
                                    try
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            string apifile1 = "sync-caf-media-path-1-client-update-api.php";

                                            var path1link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile1;

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

                                            var path1response = await Constants.client.PostAsync(path1link, new StringContent(path1json.ToString(), Encoding.UTF8, contentType));

                                            if (path1response.IsSuccessStatusCode)
                                            {
                                                var path1content = await path1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(path1content))
                                                {
                                                    try
                                                    {
                                                        var path1result = JsonConvert.DeserializeObject<List<ServerMessage>>(path1content, settings);

                                                        var path1item = path1result[0];
                                                        var path1message = path1item.Message;

                                                        if (path1message.Equals("Inserted"))
                                                        {
                                                            string apifile2 = "sync-caf-media-path-2-client-update-api.php";

                                                            var path2link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile2;

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

                                                            var path2response = await Constants.client.PostAsync(path2link, new StringContent(path2json.ToString(), Encoding.UTF8, contentType));

                                                            if (path2response.IsSuccessStatusCode)
                                                            {
                                                                var path2content = await path2response.Content.ReadAsStringAsync();
                                                                if (!string.IsNullOrEmpty(path2content))
                                                                {
                                                                    try
                                                                    {
                                                                        var path2result = JsonConvert.DeserializeObject<List<ServerMessage>>(path2content, settings);

                                                                        var path2item = path2result[0];
                                                                        var path2message = path2item.Message;

                                                                        if (path2message.Equals("Inserted"))
                                                                        {
                                                                            string apifile3 = "sync-caf-media-path-3-client-update-api.php";

                                                                            var path3link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile3;

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

                                                                            var path3response = await Constants.client.PostAsync(path3link, new StringContent(path3json.ToString(), Encoding.UTF8, contentType));

                                                                            if (path3response.IsSuccessStatusCode)
                                                                            {
                                                                                var path3content = await path3response.Content.ReadAsStringAsync();
                                                                                if (!string.IsNullOrEmpty(path3content))
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        var path3result = JsonConvert.DeserializeObject<List<ServerMessage>>(path3content, settings);

                                                                                        var path3item = path3result[0];
                                                                                        var path3message = path3item.Message;

                                                                                        if (path3message.Equals("Inserted"))
                                                                                        {
                                                                                            if (String.IsNullOrEmpty(mobileVideo))
                                                                                            {
                                                                                                await conn.QueryAsync<CAFTable>("UPDATE tblCAF SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), cafNo);

                                                                                                clientupdate++;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                string apifile4 = "sync-caf-media-path-4-client-update-api.php";

                                                                                                var path4link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile4;

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

                                                                                                var path4response = await Constants.client.PostAsync(path4link, new StringContent(path4json.ToString(), Encoding.UTF8, contentType));

                                                                                                if (path4response.IsSuccessStatusCode)
                                                                                                {
                                                                                                    var path4content = await path4response.Content.ReadAsStringAsync();
                                                                                                    if (!string.IsNullOrEmpty(path4content))
                                                                                                    {
                                                                                                        try
                                                                                                        {
                                                                                                            var path4result = JsonConvert.DeserializeObject<List<ServerMessage>>(path4content, settings);

                                                                                                            var path4item = path4result[0];
                                                                                                            var path4message = path4item.Message;

                                                                                                            if (path4message.Equals("Inserted"))
                                                                                                            {
                                                                                                                await conn.QueryAsync<CAFTable>("UPDATE tblCAF SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), cafNo);

                                                                                                                clientupdate++;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path4message + "\n\n Do you want to retry?", "Yes", "No");

                                                                                                                if (retry.Equals(true))
                                                                                                                {
                                                                                                                    SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    OnSyncFailed();
                                                                                                                };
                                                                                                            }

                                                                                                        }
                                                                                                        catch
                                                                                                        {
                                                                                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path4content + "\n\n Do you want to retry?", "Yes", "No");

                                                                                                            if (retry.Equals(true))
                                                                                                            {
                                                                                                                SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                OnSyncFailed();
                                                                                                            };
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + path3response.StatusCode + " Do you want to retry?", "Yes", "No");

                                                                                                    if (retry.Equals(true))
                                                                                                    {
                                                                                                        SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        OnSyncFailed();
                                                                                                    };
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path3message + "\n\n Do you want to retry?", "Yes", "No");

                                                                                            if (retry.Equals(true))
                                                                                            {
                                                                                                SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                OnSyncFailed();
                                                                                            };
                                                                                        }

                                                                                    }
                                                                                    catch
                                                                                    {
                                                                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path3content + "\n\n Do you want to retry?", "Yes", "No");

                                                                                        if (retry.Equals(true))
                                                                                        {
                                                                                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            OnSyncFailed();
                                                                                        };
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + path3response.StatusCode + " Do you want to retry?", "Yes", "No");

                                                                                if (retry.Equals(true))
                                                                                {
                                                                                    SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                                }
                                                                                else
                                                                                {
                                                                                    OnSyncFailed();
                                                                                };
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path2message + "\n\n Do you want to retry?", "Yes", "No");

                                                                            if (retry.Equals(true))
                                                                            {
                                                                                SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                            }
                                                                            else
                                                                            {
                                                                                OnSyncFailed();
                                                                            };
                                                                        }

                                                                    }
                                                                    catch
                                                                    {
                                                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path2content + "\n\n Do you want to retry?", "Yes", "No");

                                                                        if (retry.Equals(true))
                                                                        {
                                                                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                        }
                                                                        else
                                                                        {
                                                                            OnSyncFailed();
                                                                        };
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + path2response.StatusCode + " Do you want to retry?", "Yes", "No");

                                                                if (retry.Equals(true))
                                                                {
                                                                    SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                                }
                                                                else
                                                                {
                                                                    OnSyncFailed();
                                                                };
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path1message + "\n\n Do you want to retry?", "Yes", "No");

                                                            if (retry.Equals(true))
                                                            {
                                                                SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                            }
                                                            else
                                                            {
                                                                OnSyncFailed();
                                                            };
                                                        }

                                                    }
                                                    catch
                                                    {
                                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + path1content + "\n\n Do you want to retry?", "Yes", "No");

                                                        if (retry.Equals(true))
                                                        {
                                                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                        }
                                                        else
                                                        {
                                                            OnSyncFailed();
                                                        };
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + path1response.StatusCode + " Do you want to retry?", "Yes", "No");

                                                if (retry.Equals(true))
                                                {
                                                    SyncCAFClientUpdate(host, database, contact, ipaddress);
                                                }
                                                else
                                                {
                                                    OnSyncFailed();
                                                };
                                            }
                                        }
                                        else
                                        {
                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + datamessage + "\n\n Do you want to retry?", "Yes", "No");

                                            if (retry.Equals(true))
                                            {
                                                SyncCAFClientUpdate(host, database, contact, ipaddress);
                                            }
                                            else
                                            {
                                                OnSyncFailed();
                                            };
                                        }
                                    }
                                    catch
                                    {
                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + content + "\n\n Do you want to retry?", "Yes", "No");

                                        if (retry.Equals(true))
                                        {
                                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                                        }
                                        else
                                        {
                                            OnSyncFailed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n"+ response.StatusCode +" Do you want to retry?", "Yes", "No");

                                if (retry.Equals(true))
                                {
                                    SyncCAFClientUpdate(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced client caf update: " + clientupdate + "\n";

                        var logType = "App Log";
                        var log = "Sent client updates to the server (<b>CAF</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                        int logdeleted = 0;

                        Save_Logs(contact, logType, log, database, logdeleted);
                    }

                    SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                }
                else
                {

                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        SyncCAFClientUpdate(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    SyncCAFClientUpdate(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void SyncCAFActivityClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "sync-caf-activity-client-update-api.php";
                
                 Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Initializing caf activity client changes sync";

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var datachanges = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
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
                            var contactid = result.ContactID;
                            var activityID = result.ActivityID;
                            var lastsync = DateTime.Parse(current_datetime);
                            var lastupdated = result.LastUpdated;
                            var deleted = result.Deleted;

                            var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                            string contentType = "application/json";
                            JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database },
                                { "CAFNo", cafNo },
                                { "ContactID", contactid },
                                { "ActivityID", activityID },
                                { "LastUpdated", lastupdated },
                                { "Deleted", deleted }
                            };

                            var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    try
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), cafNo);

                                            clientupdate++;
                                        }
                                        else
                                        {
                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + datamessage + "\n\n Do you want to retry?", "Yes", "No");

                                            if (retry.Equals(true))
                                            {
                                                SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                                            }
                                            else
                                            {
                                                OnSyncFailed();
                                            };
                                        }
                                    }
                                    catch
                                    {
                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + content + "\n\n Do you want to retry?", "Yes", "No");

                                        if (retry.Equals(true))
                                        {
                                            SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                                        }
                                        else
                                        {
                                            OnSyncFailed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n"+ response.StatusCode +" Do you want to retry?", "Yes", "No");

                                if (retry.Equals(true))
                                {
                                    SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced client caf activity update: " + clientupdate + "\n";

                        var logType = "App Log";
                        var log = "Sent client updates to the server (<b>CAF Activity</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                        int logdeleted = 0;

                        Save_Logs(contact, logType, log, database, logdeleted);

                        SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void SyncEmailRecipientClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "sync-email-recipient-client-update-api.php";
                
                 Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                if (CrossConnectivity.Current.IsConnected)
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

                            var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
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

                            var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    try
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contactsID);

                                            clientupdate++;
                                        }
                                        else
                                        {
                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + datamessage + "\n\n Do you want to retry?", "Yes", "No");

                                            if (retry.Equals(true))
                                            {
                                                SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                                            }
                                            else
                                            {
                                                OnSyncFailed();
                                            };
                                        }
                                    }
                                    catch
                                    {
                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + content + "\n\n Do you want to retry?", "Yes", "No");

                                        if (retry.Equals(true))
                                        {
                                            SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                                        }
                                        else
                                        {
                                            OnSyncFailed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n"+ response.StatusCode +" Do you want to retry?", "Yes", "No");

                                if (retry.Equals(true))
                                {
                                    SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced client email recipient update: " + clientupdate + "\n";

                        var logType = "App Log";
                        var log = "Sent client updates to the server (<b>Email Recipient</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                        int logdeleted = 0;

                        Save_Logs(contact, logType, log, database, logdeleted);

                        SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void SyncUserLogsClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "sync-user-logs-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    lblStatus.Text = "Initializing user logs client changes sync";

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

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

                            var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
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

                            var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    try
                                    {
                                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                                        var dataitem = dataresult[0];
                                        var datamessage = dataitem.Message;

                                        if (datamessage.Equals("Inserted"))
                                        {
                                            await conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET LastSync = ? WHERE ContactID = ? AND LogType = ? AND Log = ? AND LogDate = ? AND DatabaseName = ?", DateTime.Parse(current_datetime), contactsID, logtype, logs, logDate, database);

                                            clientupdate++;
                                        }
                                        else
                                        {
                                            var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + datamessage + "\n\n Do you want to retry?", "Yes", "No");

                                            if (retry.Equals(true))
                                            {
                                                SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                                            }
                                            else
                                            {
                                                OnSyncFailed();
                                            };
                                        }
                                    }
                                    catch
                                    {
                                        var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + content + "\n\n Do you want to retry?", "Yes", "No");

                                        if (retry.Equals(true))
                                        {
                                            SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                                        }
                                        else
                                        {
                                            OnSyncFailed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n"+ response.StatusCode +" Do you want to retry?", "Yes", "No");

                                if (retry.Equals(true))
                                {
                                    SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                                }
                                else
                                {
                                    OnSyncFailed();
                                }
                            }
                        }

                        synccount += "Total synced client user logs update: " + clientupdate + "\n";

                        var logType = "App Log";
                        var log = "Sent client updates to the server (<b>User Logs</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                        int logdeleted = 0;

                        Save_Logs(contact, logType, log, database, logdeleted);

                        OnSyncComplete();
                    }
                    else
                    {
                        OnSyncComplete();
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        SyncUserLogsClientUpdate(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        // ------------------------------ Re-Sync All Function ------------------------------ //

        public async void ReSyncAllContacts(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-contacts-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting contact data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking contacts " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var contactID = item.ContactID;

                                await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ? WHERE ContactID = ?", 1, contactID);

                                count++;
                            }

                            await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            ReSyncAllRetailerOutlet(host, database, contact, ipaddress);
                        }
                        else
                        {
                            ReSyncAllRetailerOutlet(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncAllContacts(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncAllContacts(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncAllContacts(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void ReSyncAllRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-retailer-outlet-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting retailer outlet data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking retailer outlet " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var retailerCode = item.RetailerCode;

                                await conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET Existed = ? WHERE RetailerCode = ?", 1, retailerCode);

                                count++;
                            }

                            await conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            ReSyncAllCAF(host, database, contact, ipaddress);
                        }
                        else
                        {
                            ReSyncAllCAF(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncAllRetailerOutlet(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncAllRetailerOutlet(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncAllRetailerOutlet(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void ReSyncAllCAF(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-caf-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<CAFData>("UPDATE tblCaf SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting caf data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking caf " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;

                                await conn.QueryAsync<CAFData>("UPDATE tblCaf SET Existed = ? WHERE CAFNo = ?", 1, cafNo);

                                count++;
                            }

                            await conn.QueryAsync<CAFData>("UPDATE tblCaf SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            ReSyncAllCAFActivity(host, database, contact, ipaddress);
                        }
                        else
                        {
                            ReSyncAllCAFActivity(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncAllCAF(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncAllCAF(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncAllCAF(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void ReSyncAllCAFActivity(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-caf-activity-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<ActivityData>("UPDATE tblActivity SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting caf activity data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ActivityData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking caf activity " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;
                                var act = item.ActivityID;

                                await conn.QueryAsync<ActivityData>("UPDATE tblActivity SET Existed = ? WHERE CAFNo = ? AND ActivityID = ?", 1, cafNo, act);

                                count++;
                            }

                            await conn.QueryAsync<ActivityData>("UPDATE tblActivity SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncAllCAFActivity(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncAllCAFActivity(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncAllCAFActivity(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        // ------------------------------ Re-Sync Function ------------------------------ //

        public async void ReSyncContacts(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-contacts-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting contact data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking contacts " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var contactID = item.ContactID;

                                await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ? WHERE ContactID = ?", 1, contactID);

                                count++;
                            }

                            await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncContacts(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncContacts(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncContacts(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void ReSyncRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-retailer-outlet-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting retailer outlet data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking retailer outlet " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var retailerCode = item.RetailerCode;

                                await conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET Existed = ? WHERE RetailerCode = ?", 1, retailerCode);

                                count++;
                            }

                            await conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncRetailerOutlet(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncRetailerOutlet(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncRetailerOutlet(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void ReSyncCAF(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-caf-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<CAFData>("UPDATE tblCaf SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting caf data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking caf " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;

                                await conn.QueryAsync<CAFData>("UPDATE tblCaf SET Existed = ? WHERE CAFNo = ?", 1, cafNo);

                                count++;
                            }

                            await conn.QueryAsync<CAFData>("UPDATE tblCaf SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncCAF(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncCAF(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncCAF(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public async void ReSyncCAFActivity(string host, string database, string contact, string ipaddress)
        {
            try
            {
                lblStatus.Text = "Checking internet connection";

                string apifile = "resync-caf-activity-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    var default_datetime = "0001-01-01 00:00:00";

                    await conn.QueryAsync<ActivityData>("UPDATE tblActivity SET Existed = ?", 0);

                    int count = 1;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    lblStatus.Text = "Getting caf activity data from the server";

                    var link = "http://" + ipaddress + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", host },
                        { "Database", database },
                        { "ContactID", contact }
                    };

                    
                     Constants.client.DefaultRequestHeaders.ConnectionClose = false;

                    var response = await Constants.client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ActivityData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                lblStatus.Text = "Checking caf activity " + count + " out of " + datacount;

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;
                                var act = item.ActivityID;

                                await conn.QueryAsync<ActivityData>("UPDATE tblActivity SET Existed = ? WHERE CAFNo = ? AND ActivityID = ?", 1, cafNo, act);

                                count++;
                            }

                            await conn.QueryAsync<ActivityData>("UPDATE tblActivity SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);

                            SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        var retry = await DisplayAlert("Application Error", "Syncing failed. Server is unreachable.\n\n Error:\n\n" + response.StatusCode + " Do you want to retry?", "Yes", "No");

                        if (retry.Equals(true))
                        {
                            ReSyncCAFActivity(host, database, contact, ipaddress);
                        }
                        else
                        {
                            OnSyncFailed();
                        }
                    }
                }
                else
                {
                    var retry = await DisplayAlert("Application Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry.Equals(true))
                    {
                        ReSyncCAFActivity(host, database, contact, ipaddress);
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
                var retry = await DisplayAlert("Application Error", "Syncing failed. Failed to send the data.\n\n Error:\n\n" + ex.Message.ToString() + "\n\n Do you want to retry?", "Yes", "No");

                if (retry.Equals(true))
                {
                    ReSyncCAFActivity(host, database, contact, ipaddress);
                }
                else
                {
                    OnSyncFailed();
                };
            }
        }

        public void OnSyncComplete()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (synccount == "Sync Summary: \n\n")
                {
                    DisplayAlert("Sync Completed", "Data has been synced successfully", "Ok");
                }
                else
                {
                    DisplayAlert("Sync Completed", synccount, "Ok");
                }
                
                synccount = "";
                lblStatus.Text = "Online - Connected to server";
                lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
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
            if (CrossConnectivity.Current.IsConnected)
            {
                lblStatus.Text = "Online - Connected to server";
                lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
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

            return;
        }

        private async void BtnResend_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                string action = await DisplayActionSheet("What data do you want to sync?", "Cancel", null, "All Data", "Retailer Data", "Retailer Outlet Data", "CAF Data", "CAF Activity Data");

                if(action == "All Data")
                {
                    Disable_ui();
                    await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ? WHERE Supervisor = ?", 0, contact);
                    await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET Existed = ? WHERE Supervisor = ?", 0, contact);
                    await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET Existed = ? WHERE EmployeeID = ?", 0, contact);
                    await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET Existed = ? WHERE ContactID = ?", 0, contact);

                    ReSyncAllContacts(host, database, contact, ipaddress);
                }
                else if(action == "Retailer Data")
                {
                    Disable_ui();
                    await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ? WHERE Supervisor = ?", 0, contact);

                    ReSyncContacts(host, database, contact, ipaddress);
                }
                else if (action == "Retailer Outlet Data")
                {
                    Disable_ui();
                    await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET Existed = ? WHERE Supervisor = ?", 0, contact);

                    ReSyncRetailerOutlet(host, database, contact, ipaddress);
                }
                else if (action == "CAF Data")
                {
                    Disable_ui();
                    await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET Existed = ? WHERE EmployeeID = ?", 0, contact);

                    ReSyncCAF(host, database, contact, ipaddress);
                }
                else if (action == "CAF Activity Data")
                {
                    Disable_ui();
                    await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET Existed = ? WHERE ContactID = ?", 0, contact);

                    ReSyncCAFActivity(host, database, contact, ipaddress);
                }
            }
            else
            {
                await DisplayAlert("Application Error", "Re-sync failed. Please connect to the internet to re-sync your data", "Ok");
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

            await conn.InsertAsync(logs_insert);
        }

        public void Disable_ui()
        {
            lblStatus.Text = "Initializing data sync";
            lblStatus.BackgroundColor = Color.FromHex("#27ae60");

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