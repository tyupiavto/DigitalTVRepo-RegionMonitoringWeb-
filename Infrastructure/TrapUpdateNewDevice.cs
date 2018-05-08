using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class TrapUpdateNewDevice
    {
        DeviceContext db = new DeviceContext();
        public TrapUpdateNewDevice(string IPaddress)
        {
            try
            {
                DateTime end = DateTime.Now;
                DateTime start = end.Add(new TimeSpan(-524, 0, 0));
                db.Database.ExecuteSqlCommand("[dbo].[TrapFill] @startTime,@endTime,@IPaddres",
                   new SqlParameter("@startTime", start),
                   new SqlParameter("@endTime", end),
                   new SqlParameter("@IPaddres", IPaddress));
            }
            catch { }
            }
    }
}