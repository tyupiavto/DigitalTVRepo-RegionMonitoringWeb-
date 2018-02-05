using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("ScanningInterval")]
    public class ScanningInterval
    {
        [Key]
        public int ID { get; set; }
        public int Interval { get; set; }
        public int IntervalID { get; set; }
    }
}