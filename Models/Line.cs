using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("Line")]
    public class Line
    {
        [Key]
        public int ID { get; set; }
        public int FromTID { get; set; }
        public int ToTID { get; set; }

        [ForeignKey("FromTID")]
        public Tower Tower { get; set; }

        [ForeignKey("ToTID")]
        public Line Lines { get; set; }
    }
}