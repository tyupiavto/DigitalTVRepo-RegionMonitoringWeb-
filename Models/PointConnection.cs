using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("PointConnection")]
    public class PointConnection
    {
        [Key]
        public int ID { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public string GetUuids { get; set; }
        public string PointRight { get; set; }
        public string PointLeft { get; set; }
    }
}