using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using PagedList;
using PagedList.Mvc;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Globalization;

namespace AdminPanelDevice.Controllers
{
    public class DiagramController : Controller
    {
        DeviceContext db = new DeviceContext();
       public static List<WalkDevice> wlk = new List<WalkDevice>();
        public static string device_tower_IP;
        public static int IDX = 0;
        public static MibGet device;
        public static int indicator = 0;
        public struct DiagramPoint
        {
            public int X;
            public double Y;
        }
      public static  List<DiagramPoint> Ycordinat = new List<DiagramPoint>();
        // GET: Diagram
        public ActionResult Index()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                device_tower_IP = "192.168.24.12";
                device = connection.Query<MibGet>($"select * from MibGet where IP='{device_tower_IP}'").FirstOrDefault();
            }
            //    
            //device = db.MibGets.Where(d => d.IP =='192.168.24.12').FirstOrDefault().ID;
            //page = 1;
            return View();
        }

        [HttpPost]
        public PartialViewResult PresetDiagramSearch(int? page, string device_IP)
        {
            page = 1;
            var device = db.devices.Where(d => d.IP == device_IP).FirstOrDefault().DeviceTID;
            var preset = db.Presets.Where(p => p.DeviceTypeID == device).FirstOrDefault().ID;
            var walkdevice = db.WalkDevices.Where(w => w.PresetID == preset).ToList();
            //List<WalkDevice> wlk = new List<WalkDevice>();

            wlk.AddRange(walkdevice);
            ViewBag.IP = device_tower_IP;
            return PartialView("_PresetDiagramSearch", wlk.ToPagedList(page ?? 1,1000));
        }

        public JsonResult ChartLive()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                List<DiagramPoint> LiveDiagram = new List<DiagramPoint>();
                if (indicator == 0)
                {
                    indicator = 1;
                    var livetime = DateTime.Now;
                    var end = livetime.Add(new TimeSpan(-1, 0, 0));
                    var mibgets = connection.Query<MibGet>($"select * from MibGet where DeviceID='{device.DeviceID}'and WalkOID='1.3.6.1.4.1.23180.2.1.1.1.3.8.2.1.14.1.2'").ToList();
                    //List<DiagramPoint> StaticDiagram = new List<DiagramPoint>();
                    DiagramPoint cordinate = new DiagramPoint();
                    foreach (var item in mibgets)
                    {
                        cordinate.X = IDX;
                        string mibvalues = item.Value.Substring(0, item.Value.Length - 5);
                        cordinate.Y = double.Parse(mibvalues, CultureInfo.InvariantCulture);
                        //cordinate.Y = Convert.ToDouble(item.Value);
                        //cordinate.Y = cordinate.Y/* - 898000.0*/;
                        LiveDiagram.Add(cordinate);
                        IDX++;
                    }

                }
                //var mibget = db.MibGets.Where(m => m.DeviceID == device.ID).ToList().LastOrDefault();
                var mibget = connection.Query<MibGet>($"select * from MibGet where DeviceID='{device.DeviceID}'and WalkOID='1.3.6.1.4.1.23180.2.1.1.1.3.8.2.1.14.1.2'").LastOrDefault();
                //List<DiagramPoint> LiveDiagram = new List<DiagramPoint>();
                DiagramPoint cordinat = new DiagramPoint();
                cordinat.X = IDX;
                string mibvalue = mibget.Value.Substring(0, mibget.Value.Length - 5);
                cordinat.Y = double.Parse(mibvalue, CultureInfo.InvariantCulture);
                //cordinat.Y = cordinat.Y;
                LiveDiagram.Add(cordinat);
                IDX++;

                return Json(new { LiveDiagram }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ChartStatic()
        {
            //var mibget = db.MibGets.Where(m => m.DeviceID == device.ID).ToList();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                //var mibget = db.MibGets.Where(m => m.DeviceID == device.ID).ToList().LastOrDefault();
                var mibget = connection.Query<MibGet>($"select * from MibGet where DeviceID='{device.DeviceID}'and WalkOID='1.3.6.1.4.1.23180.2.1.1.1.3.8.2.1.14.1.2'").ToList();
                List<DiagramPoint> StaticDiagram = new List<DiagramPoint>();
                DiagramPoint cordinat = new DiagramPoint();
                foreach (var item in mibget)
                {
                    cordinat.X = IDX;
                    cordinat.Y = Convert.ToDouble(item.Value);
                    cordinat.Y = cordinat.Y - 898000.0;
                    StaticDiagram.Add(cordinat);
                    IDX++;
                }
                return Json(new { StaticDiagram }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}