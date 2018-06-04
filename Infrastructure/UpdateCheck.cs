using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AdminPanelDevice.Models;
using System.Linq;
using System.Web;
using System.Drawing;

namespace AdminPanelDevice.Infrastructure
{
    public class UpdateCheck : UpdateCheckLogMap
    {
        DeviceContext db = new DeviceContext();
        public UpdateCheck()
        {

        }
        public void UpdateChechkLog(int chechkLog, int walkCheckID, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set LogID='" + chechkLog + "' where WalkID='" + walkCheckID + "'and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
            }
        }
        public void UpdateChechkMap(int checkMap, int walkCheckID, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set MapID='" + checkMap + "' where WalkID='" + walkCheckID + "'and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
            }
        }
        public void UpdateInterval(int intervalID, int Interval, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set ScanInterval='" + Interval + "' where WalkID='" + intervalID + "' and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
            }
        }

        public void UpdateChechkGps(int checkGps, int walkCheckID, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set GpsID='" + checkGps + "' where WalkID='" + walkCheckID + "'and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
            }
        }

        public void UpdateLogMapSetting (LogMapSettingValue value,string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>($"Update WalkTowerDevice Set StartCorrect='{value.startCorrect}',EndCorrect='{value.endCorrect}', OneStartError='{value.oneStartError}', OneEndError='{value.oneEndError}', OneStartCrash='{value.oneStartCrash}', OneEndCrash='{value.oneEndCrash}', TwoStartError='{value.twoStartError}', TwoEndError='{value.twoEndError}', TwoStartCrash='{value.twoStartCrash}', TwoEndCrash='{value.twoEndCrash}' where WalkID='{value.settingID}'and TowerName='{value.towerName}' and DeviceID='{deviceID}'");
            }
        }

        public void WalkPresetSave(List<WalkTowerDevice> walkList, int presetID, string DeviceName, string TowerID, int deviceID)
        {
            List<WalkPreset> walkPresetList = new List<WalkPreset>();

            List<Point> p = new List<Point>();
            WalkPreset wP = new WalkPreset();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                List<int> Log = connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and LogID<>0 and DeviceID='" + deviceID + "'").ToList().Select(s => s.WalkID).ToList();
                List<int> Map = connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and MapID<>0 and DeviceID='" + deviceID + "'").ToList().Select(s => s.WalkID).ToList();
                List<int> Gps = connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and GpsID<>0 and DeviceID='" + deviceID + "'").ToList().Select(s => s.WalkID).ToList();
                Log.ForEach(l =>
                {
                    WalkPreset wlk = new WalkPreset();
                    if (Map.Contains(l))
                    {
                        Map.Remove(l);
                        wlk.LogID = l;
                        wlk.MapID = l;
                    }
                    else
                    {
                        wlk.LogID = l;
                    }
                    if (Gps.Contains(l))
                    {
                        Gps.Remove(l);
                        wlk.GpsID = l;
                    }
                    wlk.PresetID = presetID;
                    wlk.DeviceName = DeviceName;
                    wlk.IntervalID = l;
                    wlk.DeviceID = deviceID;
                    wlk.Interval = walkList[l - 1].ScanInterval;
                    walkPresetList.Add(wlk);
                });

                Map.ForEach(m =>
                {
                    WalkPreset wlk = new WalkPreset();
                    if (Gps.Contains(m))
                    {
                        Gps.Remove(m);
                        wlk.GpsID = m;
                    }

                    wlk.MapID = m;
                    wlk.PresetID = presetID;
                    wlk.DeviceName = DeviceName;
                    wlk.IntervalID = m;
                    wlk.DeviceID = deviceID;
                    wlk.Interval = walkList[m - 1].ScanInterval;
                    walkPresetList.Add(wlk);
                });
                Gps.ForEach(g =>
                {
                    WalkPreset wlk = new WalkPreset();
                    wlk.GpsID = g;
                    wlk.PresetID = presetID;
                    wlk.DeviceName = DeviceName;
                    wlk.IntervalID = g;
                    wlk.DeviceID = deviceID;
                    wlk.Interval = walkList[g - 1].ScanInterval;
                    walkPresetList.Add(wlk);
                });
                db.WalkPresets.AddRange(walkPresetList);
                db.SaveChanges();
            }
        }
    }
}