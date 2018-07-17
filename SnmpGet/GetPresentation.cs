using AdminPanelDevice.Infrastructure;
using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.SnmpGet
{
    public class GetPresentation
    {
        private GetLogic getLogic = new GetLogic();
        public GetPresentation() { }

        public bool DeviceTheadOnOff (List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, int deviceID, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
          return  getLogic.TheadDevicePlayStop(getThread, returnedThreadList, towerName, deviceID,towerID,db,getThreadPreset);
        }

        public List<int> PlayTheadDevice (bool treadListInd, List<GetSleepThread> getThread, SleepInformation returnedThreadList, List<int> playGet, string towerName, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            return getLogic.PlayThreadList(treadListInd, getThread, returnedThreadList, playGet, towerName, towerID, db, getThreadPreset);
        }

        public void StopThread(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, List<int> stopGet)
        {
            getLogic.StopThreadList(getThread, returnedThreadList, towerName, stopGet);
        }
    }
}