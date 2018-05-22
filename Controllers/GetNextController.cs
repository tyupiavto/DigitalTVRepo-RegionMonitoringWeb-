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

        public List<GetSleepThread> getThread = new List<GetSleepThread>();
        public SnmpPacket result;
        public static int thbool = 0;
        public static bool treadListInd = true;
        public GetThread getThreadPreset=new GetThread();
        SleepInformation returnedThreadList = new SleepInformation();
        // GET: GetNext 
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult Get(string towerName ,int towerID, int deviceID)
        {
            getThread = returnedThreadList.SleepGetInformation(false);
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var oofDevice = connection.Query<GetSleepThread>($"select * from GetSleepThread where TowerName='{ towerName }' and DeviceID='{deviceID}'").ToList();
                if (oofDevice.Count != 0)
                {
                    connection.Query<GetSleepThread>($"delete from GetSleepThread where TowerName='{towerName}' and DeviceID='{ deviceID }'");
                    var thb = getThread.Where(t => t.TowerName == towerName && t.DeviceID == deviceID).ToList();
                    thb.ForEach(t =>
                    {
                        t.thread.Abort();
                    });
                    var toweroff = connection.Query<GetSleepThread>("select * from  GetSleepThread where TowerID='" + towerID + "'").FirstOrDefault();
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
                   
                    Log.ForEach(l =>
                    {
                        GetSleepThread gtl = new GetSleepThread();
                        gtl.DeviceID = l.DeviceID;
                        gtl.TowerName = l.TowerName;
                        gtl.IP = l.IP;
                        gtl.ScanInterval = l.ScanInterval;
                        gtl.WalkOid = l.WalkOID;
                        gtl.Version = l.Version;
                        gtl.TowerID = towerID;
                        gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version));
                        gtl.thread.Start();
                        getThread.Add(gtl);
                    });
                    db.GetSleepThread.AddRange(getThread);
                    db.SaveChanges();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
              
        }

        [HttpPost]
        public JsonResult GetStop (string towerName, List<int> stopGet)
        {
            getThread = returnedThreadList.SleepGetInformation(false);
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                stopGet.ForEach(deviceID =>
                {
                    var oofDevice = connection.Query<GetSleepThread>($"select * from GetSleepThread where TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();
                    if (oofDevice.Count != 0)
                    {
                        connection.Query<GetSleepThread>("delete from GetSleepThread where TowerName='" + towerName + "' and DeviceID='" + deviceID + "'");
                        var thb = getThread.Where(t => t.TowerName == towerName && t.DeviceID == deviceID).ToList();
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
                    var Log = connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where LogID<>0 and TowerName='{towerName}' and DeviceID='{deviceID}'").ToList();

                    Log.ForEach(l =>
                    {
                        GetSleepThread gtl = new GetSleepThread();
                        gtl.DeviceID = l.DeviceID;
                        gtl.TowerName = l.TowerName;
                        gtl.IP = l.IP;
                        gtl.ScanInterval = l.ScanInterval;
                        gtl.WalkOid = l.WalkOID;
                        gtl.Version = l.Version;
                        gtl.TowerID = towerID;
                        gtl.thread = new Thread(() => getThreadPreset.ThreadPreset(l.IP, l.ScanInterval, l.DeviceID, l.WalkOID, l.Version));
                        gtl.thread.Start();
                        getThread.Add(gtl);
                    });

                });
                db.GetSleepThread.AddRange(getThread);
                db.SaveChanges();
            }

            if (treadListInd == true)
            {
                getThread.AddRange( returnedThreadList.SleepGetInformation(false));
                treadListInd = false;
            }

            return Json("");
        }
    }
}