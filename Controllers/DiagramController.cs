using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using PagedList;
using PagedList.Mvc;
using System.Drawing;

namespace AdminPanelDevice.Controllers
{
    public class DiagramController : Controller
    {
        DeviceContext db = new DeviceContext();
       public static List<WalkDevice> wlk = new List<WalkDevice>();
        public static string device_tower_IP;
        public static int IDX = 0;
        public static int device;
        public struct DiagramPoint
        {
            public int X;
            public double Y;
        }
      public static  List<DiagramPoint> Ycordinat = new List<DiagramPoint>();
        // GET: Diagram
        public ActionResult Index()
        {
            //device_tower_IP = device_IP;
            device_tower_IP = "192.168.36.13";
            device = db.devices.Where(d => d.IP == device_tower_IP).FirstOrDefault().ID;
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
            var mibget = db.MibGets.Where(m => m.DeviceID == device).ToList().LastOrDefault();
                List<DiagramPoint> LiveDiagram = new List<DiagramPoint>();
                DiagramPoint cordinat = new DiagramPoint();
                cordinat.X = IDX;
                cordinat.Y = Convert.ToDouble(mibget.Value);
                cordinat.Y = cordinat.Y - 898000.0;
                LiveDiagram.Add(cordinat);
            IDX++;
          
            return Json(new { LiveDiagram }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ChartStatic()
        {
            var mibget = db.MibGets.Where(m => m.DeviceID == device).ToList();
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