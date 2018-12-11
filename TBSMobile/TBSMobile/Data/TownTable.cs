﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    [Table("tblTown")]
    public class TownTable
    {
        [PrimaryKey, MaxLength(50)]
        public string TownID { get; set; }
        [MaxLength(50)]
        public string ProvinceID { get; set; }
        [MaxLength(200)]
        public string Town { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{TownID}/{Town}";
            }
        }
    }
}
