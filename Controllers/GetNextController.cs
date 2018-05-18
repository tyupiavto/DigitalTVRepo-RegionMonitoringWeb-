using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using AdminPanelDevice.Infrastructure;

namespace AdminPanelDevice.Controllers
{
    public class GetNextController : Controller
    {
        public List<string> getoid = new List<string>();
        public static List<WalkDevice> walkList = new List<WalkDevice>();

        public static List<Pdu> pdu = new List<Pdu>();

        public static List<presetThread> presetInf = new List<presetThread>();
        public static List<Thread> the = new List<Thread>();
        public List<MibGet> mib = new List<MibGet>();
        public List<MibGet> mibs = new List<MibGet>();
        public int count = -1;
        public int devID = 0;
        static bool Thr = true;
        public static List<WalkDevice> walkdvc = new List<WalkDevice>();
        public static DeviceContext db = new DeviceContext();
        public LiveValue live = new LiveValue();

        public static List<GetThreadListen> getThread = new List<GetThreadListen>();
        public SnmpPacket result;
        public static int thbool = 0;
        public GetThread getThreadPreset=new GetThread();
        // GET: GetNext 
        public ActionResult Index()
        {
            return View();
        }

        //public void ThreadPreset(string IP, int time, int Deviceid,string getOid)
        //{
        //    string Version = "V2";
        //    string communityRead = "public";
        //    int Port = 161;
        //    while (Thr == true)
        //    {
        //        OctetString community = new OctetString(communityRead);

        //        AgentParameters param = new AgentParameters(community);
        //        if (Version == "V1")
        //        {
        //            param.Version = SnmpVersion.Ver1;
        //        }
        //        if (Version == "V2")
        //        {
        //            param.Version = SnmpVersion.Ver2;
        //        }
        //        IpAddress agent = new IpAddress(IP);

        //        UdpTarget target = new UdpTarget((IPAddress)agent, Port, 2000, 1);

        //        Pdu pdu = new Pdu(PduType.Get);
        //        try
        //        {
        //            if (pdu.RequestId != 0)
        //            {
        //                pdu.RequestId += 1;
        //            }
        //            pdu.VbList.Clear();
        //            pdu.VbList.Add(getOid);

        //            if (Version == "V1")
        //            {
        //                result = (SnmpV1Packet)target.Request(pdu, param);
        //            }
        //            if (Version == "V2")
        //            {
        //                result = (SnmpV2Packet)target.Request(pdu, param);
        //            }

        //            foreach (Vb v in result.Pdu.VbList)
        //            {
                       
        //                MibGet get = new MibGet();
        //                get.Value = v.Value.ToString();
        //                get.DeviceID = Deviceid;
        //                get.dateTime = DateTime.Now;
        //                get.WalkOID = v.Oid.ToString();

        //                db.MibGets.Add(get);
        //                db.SaveChanges();
        //            }
        //            }
        //        catch (Exception e) { }

        //        target.Close();
        //        Thread.Sleep((time * 1000));
        //    }
        //}

        [HttpPost]
        public JsonResult Get(string towerName ,int towerID, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                DeviceContext db = new DeviceContext();
                walkdvc = db.WalkDevices.ToList();
                Oid rootOid = new Oid(); // ifDescr
                Oid lastOid = new Oid();

                var oofDevice = connection.Query<DeviceThreadOnOff>("select * from DeviceThreadOnOff where TowerName='" + towerName + "' and DeviceID='" + deviceID + "'").ToList();
                if (oofDevice.Count != 0)
                {
                    connection.Query<DeviceThreadOnOff>("delete from DeviceThreadOnOff where TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
                    var thb = getThread.Where(t => t.towerName == towerName && t.deviceID == deviceID).ToList();
                    thb.ForEach(t =>
                    {
                        t.thread.Abort();
                    });
                    var toweroff = connection.Query<DeviceThreadOnOff>("select * from  DeviceThreadOnOff where TowerID='" + towerID + "'").FirstOrDefault();
                    if (toweroff != null)
                    {
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var Log = connection.Query<WalkTowerDevice>("select * from WalkTowerDevice where LogID<>0 and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'").ToList();
                    if (Log != null)
                    {
                        DeviceThreadOnOff on = new DeviceThreadOnOff();
                        on.DeviceID = deviceID;
                        on.TowerName = towerName;
                        on.TowerID = towerID;

                        db.DeviceThreadOnOff.Add(on);
                        db.SaveChanges();
                    }

                    Log.ForEach(l =>
                    {
                        GetThreadListen gtl = new GetThreadListen();
                        gtl.deviceID = l.DeviceID;
                        gtl.towerName = l.TowerName;
                        gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID,l.Version));
                        gtl.thread.Start();
                        getThread.Add(gtl);
                    });
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
              
        }

        [HttpPost]
        public JsonResult GetStop (string towerName, List<int> stopGet)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                stopGet.ForEach(deviceID =>
                {
                    var oofDevice = connection.Query<DeviceThreadOnOff>("select * from DeviceThreadOnOff where TowerName='" + towerName + "' and DeviceID='" + deviceID + "'").ToList();
                    if (oofDevice.Count != 0)
                    {
                        connection.Query<DeviceThreadOnOff>("delete from DeviceThreadOnOff where TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
                        var thb = getThread.Where(t => t.towerName == towerName && t.deviceID == deviceID).ToList();
                        thb.ForEach(t =>
                        {
                            t.thread.Abort();
                        });
                    }
                });
            }
                return Json("");
        }

        [HttpPost]
        public JsonResult GetPlay(string towerName,int towerID, List<int> playGet)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                playGet.ForEach(deviceID =>
                {
                    var Log = connection.Query<WalkTowerDevice>("select * from WalkTowerDevice where LogID<>0 and TowerName='" + towerName + "' and DeviceID='" + deviceID + "'").ToList();
                if (Log != null)
                {
                    DeviceThreadOnOff on = new DeviceThreadOnOff();
                    on.DeviceID = deviceID;
                    on.TowerName = towerName;
                    on.TowerID = towerID;

                    db.DeviceThreadOnOff.Add(on);
                    db.SaveChanges();
                }

                Log.ForEach(l =>
                {
                    GetThreadListen gtl = new GetThreadListen();
                    gtl.deviceID = l.DeviceID;
                    gtl.towerName = l.TowerName;
                    gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID,l.Version));
                    gtl.thread.Start();
                    getThread.Add(gtl);
                });
                });
            }
                
                return Json("");
        }
    }
}