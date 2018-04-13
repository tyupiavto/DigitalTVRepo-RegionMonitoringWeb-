using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("Trap")]
    public class Trap
    {
        [Key]
        public int ID { get; set; }
        public string IpAddres { get; set; }
        public string CurrentOID { get; set; }
        public string ReturnedOID { get; set; }
        public string Value { get; set; }
        public DateTime dateTimeTrap { get; set; }
    }
}