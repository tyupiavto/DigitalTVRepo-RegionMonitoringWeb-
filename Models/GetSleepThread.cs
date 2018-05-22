using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("GetSleepThread")]
    public class GetSleepThread
    {
        [Key]
        public int ID { get; set; }
        public int DeviceID { get; set; }
        public string TowerName { get; set; }
        public string IP { get; set; }
        public int ScanInterval { get; set; }
        public string WalkOid { get; set; }
        public string Version { get; set; }
        public int TowerID { get; set; }

        [NotMapped]
        public Thread thread { get; set; }
    }
}