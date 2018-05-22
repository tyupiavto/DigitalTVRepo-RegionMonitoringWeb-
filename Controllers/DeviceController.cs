using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.IO;
using AdminPanelDevice.Infrastructure;

namespace AdminPanelDevice.Controllers
{
    public class DeviceController : Controller
    {
        DeviceContext db = new DeviceContext();
        DeviceContext dbD = new DeviceContext();
        DeviceType devicetype = new DeviceType();
        // GET: Device
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertDevice(DeviceType type)
        {
            string pathname = "";
            try
            {
                if (type.mib_file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(type.mib_file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/MibFiles/"), _FileName);
                    pathname = "MibFiles/" + _FileName;
                    type.mib_file.SaveAs(_path);
                }

                devicetype.Name = type.Name;
                devicetype.Model = type.Model;
                devicetype.Manufacture = type.Manufacture;
                devicetype.Purpose = type.Purpose;
                devicetype.MibParser = pathname;
                devicetype.NumberID = db.devicesTypes.Select(s => s.NumberID).ToList().LastOrDefault() + 1;
                db.devicesTypes.Add(devicetype);
                db.SaveChanges();
                int dvcID = db.devicesTypes.Select(s => s.ID).ToList().LastOrDefault();

                new BuildMIBTree(pathname, dvcID); // mib file save 

                var dvD = db.devicesTypes.ToList();
                string[] separators = { "/" };
                foreach (var item in dvD)
                {
                    if (item.MibParser != "")
                    {

                        string[] words = item.MibParser.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        item.MibParser = words[1];
                    }
                }
               
                return RedirectToAction("DeviceOpen", "Device");
            }
            catch
            {
                return RedirectToAction("DeviceOpen", "Device");
            }
        }
        public ActionResult DeviceOpen()
        {
            var  dvc = db.devicesTypes.ToList();
            string[] separators = { "/" };
            foreach (var item in dvc )
            {
                if (item.MibParser!="")
                {
                   
                    string[] words= item.MibParser.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    item.MibParser = words[1];
                }
            }
            return View(dvc);
        }

        [HttpPost]
        public PartialViewResult DeleteDevice(int DeleteID)
        {
            var DeviceDel = db.devicesTypes.Where(d => d.ID == DeleteID).FirstOrDefault();
            try
            {
                db.devicesTypes.Remove(DeviceDel);
                db.SaveChanges();
            }

            catch (Exception e) { }
            DeviceContext dbd = new DeviceContext();
            List<DeviceType> devicetype = dbd.devicesTypes.ToList();
            return PartialView("DevicePanel", devicetype);

        }

        public ActionResult DeviceEdit (int ? ID)
        {
            var dvc = db.devicesTypes.Where(d => d.ID == ID).FirstOrDefault();
            string[] separators = { "/" };
            string[] words = dvc.MibParser.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //if (words!=null)
            // dvc.MibParser = words[1];
            return View(dvc);
        }

        [HttpPost]
        public ActionResult DeviceUpdate(DeviceType type)
        {
            DeviceType deviceUpdate = new DeviceType();
            string pathname = "";
            try
            {
                if (type.mib_file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(type.mib_file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/MibFiles/"), _FileName);
                    pathname = "/MibFiles/" + _FileName;
                    type.mib_file.SaveAs(_path);
                }
            }
            catch
            {
                //return Json("", JsonRequestBehavior.AllowGet);
            }
                deviceUpdate.ID = type.ID;
                deviceUpdate.Name = type.Name;
                deviceUpdate.Model = type.Model;
                deviceUpdate.Manufacture = type.Manufacture;
                deviceUpdate.Purpose = type.Purpose;
                deviceUpdate.NumberID = deviceUpdate.NumberID;
            if (pathname != "")
                deviceUpdate.MibParser = pathname;
            else
                deviceUpdate.MibParser = type.MibParser;

                db.Entry(deviceUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            return Redirect("DeviceOpen/Device");
        }

        [HttpPost]
        public JsonResult DevicePanelInsert (string Tower , string Device , string IP , string DevSerialNumber, string presetName)
        {
            try
            {
                //Devices dv = new Devices();
                //var tw = db.towers.Where(t => t.Name == Tower).FirstOrDefault();
                //var dvc = db.devicesTypes.Where(d => d.Name == Device).FirstOrDefault();
                //dv.DeviceTID = dvc.ID;
                //dv.TowerID = tw.ID;
                //dv.IP = IP;
                //dv.DevSerialNumber = Convert.ToInt32(DevSerialNumber);
                //dv.PresetName = presetName;
                //dv.NumberID = db.devices.Select(s => s.NumberID).ToList().LastOrDefault() + 1;

                //db.devices.Add(dv);
                //db.SaveChanges();
            }
            catch (Exception e) { }
            return Json("",JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SecondInterval(int Second)
        {
            ScanningInterval sc = new ScanningInterval();
            sc.Interval = Second;
            sc.IntervalID = db.ScanningIntervals.Select(s => s.IntervalID).ToList().LastOrDefault() + 1;
            db.ScanningIntervals.Add(sc);
            db.SaveChanges();
            ViewBag.Second = db.ScanningIntervals.ToList();
            return Json("",JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteInterval(string DeleteID)
        {
            int deleteId = Convert.ToInt32(DeleteID);
            var deleteInt = db.ScanningIntervals.Where(s => s.Interval == deleteId).FirstOrDefault();
            try
            {
                db.ScanningIntervals.Remove(deleteInt);
                db.SaveChanges();
            }

            catch (Exception e) { }
            return Json("", JsonRequestBehavior.AllowGet);

        }
    }
}