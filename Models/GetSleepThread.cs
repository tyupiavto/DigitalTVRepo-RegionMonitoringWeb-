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
        public int CheckID { get; set; }
        public int LogID { get; set; }
        public int MapID { get; set; }
        public int WalkID { get; set; }

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

        [NotMapped]
        public Thread thread { get; set; }
    }
}