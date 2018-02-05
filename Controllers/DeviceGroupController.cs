using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
namespace AdminPanelDevice.Controllers
{
    public class DeviceGroupController : Controller
    {
        DeviceContext db = new DeviceContext();
        // GET: DeviceGroup
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GroupCreate(string GroupName)
        {
            Group group = new Group();
            group.GroupName = GroupName;
            db.Groups.Add(group);
            db.SaveChanges();
            return Json("",JsonRequestBehavior.AllowGet);
        }


    }
}