using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using Dapper;
using PagedList;

namespace AdminPanelDevice.Controllers
{
    public class MapController : Controller
    {
        DeviceContext db = new DeviceContext();
        public List<TowerGps> towerMap = new List<TowerGps>();

        public List<mapTower> TowerMapCord = new List<mapTower>();
        public List<LineConnection> TowerLine = new List<LineConnection>();
        public List<mapLine> LinesCon = new List<mapLine>();

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult style()
        {
            //var html = System.IO.File.ReadAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\HtmlText\style.txt");
            var html = System.IO.File.ReadAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\MapStyle\start.txt");

            return Json(html);
        }
        [HttpPost]
        public ActionResult openMap ()
        {
            var dat = DateTime.Now;

            return Json(Url.Action("mapStyle","Map"));
        }

        public ActionResult mapStyle () {

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                towerMap = connection.Query<TowerGps>("Select * From  TowerGps ").ToList();
                TowerLine = connection.Query<LineConnection>("Select * From LineConnection").ToList();
                TowerLine= TowerLine.OrderBy(o => o.ParentTowerID).ToList();

                    towerMap.ForEach(item =>
                    {
                        mapTower mapt = new mapTower();
                        mapt.lattitube = Double.Parse(item.Lattitube.Remove(item.Lattitube.Length - 2), CultureInfo.InvariantCulture);
                        mapt.longitube = Double.Parse(item.Longitube.Remove(item.Longitube.Length - 2), CultureInfo.InvariantCulture);
                        if (item.PresetName != null)
                        {
                            mapt.cityname = item.PresetName;
                        }

                        DateTime start = DateTime.Now;
                        DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                        var alarmColor = connection.Query<Trap>($"select * from Trap where dateTimeTrap BETWEEN '{end}' and '{start}' and TowerName='{item.TowerName}'").ToList();

                        if (alarmColor.Count != 0)
                        {
                            if (alarmColor.LastOrDefault().AlarmStatus == "green" || alarmColor.LastOrDefault().AlarmStatus == "white" || alarmColor.LastOrDefault().AlarmStatus==null)
                            {
                                mapt.AlarmColor = "rgb(51, 51, 51)";
                                mapt.TextColor = "white";
                            }
                            else
                            {
                                mapt.AlarmColor = alarmColor.LastOrDefault().AlarmStatus;
                                mapt.TextColor = "white";
                            }
                            if (alarmColor.LastOrDefault().AlarmStatus == "yellow")
                            {
                                mapt.TextColor = "black";
                            }
                        }
                       if (alarmColor.Count==0 || alarmColor.LastOrDefault().AlarmStatus=="white") {
                            mapt.AlarmColor = "rgb(51, 51, 51)";
                        }
                        mapt.towerID = item.TowerID;
                        mapt.towerCityName = item.TowerName.Substring(0, item.TowerName.IndexOf('_'));
                        TowerMapCord.Add(mapt);
                    });

                    TowerLine.ForEach(line =>
                    {
                        mapLine ml = new mapLine();

                        var latlon = TowerMapCord.Where(t => t.towerID == line.ParentTowerID).FirstOrDefault();
                        ml.parentlattitube = latlon.lattitube;
                        ml.parentlongitube = latlon.longitube;
                        if (latlon.AlarmColor == "rgb(51, 51, 51)")
                        {
                            ml.AlarmLineColor = "#006699";
                        }
                        else
                        {
                            ml.AlarmLineColor = latlon.AlarmColor;
                        }
                        latlon = TowerMapCord.Where(t => t.towerID == line.ChildTowerID).FirstOrDefault();
                        ml.childlattitube = latlon.lattitube;
                        ml.childlongitube = latlon.longitube;
                        LinesCon.Add(ml);
                    });

               // LinesCon.Add(new mapLine());
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
            var linCon = db.LineConnections.Where(l => l.ParentTowerID == parentTowerID && l.ChildTowerID == childTowerID).FirstOrDefault();
            if (linCon != null)
            {
                db.LineConnections.Remove(linCon);
                db.SaveChanges();
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult MapStyle ()
        {
            string style = System.IO.File.ReadAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\MapStyle\mapstyle.txt");
            ViewBag.Style = style;

          
            return Json("");
        }

    }
}