using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("AlarmLogStatus")]
    public class AlarmLogStatus
    {
        [Key]
        public int ID { get; set; }
        public string DeviceName{ get; set; }
        public string AlarmText { get; set; }
        public string AlarmStatus { get; set; }
    }
}