﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("City")]
    public class City
    {
        [Key]
        public int ID { get; set; }
        public string CityName { get; set; }
        [ForeignKey("StateID")]
        public States States { get; set; }
        public int StateID { get; set; }
        [NotMapped]
        public int CheckedID { get; set; }
    }
}