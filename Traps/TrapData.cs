using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Traps
{
    public class TrapData
    {
        public TrapData() { }

        public List<Trap> DateTimeSearchLog(DateTime end, DateTime start)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}'").ToList();
            }
        }
        public List<Trap> MapTowerLog (DateTime end, DateTime start,string mapTowerDeviceName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Trap>($"select * from Trap where  dateTimeTrap BETWEEN '{end}' and '{start}' and TowerName='{mapTowerDeviceName}' and AlarmStatus<>'white'").ToList();
            }
        }

        public void AlarmLogStatusUpdate (string alarmColor, string returnOidText, string currentOidText, string alarmDescription,string alarmtextdecode)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<Trap>($"update Trap set AlarmStatus='{alarmColor}',AlarmDescription=N'{alarmDescription}' where Value like '%{alarmtextdecode}%' and ReturnedOID='{returnOidText}' and CurrentOID='{currentOidText}'");
            }
        }
        public void AlarmLogStatusDelete (string alarmColor, string returnOidText, string currentOidText, string alarmtextdecode)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<AlarmLogStatus>($"delete from AlarmLogStatus where AlarmText like '%{alarmtextdecode}%' and ReturnOidText='{returnOidText}' and CurrentOidText='{currentOidText}'");
            }
        }

        public void TrapNameSelectUpdate (string trapListName, string check)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<TrapListNameCheck>($"update TrapListNameCheck set Checked='{check}' where ListName='{trapListName}'");
            }
        }

        public List<TrapListNameCheck> TrapTitleSelectedListReturn()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<TrapListNameCheck>("select * from TrapListNameCheck ").ToList();
            }
        }

        public List<MibTreeInformation> MibTreeInformationList()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<MibTreeInformation>("select * from TreeInformation").ToList();
            }
        }

        public List<TowerDevices> TowerDevicesList()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<TowerDevices>("select * from TowerDevices").ToList();
            }
        }

        public List<AlarmLogStatus> AlarmLogStatusList()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<AlarmLogStatus>("select * from TowerDevices").ToList();
            }
        }

        public void AlarmLogStatusSave (DeviceContext db, AlarmLogStatus alarmlog)
        {
            db.AlarmLogStatus.Add(alarmlog);
            db.SaveChanges();
        }
    }
}