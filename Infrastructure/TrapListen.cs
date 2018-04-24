using AdminPanelDevice.Models;
using Dapper;
using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class TrapListen
    {
        DeviceContext db = new DeviceContext();
        public TrapListen()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 162);
            EndPoint ep = (EndPoint)ipep;
            socket.Bind(ep);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var mibTreeInformation = connection.Query<MibTreeInformation>("select * from TreeInformation").ToList();
                bool run = true;
                int inlen = -1;
                while (run)
                {
                    byte[] indata = new byte[16 * 1024];

                    IPEndPoint peer = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint inep = (EndPoint)peer;
                    try
                    {
                        inlen = socket.ReceiveFrom(indata, ref inep);
                    }
                    catch (Exception ex)
                    {
                        inlen = -1;
                    }
                    if (inlen > 0)
                    {
                        var towerDevices = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                        // Check protocol version int 
                        int ver = SnmpPacket.GetProtocolVersion(indata, inlen);
                        if (ver == 0)
                        {
                            try
                            {
                                //SnmpV1Packet pkt = new SnmpV1Packet();
                                SnmpV1TrapPacket pkt = new SnmpV1TrapPacket();
                                pkt.decode(indata, inlen);

                                foreach (Vb v in pkt.Pdu.VbList)
                                {
                                    Trap trap = new Trap();
                                    string IP = inep.ToString();
                                    trap.IpAddres = pkt.Pdu.AgentAddress.ToString();
                                    trap.CurrentOID = pkt.Pdu.Enterprise.ToString();
                                    trap.ReturnedOID = v.Oid.ToString();
                                    trap.Value = v.Value.ToString();
                                    trap.dateTimeTrap = DateTime.Now;
                                    var tDevice = towerDevices.Where(t => t.IP == pkt.Pdu.AgentAddress.ToString()).FirstOrDefault();
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
                                            trap.Description = "Not Description";
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
                                        trap.Description = "Not Description";
                                    }
                                    db.Traps.Add(trap);
                                    db.SaveChanges();
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }

                        if (ver == 2 || ver == 1)
                        {
                            try
                            {

                                SnmpV2Packet pkt = new SnmpV2Packet();
                                pkt.decode(indata, inlen);

                                foreach (Vb v in pkt.Pdu.VbList)
                                {
                                    Trap trap = new Trap();
                                    string IP = inep.ToString();
                                    trap.IpAddres = IP.Remove(13, (IP.Length - 13));
                                    trap.CurrentOID = pkt.Pdu.TrapObjectID.ToString();
                                    trap.ReturnedOID = v.Oid.ToString();
                                    trap.Value = v.Value.ToString();
                                    trap.dateTimeTrap = DateTime.Now;
                                    var tDevice = towerDevices.Where(t => t.IP == trap.IpAddres).FirstOrDefault();
                                    trap.Countrie = tDevice.CountrieName;
                                    trap.States = tDevice.StateName;
                                    trap.City = tDevice.CityName;
                                    trap.DeviceName = tDevice.DeviceName;
                                    trap.TowerName = tDevice.TowerName;
                                    string oid = pkt.Pdu.TrapObjectID.ToString();
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
                                            trap.Description = "Not Description";
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
                                        trap.Description = "Not Description";
                                    }

                                    db.Traps.Add(trap);
                                    db.SaveChanges();
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    //else
                    //{
                    //    if (inlen == 0)
                    //        Console.WriteLine("Zero length packet received.");

                    //}
                }
            }
        }


    }
}