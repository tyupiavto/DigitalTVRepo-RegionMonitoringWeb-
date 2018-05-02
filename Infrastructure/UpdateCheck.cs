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
        public void UpdateChechkLog(int chechkLog, int walkCheckID,string towerName, string IP)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set LogID='" + chechkLog + "' where WalkID='" + walkCheckID + "'and TowerName='" + towerName + "' and IP='" + IP + "'");
            }
        }
        public void UpdateChechkMap(int checkMap, int walkCheckID, string towerName, string IP)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set MapID='" + checkMap + "' where WalkID='" + walkCheckID + "'and TowerName='" + towerName + "' and IP='" + IP + "'");
            }
        }
        public void UpdateInterval(int intervalID, int Interval, string towerName, string IP)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set ScanInterval='" + Interval + "' where WalkID='" + intervalID + "' and TowerName='"+towerName+ "' and IP='" + IP + "'");
            }
        }
        public void WalkPresetSave(List<WalkTowerDevice> walkList,int presetID,string DeviceName,string TowerID, string IP)
        {
            List<WalkPreset> walkPresetList = new List<WalkPreset>();
            
            List<Point> p = new List<Point>();
            WalkPreset wP = new WalkPreset();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                List<int> Log= connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and LogID<>0 and IP='"+IP+"'").ToList().Select(s=>s.WalkID).ToList();
                List<int> Map = connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and MapID<>0 and IP='" + IP + "'").ToList().Select(s => s.WalkID).ToList();
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
                    wlk.PresetID = presetID;
                    wlk.DeviceName = DeviceName;
                    wlk.IntervalID = l;
                    wlk.IP = IP;
                    wlk.Interval = walkList[l - 1].ScanInterval;
                    walkPresetList.Add(wlk);
                });

                Map.ForEach(m =>
                {
                    WalkPreset wlk = new WalkPreset();
                    wlk.MapID = m;
                    wlk.PresetID = presetID;
                    wlk.DeviceName = DeviceName;
                    wlk.IntervalID = m;
                    wlk.IP = IP;
                    wlk.Interval = walkList[m - 1].ScanInterval;
                    walkPresetList.Add(wlk);
                });
            }
            db.WalkPresets.AddRange(walkPresetList);
            db.SaveChanges();
        }
    }
}