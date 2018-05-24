using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class GetThreadPlayStop
    {
        public GetThreadPlayStop() { }

        public void PlayThread(bool treadListInd, List<GetSleepThread> getThread, SleepInformation returnedThreadList, List<int> playGet, string towerName, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            //if (treadListInd == true)
            //{
            //    getThread.AddRange(returnedThreadList.SleepGetInformation(false));
            //    treadListInd = false;
            //}

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                playGet.ForEach(deviceID =>
                {
                    var Log = connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where LogID<>0 and TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();
                    var Map = connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where MapID<>0 and TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();

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
                        if (l.MapID == 1)
                        {
                            gtl.MapID = 1;
                        }
                        gtl.LogID = 1;
                        gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version));
                        gtl.thread.Start();
                        getThread.Add(gtl);

                        db.GetSleepThread.Add(gtl);
                        db.SaveChanges();

                    });

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
                            gtl.MapID = 1;
                            gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version));
                            gtl.thread.Start();
                            getThread.Add(gtl);

                            db.GetSleepThread.Add(gtl);
                            db.SaveChanges();
                        }
                    });
                });
            }
        }

        public void StopThread(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, List<int> stopGet)
        {
            var gt = returnedThreadList.SleepGetInformation(false);
            if (gt.Count != 0)
            {
                getThread.AddRange(gt);
            }
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                stopGet.ForEach(deviceID =>
                {
                    var oofDevice = connection.Query<GetSleepThread>($"select * from GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();
                    if (oofDevice.Count != 0)
                    {
                        connection.Query<GetSleepThread>("delete from GetSleepThread where TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
                        var thb = getThread.Where(t => t.TowerName == towerName && t.DeviceID == deviceID).ToList();
                        thb.ForEach(t =>
                        {
                            t.thread.Abort();
                            getThread.Remove(t);
                        });

                    }
                });
            }
        }

        public bool Get(List<GetSleepThread> getThread, SleepInformation returnedThreadList, string towerName, int deviceID, int towerID, DeviceContext db, GetThread getThreadPreset)
        {
            var gt = returnedThreadList.SleepGetInformation(false);
            if (gt.Count != 0)
            {
                getThread.AddRange(gt);
            }
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var oofDevice = connection.Query<GetSleepThread>($"select * from GetSleepThread where TowerName='{ towerName }' and DeviceID='{deviceID}'").ToList();
                if (oofDevice.Count != 0)
                {
                    connection.Query<GetSleepThread>($"delete from GetSleepThread where TowerName='{towerName}' and DeviceID='{ deviceID }'");
                    var thb = getThread.Where(t => t.TowerName == towerName && t.DeviceID == deviceID).ToList();
                    thb.ForEach(t =>
                    {
                        t.thread.Abort();
                        getThread.Remove(t);
                    });
                    var toweroff = connection.Query<GetSleepThread>("select * from  GetSleepThread where TowerID='" + towerID + "'").FirstOrDefault();
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
                    var Log = connection.Query<WalkTowerDevice>("select * from WalkTowerDevice where LogID<>0 and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'").ToList();
                    var Map = connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where MapID<>0 and TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();

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
                        if (l.MapID == 1)
                        {
                            gtl.MapID = 1;
                        }
                        gtl.LogID = 1;
                        gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version));
                        gtl.thread.Start();
                        getThread.Add(gtl);
                        db.GetSleepThread.Add(gtl);
                        db.SaveChanges();

                    });

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
                            gtl.MapID = 1;
                            gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version));
                            gtl.thread.Start();
                            getThread.Add(gtl);

                            db.GetSleepThread.Add(gtl);
                            db.SaveChanges();
                        }
                    });

                }
            }
            return true;
        }
    }
}