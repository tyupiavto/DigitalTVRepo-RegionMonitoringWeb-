using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    [Table("TowerPreset")]
    public class TowerPreset
    {
        [Key]
        public int ID { get; set; }
        public int NumberID { get; set; }
        public string Name { get; set; }
        public string PresetName { get; set; }
        public int CountriesID { get; set; }
        public int StateID { get; set; }
        public int CityCheckedID { get; set; }
        public int CountriesListID { get; set; }
    }
}