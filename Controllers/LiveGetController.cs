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

namespace AdminPanelDevice.Controllers
{
    public class LiveGetController : Controller
    {
        // GET: LiveTrapGet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LiveGet ()
        {

            return View();
        }

        public PartialViewResult GetLiveSensor ()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var Sensors = connection.Query<WalkTowerDevice>("select * from  WalkTowerDevice where IP='192.168.4.42' and MapID<>0 and LogID<>0").ToList();

                return PartialView("_GetLiveInformation", Sensors);
            }
        }

    }
}