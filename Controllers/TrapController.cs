using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using PagedList;

namespace AdminPanelDevice.Controllers
{
    public class TrapController : Controller
    {
        public static List<TrapLog> TrapLogList = new List<TrapLog>();
        public static List<TrapLog> TrapLogListSearch = new List<TrapLog>();

        public static int SearchIndicator = 0;

        DeviceContext db = new DeviceContext();
        // GET: Trap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendTrap(string FileName)
        {

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 162);
            EndPoint ep = (EndPoint)ipep;
            socket.Bind(ep);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

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
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult LogSetting(int? page)
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult LogShow(int? page)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                DateTime start = DateTime.Now;
                DateTime end = start.Add(new TimeSpan(-72, 0, 0));
                var trap = connection.Query<Trap>("select * from Trap where dateTimeTrap BETWEEN '" + end + "'and '" + start + "'").ToList();
                var Countrie = connection.Query<Countrie>("select * from Countrie").ToList();
                var States = connection.Query<States>("select * from States").ToList();
                var City = connection.Query<City>("select * from City").ToList();
                var towerDevices = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                var mibTreeInformation = connection.Query<MibTreeInformation>("select * from TreeInformation").ToList();

                TrapLogList.Clear();
                int ID = 1;
                foreach (var item in trap)
                {
                    TrapLog traplog = new TrapLog();
                    var towerDevice = towerDevices.Where(t => t.IP == item.IpAddres).FirstOrDefault();
                    if (towerDevice != null)
                    {
                        var cityName = towerDevice.TowerName;
                        cityName = cityName.Remove(cityName.IndexOf("\u00AD_"), cityName.Length - cityName.IndexOf("\u00AD_"));
                        var stateID = City.Where(c => c.CityName == cityName).FirstOrDefault().StateID;
                        string StateName = States.Where(s => s.ID == stateID).FirstOrDefault().StateName;
                        //connection.Query<States>("select * from States where ID='" + stateID + "'").FirstOrDefault().StateName;
                        var countrieID = States.Where(s => s.StateName == StateName).FirstOrDefault().CountrieID;
                        //connection.Query<States>("select * from States where StateName='" + StateName + "'").FirstOrDefault().CountrieID;
                        traplog.Countrie = Countrie.Where(c => c.ID == countrieID).FirstOrDefault().CountrieName;
                        //connection.Query<Countrie>("select * from Countrie where ID='" + countrieID + "'").FirstOrDefault().CountrieName;
                        traplog.States = StateName;
                        traplog.City = cityName;
                        traplog.DeviceName = towerDevice.DeviceName;
                        traplog.TowerName = towerDevice.TowerName;
                    }

                    traplog.IP = item.IpAddres;
                    traplog.Value = item.Value;
                    traplog.OID = item.CurrentOID;
                    traplog.DateTime = item.dateTimeTrap;
                    traplog.ID = ID;
                    ID++;

                    string oid = item.CurrentOID;
                    //  var OidMibdescription = db.MibTreeInformations.Where(m => m.OID == oid).FirstOrDefault();
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
                            traplog.Description = OidMibdescription.Description;
                        else
                        {
                            traplog.Description = "Not Description";
                        }
                    }
                    else
                    {
                        if (OidMibdescription.Description != null)
                        {
                            traplog.Description = OidMibdescription.Description;
                        }
                    }
                    if (traplog.Description == "")
                    {
                         traplog.Description = "Not Description";
                    }
                    TrapLogList.Add(traplog);
                }
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1,20));
            }
        }
        public PartialViewResult PageLog(int? page)
        {
            if (SearchIndicator == 0)
            {
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, 20));
            }
            else
            {
                return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, 20));
            }
        }

        [HttpPost]
        public PartialViewResult LogSearch(int? page, string SearchName,int SearchInd)
        {
            if (SearchInd == 0)
            {
                SearchIndicator = 0;
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, 20));
            }
            else
            {
                SearchIndicator = 1;
                TrapLogListSearch.Clear();
                TrapLogListSearch = TrapLogList.Where(s => s.Countrie.Contains(SearchName) || s.States.Contains(SearchName) || s.City.Contains(SearchName) || s.TowerName.Contains(SearchName) || s.DeviceName.Contains(SearchName) || s.Description.Contains(SearchName) || s.IP.Contains(SearchName) || s.OID.Contains(SearchName) || s.Value.Contains(SearchName)).ToList();
                return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, 20));
            }
        }
    }
}