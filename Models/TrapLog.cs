using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class TrapLog
    {
        public int ID { get; set; }
        public string Countrie { get; set; }
        public string States { get; set; }
        public string City { get; set; }
        public string TowerName { get; set; }
        public string DeviceName { get; set; }
        public string IP { get; set; }
        public string Description { get; set; }
        public string OID { get; set; }
        public string Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}