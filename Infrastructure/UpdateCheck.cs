using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AdminPanelDevice.Models;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class UpdateCheck : UpdateCheckLogMap
    {
        public UpdateCheck()
        {

        }
        public void UpdateChechkLog(int chechkLog, int walkCheckID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set LogID='" + chechkLog + "' where WalkID='" + walkCheckID + "'");
            }
        }
        public void UpdateChechkMap(int checkMap, int walkCheckID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set MapID='" + checkMap + "' where WalkID='" + walkCheckID + "'");
            }
        }
        public void UpdateInterval(int intervalID, int Interval)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>("Update WalkTowerDevice Set ScanInterval='" + Interval + "' where WalkID='" + intervalID + "'");
            }
        }
        public void WalkPresetSave(string DeviceName,string TowerID)
        {
            WalkPreset wP = new WalkPreset();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var Log= connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and LogID<>0").ToList();
                var Map = connection.Query<WalkTowerDevice>("select WalkID from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and MapID<>0").ToList();
                var Interval= connection.Query<WalkTowerDevice>("select WalkID,ScanInterval from WalkTowerDevice where DeviceName=N'" + DeviceName + "' and TowerName='" + TowerID + "' and ScanInterval<>60").ToList();
                var logmap = Map.Zip(Log, (map, log) => map.WalkID != log.WalkID);
               // Log.ForEach(l => wP.LogID = (l.WalkID)wP.DeviceName = DeviceName);
            }

        }
    }
}