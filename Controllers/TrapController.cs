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
using AdminPanelDevice.Infrastructure;
using System.Threading;
using System.Globalization;

namespace AdminPanelDevice.Controllers
{
    public class TrapController : Controller
    {
        public static bool trapInd = true;
        public static List<Trap> TrapLogList = new List<Trap>();
        public static List<Trap> TrapLogListSearch = new List<Trap>();

        public static int SearchIndicator = 0;

        DeviceContext db = new DeviceContext();
        // GET: Trap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendTrap()
        {
          
            if (trapInd == true)
            {
                trapInd = false;
                new TrapListen();
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
                DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{ end }' and '{ start}'").ToList();
                TrapLogList= TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
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
        public PartialViewResult LogSearch(int? page, string SearchName,int SearchClear, string startTime, string endTime)
        {
            if (SearchName == "" && startTime != "" && endTime != "")
            {
                using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                {
                    var startTm = DateTime.ParseExact(startTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    var EndTm = DateTime.ParseExact(endTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{startTm}' and '{EndTm}'").ToList();
                    TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, 20));
                }
            }
            else
            {

                if (SearchClear == 0)
                {
                    SearchIndicator = 0;
                    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, 20));
                }
                else
                {
                    SearchIndicator = 1;
                    TrapLogListSearch.Clear();
                    TrapLogListSearch = TrapLogList.Where(s => s.Countrie.Contains(SearchName) || s.States.Contains(SearchName) || s.City.Contains(SearchName) || s.TowerName.Contains(SearchName) || s.DeviceName.Contains(SearchName) || s.Description != null && s.Description.Contains(SearchName) || s.IpAddres.Contains(SearchName) || s.CurrentOID.Contains(SearchName) || s.Value.Contains(SearchName)).ToList();
                    TrapLogListSearch = TrapLogListSearch.OrderByDescending(t => t.dateTimeTrap).ToList();
                    return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, 20));
                }
            }
        }

        //[HttpPost]
        //public PartialViewResult SearchDateTime(int? page, DateTime startTime, DateTime endTime)
        //{
        //    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
        //    {
        //        TrapLogList = connection.Query<Trap>("select * from Trap where dateTimeTrap BETWEEN '" + startTime + "'and '" + endTime + "'").ToList();
        //        return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, 20));
        //    }
        //}
        [HttpGet]
        public JsonResult TrapFillNewDevice(string IPaddress)
        {
            Thread thread = new Thread(() => new TrapUpdateNewDevice(IPaddress));
            thread.Start();
          //  new TrapUpdateNewDevice(IPaddress);
            return Json("",JsonRequestBehavior.AllowGet);
            
        }
        [HttpGet]
        public JsonResult NotTrapFill (string IPaddress)
        {
            Thread thread = new Thread(() => new TrapDelete(IPaddress));
            thread.Start();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlarmLog (string alarmColor, string deviceName, string alarmText)
        {
           var alarmtextdecode= System.Uri.UnescapeDataString(alarmText);
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{

            //}
            AlarmLogStatus alarmlog = new AlarmLogStatus();
            alarmlog.AlarmStatus = alarmColor;
            alarmlog.AlarmText = alarmtextdecode;
            alarmlog.DeviceName = deviceName;

            db.AlarmLogStatus.Add(alarmlog);
            db.SaveChanges();

            return Json("");
        }
    }
}