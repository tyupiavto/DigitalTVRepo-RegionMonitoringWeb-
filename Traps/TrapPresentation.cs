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

        public TrapLogInformationList TrapLogSearchList (string SearchName, int SearchClear, string startTime, string endTime, List<Trap> TrapLogList, List<Trap> TrapLogListSearch, int SearchIndicator, string mapTowerDeviceName, int LogInd)
        {
            return trapBusinessLogic.LogLoad(SearchName, SearchClear, startTime, endTime, TrapLogList, TrapLogListSearch, SearchIndicator, mapTowerDeviceName, LogInd);
        }

        public List<Trap> AlarmColorSearchList (string correctColor, string errorColor, string crashColor, string whiteColor, int all, List<Trap> TrapLogListSearch, List<Trap> TrapLogList, int SearchIndicator)
        {
            return trapBusinessLogic.AlarmColorSearch(correctColor, errorColor, crashColor, whiteColor, all, TrapLogListSearch, TrapLogList,SearchIndicator);
        }

        public List<Trap> AlarmLogStatusResult(string alarmColor, string deviceName, string alarmText, string returnOidText, string currentOidText, string alarmDescription, List<Trap> TrapLogList, DeviceContext db)
        {
          return trapBusinessLogic.AlarmLogStatus(alarmColor, deviceName, alarmText, returnOidText, currentOidText, alarmDescription, TrapLogList, db);
        }
    }
}