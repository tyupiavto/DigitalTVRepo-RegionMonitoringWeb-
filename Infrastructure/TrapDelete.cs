using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class TrapDelete
    {
        DeviceContext db = new DeviceContext();
        public TrapDelete(string IPaddress)
        {
            DateTime end = DateTime.Now;
            DateTime start = end.Add(new TimeSpan(-524, 0, 0));
            db.Database.ExecuteSqlCommand("[dbo].[DeleteTrap] @startTime,@endTime,@IPaddres",
               new SqlParameter("@startTime", start),
               new SqlParameter("@endTime", end),
               new SqlParameter("@IPaddres", IPaddress));
        }
    }
}