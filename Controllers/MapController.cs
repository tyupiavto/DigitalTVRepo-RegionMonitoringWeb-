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
            public string cityname { get; set; }
            public int towerID { get; set; }
        }
        public struct mapLine
        {
            public double parentlattitube { get; set; }
            public double parentlongitube { get; set; }
            public double childlattitube { get; set; }
            public double childlongitube { get; set; }
        }
        public List<mapTower> TowerMapCord = new List<mapTower>();
        public List<LineConnection> TowerLine = new List<LineConnection>();
        public List<mapLine> LinesCon = new List<mapLine>();
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult mapStyle () {

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                towerMap = connection.Query<TowerGps>("Select * From  TowerGps ").ToList();
                TowerLine = connection.Query<LineConnection>("Select * From LineConnection").ToList();
                TowerLine= TowerLine.OrderBy(o => o.ParentTowerID).ToList();

                foreach (var item in towerMap)
                {
                    mapTower mapt = new mapTower();
                    item.Lattitube = item.Lattitube.Remove(item.Lattitube.Length - 2);
                    mapt.lattitube = Convert.ToDouble(item.Lattitube);
                    item.Longitube = item.Longitube.Remove(item.Longitube.Length - 2);
                    mapt.longitube = Convert.ToDouble(item.Longitube);
                    if (item.PresetName != null)
                    {
                        mapt.cityname = item.PresetName;
                    }
                    mapt.towerID = item.ID;

                    TowerMapCord.Add(mapt);
                }

                foreach (var line in TowerLine)
                {
                    mapLine ml = new mapLine();
                    var latlon = TowerMapCord.Where(t => t.towerID == line.ParentTowerID).FirstOrDefault();
                    ml.parentlattitube = latlon.lattitube;
                    ml.parentlongitube = latlon.longitube;
                    latlon= TowerMapCord.Where(t => t.towerID == line.ChildTowerID).FirstOrDefault();
                    ml.childlattitube = latlon.lattitube;
                    ml.childlongitube = latlon.longitube;
                    LinesCon.Add(ml);
                }
                LinesCon.Add(new mapLine());
                ViewBag.MapGPS = TowerMapCord;
                ViewBag.TowerLine = LinesCon;

            }
            return View();
        }

        [HttpPost]
        public JsonResult AddTowerLine(int parentTowerID, int childTowerID)
        {
            LineConnection towerLine = new LineConnection();
            towerLine.ParentTowerID = parentTowerID;
            towerLine.ChildTowerID = childTowerID;

            db.LineConnections.Add(towerLine);
            db.SaveChanges();

            return Json("");
        }
        [HttpPost]
        public JsonResult RemoveTowerLine (int parentTowerID, int childTowerID)
        {
            //LineConnection linCon = new LineConnection();
            //linCon.ParentTowerID = parentTowerID;
            //linCon.ChildTowerID = childTowerID;
            var linCon = db.LineConnections.Where(l => l.ParentTowerID == parentTowerID && l.ChildTowerID == childTowerID).FirstOrDefault();
            if (linCon != null)
            {
                db.LineConnections.Remove(linCon);
                db.SaveChanges();
            }
            return Json("");
        }
    }
}