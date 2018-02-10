using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("States")]
    public class States
    {
        [Key]
        public int ID { get; set; }
        public string StateName { get; set; }
        public int CountrieID { get; set; }
    }
}