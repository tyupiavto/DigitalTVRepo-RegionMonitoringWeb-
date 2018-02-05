using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Net;
using System.IO;
using AdminPanelDevice.Infrastructure;
using AxNetwork;
namespace AdminPanelDevice.Controllers
{
    public class HomeController : Controller
    {
        DeviceContext db = new DeviceContext();
        Tower tower = new Tower();
        DeviceType devicetype = new DeviceType();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.DeviceType = db.devicesTypes.ToList();
            ViewBag.Tower = db.towers.ToList();
            ViewBag.Second = db.ScanningIntervals.ToList();
            return View();
        }

        //[HttpPost]
        //public ActionResult InsertDevice(DeviceType type)
        //{
            
        //    //new BuildMIBTree();
        //    string pathname="";
        //    try
        //    {
        //        if (type.mib_file.ContentLength > 0)
        //        {
        //            string _FileName = Path.GetFileName(type.mib_file.FileName);
        //            string _path = Path.Combine(Server.MapPath("~/MibFiles/"), _FileName);
        //            pathname = "MibFiles/" + _FileName;
        //            type.mib_file.SaveAs(_path);
        //        }

        //        new BuildMIBTree(pathname);

        //        devicetype.Name = type.Name;
        //        devicetype.Model = type.Model;
        //        devicetype.Manufacture = type.Manufacture;
        //        devicetype.Purpose = type.Purpose;
        //        devicetype.MibParser = pathname;

        //        db.devicesTypes.Add(devicetype);
        //        db.SaveChanges();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase mib_file)
        //{
        //    try
        //    {
        //        if (mib_file.ContentLength > 0)
        //        {
        //            string _FileName = Path.GetFileName(mib_file.FileName);
        //            string _path = Path.Combine(Server.MapPath("~/MibFiles/"), _FileName);
        //            ViewBag.Path = "MibFiles/"+ _FileName;
        //            mib_file.SaveAs(_path);
        //        }

        //        return RedirectToAction("Index","Home");
        //    }
        //    catch
        //    {
        //          return RedirectToAction("Index","Home");
        //    }
        //}
    }
}