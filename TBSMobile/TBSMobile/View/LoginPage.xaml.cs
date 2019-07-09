using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using System;
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
                        await StartListening();
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
            
            lblDomain.Text = Constants.domain;
            lblHost.Text = Constants.hostname;

            Preferences.Set("domain", Constants.domain, "private_prefs");
            Preferences.Set("host", Constants.hostname, "private_prefs");

            var server = Preferences.Get("server", String.Empty, "private_prefs");

            if (String.IsNullOrEmpty(server))
            {
                lblServer.Text = "Connected: Live Server";
                serverPicker.SelectedIndex = 0;

                lblDatabase.Text = Constants.livedatabase;
                lblApi.Text = Constants.liveapifolder;

                Preferences.Set("database", Constants.livedatabase, "private_prefs");
                Preferences.Set("apifolder", Constants.liveapifolder, "private_prefs");
            }
            else if (server.Equals("Live Server"))
            {
                lblServer.Text = "Connected: " + server;
                serverPicker.SelectedIndex = 0;

                lblDatabase.Text = Constants.livedatabase;
                lblApi.Text = Constants.liveapifolder;

                Preferences.Set("database", Constants.livedatabase, "private_prefs");
                Preferences.Set("apifolder", Constants.liveapifolder, "private_prefs");
            }
            else if(server.Equals("Test Server"))
            {
                lblServer.Text = "Connected: " + server;
                serverPicker.SelectedIndex = 1;

                lblDatabase.Text = Constants.testdatabase;
                lblApi.Text = Constants.testapifolder;

                Preferences.Set("database", Constants.testdatabase, "private_prefs");
                Preferences.Set("apifolder", Constants.testapifolder, "private_prefs");
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

            if (firstLaunch)
            {
                Preferences.Set("isfirsttimesync", "1", "private_prefs");
            }
        }

        public async void CreateTableAsync()
        {
            try
            {
                if (Constants.conn != null)
                {
                    try
                    {
                        await Constants.conn.CreateTableAsync<UserTable>();
                        await Constants.conn.CreateTableAsync<ContactsTable>();
                        await Constants.conn.CreateTableAsync<ActivityTable>();
                        await Constants.conn.CreateTableAsync<CAFTable>();
                        await Constants.conn.CreateTableAsync<RetailerGroupTable>();
                        await Constants.conn.CreateTableAsync<UserEmailTable>();
                        await Constants.conn.CreateTableAsync<UserLogsTable>();
                        await Constants.conn.CreateTableAsync<SubscriptionTable>();
                        await Constants.conn.CreateTableAsync<ProvinceTable>();
                        await Constants.conn.CreateTableAsync<TownTable>();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
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
            var userName = entUser.Text;
            var password = entPassword.Text;

            var host = Preferences.Get("host", String.Empty, "private_prefs");
            var database = Preferences.Get("database", String.Empty, "private_prefs");
            var domain = Preferences.Get("domain", String.Empty, "private_prefs");
            var apifolder = Preferences.Get("apifolder", String.Empty, "private_prefs");
            string apifile = "check-version-api.php";

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
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

                await DisplayAlert("Login Error", "Please enter your username and password", "Ok");
            }
            else
            {
                await App.TodoManager.CheckVersion(host, database, domain, apifolder, apifile, userName, password, LoginStatus);
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
            Save();
        }

        public async void Save()
        {
            var picker = serverPicker;
            int selectedIndex = picker.SelectedIndex;
            string server = (string)picker.ItemsSource[selectedIndex];

            Preferences.Set("server", server, "private_prefs");

            lblServer.Text = "Connected: " + server;

            if (selectedIndex == 0)
            {
                Preferences.Set("database", Constants.livedatabase, "private_prefs");
                Preferences.Set("apifolder", Constants.liveapifolder, "private_prefs");
            }
            else if (selectedIndex == 1)
            {
                Preferences.Set("database", Constants.testdatabase, "private_prefs");
                Preferences.Set("apifolder", Constants.testapifolder, "private_prefs");
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Connection Error", "Connection error, switching to offline mode", "Ok");
            }

            await DisplayAlert("Connection To Server Warning", "Warning:\nYou are now connecting to " + server, "Ok");

            connectstack.IsVisible = false;
            loginstack.IsVisible = true;
        }

        private void ServerPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            
            if(selectedIndex == 0)
            {
                lblDatabase.Text = Constants.livedatabase;
                lblApi.Text = Constants.liveapifolder;
            }
            else if (selectedIndex == 1)
            {
                lblDatabase.Text = Constants.testdatabase;
                lblApi.Text = Constants.testapifolder;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            connectstack.IsVisible = true;
            loginstack.IsVisible = false;
        }

        private void LoginStatus(string status)
        {
            Device.BeginInvokeOnMainThread(() => {
                string[] loginstatus = status.Split(new char[] { '-' });
                string type = loginstatus[0];
                string message = loginstatus[1];

                lblstatus.Text = message;

                if (type == "0")
                {
                    btnLogin.IsEnabled = false;
                    lblstatus.IsVisible = true;
                }
                else if(type == "1")
                {
                    btnLogin.IsEnabled = true;
                    lblstatus.IsVisible = false;
                }
            });
        }

        async Task StartListening()
        {
            if (!CrossGeolocator.Current.IsListening)
            {
                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(30), 200, false);
            }
        }
    }
}