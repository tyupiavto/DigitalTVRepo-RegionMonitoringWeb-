using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("DeviceThreadOnOff")]
    public class DeviceThreadOnOff
    {
        [Key]
        public int ID { get; set; }
        public int DeviceID { get; set; }
        public string TowerName { get; set; }
        public int TowerID { get; set; }
    }
}