using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblSubscription")]
    public class SubscriptionTable
    {
        [PrimaryKey, MaxLength(200)]
        public string SerialNumber { get; set; }
        [MaxLength(50)]
        public string ContactID { get; set; }
        public DateTime DateStart { get; set; }
        [MaxLength(200)]
        public string NoOfDays { get; set; }
        [MaxLength(200)]
        public string Trials { get; set; }
        [MaxLength(2000)]
        public string InputSerialNumber { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
    }
}
