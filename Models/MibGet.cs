using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("MibGet")]
    public class MibGet
    {
        [Key]
        public int ID { get; set; }
        public int MibID { get; set; }
        public string WalkOID { get; set; }
        public string Value { get; set; }
        public int DeviceID { get; set; }
        public DateTime dateTime { get; set; }
    }
}