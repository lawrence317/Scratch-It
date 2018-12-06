using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using Xamarin.Forms;

namespace TBSMobile.Data
{
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

    public class SyncFunction
    {
        public static async void SyncUser(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblUser WHERE ContactID = '" + contact + "' AND Deleted != '1'";
                    var getUser = conn.QueryAsync<UserTable>(sql);
                    var resultCount = getUser.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblUser WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getUserChanges = conn.QueryAsync<UserTable>(changessql);
                        var changesresultCount = getUserChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < changesresultCount; i++)
                            {
                                try
                                {
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

                                    var crupdate_sql = "UPDATE tblUser SET LastSync='" + current_datetime + "' WHERE ContactID='" + contact + "'";
                                    await conn.ExecuteAsync(crupdate_sql);

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.IsSuccessStatusCode)
                                    {
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

                                                    var crsql = "SELECT * FROM tblUser WHERE ContactID = '" + crcontactID + "'";
                                                    var crgetUser = conn.QueryAsync<UserTable>(crsql);
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
                                                    Console.Write("Syncing user error " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing user Error " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=79MbtQ";
                                string chcontentType = "application/json";
                                JObject json = new JObject
                                {
                                    { "ContactID", contact }
                                };

                                HttpClient client = new HttpClient();
                                var chresponse = await client.PostAsync(chlink, new StringContent(json.ToString(), Encoding.UTF8, chcontentType));

                                if (chresponse.IsSuccessStatusCode)
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
                                                if (chlastUpdated > chgetUser.Result[0].LastUpdated)
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
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Syncing User Error " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
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
                                    var userresult = JsonConvert.DeserializeObject<List<UserData>>(content);
                                    for (int i = 0; i < userresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing User Error " + ex.Message);
                        }
                    }

                    SyncRetailer(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing User Error " + ex.Message);
                }
            }
        }

