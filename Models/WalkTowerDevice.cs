﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("WalkTowerDevice")]
    public class WalkTowerDevice
    {
        [Key]
        public int ID { get; set; }
        public string OIDName { get; set; }
        public string WalkDescription { get; set; }
        public string Value { get; set; }
        public int MapID { get; set; }
        public int LogID { get; set; }
        public int ScanInterval { get; set; }
        public string DeviceName { get; set; }
        public string TowerName { get; set; }
        public int WalkID { get; set; }
        public string WalkOID { get; set; }
        public string Type { get; set; }
    }
}