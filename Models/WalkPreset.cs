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
        public int DeviceID { get; set; }
        public string OneStartError { get; set; }
        public string OneEndError { get; set; }
        public string OneStartCrash { get; set; }
        public string OneEndCrash { get; set; }
        public string StartCorrect { get; set; }
        public string EndCorrect { get; set; }
        public string TwoStartError { get; set; }
        public string TwoEndError { get; set; }
        public string TwoStartCrash { get; set; }
        public string TwoEndCrash { get; set; }
        [ForeignKey("PresetID")]
        public Preset Preset { get; set; }
    }
}