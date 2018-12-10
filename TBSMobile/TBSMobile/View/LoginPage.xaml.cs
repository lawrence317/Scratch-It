using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
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
            await CreateTableAsync();
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
                        AutoLogin();
                    }
                    else
                    {
                        btnLogin.IsEnabled = false;
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        void Init() 
        {
            entHost.Text = Constants.hostname;
            entDatabase.Text = Constants.database;
            entIPAddress.Text = Constants.server_ip;
            entUser.Completed += (s, e) => entPassword.Focus();
            entPassword.Completed += (s, e) => Login();
            lblVersion.Text = Constants.appversion;
            lblRegistrationCode.Text = "Device ID: " + CrossDeviceInfo.Current.Id;
        }

        public async Task CreateTableAsync()
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
            }
        }

        public void AutoLogin()
        {
            try
            {
                var username = Preferences.Get("username", String.Empty, "private_prefs");
                var password = Preferences.Get("password", String.Empty, "private_prefs");

                if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
                {
                    entUser.Text = username;
                    entPassword.Text = password;
                    Login();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void Login()
        {
            //Get Database Connection
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var hostName = entHost.Text;
            var database = entDatabase.Text;
            var userName = entUser.Text;
            var password = entPassword.Text;
            var ipaddress = entIPAddress.Text;
            
            string[] pingip = ipaddress.Split(new char[] { '.' });
            byte[] pingipaddress = new byte[] { byte.Parse(pingip[0]), byte.Parse(pingip[1]), byte.Parse(pingip[2]), byte.Parse(pingip[3]) }; 

            //Check if hostname, database, username, password is not empty
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

                await DisplayAlert("Login Error", "Please fill-up the form", "Got it");
            }
            else
            {
                //Check if there is an internet connection
                if (CrossConnectivity.Current.IsConnected)
                {
                    {
                        try
                        {
                            //If there is an internet connection login using server
                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + hostName + "&Database=" + database + "&User=" + userName + "&Password=" + password + "&Request=7ZEGvK" + "&Code=" + Constants.deviceID;
                            var request = HttpWebRequest.Create(string.Format(@link));

                            request.ContentType = "application/json";
                            request.Method = "GET";

                            var ping = new Ping();
                            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

                            if (reply.Status == IPStatus.Success)
                            {
                                //Check HTTPWebResponse
                                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                                {
                                    //If statuscode is Not equal to 'OK' display alert
                                    if (response.StatusCode != HttpStatusCode.OK)
                                    {
                                        await DisplayAlert("Login Error", "Error fetching data. Server returned status code: {0} " + response.StatusCode, "Ok");
                                    }
                                    else
                                    {
                                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                        {
                                            var content = reader.ReadToEnd();

                                            if (!content.Equals("[]") || !string.IsNullOrWhiteSpace(content) || !string.IsNullOrEmpty(content))
                                            {
                                                try
                                                {
                                                    if (content.Equals("[{\"Message\":\"SubscriptionExp\"}]"))
                                                    {
                                                        await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                                        var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                                        var body = "Good Day!<br/><br/> " +
                                                            "This user needs new product key.<br/><br/>" +
                                                            "Username: " + userName + "<br/>" +
                                                            "Device ID: " + Constants.deviceID + "<br/>";

                                                        var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                                        if (emailMessenger.CanSendEmail)
                                                        {
                                                            var emailsend = new EmailMessageBuilder()
                                                            .To(Constants.email)
                                                            .Subject(subject)
                                                            .BodyAsHtml(body)
                                                            .Build();

                                                            emailMessenger.SendEmail(emailsend);
                                                        }
                                                    }
                                                    else if (content.Equals("[{\"Message\":\"Subscription\"}]"))
                                                    {
                                                        await DisplayAlert("Login Error", "Your device is not registered, please contact your administrator to register your device", "Send Email");

                                                        var subject = "Register Device: " + userName + " - " + Constants.deviceID;
                                                        var body = "Good Day!<br/><br/> " +
                                                            "This user needs to register the device.<br/><br/>" +
                                                            "Username: " + userName + "<br/>" +
                                                            "Device ID: " + Constants.deviceID + "<br/>";

                                                        var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                                        if (emailMessenger.CanSendEmail)
                                                        {
                                                            var emailsend = new EmailMessageBuilder()
                                                            .To(Constants.email)
                                                            .Subject(subject)
                                                            .BodyAsHtml(body)
                                                            .Build();

                                                            emailMessenger.SendEmail(emailsend);
                                                        }
                                                    }
                                                    else if (content.Equals("[{\"Message\":\"Credential\"}]"))
                                                    {
                                                        await DisplayAlert("Login Error", "Username or password is incorrect", "Got it");
                                                    }
                                                    else
                                                    {
                                                        var result = JsonConvert.DeserializeObject<List<UserTable>>(content);
                                                        var contactID = result[0].ContactID;

                                                        Preferences.Set("username", userName, "private_prefs");
                                                        Preferences.Set("password", password, "private_prefs");

                                                        await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress, pingipaddress));
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Crashes.TrackError(ex);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    var getUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE UserID = ? AND UserPassword = ? AND UserStatus='Active'", userName, password);
                                    var result = getUser.Result.Count;
                                    var logType = "Login";
                                    var logDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                                    if (result < 1)
                                    {
                                        await DisplayAlert("Login Error", "Username or password is incorrect", "Got it");
                                    }
                                    else
                                    {

                                        var item = getUser.Result[0];
                                        var contactID = item.ContactID;

                                        var getSubscription = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE RegistrationNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                                        var subresult = getSubscription.Result.Count;

                                        //Check if the device is registered
                                        if (subresult < 1)
                                        {
                                            await DisplayAlert("Subscription Error", "Your device is not registered, please contact your administrator to register your device", "Send Email");

                                            var subject = "Register Device: " + userName + " - " + Constants.deviceID;
                                            var body = "Good Day!<br/><br/> " +
                                                "This user needs to register the device.<br/><br/>" +
                                                "Username: " + userName + "<br/>" +
                                                "Device ID: " + Constants.deviceID + "<br/>";

                                            var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                            if (emailMessenger.CanSendEmail)
                                            {
                                                var emailsend = new EmailMessageBuilder()
                                                .To(Constants.email)
                                                .Subject(subject)
                                                .BodyAsHtml(body)
                                                .Build();

                                                emailMessenger.SendEmail(emailsend);
                                            }
                                        }
                                        else
                                        {
                                            var subitem = getSubscription.Result[0];
                                            var NoOfDays = subitem.NoOfDays;
                                            var ExpirationDate = subitem.ExpirationDate;

                                            if (NoOfDays == "30" || NoOfDays == "365")
                                            {
                                                var date_now = DateTime.Today;
                                                var exp_date = DateTime.Parse(ExpirationDate);

                                                //Check subscription expiration
                                                if (date_now <= exp_date)
                                                {
                                                    entUser.Text = "";
                                                    entPassword.Text = "";

                                                    var logs_insert = new UserLogsTable
                                                    {
                                                        ContactID = contactID,
                                                        LogType = logType,
                                                        Log = "",
                                                        LogDate = DateTime.Parse(logDate),
                                                        LastUpdated = DateTime.Parse(current_datetime),
                                                        LastSync = DateTime.Parse(current_datetime)
                                                    };

                                                    await conn.InsertAsync(logs_insert);

                                                    Preferences.Set("username", userName, "private_prefs");
                                                    Preferences.Set("password", password, "private_prefs");

                                                    await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress, pingipaddress));
                                                }
                                                else
                                                {
                                                    await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                                    var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                                    var body = "Good Day!<br/><br/> " +
                                                        "This user needs new product key.<br/><br/>" +
                                                        "Username: " + userName + "<br/>" +
                                                        "Device ID: " + Constants.deviceID + "<br/>";

                                                    var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                                    if (emailMessenger.CanSendEmail)
                                                    {
                                                        var emailsend = new EmailMessageBuilder()
                                                        .To(Constants.email)
                                                        .Subject(subject)
                                                        .BodyAsHtml(body)
                                                        .Build();

                                                        emailMessenger.SendEmail(emailsend);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                entUser.Text = "";
                                                entPassword.Text = "";

                                                var logs_insert = new UserLogsTable
                                                {
                                                    ContactID = contactID,
                                                    LogType = logType,
                                                    Log = "",
                                                    LogDate = DateTime.Parse(logDate),
                                                    LastUpdated = DateTime.Parse(current_datetime),
                                                    LastSync = DateTime.Parse(current_datetime)
                                                };

                                                await conn.InsertAsync(logs_insert);

                                                Preferences.Set("username", userName, "private_prefs");
                                                Preferences.Set("password", password, "private_prefs");

                                                await conn.InsertAsync(logs_insert);
                                                await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress, pingipaddress));
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }
                        }
                        catch
                        {
                            await DisplayAlert("Login Error", "Server is unreachable, please check your connection", "Ok");
                        }
                    }
                }
                else
                {
                    try
                    {
                        var getUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE UserID = ? AND UserPassword = ? AND UserStatus='Active'", userName, password);
                        var result = getUser.Result.Count;
                        var logType = "Login";
                        var logDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        if (result < 1)
                        {
                            await DisplayAlert("Login Error", "Username or password is incorrect", "Got it");
                        }
                        else
                        {

                            var item = getUser.Result[0];
                            var contactID = item.ContactID;

                            var getSubscription = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE RegistrationNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            var subresult = getSubscription.Result.Count;

                            //Check if the device is registered
                            if(subresult < 1)
                            {
                                await DisplayAlert("Subscription Error", "Your device is not registered, please contact your administrator to register your device", "Send Email");

                                var subject = "Register Device: " + userName + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs to register the device.<br/><br/>" +
                                    "Username: " + userName + "<br/>" +
                                    "Device ID: " + Constants.deviceID + "<br/>";

                                var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                if (emailMessenger.CanSendEmail)
                                {
                                    var emailsend = new EmailMessageBuilder()
                                    .To(Constants.email)
                                    .Subject(subject)
                                    .BodyAsHtml(body)
                                    .Build();

                                    emailMessenger.SendEmail(emailsend);
                                }
                            }
                            else
                            {
                                var subitem = getSubscription.Result[0];
                                var NoOfDays = subitem.NoOfDays;
                                var ExpirationDate = subitem.ExpirationDate;

                                if (NoOfDays == "30" || NoOfDays == "365")
                                {
                                    var date_now = DateTime.Today;
                                    var exp_date = DateTime.Parse(ExpirationDate);
                                    
                                    //Check subscription expiration
                                    if (date_now <= exp_date)
                                    {
                                        entUser.Text = "";
                                        entPassword.Text = "";

                                        var logs_insert = new UserLogsTable
                                        {
                                            ContactID = contactID,
                                            LogType = logType,
                                            Log = "",
                                            LogDate = DateTime.Parse(logDate),
                                            LastUpdated = DateTime.Parse(current_datetime),
                                            LastSync = DateTime.Parse(current_datetime)
                                        };

                                        await conn.InsertAsync(logs_insert);

                                        Preferences.Set("username", userName, "private_prefs");
                                        Preferences.Set("password", password, "private_prefs");

                                        await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress, pingipaddress));
                                    }
                                    else
                                    {
                                        await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                        var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                        var body = "Good Day!<br/><br/> " +
                                            "This user needs new product key.<br/><br/>" +
                                            "Username: " + userName + "<br/>" +
                                            "Device ID: " + Constants.deviceID + "<br/>";

                                        var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                        if (emailMessenger.CanSendEmail)
                                        {
                                            var emailsend = new EmailMessageBuilder()
                                            .To(Constants.email)
                                            .Subject(subject)
                                            .BodyAsHtml(body)
                                            .Build();

                                            emailMessenger.SendEmail(emailsend);
                                        }
                                    }
                                }
                                else
                                {
                                    entUser.Text = "";
                                    entPassword.Text = "";

                                    var logs_insert = new UserLogsTable
                                    {
                                        ContactID = contactID,
                                        LogType = logType,
                                        Log = "",
                                        LogDate = DateTime.Parse(logDate),
                                        LastUpdated = DateTime.Parse(current_datetime),
                                        LastSync = DateTime.Parse(current_datetime)
                                    };

                                    await conn.InsertAsync(logs_insert);

                                    Preferences.Set("username", userName, "private_prefs");
                                    Preferences.Set("password", password, "private_prefs");

                                    await conn.InsertAsync(logs_insert);
                                    await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress, pingipaddress));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            Login();
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
    }
}