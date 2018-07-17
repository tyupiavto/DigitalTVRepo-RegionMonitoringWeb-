using AdminPanelDevice.Infrastructure;
using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdminPanelDevice.SnmpGet
{
    public class GetLogic
    {
        private GetData getData = new GetData();
        public GetLogic() { }

        public bool TheadDevicePlayStop(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, int deviceID, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            var gt = getThread = HangfireBootstrapper.Instance.GetThreadStart();
            if (gt.Count != 0)
            {
                getThread.AddRange(gt);
            }

            var oofDevice = getData.SleepTheadList(towerName, deviceID);
            if (oofDevice.Count != 0)
            {
                var thb = getThread.Where(t => t.TowerName == towerName && t.DeviceID == deviceID).ToList();
                thb.ForEach(t =>
                {
                    t.thread.Abort();
                    getThread.Remove(t);
                });
                var toweroff = getData.TheadDefineOffOn(towerID);
                if (toweroff != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                List<WalkTowerDevice> Log = getData.SelectedLogList(towerName, deviceID);
                List<WalkTowerDevice> Map = getData.SelectedMapList(towerName, deviceID);
                LogSelectThead(Log, getThreadPreset, towerID, getThread, db);
                MapSelectThead(Log, getThreadPreset, towerID, getThread, db);

                if (Log.Count == 0 && Map.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public List<int> PlayThreadList(bool treadListInd, List<GetSleepThread> getThread, SleepInformation returnedThreadList, List<int> playGet, string towerName, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            List<int> palayStop = new List<int>();
            playGet.ForEach(deviceID =>
            {
                List<WalkTowerDevice> Log = getData.SelectedLogList(towerName, deviceID);
                List<WalkTowerDevice> Map = getData.SelectedMapList(towerName, deviceID);
                LogSelectThead(Log, getThreadPreset, towerID, getThread, db);
                MapSelectThead(Log, getThreadPreset, towerID, getThread, db);
                if (Log.Count == 0 && Map.Count == 0)
                {
                    palayStop.Add(deviceID);
                }
            });
            return palayStop;

        }

        public void StopThreadList(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, List<int> stopGet)
        {
            var gt = HangfireBootstrapper.Instance.GetThreadStart();
            if (gt.Count != 0)
            {
                getThread.AddRange(gt);
            }
            stopGet.ForEach(deviceID =>
            {
                var oofDevice = getData.SleepTheadList(towerName, deviceID);
                if (oofDevice.Count != 0)
                {
                    getData.SleepTheadDelete(towerName, deviceID);
                    var thb = getThread.Where(t => t.TowerName == towerName && t.DeviceID == deviceID).ToList();
                    thb.ForEach(t =>
                    {
                        t.thread.Abort();
                        getThread.Remove(t);
                    });
                }
            });
        }


        public void LogSelectThead(List<WalkTowerDevice> Log, GetThread getThreadPreset, int towerID, List<GetSleepThread> getThread, DeviceContext db)
        {
            Log.ForEach(l =>
            {
                GetSleepThread gtl = new GetSleepThread();
                gtl.DeviceID = l.DeviceID;
                gtl.TowerName = l.TowerName;
                gtl.IP = l.IP;
                gtl.ScanInterval = l.ScanInterval;
                gtl.WalkOid = l.WalkOID;
                gtl.Version = l.Version;
                gtl.TowerID = towerID;
                gtl.CheckID = l.WalkID;
                gtl.StartCorrect = l.StartCorrect;
                gtl.EndCorrect = l.EndCorrect;
                gtl.OneStartError = l.OneStartError;
                gtl.OneEndError = l.OneEndError;
                gtl.OneStartCrash = l.OneStartCrash;
                gtl.OneEndCrash = l.OneEndCrash;
                gtl.TwoStartError = l.TwoStartError;
                gtl.TwoEndError = l.TwoEndError;
                gtl.TwoStartCrash = l.TwoStartCrash;
                gtl.TwoEndCrash = l.TwoEndCrash;
                gtl.WalkID = l.ID;
                gtl.DivideMultiply = l.DivideMultiply;
                gtl.StringParserInd = l.StringParserInd;
                if (l.MapID == 1)
                {
                    gtl.MapID = 1;
                }
                gtl.LogID = 1;
                gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.WalkID, l.StringParserInd, l.DivideMultiply, l.ID, towerID, l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version, l.StartCorrect, l.EndCorrect, l.OneStartError, l.OneEndError, l.OneStartCrash, l.OneEndCrash, l.TwoStartError, l.TwoEndError, l.TwoStartCrash, l.TwoEndCrash));
                gtl.thread.Start();
                getThread.Add(gtl);

                getData.GetSleepThreadSave(db, gtl);

            });
        }

        public void MapSelectThead(List<WalkTowerDevice> Map, GetThread getThreadPreset, int towerID, List<GetSleepThread> getThread, DeviceContext db)
        {
            Map.ForEach(l =>
            {
                if (l.LogID != 1)
                {
                    GetSleepThread gtl = new GetSleepThread();
                    gtl.DeviceID = l.DeviceID;
                    gtl.TowerName = l.TowerName;
                    gtl.IP = l.IP;
                    gtl.ScanInterval = l.ScanInterval;
                    gtl.WalkOid = l.WalkOID;
                    gtl.Version = l.Version;
                    gtl.TowerID = towerID;
                    gtl.CheckID = l.WalkID;
                    gtl.StartCorrect = l.StartCorrect;
                    gtl.EndCorrect = l.EndCorrect;
                    gtl.OneStartError = l.OneStartError;
                    gtl.OneEndError = l.OneEndError;
                    gtl.OneStartCrash = l.OneStartCrash;
                    gtl.OneEndCrash = l.OneEndCrash;
                    gtl.TwoStartError = l.TwoStartError;
                    gtl.TwoEndError = l.TwoEndError;
                    gtl.TwoStartCrash = l.TwoStartCrash;
                    gtl.TwoEndCrash = l.TwoEndCrash;
                    gtl.WalkID = l.ID;
                    gtl.DivideMultiply = l.DivideMultiply;
                    gtl.StringParserInd = l.StringParserInd;
                    gtl.MapID = 1;
                    gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.WalkID, l.StringParserInd, l.DivideMultiply, l.ID, towerID, l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version, l.StartCorrect, l.EndCorrect, l.OneStartError, l.OneEndError, l.OneStartCrash, l.OneEndCrash, l.TwoStartError, l.TwoEndError, l.TwoStartCrash, l.TwoEndCrash));
                    gtl.thread.Start();
                    getThread.Add(gtl);

                    getData.GetSleepThreadSave(db, gtl);
                }
            });
        }
    }
}