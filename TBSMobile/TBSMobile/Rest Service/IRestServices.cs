using System;
using System.Threading.Tasks;

namespace TBSMobile.Rest_Service
{
    public interface IRestServices
    {
        Task CheckVersion(string host, string database, string domain, string apifolder, string apifile, string username, string password, Action<string> LoginStatus);

        Task FirstTimeSyncUser(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus);
        Task FirstTimeSyncSystemSerial(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncContacts(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncRetailerOutlet(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncCAF(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncCAFActivity(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncEmailRecipient(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncProvince(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task FirstTimeSyncTown(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);

        Task SyncUserClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task UpdateContacts(string contact);
        Task SyncContactsClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncContactsMedia1ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncContactsMedia2ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncContactsMedia3ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncContactsMedia4ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncRetailerOutletClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task UpdateCAF(string contact);
        Task SyncCAFClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncCAFMedia1ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncCAFMedia2ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncCAFMedia3ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncCAFMedia4ClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncCAFActivityClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncEmailRecipientClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncUserLogsClientUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);

        Task SyncUserServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncSystemSerialServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncContactsServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncRetailerOutletServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncProvinceServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task SyncTownServerUpdate(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);

        Task ReSynContacts(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task ReSyncRetailerOutlet(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task ReSyncCAF(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);
        Task ReSyncCAFActivity(string host, string database, string domain, string apifolder, string contact, Action<string>SyncStatus);

        Task OnSyncComplete(string host, string database, string domain, string contact);

        Task CheckContactsData(string contact);
        Task CheckRetailerOutletData(string contact);
        Task CheckCAFData(string contact);
        Task CheckCAFActivityData(string contact);
        Task CheckEmailRecipientData(string contact);
        Task CheckAutoSync(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus);

        Task SendCAFDirectly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others, string rapport, string stock, string replenish, string retouch, string feed, string feedback);
        Task SendCAFMedia1Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others, string rapport, string stock, string replenish, string retouch, string feed, string feedback);
        Task SendCAFMedia2Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others, string rapport, string stock, string replenish, string retouch, string feed, string feedback);
        Task SendCAFMedia3Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others, string rapport, string stock, string replenish, string retouch, string feed, string feedback);
        Task SendCAFMedia4Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string recordlog, string rekorida, string merchandizing, string tradecheck, string others, string rapport, string stock, string replenish, string retouch, string feed, string feedback);
        Task SaveCAFToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string retailercode, string employeenumber, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string date, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, string actlocation, string otherconcern, string remarks, string feedback, string recordlog);
        Task SaveRetailerOutletToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string location, string recordlog);
        Task SaveCAFActivityToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string caf, string employeenumber, string recordlog, string rekorida, string merchandizing, string tradecheck, string others, string rapport, string stock, string replenish, string retouch, string feed);

        Task SendProspectRetailerDirectly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog);
        Task SendProspectRetailerMedia1Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog);
        Task SendProspectRetailerMedia2Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog);
        Task SendProspectRetailerMedia3Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog);
        Task SendProspectRetailerMedia4Directly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog);
        Task SaveProspectRetailerToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string firstname, string middlename, string lastname, string fileas, string retailertype, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string date, string remarks, string starttime, string endtime, string photo1url, string photo2url, string photo3url, string videourl, int employee, int customer, int deleted, string recordlog);

        Task SendRetailerOutletDirectly(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog);
        Task SaveRetailerOutletToLocalDatabaseFailed(string host, string database, string domain, string apifolder, string contact, Action<string> SyncStatus, string id, string retailercode, string street, string barangay, string town, string district, string province, string country, string landmark, string telephone1, string telephone2, string mobile, string email, string deleted, string location, string recordlog);

        Task OnSendComplete(string host, string database, string domain, string contact);
        Task OnSendCompleteModal(string host, string database, string domain, string contact);
    }
}
