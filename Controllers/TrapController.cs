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
                TrapLogList = connection.Query<Trap>("select * from Trap where dateTimeTrap BETWEEN '" + end + "'and '" + start + "'").ToList();
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
                TrapLogListSearch = TrapLogList.Where(s => s.Countrie.Contains(SearchName) || s.States.Contains(SearchName) || s.City.Contains(SearchName) || s.TowerName.Contains(SearchName) || s.DeviceName.Contains(SearchName) || s.Description!=null && s.Description.Contains(SearchName) || s.IpAddres.Contains(SearchName) || s.CurrentOID.Contains(SearchName) || s.Value.Contains(SearchName)).ToList();
                return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, 20));
            }
        }

        [HttpPost]
        public PartialViewResult SearchDateTime(int? page, DateTime startTime, DateTime endTime)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                //TrapLogListSearch.Clear();
                //TrapLogListSearch = TrapLogList.Where(t => t.dateTimeTrap >= startTime && t.dateTimeTrap <= endTime).ToList();
                TrapLogList = connection.Query<Trap>("select * from Trap where dateTimeTrap BETWEEN '" + startTime + "'and '" + endTime + "'").ToList();
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, 20));
            }
        }
        [HttpGet]
        public JsonResult TrapFillNewDevice(string IPaddress)
        {
            Thread thread = new Thread(() => new TrapUpdateNewDevice(IPaddress));
            thread.Start();
           // new TrapUpdateNewDevice(IPaddress);
            return Json("",JsonRequestBehavior.AllowGet);
            
        }
        [HttpGet]
        public JsonResult NotTrapFill (string IPaddress)
        {
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
            //    DateTime start = DateTime.Now;
            //    DateTime end = start.Add(new TimeSpan(-24, 0, 0));
            //    connection.Query<Trap>("delete from Trap where dateTimeTrap BETWEEN '" + end + "'and '" + start + "'and IpAddres='" + IPaddress + "'");
            //}
            Thread thread = new Thread(() => new TrapDelete(IPaddress));
            thread.Start();
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}