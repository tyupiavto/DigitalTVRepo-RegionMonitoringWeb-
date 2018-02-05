using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("LiveValue")]
    public class LiveValue

    { 
        [Key]
        public int ID { get; set; }
        public string liveValue { get; set; }
        public int Time { get; set; }
    }
}