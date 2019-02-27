using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblUserLogs")]
    public class UserLogsTable
    {
        [MaxLength(50)]
        public string ContactID { get; set; }
        [MaxLength(20)]
        public string LogType { get; set; }
        public string Log { get; set; }
        public DateTime LogDate { get; set; }
        [MaxLength(20)]
        public string DatabaseName { get; set; }
        public int Checked { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
    }
}
