using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
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
    public partial class SyncPage : ContentPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;
        string synccount = "Sync Summary: \n\n";

        public SyncPage(string host, string database, string contact, string ipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Check if there is an internet connection
            if (CrossConnectivity.Current.IsConnected)
            {
                Ping ping = new Ping();
                PingReply pingresult = ping.Send(ipaddress, 5000);
                
                if (pingresult.Status.ToString() == "Success")
                {
                    FirstTimeSyncUser(host, database, contact, ipaddress);
                }
                else
                {
                    Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress));
                }
            }
            else
            {
                Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress));
            }
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

        public class ServerMessage
        {
            public string Message { get; set; }
        }

        // ------------------------------ User Sync ------------------------------ //
        public async void FirstTimeSyncUser(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-user-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time user sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;
                       
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<UserData>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing user " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced user: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>User</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncSystemSerial(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncUserClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncUserClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-user-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing user client changes sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var datachanges = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
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
                                syncStatus.Text = "Sending user changes to server " + clientupdate + " out of " + changesresultCount;

                                var result = datachanges.Result[i];
                                var userid = result.UserID;
                                var usrpassword = result.UsrPassword;
                                var usertypeid = result.UserTypeID;
                                var userstatus = result.UserStatus;
                                var lastsync = DateTime.Parse(current_datetime);
                                var lastupdated = result.LastUpdated;
                                var deleted = result.Deleted;

                                var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                                string contentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "UserID", userid },
                                    { "UsrPassword", usrpassword },
                                    { "ContactID", contact },
                                    { "UserTypeID", usertypeid },
                                    { "UserStatus", userstatus },
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
                                            await conn.QueryAsync<UserTable>("UPDATE tblUsers SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);

                                            clientupdate++;
                                        }
                                    }
                                }
                                else
                                {
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
                                }
                            }

                            synccount += "Total synced client user update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>User</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncUserServerUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncUserServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-user-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing user server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting user updates from server";                        

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<UserData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;


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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced user update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>User</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncSystemSerial(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ System Serial Sync ------------------------------ //
        public async void FirstTimeSyncSystemSerial(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-system-serial-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time system serial sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE SerialNumber = ? AND Deleted != '1'", Constants.deviceID);
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

                            var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                            string contentType = "application/json";
                            JObject json = new JObject
                            {
                                { "Host", host },
                                { "Database", database },
                                { "ContactID", contact },
                                { "RegistrationCode", Constants.deviceID }
                            };

                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<SubscriptionTable>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing system serial " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced system serial: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>System Serial</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncContacts(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncSystemSerialServerUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncSystemSerialServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-system-serial-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing system serial server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting system serial updates from server";

                        var link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + apifile;
                        string contentType = "application/json";

                        JObject json = new JObject
                        {
                            { "Host", host },
                            { "Database", database },
                            { "ContactID", contact },
                            { "RegistrationCode", Constants.deviceID }
                        };

                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced system serial update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>System Serial</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncContacts(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ Contacts Sync ------------------------------ //
        public async void FirstTimeSyncContacts(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-contacts-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time contacts sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Supervisor = ? AND Deleted != '1'", contact);
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing contacts " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced contacts: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>Contacts</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncRetailerOutlet(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncContactsClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncContactsClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-contacts-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing contacts client changes sync";

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
                                syncStatus.Text = "Sending contacts changes to server " + clientupdate + " out of " + changesresultCount;

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
                                                        var path2response = await path2client.PostAsync(path1link, new StringContent(path2json.ToString(), Encoding.UTF8, path2contentType));

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
                                                                    var path3response = await path3client.PostAsync(path1link, new StringContent(path3json.ToString(), Encoding.UTF8, path3contentType));

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
                                                                                var path4response = await path4client.PostAsync(path1link, new StringContent(path4json.ToString(), Encoding.UTF8, path4contentType));

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
                                                                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                                                    Sync_Failed();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                                        Sync_Failed();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                            Sync_Failed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                Sync_Failed();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
                                }
                            }

                            synccount += "Total synced client contacts update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>Contacts</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncContactsServerUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncContactsServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-contacts-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing contacts server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting contacts updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced contacts update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>Contacts</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncRetailerOutlet(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ Retailer Outlet Sync ------------------------------ //
        public async void FirstTimeSyncRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-retailer-outlet-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time retailer outlet sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ?", contact);
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing retailer outlet " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced retailer outlet: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>Retailer Outlet</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncCAF(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncRetailerOutletClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
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
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-retailer-outlet-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing retailer outlet client changes sync";

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
                                syncStatus.Text = "Sending retailer outlet changes to server " + clientupdate + " out of " + changesresultCount;

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
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
                                }
                            }

                            synccount += "Total synced client retailer outlet update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>Retailer Outlet</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncRetailerOutletServerUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncRetailerOutletServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-retailer-outlet-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing retailer outlet server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting retailer outlet updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced retailer outlet update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>Retailer Outlet</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncCAF(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ CAF Sync ------------------------------ //
        public async void FirstTimeSyncCAF(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-caf-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time caf sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<CAFTable>("SELECT * FROM tblCAF WHERE EmployeeID = ? AND Deleted != '1'", contact);
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing caf " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(inserdata);

                                        count++;
                                    }

                                    synccount += "Total synced caf: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>CAF</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncCAFActivity(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncCAFClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
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
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-caf-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing caf client changes sync";

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
                                syncStatus.Text = "Sending caf changes to server " + clientupdate + " out of " + changesresultCount;

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
                                                        var path2response = await path2client.PostAsync(path1link, new StringContent(path2json.ToString(), Encoding.UTF8, path2contentType));

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
                                                                    var path3response = await path3client.PostAsync(path1link, new StringContent(path3json.ToString(), Encoding.UTF8, path3contentType));

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
                                                                                var path4response = await path4client.PostAsync(path1link, new StringContent(path4json.ToString(), Encoding.UTF8, path4contentType));

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
                                                                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                                                    Sync_Failed();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                                        Sync_Failed();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                            Sync_Failed();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                                Sync_Failed();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
                                }
                            }

                            synccount += "Total synced client caf update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>CAF</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncCAFServerUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncCAFServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-caf-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing caf server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting caf updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(inserdata);

                                    updatecount++;
                                }

                                synccount += "Total synced caf update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>CAF</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncCAFActivity(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ CAF Activity Sync ------------------------------ //
        public async void FirstTimeSyncCAFActivity(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-caf-activity-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time caf activity sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE Deleted != '1'");
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<ActivityTable>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing caf activity " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced caf activity: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>CAF Activity</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncEmailRecipient(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncCAFActivityClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
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
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-caf-activity-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing caf activity client changes sync";

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
                                syncStatus.Text = "Sending caf activity changes to server " + clientupdate + " out of " + changesresultCount;

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
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
                                }
                            }

                            synccount += "Total synced client caf activity update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>CAF Activity</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);
                        }

                        SyncCAFActivityServerUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncCAFActivityServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-caf-activity-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing caf activity server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting caf activity updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<ActivityData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced caf activity update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>CAF Activity</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncEmailRecipient(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ Email Recipient Sync ------------------------------ //
        public async void FirstTimeSyncEmailRecipient(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-email-recipient-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time email recipient sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ? AND Deleted != '1'", contact);
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<UserEmailTable>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing email recipient " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced email recipient: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>Email Recipient</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncProvince(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncEmailRecipientClientUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
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
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-email-recipient-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing email recipient client changes sync";

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
                                syncStatus.Text = "Sending email recipient changes to server " + clientupdate + " out of " + changesresultCount;

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
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
                                }
                            }

                            synccount += "Total synced client email recipient update: " + (clientupdate - 1) + "\n";

                            var logType = "App Log";
                            var log = "Sent client updates to the server (<b>Email Recipient</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                            int logdeleted = 0;

                            Save_Logs(contact, logType, log, database, logdeleted);


                        }

                        SyncEmailRecipientServerUpdate(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncEmailRecipientServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-email-recipient-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing email recipient server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting email recipient updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<UserEmailData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced email recipient update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>Email Recipient</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncProvince(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ Province Sync ------------------------------ //
        public async void FirstTimeSyncProvince(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-province-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time province sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<ProvinceTable>("SELECT * FROM tblProvince WHERE Deleted != '1'");
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing province " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced province: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>Province</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                FirstTimeSyncTown(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncProvinceServerUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncProvinceServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-province-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing province server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting province updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced province update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>Province</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            FirstTimeSyncTown(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ Town Sync ------------------------------ //
        public async void FirstTimeSyncTown(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "first-time-sync-town-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing first-time town sync";

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getData = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE Deleted != '1'");
                        var resultCount = getData.Result.Count;

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int count = 0;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        if (resultCount == 0)
                        {
                            syncStatus.Text = "Getting data from the server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var dataresult = JsonConvert.DeserializeObject<List<TownData>>(content, settings);
                                    var datacount = dataresult.Count;

                                    for (int i = 0; i < datacount; i++)
                                    {
                                        syncStatus.Text = "Syncing town " + count + " out of " + datacount;

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

                                        await conn.InsertOrReplaceAsync(insertdata);

                                        count++;
                                    }

                                    synccount += "Total synced town: " + (count + 1) + " out of " + datacount + "\n";

                                    var logType = "App Log";
                                    var log = "Initialized first-time sync (<b>Town</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                    int logdeleted = 0;

                                    Save_Logs(contact, logType, log, database, logdeleted);
                                }

                                SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                            }
                            else
                            {
                                syncStatus.Text = "Syncing failed. Server is unreachable.";
                                Sync_Failed();
                            }
                        }
                        else
                        {
                            SyncTownServerUpdate(host, database, contact, ipaddress);
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void SyncTownServerUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-town-server-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing customer salesman server changes sync";

                        var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        syncStatus.Text = "Getting town updates from server";

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

                            if (!string.IsNullOrEmpty(content))
                            {
                                var dataresult = JsonConvert.DeserializeObject<List<TownData>>(content, settings);

                                int updatecount = 1;

                                for (int i = 0; i < dataresult.Count; i++)
                                {
                                    syncStatus.Text = "Checking server update " + updatecount + " out of " + dataresult.Count;

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

                                    await conn.InsertOrReplaceAsync(insertdata);

                                    updatecount++;
                                }

                                synccount += "Total synced town update: " + (updatecount - 1) + "\n";

                                var logType = "App Log";
                                var log = "Checked server updates (<b>Town</b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                int logdeleted = 0;

                                Save_Logs(contact, logType, log, database, logdeleted);
                            }

                            SyncUserLogsClientUpdate(host, database, contact, ipaddress);
                        }
                        else
                        {
                            syncStatus.Text = "Syncing failed. Server is unreachable.";
                            Sync_Failed();
                        }
                    }
                    else
                    {
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        // ------------------------------ User Logs Sync ------------------------------ //
        public async void SyncUserLogsClientUpdate(string host, string database, string contact, string ipaddress)
        {
            try
            {
                syncStatus.Text = "Checking internet connection";

                var port = "7777";
                var apifolder = "TBSApp";
                string apifile = "sync-user-logs-client-update-api.php";

                if (CrossConnectivity.Current.IsConnected)
                {
                    syncStatus.Text = "Checking connection to server";

                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipaddress, 2000);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        syncStatus.Text = "Initializing user logs client changes sync";

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
                                syncStatus.Text = "Sending user logs changes to server " + clientupdate + " out of " + changesresultCount;

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
                                    syncStatus.Text = "Syncing failed. Server is unreachable.";
                                    Sync_Failed();
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
                        syncStatus.Text = "Syncing failed. Server is unreachable.";
                        Sync_Failed();
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing failed. Please connect to the internet to sync your data.";
                    Sync_Failed();
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public void Sync_Failed()
        {
            btnBack.IsVisible = true;
            btnContinue.IsVisible = false;
        }

        public void OnSyncComplete()
        {

            if (synccount == "Sync Summary: \n\n")
            {
                syncStatus.Text = "Data has been synced successfully";
            }
            else
            {
                syncStatus.Text = synccount;
            }

            btnContinue.IsVisible = true;
            btnContinue.IsEnabled = true;
            btnBack.IsVisible = false;
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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress));
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("username", String.Empty, "private_prefs");
            Preferences.Set("password", String.Empty, "private_prefs");
            await Navigation.PopToRootAsync();
        }
    }
}