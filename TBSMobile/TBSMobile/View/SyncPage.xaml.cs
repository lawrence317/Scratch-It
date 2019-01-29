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
        byte[] pingipaddress;
        string synccount = "Sync Summary: \n\n";

        public SyncPage(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            this.pingipaddress = pingipaddress;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Check if there is an internet connection
            if (CrossConnectivity.Current.IsConnected)
            {
                var ping = new Ping();
                var reply = ping.Send(new IPAddress(pingipaddress), 800);
                
                if (reply.Status == IPStatus.Success)
                {
                    SyncUser(host, database, contact, ipaddress, pingipaddress);
                }
                else
                {
                    Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress, pingipaddress));
                }
            }
            else
            {
                Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress, pingipaddress));
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
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class ActivityData
        {
            public string CAFNo { get; set; }
            public string ContactID { get; set; }
            public string ActivityID { get; set; }
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

        public async void SyncUser(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
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

                                if (chresponse.StatusCode == HttpStatusCode.OK)
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

                                        int changescount = 1;

                                        for (int i = 0; i < chuserresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing user update " + changescount + " out of " + chuserresult.Count;

                                            var item = chuserresult[i];
                                            var chcontactID = item.ContactID;
                                            var chuserID = item.UserID;
                                            var chuserPassword = item.UsrPassword;
                                            var chuserType = item.UserTypeID;
                                            var chuserStatus = item.UserStatus;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = item.LastUpdated;
                                            var chdltd = item.Deleted;

                                            var chsql = "SELECT * FROM tblUser WHERE ContactID = '" + chcontactID + "'";
                                            var chgetUser = conn.QueryAsync<UserTable>(chsql);
                                            var chresultCount = chgetUser.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated >= chgetUser.Result[0].LastUpdated)
                                                {
                                                    var chuser = new UserTable
                                                    {
                                                        ContactID = chcontactID,
                                                        UserID = chuserID,
                                                        UsrPassword = chuserPassword,
                                                        UserTypeID = chuserType,
                                                        UserStatus = chuserStatus,
                                                        LastSync = chlastSync,
                                                        LastUpdated = chlastUpdated,
                                                        Deleted = chdltd
                                                    };

                                                    await conn.InsertOrReplaceAsync(chuser);
                                                    syncStatus.Text = "Syncing user updates of " + chuserID;
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
                                                    LastSync = chlastSync,
                                                    LastUpdated = chlastUpdated,
                                                    Deleted = chdltd
                                                };

                                                await conn.InsertOrReplaceAsync(cheuser);
                                                syncStatus.Text = "Syncing new user (" + chuserID + ")";
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced user updates: " + (changescount - 1) + " out of " + chuserresult.Count + "\n";
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
                                syncStatus.Text = "Getting user updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=79MbtQ";
                                string chcontentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "ContactID", contact }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(json.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.StatusCode == HttpStatusCode.OK)
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

                                        int changescount = 1;

                                        for (int i = 0; i < chuserresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing user update " + changescount + " out of " + chuserresult.Count;

                                            var item = chuserresult[i];
                                            var chcontactID = item.ContactID;
                                            var chuserID = item.UserID;
                                            var chuserPassword = item.UsrPassword;
                                            var chuserType = item.UserTypeID;
                                            var chuserStatus = item.UserStatus;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = item.LastUpdated;
                                            var chdltd = item.Deleted;

                                            var chsql = "SELECT * FROM tblUser WHERE ContactID = '" + chcontactID + "'";
                                            var chgetUser = conn.QueryAsync<UserTable>(chsql);
                                            var chresultCount = chgetUser.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                               if(chlastUpdated >= chgetUser.Result[0].LastUpdated)
                                               {
                                                    var chuser = new UserTable
                                                    {
                                                        ContactID = chcontactID,
                                                        UserID = chuserID,
                                                        UsrPassword = chuserPassword,
                                                        UserTypeID = chuserType,
                                                        UserStatus = chuserStatus,
                                                        LastSync = chlastSync,
                                                        LastUpdated = chlastUpdated,
                                                        Deleted = chdltd
                                                    };

                                                    await conn.InsertOrReplaceAsync(chuser);
                                                    syncStatus.Text = "Syncing user updates of " + chuserID;
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
                                                    LastSync = chlastSync,
                                                    LastUpdated = chlastUpdated,
                                                    Deleted = chdltd
                                                };

                                                await conn.InsertOrReplaceAsync(cheuser);
                                                syncStatus.Text = "Syncing new user (" + chuserID + ")";
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced user updates: " + (changescount - 1) + " out of " + chuserresult.Count + "\n";
                                    }
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
                            syncStatus.Text = "Getting user data from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=8qApc8";
                            string contentType = "application/json";
                            JObject json = new JObject
                            {
                                { "ContactID", contact }
                            };

                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.StatusCode == HttpStatusCode.OK)
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncRetailer(host, database, contact, ipaddress, pingipaddress);
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
        
        public async void SyncRetailer(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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
                                        { "Supervisor", crsupervisor },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };
                                    
                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent) || !crcontent.Equals("[]") || !crcontent.Equals("[[],[]]") || !crcontent.Equals("[[],[]]"))
                                        {
                                            byte[] crPhoto1Data = File.ReadAllBytes(crphoto1);

                                            var ph1link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=tWyd43";
                                            string ph1contentType = "application/json";
                                            JObject ph1json = new JObject
                                            {
                                                { "ContactID", crcontactID },
                                                { "Photo1", crPhoto1Data }
                                            };

                                            HttpClient ph1client = new HttpClient();
                                            var ph1response = await ph1client.PostAsync(ph1link, new StringContent(ph1json.ToString(), Encoding.UTF8, ph1contentType));

                                            if (ph1response.StatusCode == HttpStatusCode.OK)
                                            {
                                                var ph1content = await ph1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(ph1content) || !ph1content.Equals("[]") || !ph1content.Equals("[[],[]]") || !ph1content.Equals("[[],[]]"))
                                                {
                                                    byte[] crPhoto2Data = File.ReadAllBytes(crphoto2);

                                                    var ph2link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=qAWS26";
                                                    string ph2contentType = "application/json";
                                                    JObject ph2json = new JObject
                                                    {
                                                        { "ContactID", crcontactID },
                                                        { "Photo2", crPhoto2Data }
                                                    };

                                                    HttpClient ph2client = new HttpClient();
                                                    var ph2response = await ph2client.PostAsync(ph2link, new StringContent(ph2json.ToString(), Encoding.UTF8, ph2contentType));

                                                    if (ph2response.StatusCode == HttpStatusCode.OK)
                                                    {
                                                        var ph2content = await ph2response.Content.ReadAsStringAsync();

                                                        if (!string.IsNullOrEmpty(ph2content) || !ph2content.Equals("[]") || !ph2content.Equals("[[],[]]") || !ph2content.Equals("[[],[]]"))
                                                        {
                                                            byte[] crPhoto3Data = File.ReadAllBytes(crphoto3);

                                                            var ph3link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=XuY4RN";
                                                            string ph3contentType = "application/json";
                                                            JObject ph3json = new JObject
                                                            {
                                                                { "ContactID", crcontactID },
                                                                { "Photo3", crPhoto3Data }
                                                            };

                                                            HttpClient ph3client = new HttpClient();
                                                            var ph3response = await ph3client.PostAsync(ph3link, new StringContent(ph3json.ToString(), Encoding.UTF8, ph3contentType));

                                                            if (ph3response.StatusCode == HttpStatusCode.OK)
                                                            {
                                                                var ph3content = await ph3response.Content.ReadAsStringAsync();
                                                                if (!string.IsNullOrEmpty(ph3content) || !ph3content.Equals("[]") || !ph3content.Equals("[[],[]]") || !ph3content.Equals("[[],[]]"))
                                                                {
                                                                    if (!string.IsNullOrEmpty(crvideo))
                                                                    {
                                                                        try
                                                                        {
                                                                            byte[] crVideoData;

                                                                            if (!string.IsNullOrEmpty(crvideo))
                                                                            {
                                                                                crVideoData = File.ReadAllBytes(crvideo);
                                                                            }
                                                                            else
                                                                            {
                                                                                crVideoData = null;
                                                                            }

                                                                            var vidlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=PsxQ7v";
                                                                            string vidcontentType = "application/json";
                                                                            JObject vidjson = new JObject
                                                                                {
                                                                                    { "ContactID", crcontactID },
                                                                                    { "Video", crVideoData }
                                                                                };

                                                                            HttpClient vidclient = new HttpClient();
                                                                            var vidresponse = await vidclient.PostAsync(vidlink, new StringContent(vidjson.ToString(), Encoding.UTF8, vidcontentType));

                                                                            if (vidresponse.StatusCode == HttpStatusCode.OK)
                                                                            {
                                                                                var vidcontent = await vidresponse.Content.ReadAsStringAsync();

                                                                                if (!string.IsNullOrEmpty(vidcontent) || !vidcontent.Equals("[]") || !vidcontent.Equals("[[],[]]") || !vidcontent.Equals("[[],[]]"))
                                                                                {
                                                                                    await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), crcontactID);
                                                                                    count++;
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
                                                                        await conn.QueryAsync<ContactsTable>("UPDATE tblContacts SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), crcontactID);
                                                                        count++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced retailer changes: " + (count - 1) + " out of " + changesresultCount + "\n";

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

                                if (chresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        try
                                        {
                                            var settings = new JsonSerializerSettings
                                            {
                                                NullValueHandling = NullValueHandling.Ignore,
                                                MissingMemberHandling = MissingMemberHandling.Ignore
                                            };

                                            var chcontactsresults = JsonConvert.DeserializeObject<List<ContactsData>>(chcontent, settings);
                                            var chcount = chcontactsresults.Count;

                                            int changescount = 1;

                                            for (int i = 0; i < chcount; i++)
                                            {
                                                syncStatus.Text = "Syncing retailer update " + changescount + " out of " + chcount;

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
                                                var chlastSync = DateTime.Parse(current_datetime);
                                                var chlastUpdated = chitem.LastUpdated;
                                                var chdeleted = chitem.Deleted;

                                                var chgetRetailer = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID = ?", chcontactID);
                                                var chresultCount = chgetRetailer.Result.Count;

                                                if (chresultCount > 0)
                                                {
                                                    if (chlastUpdated >= chgetRetailer.Result[0].LastUpdated)
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
                                                            LastSync = chlastSync,
                                                            Deleted = chdeleted,
                                                            LastUpdated = chlastUpdated
                                                        };

                                                        await conn.InsertOrReplaceAsync(chretailer);
                                                        syncStatus.Text = "Syncing retailer updates of " + chfileAs;
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
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(cheretailer);
                                                    syncStatus.Text = "Syncing new retailer (" + chfileAs + ")";
                                                }

                                                changescount++;
                                            }

                                            synccount += "Total synced retailer updates: " + (changescount - 1) + " out of " + chcount + "\n";
                                        }
                                        catch (Exception ex)
                                        {
                                            Crashes.TrackError(ex);
                                        }
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
                                syncStatus.Text = "Getting retailer updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=kq7K5P";
                                string chcontentType = "application/json";
                                JObject chjson = new JObject
                                {
                                    { "ContactID", contact }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        try
                                        {
                                            var settings = new JsonSerializerSettings
                                            {
                                                NullValueHandling = NullValueHandling.Ignore,
                                                MissingMemberHandling = MissingMemberHandling.Ignore
                                            };

                                            var chcontactsresults = JsonConvert.DeserializeObject<List<ContactsData>>(chcontent, settings);
                                            var chcount = chcontactsresults.Count;

                                            int changescount = 1;

                                            for (int i = 0; i < chcount; i++)
                                            {
                                                syncStatus.Text = "Syncing retailer update " + changescount + " out of " + chcount;

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
                                                var chlastSync = DateTime.Parse(current_datetime);
                                                var chlastUpdated = chitem.LastUpdated;
                                                var chdeleted = chitem.Deleted;

                                                var chgetRetailer = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID = ?", chcontactID);
                                                var chresultCount = chgetRetailer.Result.Count;

                                                if (chresultCount > 0)
                                                {
                                                    if (chlastUpdated >= chgetRetailer.Result[0].LastUpdated)
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
                                                            LastSync = chlastSync,
                                                            Deleted = chdeleted,
                                                            LastUpdated = chlastUpdated
                                                        };

                                                        await conn.InsertOrReplaceAsync(chretailer);
                                                        syncStatus.Text = "Syncing retailer updates of " + chfileAs;
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
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(cheretailer);
                                                    syncStatus.Text = "Syncing new retailer (" + chfileAs + ")";
                                                }

                                                changescount++;
                                            }

                                            synccount += "Total synced retailer updates: " + (changescount - 1) + " out of " + chcount + "\n";
                                        }
                                        catch (Exception ex)
                                        {
                                            Crashes.TrackError(ex);
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

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var content = await response.Content.ReadAsStringAsync();

                                int count = 1;

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncRetailerOutlet(host, database, contact, ipaddress, pingipaddress);
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

        public async void SyncRetailerOutlet(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success) {
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
                                    { "Deleted", crdeleted },
                                    { "LastUpdated", crlastUpdated }
                                };

                                HttpClient crclient = new HttpClient();
                                var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                if (crresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var crcontent = await crresponse.Content.ReadAsStringAsync();
                                    if (!string.IsNullOrEmpty(crcontent) || !crcontent.Equals("[]") || !crcontent.Equals("[[],[]]") || !crcontent.Equals("[[],[]]"))
                                    {
                                        await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE RetailerCode = ?", DateTime.Parse(current_datetime), crretailerCode);
                                        count++;
                                    }
                                }
                            }

                            synccount += "Total synced retailer outlet changes: " + (count - 1) + " out of " + changesresultCount + "\n";

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

                                if (chresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        var settings = new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        };

                                        var chcontactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(chcontent, settings);

                                        int changescount = 1;

                                        for (int i = 0; i < chcontactsresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing retailer outlet update " + changescount + " out of " + chcontactsresult.Count;

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
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;

                                            var chgetRetailerOutlet = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode = ?", chretailerCode);
                                            var chresultCount = chgetRetailerOutlet.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated >= chgetRetailerOutlet.Result[0].LastUpdated)
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
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(chretailer);
                                                    syncStatus.Text = "Syncing retailer outlet updates of " + chretailerCode;
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
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(cheretailer);
                                                syncStatus.Text = "Syncing new retailer outlet (" + chretailerCode + ")";
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced retailer outlet updates: " + (changescount - 1) + " out of " + chcontactsresult.Count + "\n";
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
                                syncStatus.Text = "Getting retailer outlet updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=X4eFLJ";
                                string chcontentType = "application/json";
                                JObject chjson = new JObject
                                {
                                    { "ContactID", contact }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        var settings = new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        };

                                        var chcontactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(chcontent, settings);

                                        int changescount = 1;

                                        for (int i = 0; i < chcontactsresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing retailer outlet update " + changescount + " out of " + chcontactsresult.Count;

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
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;
                                            
                                            var chgetRetailerOutlet = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode = ?", chretailerCode);
                                            var chresultCount = chgetRetailerOutlet.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated >= chgetRetailerOutlet.Result[0].LastUpdated)
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
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(chretailer);
                                                    syncStatus.Text = "Syncing retailer outlet updates of " + chretailerCode;
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
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(cheretailer);
                                                syncStatus.Text = "Syncing new retailer outlet (" + chretailerCode + ")";
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced retailer outlet updates: " + (changescount - 1) + " out of " + chcontactsresult.Count + "\n";
                                    }
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
                            syncStatus.Text = "Getting retailer outlet data from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Vf6HfC";
                            string contentType = "application/json";
                            JObject json = new JObject
                            {
                                { "ContactID", contact }
                            };

                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.StatusCode == HttpStatusCode.OK)
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncCaf(host, database, contact, ipaddress, pingipaddress);
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
        
        public async void SyncCaf(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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
                                    var crremarks = crresult.Remarks;
                                    var crotherConcern = crresult.OtherConcern;
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
                                        { "Remarks", crremarks },
                                        { "OtherConcern", crotherConcern },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent) || !crcontent.Equals("[]") || !crcontent.Equals("[[],[]]") || !crcontent.Equals("[[],[]]"))
                                        {
                                            byte[] crPhoto1Data = File.ReadAllBytes(crphoto1);

                                            var ph1link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=N4f5GL";
                                            string ph1contentType = "application/json";
                                            JObject ph1json = new JObject
                                            {
                                                { "CAFNo", crcafNo },
                                                { "CAFDate", crcafDate },
                                                { "Photo1", crPhoto1Data }
                                            };

                                            HttpClient ph1client = new HttpClient();
                                            var ph1response = await ph1client.PostAsync(ph1link, new StringContent(ph1json.ToString(), Encoding.UTF8, ph1contentType));
                                            if (ph1response.StatusCode == HttpStatusCode.OK)
                                            {
                                                var ph1content = await ph1response.Content.ReadAsStringAsync();
                                                if (!string.IsNullOrEmpty(ph1content) || !ph1content.Equals("[]") || !ph1content.Equals("[[],[]]") || !ph1content.Equals("[[],[]]"))
                                                {
                                                    byte[] crPhoto2Data = File.ReadAllBytes(crphoto2);

                                                    var ph2link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=6LqMxW";
                                                    string ph2contentType = "application/json";
                                                    JObject ph2json = new JObject
                                                    {
                                                        { "CAFNo", crcafNo },
                                                        { "CAFDate", crcafDate },
                                                        { "Photo2", crPhoto2Data }
                                                    };

                                                    HttpClient ph2client = new HttpClient();
                                                    var ph2response = await ph2client.PostAsync(ph2link, new StringContent(ph2json.ToString(), Encoding.UTF8, ph2contentType));

                                                    var ph2content = await ph2response.Content.ReadAsStringAsync();
                                                    if (!string.IsNullOrEmpty(ph2content) || !ph2content.Equals("[]") || !ph2content.Equals("[[],[]]") || !ph2content.Equals("[[],[]]"))
                                                    {
                                                        byte[] crPhoto3Data = File.ReadAllBytes(crphoto3);

                                                        var ph3link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Mpt2Y9";
                                                        string ph3contentType = "application/json";
                                                        JObject ph3json = new JObject
                                                        {
                                                            { "CAFNo", crcafNo },
                                                            { "CAFDate", crcafDate },
                                                            { "Photo3", crPhoto3Data }
                                                        };

                                                        HttpClient ph3client = new HttpClient();
                                                        var ph3response = await ph3client.PostAsync(ph3link, new StringContent(ph3json.ToString(), Encoding.UTF8, ph3contentType));

                                                        if (ph3response.StatusCode == HttpStatusCode.OK)
                                                        {
                                                            var ph3content = await ph3response.Content.ReadAsStringAsync();
                                                            if (!string.IsNullOrEmpty(ph3content) || !ph3content.Equals("[]") || !ph3content.Equals("[[],[]]") || !ph3content.Equals("[[],[]]"))
                                                            {
                                                                if (!string.IsNullOrEmpty(crvideo))
                                                                {
                                                                    try
                                                                    {
                                                                        byte[] crVideoData;

                                                                        if (!string.IsNullOrEmpty(crvideo))
                                                                        {
                                                                            crVideoData = File.ReadAllBytes(crvideo);
                                                                        }
                                                                        else
                                                                        {
                                                                            crVideoData = null;
                                                                        }

                                                                        var vidlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=Lqr9fy";
                                                                        string vidcontentType = "application/json";
                                                                        JObject vidjson = new JObject
                                                                        {
                                                                            { "CAFNo", crcafNo },
                                                                            { "CAFDate", crcafDate },
                                                                            { "Video", crVideoData }
                                                                        };

                                                                        HttpClient vidclient = new HttpClient();
                                                                        var vidresponse = await vidclient.PostAsync(vidlink, new StringContent(vidjson.ToString(), Encoding.UTF8, vidcontentType));

                                                                        if (vidresponse.StatusCode == HttpStatusCode.OK)
                                                                        {
                                                                            var vidcontent = await vidresponse.Content.ReadAsStringAsync();
                                                                            if (!string.IsNullOrEmpty(vidcontent) || !vidcontent.Equals("[]") || !vidcontent.Equals("[[],[]]") || !vidcontent.Equals("[[],[]]"))
                                                                            {
                                                                                await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), crcafNo);
                                                                                count++;
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
                                                                    await conn.QueryAsync<CAFTable>("UPDATE tblCaf SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), crcafNo);
                                                                    count++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
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

                            if (response.StatusCode == HttpStatusCode.OK)
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
                                        var remarks = item.Remarks;
                                        var otherConcern = item.OtherConcern;
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
                                            Remarks = remarks,
                                            OtherConcern = otherConcern,
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncActivities(host, database, contact, ipaddress, pingipaddress);
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

        public async void SyncActivities(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=b7Q9XU";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "CAFNo", crcafNo },
                                        { "ContactID", crcontactId },
                                        { "ActivityID", cractivityid },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent) || !crcontent.Equals("[]") || !crcontent.Equals("[[],[]]") || !crcontent.Equals("[[],[]]"))
                                        {
                                            await conn.QueryAsync<ActivityTable>("UPDATE tblActivity SET LastSync = ? WHERE CAFNo = ?", DateTime.Parse(current_datetime), crcafNo);
                                            count++;
                                        }
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

                            if (response.StatusCode == HttpStatusCode.OK)
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

                                    var actresult = JsonConvert.DeserializeObject<List<ActivityData>>(content, settings);
                                    for (int i = 0; i < actresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing activity " + count + " out of " + actresult.Count;

                                        var item = actresult[i];
                                        var cafNo = item.CAFNo;
                                        var contactId = item.ContactID;
                                        var activityid = item.ActivityID;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var act = new ActivityTable
                                        {
                                            CAFNo = cafNo,
                                            ContactID = contactId,
                                            ActivityID = activityid,
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncSubscription(host, database, contact, ipaddress, pingipaddress);
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

        public async void SyncSubscription(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
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

                                if (chresponse.StatusCode == HttpStatusCode.OK)
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

                                        int changescount = 1;

                                        for (int i = 0; i < chsubresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing device registration update " + changescount + " out of " + chsubresult.Count;

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
                                                if (chlastUpdated >= chgetSubscription.Result[0].LastUpdated)
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
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced device registration updates: " + (changescount - 1) + " out of " + chsubresult.Count + "\n";
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

                                if (chresponse.StatusCode == HttpStatusCode.OK)
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

                                        int changescount = 1;

                                        for (int i = 0; i < chsubresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing device registration update " + changescount + " out of " + chsubresult.Count;

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
                                                if (chlastUpdated >= chgetSubscription.Result[0].LastUpdated)
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
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced device registration updates: " + (changescount - 1) + " out of " + chsubresult.Count + "\n";
                                    }
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

                            if (subresponse.StatusCode == HttpStatusCode.OK)
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncEmail(host, database, contact, ipaddress, pingipaddress);
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
        
        public async void SyncEmail(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=kcZw9g";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "ContactID", crcontactID },
                                        { "Email", cremail },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);
                                        count++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }

                            synccount += "Total synced email recipient changes: " + (count - 1) + " out of " + changesresultCount + "\n";

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

                                if (chresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        var settings = new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        };

                                        int changescount = 1;

                                        var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent, settings);
                                        for (int i = 0; i < chemailresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing email recipient update " + changescount + " out of " + chemailresult.Count;

                                            var chitem = chemailresult[i];
                                            var chcontactID = chitem.ContactID;
                                            var chemail = chitem.Email;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;

                                            var chgetEmail = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ?", contact);
                                            var chresultCount = chgetEmail.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated >= chgetEmail.Result[0].LastUpdated)
                                                {
                                                    var cheml = new UserEmailTable
                                                    {
                                                        ContactID = chcontactID,
                                                        Email = chemail,
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(cheml);
                                                    syncStatus.Text = "Syncing email recipient updates of " + chcontactID;
                                                }
                                            }
                                            else
                                            {
                                                var cheeml = new UserEmailTable
                                                {
                                                    ContactID = chcontactID,
                                                    Email = chemail,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(cheeml);
                                                syncStatus.Text = "Syncing email recipient (" + chcontactID + ")";
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced email recipient updates: " + (changescount - 1) + " out of " + chemailresult.Count + "\n";
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
                                syncStatus.Text = "Getting email recipient updates from server";

                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=pkGJ6V";
                                string chcontentType = "application/json";
                                JObject chjson = new JObject
                                {
                                    { "ContactID", contact }
                                };

                                HttpClient chclient = new HttpClient();
                                var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    var chcontent = await chresponse.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(chcontent))
                                    {
                                        var settings = new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        };

                                        int changescount = 1;

                                        var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent, settings);
                                        for (int i = 0; i < chemailresult.Count; i++)
                                        {
                                            syncStatus.Text = "Syncing email recipient update " + changescount + " out of " + chemailresult.Count;

                                            var chitem = chemailresult[i];
                                            var chcontactID = chitem.ContactID;
                                            var chemail = chitem.Email;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;
                                            
                                            var chgetEmail = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ?", contact);
                                            var chresultCount = chgetEmail.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated >= chgetEmail.Result[0].LastUpdated)
                                                {
                                                    var cheml = new UserEmailTable
                                                    {
                                                        ContactID = chcontactID,
                                                        Email = chemail,
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(cheml);
                                                    syncStatus.Text = "Syncing email recipient updates of " + chcontactID;
                                                }
                                            }
                                            else
                                            {
                                                var cheeml = new UserEmailTable
                                                {
                                                    ContactID = chcontactID,
                                                    Email = chemail,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(cheeml);
                                                syncStatus.Text = "Syncing email recipient (" + chcontactID + ")";
                                            }

                                            changescount++;
                                        }

                                        synccount += "Total synced email recipient updates: " + (changescount - 1) + " out of " + chemailresult.Count + "\n";
                                    }
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
                            syncStatus.Text = "Getting user email data from server";

                            var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=3sEW7W";
                            string chcontentType = "application/json";
                            JObject chjson = new JObject
                            {
                                { "ContactID", contact }
                            };

                            HttpClient chclient = new HttpClient();
                            var chresponse = await chclient.PostAsync(chlink, new StringContent(chjson.ToString(), Encoding.UTF8, chcontentType));

                            if (chresponse.StatusCode == HttpStatusCode.OK)
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
                                        var chlastSync = DateTime.Parse(current_datetime);
                                        var chlastUpdated = chitem.LastUpdated;
                                        var chdeleted = chitem.Deleted;

                                        var cheml = new UserEmailTable
                                        {
                                            ContactID = chcontactID,
                                            Email = chemail,
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncProvince(host, database, contact, ipaddress, pingipaddress);
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
        
        public async void SyncProvince(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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

                            if (response.StatusCode == HttpStatusCode.OK)
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
                                        syncStatus.Text = "Syncing province update " + count + " out of " + provinceresult.Count;

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
                                            if (lastUpdated >= chgetProvince.Result[0].LastUpdated)
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
                                        }
                                        
                                        count++;
                                    }

                                    synccount += "Total synced province updates: " + (count - 1) + " out of " + provinceresult.Count + "\n";
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
                            syncStatus.Text = "Getting provinces from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=EfMv7c";
                            string contentType = "application/json";
                            JObject json = new JObject
                            {
                                { "ContactID", contact }
                            };

                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(link, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                            if (response.StatusCode == HttpStatusCode.OK)
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    SyncTown(host, database, contact, ipaddress, pingipaddress);
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
        
        public async void SyncTown(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 5000);

            if (reply.Status == IPStatus.Success)
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

                            if (response.StatusCode == HttpStatusCode.OK)
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

                                    var townresult = JsonConvert.DeserializeObject<List<TownData>>(content, settings);
                                    for (int i = 0; i < townresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing town update " + count + " out of " + townresult.Count;

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
                                            if (lastUpdated >= chgetTown.Result[0].LastUpdated)
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
                                        }

                                        count++;
                                    }

                                    synccount += "Total synced town updates: " + (count - 1) + " out of " + townresult.Count + "\n";
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
                            syncStatus.Text = "Getting town from server";

                            var twlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=F9jq3k";
                            string twcontentType = "application/json";
                            JObject twjson = new JObject
                            {
                                { "ContactID", contact }
                            };

                            HttpClient twclient = new HttpClient();
                            var twresponse = await twclient.PostAsync(twlink, new StringContent(twjson.ToString(), Encoding.UTF8, twcontentType));

                            if (twresponse.StatusCode == HttpStatusCode.OK)
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
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
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
                syncStatus.Text = "Syncing town failed. Server is unreachable.";
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
            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress, pingipaddress));
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("username", String.Empty, "private_prefs");
            Preferences.Set("password", String.Empty, "private_prefs");
            await Navigation.PopToRootAsync();
        }
    }
}