using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IToolS.IOServers.Snmp;

namespace AdminPanelDevice.Models
{
    public class presetThread
    {
        public string IP { get; set; }
        public int Time { get; set; }
        public int DeviceID { get; set; }
        public int MibID { get; set; }
    }
}