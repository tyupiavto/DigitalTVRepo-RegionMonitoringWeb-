using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("WalkDevice")]
    public class WalkDevice
    {
        [Key]
        public int ID { get; set; }
        public string WalkOID { get; set; }
        public string WalkDescription { get; set; }
        public string Type { get; set; }
        public int WalkID { get; set; }
        public int LogID { get; set; }
        public int MapID { get; set; }
        public int OidID { get; set; }
        public int Time { get; set; }
        public int PresetID { get; set; }
        public int DeviceID { get; set; }
        public int CheckID { get; set; }
        [NotMapped]
        public string value { get; set; }
        [NotMapped]
        public string OIDName { get; set; }

        [ForeignKey("PresetID")]
        public Preset Preset { get; set; }
    }
}