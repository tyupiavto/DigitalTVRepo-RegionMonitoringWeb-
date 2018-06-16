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
using System.Text.RegularExpressions;

namespace AdminPanelDevice.Controllers
{
    public class TrapController : Controller
    {
        public static bool trapInd = true;
        public static List<Trap> TrapLogList = new List<Trap>();
        public static List<Trap> TrapLogListSearch = new List<Trap>();
        public static int pageListNumber=20;
        public static int SearchIndicator = 0;
        public static int maplog=0;

        DeviceContext db = new DeviceContext();
        // GET: Trap
        public ActionResult Index()
        {
            maplog = 0;
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
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var checkList = connection.Query<TrapListNameCheck>("select * from TrapListNameCheck ").ToList();
                ViewBag.TrapCheck = checkList;
                return View(checkList);
            }
        }

        [HttpPost]
        public PartialViewResult LogShow(int? page)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                ViewBag.pageNumber = pageListNumber;
                if (maplog == 0)
                {
                    DateTime start = DateTime.Now;
                    DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                    TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{ end }' and '{ start}'").ToList();
                    TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                    ViewBag.pageNumber = pageListNumber;
                }
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            }
        }
        public PartialViewResult PageLog(int? page)
        {
            //pageListNumber = listNumber;
            ViewBag.pageNumber = pageListNumber;
            if (SearchIndicator == 0)
            {
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
            }
        }
        public PartialViewResult PageLogList(int? page, int listNumber)
        {
            pageListNumber = listNumber;
            ViewBag.pageNumber = pageListNumber;
            if (SearchIndicator == 0)
            {
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
            }
        }

        [HttpPost]
        public PartialViewResult LogSearch(int? page, string SearchName,int SearchClear, string startTime, string endTime)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                ViewBag.pageNumber = pageListNumber;
                if (SearchName == "" && startTime != "" && startTime != null && endTime != "" && endTime != null)
                {
                    var startTm = DateTime.ParseExact(startTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    var EndTm = DateTime.ParseExact(endTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{startTm}' and '{EndTm}'").ToList();
                    TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
                }
                else
                {
                    if (SearchName == "" && startTime == "" && endTime == "")
                    {
                        maplog = 0;
                        DateTime start = DateTime.Now;
                        DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                        TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{ end }' and '{ start}'").ToList();
                        TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                        return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
                    }
                    else
                    {

                        if (SearchClear == 0)
                        {
                            SearchIndicator = 0;
                            return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
                        }
                        else
                        {
                            SearchIndicator = 1;
                            TrapLogListSearch.Clear();

                            var searchName = SearchName.First().ToString().ToUpper() + SearchName.Substring(1);
                            TrapLogListSearch = TrapLogList.Where(s => s.Countrie.Contains(SearchName) || s.States.Contains(SearchName) || s.City.Contains(SearchName) || s.TowerName.Contains(SearchName) || s.DeviceName.Contains(SearchName) || s.Description != null && s.Description.Contains(SearchName) || s.OIDName != null && s.OIDName.Contains(SearchName) || s.IpAddres.Contains(SearchName) || s.CurrentOID.Contains(SearchName) || s.ReturnedOID.Contains(SearchName) || s.Value.Contains(SearchName) || s.AlarmDescription != null && s.AlarmDescription.Contains(SearchName) || s.Countrie.Contains(searchName) || s.States.Contains(searchName) || s.City.Contains(searchName) || s.TowerName.Contains(searchName) || s.DeviceName.Contains(searchName) || s.Description != null && s.Description.Contains(searchName) || s.OIDName != null && s.OIDName.Contains(searchName) || s.IpAddres.Contains(searchName) || s.CurrentOID.Contains(searchName) || s.ReturnedOID.Contains(searchName) || s.Value.Contains(searchName) || s.AlarmDescription != null && s.AlarmDescription.Contains(searchName)).ToList();
                            TrapLogListSearch = TrapLogListSearch.OrderByDescending(t => t.dateTimeTrap).ToList();
                            return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
                        }
                    }
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
        public JsonResult AlarmLog (string alarmColor, string deviceName, string alarmText,string returnOidText,string currentOidText,string alarmDescription)
        {
           var alarmtextdecode= System.Uri.UnescapeDataString(alarmText);
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<Trap>($"update Trap set AlarmStatus='{alarmColor}',AlarmDescription='{alarmDescription}' where Value like '%{alarmtextdecode}%' and ReturnedOID='{returnOidText}' and CurrentOID='{currentOidText}'");
          
                connection.Query<AlarmLogStatus>($"delete from AlarmLogStatus where AlarmText like '%{alarmtextdecode}%' and ReturnOidText='{returnOidText}' and CurrentOidText='{currentOidText}'");
            }
           
            TrapLogList.ForEach(item =>
            {
             bool status = Regex.IsMatch(item.Value,alarmtextdecode);
             if (status == true && item.CurrentOID == currentOidText && item.ReturnedOID == returnOidText) {
                    item.AlarmStatus = alarmColor;
                    item.AlarmDescription = alarmDescription;
                }
            });

            if (alarmColor != "white")
            {
                AlarmLogStatus alarmlog = new AlarmLogStatus();
                alarmlog.AlarmStatus = alarmColor;
                alarmlog.AlarmText = alarmtextdecode;
                alarmlog.DeviceName = deviceName;
                alarmlog.ReturnOidText = returnOidText;
                alarmlog.CurrentOidText = currentOidText;
                alarmlog.AlarmDescription = alarmDescription;
                db.AlarmLogStatus.Add(alarmlog);
                db.SaveChanges();
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult TrapNameCheck (string trapListName,string check)
        {
            using (IDbConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<TrapListNameCheck>($"update TrapListNameCheck set Checked='{check}' where ListName='{trapListName}'");
            }
                return Json("");
        }

        [HttpPost]
        public PartialViewResult ColorSearch(int? page, string correctColor, string errorColor, string crashColor,string whiteColor, int all)
        {
            ViewBag.pageNumber = pageListNumber;
            if (correctColor == " " && errorColor == " " && crashColor == " " && whiteColor== " ")
            {
                all = 1;
            }
            if (all == 0)
            {
                SearchIndicator = 1;
            
                TrapLogListSearch = TrapLogList.Where(s => s.AlarmStatus!=null && s.AlarmStatus.Contains(correctColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(errorColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(crashColor) || s.AlarmStatus != null &&  s.AlarmStatus.Contains(whiteColor)).ToList();
                return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                SearchIndicator = 0;
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            }
        }


        [HttpPost]
        public PartialViewResult TowerLog(int? page, string mapTowerName, string mapTowerID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                TrapLogList.Clear();
                string towername = mapTowerName + "_Tower" + mapTowerID;
                var deviceList = connection.Query<TowerDevices>($"select * from TowerDevices where TowerName='{towername}'").ToList();
                DateTime start = DateTime.Now;
                DateTime end = start.Add(new TimeSpan(-24, 0, 0));

                deviceList.ForEach(dev =>
                {
                    TrapLogList.AddRange(connection.Query<Trap>($"select * from Trap where  dateTimeTrap BETWEEN '{end}' and '{start}' and IpAddres='{dev.IP}' and AlarmStatus<>'white'").ToList());
                    TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                });

                ViewBag.pageNumber = pageListNumber;
                maplog = 1;
                var listview = connection.Query<TrapListNameCheck>("select * from TrapListNameCheck").ToList();
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            }
        }
    }
}