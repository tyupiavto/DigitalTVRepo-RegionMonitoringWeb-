using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Traps
{
    public class TrapBusinessLogic
    {
        public TrapBusinessLogic () { }
        private TrapData trapData = new TrapData();

        public List<Trap> TrapLogLoad (List<Trap> TrapLogList,int LogInd)
        {
            if (LogInd == 0)
            {
                DateTime start = DateTime.Now;
                DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                TrapLogList = trapData.OneDeyList(end, start);
                TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();   
            }
            return TrapLogList;
        }
    }
}