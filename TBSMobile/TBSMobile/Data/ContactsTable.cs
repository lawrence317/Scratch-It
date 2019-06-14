using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblContacts")]
    public class ContactsTable
    {
        [PrimaryKey, MaxLength(50)]
        public string ContactID { get; set; }
        [MaxLength(300)]
        public string FileAs { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string MiddleName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Position { get; set; }
        [MaxLength(100)]
        public string Company { get; set; }
        [MaxLength(100)]
        public string CompanyID { get; set; }
        [MaxLength(100)]
        public string RetailerType { get; set; }
        [MaxLength(300)]
        public string PresStreet { get; set; }
        [MaxLength(90)]
        public string PresBarangay { get; set; }
        [MaxLength(90)]
        public string PresDistrict { get; set; }
        [MaxLength(90)]
        public string PresTown { get; set; }
        [MaxLength(90)]
        public string PresProvince { get; set; }
        [MaxLength(90)]
        public string PresCountry { get; set; }
        [MaxLength(500)]
        public string Landmark { get; set; }
        [MaxLength(500)]
        public string CustomerRemarks { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [MaxLength(30)]
        public string Telephone1 { get; set; }
        [MaxLength(30)]
        public string Telephone2 { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Video { get; set; }
        public string MobilePhoto1 { get; set; }
        public string MobilePhoto2 { get; set; }
        public string MobilePhoto3 { get; set; }
        public string MobileVideo { get; set; }
        [MaxLength(2)]
        public int Employee { get; set; }
        [MaxLength(2)]
        public int Customer { get; set; }
        [MaxLength(100)]
        public string RecordLog { get; set; }
        [MaxLength(50)]
        public string Supervisor { get; set; }
        public int Existed { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public int ThisSynced { get; set; }
        public int Media1Synced { get; set; }
        public int Media2Synced { get; set; }
        public int Media3Synced { get; set; }
        public int Media4Synced { get; set; }
    }
}
