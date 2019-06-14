using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Init();
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
                        btnLogin.IsEnabled = true;
                    }
                    else
                    {
                       btnLogin.IsEnabled = false;
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                }
            }
        }

        void Init() 
        {
            CreateTableAsync();
            entIPAddress.Text = "tbs.scratchit.ph";

            var host = Preferences.Get("host", String.Empty, "private_prefs");
            if (string.IsNullOrEmpty(host))
            {
                entHost.Text = Constants.hostname;
            }
            else
            {
                entHost.Text = host;
            }

            var database = Preferences.Get("database", String.Empty, "private_prefs");
            if (string.IsNullOrEmpty(database))
            {
                entDatabase.Text = Constants.database;
            }
            else
            {
                entDatabase.Text = database;
            }

            var userName = Preferences.Get("username", String.Empty, "private_prefs");
            var password = Preferences.Get("password", String.Empty, "private_prefs");

            entUser.Text = userName;
            entPassword.Text = password;

            entUser.Completed += (s, e) => entPassword.Focus();
            entPassword.Completed += (s, e) => Check_Version();
            lblVersion.Text = Constants.appversion;
            lblRegistrationCode.Text = "Device ID: " + CrossDeviceInfo.Current.Id;

            var firstLaunch = VersionTracking.IsFirstLaunchEver;

            if (firstLaunch == true)
            {
                string firsttime = "1";
                Preferences.Set("isfirsttimesync", firsttime, "private_prefs");
            }
        }

        public async void CreateTableAsync()
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (conn != null)
                {
                    try
                    {
                        await conn.CreateTableAsync<UserTable>();
                        await conn.CreateTableAsync<ContactsTable>();
                        await conn.CreateTableAsync<ActivityTable>();
                        await conn.CreateTableAsync<CAFTable>();
                        await conn.CreateTableAsync<RetailerGroupTable>();
                        await conn.CreateTableAsync<UserEmailTable>();
                        await conn.CreateTableAsync<UserLogsTable>();
                        await conn.CreateTableAsync<SubscriptionTable>();
                        await conn.CreateTableAsync<ProvinceTable>();
                        await conn.CreateTableAsync<TownTable>();
                    }
                    catch (Exception ex)
                    {
                        Console.Write("Creating table error " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public class ServerMessage
        {
            public string Message { get; set; }
            public string ContactID { get; set; }
        }

        public async void Check_Version()
        {
            var hostName = entHost.Text;
            var database = entDatabase.Text;
            var userName = entUser.Text;
            var password = entPassword.Text;
            var ipaddress = entIPAddress.Text;

            if (string.IsNullOrEmpty(hostName) || string.IsNullOrEmpty(database) || string.IsNullOrEmpty(ipaddress) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                if (string.IsNullOrEmpty(userName))
                {
                    usernameFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    usernameFrame.BorderColor = Color.FromHex("#f2f2f5");
                }

                if (string.IsNullOrEmpty(password))
                {
                    passwordFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    passwordFrame.BorderColor = Color.FromHex("#f2f2f5");
                }

                await DisplayAlert("Login Error", "Please fill-up the form", "Ok");
            }
            else
            {
                string apifile = "check-version-api.php";

                await App.TodoManager.CheckVersion(hostName, database, ipaddress, apifile, userName, password);
            }
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            Check_Version();
        }

        private void entUser_Unfocused(object sender, FocusEventArgs e)
        {
            if(string.IsNullOrEmpty(entUser.Text))
            {
                usernameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                usernameFrame.BorderColor = Color.FromHex("#f2f2f5");
            }
        }

        private void entPassword_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entPassword.Text))
            {
                passwordFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                passwordFrame.BorderColor = Color.FromHex("#f2f2f5");
            }
        }

        private void BtnConnect_Clicked(object sender, EventArgs e)
        {
            Connect();
        }

        private void BtnChange_Clicked(object sender, EventArgs e)
        {
            connectstack.IsVisible = true;
            loginstack.IsVisible = false;
        }

        public async void Connect()
        {
            var hostName = entHost.Text;
            var database = entDatabase.Text;
            var ipaddress = entIPAddress.Text;

            if (String.IsNullOrEmpty(hostName) || String.IsNullOrEmpty(database) || String.IsNullOrEmpty(ipaddress))
            {
                if (string.IsNullOrEmpty(entHost.Text))
                {
                    hostFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    hostFrame.BorderColor = Color.FromHex("#f2f2f5");
                }

                if (string.IsNullOrEmpty(entDatabase.Text))
                {
                    databaseFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    databaseFrame.BorderColor = Color.FromHex("#f2f2f5");
                }

                if (string.IsNullOrEmpty(entIPAddress.Text))
                {
                    ipaddressFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    ipaddressFrame.BorderColor = Color.FromHex("#f2f2f5");
                }

                await DisplayAlert("Login Error", "Please fill-up the form", "Ok");
            }
            else
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //Check if there is an internet connection
                    connectstack.IsVisible = false;
                    loginstack.IsVisible = true;

                    Preferences.Set("ipaddress", ipaddress, "private_prefs");
                    Preferences.Set("host", hostName, "private_prefs");
                    Preferences.Set("database", database, "private_prefs");
                }
                else
                {
                    //Check if there is an internet connection
                    connectstack.IsVisible = false;
                    loginstack.IsVisible = true;

                    Preferences.Set("ipaddress", ipaddress, "private_prefs");
                    Preferences.Set("host", hostName, "private_prefs");
                    Preferences.Set("database", database, "private_prefs");

                    await DisplayAlert("Connection Error", "Connection error, switching to offline mode", "Ok");
                }
            }
        }
    }
}