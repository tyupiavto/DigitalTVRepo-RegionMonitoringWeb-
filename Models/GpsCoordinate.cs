using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class GpsCoordinate
    {
        public List<string> DeviceName = new List<string>();
        public string Lattitube { get; set; }
        public string Longitube { get; set; }
        public string Altitube { get; set; }
    }
}