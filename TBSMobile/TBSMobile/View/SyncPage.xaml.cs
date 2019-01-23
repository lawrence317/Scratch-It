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
            public string UserPassword { get; set; }
            public string UserType { get; set; }
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
            public string ContactType { get; set; }
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
            public string Coordinator { get; set; }
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
            public string Coordinator { get; set; }
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
            public string Activity { get; set; }
            public int ActivitySwitch { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class SubscriptionData
        {
            public string RegistrationNumber { get; set; }
            public string ContactID { get; set; }
            public string NoOfDays { get; set; }
            public string InputDate { get; set; }
            public string ExpirationDate { get; set; }
            public string ProductKey { get; set; }
            public DateTime LastSync { get; set; }
            public int Deleted { get; set; }
            public DateTime LastUpdated { get; set; }
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
                    syncStatus.Text = "Initializing user data sync";

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
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Checking client updates";

                                    var result = getUserChanges.Result[i];
                                    var cruserID = result.UserID;
                                    var cruserPassword = result.UserPassword;
                                    var cruserStatus = result.UserStatus;
                                    var cruserType = result.UserType;
                                    var crdeleted = result.Deleted;
                                    var crlastUpdated = result.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=LX7swp";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "ContactID", contact },
                                        { "UserID", cruserID },
                                        { "UserPassword", cruserPassword },
                                        { "UserStatus", cruserStatus },
                                        { "UserType", cruserType },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        await conn.QueryAsync<UserTable>("UPDATE tblUser SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);

                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent))
                                        {
                                            var cruserresult = JsonConvert.DeserializeObject<List<UserData>>(crcontent);

                                            for (i = 0; i < cruserresult.Count; i++)
                                            {
                                                try
                                                {
                                                    var critem = cruserresult[i];
                                                    var crcontactID = critem.ContactID;
                                                    var cruID = critem.UserID;
                                                    var cruPassword = critem.UserPassword;
                                                    var cruType = critem.UserType;
                                                    var cruStatus = critem.UserStatus;
                                                    var crlSync = DateTime.Parse(current_datetime);
                                                    var crlUpdated = critem.LastUpdated;
                                                    var crdltd = critem.Deleted;
                                                    
                                                    var crgetUser = conn.QueryAsync<UserTable>("SELECT * FROM tblUser WHERE ContactID = ?", crcontactID);
                                                    var crresultCount = crgetUser.Result.Count;

                                                    if (crresultCount > 0)
                                                    {
                                                        if (crlastUpdated > crgetUser.Result[0].LastUpdated)
                                                        {
                                                            var chuser = new UserTable
                                                            {
                                                                ContactID = crcontactID,
                                                                UserID = cruserID,
                                                                UserPassword = cruserPassword,
                                                                UserType = cruserType,
                                                                UserStatus = cruserStatus,
                                                                LastSync = crlSync,
                                                                LastUpdated = crlastUpdated,
                                                                Deleted = crdltd
                                                            };

                                                            await conn.InsertOrReplaceAsync(chuser);
                                                            syncStatus.Text = "Syncing user updates of " + cruserID;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var cheuser = new UserTable
                                                        {
                                                            ContactID = crcontactID,
                                                            UserID = cruserID,
                                                            UserPassword = cruserPassword,
                                                            UserType = cruserType,
                                                            UserStatus = cruserStatus,
                                                            LastSync = crlSync,
                                                            LastUpdated = crlastUpdated,
                                                            Deleted = crdltd
                                                        };

                                                        await conn.InsertOrReplaceAsync(cheuser);
                                                        syncStatus.Text = "Syncing new user (" + cruserID + ")";
                                                    }

                                                    var cruser = new UserTable
                                                    {
                                                        ContactID = crcontactID,
                                                        UserID = cruID,
                                                        UserPassword = cruPassword,
                                                        UserType = cruType,
                                                        UserStatus = cruStatus,
                                                        LastSync = crlSync,
                                                        Deleted = crdltd,
                                                        LastUpdated = crlUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(cruser);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Crashes.TrackError(ex);
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
                        }
                        else
                        {
                            try
                            {
                                syncStatus.Text = "Checking server updates";

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
                                        var chuserresult = JsonConvert.DeserializeObject<List<UserData>>(chcontent);
                                        for (int i = 0; i < chuserresult.Count; i++)
                                        {
                                            var item = chuserresult[i];
                                            var chcontactID = item.ContactID;
                                            var chuserID = item.UserID;
                                            var chuserPassword = item.UserPassword;
                                            var chuserType = item.UserType;
                                            var chuserStatus = item.UserStatus;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = item.LastUpdated;
                                            var chdltd = item.Deleted;

                                            var chsql = "SELECT * FROM tblUser WHERE ContactID = '" + chcontactID + "'";
                                            var chgetUser = conn.QueryAsync<UserTable>(chsql);
                                            var chresultCount = chgetUser.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                               if(chlastUpdated > chgetUser.Result[0].LastUpdated)
                                               {
                                                    var chuser = new UserTable
                                                    {
                                                        ContactID = chcontactID,
                                                        UserID = chuserID,
                                                        UserPassword = chuserPassword,
                                                        UserType = chuserType,
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
                                                    UserPassword = chuserPassword,
                                                    UserType = chuserType,
                                                    UserStatus = chuserStatus,
                                                    LastSync = chlastSync,
                                                    LastUpdated = chlastUpdated,
                                                    Deleted = chdltd
                                                };

                                                await conn.InsertOrReplaceAsync(cheuser);
                                                syncStatus.Text = "Syncing new user (" + chuserID + ")";
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
                                    var userresult = JsonConvert.DeserializeObject<List<UserData>>(content);
                                    for (int i = 0; i < userresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing user " + count + " out of " + userresult.Count;

                                        var item = userresult[i];
                                        var contactID = item.ContactID;
                                        var userID = item.UserID;
                                        var userPassword = item.UserPassword;
                                        var userType = item.UserType;
                                        var userStatus = item.UserStatus;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var chuser = new UserTable
                                        {
                                            ContactID = contactID,
                                            UserID = userID,
                                            UserPassword = userPassword,
                                            UserType = userType,
                                            UserStatus = userStatus,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(chuser);

                                        count++;
                                    }
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
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var getContacts = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Coordinator = ? AND Deleted != '1'", contact);
                    var resultCount = getContacts.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if(resultCount > 0)
                    {
                        int count = 1;
                        
                        var getContactsChanges = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE Coordinator = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
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
                                    var crcontactType = crresult.ContactType;
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
                                    var crcoordinator = crresult.Coordinator;
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
                                        { "ContactType", crcontactType },
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
                                        { "Coordinator", crcoordinator },
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

                                            var crretailerresult = JsonConvert.DeserializeObject<List<ContactsData>>(crcontent);

                                            for (i = 0; i < crretailerresult.Count; i++)
                                            {
                                                try
                                                {
                                                    var critem = crretailerresult[i];
                                                    var crcID = critem.ContactID;
                                                    var crfAs = critem.FileAs;
                                                    var crfName = critem.FirstName;
                                                    var crmName = critem.MiddleName;
                                                    var crlName = critem.LastName;
                                                    var crpos = critem.Position;
                                                    var crcomp = critem.Company;
                                                    var crcompID = critem.CompanyID;
                                                    var crcType = critem.ContactType;
                                                    var crrType = critem.RetailerType;
                                                    var crpStreet = critem.PresStreet;
                                                    var crpBarangay = critem.PresBarangay;
                                                    var crpDistrict = critem.PresDistrict;
                                                    var crpTown = critem.PresTown;
                                                    var crpProvince = critem.PresProvince;
                                                    var crpCountry = critem.PresCountry;
                                                    var crlndmark = critem.Landmark;
                                                    var crtel1 = critem.Telephone1;
                                                    var crtel2 = critem.Telephone2;
                                                    var crmob = critem.Mobile;
                                                    var creml = critem.Email;
                                                    var crpt1 = critem.Photo1;
                                                    var crpt2 = critem.Photo2;
                                                    var crpt3 = critem.Photo3;
                                                    var crvd = critem.Video;
                                                    var crmpt1 = critem.MobilePhoto1;
                                                    var crmpt2 = critem.MobilePhoto2;
                                                    var crmpt3 = critem.MobilePhoto3;
                                                    var crmvd = critem.MobileVideo;
                                                    var cremp = critem.Employee;
                                                    var crcust = critem.Customer;
                                                    var crcoord = critem.Coordinator;
                                                    var crlSync = DateTime.Parse(current_datetime);
                                                    var crlUpdated = critem.LastUpdated;
                                                    var crdltd = critem.Deleted;

                                                    var crsql = "SELECT * FROM tblContacts WHERE ContactID = '" + crcID + "'";
                                                    var crgetRetailer = conn.QueryAsync<ContactsTable>(crsql);
                                                    var crresultCount = crgetRetailer.Result.Count;

                                                    if (crresultCount > 0)
                                                    {
                                                        if (crlastUpdated > crgetRetailer.Result[0].LastUpdated)
                                                        {
                                                            var crretailer = new ContactsTable
                                                            {
                                                                ContactID = crcID,
                                                                FileAs = crfAs,
                                                                FirstName = crfName,
                                                                MiddleName = crmName,
                                                                LastName = crlName,
                                                                Position = crpos,
                                                                Company = crcomp,
                                                                CompanyID = crcompID,
                                                                ContactType = crcType,
                                                                RetailerType = crrType,
                                                                PresStreet = crpStreet,
                                                                PresBarangay = crpBarangay,
                                                                PresDistrict = crpDistrict,
                                                                PresTown = crpTown,
                                                                PresProvince = crpProvince,
                                                                PresCountry = crpCountry,
                                                                Landmark = crlndmark,
                                                                Telephone1 = crtel1,
                                                                Telephone2 = crtel2,
                                                                Mobile = crmob,
                                                                Email = creml,
                                                                Photo1 = crpt1,
                                                                Photo2 = crpt2,
                                                                Photo3 = crpt3,
                                                                Video = crvd,
                                                                MobilePhoto1 = crmpt1,
                                                                MobilePhoto2 = crmpt2,
                                                                MobilePhoto3 = crmpt3,
                                                                MobileVideo = crmvd,
                                                                Employee = cremp,
                                                                Customer = crcust,
                                                                Coordinator = crcoord,
                                                                LastSync = crlSync,
                                                                Deleted = crdltd,
                                                                LastUpdated = crlUpdated
                                                            };

                                                            await conn.InsertOrReplaceAsync(crretailer);
                                                            syncStatus.Text = "Syncing retailer updates of " + crfileAs;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var creretailer = new ContactsTable
                                                        {
                                                            ContactID = crcID,
                                                            FileAs = crfAs,
                                                            FirstName = crfName,
                                                            MiddleName = crmName,
                                                            LastName = crlName,
                                                            Position = crpos,
                                                            Company = crcomp,
                                                            CompanyID = crcompID,
                                                            ContactType = crcType,
                                                            RetailerType = crrType,
                                                            PresStreet = crpStreet,
                                                            PresBarangay = crpBarangay,
                                                            PresDistrict = crpDistrict,
                                                            PresTown = crpTown,
                                                            PresProvince = crpProvince,
                                                            PresCountry = crpCountry,
                                                            Landmark = crlndmark,
                                                            Telephone1 = crtel1,
                                                            Telephone2 = crtel2,
                                                            Mobile = crmob,
                                                            Email = creml,
                                                            Photo1 = crpt1,
                                                            Photo2 = crpt2,
                                                            Photo3 = crpt3,
                                                            Video = crvd,
                                                            MobilePhoto1 = crmpt1,
                                                            MobilePhoto2 = crmpt2,
                                                            MobilePhoto3 = crmpt3,
                                                            MobileVideo = crmvd,
                                                            Employee = cremp,
                                                            Customer = crcust,
                                                            Coordinator = crcoord,
                                                            LastSync = crlSync,
                                                            Deleted = crdltd,
                                                            LastUpdated = crlUpdated
                                                        };

                                                        await conn.InsertOrReplaceAsync(creretailer);
                                                        syncStatus.Text = "Syncing new retailer (" + crfileAs + ")";
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
                                syncStatus.Text = "Checking server updates";

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
                                        var chcontactsresult = JsonConvert.DeserializeObject<List<ContactsData>>(chcontent);
                                        for (int i = 0; i < chcontactsresult.Count; i++)
                                        {
                                            var chitem = chcontactsresult[i];
                                            var chcontactID = chitem.ContactID;
                                            var chfileAs = chitem.FileAs;
                                            var chfirstName = chitem.FirstName;
                                            var chmiddleName = chitem.MiddleName;
                                            var chlastName = chitem.LastName;
                                            var chposition = chitem.Position;
                                            var chcompany = chitem.Company;
                                            var chcompanyID = chitem.CompanyID;
                                            var chcontactType = chitem.ContactType;
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
                                            var chcoordinator = chitem.Coordinator;
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
                                                        ContactType = chcontactType,
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
                                                        Coordinator = chcoordinator,
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
                                                    ContactType = chcontactType,
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
                                                    Coordinator = chcoordinator,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(cheretailer);
                                                syncStatus.Text = "Syncing new retailer (" + chfileAs + ")";
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
                                    var contactsresult = JsonConvert.DeserializeObject<List<ContactsData>>(content);
                                    for (int i = 0; i < contactsresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing retailer changes " + count + " out of " + contactsresult.Count;

                                        var item = contactsresult[i];
                                        var contactID = item.ContactID;
                                        var fileAs = item.FileAs;
                                        var firstName = item.FirstName;
                                        var middleName = item.MiddleName;
                                        var lastName = item.LastName;
                                        var position = item.Position;
                                        var company = item.Company;
                                        var companyID = item.CompanyID;
                                        var contactType = item.ContactType;
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
                                        var coordinator = item.Coordinator;
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
                                            ContactType = contactType,
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
                                            Coordinator = coordinator,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(retailer);

                                        count++;
                                    }
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
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getRetailerGroup = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Coordinator = ?", contact);
                    var resultCount = getRetailerGroup.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if(resultCount > 0)
                    {
                        int count = 1;
                        
                        var getOutletChanges = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Coordinator = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getOutletChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
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
                                    var crcoordinator = crresult.Coordinator;
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
                                        { "Coordinator", crcoordinator },
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
                                            var crretailerresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(crcontent);

                                            for (int a = 0; a < crretailerresult.Count; a++)
                                            {
                                                try
                                                {
                                                    var critem = crretailerresult[a];
                                                    var crrCode = critem.RetailerCode;
                                                    var crcID = critem.ContactID;
                                                    var crpStreet = critem.PresStreet;
                                                    var crpBarangay = critem.PresBarangay;
                                                    var crpDistrict = critem.PresDistrict;
                                                    var crpTown = critem.PresTown;
                                                    var crpProvince = critem.PresProvince;
                                                    var crpCountry = critem.PresCountry;
                                                    var crtel1 = critem.Telephone1;
                                                    var crtel2 = critem.Telephone2;
                                                    var crmob = critem.Mobile;
                                                    var creml = critem.Email;
                                                    var crlmark = critem.Landmark;
                                                    var crgps = critem.GPSCoordinates;
                                                    var crcoord = critem.Coordinator;
                                                    var crlSync = DateTime.Parse(current_datetime);
                                                    var crlUpdated = critem.LastUpdated;
                                                    var crdltd = critem.Deleted;

                                                    var crsql = "SELECT * FROM tblRetailerGroup WHERE RetailerCode = '" + crrCode + "'";
                                                    var crgetRetailerOutlet = conn.QueryAsync<RetailerGroupTable>(crsql);
                                                    var crresultCount = crgetRetailerOutlet.Result.Count;

                                                    if (crresultCount > 0)
                                                    {
                                                        if (crlastUpdated > crgetRetailerOutlet.Result[0].LastUpdated)
                                                        {
                                                            var crretailer = new RetailerGroupTable
                                                            {
                                                                RetailerCode = crrCode,
                                                                ContactID = crcID,
                                                                PresStreet = crpStreet,
                                                                PresBarangay = crpBarangay,
                                                                PresDistrict = crpDistrict,
                                                                PresTown = crpTown,
                                                                PresProvince = crpProvince,
                                                                PresCountry = crpCountry,
                                                                Telephone1 = crtel1,
                                                                Telephone2 = crtel2,
                                                                Mobile = crmob,
                                                                Email = creml,
                                                                Landmark = crlmark,
                                                                GPSCoordinates = crgps,
                                                                Coordinator = crcoord,
                                                                LastSync = crlSync,
                                                                Deleted = crdltd,
                                                                LastUpdated = crlUpdated
                                                            };

                                                            await conn.InsertOrReplaceAsync(crretailer);
                                                            syncStatus.Text = "Syncing retailer outlet updates of " + crrCode;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var creretailer = new RetailerGroupTable
                                                        {
                                                            RetailerCode = crrCode,
                                                            ContactID = crcID,
                                                            PresStreet = crpStreet,
                                                            PresBarangay = crpBarangay,
                                                            PresDistrict = crpDistrict,
                                                            PresTown = crpTown,
                                                            PresProvince = crpProvince,
                                                            PresCountry = crpCountry,
                                                            Telephone1 = crtel1,
                                                            Telephone2 = crtel2,
                                                            Mobile = crmob,
                                                            Email = creml,
                                                            Landmark = crlmark,
                                                            GPSCoordinates = crgps,
                                                            Coordinator = crcoord,
                                                            LastSync = crlSync,
                                                            Deleted = crdltd,
                                                            LastUpdated = crlUpdated
                                                        };

                                                        await conn.InsertOrReplaceAsync(creretailer);
                                                        syncStatus.Text = "Syncing new retailer outlet (" + crrCode + ")";
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Crashes.TrackError(ex);
                                                }
                                            }

                                            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET LastSync = ? WHERE RetailerCode = ?", DateTime.Parse(current_datetime), crretailerCode);
                                            count++;
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
                                        var chcontactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(chcontent);
                                        for (int i = 0; i < chcontactsresult.Count; i++)
                                        {
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
                                            var chcoordinator = chitem.Coordinator;
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
                                                        Coordinator = chcoordinator,
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
                                                    Coordinator = chcoordinator,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(cheretailer);
                                                syncStatus.Text = "Syncing new retailer outlet (" + chretailerCode + ")";
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

                                    var contactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content);
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
                                        var coordinator = item.Coordinator;
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
                                            Coordinator = coordinator,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(retailer);

                                        count++;
                                    }
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
                                    syncStatus.Text = "Sending coordinator activity changes to server " + count + " out of " + changesresultCount;

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
                                    var cafresult = JsonConvert.DeserializeObject<List<CAFData>>(content);
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
                                    syncStatus.Text = "Sending coordinator activity changes to server " + count + " out of " + changesresultCount;

                                    var crresult = getActivityChanges.Result[i];
                                    var crcafNo = crresult.CAFNo;
                                    var crcontactId = crresult.ContactID;
                                    var cractivity = crresult.Activity;
                                    var cractivitySwitch = crresult.ActivitySwitch;
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=b7Q9XU";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "CAFNo", crcafNo },
                                        { "ContactID", crcontactId },
                                        { "Activity", cractivity },
                                        { "ActivitySwitch", cractivitySwitch },
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

                                    var actresult = JsonConvert.DeserializeObject<List<ActivityData>>(content);
                                    for (int i = 0; i < actresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing activity " + count + " out of " + actresult.Count;

                                        var item = actresult[i];
                                        var cafNo = item.CAFNo;
                                        var contactId = item.ContactID;
                                        var activity = item.Activity;
                                        var activitySwitch = item.ActivitySwitch;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

                                        var act = new ActivityTable
                                        {
                                            CAFNo = cafNo,
                                            ContactID = contactId,
                                            Activity = activity,
                                            ActivitySwitch = activitySwitch,
                                            LastSync = lastSync,
                                            Deleted = deleted,
                                            LastUpdated = lastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(act);

                                        count++;
                                    }
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
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();
                    
                    var getSub = conn.QueryAsync<SubscriptionData>("SELECT * FROM tblSubscription WHERE ContactID = ?", contact);
                    var resultCount = getSub.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    if (resultCount > 0)
                    {
                        var getSubscriptionChanges = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE ContactID = ? AND LastUpdated > LastSync AND Deleted != '1'", contact);
                        var changesresultCount = getSubscriptionChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
                                    syncStatus.Text = "Getting device registration data from local database";

                                    var crresult = getSubscriptionChanges.Result[i];
                                    var crregistrationNumber = crresult.RegistrationNumber;
                                    var crcontactID = crresult.ContactID;
                                    var crnoOfDays = crresult.NoOfDays;
                                    var crinputDate = crresult.InputDate;
                                    var crexpirationDate = crresult.ExpirationDate;
                                    var crproductKey = crresult.ProductKey;
                                    var crdeleted = crresult.Deleted;
                                    var crlastUpdated = crresult.LastUpdated;

                                    var crlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=59EkmJ";
                                    string crcontentType = "application/json";
                                    JObject crjson = new JObject
                                    {
                                        { "ContactID", crcontactID },
                                        { "RegistrationNumber", crregistrationNumber },
                                        { "NoOfDays", crnoOfDays },
                                        { "InputDate", crinputDate },
                                        { "ExpirationDate", crexpirationDate },
                                        { "ProductKey", crproductKey },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent))
                                        {
                                            var crsubresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(crcontent);

                                            for (i = 0; i < crsubresult.Count; i++)
                                            {
                                                try
                                                {
                                                    var critem = crsubresult[i];
                                                    var crregNumber = critem.RegistrationNumber;
                                                    var crcID = critem.ContactID;
                                                    var crnoofDays = critem.NoOfDays;
                                                    var criDate = critem.InputDate;
                                                    var crexpDate = critem.ExpirationDate;
                                                    var crpKey = critem.ProductKey;
                                                    var crlSync = DateTime.Parse(current_datetime);
                                                    var crlUpdated = critem.LastUpdated;
                                                    var crdltd = critem.Deleted;

                                                    var crsql = "SELECT * FROM tblSubscription WHERE ContactID = '" + crcontactID + "' AND RegistrationNumber = '" + crregNumber + "'";
                                                    var crgetSubscription = conn.QueryAsync<SubscriptionTable>(crsql);
                                                    var crresultCount = crgetSubscription.Result.Count;

                                                    if (crresultCount > 0)
                                                    {
                                                        if (crlastUpdated > crgetSubscription.Result[0].LastUpdated)
                                                        {
                                                            var crsub = new SubscriptionTable
                                                            {
                                                                ContactID = crcID,
                                                                RegistrationNumber = crregNumber,
                                                                NoOfDays = crnoofDays,
                                                                InputDate = criDate,
                                                                ExpirationDate = crexpDate,
                                                                ProductKey = crpKey,
                                                                LastSync = crlSync,
                                                                Deleted = crdltd,
                                                                LastUpdated = crlUpdated
                                                            };

                                                            await conn.InsertOrReplaceAsync(crsub);
                                                            syncStatus.Text = "Syncing subscription updates of " + crregNumber;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var cresub = new SubscriptionTable
                                                        {
                                                            ContactID = crcID,
                                                            RegistrationNumber = crregNumber,
                                                            NoOfDays = crnoofDays,
                                                            InputDate = criDate,
                                                            ExpirationDate = crexpDate,
                                                            ProductKey = crpKey,
                                                            LastSync = crlSync,
                                                            Deleted = crdltd,
                                                            LastUpdated = crlUpdated
                                                        };

                                                        await conn.InsertOrReplaceAsync(cresub);
                                                        syncStatus.Text = "Syncing new subscription (" + crregNumber + ")";
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Crashes.TrackError(ex);
                                                }
                                            }
                                        }

                                        await conn.QueryAsync<SubscriptionTable>("UPDATE tblSubscription SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);
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
                                        var chsubresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(chcontent);
                                        for (int i = 0; i < chsubresult.Count; i++)
                                        {
                                            var chitem = chsubresult[i];
                                            var chregistrationNumber = chitem.RegistrationNumber;
                                            var chcontactID = chitem.ContactID;
                                            var chnoOfDays = chitem.NoOfDays;
                                            var chinputDate = chitem.InputDate;
                                            var chexpirationDate = chitem.ExpirationDate;
                                            var chproductKey = chitem.ProductKey;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;
                                            
                                            var chgetSubscription = conn.QueryAsync<SubscriptionTable>("SELECT * FROM tblSubscription WHERE ContactID = ? AND RegistrationNumber = ?", chcontactID, chregistrationNumber);
                                            var chresultCount = chgetSubscription.Result.Count;

                                            if (chresultCount > 0)
                                            {
                                                if (chlastUpdated > chgetSubscription.Result[0].LastUpdated)
                                                {
                                                    var chsub = new SubscriptionTable
                                                    {
                                                        RegistrationNumber = chregistrationNumber,
                                                        ContactID = chcontactID,
                                                        NoOfDays = chnoOfDays,
                                                        InputDate = chinputDate,
                                                        ExpirationDate = chexpirationDate,
                                                        ProductKey = chproductKey,
                                                        LastSync = chlastSync,
                                                        Deleted = chdeleted,
                                                        LastUpdated = chlastUpdated
                                                    };

                                                    await conn.InsertOrReplaceAsync(chsub);
                                                    syncStatus.Text = "Syncing subscription updates of " + chregistrationNumber;
                                                }
                                            }
                                            else
                                            {
                                                var chesub = new SubscriptionTable
                                                {
                                                    RegistrationNumber = chregistrationNumber,
                                                    ContactID = chcontactID,
                                                    NoOfDays = chnoOfDays,
                                                    InputDate = chinputDate,
                                                    ExpirationDate = chexpirationDate,
                                                    ProductKey = chproductKey,
                                                    LastSync = chlastSync,
                                                    Deleted = chdeleted,
                                                    LastUpdated = chlastUpdated
                                                };

                                                await conn.InsertOrReplaceAsync(chesub);
                                                syncStatus.Text = "Syncing new subscription (" + chregistrationNumber + ")";
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
                    }
                    else
                    {
                        try
                        {
                            syncStatus.Text = "Getting device registration data from server";

                            var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=qtF5Ej";
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
                                    int count = 1;

                                    var chsubresult = JsonConvert.DeserializeObject<List<SubscriptionData>>(chcontent);
                                    for (int i = 0; i < chsubresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing device registration " + count + " out of " + chsubresult.Count;

                                        var chitem = chsubresult[i];
                                        var chregistrationNumber = chitem.RegistrationNumber;
                                        var chcontactID = chitem.ContactID;
                                        var chnoOfDays = chitem.NoOfDays;
                                        var chinputDate = chitem.InputDate;
                                        var chexpirationDate = chitem.ExpirationDate;
                                        var chproductKey = chitem.ProductKey;
                                        var chlastSync = DateTime.Parse(current_datetime);
                                        var chlastUpdated = chitem.LastUpdated;
                                        var chdeleted = chitem.Deleted;

                                        var chsub = new SubscriptionTable
                                        {
                                            RegistrationNumber = chregistrationNumber,
                                            ContactID = chcontactID,
                                            NoOfDays = chnoOfDays,
                                            InputDate = chinputDate,
                                            ExpirationDate = chexpirationDate,
                                            ProductKey = chproductKey,
                                            LastSync = chlastSync,
                                            Deleted = chdeleted,
                                            LastUpdated = chlastUpdated
                                        };

                                        await conn.InsertOrReplaceAsync(chsub);

                                        count++;
                                    }
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
                                    syncStatus.Text = "Sending email changes to server " + count + " out of " + changesresultCount;

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
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent))
                                        {
                                            var cremailresult = JsonConvert.DeserializeObject<List<EmailData>>(crcontent);

                                            for (int a = 0; a < cremailresult.Count; a++)
                                            {
                                                try
                                                {
                                                    var critem = cremailresult[a];
                                                    var crcID = critem.ContactID;
                                                    var creml = critem.Email;
                                                    var crlSync = DateTime.Parse(current_datetime);
                                                    var crlUpdated = critem.LastUpdated;
                                                    var crdltd = critem.Deleted;

                                                    var crsql = "SELECT * FROM tblUserEmail WHERE ContactID = '" + contact + "'";
                                                    var crgetEmail = conn.QueryAsync<UserEmailTable>(crsql);
                                                    var crresultCount = crgetEmail.Result.Count;

                                                    if (crresultCount > 0)
                                                    {
                                                        if (crlUpdated > crgetEmail.Result[0].LastUpdated)
                                                        {
                                                            var crel = new UserEmailTable
                                                            {
                                                                ContactID = crcID,
                                                                Email = creml,
                                                                LastSync = crlSync,
                                                                Deleted = crdltd,
                                                                LastUpdated = crlUpdated
                                                            };

                                                            await conn.InsertOrReplaceAsync(crel);
                                                            syncStatus.Text = "Syncing user email updates of " + crcID;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var creel = new UserEmailTable
                                                        {
                                                            ContactID = crcID,
                                                            Email = creml,
                                                            LastSync = crlSync,
                                                            Deleted = crdltd,
                                                            LastUpdated = crlUpdated
                                                        };

                                                        await conn.InsertOrReplaceAsync(creel);
                                                        syncStatus.Text = "Syncing new user email (" + crcID + ")";
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Crashes.TrackError(ex);
                                                }
                                            }
                                        }
                                        
                                        await conn.QueryAsync<UserEmailTable>("UPDATE tblUserEmail SET LastSync = ? WHERE ContactID = ?", DateTime.Parse(current_datetime), contact);
                                        count++;
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
                                        var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent);
                                        for (int i = 0; i < chemailresult.Count; i++)
                                        {
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
                                                if (chlastUpdated > chgetEmail.Result[0].LastUpdated)
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
                                                    syncStatus.Text = "Syncing user email updates of " + chcontactID;
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
                                                syncStatus.Text = "Syncing new user email (" + chcontactID + ")";
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

                                    var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent);
                                    for (int i = 0; i < chemailresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing user email " + count + " out of " + chemailresult.Count;

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
                            syncStatus.Text = "Getting provinces from server";

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
                                    var provinceresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content);
                                    for (int i = 0; i < provinceresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing provinces " + count + " out of " + provinceresult.Count;

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
                                    var provinceresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content);
                                    for (int i = 0; i < provinceresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing provinces " + count + " out of " + provinceresult.Count;

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
                            syncStatus.Text = "Getting towns from server";

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

                                    var townresult = JsonConvert.DeserializeObject<List<TownData>>(content);
                                    for (int i = 0; i < townresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing towns " + count + " out of " + townresult.Count;

                                        var item = townresult[i];
                                        var townID = item.TownID;
                                        var provinceID = item.ProvinceID;
                                        var town = item.Town;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

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

                                        count++;
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
                            syncStatus.Text = "Getting towns from server";

                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=F9jq3k";
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

                                    var townresult = JsonConvert.DeserializeObject<List<TownData>>(content);
                                    for (int i = 0; i < townresult.Count; i++)
                                    {
                                        syncStatus.Text = "Syncing towns " + count + " out of " + townresult.Count;

                                        var item = townresult[i];
                                        var townID = item.TownID;
                                        var provinceID = item.ProvinceID;
                                        var town = item.Town;
                                        var lastSync = DateTime.Parse(current_datetime);
                                        var lastUpdated = item.LastUpdated;
                                        var deleted = item.Deleted;

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

                                        count++;
                                    }
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
            syncStatus.Text = "Data sync successfully";
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