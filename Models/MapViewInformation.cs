using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class MapViewInformation
    {
        public string MapColor { get; set; }
        public string TextColor { get; set; }
        public string LineColor { get; set; }
        public string Value { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
        public int TowerID { get; set; }
        public double StartLattitube { get; set; }
        public double StartLongitube { get; set; }
        public string GetTrap { get; set; }

        public List<mapTower> TowerLine = new List<mapTower>();
    }
}