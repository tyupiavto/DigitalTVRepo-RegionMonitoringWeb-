using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.LiveTrap
{
    public class LiveTrapBusinessLogic
    {
        private LiveTrapData liveTrapData = new LiveTrapData();
        public LiveTrapBusinessLogic() { }

        public List<Trap> LiveTrapErrorCorrectDefine(List<Trap> LiveTrapList, Trap TrapResponse)
        {
            if (TrapResponse.AlarmStatus == "red" || TrapResponse.AlarmStatus == "yellow")
            {
                LiveTrapList.Add(TrapResponse);
            }
            if (TrapResponse.AlarmStatus == "green")
            {
                var trapLive = LiveTrapList.Where(e => e.IpAddres == TrapResponse.IpAddres && e.CurrentOID == TrapResponse.CurrentOID && e.ReturnedOID == TrapResponse.ReturnedOID && e.TowerName == TrapResponse.TowerName).FirstOrDefault();
                LiveTrapList.Remove(trapLive);
            }
            LiveTrapList = LiveTrapList.OrderByDescending(t => t.dateTimeTrap).ToList();
            return LiveTrapList;
        }

        public List<Trap> TrapCurrentAlarm()
        {
            List<Trap> TrapCurrentErrorList = new List<Trap>();
            List<Trap> TrapCurrentCorrectList = new List<Trap>();
            List<Trap> TrapCorrectList = new List<Trap>();
            DateTime start = DateTime.Now;
            DateTime end = start.Add(new TimeSpan(-24, 0, 0));

            TrapCorrectList = liveTrapData.TrapCurrentAlarmList(start, end);
            TrapCurrentErrorList = TrapCorrectList.Where(e => e.AlarmStatus == "red" || e.AlarmStatus == "yellow").ToList();

            TrapCurrentCorrectList = TrapCorrectList.Where(e => e.AlarmStatus == "green").ToList();

            TrapCurrentCorrectList.ForEach(itm =>
            {
                var trap = TrapCurrentErrorList.Where(e => e.IpAddres == itm.IpAddres && e.CurrentOID == itm.CurrentOID && e.ReturnedOID == itm.ReturnedOID && e.TowerName == itm.TowerName).ToList();
                trap.ForEach(t => {
                    TrapCurrentErrorList.Remove(t);
                });
               
            });
            return TrapCurrentErrorList;
        }

        public List<TrapListNameCheck> TrapNameSelected()
        {
            return liveTrapData.TrapHeaderNameSelectList();
        }

        public TrapColorCount LiveTrapErrorCorrectedCount (List<Trap> TrapAlarmList)
        {
            TrapColorCount errorCorrectCount = new TrapColorCount();
            errorCorrectCount.ErrorCount = TrapAlarmList.Where(w => w.AlarmStatus == "red").ToList().Count;
            errorCorrectCount.CrashCount = TrapAlarmList.Where(w => w.AlarmStatus == "yellow").ToList().Count;
            return errorCorrectCount;
        }
    }
}