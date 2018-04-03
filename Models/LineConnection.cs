using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("LineConnection")]
    public class LineConnection
    {
        [Key]
        public int ID { get; set; }
        public int ParentTowerID { get; set; }
        public int ChildTowerID { get; set; }
    }
}