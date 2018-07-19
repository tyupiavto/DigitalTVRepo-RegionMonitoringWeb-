using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Traps
{
    public class TrapPresentation
    {
        public TrapPresentation() { }
        private TrapBusinessLogic trapBusinessLogic = new TrapBusinessLogic();
        public List<Trap> TrapLogShow (List<Trap> TrapLogList,int LogInd)
        {
            return trapBusinessLogic.TrapLogLoad(TrapLogList, LogInd);
        }
    }
}