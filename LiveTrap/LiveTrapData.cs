using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.LiveTrap
{
    public class LiveTrapData
    {
        public LiveTrapData () {}

        public List<TrapListNameCheck> TrapHeaderNameSelectList ()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<TrapListNameCheck>("select * from TrapListNameCheck").ToList();
            }
        }

        public List<Trap> TrapCurrentAlarmList (DateTime start, DateTime end)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}'").ToList();
            }
        }
    }
}