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

        public List<Trap> OneDeyList(DateTime end, DateTime start )
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}'").ToList();
            }
        }
    }
}