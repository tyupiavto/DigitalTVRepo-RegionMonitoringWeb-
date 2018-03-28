using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using Dapper;

namespace AdminPanelDevice.Controllers
{
    public class MapController : Controller
    {
        DeviceContext db = new DeviceContext();
        public List<TowerGps> towerMap = new List<TowerGps>();

        public struct mapTower
        {
            public double lattitube { get; set; }
            public double longitube { get; set; }
            //public string cityname { get; set; }
        }
        public List<mapTower> TowerMapCord = new List<mapTower>();
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult mapStyle () {

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                towerMap = connection.Query<TowerGps>("Select * From  TowerGps ").ToList();
            }
            mapTower mapt = new mapTower();

            foreach(var item in towerMap)
            {
                item.Lattitube = item.Lattitube.Remove(item.Lattitube.Length - 2);
                mapt.lattitube =Convert.ToDouble( item.Lattitube);
                item.Longitube = item.Longitube.Remove(item.Longitube.Length - 2);
                mapt.longitube = Convert.ToDouble(item.Longitube);
                TowerMapCord.Add(mapt);
            }
            ViewBag.MapGPS = TowerMapCord;

            return View();
        }
    }
}