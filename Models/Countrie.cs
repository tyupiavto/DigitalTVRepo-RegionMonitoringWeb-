using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("Countrie")]
    public class Countrie
    {
        [Key]
        public int ID { get; set; }
        public string SortName { get; set; }
        public string CountrieName { get; set; }
        public int PhoneCode { get; set; }
    }
}