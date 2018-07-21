using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanelDevice.LiveTrap;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.LiveTrap
{
    public class LiveTrapPresentation
    {
        private LiveTrapBusinessLogic liveTrapBusinessLogic = new LiveTrapBusinessLogic();
        public LiveTrapPresentation() { }

        public List<Trap> LiveTrapList (List<Trap> ListTrap, Trap TrapResponse)
        {
            return liveTrapBusinessLogic.LiveTrapErrorCorrectDefine(ListTrap,TrapResponse);
        }

        public List<TrapListNameCheck> TrapListHeaderCheckSelected ()
        {
            return liveTrapBusinessLogic.TrapNameSelected();
        }

        public List<Trap> TrapCurrentAlarmResult (int AlarmInd)
        {
            return liveTrapBusinessLogic.TrapCurrentAlarm(AlarmInd);
        }

        public TrapColorCount TrapCurrentAlarmCount (List<Trap> TrapAlarmList)
        {
            return liveTrapBusinessLogic.LiveTrapErrorCorrectedCount(TrapAlarmList);
        } 
        public void TrapLogClear ()
        {
            liveTrapBusinessLogic.TrapClear();
        }
    }
}