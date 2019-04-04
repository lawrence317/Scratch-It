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
using System.Net.Sockets;
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
                    }
                    else
                    {
                        btnLogin.IsEnabled = false;
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
                }
            }
        }

        void Init() 
        {
            var ipaddress = Preferences.Get("ipaddress", String.Empty, "private_prefs");
            if (string.IsNullOrEmpty(ipaddress))
            {
                entIPAddress.Text = Constants.server_ip;
            }
            else
            {
                entIPAddress.Text = ipaddress;
            }

            var host = Preferences.Get("host", String.Empty, "private_prefs");
            if (string.IsNullOrEmpty(ipaddress))
            {
                entHost.Text = Constants.hostname;
            }
            else
            {
                entHost.Text = host;
            }

            var database = Preferences.Get("database", String.Empty, "private_prefs");
            if (string.IsNullOrEmpty(ipaddress))
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
            entPassword.Completed += (s, e) => Login();
            lblVersion.Text = Constants.appversion;
            lblRegistrationCode.Text = "Device ID: " + CrossDeviceInfo.Current.Id;
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
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        public class ServerMessage
        {
            public string Message { get; set; }
            public string ContactID { get; set; }
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
            
            

            //Check if hostname, database, userName, password is not empty
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
                    string apifile = "login-api.php";

                    var link = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + apifile;
                    string contentType = "application/json";
                    JObject json = new JObject
                    {
                        { "Host", hostName },
                        { "Database", database },
                        { "userName", userName },
                        { "Password", password },
                        { "RegistrationCode", Constants.deviceID }
                    };

                    HttpClient client = new HttpClient();
                    var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var settings = new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };

                            var loginresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                            var item = loginresult[0];
                            var message = item.Message;

                            if (message.Equals("Subscription Expired"))
                            {
                                await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "userName: " + userName + "<br/>" +
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
                            else if (message.Equals("Subscription Trial Expired"))
                            {
                                await DisplayAlert("Trial Subscription Error", "Your trial subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "userName: " + userName + "<br/>" +
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
                            else if (message.Equals("Subscription Not Found"))
                            {
                                var trialsub = await DisplayAlert("Subscription Not Found", "Your device is not registered, please contact your administrator to register your device", "Activate Trial", "Send Email");

                                if (trialsub == true)
                                {
                                    var current_date = DateTime.Now.ToString("yyyy-MM-dd");

                                    string trialapifile = "activate-trial-api.php";

                                    var triallink = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + trialapifile;
                                    string trialcontentType = "application/json";
                                    JObject trialjson = new JObject
                                    {
                                        { "Host", hostName },
                                        { "Database", database },
                                        { "RegistrationCode", Constants.deviceID },
                                        { "Date",  DateTime.Parse(current_date)},
                                        { "Username", userName}
                                    };

                                    HttpClient trialclient = new HttpClient();
                                    var trialresponse = await trialclient.PostAsync(triallink, new StringContent(trialjson.ToString(), Encoding.UTF8, trialcontentType));

                                    if (trialresponse.IsSuccessStatusCode)
                                    {
                                        var trcontent = await trialresponse.Content.ReadAsStringAsync();

                                        if (!string.IsNullOrEmpty(trcontent))
                                        {
                                            var trialresult = JsonConvert.DeserializeObject<List<ServerMessage>>(trcontent, settings);

                                            var trialitem = trialresult[0];
                                            var trialmessage = trialitem.Message;
                                            var trialcontactid = trialitem.ContactID;

                                            if (trialmessage.Equals("Inserted"))
                                            {
                                                var logType = "App Log";
                                                var log = "Activated Trial (<b>" + userName + "</b>) <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                                int deleted = 0;

                                                Save_Logs(trialcontactid, logType, log, database, deleted);

                                                await DisplayAlert("Trial Activated", "You activated trial for 30 days", "Got it");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Offline_Login();
                                    }
                                }
                                else
                                {
                                    var subject = "Register Device: " + userName + " - " + Constants.deviceID;
                                    var body = "Good Day!<br/><br/> " +
                                        "This user needs to register the device.<br/><br/>" +
                                        "userName: " + userName + "<br/>" +
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
                            else if (message.Equals("Incorrect Login"))
                            {
                                await DisplayAlert("Login Error", "userName or password is incorrect", "Got it");
                            }
                            else if (message.Equals("Credential Correct"))
                            {
                                var result = JsonConvert.DeserializeObject<List<ServerMessage>>(content);

                                var contactID = result[0].ContactID;

                                var logType = "App Log";
                                var log = "Logged in (<b>" + userName + "</b>) <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int deleted = 0;

                                Save_Preferences(userName, password, contactID);
                                Save_Logs(contactID, logType, log, database, deleted);

                                await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress));
                            }
                            else if (message.Equals("Not Connected"))
                            {
                                await DisplayAlert("Login Error", "Please check server and database name", "Got it");
                            }
                        }
                    }
                    else
                    {
                        Offline_Login();
                    }
                }
                else
                {
                    Offline_Login();
                }
            }
        }

        public async void Offline_Login()
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var hostName = entHost.Text;
                var database = entDatabase.Text;
                var userName = entUser.Text;
                var password = entPassword.Text;
                var ipaddress = entIPAddress.Text;

                var getUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE UserID = ? AND UsrPassword = ? AND UserStatus='Active'", userName, password);
                var result = getUser.Result.Count;

                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (result < 1)
                {
                    await DisplayAlert("Login Error", "userName or password is incorrect", "Got it");
                }
                else
                {
                    var item = getUser.Result[0];
                    var contactID = item.ContactID;

                    var getSubscription = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                    var subresult = getSubscription.Result.Count;

                    //Check if the device is registered
                    if (subresult < 1)
                    {
                        await DisplayAlert("Subscription Error", "Your device is not registered, please contact your administrator to register your device", "Send Email");

                        var subject = "Register Device: " + userName + " - " + Constants.deviceID;
                        var body = "Good Day!<br/><br/> " +
                            "This user needs to register the device.<br/><br/>" +
                            "userName: " + userName + "<br/>" +
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
                        var startDate = subitem.DateStart;
                        var Trials = subitem.Trials;

                        if (Trials == "0" || Trials == "1")
                        {
                            var ExpirationDate = startDate.AddDays(30);

                            if (DateTime.Now > ExpirationDate)
                            {
                                await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "userName: " + userName + "<br/>" +
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

                                await conn.QueryAsync<SubscriptionTable>("DELETE FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            }
                            else
                            {
                                Preferences.Set("username", userName, "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress));
                            }
                        }
                        else if (Trials == "3")
                        {
                            var ExpirationDate = startDate.AddYears(1);

                            if (DateTime.Now > ExpirationDate)
                            {
                                await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "userName: " + userName + "<br/>" +
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

                                await conn.QueryAsync<SubscriptionTable>("DELETE FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            }
                            else
                            {
                                Preferences.Set("username", userName, "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress));
                            }
                        }
                        else if (Trials == "4")
                        {
                            var ExpirationDate = startDate.AddYears(2);

                            if (DateTime.Now > ExpirationDate)
                            {
                                await DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + userName + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "userName: " + userName + "<br/>" +
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

                                await conn.QueryAsync<SubscriptionTable>("DELETE FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            }
                            else
                            {
                                Preferences.Set("username", userName, "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress));
                            }
                        }
                        else
                        {
                            var logtype = "Mobile Log";
                            var log = "Logged in (<b>" + userName + "</b>)" + "Version: <b>" + Constants.appversion + "</b> Device ID: <b>" + CrossDeviceInfo.Current.Id + "</b>";
                            int deleted = 0;

                            var logs_insert = new UserLogsTable
                            {
                                ContactID = contactID,
                                LogType = logtype,
                                Log = log,
                                LogDate = DateTime.Parse(current_datetime),
                                DatabaseName = database,
                                Deleted = deleted,
                                LastUpdated = DateTime.Parse(current_datetime)
                            };

                            await conn.InsertOrReplaceAsync(logs_insert);

                            Preferences.Set("username", userName, "private_prefs");
                            Preferences.Set("ipaddress", ipaddress, "private_prefs");
                            Preferences.Set("host", hostName, "private_prefs");
                            Preferences.Set("database", database, "private_prefs");
                            Preferences.Set("password", password, "private_prefs");

                            await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        public void Save_Preferences(string username, string password, string contactID)
        {
            Preferences.Set("username", username, "private_prefs");
            Preferences.Set("password", password, "private_prefs");

            Preferences.Set("contactid", contactID, "private_prefs");
        }

        public async void Save_Logs(string contactID, string logType, string log, string databasename, int deleted)
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
                DatabaseName = databasename,
                Deleted = deleted,
                LastUpdated = DateTime.Parse(current_datetime)
            };

            await conn.InsertOrReplaceAsync(logs_insert);
        }

        public void Send_Email(string username)
        {
            var subject = "Subscription Expired: " + username + " - " + Constants.deviceID;
            var body = "Good Day!<br/><br/> " +
                "This user needs new product key.<br/><br/>" +
                "Username: " + username + "<br/>" +
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

                await DisplayAlert("Login Error", "Please fill-up the form", "Got it");
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