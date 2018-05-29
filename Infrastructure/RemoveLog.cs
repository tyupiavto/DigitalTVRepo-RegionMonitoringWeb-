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
    public class RemoveLog/* : IJob*/
    {
        DeviceContext db = new DeviceContext();
        public void Execute(IJobExecutionContext context)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                con.Open();
                // string str = "RemoveTrapTime";
                //using (SqlCommand cmd = new SqlCommand("[dbo].[RemoveTrapTime]", con))
                //{
                //    DateTime original = DateTime.Now;
                //    DateTime start = original.Add(new TimeSpan(-6, 0, 0));
                //    DateTime end = original.Add(new TimeSpan(-3, 20, 0));
                //    cmd.CommandType=CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@startTime", start));
                //    cmd.Parameters.Add(new SqlParameter("@endTime", end));
                //    cmd.ExecuteNonQuery();
                //}
                //using (SqlCommand cmd = new SqlCommand("[dbo].[TrapTableList]", con))
                //{
                //    DateTime original = DateTime.Now;
                //    DateTime start = original.Add(new TimeSpan(-2, 0, 0));
                //    DateTime end = original.Add(new TimeSpan(0, 0, 0));
                //  //  cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@startTime", start));
                //    cmd.Parameters.Add(new SqlParameter("@endTime", end));
                //    var table = cmd.ExecuteScalar();
                //}
              //var trap = con.Query<Trap>("select * from Trap where dateTimeTrap BETWEEN '2018-04-01 16:09:57.560' and '2018-04-21 12:09:48.470'").ToList();
               
                
              //    var tbl=con.Query<Trap>("select * from TrapTableList('2018-04-01 16:09:57.560','2018-04-21 21:09:48.470')").ToList();

                con.Close();
            }
        }
    }
}