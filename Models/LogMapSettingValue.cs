using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class LogMapSettingValue
    {
        public string towerName { get; set; }
        public int settingID { get; set; }
        public string oneStartError { get; set; }
        public string oneEndError { get; set; }
        public string oneStartCrash { get; set; }
        public string oneEndCrash { get; set; }
        public string startCorrect { get; set; }
        public string endCorrect { get; set; }
        public string twoStartError { get; set; }
        public string twoEndError { get; set; }
        public string twoStartCrash { get; set; }
        public string twoEndCrash { get; set; }
        public string dividedMultiply { get; set; }
    }
}