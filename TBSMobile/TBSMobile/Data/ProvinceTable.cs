using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblProvince")]
    public class ProvinceTable
    {
        [PrimaryKey, MaxLength(50)]
        public string ProvinceID { get; set; }
        [MaxLength(2000)]
        public string Province { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{ProvinceID}/{Province}";
            }
        }
    }
}
