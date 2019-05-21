using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblCaf")]
    public class CAFTable
    {
        [PrimaryKey, MaxLength(50)]
        public string CAFNo { get; set; }
        [MaxLength(50)]
        public string EmployeeID { get; set; }
        public DateTime CAFDate { get; set; }
        [MaxLength(50)]
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
        [MaxLength(2000)]
        public string Remarks { get; set; }
        [MaxLength(2000)]
        public string OtherConcern { get; set; }
        [MaxLength(2000)]
        public string GPSCoordinates { get; set; }
        [MaxLength(100)]
        public string RecordLog { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Existed { get; set; }
        public int Deleted { get; set; }
        public string ActNumber
        {
            get
            {
                return $"Activity Number: {CAFNo}";
            }
        }

        public string ActDate
        {
            get
            {
                return $"Activity Date: { CAFDate.Date.ToString("MM/dd/yyyy")}";
            }
        }
    }
}
