﻿using IToolS.IOServers.Snmp;
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
using Microsoft.AspNet.SignalR;
using AdminPanelDevice.Traps;
namespace AdminPanelDevice.Controllers
{
    public class TrapController : Controller
    {
        public static string mapTowerDeviceName = "";
        public static bool trapInd = true;
        public static List<Trap> TrapLogList = new List<Trap>();
        public static List<Trap> TrapLogListSearch = new List<Trap>();
        public static int pageListNumber = 50;
        public static int SearchIndicator = 0;
        public static int LogInd = 0;
        public TrapPresentation trapPresentation = new TrapPresentation();
       
        DeviceContext db = new DeviceContext();
        // GET: Trap
        public ActionResult Index()
        {
            LogInd = 0;
            return View();
        }

        [HttpPost]
        public JsonResult SendTrap()
        {
           // trapPresentation.SendTrapListenPresentaion(trapInd);
            if (trapInd == true)
            {
                trapInd = false;
                new TrapListen();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult LogSetting(int? page)
        {
                var checkList = trapPresentation.TrapTitleSelectList();
                ViewBag.TrapCheck = checkList;
                return View(checkList);
        }

        [HttpPost]
        public PartialViewResult LogShow(int? page)
        {
            ViewBag.pageNumber = pageListNumber;
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
            //    ViewBag.pageNumber = pageListNumber;
            //    if (LogInd == 0)
            //    {
            //        DateTime start = DateTime.Now;
            //        DateTime end = start.Add(new TimeSpan(-24, 0, 0));
            //        TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}'").ToList();
            //        TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
            //    }
            //    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            //}
            return PartialView("_TrapLogInformation", trapPresentation.TrapLogShow(TrapLogList,LogInd).ToPagedList(page ?? 1, pageListNumber));
        }
        public PartialViewResult PageLog(int? page)
        {
            ViewBag.pageNumber = pageListNumber;
            ViewBag.ColorDefine = 1;
            //if (SearchIndicator == 0 || SearchIndicator==2)
            //{
            //    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            //}
            //else
            //{
            //    return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
            //}
            return PartialView("_TrapLogInformation", trapPresentation.PageLogNumberList(TrapLogList,TrapLogListSearch,SearchIndicator).ToPagedList(page ?? 1, pageListNumber));
        }
        public PartialViewResult LogListCountSize(int? page, int listNumber)
        {
            pageListNumber = listNumber;
           
            TrapLogInformationList trapLogInformationList = new TrapLogInformationList();
            ViewBag.pageNumber = pageListNumber;
            trapLogInformationList = trapPresentation.TrapLogListSize(TrapLogList,TrapLogListSearch,SearchIndicator);
            ViewBag.errorCount = trapLogInformationList.ErrorCount;
            ViewBag.correctCount = trapLogInformationList.CorrectCount;
            ViewBag.crashCount = trapLogInformationList.CrashCount;
            ViewBag.whiteCount = trapLogInformationList.WhiteCount;
            ViewBag.allCount = trapLogInformationList.AllCount;
            //if (SearchIndicator == 0 || SearchIndicator == 2)
            //{
            //    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            //}
            //else
            //{
            //    return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
            //}
            return PartialView("_TrapLogInformation", trapLogInformationList.TrapLogList.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public PartialViewResult LogSearch(int? page, string SearchName, int SearchClear, string startTime, string endTime)
        {
            //TrapLogInformationList trapLogInformationList = new TrapLogInformationList();
            //ViewBag.pageNumber = pageListNumber;
            //trapLogInformationList = trapPresentation.TrapLogSearchList(SearchName, SearchClear, startTime, endTime, TrapLogList, TrapLogListSearch, SearchIndicator, mapTowerDeviceName, LogInd);
            //ViewBag.errorCount = trapLogInformationList.ErrorCount;
            //ViewBag.correctCount = trapLogInformationList.CorrectCount;
            //ViewBag.crashCount = trapLogInformationList.CrashCount;
            //ViewBag.whiteCount = trapLogInformationList.WhiteCount;
            //ViewBag.allCount = trapLogInformationList.AllCount;

            //return PartialView("_TrapLogInformation", trapLogInformationList.TrapLogList.ToPagedList(page ?? 1, pageListNumber));

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                DateTime start = DateTime.Now;
                DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                ViewBag.pageNumber = pageListNumber;
                int trapID;
                if (SearchName == "" && startTime != "" && startTime != null && endTime != "" && endTime != null)
                {
                    ViewBag.ColorDefine = 1;

                    var startTm = DateTime.ParseExact(startTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    var EndTm = DateTime.ParseExact(endTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{startTm}' and '{EndTm}'").ToList();
                    TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();

                    ViewBag.errorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                    ViewBag.correctCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                    ViewBag.crashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                    ViewBag.whiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                    ViewBag.allCount = TrapLogList.Count;
                    TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();

                    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
                }
                else
                {
                    if (SearchName == "" && startTime == "" && endTime == "")
                    {
                        if (SearchIndicator == 2)
                        {
                            TrapLogList = connection.Query<Trap>($"select * from Trap where  dateTimeTrap BETWEEN '{end}' and '{start}' and TowerName='{mapTowerDeviceName}' and AlarmStatus<>'white'").ToList();
                            LogInd = 1;
                        }
                        else
                        {
                            SearchIndicator = 0;
                            LogInd = 0;
                            TrapLogList = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}'").ToList();
                        }
                        ViewBag.errorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                        ViewBag.correctCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                        ViewBag.crashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                        ViewBag.whiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                        ViewBag.allCount = TrapLogList.Count;

                        TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                        return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
                    }
                    else
                    {

                        if (SearchClear == 0)
                        {
                            SearchIndicator = 0;

                            ViewBag.errorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                            ViewBag.correctCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                            ViewBag.crashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                            ViewBag.whiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                            ViewBag.allCount = TrapLogList.Count;

                            return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
                        }
                        else
                        {
                            SearchIndicator = 1;
                            TrapLogListSearch.Clear();
                            int number;
                            var convertationInt = int.TryParse(SearchName, out number);
                            if (convertationInt != false)
                            {
                                trapID = Convert.ToInt32(SearchName);
                            }
                            else
                            {
                                trapID = -1;
                            }

                            var searchName = SearchName.First().ToString().ToUpper() + SearchName.Substring(1);
                            //  TrapLogListSearch = TrapLogList.Where(s => s.Countrie.Contains(SearchName) || s.States.Contains(SearchName) || s.City.Contains(SearchName) || s.TowerName.Contains(SearchName) || s.DeviceName.Contains(SearchName) || s.Description != null && s.Description.Contains(SearchName) || s.OIDName != null && s.OIDName.Contains(SearchName) || s.IpAddres.Contains(SearchName) || s.CurrentOID.Contains(SearchName) || s.ReturnedOID.Contains(SearchName) || s.Value.Contains(SearchName) || s.AlarmDescription != null && s.AlarmDescription.Contains(SearchName) || s.Countrie.Contains(searchName) || s.States.Contains(searchName) || s.City.Contains(searchName) || s.TowerName.Contains(searchName) || s.DeviceName.Contains(searchName) || s.Description != null && s.Description.Contains(searchName) || s.OIDName != null && s.OIDName.Contains(searchName) || s.IpAddres.Contains(searchName) || s.CurrentOID.Contains(searchName) || s.ReturnedOID.Contains(searchName) || s.Value.Contains(searchName) || s.AlarmDescription != null && s.AlarmDescription.Contains(searchName) || s.ID == trapID).ToList();
                            TrapLogListSearch = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}' and ID like N'{searchName}%' or IpAddres like N'{searchName}%' or CurrentOID like N'{searchName}%' or ReturnedOID like N'{searchName}%' or Value like N'{searchName}%' or dateTimeTrap like N'{searchName}%'or Countrie like N'{searchName}%'or States like N'{searchName}%'or City like N'{searchName}%'or TowerName like N'{searchName}%' or DeviceName like N'{searchName}%' or Description like N'{searchName}%' or OIDName like N'{searchName}%'  or AlarmDescription like N'{searchName}%'").ToList();
                            TrapLogListSearch = TrapLogListSearch.OrderByDescending(t => t.dateTimeTrap).ToList();

                            ViewBag.errorCount = TrapLogListSearch.Where(t => t.AlarmStatus == "red").ToList().Count;
                            ViewBag.correctCount = TrapLogListSearch.Where(t => t.AlarmStatus == "green").ToList().Count;
                            ViewBag.crashCount = TrapLogListSearch.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                            ViewBag.whiteCount = TrapLogListSearch.Where(t => t.AlarmStatus == "white").ToList().Count;
                            ViewBag.allCount = TrapLogListSearch.Count;

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

           TrapLogList =trapPresentation.AlarmLogStatusResult(alarmColor, deviceName, alarmText, returnOidText, currentOidText, alarmDescription, TrapLogList, db);
           //var alarmtextdecode= System.Uri.UnescapeDataString(alarmText);
           // using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
           // {

           //     if (alarmColor != "white")
           //     {
           //         connection.Query<Trap>($"update Trap set AlarmStatus='{alarmColor}',AlarmDescription=N'{alarmDescription}' where Value like '%{alarmtextdecode}%' and ReturnedOID='{returnOidText}' and CurrentOID='{currentOidText}'");
           //     }
           //     else
           //     {
           //         alarmDescription = "";
           //         connection.Query<Trap>($"update Trap set AlarmStatus='{alarmColor}',AlarmDescription='{alarmDescription}' where Value like '%{alarmtextdecode}%' and ReturnedOID='{returnOidText}' and CurrentOID='{currentOidText}'");
           //     }
           //     connection.Query<AlarmLogStatus>($"delete from AlarmLogStatus where AlarmText like '%{alarmtextdecode}%' and ReturnOidText='{returnOidText}' and CurrentOidText='{currentOidText}'");
           // }
           
           // TrapLogList.ForEach(item =>
           // {
           //  bool status = Regex.IsMatch(item.Value,alarmtextdecode);
           //  if (status == true && item.CurrentOID == currentOidText && item.ReturnedOID == returnOidText) {
           //         item.AlarmStatus = alarmColor;
           //         item.AlarmDescription = alarmDescription;
           //     }
           // });

           // if (alarmColor != "white")
           // {
           //     AlarmLogStatus alarmlog = new AlarmLogStatus();
           //     alarmlog.AlarmStatus = alarmColor;
           //     alarmlog.AlarmText = alarmtextdecode;
           //     alarmlog.DeviceName = deviceName;
           //     alarmlog.ReturnOidText = returnOidText;
           //     alarmlog.CurrentOidText = currentOidText;
           //     alarmlog.AlarmDescription = alarmDescription;
           //     db.AlarmLogStatus.Add(alarmlog);
           //     db.SaveChanges();
           // }
            return Json("");
        }

        [HttpPost]
        public JsonResult TrapNameCheck(string trapListName, string check)
        {
            //using (IDbConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
            //    connection.Query<TrapListNameCheck>($"update TrapListNameCheck set Checked='{check}' where ListName='{trapListName}'");
            //}
            trapPresentation.TrapSelectName(trapListName, check);
            return Json("");
        }

        [HttpPost]
        public PartialViewResult ColorSearch(int? page, string correctColor, string errorColor, string crashColor,string whiteColor, int all)
        {
            ViewBag.pageNumber = pageListNumber;
            ViewBag.ColorDefine = 1;
            return PartialView("_TrapLogInformation", trapPresentation.AlarmColorSearchList(correctColor, errorColor, crashColor, whiteColor, all, TrapLogListSearch, TrapLogList, SearchIndicator).ToPagedList(page ?? 1, pageListNumber)); 
            //if (correctColor == " " && errorColor == " " && crashColor == " " && whiteColor== " ")
            //{
            //    all = 1;
            //}
            //if (all == 0)
            //{
            //    SearchIndicator = 1;
            
            //    TrapLogListSearch = TrapLogList.Where(s => s.AlarmStatus!=null && s.AlarmStatus.Contains(correctColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(errorColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(crashColor) || s.AlarmStatus != null &&  s.AlarmStatus.Contains(whiteColor)).ToList();
            //    return PartialView("_TrapLogInformation", TrapLogListSearch.ToPagedList(page ?? 1, pageListNumber));
            //}
            //else
            //{
            //    SearchIndicator = 0;
            //    return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            //}
        }


        [HttpPost]
        public PartialViewResult TowerLog(int? page, string mapTowerName, string mapTowerID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                //SearchIndicator = 0;
                //TrapLogList.Clear();
                //string towername = mapTowerName + "_Tower" + mapTowerID;
                mapTowerDeviceName = mapTowerName + "_Tower" + mapTowerID;
                //    var deviceList = connection.Query<TowerDevices>($"select * from TowerDevices where TowerName='{towername}'").ToList();
                //DateTime start = DateTime.Now;
                //DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                SearchIndicator = 2;
                //deviceList.ForEach(dev =>
                //{
                //    TrapLogList=connection.Query<Trap>($"select * from Trap where  dateTimeTrap BETWEEN '{end}' and '{start}' and TowerName='{towername}' and AlarmStatus<>'white'").ToList();
                //    SearchIndicator = 2;
                ////});
                //TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
                //ViewBag.pageNumber = pageListNumber;
                //maplog = 1;
            //    var listview = connection.Query<TrapListNameCheck>("select * from TrapListNameCheck").ToList();
                return PartialView("_TrapLogInformation", TrapLogList.ToPagedList(page ?? 1, pageListNumber));
            }
        }
    }
}