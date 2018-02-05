using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{ 
    [Table("Devices")]
    public class Devices
    {
        [Key]
        public int ID { get; set; }
        public int NumberID { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int Ping { get; set; }
        public int Frequency { get; set; }
        public int Channel { get; set; }
        public int DevSerialNumber { get; set; }
        public int DeviceTID { get; set; }
        public int TowerID { get; set; }
        public string PresetName { get; set; }
        [ForeignKey("TowerID")]
        public Tower Tower { get; set; }
        [ForeignKey("DeviceTID")]
        public DeviceType DeviceType { get; set; }
        
    }
}