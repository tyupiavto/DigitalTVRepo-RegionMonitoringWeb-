using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace AdminPanelDevice.SnmpGet
{
    public class GetData
    {
        public GetData() { }

        public List<GetSleepThread> SleepTheadList(string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<GetSleepThread>($"select * from GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();
            }
        }

        public void SleepTheadDelete(string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<GetSleepThread>($"delete from GetSleepThread where TowerName='{towerName}' and DeviceID='{ deviceID }'");
            }
        }

        public GetSleepThread TheadDefineOffOn(int towerID) {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
               return connection.Query<GetSleepThread>($"select * from  GetSleepThread where TowerID='{towerID}'").FirstOrDefault();
            }
        }

        public List<WalkTowerDevice> SelectedLogList(string towerName,int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where LogID<>0 and TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();
            }
        }

        public List<WalkTowerDevice> SelectedMapList(string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where MapID<>0 and TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();
            }
        }

        public  void GetSleepThreadSave (DeviceContext db, GetSleepThread gtl)
        {
            db.GetSleepThread.Add(gtl);
            db.SaveChanges();
        } 
    }
}