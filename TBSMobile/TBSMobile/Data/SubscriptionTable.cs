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
        public string RegistrationNumber { get; set; }
        [MaxLength(50)]
        public string ContactID { get; set; }
        [MaxLength(50)]
        public string NoOfDays { get; set; }
        [MaxLength(200)]
        public string InputDate { get; set; }
        [MaxLength(200)]
        public string ExpirationDate { get; set; }
        [MaxLength(2000)]
        public string ProductKey { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
    }
}
