using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using AdminPanelDevice.Models;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class CheckUncheckGetThread
    {
       
        public GetSleepThread checkdGet(int chechkID, string towerName, int deviceID,DeviceContext db, int towerID,string LogMap)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var getcheck = connection.Query<GetSleepThread>($"select * from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}'").FirstOrDefault();
                GetSleepThread checkMapLog = new GetSleepThread();
                GetThread getThreadPreset = new GetThread();
                WalkTowerDevice addthread = new WalkTowerDevice();
                if (getcheck != null)
                {
                    if (LogMap == "Log")
                    {
                        addthread = connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where LogID<>0 and WalkID='{chechkID}' and TowerName='{towerName}' and DeviceID='{deviceID}'").FirstOrDefault();
                        checkMapLog.LogID = 1;
                    }
                    else
                    {
                        addthread = connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where MapID<>0 and WalkID='{chechkID}' and TowerName='{towerName}' and DeviceID='{deviceID}'").FirstOrDefault();
                        checkMapLog.MapID = 1;
                    }
                    var maplogExistence = connection.Query<GetSleepThread>($"select * from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}' and CheckID='{chechkID}'").FirstOrDefault();

                    if (maplogExistence==null)
                    {
                        checkMapLog.DeviceID = addthread.DeviceID;
                        checkMapLog.TowerName = addthread.TowerName;
                        checkMapLog.IP = addthread.IP;
                        checkMapLog.ScanInterval = addthread.ScanInterval;
                        checkMapLog.WalkOid = addthread.WalkOID;
                        checkMapLog.Version = addthread.Version;
                        checkMapLog.CheckID = addthread.WalkID;
                        checkMapLog.TowerID = towerID;
                        checkMapLog.thread = new Thread(() => getThreadPreset.ThreadPreset(addthread.IP, addthread.ScanInterval, addthread.DeviceID, addthread.WalkOID, addthread.Version));
                        checkMapLog.thread.Start();

                        db.GetSleepThread.Add(checkMapLog);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (LogMap=="Log")
                        {
                            connection.Query<GetSleepThread>($"update GetSleepThread set LogID=1 where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}' and CheckID='{chechkID}'");
                        }
                        else
                        {
                            connection.Query<GetSleepThread>($"update GetSleepThread set MapID=1 where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}' and CheckID='{chechkID}'");
                        }
                    }
                    return checkMapLog;
                }
                else
                {
                    return checkMapLog;
                }
            }
        }

        public List<GetSleepThread> UnCheckdGet (int unChechkID, string towerName, int deviceID, int towerID, string LogMap, List<GetSleepThread> getThread)
        {
            GetSleepThread uncheckMapLog = new GetSleepThread();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var getcheck = connection.Query<GetSleepThread>($"select * from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}'  and TowerID='{towerID}'").FirstOrDefault();
                if (LogMap=="Log")
                {
                    if (getcheck != null && getcheck.MapID!=1)
                    {
                        connection.Query<GetSleepThread>($"delete from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}' and TowerID='{towerID}'");
                        connection.Query<GetSleepThread>($"Update GetSleepThread Set LogID=0 where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}' and TowerID='{towerID}'");
                        var removeLog= getThread.Where(g => g.TowerName == towerName && g.DeviceID == deviceID && g.CheckID == unChechkID && g.TowerID == towerID).FirstOrDefault();
                        removeLog.thread.Abort();
                        getThread.Remove(removeLog);

                    }
                    else
                    {
                        connection.Query<GetSleepThread>($"Update GetSleepThread Set LogID=0 where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}' and TowerID='{towerID}'");
                    }
                }
                else
                {
                    if (getcheck!=null && getcheck.LogID != 1)
                    {
                        connection.Query<GetSleepThread>($"delete from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}' and TowerID='{towerID}'");
                        connection.Query<GetSleepThread>($"Update GetSleepThread Set MapID=0 where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}' and TowerID='{towerID}'");
                        var removeLog = getThread.Where(g => g.TowerName == towerName && g.DeviceID == deviceID && g.CheckID == unChechkID && g.TowerID==towerID).FirstOrDefault();
                        removeLog.thread.Abort();
                        getThread.Remove(removeLog);
                    }
                    else
                    {
                        connection.Query<GetSleepThread>($"Update GetSleepThread Set MapID=0 where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{unChechkID}' and TowerID='{towerID}'");
                    }
                }
            }
            return getThread;
        }

        public List<GetSleepThread> ChangeInterval(int intervalID, int Interval, string towerName, int deviceID, int towerID, List<GetSleepThread> getThread)
        {
            GetThread getThreadPreset = new GetThread();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<GetSleepThread>($"Update GetSleepThread Set ScanInterval='{Interval}' where TowerName='{towerName}' and DeviceID='{deviceID}' and CheckID='{intervalID}' and TowerID='{towerID}'");
                var intervalChange = getThread.Where(g => g.TowerName == towerName && g.DeviceID == deviceID && g.CheckID == intervalID && g.TowerID == towerID).FirstOrDefault();
                if (intervalChange != null)
                {
                    intervalChange.thread.Abort();
                    intervalChange.ScanInterval = Interval;
                    getThread.Remove(intervalChange);
                    intervalChange.thread= new Thread(() => getThreadPreset.ThreadPreset(intervalChange.IP, intervalChange.ScanInterval, intervalChange.DeviceID, intervalChange.WalkOid, intervalChange.Version));
                    intervalChange.thread.Start();
                    getThread.Add(intervalChange);
                }
            }
            return getThread;
        }
    }
}