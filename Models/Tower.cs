using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("Tower")]
    public class Tower
    {
        [Key]
        public int ID { get; set; }
        public int NumberID { get; set; }
        public string Name { get; set; }
        public double LattiTube { get; set; }
        public double LongiTube { get; set; }
        public string IP { get; set; }
        public int Phone { get; set; }
        public string Status { get; set; }
    }
}