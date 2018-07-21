﻿using AdminPanelDevice.LiveTrap;
using AdminPanelDevice.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
namespace AdminPanelDevice.Controllers
{
    public class LiveTrapController : Controller
    {
        public static List<Trap> LiveTrapErrorCrashList = new List<Trap>();
        public LiveTrapPresentation liveTrapPresentation = new LiveTrapPresentation();
        // GET: LiveTrap
        public ActionResult Index()
        {
             return View();
        }

        public ActionResult LiveTrap ()
        {
         return View(liveTrapPresentation.TrapListHeaderCheckSelected());
        }
       
        [HttpPost]
        public PartialViewResult TrapCurrentAlarms ()
        {
            LiveTrapErrorCrashList.Clear();
            //LiveTrapErrorCrashList = liveTrapPresentation.TrapCurrentAlarmResult();

            //var correctError = liveTrapPresentation.TrapCurrentAlarmCount(LiveTrapErrorCrashList);
            //ViewBag.ErrorCount = correctError.ErrorCount.ToString();
            //ViewBag.CrashCount = correctError.CrashCount.ToString();

            return PartialView("_LiveTrapError", LiveTrapErrorCrashList);
        }

        [HttpPost]
        public PartialViewResult LiveTrapError (Trap TrapResponse)
        {
            LiveTrapErrorCrashList =liveTrapPresentation.LiveTrapList(LiveTrapErrorCrashList, TrapResponse);

            var correctError = liveTrapPresentation.TrapCurrentAlarmCount(LiveTrapErrorCrashList);
            ViewBag.ErrorCount = correctError.ErrorCount.ToString();
            ViewBag.CrashCount = correctError.CrashCount.ToString();
            return PartialView("_LiveTrapError", LiveTrapErrorCrashList);
        }

        [HttpPost]
        public PartialViewResult TrapClearArchive(int archiveInd)
        {
            if (archiveInd == 1)
            {
                LiveTrapErrorCrashList = liveTrapPresentation.TrapCurrentAlarmResult();

                var correctError = liveTrapPresentation.TrapCurrentAlarmCount(LiveTrapErrorCrashList);
                ViewBag.ErrorCount = correctError.ErrorCount.ToString();
                ViewBag.CrashCount = correctError.CrashCount.ToString();
            }
            else
            {
                ViewBag.ErrorCount = 0;
                ViewBag.CrashCount = 0;
                LiveTrapErrorCrashList.Clear();
            }
            return PartialView("_LiveTrapError", LiveTrapErrorCrashList);
        }

        [HttpPost]
        public JsonResult ClearTrapLog ()
        {
            liveTrapPresentation.TrapLogClear();
            return Json("");
        }
    }
}