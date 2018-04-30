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
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class WalkSendReturned
    {
        private SnmpPacket result;
        private int walkTimeOutOID;
        private int WalkTimeOunt;
        private int ID;
        public WalkSendReturned()
        {

        }
        public List<WalkTowerDevice> WalkSendReturn(string IP, int Port, string Version, string communityRead, List<WalkTowerDevice> walkList, string towerName, string DeviceName)
        {
    
            string walkTimeOutOID = "";
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var mibInf = connection.Query<MibTreeInformation>("Select * From  [TreeInformation]").ToList();
                // MibWalkIndicator = false;

                //PresetIND = 0;
                //WalkTimeOunt = 0;
                //walkList.Clear();
                //GPSCoordinate.Clear();
                //CheckedLog.Clear();
                //CheckedMap.Clear();
                //IPadrress = IP;
                //ViewBag.IP = IPadrress;

                OctetString community = new OctetString(communityRead);

                AgentParameters param = new AgentParameters(community);
                if (Version == "V1")
                {
                    param.Version = SnmpVersion.Ver1;
                }
                if (Version == "V2")
                {
                    param.Version = SnmpVersion.Ver2;
                }
                IpAddress agent = new IpAddress(IP);

                UdpTarget target = new UdpTarget((IPAddress)agent, Port, 2000, 1);
                Oid rootOid = new Oid(".1.3.6.1.4");

                Oid lastOid = (Oid)rootOid.Clone();

                Pdu pdu = new Pdu(PduType.GetNext);

                while (lastOid != null)
                {
                    try
                    {
                        if (pdu.RequestId != 0)
                        {
                            pdu.RequestId += 1;
                        }
                        pdu.VbList.Clear();
                        pdu.VbList.Add(lastOid);
                        if (walkTimeOutOID == lastOid.ToString())
                        {
                            WalkTimeOunt++;
                        }
                        if (WalkTimeOunt <= 10)
                        {
                            walkTimeOutOID = lastOid.ToString();
                        }
                        else
                        {
                            return walkList;
                        }

                        if (Version == "V1")
                        {
                            result = (SnmpV1Packet)target.Request(pdu, param);
                        }
                        if (Version == "V2")
                        {
                            result = (SnmpV2Packet)target.Request(pdu, param);
                        }

                        if (result != null)
                        {
                            if (result.Pdu.ErrorStatus != 0)
                            {
                                lastOid = null;
                                break;
                            }
                            else
                            {
                                foreach (Vb v in result.Pdu.VbList)
                                {

                                    if (rootOid.IsRootOf(v.Oid))
                                    {
                                        WalkTowerDevice walk = new WalkTowerDevice();
                                        ID++;
                                        walk.ID = ID;
                                        walk.WalkID = ID;
                                        string oid = v.Oid.ToString();
                                        var OidMibdescription = mibInf.Where(m => m.OID == oid).FirstOrDefault();
                                        if (OidMibdescription == null)
                                        {
                                            oid = oid.Remove(oid.Length - 1);
                                            oid = oid.Remove(oid.Length - 1);
                                            OidMibdescription = mibInf.Where(o => o.OID == oid).FirstOrDefault();
                                        }
                                        if (OidMibdescription == null)
                                        {
                                            oid = oid.Remove(oid.Length - 1);
                                            oid = oid.Remove(oid.Length - 1);
                                            OidMibdescription = mibInf.Where(o => o.OID == oid).FirstOrDefault();

                                            if (OidMibdescription != null)
                                                walk.WalkDescription = OidMibdescription.Description;
                                        }
                                        else
                                        {
                                            walk.WalkDescription = OidMibdescription.Description;
                                        }
                                        if (OidMibdescription != null)
                                            walk.WalkDescription = OidMibdescription.Description;

                                        walk.WalkOID = v.Oid.ToString();
                                        walk.OIDName = mibInf.Where(o => o.OID == oid).FirstOrDefault().Name;
                                        walk.Type = v.Value.ToString();
                                        walk.Value = SnmpConstants.GetTypeName(v.Value.Type);
                                        walk.ScanInterval = 60;
                                        walk.DeviceName = DeviceName;
                                        walk.TowerName = towerName;
                                        walkList.Add(walk);
                                        lastOid = v.Oid;

                                    }
                                    else
                                    {
                                        lastOid = null;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e) { }
                }
                target.Close();
            }
            return walkList;
        }

    }
}