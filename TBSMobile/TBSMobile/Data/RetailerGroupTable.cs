using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblRetailerGroup")]
    public class RetailerGroupTable
    {
        [PrimaryKey, MaxLength(100)]
        public string RetailerCode { get; set; }
        [MaxLength(50)]
        public string ContactID { get; set; }
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
        [MaxLength(30)]
        public string Telephone1 { get; set; }
        [MaxLength(30)]
        public string Telephone2 { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(500)]
        public string Landmark { get; set; }
        [MaxLength(200)]
        public string GPSCoordinates { get; set; }
        [MaxLength(50)]
        public string Supervisor { get; set; }
        [MaxLength(100)]
        public string RecordLog { get; set; }
        public int Checked { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{RetailerCode}-{Landmark}";
            }
        }
        public string ListText
        {
            get
            {
                return $"Outlet Name: {Landmark}";
            }
        }
    }
}
