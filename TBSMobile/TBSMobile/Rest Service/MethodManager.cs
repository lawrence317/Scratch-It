using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TBSMobile.Rest_Service
{
    public class MethodManager
    {
        IRestServices restService;

        public MethodManager(IRestServices service)
        {
            restService = service;
        }

        public Task CheckVersion(string host, string database, string domain, string apifile, string username, string password)
        {
            return restService.CheckVersion(host, database, domain, apifile, username, password);
        }

        public Task FirstTimeSyncUser(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncUser(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncSystemSerial(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncSystemSerial(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncContacts(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncContacts(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncRetailerOutlet(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncRetailerOutlet(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncCAF(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncCAF(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncCAFActivity(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncCAFActivity(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncEmailRecipient(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncEmailRecipient(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncProvince(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncProvince(host, database, domain, contact, SyncStatus);
        }

        public Task FirstTimeSyncTown(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.FirstTimeSyncTown(host, database, domain, contact, SyncStatus);
        }

        public Task SyncUserClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncUserClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task UpdateContacts(string contact)
        {
            return restService.UpdateContacts(contact);
        }

        public Task SyncContactsClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncContactsClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncContactsMedia1ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncContactsMedia1ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncContactsMedia2ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncContactsMedia2ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncContactsMedia3ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncContactsMedia3ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncContactsMedia4ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncContactsMedia4ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncRetailerOutletClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncRetailerOutletClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task UpdateCAF(string contact)
        {
            return restService.UpdateCAF(contact);
        }

        public Task SyncCAFClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncCAFClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncCAFMedia1ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncCAFMedia1ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncCAFMedia2ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncCAFMedia2ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncCAFMedia3ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncCAFMedia3ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncCAFMedia4ClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncCAFMedia4ClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncCAFActivityClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncCAFActivityClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncEmailRecipientClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncEmailRecipientClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncUserLogsClientUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncUserLogsClientUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncUserServerUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncUserServerUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncSystemSerialServerUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncSystemSerialServerUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncContactsServerUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncContactsServerUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncRetailerOutletServerUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncRetailerOutletServerUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncProvinceServerUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncProvinceServerUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task SyncTownServerUpdate(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.SyncTownServerUpdate(host, database, domain, contact, SyncStatus);
        }

        public Task ReSynContacts(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.ReSynContacts(host, database, domain, contact, SyncStatus);
        }

        public Task ReSyncRetailerOutlet(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.ReSyncRetailerOutlet(host, database, domain, contact, SyncStatus);
        }

        public Task ReSyncCAF(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.ReSyncCAF(host, database, domain, contact, SyncStatus);
        }

        public Task ReSyncCAFActivity(string host, string database, string domain, string contact, Action<string>SyncStatus)
        {
            return restService.ReSyncCAFActivity(host, database, domain, contact, SyncStatus);
        }

        public Task OnSyncComplete(string host, string database, string domain, string contact)
        {
            return restService.OnSyncComplete(host, database, domain, contact);
        }

        public Task CheckContactsData(string host, string database, string domain, string contact)
        {
            return restService.CheckContactsData(host, database, domain, contact);
        }

        public Task CheckRetailerOutletData(string host, string database, string domain, string contact)
        {
            return restService.CheckRetailerOutletData(host, database, domain, contact);
        }

        public Task CheckCAFData(string host, string database, string domain, string contact)
        {
            return restService.CheckCAFData(host, database, domain, contact);
        }

        public Task CheckCAFActivityData(string host, string database, string domain, string contact)
        {
            return restService.CheckCAFActivityData(host, database, domain, contact);
        }

        public Task CheckEmailRecipientData(string host, string database, string domain, string contact)
        {
            return restService.CheckEmailRecipientData(host, database, domain, contact);
        }

        public Task CheckAutoSync(string host, string database, string domain, string contact, Action<string> SyncStatus)
        {
            return restService.CheckAutoSync(host, database, domain, contact, SyncStatus);
        }

        public Task SendCAFDirectly(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            return restService.SendCAFDirectly(host, database, domain, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
        }

        public Task SendCAFMedia1Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            return restService.SendCAFMedia1Directly(host, database, domain, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
        }

        public Task SendCAFMedia2Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            return restService.SendCAFMedia2Directly(host, database, domain, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
        }

        public Task SendCAFMedia3Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            return restService.SendCAFMedia3Directly(host, database, domain, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
        }

        public Task SendCAFMedia4Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            return restService.SendCAFMedia4Directly(host, database, domain, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
        }

        public Task OnSendComplete(string host, string database, string domain, string contact)
        {
            return restService.OnSendComplete(host, database, domain, contact);
        }

        public Task OnSendCompleteModal(string host, string database, string domain, string contact)
        {
            return restService.OnSendCompleteModal(host, database, domain, contact);
        }

        public Task SaveCAFToLocalDatabaseFailed(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog)
        {
            return restService.SaveCAFToLocalDatabaseFailed(host, database, domain, contact, SyncStatus, caf, retailercode, employeenumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, starttime, endtime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
        }

        public Task SaveRetailerOutletToLocalDatabaseFailed(string host, string database, string domain, string contact, Action<string> SyncStatus, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string recordlog)
        {
            return restService.SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, contact, SyncStatus, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
        }

        public Task SaveCAFActivityToLocalDatabaseFailed(string host, string database, string domain, string contact, Action<string> SyncStatus, string caf, string employeenumber, string recordlog, string rekorida, string merchandizing, string tradecheck, string others)
        {
            return restService.SaveCAFActivityToLocalDatabaseFailed(host, database, domain, contact, SyncStatus, caf, employeenumber, recordlog, rekorida, merchandizing, tradecheck, others);
        }

        public Task SendProspectRetailerDirectly(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            return restService.SendProspectRetailerDirectly(host, database, domain, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
        }

        public Task SendProspectRetailerMedia1Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            return restService.SendProspectRetailerMedia1Directly(host, database, domain, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
        }

        public Task SendProspectRetailerMedia2Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            return restService.SendProspectRetailerMedia2Directly(host, database, domain, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
        }

        public Task SendProspectRetailerMedia3Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            return restService.SendProspectRetailerMedia3Directly(host, database, domain, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
        }

        public Task SendProspectRetailerMedia4Directly(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            return restService.SendProspectRetailerMedia4Directly(host, database, domain, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
        }

        public Task SaveProspectRetailerToLocalDatabaseFailed(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog)
        {
            return restService.SaveProspectRetailerToLocalDatabaseFailed(host, database, domain, contact, SyncStatus, id, firstname, middlename, lastname, fileas, retailertype, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, starttime, endtime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
        }

        public Task SendRetailerOutletDirectly(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog)
        {
            return restService.SendRetailerOutletDirectly(host, database, domain, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
        }

        public Task SaveRetailerOutletToLocalDatabaseFailed(string host, string database, string domain, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog)
        {
            return restService.SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, contact, SyncStatus, id, retailercode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
        }
    }
}
