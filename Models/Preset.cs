using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("Preset")]
    public class Preset
    {
        [Key]
        public int ID { get; set; }
        public string PresetName { get; set; }
        public string DeviceIP { get; set; }
        public int DeviceID { get; set; }
        public int DeviceTypeID { get; set; }
    }
}