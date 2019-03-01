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

            var username = Preferences.Get("username", String.Empty, "private_prefs");
            var password = Preferences.Get("password", String.Empty, "private_prefs");

            entUser.Text = username;
            entPassword.Text = password;

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

        public class ServerMessage
        {
            public string Message { get; set; }
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
                    try
                    {
                        Ping ping = new Ping();
                        PingReply pingresult = ping.Send(ipaddress, 200);

                        if (pingresult.Status.ToString() == "Success")
                        {
                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + hostName + "&Database=" + database + "&Request=7ZEGvK";
                            string contentType = "application/json";
                            JObject json = new JObject
                            {
                                { "User", userName },
                                { "Password", password },
                                { "Code", Constants.deviceID }
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
                                    else if (message.Equals("Subscription Trial Expired"))
                                    {
                                        await DisplayAlert("Trial Subscription Error", "Your trial subscription has been expired, please contact your administrator to register your device", "Send Email");

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
                                    else if (message.Equals("Subscription Not Found"))
                                    {
                                        var trialsub = await DisplayAlert("Subscription Not Found", "Your device is not registered, please contact your administrator to register your device", "Activate Trial", "Send Email");

                                        if (trialsub == true)
                                        {
                                            var current_date = DateTime.Now.ToString("yyyy-MM-dd");

                                            var triallink = "http://" + ipaddress + Constants.requestUrl + "Host=" + hostName + "&Database=" + database + "&Request=Nv237z";
                                            string trialcontentType = "application/json";
                                            JObject trialjson = new JObject
                                            {
                                                { "Serial", Constants.deviceID },
                                                { "Date",  DateTime.Parse(current_date)},
                                                { "ContactID",  userName},
                                                { "Version", Constants.appversion }
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

                                                    if (trialmessage.Equals("Inserted"))
                                                    {
                                                        await DisplayAlert("Trial Activated", "You activated trial for 30 days", "Got it");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
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
                                    }
                                    else if (message.Equals("Incorrect Login"))
                                    {
                                        await DisplayAlert("Login Error", "Username or password is incorrect", "Got it");
                                    }
                                    else
                                    {
                                        var result = JsonConvert.DeserializeObject<List<ServerMessage>>(content);
                                        var contactID = result[0].Message;

                                        Preferences.Set("username", userName, "private_prefs");
                                        Preferences.Set("ipaddress", ipaddress, "private_prefs");
                                        Preferences.Set("host", hostName, "private_prefs");
                                        Preferences.Set("database", database, "private_prefs");
                                        Preferences.Set("password", password, "private_prefs");

                                        var logtype = "Mobile Log";
                                        var log = "Logged in (<b>" + userName + "</b>)" + "Version: <b>" + Constants.appversion + "</b> Device ID: <b>" + CrossDeviceInfo.Current.Id + "</b>";
                                        var deleted = "0";
                                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                        string logsurl = "http://" + ipaddress + Constants.requestUrl + "Host=" + hostName + "&Database=" + database + "&Request=pQ412v";
                                        string logscontentType = "application/json";
                                        JObject logsjson = new JObject
                                        {
                                            { "ContactID", contactID },
                                            { "LogType", logtype },
                                            { "Log", log },
                                            { "LogDate", DateTime.Parse(current_datetime) },
                                            { "DatabaseName", database },
                                            { "Deleted", deleted },
                                            { "LastUpdated", DateTime.Parse(current_datetime) }
                                        };

                                        HttpClient logsclient = new HttpClient();
                                        var logsresponse = await logsclient.PostAsync(logsurl, new StringContent(logsjson.ToString(), Encoding.UTF8, logscontentType));

                                        if (logsresponse.IsSuccessStatusCode)
                                        {
                                            var logscontent = await logsresponse.Content.ReadAsStringAsync();
                                            if (!string.IsNullOrEmpty(logscontent))
                                            {
                                                var logsdataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(logscontent, settings);

                                                var logsdataitem = logsdataresult[0];
                                                var logsdatamessage = logsdataitem.Message;

                                                if (logsdatamessage.Equals("Inserted"))
                                                {
                                                    var logs_insert = new UserLogsTable
                                                    {
                                                        ContactID = contactID,
                                                        LogType = logtype,
                                                        Log = log,
                                                        LogDate = DateTime.Parse(current_datetime),
                                                        DatabaseName = database,
                                                        Deleted = Int32.Parse(deleted),
                                                        LastUpdated = DateTime.Parse(current_datetime),
                                                        LastSync = DateTime.Parse(current_datetime)
                                                    };

                                                    await conn.InsertOrReplaceAsync(logs_insert);

                                                    await Application.Current.MainPage.Navigation.PushAsync(new SyncPage(hostName, database, contactID, ipaddress));
                                                }
                                            }
                                        }                                        
                                    }
                                }
                            }
                            else
                            {
                                await DisplayAlert("Login Error", "Error fetching data. Server returned status code: {0} " + response.StatusCode, "Ok");
                            }
                        }
                        else
                        {
                            try
                            {
                                var getUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE UserID = ? AND UsrPassword = ? AND UserStatus='Active'", userName, password);
                                var result = getUser.Result.Count;

                                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                if (result < 1)
                                {
                                    await DisplayAlert("Login Error", "Username or password is incorrect", "Got it");
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
                                Crashes.TrackError(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                else
                {
                    try
                    {
                        var getUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE UserID = ? AND UsrPassword = ? AND UserStatus='Active'", userName, password);
                        var result = getUser.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        if (result < 1)
                        {
                            await DisplayAlert("Login Error", "Username or password is incorrect", "Got it");
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
                //Check if there is an internet connection
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                       Ping ping = new Ping();
                        PingReply pingresult = ping.Send(ipaddress, 200);

                        if (pingresult.Status.ToString() == "Success")
                        {
                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + hostName + "&Database=" + database + "&Request=M8g5E6";
                            var request = HttpWebRequest.Create(string.Format(@link));

                            request.ContentType = "application/json";
                            request.Method = "GET";

                            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                            {
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    await DisplayAlert("Login Error", "Error fetching data. Server returned status code: {0} " + response.StatusCode, "Ok");
                                }
                                else
                                {
                                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                    {
                                        var content = reader.ReadToEnd();

                                        if (!string.IsNullOrEmpty(content))
                                        {
                                            if (content.Equals("[{\"Message\":\"Connected\"}]"))
                                            {
                                                connectstack.IsVisible = false;
                                                loginstack.IsVisible = true;

                                                Preferences.Set("ipaddress", ipaddress, "private_prefs");
                                                Preferences.Set("host", hostName, "private_prefs");
                                                Preferences.Set("database", database, "private_prefs");
                                            }
                                            else if (content.Equals("[{\"Message\":\"Not Connected\"}]"))
                                            {
                                                await DisplayAlert("Connection Error", "Cannot connect to server", "Got it");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Connection Error", "Server unreachable. Switching to offline mode", "Got it");
                            connectstack.IsVisible = false;
                            loginstack.IsVisible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                else
                {
                    await DisplayAlert("Connection Error", "Server unreachable. Switching to offline mode", "Got it");
                    connectstack.IsVisible = false;
                    loginstack.IsVisible = true;
                }
            }
        }
    }
}