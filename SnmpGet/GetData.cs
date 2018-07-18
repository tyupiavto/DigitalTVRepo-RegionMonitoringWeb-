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
        
        public WalkTowerDevice CheckLog(int checkID,string towerName,int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where LogID<>0 and WalkID='{checkID}' and TowerName='{towerName}' and DeviceID='{deviceID}'").FirstOrDefault();
            }
        }
        public WalkTowerDevice CheckMap(int checkID, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where MapID<>0 and WalkID='{checkID}' and TowerName='{towerName}' and DeviceID='{deviceID}'").FirstOrDefault();
            }
        }
        public GetSleepThread GetCheckFirsDefine (string towerName,int deviceID,int towerID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
              return  connection.Query<GetSleepThread>($"select * from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}'").FirstOrDefault();
            }
        }

        public GetSleepThread MapLogExistence(string towerName, int deviceID, int towerID,int checkID) {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<GetSleepThread>($"select * from  GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}' and CheckID='{checkID}'").FirstOrDefault();
            }
        }

        public void SelectedLog (int LogMapStatus,string towerName, int deviceID, int towerID, int checkID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<GetSleepThread>($"update GetSleepThread set LogID='{LogMapStatus}' where TowerName='{towerName}' and DeviceID='{deviceID}' and WalkID='{checkID}'");
            }
        }
        public void SelectedMap(int LogMapStatus,string towerName, int deviceID, int towerID, int checkID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<GetSleepThread>($"update GetSleepThread set MapID='{LogMapStatus}' where TowerName='{towerName}' and DeviceID='{deviceID}' and TowerID='{towerID}' and CheckID='{checkID}'");
            }
        }

        public int LogSelectedCount (int chechkLog, int walkCheckID, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where WalkID='{walkCheckID}'and TowerName='{towerName}' and DeviceID='{deviceID}' and LogID<>0").ToList().Count;
            }
        }

        public void UpdateLog(int LogMapStatus, string towerName, int deviceID, int checkID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>($"Update WalkTowerDevice Set LogID='{LogMapStatus}' where WalkID='{checkID}'and TowerName='{towerName}' and DeviceID='{deviceID}'");
            }
        }
    }
}