﻿using Microsoft.AppCenter.Crashes;
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
                PingReply pingresult = ping.Send(ipaddress, 200);
                
                if (pingresult.Status.ToString() == "Success")
                {
                    SyncUser(host, database, contact, ipaddress);
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
            public string DatabaseName { get; set; }
            public DateTime LastSync { get; set; }
            public DateTime LastUpdated { get; set; }
            public int Deleted { get; set; }
        }

        public class ServerMessage
        {
            public string Message { get; set; }
        }

        public async void SyncUser(string host, string database, string contact, string ipaddress)
        {
            Ping ping = new Ping();
            PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success")
            {
                try
                {
                    syncStatus.Text = "Initializing user sync";

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
                    var resultCount = getUser.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if(resultCount > 0)
                    {
                        var getUserChanges = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getUserChanges.Result.Count;

                        if(changesresultCount > 0)
                        {
                            int clientupdate = 1;

                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Sending user changes to server " + clientupdate + " out of " + changesresultCount;

                                    var result = getUserChanges.Result[i];
                                    var cruserID = result.UserID;
                                    var cruserPassword = result.UsrPassword;
                                    var cruserStatus = result.UserStatus;
                                    var cruserType = result.UserTypeID;
                                    var crrecordLog = result.RecordLog;
                                    var crdeleted = result.Deleted;
                                    var crlastUpdated = result.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=LX7swp";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "ContactID", contact },
                                        { "UserID", cruserID },
                                        { "UsrPassword", cruserPassword },
                                        { "UserStatus", cruserStatus },
                                        { "UserTypeID", cruserType },
                                        { "RecordLog", crrecordLog },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.IsSuccessStatusCode)
                                    {
                                        await conn.QueryAsync<UserTable>("UPDATE tblUser SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);

                                        clientupdate++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced user changes: " + (clientupdate - 1) + " out of " + changesresultCount + "\n";

                            try
                            {
                                syncStatus.Text = "Getting user updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=79MbtQ";
                                string chcontentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "ContactID", contact }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(json.ToString(), Encoding.UTF8, chcontentType));
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
                            syncStatus.Text = "Getting user data from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=8qApc8";
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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int count = 1;
                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var userresult = JsonConvert.DeserializeObject<List<UserData>>(content, settings);
                                    for (int i = 0; i < userresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing user " + count + " out of " + userresult.Count;

                                        var item = userresult[i];
                                        var contactID = item.ContactID;
                                        var userID = item.UserID;
                                        var userPassword = item.UsrPassword;
                                        var userType = item.UserTypeID;
                                        var userStatus = item.UserStatus;
                                        var recordLog = item.RecordLog;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var chuser = new UserTable
                                        {
                                            ContactID = contactID,
                                            UserID = userID,
                                            UsrPassword = userPassword,
                                            UserTypeID = userType,
                                            UserStatus = userStatus,
                                            RecordLog = recordLog,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(chuser);

                                        count++;
                                    }

                                    synccount += "Total synced user updates: " + (count - 1) + " out of " + userresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing user failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncUserUpdate(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing user failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncUserUpdate(string host, string database, string contact, string ipaddress)
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success")
            {
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                try
                {
                    syncStatus.Text = "Getting user updates from server";

                    var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=79MbtQ";
                    string chcontentType = "application/json";
                    JObject json = new JObject
                    {
                        { "ContactID", contact }
                    };

                    HttpClient chclient = new HttpClient();
                    var chresponse = await chclient.PostAsync(chlink, new StringContent(json.ToString(), Encoding.UTF8, chcontentType));

                    if (chresponse.IsSuccessStatusCode)
                    {
                        var chcontent = await chresponse.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(chcontent))
                        {
                            var settings = new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };

                            var chuserresult = JsonConvert.DeserializeObject<List<UserData>>(chcontent, settings);

                            int updatecount = 1;
                            int newcount = 1;
                            int servercount = 1;

                            for (int i = 0; i < chuserresult.Count; i++)
                            {
                                syncStatus.Text = "Checking user update " + servercount + " out of " + chuserresult.Count;

                                var item = chuserresult[i];
                                var chcontactID = item.ContactID;
                                var chuserID = item.UserID;
                                var chuserPassword = item.UsrPassword;
                                var chuserType = item.UserTypeID;
                                var chuserStatus = item.UserStatus;
                                var chrecordLog = item.RecordLog;
                                var chlastSync = DateTime.Parse(current_datetime);
                                var chlastUpdated = item.LastUpdated;
                                var chdltd = item.Deleted;

                                var chsql = "SELECT * FROM tblUser WHERE ContactID = '" + chcontactID + "'";
                                var chgetUser = conn.QueryAsync<UserTable>(chsql);
                                var chresultCount = chgetUser.Result.Count;

                                if (chresultCount > 0)
                                {
                                    if (chlastUpdated > chgetUser.Result[0].LastUpdated)
                                    {
                                        var chuser = new UserTable
                                        {
                                            ContactID = chcontactID,
                                            UserID = chuserID,
                                            UsrPassword = chuserPassword,
                                            UserTypeID = chuserType,
                                            UserStatus = chuserStatus,
                                            RecordLog = chrecordLog,
                                            LastSync = chlastSync,
                                            LastUpdated = chlastUpdated,
                                            Deleted = chdltd
                                        };

                                        await conn.InsertOrReplaceAsync(chuser);

                                        updatecount++;
                                    }
                                }
                                else
                                {
                                    var cheuser = new UserTable
                                    {
                                        ContactID = chcontactID,
                                        UserID = chuserID,
                                        UsrPassword = chuserPassword,
                                        UserTypeID = chuserType,
                                        UserStatus = chuserStatus,
                                        RecordLog = chrecordLog,
                                        LastSync = chlastSync,
                                        LastUpdated = chlastUpdated,
                                        Deleted = chdltd
                                    };

                                    await conn.InsertOrReplaceAsync(cheuser);

                                    newcount++;
                                }

                                servercount++;
                            }

                            synccount += "Total synced updated user: " + (updatecount - 1) + "\n";
                            synccount += "Total synced new user: " + (newcount - 1) + "\n";
                        }

                        SyncRetailer(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing user updates failed. Server is unreachable.";
                        btnBack.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing user updates failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }
        
        public async void SyncRetailer(string host, string database, string contact, string ipaddress)
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
                syncStatus.Text = "Initializing retailer sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var getContacts = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Supervisor = ? AND Deleted != '1'", contact);
                    var resultCount = getContacts.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if(resultCount > 0)
                    {
                        int count = 1;
                        
                        var getContactsChanges = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getContactsChanges.Result.Count;

                        if(changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Sending retailer outlet changes to server " + count + " out of " + changesresultCount;

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
                                    var crrecordLog = crresult.RecordLog;
                                    var crsupervisor = crresult.Supervisor;
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
                                        { "RecordLog", crrecordLog },
                                        { "Supervisor", crsupervisor },
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
                                                                                    if (!string.IsNullOrEmpty(crmobileVideo))
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
                                                                                            syncStatus.Text = "Re-syncing retailer failed. Server is unreachable.";
                                                                                            OnSyncComplete();
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
                                                                            syncStatus.Text = "Syncing retailer failed. Server is unreachable.";
                                                                            btnBack.IsVisible = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                syncStatus.Text = "Syncing retailer failed. Server is unreachable.";
                                                                btnBack.IsVisible = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    syncStatus.Text = "Syncing retailer failed. Server is unreachable.";
                                                    btnBack.IsVisible = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        syncStatus.Text = "Syncing retailer failed. Server is unreachable.";
                                        btnBack.IsVisible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced retailer changes: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            syncStatus.Text = "Getting retailer data from server";

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

                                int count = 1;

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var contactsresult = JsonConvert.DeserializeObject<List<ContactsData>>(content, settings);
                                    for (int i = 0; i < contactsresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing retailer " + count + " out of " + contactsresult.Count;

                                        var item = contactsresult[i];
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

                                        var retailer = new ContactsTable
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

                                        await conn.InsertOrReplaceAsync(retailer);

                                        count++;
                                    }

                                    synccount += "Total synced retailer: " + (count - 1) + " out of " + contactsresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing retailer failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncRetailerUpdates(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing retailer failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncRetailerUpdates(string host, string database, string contact, string ipaddress)
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
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                try
                {
                    syncStatus.Text = "Getting retailer updates from server";

                    var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=kq7K5P";
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

                        if (!string.IsNullOrEmpty(chcontent))
                        {
                            try
                            {

                                var chcontactsresults = JsonConvert.DeserializeObject<List<ContactsData>>(chcontent, settings);
                                var chcount = chcontactsresults.Count;

                                int updatecount = 1;
                                int newcount = 1;
                                int servercount = 1;

                                for (int i = 0; i < chcount; i++)
                                {
                                    syncStatus.Text = "Checking retailer update " + servercount + " out of " + chcount;

                                    var chitem = chcontactsresults[i];
                                    var chcontactID = chitem.ContactID;
                                    var chfileAs = chitem.FileAs;
                                    var chfirstName = chitem.FirstName;
                                    var chmiddleName = chitem.MiddleName;
                                    var chlastName = chitem.LastName;
                                    var chposition = chitem.Position;
                                    var chcompany = chitem.Company;
                                    var chcompanyID = chitem.CompanyID;
                                    var chretailerType = chitem.RetailerType;
                                    var chpresStreet = chitem.PresStreet;
                                    var chpresBarangay = chitem.PresBarangay;
                                    var chpresDistrict = chitem.PresDistrict;
                                    var chpresTown = chitem.PresTown;
                                    var chpresProvince = chitem.PresProvince;
                                    var chpresCountry = chitem.PresCountry;
                                    var chlandmark = chitem.Landmark;
                                    var chremarks = chitem.CustomerRemarks;
                                    var chrecordDate = chitem.RecordDate;
                                    var chstartTime = chitem.StartTime;
                                    var chendTime = chitem.EndTime;
                                    var chtelephone1 = chitem.Telephone1;
                                    var chtelephone2 = chitem.Telephone2;
                                    var chmobile = chitem.Mobile;
                                    var chphoto1 = chitem.Photo1;
                                    var chphoto2 = chitem.Photo2;
                                    var chphoto3 = chitem.Photo3;
                                    var chvideo = chitem.Video;
                                    var chmobilePhoto1 = chitem.MobilePhoto1;
                                    var chmobilePhoto2 = chitem.MobilePhoto2;
                                    var chmobilePhoto3 = chitem.MobilePhoto3;
                                    var chmobileVideo = chitem.MobileVideo;
                                    var chemail = chitem.Email;
                                    var chemployee = chitem.Employee;
                                    var chcustomer = chitem.Customer;
                                    var chSupervisor = chitem.Supervisor;
                                    var chRecordLog = chitem.RecordLog;
                                    var chlastSync = DateTime.Parse(current_datetime);
                                    var chlastUpdated = chitem.LastUpdated;
                                    var chdeleted = chitem.Deleted;

                                    var chgetRetailer = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID = ?", chcontactID);
                                    var chresultCount = chgetRetailer.Result.Count;

                                    if (chresultCount > 0)
                                    {
                                        if (chlastUpdated > chgetRetailer.Result[0].LastUpdated)
                                        {
                                            var chretailer = new ContactsTable
                                            {
                                                ContactID = chcontactID,
                                                FileAs = chfileAs,
                                                FirstName = chfirstName,
                                                MiddleName = chmiddleName,
                                                LastName = chlastName,
                                                Position = chposition,
                                                Company = chcompany,
                                                CompanyID = chcompanyID,
                                                RetailerType = chretailerType,
                                                PresStreet = chpresStreet,
                                                PresBarangay = chpresBarangay,
                                                PresDistrict = chpresDistrict,
                                                PresTown = chpresTown,
                                                PresProvince = chpresProvince,
                                                PresCountry = chpresCountry,
                                                Landmark = chlandmark,
                                                RecordDate = chrecordDate,
                                                StartTime = chstartTime,
                                                EndTime = chendTime,
                                                CustomerRemarks = chremarks,
                                                Telephone1 = chtelephone1,
                                                Telephone2 = chtelephone2,
                                                Mobile = chmobile,
                                                Email = chemail,
                                                Photo1 = chphoto1,
                                                Photo2 = chphoto2,
                                                Photo3 = chphoto3,
                                                Video = chvideo,
                                                MobilePhoto1 = chmobilePhoto1,
                                                MobilePhoto2 = chmobilePhoto2,
                                                MobilePhoto3 = chmobilePhoto3,
                                                MobileVideo = chmobileVideo,
                                                Employee = chemployee,
                                                Customer = chcustomer,
                                                Supervisor = chSupervisor,
                                                RecordLog = chRecordLog,
                                                LastSync = chlastSync,
                                                Deleted = chdeleted,
                                                LastUpdated = chlastUpdated
                                            };

                                            await conn.InsertOrReplaceAsync(chretailer);

                                            updatecount++;
                                        }
                                    }
                                    else
                                    {
                                        var cheretailer = new ContactsTable
                                        {
                                            ContactID = chcontactID,
                                            FileAs = chfileAs,
                                            FirstName = chfirstName,
                                            MiddleName = chmiddleName,
                                            LastName = chlastName,
                                            Position = chposition,
                                            Company = chcompany,
                                            CompanyID = chcompanyID,
                                            RetailerType = chretailerType,
                                            PresStreet = chpresStreet,
                                            PresBarangay = chpresBarangay,
                                            PresDistrict = chpresDistrict,
                                            PresTown = chpresTown,
                                            PresProvince = chpresProvince,
                                            PresCountry = chpresCountry,
                                            Landmark = chlandmark,
                                            CustomerRemarks = chremarks,
                                            RecordDate = chrecordDate,
                                            StartTime = chstartTime,
                                            EndTime = chendTime,
                                            Telephone1 = chtelephone1,
                                            Telephone2 = chtelephone2,
                                            Mobile = chmobile,
                                            Email = chemail,
                                            Photo1 = chphoto1,
                                            Photo2 = chphoto2,
                                            Photo3 = chphoto3,
                                            Video = chvideo,
                                            MobilePhoto1 = chmobilePhoto1,
                                            MobilePhoto2 = chmobilePhoto2,
                                            MobilePhoto3 = chmobilePhoto3,
                                            MobileVideo = chmobileVideo,
                                            Employee = chemployee,
                                            Customer = chcustomer,
                                            Supervisor = chSupervisor,
                                            RecordLog = chRecordLog,
                                            LastSync = chlastSync,
                                            Deleted = chdeleted,
                                            LastUpdated = chlastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(cheretailer);

                                        newcount++;
                                    }

                                    servercount++;
                                }

                                synccount += "Total synced updated retailer: " + (updatecount - 1) + "\n";
                                synccount += "Total synced new retailer: " + (newcount - 1) + "\n";
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                        }

                        SyncRetailerOutlet(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing retailer updates failed. Server is unreachable.";
                        btnBack.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing retailer updates failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncRetailerOutlet(string host, string database, string contact, string ipaddress)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success") {
                syncStatus.Text = "Initializing retailer outlet sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getRetailerGroup = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ?", contact);
                    var resultCount = getRetailerGroup.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if(resultCount > 0)
                    {
                        int count = 1;

                        var getOutletChanges = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getOutletChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                syncStatus.Text = "Sending retailer outlet changes to server " + count + " out of " + changesresultCount;

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
                                var crrecordLog = crresult.RecordLog;
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
                                    { "RecordLog", crrecordLog },
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
                                    syncStatus.Text = "Syncing retailer outlet failed. Server is unreachable.";
                                    btnBack.IsVisible = true;
                                }
                            }

                            synccount += "Total synced retailer outlet changes: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            syncStatus.Text = "Getting retailer outlet data from server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int count = 1;

                                    var contactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content, settings);
                                    for (int i = 0; i < contactsresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing retailer outlet " + count + " out of " + contactsresult.Count;

                                        var item = contactsresult[i];
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

                                        var retailer = new RetailerGroupTable
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

                                        await conn.InsertOrReplaceAsync(retailer);

                                        count++;
                                    }

                                    synccount += "Total synced retailer outlet: " + (count - 1) + " out of " + contactsresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing retailer outlet failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncRetailerOutletUpdates(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing retailer outlet failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncRetailerOutletUpdates(string host, string database, string contact, string ipaddress)
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
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                try
                {
                    syncStatus.Text = "Getting retailer outlet updates from server";

                    var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=X4eFLJ";
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

                        if (!string.IsNullOrEmpty(chcontent))
                        {
                            var chcontactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(chcontent, settings);

                            int updatecount = 1;
                            int newcount = 1;
                            int servercount = 1;

                            for (int i = 0; i < chcontactsresult.Count; i++)
                            {
                                syncStatus.Text = "Checking retailer outlet update " + servercount + " out of " + chcontactsresult.Count;

                                var chitem = chcontactsresult[i];
                                var chretailerCode = chitem.RetailerCode;
                                var chcontactID = chitem.ContactID;
                                var chpresStreet = chitem.PresStreet;
                                var chpresBarangay = chitem.PresBarangay;
                                var chpresDistrict = chitem.PresDistrict;
                                var chpresTown = chitem.PresTown;
                                var chpresProvince = chitem.PresProvince;
                                var chpresCountry = chitem.PresCountry;
                                var chtelephone1 = chitem.Telephone1;
                                var chtelephone2 = chitem.Telephone2;
                                var chmobile = chitem.Mobile;
                                var chemail = chitem.Email;
                                var chlandmark = chitem.Landmark;
                                var chgpsCoordinates = chitem.GPSCoordinates;
                                var chSupervisor = chitem.Supervisor;
                                var chRecordLog = chitem.RecordLog;
                                var chlastSync = DateTime.Parse(current_datetime);
                                var chlastUpdated = chitem.LastUpdated;
                                var chdeleted = chitem.Deleted;

                                var chgetRetailerOutlet = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode = ?", chretailerCode);
                                var chresultCount = chgetRetailerOutlet.Result.Count;

                                if (chresultCount > 0)
                                {
                                    if (chlastUpdated > chgetRetailerOutlet.Result[0].LastUpdated)
                                    {
                                        var chretailer = new RetailerGroupTable
                                        {
                                            RetailerCode = chretailerCode,
                                            ContactID = chcontactID,
                                            PresStreet = chpresStreet,
                                            PresBarangay = chpresBarangay,
                                            PresDistrict = chpresDistrict,
                                            PresTown = chpresTown,
                                            PresProvince = chpresProvince,
                                            PresCountry = chpresCountry,
                                            Telephone1 = chtelephone1,
                                            Telephone2 = chtelephone2,
                                            Mobile = chmobile,
                                            Email = chemail,
                                            Landmark = chlandmark,
                                            GPSCoordinates = chgpsCoordinates,
                                            Supervisor = chSupervisor,
                                            RecordLog = chRecordLog,
                                            LastSync = chlastSync,
                                            Deleted = chdeleted,
                                            LastUpdated = chlastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(chretailer);

                                        updatecount++;
                                    }
                                }
                                else
                                {
                                    var cheretailer = new RetailerGroupTable
                                    {
                                        RetailerCode = chretailerCode,
                                        ContactID = chcontactID,
                                        PresStreet = chpresStreet,
                                        PresBarangay = chpresBarangay,
                                        PresDistrict = chpresDistrict,
                                        PresTown = chpresTown,
                                        PresProvince = chpresProvince,
                                        PresCountry = chpresCountry,
                                        Telephone1 = chtelephone1,
                                        Telephone2 = chtelephone2,
                                        Mobile = chmobile,
                                        Email = chemail,
                                        Landmark = chlandmark,
                                        GPSCoordinates = chgpsCoordinates,
                                        Supervisor = chSupervisor,
                                        RecordLog = chRecordLog,
                                        LastSync = chlastSync,
                                        Deleted = chdeleted,
                                        LastUpdated = chlastUpdated
                                    };

                                    await conn.InsertOrReplaceAsync(cheretailer);

                                    newcount++;
                                }

                                servercount++;
                            }

                            synccount += "Total synced updated retailer outlet: " + (updatecount - 1) + "\n";
                            synccount += "Total synced new retailer outlet: " + (newcount - 1) + "\n";
                        }

                        SyncCaf(host, database, contact, ipaddress);
                    }
                    else
                    {
                        syncStatus.Text = "Syncing retailer outlet updates failed. Server is unreachable.";
                        btnBack.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing retailer outlet updates failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncCaf(string host, string database, string contact, string ipaddress)
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
                syncStatus.Text = "Initializing field activity sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblCaf WHERE EmployeeID = '" + contact + "'";
                    var getCAF = conn.QueryAsync<CAFTable>(sql);
                    var resultCount = getCAF.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if(resultCount > 0)
                    {
                        int count = 1;
                        
                        var getCAFChanges = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getCAFChanges.Result.Count;

                        if(changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Sending Supervisor activity changes to server " + count + " out of " + changesresultCount;

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
                                    var crgpsLocation = crresult.GPSCoordinates;
                                    var crremarks = crresult.Remarks;
                                    var crotherConcern = crresult.OtherConcern;
                                    var crrecordLog = crresult.RecordLog;
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
                                        { "GPSCoordinates", crgpsLocation },
                                        { "Remarks", crremarks },
                                        { "OtherConcern", crotherConcern },
                                        { "RecordLog", crrecordLog },
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
                                                                                    if (!string.IsNullOrEmpty(crmobileVideo))
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
                                                                                            syncStatus.Text = "Re-syncing field activity failed. Server is unreachable.";
                                                                                            OnSyncComplete();
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
                                                                            syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                                                            btnBack.IsVisible = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                                                btnBack.IsVisible = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                                    btnBack.IsVisible = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                        btnBack.IsVisible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced field activity: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            syncStatus.Text = "Getting field activity data from server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int count = 1;

                                    var cafresult = JsonConvert.DeserializeObject<List<CAFData>>(content, settings);
                                    for (int i = 0; i < cafresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing field activity " + count + " out of " + cafresult.Count;

                                        var item = cafresult[i];
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

                                        var caf = new CAFTable
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

                                        await conn.InsertOrReplaceAsync(caf);
                                        count++;
                                    }

                                    synccount += "Total synced field activity: " + (count - 1) + " out of " + cafresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncActivities(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncActivities(string host, string database, string contact, string ipaddress)
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
                syncStatus.Text = "Initializing activity sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getAct = conn.QueryAsync<ActivityData>("SELECT * FROM tblActivity WHERE ContactID = ?", contact);
                    var resultCount = getAct.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if (resultCount > 0)
                    {
                        int count = 1;
                        
                        var getActivityChanges = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getActivityChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Sending Supervisor activity changes to server " + count + " out of " + changesresultCount;

                                    var crresult = getActivityChanges.Result[i];
                                    var crcafNo = crresult.CAFNo;
                                    var crcontactId = crresult.ContactID;
                                    var cractivityid = crresult.ActivityID;
                                    var crrecordLog = crresult.RecordLog;
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=b7Q9XU";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "CAFNo", crcafNo },
                                        { "ContactID", crcontactId },
                                        { "ActivityID", cractivityid },
                                        { "RecordLog", crrecordLog },
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
                                        syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                        btnBack.IsVisible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced activity: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            syncStatus.Text = "Getting activity data from server";

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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int count = 1;
                                    
                                    var actresult = JsonConvert.DeserializeObject<List<ActivityData>>(content, settings);
                                    for (int i = 0; i < actresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing activity " + count + " out of " + actresult.Count;

                                        var item = actresult[i];
                                        var cafNo = item.CAFNo;
                                        var contactId = item.ContactID;
                                        var activityid = item.ActivityID;
                                        var recordlog = item.RecordLog;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var act = new ActivityTable
                                        {
                                            CAFNo = cafNo,
                                            ContactID = contactId,
                                            ActivityID = activityid,
                                            RecordLog = recordlog,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(act);

                                        count++;
                                    }

                                    synccount += "Total synced activity: " + (count - 1) + " out of " + actresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing field activity failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncSubscription(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing activity failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncSubscription(string host, string database, string contact, string ipaddress)
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success")
            {
                syncStatus.Text = "Initializing device registration sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getSub = conn.QueryAsync<SubscriptionData>("SELECT * FROM tblSubscription WHERE SerialNumber = ?", Constants.deviceID);
                    var resultCount = getSub.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if (resultCount > 0)
                    {
                        var getSubscriptionChanges = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getSubscriptionChanges.Result.Count;

                        int count = 1;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Sending device registration changes to server " + count + " out of " + changesresultCount;

                                    var crresult = getSubscriptionChanges.Result[i];
                                    var crserialNumber = crresult.SerialNumber;
                                    var crcontactID = crresult.ContactID;
                                    var crdateStart = crresult.DateStart;
                                    var crnoOfDays = crresult.NoOfDays;
                                    var crtrials = crresult.Trials;
                                    var crinputserialnumber = crresult.InputSerialNumber;
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=59EkmJ";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "ContactID", crcontactID },
                                        { "SerialNumber", crserialNumber },
                                        { "DateStart", crdateStart },
                                        { "Trials", crtrials },
                                        { "InputSerialNumber", crinputserialnumber },
                                        { "NoOfDays", crnoOfDays },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.IsSuccessStatusCode)
                                    {
                                        await conn.QueryAsync<SubscriptionTable>("UPDATE tblSubscription SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);

                                        count++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced device registration changes: " + (count - 1) + " out of " + changesresultCount + "\n";

                            try
                            {
                                syncStatus.Text = "Getting device registration updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=spw5SD";
                                string chcontentType = "application/json";
                                JObject chjson = new JObject
                                {
                                    { "ContactID", contact },
                                    { "DeviceID", Constants.deviceID }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.IsSuccessStatusCode)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        var settings = new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        };

                                        var chsubresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(chcontent, settings);

                                        int updatecount = 1;
                                        int newcount = 1;
                                        int servercount = 1;

                                        for (int i = 0; i < chsubresult.Count; i++)
                                        {
                                            syncStatus.Text = "Checking device registration update " + servercount + " out of " + chsubresult.Count;

                                            var chitem = chsubresult[i];
                                            var chSerialNumber = chitem.SerialNumber;
                                            var chcontactID = chitem.ContactID;
                                            var chnoOfDays = chitem.NoOfDays;
                                            var chdateStart = chitem.DateStart;
                                            var chtrials = chitem.Trials;
                                            var chinputserialnumber = chitem.InputSerialNumber;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;

                                            var chgetSubscription = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE ContactID = ? AND SerialNumber = ?", chcontactID, chSerialNumber);
                                            var chresultCount = chgetSubscription.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated > chgetSubscription.Result[0].LastUpdated)
                                                {
                                                    var chsub = new SubscriptionTable
                                                    {
                                                        SerialNumber = chSerialNumber,
                                                        ContactID = chcontactID,
                                                        NoOfDays = chnoOfDays,
                                                        DateStart = chdateStart,
                                                        Trials = chtrials,
                                                        InputSerialNumber = chinputserialnumber,
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(chsub);
                                                    syncStatus.Text = "Syncing subscription updates of " + chSerialNumber;

                                                    updatecount++;
                                                }
                                            }
                                            else
                                            {
                                                var chesub = new SubscriptionTable
                                                {
                                                    SerialNumber = chSerialNumber,
                                                    ContactID = chcontactID,
                                                    NoOfDays = chnoOfDays,
                                                    DateStart = chdateStart,
                                                    Trials = chtrials,
                                                    InputSerialNumber = chinputserialnumber,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(chesub);
                                                syncStatus.Text = "Syncing new subscription (" + chSerialNumber + ")";

                                                newcount++;
                                            }

                                            servercount++;
                                        }

                                        synccount += "Total synced updated device registration: " + (updatecount - 1) + "\n";
                                        synccount += "Total synced new device registration: " + (newcount - 1) + "\n";
                                    }
                                }
                                else
                                {
                                    syncStatus.Text = "Syncing subscription failed. Server is unreachable.";
                                    btnBack.IsVisible = true;
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
                                syncStatus.Text = "Getting device registration updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=spw5SD";
                                string chcontentType = "application/json";
                                JObject chjson = new JObject
                                {
                                    { "ContactID", contact },
                                    { "DeviceID", Constants.deviceID }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.IsSuccessStatusCode)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        var settings = new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        };

                                        var chsubresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(chcontent, settings);

                                        int updatecount = 1;
                                        int newcount = 1;
                                        int servercount = 1;

                                        for (int i = 0; i < chsubresult.Count; i++)
                                        {
                                            syncStatus.Text = "Checking device registration update " + servercount + " out of " + chsubresult.Count;

                                            var chitem = chsubresult[i];
                                            var chSerialNumber = chitem.SerialNumber;
                                            var chcontactID = chitem.ContactID;
                                            var chnoOfDays = chitem.NoOfDays;
                                            var chdateStart = chitem.DateStart;
                                            var chtrials = chitem.Trials;
                                            var chinputserialnumber = chitem.InputSerialNumber;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;
                                            
                                            var chgetSubscription = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE ContactID = ? AND SerialNumber = ?", chcontactID, chSerialNumber);
                                            var chresultCount = chgetSubscription.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated > chgetSubscription.Result[0].LastUpdated)
                                                {
                                                    var chsub = new SubscriptionTable
                                                    {
                                                        SerialNumber = chSerialNumber,
                                                        ContactID = chcontactID,
                                                        NoOfDays = chnoOfDays,
                                                        DateStart = chdateStart,
                                                        Trials = chtrials,
                                                        InputSerialNumber = chinputserialnumber,
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(chsub);
                                                    syncStatus.Text = "Syncing subscription updates of " + chSerialNumber;

                                                    updatecount++;
                                                }
                                            }
                                            else
                                            {
                                                var chesub = new SubscriptionTable
                                                {
                                                    SerialNumber = chSerialNumber,
                                                    ContactID = chcontactID,
                                                    NoOfDays = chnoOfDays,
                                                    DateStart = chdateStart,
                                                    Trials = chtrials,
                                                    InputSerialNumber = chinputserialnumber,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(chesub);
                                                syncStatus.Text = "Syncing new subscription (" + chSerialNumber + ")";

                                                newcount++;
                                            }

                                            servercount++;
                                        }

                                        synccount += "Total synced updated device registration: " + (updatecount - 1) + "\n";
                                        synccount += "Total synced new device registration: " + (newcount - 1) + "\n";
                                    }
                                }
                                else
                                {
                                    syncStatus.Text = "Syncing subscription failed. Server is unreachable.";
                                    btnBack.IsVisible = true;
                                }
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
                            syncStatus.Text = "Getting device registration data from server";

                            var sublink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=qtF5Ej";
                            string subcontentType = "application/json";
                            JObject subjson = new JObject
                            {
                                { "ContactID", contact },
                                { "DeviceID", Constants.deviceID }
                            };

                            HttpClient subclient = new HttpClient();
                            var subresponse = await subclient.PostAsync(sublink, new StringContent(subjson.ToString(), Encoding.UTF8, subcontentType));

                            if (subresponse.IsSuccessStatusCode)
                            {
                                var subcontent = await subresponse.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(subcontent))
                                {
                                    int count = 1;

                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var subsubresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(subcontent, settings);
                                    for (int i = 0; i < subsubresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing device registration " + count + " out of " + subsubresult.Count;

                                        var subitem = subsubresult[i];
                                        var subSerialNumber = subitem.SerialNumber;
                                        var subContactID = subitem.ContactID;
                                        var subnoOfDays = subitem.NoOfDays;
                                        var subdateStart = subitem.DateStart;
                                        var subtrials = subitem.Trials;
                                        var subinputserialnumber = subitem.InputSerialNumber;
                                        var sublastSync = DateTime.Parse(current_datetime);
                                        var sublastUpdated = subitem.LastUpdated;
                                        var subdeleted = subitem.Deleted;

                                        var subsub = new SubscriptionTable
                                        {
                                            SerialNumber = subSerialNumber,
                                            ContactID = subContactID,
                                            NoOfDays = subnoOfDays,
                                            DateStart = subdateStart,
                                            Trials = subtrials,
                                            InputSerialNumber = subinputserialnumber,
                                            LastSync = sublastSync,
                                            Deleted = subdeleted,
                                            LastUpdated = sublastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(subsub);

                                        count++;
                                    }

                                    synccount += "Total synced device registration: " + (count - 1) + " out of " + subsubresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing subscription failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncEmail(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing subscription failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }
        
        public async void SyncEmail(string host, string database, string contact, string ipaddress)
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success")
            {
                syncStatus.Text = "Initializing email recipient sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getEmail = conn.QueryAsync<EmailData>("SELECT * FROM tblUserEmail WHERE ContactID = ?", contact);
                    var resultCount = getEmail.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if (resultCount > 0)
                    {
                        int count = 1;
                        
                        var getEmailChanges = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getEmailChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Sending email recipient changes to server " + count + " out of " + changesresultCount;

                                    var crresult = getEmailChanges.Result[i];
                                    var crcontactID = crresult.ContactID;
                                    var cremail = crresult.Email;
                                    var crrecordLog = crresult.RecordLog;
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=kcZw9g";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "ContactID", crcontactID },
                                        { "Email", cremail },
                                        { "RecordLog", crrecordLog },
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
                                        syncStatus.Text = "Syncing email failed. Server is unreachable.";
                                        btnBack.IsVisible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced email recipient changes: " + (count - 1) + " out of " + changesresultCount + "\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            syncStatus.Text = "Getting user email data from server";

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

                                if (!string.IsNullOrEmpty(chcontent))
                                {
                                    int count = 1;

                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent, settings);
                                    for (int i = 0; i < chemailresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing email recipient " + count + " out of " + chemailresult.Count;

                                        var chitem = chemailresult[i];
                                        var chcontactID = chitem.ContactID;
                                        var chemail = chitem.Email;
                                        var chrecordLog = chitem.RecordLog;
                                        var chlastSync = DateTime.Parse(current_datetime);
                                        var chlastUpdated = chitem.LastUpdated;
                                        var chdeleted = chitem.Deleted;

                                        var cheml = new UserEmailTable
                                        {
                                            ContactID = chcontactID,
                                            Email = chemail,
                                            RecordLog = chrecordLog,
                                            LastSync = chlastSync,
                                            Deleted = chdeleted,
                                            LastUpdated = chlastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(cheml);

                                        count++;
                                    }

                                    synccount += "Total synced email recipient: " + (count - 1) + " out of " + chemailresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing email failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncEmailUpdates(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing email failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncEmailUpdates(string host, string database, string contact, string ipaddress)
        {
            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            try
            {
                syncStatus.Text = "Getting email recipient updates from server";

                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=pkGJ6V";
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

                    if (!string.IsNullOrEmpty(chcontent))
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        int updatecount = 1;
                        int newcount = 1;
                        int servercount = 1;

                        var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent, settings);
                        for (int i = 0; i < chemailresult.Count; i++)
                        {
                            syncStatus.Text = "Checking email recipient update " + servercount + " out of " + chemailresult.Count;

                            var chitem = chemailresult[i];
                            var chcontactID = chitem.ContactID;
                            var chemail = chitem.Email;
                            var chrecordLog = chitem.RecordLog;
                            var chlastSync = DateTime.Parse(current_datetime);
                            var chlastUpdated = chitem.LastUpdated;
                            var chdeleted = chitem.Deleted;

                            var chgetEmail = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ?", contact);
                            var chresultCount = chgetEmail.Result.Count;

                            if (chresultCount > 0)
                            {
                                if (chlastUpdated > chgetEmail.Result[0].LastUpdated)
                                {
                                    var cheml = new UserEmailTable
                                    {
                                        ContactID = chcontactID,
                                        Email = chemail,
                                        RecordLog = chrecordLog,
                                        LastSync = chlastSync,
                                        Deleted = chdeleted,
                                        LastUpdated = chlastUpdated
                                    };

                                    await conn.InsertOrReplaceAsync(cheml);

                                    updatecount++;
                                }
                            }
                            else
                            {
                                var cheeml = new UserEmailTable
                                {
                                    ContactID = chcontactID,
                                    Email = chemail,
                                    RecordLog = chrecordLog,
                                    LastSync = chlastSync,
                                    Deleted = chdeleted,
                                    LastUpdated = chlastUpdated
                                };

                                await conn.InsertOrReplaceAsync(cheeml);

                                newcount++;
                            }

                            servercount++;
                        }

                        synccount += "Total synced updated email recipient: " + (updatecount - 1) + "\n";
                        synccount += "Total synced new email recipient: " + (newcount - 1) + "\n";
                    }
                }
                else
                {
                    syncStatus.Text = "Syncing email failed. Server is unreachable.";
                    btnBack.IsVisible = true;
                }

                SyncProvince(host, database, contact, ipaddress);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void SyncProvince(string host, string database, string contact, string ipaddress)
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success")
            {
                syncStatus.Text = "Initializing province sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getProvince = conn.QueryAsync<ProvinceData>("SELECT * FROM tblProvince");
                    var resultCount = getProvince.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if(resultCount > 0)
                    {
                        try
                        {
                            syncStatus.Text = "Getting province updates from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=z9DmgJ";
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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int updatecount = 1;
                                    int newcount = 1;
                                    int servercount = 1;

                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var provinceresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content, settings);
                                    for (int i = 0; i < provinceresult.Count; i++)
                                    {
                                        syncStatus.Text = "Checking province update " + servercount + " out of " + provinceresult.Count;

                                        var item = provinceresult[i];
                                        var provinceID = item.ProvinceID;
                                        var province = item.Province;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var chgetProvince = conn.QueryAsync<ProvinceTable>("SELECT * FROM tblProvince WHERE ProvinceID = ?", provinceID);
                                        var chresultCount = chgetProvince.Result.Count;

                                        if (chresultCount > 0)
                                        {
                                            if (lastUpdated > chgetProvince.Result[0].LastUpdated)
                                            {
                                                var prov = new ProvinceTable
                                                {
                                                    ProvinceID = provinceID,
                                                    Province = province,
                                                    LastSync = lastSync,
                                                    Deleted = deleted,
                                                    LastUpdated = lastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(prov);
                                                syncStatus.Text = "Syncing province updates of " + province;

                                                updatecount++;
                                            }
                                        }
                                        else
                                        {
                                            var prov = new ProvinceTable
                                            {
                                                ProvinceID = provinceID,
                                                Province = province,
                                                LastSync = lastSync,
                                                Deleted = deleted,
                                                LastUpdated = lastUpdated
                                            };

                                            await conn.InsertOrReplaceAsync(prov);
                                            syncStatus.Text = "Syncing new province (" + province + ")";

                                            newcount++;
                                        }

                                        servercount++;
                                    }

                                    synccount += "Total synced updated province: " + (updatecount - 1) + "\n";
                                    synccount += "Total synced new province: " + (newcount - 1) + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing province failed. Server is unreachable.";
                                btnBack.IsVisible = true;
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
                            syncStatus.Text = "Getting provinces from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=EfMv7c";
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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int count = 1;

                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var provinceresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content, settings);
                                    for (int i = 0; i < provinceresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing province " + count + " out of " + provinceresult.Count;

                                        var item = provinceresult[i];
                                        var provinceID = item.ProvinceID;
                                        var province = item.Province;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var prov = new ProvinceTable
                                        {
                                            ProvinceID = provinceID,
                                            Province = province,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(prov);

                                        count++;
                                    }

                                    synccount += "Total synced province: " + (count - 1) + " out of " + provinceresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing province failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncTown(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing province failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncTown(string host, string database, string contact, string ipaddress)
        {
            Ping ping = new Ping();
             PingReply pingresult = ping.Send(ipaddress, 200);

            if (pingresult.Status.ToString() == "Success")
            {
                syncStatus.Text = "Initializing town sync";

                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getTown = conn.QueryAsync<TownData>("SELECT * FROM tblTown");
                    var resultCount = getTown.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if(resultCount > 0)
                    {
                        try
                        {
                            syncStatus.Text = "Getting town updates from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=hGv8V8";
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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    int updatecount = 1;
                                    int newcount = 1;
                                    int servercount = 1;

                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var townresult = JsonConvert.DeserializeObject<List<TownData>>(content, settings);
                                    for (int i = 0; i < townresult.Count; i++)
                                    {
                                        syncStatus.Text = "Checking town update " + servercount + " out of " + townresult.Count;

                                        var item = townresult[i];
                                        var townID = item.TownID;
                                        var provinceID = item.ProvinceID;
                                        var town = item.Town;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var chgetTown = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE TownID = ?", townID);
                                        var chresultCount = chgetTown.Result.Count;

                                        if (chresultCount > 0)
                                        {
                                            if (lastUpdated > chgetTown.Result[0].LastUpdated)
                                            {
                                                var twn = new TownTable
                                                {
                                                    ProvinceID = provinceID,
                                                    TownID = townID,
                                                    Town = town,
                                                    LastSync = lastSync,
                                                    Deleted = deleted,
                                                    LastUpdated = lastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(twn);
                                                syncStatus.Text = "Syncing town updates of " + town;

                                                updatecount++;
                                            }
                                        }
                                        else
                                        {
                                            var twn = new TownTable
                                            {
                                                ProvinceID = provinceID,
                                                TownID = townID,
                                                Town = town,
                                                LastSync = lastSync,
                                                Deleted = deleted,
                                                LastUpdated = lastUpdated
                                            };

                                            await conn.InsertOrReplaceAsync(twn);
                                            syncStatus.Text = "Syncing new town (" + town + ")";

                                            newcount++;
                                        }

                                        servercount++;
                                    }

                                    synccount += "Total synced updated town: " + (updatecount - 1) + "\n";
                                    synccount += "Total synced new town: " + (newcount - 1) + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing province failed. Server is unreachable.";
                                btnBack.IsVisible = true;
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
                            syncStatus.Text = "Getting town from server";

                            var twlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=F9jq3k";
                            string twcontentType = "application/json";
                            JObject twjson = new JObject
                            {
                                { "ContactID", contact }
                            };

                            HttpClient twclient = new HttpClient();
                            var twresponse = await twclient.PostAsync(twlink, new StringContent(twjson.ToString(), Encoding.UTF8, twcontentType));

                            if (twresponse.IsSuccessStatusCode)
                            {
                                var twcontent = await twresponse.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(twcontent))
                                {
                                    int twcount = 1;

                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                                    var twtownresult = JsonConvert.DeserializeObject<List<TownData>>(twcontent, settings);
                                    for (int i = 0; i < twtownresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing town " + twcount + " out of " + twtownresult.Count;

                                        var twitem = twtownresult[i];
                                        var twtownID = twitem.TownID;
                                        var twprovinceID = twitem.ProvinceID;
                                        var twtown = twitem.Town;
                                        var twlastSync = DateTime.Parse(current_datetime);
                                        var twlastUpdated = twitem.LastUpdated;
                                        var twdeleted = twitem.Deleted;

                                        var twtwn = new TownTable
                                        {
                                            ProvinceID = twprovinceID,
                                            TownID = twtownID,
                                            Town = twtown,
                                            LastSync = twlastSync,
                                            Deleted = twdeleted,
                                            LastUpdated = twlastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(twtwn);

                                        twcount++;
                                    }

                                    synccount += "Total synced town: " + (twcount - 1) + " out of " + twtownresult.Count + "\n";
                                }
                            }
                            else
                            {
                                syncStatus.Text = "Syncing province failed. Server is unreachable.";
                                btnBack.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }
                    
                    SyncLogs(host, database, contact, ipaddress);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing town failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }

        public async void SyncLogs(string host, string database, string contact, string ipaddress){
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
                    syncStatus.Text = "Initializing user logs sync";

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
                            try
                            {
                                syncStatus.Text = "Sending user logs to server " + clientupdate + " out of " + changesresultCount;

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
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                        }

                        synccount += "Total synced user logs: " + (clientupdate - 1) + " out of " + changesresultCount + "\n";
                    }

                    OnSyncComplete();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                syncStatus.Text = "Syncing user logs failed. Server is unreachable.";
                btnBack.IsVisible = true;
            }
        }
        
        public void OnSyncComplete()
        {
            
            if(synccount == "Sync Summary: \n\n")
            {
                syncStatus.Text = "Data has been synced successfully";
            }
            else
            {
                syncStatus.Text = synccount;
            }

            btnContinue.IsVisible = true;
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