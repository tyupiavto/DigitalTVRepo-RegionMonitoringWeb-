using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("TreeInformation")]
    public class MibTreeInformation
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string OID { get; set; }
        public string Mib { get; set; }
        public string Syntax { get; set; }
        public string Access { get; set; }
        public string Status { get; set; }
        public string DefVal { get; set; }
        public string Indexes { get; set; }
        public string Description { get; set; }
        public int MibID { get; set; }
        public int ParrentID { get; set; }

        [ForeignKey("DeviceID")]
        public DeviceType DeviceType { get; set; }

        public int DeviceID { get; set; }
        [NotMapped]
        public string ParentName { get; set; }
    }
}