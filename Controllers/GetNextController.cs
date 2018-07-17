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
using AdminPanelDevice.SnmpGet;
namespace AdminPanelDevice.Controllers
{
    public class GetNextController : Controller
    {
        public  DeviceContext db = new DeviceContext();
        public static List<GetSleepThread> getThread = new List<GetSleepThread>();
        public SnmpPacket result;
        public static bool treadListInd = true;
        public GetThread getThreadPreset=new GetThread();
        SleepInformation returnedThreadList = new SleepInformation();
        public UpdateCheck updateCheck = new UpdateCheck();
        CheckUncheckGetThread checkgetthread = new CheckUncheckGetThread();
        GetPresentation getPresentation = new GetPresentation();
    
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult Get(string towerName ,int towerID, int deviceID)
        {
           // GetThreadPlayStop get = new GetThreadPlayStop();

            //var getOnOof= get.Get(getThread, returnedThreadList, towerName, deviceID, towerID,db, getThreadPreset);
            //var getOnOof = getPresentation.DeviceTheadOnOff(getThread, returnedThreadList, towerName, deviceID, towerID, db, getThreadPreset);
            //if (getOnOof == false )
            //{
            //    return Json("1", JsonRequestBehavior.AllowGet);
            //}
            return Json(getPresentation.DeviceTheadOnOff(getThread, returnedThreadList, towerName, deviceID, towerID, db, getThreadPreset), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStop (string towerName, List<int> stopGet)
        {
            //GetThreadPlayStop stop = new GetThreadPlayStop();
            //stop.StopThread(getThread, returnedThreadList, towerName, stopGet);
             getPresentation.StopThread(getThread, returnedThreadList, towerName, stopGet);
            return Json("");
        }

        [HttpPost]
        public JsonResult GetPlay(string towerName,int towerID, List<int> playGet)
        {
            //GetThreadPlayStop play = new GetThreadPlayStop();
            //var playstoplist = play.PlayThread(treadListInd, getThread, returnedThreadList, playGet, towerName, towerID, db, getThreadPreset);
            //return Json(playstoplist);
            return Json(getPresentation.PlayTheadDevice(treadListInd, getThread, returnedThreadList, playGet, towerName, towerID, db, getThreadPreset));
        }


        [HttpPost]
        public JsonResult UncheckLog(int unChechkID, string towerName, int deviceID,int towerID) // uncheckd log
        {
            updateCheck.UpdateChechkLog(0, unChechkID, towerName, deviceID);
            CheckUncheckGetThread uncheckLog = new CheckUncheckGetThread();
            getThread=uncheckLog.UnCheckdGet(unChechkID, towerName, deviceID, towerID, "Log", getThread);
            return Json("");
        }

        [HttpPost]
        public JsonResult CheckLog(int chechkID, string towerName, int deviceID,int towerID,bool logStartStopPlay) // checked log
        {
          var LogCount=updateCheck.UpdateChechkLog(1, chechkID, towerName, deviceID);
         
            getThread.Add(checkgetthread.checkdGet(chechkID, towerName, deviceID, db, towerID,"Log",LogCount, logStartStopPlay));
            return Json("");
        }

        [HttpPost]
        public JsonResult UncheckMap(int unChechkID, string towerName, int deviceID, int towerID) // unchecked map
        {
            updateCheck.UpdateChechkMap(0, unChechkID, towerName, deviceID);
            CheckUncheckGetThread uncheckMap = new CheckUncheckGetThread();
            getThread = uncheckMap.UnCheckdGet(unChechkID, towerName, deviceID, towerID, "Map", getThread);
            return Json("");
        }

        [HttpPost]
        public JsonResult CheckMap(int chechkID, string towerName, int deviceID,int towerID, bool mapStartStopPlay) // checked map
        {
           var MapCount=updateCheck.UpdateChechkMap(1, chechkID, towerName, deviceID);
            getThread.Add(checkgetthread.checkdGet(chechkID, towerName, deviceID, db, towerID, "Map",MapCount, mapStartStopPlay));
            return Json("");
        }

        [HttpPost]
        public JsonResult IntervalSearch(int intervalID, int Interval, string towerName, int deviceID,int towerID)
        {
            updateCheck.UpdateInterval(intervalID, Interval, towerName, deviceID);
            CheckUncheckGetThread intervalChange = new CheckUncheckGetThread();
            getThread = intervalChange.ChangeInterval(intervalID, Interval, towerName, deviceID, towerID, getThread);
            return Json("");
        }

        [HttpPost]
        public JsonResult StringParser(int checkParser, int walkID, string towerName, int deviceID,int towerID)
        {       
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>($"update WalkTowerDevice Set StringParserInd='{checkParser}'  where WalkID='{walkID}'and TowerName='{towerName}' and DeviceID='{deviceID}'");
                connection.Query<GetSleepThread>($"update GetSleepThread Set StringParserInd='{checkParser}'  where CheckID='{walkID}'and TowerName='{towerName}' and DeviceID='{deviceID}'");
                getThread = HangfireBootstrapper.Instance.GetThreadStart();
                if (getThread.Count != 0)
                {
                    var th = getThread.Where(gt => gt.CheckID == walkID && gt.DeviceID == deviceID && gt.TowerName == towerName).FirstOrDefault();
                    if (th != null)
                    {
                        th.thread.Abort();

                        //getThread.Remove(th);
                        getThread.Where(gt => gt.CheckID == walkID && gt.DeviceID == deviceID && gt.TowerName == towerName).FirstOrDefault().thread = new Thread(() => getThreadPreset.ThreadPreset(th.WalkID, checkParser, th.DivideMultiply, th.ID, towerID, th.IP, th.ScanInterval, th.DeviceID, th.WalkOid, th.Version, th.StartCorrect, th.EndCorrect, th.OneStartError, th.OneEndError, th.OneStartCrash, th.OneEndCrash, th.TwoStartError, th.TwoEndError, th.TwoStartCrash, th.TwoEndCrash));
                        //getThread.Where(gt => gt.CheckID == walkID && gt.DeviceID == deviceID && gt.TowerName == towerName).FirstOrDefault().thread.Suspend();
                        getThread.Where(gt => gt.CheckID == walkID && gt.DeviceID == deviceID && gt.TowerName == towerName).FirstOrDefault().thread.Start();
                    }
                    else
                    {
                      //  getThread.Add(new Thread(() => getThreadPreset.ThreadPreset(th.WalkID, checkParser, th.DivideMultiply, th.ID, towerID, th.IP, th.ScanInterval, th.DeviceID, th.WalkOid, th.Version, th.StartCorrect, th.EndCorrect, th.OneStartError, th.OneEndError, th.OneStartCrash, th.OneEndCrash, th.TwoStartError, th.TwoEndError, th.TwoStartCrash, th.TwoEndCrash)));
                    }
                }
            }
            return Json("");
        }
        [HttpPost]
        public JsonResult ParserCheck(int walkID, string towerName, int deviceID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var pasrserID = connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where WalkID='{walkID}'and TowerName='{towerName}' and DeviceID='{deviceID}'").FirstOrDefault();

                if (pasrserID.StringParserInd == 0)
                {
                    return Json("0");
                }
                else
                {
                    return Json("1");
                }
            }
        }
    }
}