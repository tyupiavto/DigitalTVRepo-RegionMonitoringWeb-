using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
namespace AdminPanelDevice.Controllers
{
    public class TowerController : Controller
    {
        DeviceContext db = new DeviceContext();
        List<Tower> TowerInf = new List<Tower>();
        AdminPanelDevice.Models.Devices devices = new AdminPanelDevice.Models.Devices();
        Tower tower = new Tower();
        public static int k;
        // GET: Tower
        public ActionResult Index(string ID, string Name)
        {
            ViewBag.DeviceType = db.devicesTypes.ToList();
            return View();
        }

        public ActionResult TowerOpen()
        {
            TowerInf = db.towers.ToList();

            return View(TowerInf);
        }


        public ActionResult TowerEdit(int? ID) {
            var Tow = db.towers.Where(t => t.ID == ID).FirstOrDefault();
            return View(Tow);
        }

        [HttpPost]
        public JsonResult TowerUpdate(string ID, string Name, string LattiTube, string LongiTube, string IP, string Phone, string Status)
        {
            Tower Update = new Tower();
            Update.ID = Convert.ToInt32(ID);
            Update.Name = Name;
            //Update.LattiTube = Convert.ToDouble(LattiTube);
            //Update.LongiTube = Convert.ToDouble(LongiTube);
            //Update.IP = IP;
            //Update.Phone = Convert.ToInt32(Phone);
            //Update.Status = Status;
            db.Entry(Update).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Tower(string TowerID )
        {
            ViewBag.Tower = db.towers.ToList();
            //var dele = db.devices.Where(d => d.NumberID == Convert.ToInt32(TowerID)).FirstOrDefault();
            if (TowerID != null)
            {
                ViewBag.Tower = db.towers.ToList();

                int id = Convert.ToInt32(TowerID);
                ViewBag.selectedTower = id;
                var towername = db.towers.Where(t => t.NumberID == id).FirstOrDefault();
                ViewBag.deviceid = db.devices.Where(t => t.TowerID == towername.ID).ToList();
                ViewBag.towername = towername;
                ViewBag.devicetype = db.devicesTypes.ToList();
            }
            return View();
        }


        [HttpPost]
        public ActionResult TowerInsert(Tower TowerType)
        {
          
            tower.NumberID = db.towers.Select(s => s.NumberID).ToList().LastOrDefault() + 1;
            tower.Name = TowerType.Name;
            //tower.LattiTube = TowerType.LattiTube;
            //tower.LongiTube = TowerType.LongiTube;
            //tower.IP = TowerType.IP;
            //tower.Phone = TowerType.Phone;
            //tower.Status = TowerType.Status;

            db.towers.Add(tower);
            db.SaveChanges();
            return RedirectToAction("TowerOpen","Tower");
        }

        [HttpPost]
        public PartialViewResult TowerState(string TowerID)
        {
            if (TowerID == "Tower")
            {
                TowerID = "0";
            }
                int id = Convert.ToInt32(TowerID);
                var tower = db.towers.Where(t => t.NumberID == id).FirstOrDefault();
                List<AdminPanelDevice.Models.Devices> device = db.devices.Where(d => d.TowerID == tower.ID).ToList();
                ViewBag.devicetype = db.devicesTypes.ToList();
                return PartialView("DeviceTower", device);
            //}
            //else
            //{


            //    return PartialView("DeviceTower", 0);
            //}
        }

        [HttpPost]
        public JsonResult DevicePanelUpdate(int TowerID , string TowerName , string DeviceName , string DeviceIP ,int DevSerialNumber,string presetName)
        {
            AdminPanelDevice.Models.Devices device = new AdminPanelDevice.Models.Devices();
            var dvn = db.devicesTypes.Where(d => d.Name == DeviceName).FirstOrDefault();
            device = db.devices.Where(dv => dv.NumberID == TowerID).FirstOrDefault();
            device.Tower = db.towers.Where(t => t.ID == device.TowerID).FirstOrDefault();
            device.IP = DeviceIP;
            device.DeviceTID = dvn.NumberID;
            device.PresetName = presetName;
            device.DevSerialNumber =DevSerialNumber;
            device.DeviceTID = device.DeviceType.ID;
            try
            {
                db.Entry(device).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e){}
                return Json("",JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public PartialViewResult DevicePanelDelete(int DeleteID , int tower_state_id)
        {
            var device = db.devices.Where(dv => dv.NumberID == DeleteID).FirstOrDefault();
            try
            {
                db.devices.Remove(device);
                db.SaveChanges();
            }
 
            catch (Exception e) { }
            DeviceContext dbd = new DeviceContext();
            var tower = dbd.towers.Where(t => t.NumberID == tower_state_id).FirstOrDefault();
            List<AdminPanelDevice.Models.Devices> devices = dbd.devices.Where(d => d.TowerID == tower.ID).ToList();
            ViewBag.devicetype = dbd.devicesTypes.ToList();
            return PartialView("DeviceTower", devices);

        }

        [HttpPost]
        public PartialViewResult DeleteState(int DeleteID)
        {
            var towerstate = db.towers.Where(dv => dv.ID == DeleteID).FirstOrDefault();
            try
            {
                db.towers.Remove(towerstate);
                db.SaveChanges();
            }

            catch (Exception e) { }
            DeviceContext dbd = new DeviceContext();
            List<Tower> towerList = dbd.towers.ToList();
            //ViewBag.devicetype = dbd.devicesTypes.ToList();
            return PartialView("_TowerState", towerList);

        }

        [HttpPost]
        public JsonResult PresetDelete (string presetName)
        {
            if (presetName != "Preset")
            {
                var preset = db.Presets.Where(p => p.PresetName == presetName).FirstOrDefault();
                try
                {
                    db.Presets.Remove(preset);
                    db.SaveChanges();
                }
                catch (Exception e) { }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public PartialViewResult PresetDiagramSearch(string device_tower_IP)
        //{
        //    var device = db.devices.Where(d => d.IP == device_tower_IP).FirstOrDefault().DeviceTID;
        //    var preset = db.Presets.Where(p => p.DeviceTypeID == device).FirstOrDefault().ID;
        //    var walkdevice = db.WalkDevices.Where(w => w.PresetID == preset).ToList();
        //    List<WalkDevice> wlk = new List<WalkDevice>();

        //    wlk.AddRange(walkdevice);

        //    return PartialView("_walklist");
        //}

    }
}