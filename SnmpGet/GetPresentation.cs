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

        public bool DeviceTheadOnOff(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, int deviceID, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            return getLogic.TheadDevicePlayStop(getThread, returnedThreadList, towerName, deviceID, towerID, db, getThreadPreset);
        }

        public List<int> PlayTheadDevice(bool treadListInd, List<GetSleepThread> getThread, SleepInformation returnedThreadList, List<int> playGet, string towerName, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            return getLogic.PlayThreadList(treadListInd, getThread, returnedThreadList, playGet, towerName, towerID, db, getThreadPreset);
        }

        public void StopThread(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, List<int> stopGet)
        {
            getLogic.StopThreadList(getThread, returnedThreadList, towerName, stopGet);
        }

        public GetSleepThread SelectedLog(int chechkID, string towerName, int deviceID, int towerID, string LogMap, bool logStartStopPlay, DeviceContext db)
        {
            var LogMapCount = getLogic.LogCheckCount(1, chechkID, towerName, deviceID, towerID);
            return getLogic.checkedLogMap(chechkID, towerName, deviceID, db, towerID, LogMap, LogMapCount, logStartStopPlay);
        }

        public GetSleepThread SelectedMap(int chechkID, string towerName, int deviceID, int towerID, string LogMap, bool logStartStopPlay, DeviceContext db)
        {
            var LogMapCount = getLogic.MapCheckCount(1, chechkID, towerName, deviceID, towerID);
            return getLogic.checkedLogMap(chechkID, towerName, deviceID, db, towerID, LogMap, LogMapCount, logStartStopPlay);
        }

        public List<GetSleepThread> UnCheckLog(int unChechkID, string towerName, int deviceID, int towerID, string LogMap, List<GetSleepThread> getThread)
        {
            getLogic.LogCheckCount(0, unChechkID, towerName, deviceID, towerID);
            return getLogic.UnCheckLog(unChechkID, towerName, deviceID, towerID, LogMap, getThread);
        }

        public List<GetSleepThread> UnCheckMap(int unChechkID, string towerName, int deviceID, int towerID, string LogMap, List<GetSleepThread> getThread)
        {
            getLogic.MapCheckCount(0, unChechkID, towerName, deviceID, towerID);
            return getLogic.UnCheckLog(unChechkID, towerName, deviceID, towerID, LogMap, getThread);
        }

        public List<GetSleepThread> UpdateChangeInterval(int intervalID, int Interval, string towerName, int deviceID, int towerID, List<GetSleepThread> getThread)
        {
            return getLogic.ChangeInterval(intervalID, Interval, towerName, deviceID, towerID, getThread);
        }

        public List<GetSleepThread> ValueStringParseResult(int checkParser, int walkID, string towerName, int deviceID, int towerID, List<GetSleepThread> getThread, GetThread getThreadPreset)
        {
            return getLogic.WalkValueStringParse(checkParser, walkID, towerName, deviceID, towerID, getThread, getThreadPreset);
        }
        public int StringParserID (int walkID, string towerName, int deviceID)
        {
            return getLogic.ValueStringSelect(walkID, towerName, deviceID);
        }
    }
}