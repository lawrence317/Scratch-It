using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblUserEmail")]
    public class UserEmailTable
    {
        [MaxLength(50)]
        public string ContactID { get; set; }
        [MaxLength(1000)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string RecordLog { get; set; }
        public int Checked { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
    }
}
