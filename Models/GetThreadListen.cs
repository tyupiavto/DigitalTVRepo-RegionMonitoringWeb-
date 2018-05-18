using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class GetThreadListen
    {
        public string towerName { get; set; }
        public int deviceID { get; set; }
        public Thread thread { get; set; } 
    }
}