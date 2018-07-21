using AdminPanelDevice.Models;
using Dapper;
using IToolS.IOServers.Snmp;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class SnmpVersionTwo
    {
        DeviceContext db = new DeviceContext();
        Hexstring hex = new Hexstring();
        AlarmDefineColor alarmstatus = new AlarmDefineColor();
        AlarmStatusDescription alarmStatusDescription = new AlarmStatusDescription();

        public SnmpVersionTwo(SnmpV2Packet pkt, EndPoint inep, List<MibTreeInformation> mibTreeInformation, List<TowerDevices> towerDevices, List<AlarmLogStatus> alarmLog)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<HubMessage>();
        
                foreach (Vb v in pkt.Pdu.VbList)
                {
                    Trap trap = new Trap();
                    string IP = inep.ToString();
                    var leng = IP.IndexOf(":");
                    trap.IpAddres = IP.Remove(leng, (IP.Length - leng));
                    trap.CurrentOID = pkt.Pdu.TrapObjectID.ToString();
                    trap.ReturnedOID = v.Oid.ToString();
                    trap.Value = v.Value.ToString();
                    trap.dateTimeTrap = DateTime.Now.ToString();
                    var tDevice = towerDevices.Where(t => t.IP == trap.IpAddres).FirstOrDefault();

                    alarmStatusDescription = alarmstatus.AlarmColorDefines(v.Value.ToString(),trap.CurrentOID,trap.ReturnedOID, alarmLog,tDevice);

                    trap.AlarmStatus = alarmStatusDescription.AlarmStatusColor;
                    trap.AlarmDescription = alarmStatusDescription.AlarmDescription;

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
                        string oid = pkt.Pdu.TrapObjectID.ToString();
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

                            if (OidMibdescription != null)
                            {
                                trap.Description = OidMibdescription.Description;
                            }
                            else
                            {
                                trap.Description = "Unknown";
                                trap.OIDName = "Unknown";
                            }
                            trap.OIDName = OidMibdescription.Name;
                        }
                        else
                        {
                                trap.Description = OidMibdescription.Description;
                                trap.OIDName = OidMibdescription.Name;
                        }
                        if (trap.Description == "")
                        {
                            trap.Description = "Unknown";
                        }
                    }
                    context.Clients.All.onHitRecorded(trap);
                    db.Traps.Add(trap);
                    db.SaveChanges();
                }
        }
    }
}