        public static async void SyncRetailer(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblContacts WHERE Coordinator = '" + contact + "' AND Deleted != '1'";
                    var getContacts = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getContacts.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblContacts WHERE Coordinator = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getContactsChanges = conn.QueryAsync<ContactsTable>(changessql);
                        var changesresultCount = getContactsChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
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

                                    byte[] crPhoto1Data = File.ReadAllBytes(crphoto1);
                                    string crpht1 = Convert.ToBase64String(crPhoto1Data);

                                    byte[] crPhoto2Data = File.ReadAllBytes(crphoto2);
                                    string crpht2 = Convert.ToBase64String(crPhoto2Data);

                                    byte[] crPhoto3Data = File.ReadAllBytes(crphoto3);
                                    string crpht3 = Convert.ToBase64String(crPhoto3Data);

                                    string crvid;

                                    if (!string.IsNullOrEmpty(crvideo))
                                    {
                                        byte[] crVideoData = File.ReadAllBytes(crvideo);
                                        crvid = Convert.ToBase64String(crVideoData);
                                    }
                                    else
                                    {
                                        crvid = "";
                                    }

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
                                        { "Photo1", crpht1 },
                                        { "Photo2", crpht2 },
                                        { "Photo3", crpht3 },
                                        { "Video", crvid },
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

                                    var crupdate_sql = "UPDATE tblContacts SET LastSync='" + current_datetime + "' WHERE Coordinator='" + contact + "'";
                                    await conn.ExecuteAsync(crupdate_sql);

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.IsSuccessStatusCode)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent))
                                        {
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
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write("Syncing retailer error " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing retailer Error " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
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

                                            var chsql = "SELECT * FROM tblContacts WHERE ContactID = '" + chcontactID + "'";
                                            var chgetRetailer = conn.QueryAsync<ContactsTable>(chsql);
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
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Syncing Retailer Error " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
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

                                if (!string.IsNullOrEmpty(content))
                                {
                                    var contactsresult = JsonConvert.DeserializeObject<List<ContactsData>>(content);
                                    for (int i = 0; i < contactsresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Retailer Error " + ex.Message);
                        }
                    }

                    SyncRetailerOutlet(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing Retailer Error " + ex.Message);
                }
            }
        }

        public static async void SyncRetailerOutlet(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblRetailerGroup WHERE Coordinator = '" + contact + "'";
                    var getRetailerGroup = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getRetailerGroup.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblRetailerGroup WHERE Coordinator = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getOutletChanges = conn.QueryAsync<RetailerGroupTable>(changessql);
                        var changesresultCount = getOutletChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
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

                                    var crupdate_sql = "UPDATE tblRetailerGroup SET LastSync='" + current_datetime + "' WHERE RetailerCode='" + crretailerCode + "'";
                                    await conn.ExecuteAsync(crupdate_sql);

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.IsSuccessStatusCode)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent))
                                        {
                                            var crretailerresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(crcontent);

                                            for (i = 0; i < crretailerresult.Count; i++)
                                            {
                                                try
                                                {
                                                    var critem = crretailerresult[i];
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
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write("Syncing retailer group error " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing retailer group error " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
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

                                            var chsql = "SELECT * FROM tblRetailerGroup WHERE RetailerCode = '" + chretailerCode + "'";
                                            var chgetRetailerOutlet = conn.QueryAsync<RetailerGroupTable>(chsql);
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
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Syncing Retailer Group Error " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
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
                                    var contactsresult = JsonConvert.DeserializeObject<List<RetailerGroupData>>(content);
                                    for (int i = 0; i < contactsresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Retailer Group Error " + ex.Message);
                        }
                    }

                    SyncCaf(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing Retailer Error " + ex.Message);
                }
            }
        }

        public static async void SyncCaf(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblCaf WHERE EmployeeID = '" + contact + "'";
                    var getCAF = conn.QueryAsync<CAFTable>(sql);
                    var resultCount = getCAF.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblCaf WHERE EmployeeID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getCAFChanges = conn.QueryAsync<CAFTable>(changessql);
                        var changesresultCount = getCAFChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
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

                                    byte[] crPhoto1Data = File.ReadAllBytes(crphoto1);
                                    string crpht1 = Convert.ToBase64String(crPhoto1Data);

                                    byte[] crPhoto2Data = File.ReadAllBytes(crphoto2);
                                    string crpht2 = Convert.ToBase64String(crPhoto2Data);

                                    byte[] crPhoto3Data = File.ReadAllBytes(crphoto3);
                                    string crpht3 = Convert.ToBase64String(crPhoto3Data);

                                    string crvid;

                                    if (!string.IsNullOrEmpty(crvideo))
                                    {
                                        byte[] crVideoData = File.ReadAllBytes(crvideo);
                                        crvid = Convert.ToBase64String(crVideoData);
                                    }
                                    else
                                    {
                                        crvid = "";
                                    }

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
                                        { "Photo1", crpht1 },
                                        { "Photo2", crpht2 },
                                        { "Photo3", crpht3 },
                                        { "Video", crvid },
                                        { "MobilePhoto1", crmobilePhoto1 },
                                        { "MobilePhoto2", crmobilePhoto2 },
                                        { "MobilePhoto3", crmobilePhoto3 },
                                        { "MobileVideo", crmobileVideo },
                                        { "Remarks", crremarks },
                                        { "Deleted", crdeleted },
                                        { "LastUpdated", crlastUpdated }
                                    };

                                    var crupdate_sql = "UPDATE tblCaf SET LastSync='" + current_datetime + "' WHERE EmployeeID='" + contact + "'";
                                    await conn.ExecuteAsync(crupdate_sql);

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing CAF Error " + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
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
                                    var cafresult = JsonConvert.DeserializeObject<List<CAFData>>(content);
                                    for (int i = 0; i < cafresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing CAF Error " + ex.Message);
                        }
                    }

                    SyncActivities(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing CAF Error " + ex.Message);
                }
            }
        }

        public static async void SyncActivities(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblActivity WHERE ContactID = '" + contact + "'";
                    var getAct = conn.QueryAsync<ActivityData>(sql);
                    var resultCount = getAct.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblActivity WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getActivityChanges = conn.QueryAsync<ActivityTable>(changessql);
                        var changesresultCount = getActivityChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
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

                                    var crupdate_sql = "UPDATE tblActivity SET LastSync='" + current_datetime + "' WHERE ContactID='" + contact + "'";
                                    await conn.ExecuteAsync(crupdate_sql);

                                    HttpClient client = new HttpClient();
                                    var response = await client.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing CAF Activities Error " + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
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
                                    var actresult = JsonConvert.DeserializeObject<List<ActivityData>>(content);
                                    for (int i = 0; i < actresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing CAF Error " + ex.Message);
                        }
                    }

                    SyncSubscription(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing CAF Error " + ex.Message);
                }
            }
        }

        public static async void SyncSubscription(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblSubscription WHERE ContactID = '" + contact + "'";
                    var getSub = conn.QueryAsync<SubscriptionData>(sql);
                    var resultCount = getSub.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblSubscription WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getSubscriptionChanges = conn.QueryAsync<SubscriptionTable>(changessql);
                        var changesresultCount = getSubscriptionChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
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

                                    var crupdate_sql = "UPDATE  tblSubscription SET LastSync='" + current_datetime + "' WHERE ContactID='" + contact + "'";
                                    await conn.ExecuteAsync(crupdate_sql);

                                    HttpClient crclient = new HttpClient();
                                    var crresponse = await crclient.PostAsync(crlink, new StringContent(crjson.ToString(), Encoding.UTF8, crcontentType));

                                    if (crresponse.IsSuccessStatusCode)
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
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write("Syncing subscription error " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing subscription Error " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
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

                                            var chsql = "SELECT * FROM tblSubscription WHERE ContactID = '" + chcontactID + "' AND RegistrationNumber = '" + chregistrationNumber + "'";
                                            var chgetSubscription = conn.QueryAsync<SubscriptionTable>(chsql);
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
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Syncing Subscription Error " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            var chlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=qtF5Ej";
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Subscription Error " + ex.Message);
                        }
                    }

                    SyncEmail(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing Subscription Error " + ex.Message);
                }
            }
        }

        public static async void SyncEmail(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblUserEmail WHERE ContactID = '" + contact + "'";
                    var getEmail = conn.QueryAsync<EmailData>(sql);
                    var resultCount = getEmail.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        var changessql = "SELECT * FROM tblUserEmail WHERE ContactID = '" + contact + "' AND LastUpdated > LastSync AND Deleted != '1'";
                        var getEmailChanges = conn.QueryAsync<UserEmailTable>(changessql);
                        var changesresultCount = getEmailChanges.Result.Count;

                        if (changesresultCount > 0)
                        {
                            for (int i = 0; i < resultCount; i++)
                            {
                                try
                                {
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

                                    if (crresponse.IsSuccessStatusCode)
                                    {
                                        var crcontent = await crresponse.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(crcontent))
                                        {
                                            var cremailresult = JsonConvert.DeserializeObject<List<EmailData>>(crcontent);

                                            for (i = 0; i < cremailresult.Count; i++)
                                            {
                                                try
                                                {
                                                    var critem = cremailresult[i];
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
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write("Syncing Email error " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Syncing Email Error " + ex.Message);
                                }

                                var update_sql = "UPDATE  tblUserEmail SET LastSync='" + current_datetime + "' WHERE ContactID='" + contact + "'";
                                await conn.ExecuteAsync(update_sql);
                            }
                        }
                        else
                        {
                            try
                            {
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
                                        var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent);
                                        for (int i = 0; i < chemailresult.Count; i++)
                                        {
                                            var chitem = chemailresult[i];
                                            var chcontactID = chitem.ContactID;
                                            var chemail = chitem.Email;
                                            var chlastSync = DateTime.Parse(current_datetime);
                                            var chlastUpdated = chitem.LastUpdated;
                                            var chdeleted = chitem.Deleted;

                                            var chsql = "SELECT * FROM tblUserEmail WHERE ContactID = '" + contact + "'";
                                            var chgetEmail = conn.QueryAsync<UserEmailTable>(chsql);
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
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Syncing Email Error " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
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
                                    var chemailresult = JsonConvert.DeserializeObject<List<EmailData>>(chcontent);
                                    for (int i = 0; i < chemailresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Email Error " + ex.Message);
                        }
                    }

                    SyncProvince(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing Email Error " + ex.Message);
                }
            }
        }

        public static async void SyncProvince(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblProvince";
                    var getProvince = conn.QueryAsync<ProvinceData>(sql);
                    var resultCount = getProvince.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        try
                        {
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
                                    var provinceresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content);
                                    for (int i = 0; i < provinceresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Province Error " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
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
                                    var provinceresult = JsonConvert.DeserializeObject<List<ProvinceData>>(content);
                                    for (int i = 0; i < provinceresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Province Error " + ex.Message);
                        }
                    }

                    SyncTown(host, database, contact, ipaddress, pingipaddress);
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing Province Error " + ex.Message);
                }
            }
        }

        public static async void SyncTown(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            var ping = new Ping();
            var reply = ping.Send(new IPAddress(pingipaddress), 1500);

            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblTown";
                    var getTown = conn.QueryAsync<TownData>(sql);
                    var resultCount = getTown.Result.Count;
                    var current_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    if (resultCount > 0)
                    {
                        try
                        {
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
                                    var townresult = JsonConvert.DeserializeObject<List<TownData>>(content);
                                    for (int i = 0; i < townresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Town Error " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            var link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=F9jq3k";
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
                                    var townresult = JsonConvert.DeserializeObject<List<TownData>>(content);
                                    for (int i = 0; i < townresult.Count; i++)
                                    {
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
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Syncing Town Error " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("Syncing Town Error " + ex.Message);
                }
            }
        }
    }
}
