using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using TBSMobile.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TBSMobile.Rest_Service
{
    public class RestServices : IRestServices
    {
        HttpClient client;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        string default_datetime = "0001-01-01 00:00:00";
        string current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        string contentType = "application/json";

        public RestServices()
        {
            client = new HttpClient();
        }

        public class ServerMessage
        {
            public string Message { get; set; }
            public string ContactID { get; set; }
        }

        public class UserData
        {
            public string ContactID { get; set; }
            public string UserID { get; set; }
            public string UsrPassword { get; set; }
            public string UserTypeID { get; set; }
            public string UserStatus { get; set; }
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

        public class SubscriptionData
        {
            public string SerialNumber { get; set; }
            public string ContactID { get; set; }
            public DateTime DateStart { get; set; }
            public string NoOfDays { get; set; }
            public string Trials { get; set; }
            public string InputSerialNumber { get; set; }
            public DateTime LastSync { get; set; }
            public DateTime LastUpdated { get; set; }
            public int Deleted { get; set; }
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

        public class ProvinceData
        {
            public string ProvinceID { get; set; }
            public string Province { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class TownData
        {
            public string TownID { get; set; }
            public string ProvinceID { get; set; }
            public string Town { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class UserLogsData
        {
            public string ContactID { get; set; }
            public string LogType { get; set; }
            public string Log { get; set; }
            public DateTime LogDate { get; set; }
            public string database { get; set; }
            public DateTime LastSync { get; set; }
            public DateTime LastUpdated { get; set; }
            public int Deleted { get; set; }
        }

        public class UserEmailData
        {
            public string ContactID { get; set; }
            public string Email { get; set; }
            public string RecordLog { get; set; }
            public DateTime LastSync { get; set; }
            public DateTime LastUpdated { get; set; }
            public int Deleted { get; set; }
            public int Checked { get; set; }
        }

        /* LOGIN REST */

        public async Task CheckVersion(string host, string database, string domain, string apifolder, string apifile, string username, string password, Action<string> LoginStatus)
        {
            LoginStatus("0-Checking version please wait...");
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&Version=" + VersionTracking.CurrentVersion, string.Empty));

                    var response = await client.GetAsync(uri);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var loginresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                            var item = loginresult[0];
                            var message = item.Message;

                            if (!message.Equals(VersionTracking.CurrentVersion))
                            {
                                var answer = await App.Current.MainPage.DisplayAlert("Application Out-Of-Date", "Your application is out-of-date, please download the new version (" + message + ") to continue.", "Download And Install", "Download");
                                if (answer)
                                {
                                    Device.OpenUri(new Uri("https://install.appcenter.ms/users/lawrenceagulto.317-gmail.com/apps/scratch-it/distribution_groups/public%20access"));
                                }
                                else
                                {
                                    Device.OpenUri(new Uri("https://install.appcenter.ms/users/lawrenceagulto.317-gmail.com/apps/scratch-it/distribution_groups/public%20access"));
                                }

                                LoginStatus("1-Login");
                            }
                            else
                            {
                                await Login(host, database, domain, apifolder, username, password, LoginStatus);
                            }
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Checking Version Error", "Checking version failed.\n\nDo you want to retry? \n\nError:\n\nThe server returned a null value", "Yes", "Go Offline Mode");

                            if (retry)
                            {
                                await CheckVersion(host, database, domain, apifolder, apifile, username, password, LoginStatus);
                            }
                            else
                            {
                                LoginStatus("1-Login");
                                await Offline_Login(host, database, domain, apifolder, username, password);
                            }
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Checking Version Error", "Checking version failed.\n\nDo you want to retry? \n\nStatus Code:\n\n" + response.StatusCode, "Yes", "Go Offline Mode");

                        if (retry)
                        {
                            await CheckVersion(host, database, domain, apifolder, apifile, username, password, LoginStatus);
                        }
                        else
                        {
                            LoginStatus("1-Login");
                            await Offline_Login(host, database, domain, apifolder, username, password);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Checking Version Error", "Checking version failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "Go Offline Mode");

                    if (retry)
                    {
                        await CheckVersion(host, database, domain, apifolder, apifile, username, password, LoginStatus);
                    }
                    else
                    {
                        LoginStatus("1-Login");
                        await Offline_Login(host, database, domain, apifolder, username, password);
                    }
                }
            }
            else
            {
                LoginStatus("1-Login");
                await Offline_Login(host, database, domain, apifolder, username, password);
            }
        }

        public async Task Login(string host, string database, string domain, string apifolder, string username, string password, Action<string> LoginStatus)
        {
            LoginStatus("0-Checking login credentials please wait...");
            string login_apifile = "login-api.php";
            
            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + login_apifile + "?Host=" + host + "&Database=" + database + "&Username=" + username + "&Password=" + password + "&RegistrationCode=" + Constants.deviceID, string.Empty));

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                    {
                        var loginresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                        var item = loginresult[0];
                        var message = item.Message;

                        if (message.Equals("Subscription Expired"))
                        {
                            await App.Current.MainPage.DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");
                            LoginStatus("1-Login");

                            var subject = "Subscription Expired: " + username + " - " + Constants.deviceID;
                            var body = "Good Day!<br/><br/> " +
                                "This user needs new product key.<br/><br/>" +
                                "username: " + username + "<br/>" +
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
                            await App.Current.MainPage.DisplayAlert("Trial Subscription Error", "Your trial subscription has been expired, please contact your administrator to register your device", "Send Email");
                            LoginStatus("1-Login");

                            var subject = "Subscription Expired: " + username + " - " + Constants.deviceID;
                            var body = "Good Day!<br/><br/> " +
                                "This user needs new product key.<br/><br/>" +
                                "username: " + username + "<br/>" +
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
                            var trialsub = await App.Current.MainPage.DisplayAlert("Subscription Not Found", "Your device is not registered, please contact your administrator to register your device", "Activate Trial", "Send Email");

                            if (trialsub == true)
                            {
                                await Activate_Trial(host, database, domain, apifolder, username, password, LoginStatus);
                            }
                            else
                            {
                                LoginStatus("1-Login");

                                var subject = "Register Device: " + username + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs to register the device.<br/><br/>" +
                                    "username: " + username + "<br/>" +
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
                            await App.Current.MainPage.DisplayAlert("Login Error", "username or password is incorrect", "Ok");
                            LoginStatus("1-Login");
                        }
                        else if (message.Equals("Credential Correct"))
                        {
                            LoginStatus("1-Login");

                            var result = JsonConvert.DeserializeObject<List<ServerMessage>>(content);

                            var contactID = result[0].ContactID;

                            var logType = "App Log";
                            var log = "Logged in (<b>" + username + "</b>) <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int deleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contactID, logType, log, DateTime.Parse(current_datetime), database, deleted, DateTime.Parse(current_datetime));

                            Save_Preferences(username, password, contactID);

                            await Application.Current.MainPage.Navigation.PushAsync(new SyncPage());
                        }
                        else if (message.Equals("Not Connected"))
                        {
                            LoginStatus("1-Login");
                            await App.Current.MainPage.DisplayAlert("Login Error", "Please check server and database name", "Ok");
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Application Error", "Login failed.\n\nError:\n\n" + content + "\n\nDo you want to retry?", "Yes", "No");
                            if (retry)
                            {
                                await Login(host, database, domain, apifolder, username, password, LoginStatus);
                            }
                            else
                            {
                                LoginStatus("1-Login");
                                await Offline_Login(host, database, domain, apifolder, username, password);
                            }
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Login Error", "Login failed.\n\nError:\n\n The server returned a null value.", "Ok");
                        LoginStatus("1-Login");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Login Error", "Login failed.\n\nStatus Code:\n\n" + response.StatusCode, "Ok");
                    LoginStatus("1-Login");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await App.Current.MainPage.DisplayAlert("Login Error", "Login failed.\n\nError:\n\n" + ex.Message, "Ok");
                LoginStatus("1-Login");
            }
        }

        public async Task Offline_Login(string host, string database, string domain, string apifolder, string username, string password)
        {
            try
            {
                var getUser = Constants.conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE UserID = ? AND UsrPassword = ? AND UserStatus='Active'", username, password);
                var result = getUser.Result.Count;

                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (result < 1)
                {
                    await App.Current.MainPage.DisplayAlert("Login Error", "username or password is incorrect", "Ok");
                }
                else
                {
                    var item = getUser.Result[0];
                    var contactID = item.ContactID;

                    var getSubscription = Constants.conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                    var subresult = getSubscription.Result.Count;

                    //Check if the device is registered
                    if (subresult < 1)
                    {
                        await App.Current.MainPage.DisplayAlert("Subscription Error", "Your device is not registered, please contact your administrator to register your device", "Send Email");

                        var subject = "Register Device: " + username + " - " + Constants.deviceID;
                        var body = "Good Day!<br/><br/> " +
                            "This user needs to register the device.<br/><br/>" +
                            "username: " + username + "<br/>" +
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

                        if (Trials == "0" || Trials == "0")
                        {
                            var ExpirationDate = startDate.AddDays(30);

                            if (DateTime.Now > ExpirationDate)
                            {
                                await App.Current.MainPage.DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + username + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "username: " + username + "<br/>" +
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

                                await Constants.conn.QueryAsync<SubscriptionTable>("DELETE FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            }
                            else
                            {
                                Preferences.Set("username", username, "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                        else if (Trials == "3")
                        {
                            var ExpirationDate = startDate.AddYears(1);

                            if (DateTime.Now > ExpirationDate)
                            {
                                await App.Current.MainPage.DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + username + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "username: " + username + "<br/>" +
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

                                await Constants.conn.QueryAsync<SubscriptionTable>("DELETE FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            }
                            else
                            {
                                Preferences.Set("username", username, "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                        else if (Trials == "4")
                        {
                            var ExpirationDate = startDate.AddYears(2);

                            if (DateTime.Now > ExpirationDate)
                            {
                                await App.Current.MainPage.DisplayAlert("Subscription Error", "Your subscription has been expired, please contact your administrator to register your device", "Send Email");

                                var subject = "Subscription Expired: " + username + " - " + Constants.deviceID;
                                var body = "Good Day!<br/><br/> " +
                                    "This user needs new product key.<br/><br/>" +
                                    "username: " + username + "<br/>" +
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

                                await Constants.conn.QueryAsync<SubscriptionTable>("DELETE FROM tblSubscription WHERE SerialNumber = ? AND ContactID = ?", Constants.deviceID, contactID);
                            }
                            else
                            {
                                Preferences.Set("username", username, "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                        else
                        {
                            var logtype = "Mobile Log";
                            var log = "Logged in (<b>" + username + "</b>)" + "App Version: <b>" + Constants.appversion + "</b> Device ID: <b>" + CrossDeviceInfo.Current.Id + "</b>";
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

                            await Constants.conn.InsertOrReplaceAsync(logs_insert);

                            Preferences.Set("username", username, "private_prefs");
                            Preferences.Set("domain", domain, "private_prefs");
                            Preferences.Set("host", host, "private_prefs");
                            Preferences.Set("database", database, "private_prefs");
                            Preferences.Set("password", password, "private_prefs");
                            Preferences.Set("contact", contactID, "private_prefs");

                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                        }
                    }
                }                
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                await App.Current.MainPage.DisplayAlert("Login Error", "Login failed.\n\nError:\n\n" + ex.Message, "Ok");
            }
        }

        public async Task Activate_Trial(string host, string database, string domain, string apifolder, string username, string password, Action<string> LoginStatus)
        {
            LoginStatus("0-Activating trial please wait...");
            string trialapifile = "activate-trial-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + trialapifile + "?Host=" + host + "&Database=" + database + "&Username=" + username + "&RegistrationCode=" + Constants.deviceID, string.Empty));

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                    {
                        var trialresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                        var trialitem = trialresult[0];
                        var trialmessage = trialitem.Message;
                        var trialcontactid = trialitem.ContactID;

                        if (trialmessage.Equals("Inserted"))
                        {
                            var logType = "App Log";
                            var log = "Activated Trial (<b>" + username + "</b>) <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int deleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", trialcontactid, logType, log, DateTime.Parse(current_datetime), database, deleted, DateTime.Parse(current_datetime));

                            LoginStatus("1-Login");
                            await App.Current.MainPage.DisplayAlert("Trial Activated", "You activated trial for 30 days", "Ok");
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Application Error", "Activating trial failed.\n\nError:\n\n" + content + "\n\nDo you want to retry?", "Yes", "No");
                            if (retry)
                            {
                                await Login(host, database, domain, apifolder, username, password, LoginStatus);
                            }
                            else
                            {
                                LoginStatus("1-Login");
                                await Offline_Login(host, database, domain, apifolder, username, password);
                            }
                        }
                    }
                    else
                    {
                        LoginStatus("1-Login");
                        await App.Current.MainPage.DisplayAlert("Activate Trial Error", "Activate trial failed.\n\nError:\n\n The server returned a null value.", "Ok");
                    }
                }
                else
                {
                    LoginStatus("1-Login");
                    await App.Current.MainPage.DisplayAlert("Activate Trial Error", "Activate trial failed.\n\nStatus Code:\n\n" + response.StatusCode, "Ok");
                }
            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
                await App.Current.MainPage.DisplayAlert("Activate Trial Error", "Activate trial failed.\n\nError:\n\n" + ex.Message, "Ok");
            }
        }

        public void Save_Preferences(string username, string password, string contactID)
        {
            try
            {
                Preferences.Set("username", username, "private_prefs");
                Preferences.Set("password", password, "private_prefs");

                Preferences.Set("contactid", contactID, "private_prefs");
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        /* SYNC REST */

           /* FIRST-TIME SYNC REST */

        public async Task FirstTimeSyncUser(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus)
        {
            SyncStatus("Initiating first-time user sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-user-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting user data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<UserData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving user data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var userid = item.UserID;
                                var usrpassword = item.UsrPassword;
                                var usertypeid = item.UserTypeID;
                                var userstatus = item.UserStatus;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new UserTable
                                {
                                    UserID = userid,
                                    UsrPassword = usrpassword,
                                    ContactID = contact,
                                    UserTypeID = usertypeid,
                                    UserStatus = userstatus,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>User</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("userchangeslastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncSystemSerial(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncSystemSerial(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time User Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncUser(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time User Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncUser(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time User Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncUser(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncSystemSerial(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time system serial sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-system-serial-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact + "&RegistrationCode=" + Constants.deviceID, string.Empty));

                    SyncStatus("Getting system serial data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<SubscriptionTable>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving system serial data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var serialNumber = item.SerialNumber;
                                var contactid = item.ContactID;
                                var dateStart = item.DateStart;
                                var days = item.NoOfDays;
                                var trials = item.Trials;
                                var inputSerial = item.InputSerialNumber;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new SubscriptionTable
                                {
                                    SerialNumber = serialNumber,
                                    ContactID = contactid,
                                    DateStart = dateStart,
                                    NoOfDays = days,
                                    Trials = trials,
                                    InputSerialNumber = inputSerial,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>System Serial</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("systemserialchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncContacts(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncContacts(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time System Serial Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncSystemSerial(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time System Serial Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncSystemSerial(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time System Serial Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncSystemSerial(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncContacts(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time retailer sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-contacts-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting retailer data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving retailer data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var contactID = item.ContactID;
                                var fileAs = item.FileAs;
                                var firstName = item.FirstName;
                                var middleName = item.MiddleName;
                                var lastName = item.LastName;
                                var position = item.Position;
                                var company = item.Company;
                                var companyID = item.CompanyID;
                                var retailerType = item.RetailerType;
                                var presStreet = item.PresStreet;
                                var presBarangay = item.PresBarangay;
                                var presDistrict = item.PresDistrict;
                                var presTown = item.PresTown;
                                var presProvince = item.PresProvince;
                                var presCountry = item.PresCountry;
                                var landmark = item.Landmark;
                                var remarks = item.CustomerRemarks;
                                var recordDate = item.RecordDate;
                                var startTime = item.StartTime;
                                var endTime = item.EndTime;
                                var telephone1 = item.Telephone1;
                                var telephone2 = item.Telephone2;
                                var mobile = item.Mobile;
                                var photo1 = item.Photo1;
                                var photo2 = item.Photo2;
                                var photo3 = item.Photo3;
                                var video = item.Video;
                                var mobilePhoto1 = item.MobilePhoto1;
                                var mobilePhoto2 = item.MobilePhoto2;
                                var mobilePhoto3 = item.MobilePhoto3;
                                var mobileVideo = item.MobileVideo;
                                var email = item.Email;
                                var employee = item.Employee;
                                var customer = item.Customer;
                                var recordLog = item.RecordLog;
                                var Supervisor = item.Supervisor;
                                var lastSync = DateTime.Parse(current_datetime);
                                var lastUpdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new ContactsTable
                                {
                                    ContactID = contactID,
                                    FileAs = fileAs,
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    Position = position,
                                    Company = company,
                                    CompanyID = companyID,
                                    RetailerType = retailerType,
                                    PresStreet = presStreet,
                                    PresBarangay = presBarangay,
                                    PresDistrict = presDistrict,
                                    PresTown = presTown,
                                    PresProvince = presProvince,
                                    PresCountry = presCountry,
                                    Landmark = landmark,
                                    CustomerRemarks = remarks,
                                    RecordDate = recordDate,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Telephone1 = telephone1,
                                    Telephone2 = telephone2,
                                    Mobile = mobile,
                                    Email = email,
                                    Photo1 = photo1,
                                    Photo2 = photo2,
                                    Photo3 = photo3,
                                    Video = video,
                                    MobilePhoto1 = mobilePhoto1,
                                    MobilePhoto2 = mobilePhoto2,
                                    MobilePhoto3 = mobilePhoto3,
                                    MobileVideo = mobileVideo,
                                    Employee = employee,
                                    Customer = customer,
                                    Supervisor = Supervisor,
                                    RecordLog = recordLog,
                                    LastSync = lastSync,
                                    Deleted = deleted,
                                    LastUpdated = lastUpdated
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>Contacts</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("contactschangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time Retailer Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncContacts(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Retailer Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncContacts(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Retailer Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncContacts(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncRetailerOutlet(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time retailer outlet sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-retailer-outlet-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting retailer outlet data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving retailer outlet data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var retailerCode = item.RetailerCode;
                                var contactID = item.ContactID;
                                var presStreet = item.PresStreet;
                                var presBarangay = item.PresBarangay;
                                var presDistrict = item.PresDistrict;
                                var presTown = item.PresTown;
                                var presProvince = item.PresProvince;
                                var presCountry = item.PresCountry;
                                var telephone1 = item.Telephone1;
                                var telephone2 = item.Telephone2;
                                var mobile = item.Mobile;
                                var email = item.Email;
                                var landmark = item.Landmark;
                                var gpsCoordinates = item.GPSCoordinates;
                                var Supervisor = item.Supervisor;
                                var RecordLog = item.RecordLog;
                                var lastSync = DateTime.Parse(current_datetime);
                                var lastUpdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new RetailerGroupTable
                                {
                                    RetailerCode = retailerCode,
                                    ContactID = contactID,
                                    PresStreet = presStreet,
                                    PresBarangay = presBarangay,
                                    PresDistrict = presDistrict,
                                    PresTown = presTown,
                                    PresProvince = presProvince,
                                    PresCountry = presCountry,
                                    Telephone1 = telephone1,
                                    Telephone2 = telephone2,
                                    Mobile = mobile,
                                    Email = email,
                                    Landmark = landmark,
                                    GPSCoordinates = gpsCoordinates,
                                    Supervisor = Supervisor,
                                    RecordLog = RecordLog,
                                    LastSync = lastSync,
                                    Deleted = deleted,
                                    LastUpdated = lastUpdated
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>Retailer Outlet</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("retaileroutletchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Retailer Outlet Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncCAF(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time coordinator activity form sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-caf-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting coordinator activity form data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving coordinator activity form data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;
                                var employeeID = item.EmployeeID;
                                var cafDate = item.CAFDate;
                                var customerID = item.CustomerID;
                                var startTime = item.StartTime;
                                var endTime = item.EndTime;
                                var photo1 = item.Photo1;
                                var photo2 = item.Photo2;
                                var photo3 = item.Photo3;
                                var video = item.Video;
                                var mobilePhoto1 = item.MobilePhoto1;
                                var mobilePhoto2 = item.MobilePhoto2;
                                var mobilePhoto3 = item.MobilePhoto3;
                                var mobileVideo = item.MobileVideo;
                                var gpsCoordinates = item.GPSCoordinates;
                                var remarks = item.Remarks;
                                var otherConcern = item.OtherConcern;
                                var recordLog = item.RecordLog;
                                var lastSync = DateTime.Parse(current_datetime);
                                var lastUpdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var inserdata = new CAFTable
                                {
                                    CAFNo = cafNo,
                                    EmployeeID = employeeID,
                                    CAFDate = cafDate,
                                    CustomerID = customerID,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Photo1 = photo1,
                                    Photo2 = photo2,
                                    Photo3 = photo3,
                                    Video = video,
                                    MobilePhoto1 = mobilePhoto1,
                                    MobilePhoto2 = mobilePhoto2,
                                    MobilePhoto3 = mobilePhoto3,
                                    MobileVideo = mobileVideo,
                                    GPSCoordinates = gpsCoordinates,
                                    Remarks = remarks,
                                    OtherConcern = otherConcern,
                                    RecordLog = recordLog,
                                    LastSync = lastSync,
                                    Deleted = deleted,
                                    LastUpdated = lastUpdated
                                };

                                await Constants.conn.InsertOrReplaceAsync(inserdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>CAF</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("cafchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time CAF Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time CAF Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time CAF Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncCAFActivity(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time coordinator activity sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-caf-activity-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        SyncStatus("Getting coordinator activity data from server");

                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ActivityTable>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving coordinator activity data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;
                                var activityID = item.ActivityID;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new ActivityTable
                                {
                                    CAFNo = cafNo,
                                    ActivityID = activityID,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>CAF Activity</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("cafactivitychangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncEmailRecipient(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncEmailRecipient(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time CAF Activity Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncEmailRecipient(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time email recipient sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-email-recipient-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting email recipient data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<UserEmailTable>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving email recipient data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var contactsID = item.ContactID;
                                var email = item.Email;
                                var recordLog = item.RecordLog;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new UserEmailTable
                                {
                                    ContactID = contactsID,
                                    Email = email,
                                    RecordLog = recordLog,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>Email Recipient</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("emailrecipientchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncProvince(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncProvince(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time Email Recipient Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncEmailRecipient(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Email Recipient Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncEmailRecipient(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Email Recipient Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncEmailRecipient(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncProvince(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time province Sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-province-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                    SyncStatus("Getting province data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving province data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var provinceID = item.ProvinceID;
                                var province = item.Province;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new ProvinceTable
                                {
                                    ProvinceID = provinceID,
                                    Province = province,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>Province</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("provincechangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.FirstTimeSyncTown(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.FirstTimeSyncTown(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time Province Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncProvince(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Province Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncProvince(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Province Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncProvince(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task FirstTimeSyncTown(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating first-time town sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "first-time-sync-town-api.php";
                int count = 0;

                try
                {
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                    SyncStatus("Getting town data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<TownData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving town data to local database\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var townID = item.TownID;
                                var provinceID = item.ProvinceID;
                                var town = item.Town;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new TownTable
                                {
                                    TownID = townID,
                                    ProvinceID = provinceID,
                                    Town = town,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Initialized first-time sync (<b>Town</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("townchangelastcheck", current_datetime, "private_prefs");
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");

                            await App.TodoManager.SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("First-time Town Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await FirstTimeSyncTown(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "1", "private_prefs");
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Town Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncTown(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("First-time Town Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await FirstTimeSyncTown(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "1", "private_prefs");
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

           /* CLIENT UPDATE SYNC REST */

        public async Task SyncUserClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update user sync");
            SyncStatus("Checking connection to server");
            
            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-user-client-update-api.php";

                SyncStatus("Checking user data from local database");

                var datachanges = Constants.conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending user changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");
                        
                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var userid = result.UserID;
                            var usrpassword = result.UsrPassword;
                            var usertypeid = result.UserTypeID;
                            var userstatus = result.UserStatus;
                            var lastsync = DateTime.Parse(current_datetime);
                            var lastupdated = result.LastUpdated;
                            var deleted = result.Deleted;

                            JObject json = new JObject
                            {
                                { "UserID", userid },
                                { "UsrPassword", usrpassword },
                                { "ContactID", contact },
                                { "UserTypeID", usertypeid },
                                { "UserStatus", userstatus },
                                { "LastUpdated", lastupdated },
                                { "Deleted", deleted }
                            };

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<UserTable>("UPDATE tblUsers SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update User Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncUserClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update User Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncUserClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update User Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncUserClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>User</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.UpdateContacts(contact);
                    await App.TodoManager.SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.UpdateContacts(contact);
                        await App.TodoManager.SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update User Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncUserClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task UpdateContacts(string contact)
        {
            try
            {
                await Constants.conn.QueryAsync<ContactsTable>("Update tblContacts SET ThisSynced = ?, Media1Synced = ?, Media2Synced = ?, Media3Synced = ?, Media4Synced = ?  WHERE Supervisor = ? AND Deleted != '1'", 1, 1, 1, 1, 1, contact);
                await Constants.conn.QueryAsync<ContactsTable>("Update tblContacts SET ThisSynced = ?, Media1Synced = ?, Media2Synced = ?, Media3Synced = ?, Media4Synced = ?  WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", 0, 0, 0, 0, 0, contact);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task SyncContactsClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update retailer sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-contacts-client-update-api.php";

                SyncStatus("Checking retailer data from local database");

                var datachanges = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ThisSynced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending retailer changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

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

                            JObject json = new JObject
                            {
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

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET ThisSynced = ? WHERE ContactID = ?", 1, contactID);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Contacts</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncContactsMedia1ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update retailer photo 1 sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-contact-media-path-1-client-update-api.php";

                SyncStatus("Checking retailer photo 1 data from local database");

                var datachanges = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Media1Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending retailer photo 1 changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var contactID = result.ContactID;
                            var mobilePhoto1 = result.MobilePhoto1;

                            JObject path1json;
                            bool path1doesExist = File.Exists(mobilePhoto1);

                            if (!path1doesExist || string.IsNullOrEmpty(mobilePhoto1))
                            {
                                path1json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path1json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", File.ReadAllBytes(mobilePhoto1)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path1json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Media1Synced = ? WHERE ContactID = ?", 1, contactID);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 1 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 1 Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 1 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Contacts Photo 1</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 1 Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncContactsMedia2ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update retailer photo 2 Sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-contact-media-path-2-client-update-api.php";

                SyncStatus("Checking retailer photo 2 data from local database");

                var datachanges = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Media2Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending retailer photo 2 changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var contactID = result.ContactID;
                            var mobilePhoto2 = result.MobilePhoto2;

                            JObject path2json;
                            bool path2doesExist = File.Exists(mobilePhoto2);

                            if (!path2doesExist || string.IsNullOrEmpty(mobilePhoto2))
                            {
                                path2json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path2json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", File.ReadAllBytes(mobilePhoto2)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path2json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Media2Synced = ? WHERE ContactID = ?", 1, contactID);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 2 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 2 Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 2 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Contacts Photo 2</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 2 Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncContactsMedia3ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update retailer photo 3 sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-contact-media-path-3-client-update-api.php";

                SyncStatus("Checking data from local database");

                var datachanges = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Media3Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending retailer photo 3 changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var contactID = result.ContactID;
                            var mobilePhoto3 = result.MobilePhoto3;

                            JObject path3json;
                            bool path3doesExist = File.Exists(mobilePhoto3);

                            if (!path3doesExist || string.IsNullOrEmpty(mobilePhoto3))
                            {
                                path3json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path3json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", File.ReadAllBytes(mobilePhoto3)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path3json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Media3Synced = ? WHERE ContactID = ?", 1, contactID);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 3 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 3 Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 3 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Contacts Photo 3</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Photo 3 Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncContactsMedia4ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update retailer video sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-contact-media-path-4-client-update-api.php";

                SyncStatus("Checking retaile video data from local database");

                var datachanges = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Media4Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending retailer video changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var contactID = result.ContactID;
                            var mobileVideo = result.MobileVideo;

                            JObject path4json;
                            bool path4doesExist = File.Exists(mobileVideo);

                            if (!path4doesExist || string.IsNullOrEmpty(mobileVideo))
                            {
                                path4json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path4json = new JObject
                                {
                                    { "MediaID", contactID},
                                    { "Path", File.ReadAllBytes(mobileVideo)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path4json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ?, Media4Synced = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), 1,contactID);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Video Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Video Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Contacts Video Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Contacts Video</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Video Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncRetailerOutletClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update retailer outlet Sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-retailer-outlet-client-update-api.php";

                SyncStatus("Checking retailer outlet data from local database");

                var datachanges = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending retailer outlet changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

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

                            JObject json = new JObject
                            {
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

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE RetailerCode = ?", DateTime.Parse(current_datetime), retailerCode);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");
                                
                                if (retry)
                                {
                                    await SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Retailer Outlet</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.UpdateCAF(contact);
                    await App.TodoManager.SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.UpdateCAF(contact);
                        await App.TodoManager.SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Retailer Outlet Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task UpdateCAF(string contact)
        {
            try
            {
                await Constants.conn.QueryAsync<CAFTable>("Update tblCAF SET ThisSynced = ?, Media1Synced = ?, Media2Synced = ?, Media3Synced = ?, Media4Synced = ?  WHERE EmployeeID = ? AND Deleted != '1'", 1, 1, 1, 1, 1, contact);
                await Constants.conn.QueryAsync<CAFTable>("Update tblCAF SET ThisSynced = ?, Media1Synced = ?, Media2Synced = ?, Media3Synced = ?, Media4Synced = ?  WHERE EmployeeID = ? AND LastUpdated > LastSync AND Deleted != '1'", 0, 0, 0, 0, 0, contact);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task SyncCAFClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update coordinator activity form sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-caf-client-update-api.php";

                SyncStatus("Checking coordinator activty form data from local database");

                var datachanges = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE ThisSynced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending coordinator activity form changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

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

                            JObject json = new JObject
                            {
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

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<CAFTable>("UPDATE tblCAF SET ThisSynced = ? WHERE CAFNo = ?", 1, cafNo);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>CAF</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncCAFMedia1ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update coordinator activity form photo 1 sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-caf-media-path-1-client-update-api.php";

                SyncStatus("Checking coordinator activity from photo 1 data from local database");

                var datachanges = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE Media1Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending coordinator activity form photo 1 changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var cafNo = result.CAFNo;
                            var mobilePhoto1 = result.MobilePhoto1;

                            JObject path1json;
                            bool path1doesExist = File.Exists(mobilePhoto1);

                            if (!path1doesExist || string.IsNullOrEmpty(mobilePhoto1))
                            {
                                path1json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path1json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", File.ReadAllBytes(mobilePhoto1)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path1json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<CAFTable>("UPDATE tblCAF SET Media1Synced = ? WHERE CAFNo = ?", 1, cafNo);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 1 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 1 Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 1 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>CAF Photo 1</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    await App.TodoManager.SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
            }
            else
            {
                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 1 Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                if (retry)
                {
                    await SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                }
            }
        }

        public async Task SyncCAFMedia2ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update coordinator activity form photo 2 sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-caf-media-path-2-client-update-api.php";

                SyncStatus("Checking coordinator activity form photo 2 data from local database");

                var datachanges = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE Media2Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending coordinator activity form photo 2 changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");
                        
                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var cafNo = result.CAFNo;
                            var mobilePhoto2 = result.MobilePhoto2;

                            JObject path2json;
                            bool path2doesExist = File.Exists(mobilePhoto2);

                            if (!path2doesExist || string.IsNullOrEmpty(mobilePhoto2))
                            {
                                path2json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path2json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", File.ReadAllBytes(mobilePhoto2)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path2json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<CAFTable>("UPDATE tblCAF SET Media2Synced = ? WHERE CAFNo = ?", 1, cafNo);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 2 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 2 Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 2 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>CAF Photo 2</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 2 Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncCAFMedia3ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update coordinator activity form photo 3 sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-caf-media-path-3-client-update-api.php";

                SyncStatus("Checking coordinator activity form photo 3 data from local database");

                var datachanges = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE Media3Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending coordinator activity form photo 3 changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");
                        
                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var cafNo = result.CAFNo;
                            var mobilePhoto3 = result.MobilePhoto3;

                            JObject path3json;
                            bool path3doesExist = File.Exists(mobilePhoto3);

                            if (!path3doesExist || string.IsNullOrEmpty(mobilePhoto3))
                            {
                                path3json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path3json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", File.ReadAllBytes(mobilePhoto3)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path3json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<CAFTable>("UPDATE tblCAF SET Media3Synced = ? WHERE CAFNo = ?", 1, cafNo);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 3 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 3 Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 3 Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>CAF Photo 3</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Photo 3 Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncCAFMedia4ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update coordinator activity form video Sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-caf-media-path-4-client-update-api.php";

                SyncStatus("Checking coordinator activity form video data from local database");

                var datachanges = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE Media4Synced = '0'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending coordinator activity form video changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var cafNo = result.CAFNo;
                            var mobileVideo = result.MobileVideo;

                            JObject path4json;
                            bool path4doesExist = File.Exists(mobileVideo);

                            if (!path4doesExist || string.IsNullOrEmpty(mobileVideo))
                            {
                                path4json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path4json = new JObject
                                {
                                    { "MediaID", cafNo},
                                    { "Path", File.ReadAllBytes(mobileVideo)}
                                };
                            }

                            var response = await client.PostAsync(uri, new StringContent(path4json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<CAFTable>("UPDATE tblCAF SET LastSync = ?, Media4Synced = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), 1, cafNo);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Video Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Video Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Video Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>CAF Video</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Video Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncCAFActivityClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update coordinator activity sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-caf-activity-client-update-api.php";

                SyncStatus("Checking data from local database");

                var datachanges = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending coordinator activity changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var cafNo = result.CAFNo;
                            var contactid = result.ContactID;
                            var activityID = result.ActivityID;
                            var lastsync = DateTime.Parse(current_datetime);
                            var lastupdated = result.LastUpdated;
                            var deleted = result.Deleted;

                            JObject json = new JObject
                            {
                                { "CAFNo", cafNo },
                                { "ContactID", contactid },
                                { "ActivityID", activityID },
                                { "LastUpdated", lastupdated },
                                { "Deleted", deleted }
                            };

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), cafNo);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>CAF Activity</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update CAF Activity Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncEmailRecipientClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update email recipient sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-email-recipient-client-update-api.php";

                SyncStatus("Checking email recipient data from local database");

                var datachanges = Constants.conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending email recipient changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var contactsID = result.ContactID;
                            var email = result.Email;
                            var recordLog = result.RecordLog;
                            var lastsync = DateTime.Parse(current_datetime);
                            var lastupdated = result.LastUpdated;
                            var deleted = result.Deleted;

                            JObject json = new JObject
                            {
                                { "ContactID", contactsID },
                                { "Email", email },
                                { "RecordLog", recordLog },
                                { "LastUpdated", lastupdated },
                                { "Deleted", deleted }
                            };

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contactsID);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update Email Recipient Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update Email Recipient Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update Email Recipient Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                Preferences.Set("isfirsttimesync", "0", "private_prefs");
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>Email Recipient</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.SyncUserServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.SyncUserServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update Email Recipient Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncUserLogsClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating client update user logs sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-user-logs-client-update-api.php";

                SyncStatus("Checking user logs data from local database");

                var datachanges = Constants.conn.QueryAsync<UserLogsTable>("SELECT * FROM tblUserLogs WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var changesresultCount = datachanges.Result.Count;

                if (changesresultCount > 0)
                {
                    int clientupdate = 1;

                    for (int i = 0; i < changesresultCount; i++)
                    {
                        SyncStatus("Sending user logs changes to server\n (" + clientupdate + " out of " + changesresultCount + ")");

                        try
                        {
                            var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                            var result = datachanges.Result[i];
                            var contactsID = result.ContactID;
                            var logtype = result.LogType;
                            var logs = result.Log;
                            var logDate = result.LogDate;
                            var databasename = result.DatabaseName;
                            var lastsync = DateTime.Parse(current_datetime);
                            var lastupdated = result.LastUpdated;
                            var deleted = result.Deleted;

                            JObject json = new JObject
                            {
                                { "ContactID", contactsID },
                                { "LogType", logtype },
                                { "Log", logs },
                                { "LogDate", logDate },
                                { "LastUpdated", lastupdated },
                                { "Deleted", deleted }
                            };

                            var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                                        await Constants.conn.QueryAsync<UserLogsTable>("UPDATE tblUserLogs SET LastSync = ? WHERE ContactID = ? AND LogType = ? AND Log = ? AND LogDate = ? AND DatabaseName = ?", DateTime.Parse(current_datetime), contactsID, logtype, logs, logDate, database);

                                        clientupdate++;
                                    }
                                    else
                                    {
                                        var retry = await App.Current.MainPage.DisplayAlert("Client Update User Logs Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "No");

                                        if (retry)
                                        {
                                            await SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                        }
                                        else
                                        {
                                            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var retry = await App.Current.MainPage.DisplayAlert("Client Update User Logs Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                                if (retry)
                                {
                                    await SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                                }
                                else
                                {
                                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            var retry = await App.Current.MainPage.DisplayAlert("Client Update User Logs Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                            if (retry)
                            {
                                await SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                            }
                            else
                            {
                                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            }
                        }
                    }

                    var logType = "App Log";
                    var log = "Sent client updates to the server (<b>User Logs</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                    int logdeleted = 0;

                    await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                    await App.TodoManager.OnSyncComplete(host, database, domain, contact);
                }
                else
                {
                    try
                    {
                        await App.TodoManager.OnSyncComplete(host, database, domain, contact);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Client Update User Logs Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

            /* SERVER UPDATE SYNC REST */

        public async Task SyncUserServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating server update user sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-user-server-update-api.php";
                int count = 0;

                try
                {
                    var lastchecked = Preferences.Get("userchangeslastcheck", String.Empty, "private_prefs");
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact + "&LastChecked=" + lastchecked, string.Empty));

                    SyncStatus("Getting user data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<UserData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving user server update to local database\n (" + (count + 1) + " out of " + dataresult.Count + ")");

                                var item = dataresult[i];
                                var userid = item.UserID;
                                var usrpassword = item.UsrPassword;
                                var usertypeid = item.UserTypeID;
                                var userstatus = item.UserStatus;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new UserTable
                                {
                                    UserID = userid,
                                    UsrPassword = usrpassword,
                                    ContactID = contact,
                                    UserTypeID = usertypeid,
                                    UserStatus = userstatus,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Checked server updates (<b>User</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("userchangeslastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.SyncSystemSerialServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncSystemSerialServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Server Update User Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await SyncUserServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update User Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await SyncUserServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update User Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncUserServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncSystemSerialServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating server update system serial sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-system-serial-server-update-api.php";
                int count = 0;

                try
                {
                    var lastchecked = Preferences.Get("systemserialchangelastcheck", String.Empty, "private_prefs");
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact + "&RegistrationCode=" + Constants.deviceID + "&LastChecked=" + lastchecked, string.Empty));

                    SyncStatus("Getting system serial data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<SubscriptionTable>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving system serial server update to local database\n (" + (count + 1) + " out of " + dataresult.Count + ")");

                                var item = dataresult[i];
                                var serialNumber = item.SerialNumber;
                                var contactid = item.ContactID;
                                var dateStart = item.DateStart;
                                var days = item.NoOfDays;
                                var trials = item.Trials;
                                var inputSerial = item.InputSerialNumber;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new SubscriptionTable
                                {
                                    SerialNumber = serialNumber,
                                    ContactID = contactid,
                                    DateStart = dateStart,
                                    NoOfDays = days,
                                    Trials = trials,
                                    InputSerialNumber = inputSerial,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Checked server updates (<b>System Serial</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("systemserialchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.SyncContactsServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncContactsServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Server Update System Serial Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await SyncSystemSerialServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update System Serial Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await SyncSystemSerialServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update System Serial Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncSystemSerialServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncContactsServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating server update retailer sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-contacts-server-update-api.php";
                int count = 0;

                try
                {
                    var lastchecked = Preferences.Get("contactschangelastcheck", String.Empty, "private_prefs");
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact + "&LastChecked=" + lastchecked, string.Empty));

                    SyncStatus("Getting retailer data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving retailer server update to local database\n (" + (count + 1) + " out of " + dataresult.Count + ")");

                                var item = dataresult[i];
                                var contactID = item.ContactID;
                                var fileAs = item.FileAs;
                                var firstName = item.FirstName;
                                var middleName = item.MiddleName;
                                var lastName = item.LastName;
                                var position = item.Position;
                                var company = item.Company;
                                var companyID = item.CompanyID;
                                var retailerType = item.RetailerType;
                                var presStreet = item.PresStreet;
                                var presBarangay = item.PresBarangay;
                                var presDistrict = item.PresDistrict;
                                var presTown = item.PresTown;
                                var presProvince = item.PresProvince;
                                var presCountry = item.PresCountry;
                                var landmark = item.Landmark;
                                var remarks = item.CustomerRemarks;
                                var recordDate = item.RecordDate;
                                var startTime = item.StartTime;
                                var endTime = item.EndTime;
                                var telephone1 = item.Telephone1;
                                var telephone2 = item.Telephone2;
                                var mobile = item.Mobile;
                                var photo1 = item.Photo1;
                                var photo2 = item.Photo2;
                                var photo3 = item.Photo3;
                                var video = item.Video;
                                var mobilePhoto1 = item.MobilePhoto1;
                                var mobilePhoto2 = item.MobilePhoto2;
                                var mobilePhoto3 = item.MobilePhoto3;
                                var mobileVideo = item.MobileVideo;
                                var email = item.Email;
                                var employee = item.Employee;
                                var customer = item.Customer;
                                var recordLog = item.RecordLog;
                                var Supervisor = item.Supervisor;
                                var lastSync = DateTime.Parse(current_datetime);
                                var lastUpdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new ContactsTable
                                {
                                    ContactID = contactID,
                                    FileAs = fileAs,
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    Position = position,
                                    Company = company,
                                    CompanyID = companyID,
                                    RetailerType = retailerType,
                                    PresStreet = presStreet,
                                    PresBarangay = presBarangay,
                                    PresDistrict = presDistrict,
                                    PresTown = presTown,
                                    PresProvince = presProvince,
                                    PresCountry = presCountry,
                                    Landmark = landmark,
                                    CustomerRemarks = remarks,
                                    RecordDate = recordDate,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Telephone1 = telephone1,
                                    Telephone2 = telephone2,
                                    Mobile = mobile,
                                    Email = email,
                                    Photo1 = photo1,
                                    Photo2 = photo2,
                                    Photo3 = photo3,
                                    Video = video,
                                    MobilePhoto1 = mobilePhoto1,
                                    MobilePhoto2 = mobilePhoto2,
                                    MobilePhoto3 = mobilePhoto3,
                                    MobileVideo = mobileVideo,
                                    Employee = employee,
                                    Customer = customer,
                                    Supervisor = Supervisor,
                                    RecordLog = recordLog,
                                    LastSync = lastSync,
                                    Deleted = deleted,
                                    LastUpdated = lastUpdated
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Checked server updates (<b>Contacts</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("contactschangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.SyncRetailerOutletServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncRetailerOutletServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Server Update Contacts Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await SyncContactsServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Contacts Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await SyncContactsServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
            }
            else
            {
                var retry = await App.Current.MainPage.DisplayAlert("Server Update Contacts Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                if (retry)
                {
                    await SyncContactsServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                }
                else
                {
                    Preferences.Set("isfirsttimesync", "0", "private_prefs");
                }
            }
        }

        public async Task SyncRetailerOutletServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating server update retailer outlet sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-retailer-outlet-server-update-api.php";
                int count = 0;

                try
                {
                    var lastchecked = Preferences.Get("retaileroutletchangelastcheck", String.Empty, "private_prefs");
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact + "&LastChecked=" + lastchecked, string.Empty));

                    SyncStatus("Getting retailer outlet data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving retailer outlet server update to local database\n (" + (count + 1) + " out of " + dataresult.Count + ")");

                                var item = dataresult[i];
                                var retailerCode = item.RetailerCode;
                                var contactID = item.ContactID;
                                var presStreet = item.PresStreet;
                                var presBarangay = item.PresBarangay;
                                var presDistrict = item.PresDistrict;
                                var presTown = item.PresTown;
                                var presProvince = item.PresProvince;
                                var presCountry = item.PresCountry;
                                var telephone1 = item.Telephone1;
                                var telephone2 = item.Telephone2;
                                var mobile = item.Mobile;
                                var email = item.Email;
                                var landmark = item.Landmark;
                                var gpsCoordinates = item.GPSCoordinates;
                                var Supervisor = item.Supervisor;
                                var RecordLog = item.RecordLog;
                                var lastSync = DateTime.Parse(current_datetime);
                                var lastUpdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new RetailerGroupTable
                                {
                                    RetailerCode = retailerCode,
                                    ContactID = contactID,
                                    PresStreet = presStreet,
                                    PresBarangay = presBarangay,
                                    PresDistrict = presDistrict,
                                    PresTown = presTown,
                                    PresProvince = presProvince,
                                    PresCountry = presCountry,
                                    Telephone1 = telephone1,
                                    Telephone2 = telephone2,
                                    Mobile = mobile,
                                    Email = email,
                                    Landmark = landmark,
                                    GPSCoordinates = gpsCoordinates,
                                    Supervisor = Supervisor,
                                    RecordLog = RecordLog,
                                    LastSync = lastSync,
                                    Deleted = deleted,
                                    LastUpdated = lastUpdated
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Checked server updates (<b>Retailer Outlet</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("retaileroutletchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.SyncProvinceServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncProvinceServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Server Update Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await SyncRetailerOutletServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await SyncRetailerOutletServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Retailer Outlet Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncRetailerOutletServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncProvinceServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating server update province Sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-province-server-update-api.php";
                int count = 0;

                try
                {
                    var lastchecked = Preferences.Get("provincechangelastcheck", String.Empty, "private_prefs");
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&LastChecked=" + lastchecked, string.Empty));

                    SyncStatus("Getting province data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving province server update to local database\n (" + (count + 1) + " out of " + dataresult.Count + ")");

                                var item = dataresult[i];
                                var provinceID = item.ProvinceID;
                                var province = item.Province;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new ProvinceTable
                                {
                                    ProvinceID = provinceID,
                                    Province = province,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Checked server updates (<b>Province</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("provincechangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.SyncTownServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncTownServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Server Update Province Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await SyncProvinceServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Province Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await SyncProvinceServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Province Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncProvinceServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task SyncTownServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating server update town sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "sync-town-server-update-api.php";
                int count = 0;

                try
                {
                    var lastchecked = Preferences.Get("townchangelastcheck", String.Empty, "private_prefs");
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&LastChecked=" + lastchecked, string.Empty));

                    SyncStatus("Getting town data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<TownData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Saving town server update to local database\n (" + (count + 1) + " out of " + dataresult.Count + ")");

                                var item = dataresult[i];
                                var townID = item.TownID;
                                var provinceID = item.ProvinceID;
                                var town = item.Town;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = item.LastUpdated;
                                var deleted = item.Deleted;

                                var insertdata = new TownTable
                                {
                                    TownID = townID,
                                    ProvinceID = provinceID,
                                    Town = town,
                                    LastSync = lastsync,
                                    LastUpdated = lastupdated,
                                    Deleted = deleted
                                };

                                await Constants.conn.InsertOrReplaceAsync(insertdata);

                                count++;
                            }

                            var logType = "App Log";
                            var log = "Checked server updates (<b>Town</b>)  <br/>" + "App Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            await Constants.conn.QueryAsync<UserLogsTable>("INSERT INTO tblUserLogs (ContactID, LogType, Log, LogDate, DatabaseName, Deleted, LastUpdated) VALUES (?, ?, ?, ?, ?, ?, ?)", contact, logType, log, DateTime.Parse(current_datetime), database, logdeleted, DateTime.Parse(current_datetime));

                            Preferences.Set("townchangelastcheck", current_datetime, "private_prefs");

                            await App.TodoManager.SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            await App.TodoManager.SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Server Update Town Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await SyncTownServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        }
                        else
                        {
                            Preferences.Set("isfirsttimesync", "0", "private_prefs");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Town Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await SyncTownServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Server Update Town Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await SyncTownServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        Preferences.Set("isfirsttimesync", "0", "private_prefs");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

            /* RE-SYNC REST */

        public async Task ReSynContacts(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating retailer re-sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "resync-contacts-api.php";
                int count = 0;

                try
                {
                    await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ? WHERE Supervisor = ?", 0, contact);
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting retailer data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Checking retailer\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var contactID = item.ContactID;

                                await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET Existed = ? WHERE ContactID = ?", 1, contactID);

                                count++;
                            }

                            await Constants.conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Re-sync Contacts Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await ReSynContacts(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync Contacts Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await ReSynContacts(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync Contacts Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await ReSynContacts(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task ReSyncRetailerOutlet(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating retailer outlet re-sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "resync-retailer-outlet-api.php";
                int count = 0;

                try
                {
                    await Constants.conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET Existed = ? WHERE Supervisor = ?", 0, contact);
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting retailer outlet data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Checking retailer outlet\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var retailerCode = item.RetailerCode;

                                await Constants.conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET Existed = ? WHERE RetailerCode = ?", 1, retailerCode);

                                count++;
                            }

                            await Constants.conn.QueryAsync<RetailerGroupData>("UPDATE tblRetailerGroup SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Re-sync Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await ReSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync Retailer Outlet Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await ReSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync Retailer Outlet Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await ReSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task ReSyncCAF(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating coordinator activity form re-sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "resync-caf-api.php";
                int count = 0;

                try
                {
                    await Constants.conn.QueryAsync<CAFTable>("UPDATE tblCaf SET Existed = ? WHERE EmployeeID = ?", 0, contact);
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting coordinator activity form data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Checking coordinator activity form\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;

                                await Constants.conn.QueryAsync<CAFData>("UPDATE tblCaf SET Existed = ? WHERE CAFNo = ?", 1, cafNo);

                                count++;
                            }

                            await Constants.conn.QueryAsync<CAFData>("UPDATE tblCaf SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Re-sync CAF Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await ReSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync CAF Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await ReSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync CAF Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await ReSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        public async Task ReSyncCAFActivity(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus)
        {
            SyncStatus("Initiating coordinator activity re-sync");
            SyncStatus("Checking connection to server");

            if (CrossConnectivity.Current.IsConnected)
            {
                string apifile = "resync-caf-activity-api.php";
                int count = 0;

                try
                {
                    await Constants.conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET Existed = ? WHERE ContactID = ?", 0, contact);
                    var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database + "&ContactID=" + contact, string.Empty));

                    SyncStatus("Getting coordinator activity data from server");

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            var dataresult = JsonConvert.DeserializeObject<List<ActivityTable>>(content, settings);
                            var datacount = dataresult.Count;

                            for (int i = 0; i < datacount; i++)
                            {
                                SyncStatus("Checking coordinator activity\n (" + (count + 1) + " out of " + datacount + ")");

                                var item = dataresult[i];
                                var cafNo = item.CAFNo;
                                var act = item.ActivityID;

                                await Constants.conn.QueryAsync<ActivityData>("UPDATE tblActivity SET Existed = ? WHERE CAFNo = ? AND ActivityID = ?", 1, cafNo, act);

                                count++;
                            }

                            await Constants.conn.QueryAsync<ActivityData>("UPDATE tblActivity SET LastSync = ? WHERE Existed = ?", DateTime.Parse(default_datetime), 0);
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Re-sync CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry?\n\nStatus Code:\n\n" + response.StatusCode, "Yes", "No");

                        if (retry)
                        {
                            await ReSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync CAF Activity Sync Error", "Syncing failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                    if (retry)
                    {
                        await ReSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
            }
            else
            {
                try
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Re-sync CAF Activity Sync Error", "Syncing failed. Please connect to the internet to sync your data. Do you want to retry?", "Yes", "No");

                    if (retry)
                    {
                        await ReSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

            /* CHECK AUTO SYNC FUNCTION  */
        public async Task CheckContactsData(string contact)
        {
            try
            {
                var getcontactschanges = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var contactchangesresultCount = getcontactschanges.Result.Count;

                Preferences.Set("contactschanges", contactchangesresultCount.ToString(), "private_prefs");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Checking Retailer Error", "Checking retailer failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await CheckContactsData(contact);
                }
            }
        }

        public async Task CheckRetailerOutletData( string contact)
        {
            try
            {
                var getretaileroutletchanges = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var retaileroutletchangesresultCount = getretaileroutletchanges.Result.Count;

                Preferences.Set("retaileroutletchanges", retaileroutletchangesresultCount.ToString(), "private_prefs");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Checking Retailer Outlet Error", "Checking retailer outlet failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await CheckRetailerOutletData(contact);
                }
            }
        }

        public async Task CheckCAFData(string contact)
        {
            try
            {
                var getcafchanges = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var cafchangesresultCount = getcafchanges.Result.Count;

                Preferences.Set("cafchanges", cafchangesresultCount.ToString(), "private_prefs");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Checking CAF Error", "Checking caf failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await CheckCAFData(contact);
                }
            }
        }

        public async Task CheckCAFActivityData(string contact)
        {
            try
            {
                var getactchanges = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var actchangesresultCount = getactchanges.Result.Count;

                Preferences.Set("cafactivitychanges", actchangesresultCount.ToString(), "private_prefs");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Checking CAF Activity Error", "Checking caf activity failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await CheckCAFActivityData(contact);
                }
            }
        }

        public async Task CheckEmailRecipientData(string contact)
        {            
            try
            {
                var getemailchanges = Constants.conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                var emailchangesresultCount = getemailchanges.Result.Count;

                Preferences.Set("emailrecipientchanges", emailchangesresultCount.ToString(), "private_prefs");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Checking Email Recipient Activity Error", "Checking email recipient failed.\n\nDo you want to retry? \n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await CheckEmailRecipientData(contact);
                }
            }
        }

        public async Task CheckAutoSync(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus)
        {
            try
            {
                var contactschanges = Preferences.Get("contactschanges", String.Empty, "private_prefs");
                var retaileroutletchanges = Preferences.Get("retaileroutletchanges", String.Empty, "private_prefs");
                var cafchanges = Preferences.Get("cafchanges", String.Empty, "private_prefs");
                var cafactivitychanges = Preferences.Get("cafactivitychanges", String.Empty, "private_prefs");
                var emailrecipientchanges = Preferences.Get("emailrecipientchanges", String.Empty, "private_prefs");

                if (Convert.ToInt32(contactschanges) > 0 || Convert.ToInt32(retaileroutletchanges) > 0 || Convert.ToInt32(cafchanges) > 0 || Convert.ToInt32(cafactivitychanges) > 0 || Convert.ToInt32(emailrecipientchanges) > 0)
                {
                    var autosync = await App.Current.MainPage.DisplayAlert("Auto-sync Notification", "Do you want to sync the data?", "Yes", "No");

                    if (autosync == true)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new SyncPage());
                    }
                }
                else
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        SyncStatus("Online - Connected to server");
                    }
                    else
                    {
                        SyncStatus("Offline - Connect to internet");
                    }
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        /* DIRECTLY SEND TO SERVER REST */

            /* DIRECT SEND CAF TO SERVER REST */

        public async Task SendCAFDirectly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Sending coordinator activity form to server");

            string apifile = "sync-caf-directly-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject json = new JObject
                {
                    { "CAFNo", caf },
                    { "CustomerID", retailercode },
                    { "EmployeeNumber", employeenumber },
                    { "Street", street },
                    { "Barangay", barangay },
                    { "Town", town },
                    { "District", district },
                    { "Province", province },
                    { "Country", country },
                    { "Landmark", landmark },
                    { "Telephone1", telephone1 },
                    { "Telephone2", telephone2 },
                    { "Mobile", mobile },
                    { "Email", email },
                    { "Location", location },
                    { "Date", DateTime.Parse(date) },
                    { "StartTime", DateTime.Parse(starttime) },
                    { "EndTime", DateTime.Parse(endtime) },
                    { "MobilePhoto1", photo1url },
                    { "MobilePhoto2", photo2url },
                    { "MobilePhoto3", photo3url },
                    { "MobileVideo", videourl },
                    { "GPSCoordinates", actlocation },
                    { "Rekorida", rekorida },
                    { "Merchandizing", merchandizing },
                    { "TradeCheck", tradecheck },
                    { "Others", others },
                    { "OtherConcern", otherconcern },
                    { "Remarks", remarks },
                    { "RecordLog", recordlog },
                    { "LastUpdated", DateTime.Parse(current_datetime) }
                };

                var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                            await SaveRetailerOutletToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                            await SaveCAFActivityToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                            await App.TodoManager.SendCAFMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendCAFDirectly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                            else
                            {
                                await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                                await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                                await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);

                            }
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Error", "Sending failed.\n\nError:\n\n" + content + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                        if (retry)
                        {
                            await SendCAFDirectly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                        }
                        else
                        {
                            await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                            await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                            await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendCAFDirectly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                    else
                    {
                        await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                        await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                        await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendCAFDirectly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                }
                else
                {
                    await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                    await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        public async Task SendCAFMedia1Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Sending coordinator activity form photo 1 to server");

            string apifile = "sync-caf-media-path-1-client-update-api.php";
            
            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(photo1url);

                if (!pathdoesExist || string.IsNullOrEmpty(photo1url))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", File.ReadAllBytes(photo1url)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await App.TodoManager.SendCAFMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 1 Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendCAFMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                            else
                            {
                                await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                                await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                                await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 1 Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendCAFMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                    else
                    {
                        await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                        await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                        await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 1 Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendCAFMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                }
                else
                {
                    await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                    await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        public async Task SendCAFMedia2Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Sending coordinator activity form photo 2 to server");

            string apifile = "sync-caf-media-path-2-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(photo2url);

                if (!pathdoesExist || string.IsNullOrEmpty(photo2url))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", File.ReadAllBytes(photo2url)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await App.TodoManager.SendCAFMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 2 Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendCAFMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                            else
                            {
                                await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                                await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                                await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 2 Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendCAFMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                    else
                    {
                        await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                        await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                        await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 2 Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendCAFMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                }
                else
                {
                    await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                    await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        public async Task SendCAFMedia3Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Sending coordinator activity form photo 3 to server");

            string apifile = "sync-caf-media-path-3-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(photo3url);

                if (!pathdoesExist || string.IsNullOrEmpty(photo3url))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", File.ReadAllBytes(photo3url)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await App.TodoManager.SendCAFMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 3 Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendCAFMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                            else
                            {
                                await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                                await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                                await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 3 Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendCAFMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                    else
                    {
                        await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                        await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                        await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Photo 3 Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendCAFMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                }
                else
                {
                    await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                    await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        public async Task SendCAFMedia4Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Sending coordinator activity form video to server");

            string apifile = "sync-caf-media-path-4-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(videourl);

                if (!pathdoesExist || string.IsNullOrEmpty(videourl))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", caf},
                        { "Path", File.ReadAllBytes(videourl)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await SaveCAFToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Video Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendCAFMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                            else
                            {
                                await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                                await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                                await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Video Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendCAFMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                    else
                    {
                        await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                        await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                        await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending CAF Video Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendCAFMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                }
                else
                {
                    await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                    await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        public async Task SaveCAFToLocalDatabaseSuccess(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog)
        {
            SyncStatus("Saving coordinator activity form to local database");
            
            try
            {
                var caf_insert = new CAFTable
                {
                    CAFNo = caf,
                    EmployeeID = employeenumber,
                    CAFDate = DateTime.Parse(date),
                    CustomerID = retailercode,
                    StartTime = DateTime.Parse(starttime),
                    EndTime = DateTime.Parse(endtime),
                    Photo1 = photo1url,
                    Photo2 = photo2url,
                    Photo3 = photo3url,
                    Video = videourl,
                    MobilePhoto1 = photo1url,
                    MobilePhoto2 = photo2url,
                    MobilePhoto3 = photo3url,
                    MobileVideo = videourl,
                    GPSCoordinates = actlocation,
                    Remarks = remarks,
                    OtherConcern = otherconcern,
                    RecordLog = recordlog,
                    LastSync = DateTime.Parse(current_datetime),
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await Constants.conn.InsertOrReplaceAsync(caf_insert);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving CAF Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveCAFToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                }
            }
        }

        public async Task SaveRetailerOutletToLocalDatabaseSuccess(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string recordlog)
        {
            SyncStatus("Saving retailer outlet update to local database");
            
            try
            {
                await Constants.conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET PresStreet = ?, PresBarangay = ?, PresTown = ?, PresProvince = ?, PresCountry = ?, PresDistrict= ?, Landmark = ?, Telephone1 = ?, Telephone2 = ?, Mobile = ?, Email = ?, GPSCoordinates = ?, RecordLog = ?, LastUpdated = ?, LastSync = ? WHERE RetailerCode = ?", street, barangay, town, province, country, district, landmark, telephone1, telephone2, mobile, email, location, recordlog, DateTime.Parse(current_datetime), DateTime.Parse(current_datetime), retailercode);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving CAF Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveRetailerOutletToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                }
                else
                {
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                }
            }
        }

        public async Task SaveCAFActivityToLocalDatabaseSuccess(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string employeenumber, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Saving coordinator activity to local database");
            
            try
            {
                if (!String.IsNullOrEmpty(rekorida))
                {

                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00001'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var rekorida_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00001",
                            RecordLog = recordlog,
                            LastSync = DateTime.Parse(current_datetime),
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(rekorida_insert);
                    }
                }

                if (!String.IsNullOrEmpty(merchandizing))
                {
                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00002'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var merchandizing_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00002",
                            RecordLog = recordlog,
                            LastSync = DateTime.Parse(current_datetime),
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(merchandizing_insert);
                    }
                }

                if (!String.IsNullOrEmpty(tradecheck))
                {
                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00003'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var trade_check_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00003",
                            RecordLog = recordlog,
                            LastSync = DateTime.Parse(current_datetime),
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(trade_check_insert);
                    }
                }

                if (!String.IsNullOrEmpty(others))
                {
                    var getCount = Constants.conn.QueryAsync<UserTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00004'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var others_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00004",
                            RecordLog = recordlog,
                            LastSync = DateTime.Parse(current_datetime),
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(others_insert);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving CAF Activity Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveCAFActivityToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        public async Task SaveCAFToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog)
        {
            SyncStatus("Saving coordinator activity form to local database");
            
            try
            {
                var caf_insert = new CAFTable
                {
                    CAFNo = caf,
                    EmployeeID = employeenumber,
                    CAFDate = DateTime.Parse(date),
                    CustomerID = retailercode,
                    StartTime = DateTime.Parse(starttime),
                    EndTime = DateTime.Parse(endtime),
                    Photo1 = photo1url,
                    Photo2 = photo2url,
                    Photo3 = photo3url,
                    Video = videourl,
                    MobilePhoto1 = photo1url,
                    MobilePhoto2 = photo2url,
                    MobilePhoto3 = photo3url,
                    MobileVideo = videourl,
                    GPSCoordinates = actlocation,
                    Remarks = remarks,
                    OtherConcern = otherconcern,
                    RecordLog = recordlog,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await Constants.conn.InsertOrReplaceAsync(caf_insert);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving CAF Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveCAFToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                }
            }
        }

        public async Task SaveRetailerOutletToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string recordlog)
        {
            SyncStatus("Saving retailer outlet update to local database");

            try
            {
                await Constants.conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET PresStreet = ?, PresBarangay = ?, PresTown = ?, PresProvince = ?, PresCountry = ?, PresDistrict= ?, Landmark = ?, Telephone1 = ?, Telephone2 = ?, Mobile = ?, Email = ?, GPSCoordinates = ?, RecordLog = ?, LastUpdated = ? WHERE RetailerCode = ?", street, barangay, town, province, country, district, landmark, telephone1, telephone2, mobile, email, location, recordlog, DateTime.Parse(current_datetime), retailercode);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving CAF Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                }
            }
        }

        public async Task SaveCAFActivityToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string employeenumber, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            SyncStatus("Saving coordinator activity to local database");

            try
            {
                if (!String.IsNullOrEmpty(rekorida))
                {

                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00001'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if(result == 0)
                    {
                        var rekorida_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00001",
                            RecordLog = recordlog,
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(rekorida_insert);
                    }                   
                }

                if (!String.IsNullOrEmpty(merchandizing))
                {
                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00002'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var merchandizing_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00002",
                            RecordLog = recordlog,
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(merchandizing_insert);
                    }
                }

                if (!String.IsNullOrEmpty(tradecheck))
                {
                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00003'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var trade_check_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00003",
                            RecordLog = recordlog,
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(trade_check_insert);
                    }
                }

                if (!String.IsNullOrEmpty(others))
                {
                    var getCount = Constants.conn.QueryAsync<ActivityTable>("SELECT CAFNo FROM tblActivity WHERE CAFNo = ? AND ContactID = ? AND ActivityID='ACT00004'", caf, employeenumber);
                    var result = getCount.Result.Count;

                    if (result == 0)
                    {
                        var others_insert = new ActivityTable
                        {
                            CAFNo = caf,
                            ContactID = employeenumber,
                            ActivityID = "ACT00004",
                            RecordLog = recordlog,
                            LastUpdated = DateTime.Parse(current_datetime)
                        };

                        await Constants.conn.InsertOrReplaceAsync(others_insert);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving CAF Activity Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveCAFActivityToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
                }
            }
        }

        /* DIRECT SEND PROSPECT RETAILER TO SERVER REST */

        public async Task SendProspectRetailerDirectly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Sending prospect retailer to server");

            string apifile = "sync-prospect-directly-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject json = new JObject
                {
                    { "ContactID", id },
                    { "FirstName", firstname },
                    { "MiddleName", middlename },
                    { "LastName", lastname },
                    { "FileAs", fileas },
                    { "RetailerType", retailertype },
                    { "Street", street },
                    { "Barangay", barangay },
                    { "Town", town },
                    { "Province", province },
                    { "District", district },
                    { "Country", country },
                    { "Landmark", landmark },
                    { "Remarks", remarks },
                    { "RecordDate", DateTime.Parse(date) },
                    { "StartTime", DateTime.Parse(starttime) },
                    { "EndTime", DateTime.Parse(endtime) },
                    { "Telephone1", telephone1 },
                    { "Telephone2", telephone2 },
                    { "Mobile", mobile },
                    { "Email", email },
                    { "MobilePhoto1", photo1url },
                    { "MobilePhoto2", photo2url },
                    { "MobilePhoto3", photo3url },
                    { "MobileVideo", videourl },
                    { "Employee", employee },
                    { "Customer", customer },
                    { "Supervisor", contact },
                    { "RecordLog", recordlog },
                    { "Deleted", deleted },
                    { "LastSync", current_datetime },
                    { "LastUpdated", current_datetime }
                };

                var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                            await App.TodoManager.SendProspectRetailerMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendProspectRetailerDirectly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                            else
                            {
                                await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Error", "Sending failed.\n\nError:\n\n" + content + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                        if (retry)
                        {
                            await SendProspectRetailerDirectly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                        else
                        {
                            await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendProspectRetailerDirectly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                    else
                    {
                        await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendProspectRetailerDirectly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
                else
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);

                }
            }            
        }

        public async Task SendProspectRetailerMedia1Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Sending prospect retailer photo 1 to server");
            
            string apifile = "sync-contact-media-path-1-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(photo1url);

                if (!pathdoesExist || string.IsNullOrEmpty(photo1url))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", File.ReadAllBytes(photo1url)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await App.TodoManager.SendProspectRetailerMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 1 Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendProspectRetailerMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                            else
                            {
                                await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 1 Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendProspectRetailerMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                    else
                    {
                        await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 1 Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendProspectRetailerMedia1Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
                else
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
            }
        }

        public async Task SendProspectRetailerMedia2Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Sending prospect retailer photo 2 to server");
            string apifile = "sync-contact-media-path-2-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(photo2url);

                if (!pathdoesExist || string.IsNullOrEmpty(photo2url))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", File.ReadAllBytes(photo2url)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await App.TodoManager.SendProspectRetailerMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 2 Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendProspectRetailerMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                            else
                            {
                                await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 2 Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendProspectRetailerMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                    else
                    {
                        await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 2 Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendProspectRetailerMedia2Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
                else
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
            }
        }

        public async Task SendProspectRetailerMedia3Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Sending prospect retailer photo 3 to server");

            string apifile = "sync-contact-media-path-3-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(photo3url);

                if (!pathdoesExist || string.IsNullOrEmpty(photo3url))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", File.ReadAllBytes(photo3url)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                        var dataitem = dataresult[0];
                        var datamessage = dataitem.Message;

                        if (!datamessage.Equals("Inserted"))
                        {
                            await App.TodoManager.SendProspectRetailerMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 3 Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendProspectRetailerMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                            else
                            {
                                await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 3 Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendProspectRetailerMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                    else
                    {
                        await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Photo 3 Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendProspectRetailerMedia3Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
                else
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
            }
        }

        public async Task SendProspectRetailerMedia4Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Sending prospect retailer video to server");
            string apifile = "sync-caf-media-path-4-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject pathjson;
                bool pathdoesExist = File.Exists(videourl);

                if (!pathdoesExist || string.IsNullOrEmpty(videourl))
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", ""}
                    };
                }
                else
                {
                    pathjson = new JObject
                    {
                        { "MediaID", id},
                        { "Path", File.ReadAllBytes(videourl)}
                    };
                }

                var response = await client.PostAsync(uri, new StringContent(pathjson.ToString(), Encoding.UTF8, contentType));

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
                            await SaveProspectRetailerToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Video Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendProspectRetailerMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                            else
                            {
                                await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                            }
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Video Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendProspectRetailerMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                    else
                    {
                        await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending Prospect Retailer Video Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendProspectRetailerMedia4Directly(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
                else
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
            }
        }

        public async Task SaveProspectRetailerToLocalDatabaseSuccess(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Saving prospect retailer to local database");

            try
            {
                var retailer = new ContactsTable
                {
                    ContactID = id,
                    FileAs = firstname + " " + lastname + " " + middlename,
                    FirstName = firstname,
                    MiddleName = middlename,
                    LastName = lastname,
                    RetailerType = retailertype,
                    PresStreet = street,
                    PresBarangay = barangay,
                    PresDistrict = district,
                    PresTown = town,
                    PresProvince = province,
                    PresCountry = country,
                    Landmark = landmark,
                    CustomerRemarks = remarks,
                    RecordDate = DateTime.Parse(date),
                    StartTime = DateTime.Parse(starttime),
                    EndTime = DateTime.Parse(endtime),
                    Telephone1 = telephone1,
                    Telephone2 = telephone2,
                    Mobile = mobile,
                    Email = email,
                    Photo1 = photo1url,
                    Photo2 = photo2url,
                    Photo3 = photo3url,
                    Video = videourl,
                    MobilePhoto1 = photo1url,
                    MobilePhoto2 = photo2url,
                    MobilePhoto3 = photo3url,
                    MobileVideo = videourl,
                    Employee = employee,
                    Customer = customer,
                    Supervisor = contact,
                    RecordLog = recordlog,
                    Deleted = deleted,
                    LastSync = DateTime.Parse(current_datetime),
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await Constants.conn.InsertOrReplaceAsync(retailer);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving Prospect Retailer Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveProspectRetailerToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
                else
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
            }
        }

        public async Task SaveProspectRetailerToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            SyncStatus("Saving prospect retailer to local database");

            try
            {
                var retailer = new ContactsTable
                {
                    ContactID = id,
                    FileAs = firstname + " " + lastname + " " + middlename,
                    FirstName = firstname,
                    MiddleName = middlename,
                    LastName = lastname,
                    RetailerType = retailertype,
                    PresStreet = street,
                    PresBarangay = barangay,
                    PresDistrict = district,
                    PresTown = town,
                    PresProvince = province,
                    PresCountry = country,
                    Landmark = landmark,
                    CustomerRemarks = remarks,
                    RecordDate = DateTime.Parse(date),
                    StartTime = DateTime.Parse(starttime),
                    EndTime = DateTime.Parse(endtime),
                    Telephone1 = telephone1,
                    Telephone2 = telephone2,
                    Mobile = mobile,
                    Email = email,
                    Photo1 = photo1url,
                    Photo2 = photo2url,
                    Photo3 = photo3url,
                    Video = videourl,
                    MobilePhoto1 = photo1url,
                    MobilePhoto2 = photo2url,
                    MobilePhoto3 = photo3url,
                    MobileVideo = videourl,
                    Employee = employee,
                    Customer = customer,
                    Supervisor = contact,
                    RecordLog = recordlog,
                    Deleted = deleted,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await Constants.conn.InsertOrReplaceAsync(retailer);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving Prospect Retailer Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                }
            }
        }

        /* DIRECT SEND RETAILER OUTLET TO SERVER REST */

        public async Task SendRetailerOutletDirectly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog)
        {
            SyncStatus("Sending retailer outlet to server");

            string apifile = "sync-retailer-outlet-client-update-api.php";

            try
            {
                var uri = new Uri(string.Format("http://" + domain + "/TBSApp/" + apifolder + "/" + apifile + "?Host=" + host + "&Database=" + database, string.Empty));

                JObject json = new JObject
                {
                    { "ContactID", id },
                    { "RetailerCode", retailercode },
                    { "PresStreet", street },
                    { "PresBarangay", barangay },
                    { "PresDistrict", district },
                    { "PresTown", town },
                    { "PresProvince", province },
                    { "PresCountry", country },
                    { "Landmark", landmark },
                    { "Telephone1", telephone1 },
                    { "Telephone2", telephone2 },
                    { "Mobile", mobile },
                    { "Email", email },
                    { "GPSCoordinates", location },
                    { "Supervisor", contact },
                    { "Deleted", deleted },
                    { "RecordLog", recordlog },
                    { "LastUpdated", DateTime.Parse(current_datetime) }
                };

                var response = await client.PostAsync(uri, new StringContent(json.ToString(), Encoding.UTF8, contentType));

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
                            await SaveRetailerOutletToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                        }
                        else
                        {
                            var retry = await App.Current.MainPage.DisplayAlert("Sending Retailer Outlet Error", "Sending failed.\n\nError:\n\n" + datamessage + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                            if (retry)
                            {
                                await SendRetailerOutletDirectly(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                            }
                            else
                            {
                                await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                            }
                        }
                    }
                    else
                    {
                        var retry = await App.Current.MainPage.DisplayAlert("Sending Retailer Outlet Error", "Sending failed.\n\nError:\n\n" + content + "\n\nDo you want to retry?", "Yes", "Save Data Offline");

                        if (retry)
                        {
                            await SendRetailerOutletDirectly(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                        }
                        else
                        {
                            await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                        }
                    }
                }
                else
                {
                    var retry = await App.Current.MainPage.DisplayAlert("Sending Retailer Outlet Error", "Sending failed. Status Code:\n\n" + response.StatusCode, "Yes", "Save Data Offline");

                    if (retry)
                    {
                        await SendRetailerOutletDirectly(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                    }
                    else
                    {
                        await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Sending Retailer Outlet Error", "Sending failed.\n\nError:\n\n" + ex.Message, "Yes", "Save Data Offline");

                if (retry)
                {
                    await SendRetailerOutletDirectly(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                }
                else
                {
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                }
            }
        }

        public async Task SaveRetailerOutletToLocalDatabaseSuccess(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog)
        {
            SyncStatus("Saving retailer outlet to local database");

            try
            {
                var retailer_group_insert = new RetailerGroupTable
                {
                    ContactID = id,
                    RetailerCode = retailercode,
                    PresStreet = street,
                    PresBarangay = barangay,
                    PresDistrict = district,
                    PresTown = town,
                    PresProvince = province,
                    PresCountry = country,
                    Landmark = landmark,
                    Telephone1 = telephone1,
                    Telephone2 = telephone2,
                    Mobile = mobile,
                    Email = email,
                    GPSCoordinates = location,
                    Supervisor = contact,
                    RecordLog = recordlog,
                    Deleted = Convert.ToInt32(deleted),
                    LastSync = DateTime.Parse(current_datetime),
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await Constants.conn.InsertOrReplaceAsync(retailer_group_insert);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving Retailer Outlet Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveRetailerOutletToLocalDatabaseSuccess(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                }
                else
                {
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                }
            }
        }

        public async Task SaveRetailerOutletToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog)
        {
            SyncStatus("Saving retailer outlet to local database");

            try
            {
                var retailer_group_insert = new RetailerGroupTable
                {
                    ContactID = id,
                    RetailerCode = retailercode,
                    PresStreet = street,
                    PresBarangay = barangay,
                    PresDistrict = district,
                    PresTown = town,
                    PresProvince = province,
                    PresCountry = country,
                    Landmark = landmark,
                    Telephone1 = telephone1,
                    Telephone2 = telephone2,
                    Mobile = mobile,
                    Email = email,
                    GPSCoordinates = location,
                    Supervisor = contact,
                    RecordLog = recordlog,
                    Deleted = Convert.ToInt32(deleted),
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await Constants.conn.InsertOrReplaceAsync(retailer_group_insert);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var retry = await App.Current.MainPage.DisplayAlert("Saving Retailer Outlet Error", "Saving failed.\n\nError:\n\n" + ex.Message, "Yes", "No");

                if (retry)
                {
                    await SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                }
            }
        }

        public async Task OnSyncComplete(string host, string database, string domain, string contact)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task OnSendComplete(string host, string database, string domain, string contact)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Saving Data Success", "Data saved successfully", "Ok");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task OnSendCompleteModal(string host, string database, string domain, string contact)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
                await Application.Current.MainPage.DisplayAlert("Saving Data Success", "Data saved successfully", "Ok");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
