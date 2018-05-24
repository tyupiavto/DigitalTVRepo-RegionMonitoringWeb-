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
        public  DeviceContext db = new DeviceContext();
        public static List<GetSleepThread> getThread = new List<GetSleepThread>();
        public SnmpPacket result;
        public static bool treadListInd = true;
        public GetThread getThreadPreset=new GetThread();
        SleepInformation returnedThreadList = new SleepInformation();
        public UpdateCheck updateCheck = new UpdateCheck();
        CheckUncheckGetThread checkgetthread = new CheckUncheckGetThread();
        // GET: GetNext 
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult Get(string towerName ,int towerID, int deviceID)
        {
            GetThreadPlayStop get = new GetThreadPlayStop();

           var getbool= get.Get(getThread, returnedThreadList, towerName, deviceID, towerID,db, getThreadPreset);
            if (getbool==false )
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStop (string towerName, List<int> stopGet)
        {
            GetThreadPlayStop stop = new GetThreadPlayStop();
            stop.StopThread(getThread, returnedThreadList, towerName, stopGet);
            return Json("");
        }

        [HttpPost]
        public JsonResult GetPlay(string towerName,int towerID, List<int> playGet)
        {
            GetThreadPlayStop play = new GetThreadPlayStop();
            play.PlayThread(treadListInd,getThread,returnedThreadList,playGet,towerName,towerID,db,getThreadPreset);
            return Json("");
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
        public JsonResult CheckLog(int chechkID, string towerName, int deviceID,int towerID) // checked log
        {
            updateCheck.UpdateChechkLog(1, chechkID, towerName, deviceID);
         
            getThread.Add(checkgetthread.checkdGet(chechkID, towerName, deviceID, db, towerID,"Log"));
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
        public JsonResult CheckMap(int chechkID, string towerName, int deviceID,int towerID) // checked map
        {
            updateCheck.UpdateChechkMap(1, chechkID, towerName, deviceID);
            getThread.Add(checkgetthread.checkdGet(chechkID, towerName, deviceID, db, towerID, "Map"));
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
    }
}