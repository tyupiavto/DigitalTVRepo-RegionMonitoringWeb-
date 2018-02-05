using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("TreeStructure")]
    public class TreeStructure
    {
        [Key]
        public int ID { get; set; }
        public string OIDName { get; set; }
        public int Child { get; set; }
        public int Parrent { get; set; }
        public int TreeID { get; set; }
        public int DeviceID { get; set; }

        [ForeignKey("DeviceID")]
        public DeviceType DeviceType { get; set; }
    }
}