using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("TrapListNameCheck")]
    public class TrapListNameCheck
    {
        [Key]
        public int ID { get; set; }
        public string ListName { get; set; }
        public int ListID { get; set; }
        public string Checked { get; set; }
    }
}