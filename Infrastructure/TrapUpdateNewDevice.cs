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
        public TrapUpdateNewDevice(string IPaddress)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                DateTime start = DateTime.Now;
                DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                var trapnewDev = connection.Query<Trap>("select * from Trap where dateTimeTrap BETWEEN '" + end + "'and '" + start + "'and IpAddres='" + IPaddress + "'").ToList();
                var mibTreeInformation = connection.Query<MibTreeInformation>("select * from TreeInformation").ToList();
               // var towerDevices = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                var towdev = connection.Query<TowerDevices>("select * from TowerDevices where IP='" + IPaddress + "'").FirstOrDefault();
                foreach (var item in trapnewDev)
                {
                    string oid = item.CurrentOID;
                    string description = "";
                    var OidMibdescription = mibTreeInformation.Where(o => o.OID == oid).FirstOrDefault();
                    if (OidMibdescription == null)
                    {
                        oid = oid.Remove(oid.Length - 1);
                        oid = oid.Remove(oid.Length - 1);
                        OidMibdescription = mibTreeInformation.Where(o => o.OID == oid).FirstOrDefault();
                    }
                    if (OidMibdescription == null)
                    {
                        oid = oid.Remove(oid.Length - 1);
                        oid = oid.Remove(oid.Length - 1);
                        OidMibdescription = mibTreeInformation.Where(o => o.OID == oid).FirstOrDefault();
                    }
                    else
                    {
                        if (OidMibdescription.Description != null)
                        {
                            description = OidMibdescription.Description;
                        }
                    }
                    if (description == "")
                    {
                        description = "Unknown";
                    }
                    connection.Query<Trap>("update Trap SET Countrie='"+ towdev.CountrieName + "', States='"+ towdev.StateName+"',City='"+ towdev.CityName + "',Description='"+ description + "',TowerName='"+ towdev.TowerName + "',DeviceName='"+ towdev.DeviceName + "' where ID='"+ item.ID + "'");
                }
            }
        }
    }
}