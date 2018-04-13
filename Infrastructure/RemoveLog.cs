using System;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanelDevice.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Threading.Tasks;

namespace AdminPanelDevice.Infrastructure
{
    public class RemoveLog : IJob
    {
        DeviceContext db = new DeviceContext();
        public void Execute(IJobExecutionContext context)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                con.Open();
               // string str = "RemoveTrapTime";
                using (SqlCommand cmd = new SqlCommand("[dbo].[RemoveTrapTime]", con))
                {
                    DateTime original = DateTime.Now;
                    DateTime start = original.Add(new TimeSpan(-6, 0, 0));
                    DateTime end = original.Add(new TimeSpan(-3, 20, 0));
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@startTime", start));
                    cmd.Parameters.Add(new SqlParameter("@endTime", end));
                    cmd.ExecuteNonQuery();
                }
               con.Close();
            }
        }
    }
}