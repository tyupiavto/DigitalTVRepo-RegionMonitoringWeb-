using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("WalkPreset")]
    public class WalkPreset
    {
        [Key]
        public int ID { get; set; }
        public int PresetID { get; set; }
        public int LogID { get; set; }
        public int MapID { get; set; }
        public int IntervalID { get; set; }
        public int Interval { get; set; }
        public int GpsID { get; set; }
        public string DeviceName { get; set; }
    }
}