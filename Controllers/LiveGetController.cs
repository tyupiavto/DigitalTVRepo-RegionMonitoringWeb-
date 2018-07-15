using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Web;
using AdminPanelDevice.Models;
using System.Web.Mvc;
using AdminPanelDevice.Infrastructure;
using AdminPanelDevice.ChartLive;

namespace AdminPanelDevice.Controllers
{
    public class LiveGetController : Controller
    {
        public ChartSensorList chartList = new ChartSensorList();
        public ChartPrezentation chartPrezentation = new ChartPrezentation();
        public static int DeviceIDInf;
        public static string IPInf;
        // GET: LiveTrapGet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LiveGet()
        {

            return View();
        }

        [HttpPost]
        public PartialViewResult GetLiveSensor()
        {
                ViewBag.Sensor=chartPrezentation.AllSelectedSensor();
                return PartialView("_GetLiveInformation");
        }

        public ActionResult GetChart()
        {

            return View();
        }

        [HttpPost]
        public PartialViewResult ChartSensorLive()
        {
            var ch = chartPrezentation.ChartSensorResult(DeviceIDInf, IPInf);
            ViewBag.SensorDeviceCount = ch.SensorDeviceCount;
            ViewBag.SensorGetResult = ch.SensorGetResult;
            return PartialView("_ChartLiveSensor", ch);
        }

        [HttpPost]
        public JsonResult DeviceInformation(int DeviceID, string IP)
        {
            DeviceIDInf = DeviceID;
            IPInf = IP;
            return Json("");
        }
    }
}