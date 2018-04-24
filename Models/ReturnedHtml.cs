using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.Models
{
    public class ReturnedHtml
    {
        [AllowHtml]
        public string Html { get; set; }
        [AllowHtml]
        public string Xml { get; set; }
        public string PresetName { get; set; }
        public Array[] connect { get; set; } 
    }
}