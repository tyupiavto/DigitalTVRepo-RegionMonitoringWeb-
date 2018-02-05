using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class WalkInformation
    {
        public int ID { get; set; }
        public string OIDName { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int Time { get; set; }
        public string WalkName { get; set; }
        public int WalkID { get; set; }
    }
}