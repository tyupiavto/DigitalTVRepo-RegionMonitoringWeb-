using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class GetValueCorrectError
    {
        public string StartCorrect { get; set; }
        public string EndCorrect { get; set; }
        public string OneStartError { get; set; }
        public string OneEndError { get; set; }
        public string OneStartCrash { get; set; }
        public string OneEndCrash { get; set; }
        public string TwoStartError { get; set; }
        public string TwoEndError { get; set; }
        public string TwoStartCrash { get; set; }
        public string TwoEndCrash { get; set; }

    }
}