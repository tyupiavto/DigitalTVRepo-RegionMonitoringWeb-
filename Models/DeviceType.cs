using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("DeviceType")]
    public class DeviceType
    {
        [Key]
        public int ID { get; set; }
        public int NumberID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacture { get; set; }
        public string Purpose { get; set; }
        public string MibParser { get; set; }
        public int DeviceGroupID { get; set; }
        [NotMapped]
        public HttpPostedFileBase mib_file { get; set; }
    }
}