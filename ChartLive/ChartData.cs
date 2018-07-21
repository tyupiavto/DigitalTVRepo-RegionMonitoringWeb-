using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.ChartLive
{
    public class ChartData
    {
        public ChartData () { }

        public List<WalkTowerDevice> DeviceChartList (int DeviceID, string IP)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
              // var ss= connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where DeviceID='{DeviceID}' and IP='{IP}' and LogID<>0").ToList();
                return connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where DeviceID='{DeviceID}' and IP='{IP}' and LogID<>0").ToList();
            }
        }
        public List<MibGet> SensorGetList(int DeviceID,string IP,string WalkOID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<MibGet>($"select * from MibGet where DeviceID='{DeviceID}' and IP='{IP}' and WalkOID='{WalkOID}'").ToList();
            }
        }

        public List<DeviceType> deviceTypesList ()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<DeviceType>("select * from DeviceType").ToList();
            }
        }
        public List<string> TowerCountList()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<string>("select DISTINCT TowerName from TowerDevices").ToList();
            }
        }

        public List<int> DeviceMaxCount (int ID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {

                return connection.Query<int>($"SELECT count(*) as maxs FROM TowerDevices  as tower where MibID='{ID}' GROUP BY tower.TowerName ORDER BY maxs DESC ").ToList();
            }
        }

        public List<TowerDevices> TowerDevicesList (string device)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<TowerDevices>($"select * from TowerDevices where TowerName='{device}'").ToList();
            }
        }

        public List<WalkTowerDevice> SensorSelected (string IP , string DeviceName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where IP='{IP}' and DeviceName=N'{DeviceName}' and LogID<>0").ToList();
            }
        }
    }
}