using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("TowerGps")]
    public class TowerGps
    {
        [Key]
        public int ID { get; set; }
        public string Lattitube { get; set; }
        public string Longitude { get; set; }
        public string Altitude { get; set; }
        public string TowerNameID { get; set; }
        public int DeviceID { get; set; }
        public string PresetName { get; set; }
    }
}