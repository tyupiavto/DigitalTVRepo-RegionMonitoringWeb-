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
                return  connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where DeviceID='{DeviceID}' and IP='{IP}' and LogID<>0 and MapID<>0").ToList();
            }
        }
        public List<MibGet> SensorGetList(int DeviceID,string IP,string WalkOID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<MibGet>($"select * from MibGet where DeviceID='{DeviceID}' and IP='{IP}' and WalkOID='{WalkOID}'").ToList();
            }
        }
    }
}