using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanelDevice.Models;
using AdminPanelDevice.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Net;

namespace AdminPanelDevice.Infrastructure
{
    public class SnmpVersionOne
    {
        DeviceContext db = new DeviceContext();
        Hexstring hex = new Hexstring();
        public SnmpVersionOne (SnmpV1TrapPacket pkt, EndPoint inep)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var mibTreeInformation = connection.Query<MibTreeInformation>("select * from TreeInformation").ToList();
                var towerDevices = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                foreach (Vb v in pkt.Pdu.VbList)
                {
                    Trap trap = new Trap();
                    string IP = inep.ToString();
                    trap.IpAddres = pkt.Pdu.AgentAddress.ToString();
                    trap.CurrentOID = pkt.Pdu.Enterprise.ToString();
                    trap.ReturnedOID = v.Oid.ToString();
                    trap.dateTimeTrap = DateTime.Now;
                    if (v.Value.GetType().Name == "OctetString")
                    {
                        trap.Value = hex.Hexstrings(v.Value.ToString());
                    }
                    else
                    {
                        trap.Value = v.Value.ToString();
                    }
                    var tDevice = towerDevices.Where(t => t.IP == pkt.Pdu.AgentAddress.ToString()).FirstOrDefault();
                    if (tDevice == null)
                    {
                        trap.Countrie = "Unknown";
                        trap.States = "Unknown";
                        trap.City = "Unknown";
                        trap.DeviceName = "Unknown";
                        trap.TowerName = "Unknown";
                        trap.Description = "Unknown";
                    }
                    else
                    {
                        trap.Countrie = tDevice.CountrieName;
                        trap.States = tDevice.StateName;
                        trap.City = tDevice.CityName;
                        trap.DeviceName = tDevice.DeviceName;
                        trap.TowerName = tDevice.TowerName;

                        string oid = pkt.Pdu.Enterprise.ToString();
                        var OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
                        if (OidMibdescription == null)
                        {
                            oid = oid.Remove(oid.Length - 1);
                            oid = oid.Remove(oid.Length - 1);
                            OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
                        }
                        if (OidMibdescription == null)
                        {
                            oid = oid.Remove(oid.Length - 1);
                            oid = oid.Remove(oid.Length - 1);
                            OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();

                            if (OidMibdescription != null)
                                trap.Description = OidMibdescription.Description;
                            else
                            {
                                trap.Description = "Unknown";
                            }
                        }
                        else
                        {
                            if (OidMibdescription.Description != null)
                            {
                                trap.Description = OidMibdescription.Description;
                            }
                        }
                        if (trap.Description == "")
                        {
                            trap.Description = "Unknown";
                        }
                    }
                    db.Traps.Add(trap);
                    db.SaveChanges();
                }
            }
        }
    }
}