using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("PresetDiagramName")]
    public class PresetDiagramName
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}