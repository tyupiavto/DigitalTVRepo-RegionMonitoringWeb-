using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("TowerDevices")]
    public class TowerDevices
    {
        [Key]
        public int ID { get; set;}
        public string TowerName { get; set; }
        public string TowerID { get; set; }
        public string DeviceName { get; set; }
        public  int DeviceID { get; set;}
        public string IP { get; set; }
        public string CityID{ get; set; }
        public string CountrieName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public int MibID { get; set; }
    }
}