using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblActivity")]
    public class ActivityTable
    {
        [MaxLength(50)]
        public string CAFNo { get; set; }
        public string ContactID { get; set; }
        [MaxLength(100)]
        public string Activity { get; set; }
        [MaxLength(2)]
        public int ActivitySwitch { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
    }
}
