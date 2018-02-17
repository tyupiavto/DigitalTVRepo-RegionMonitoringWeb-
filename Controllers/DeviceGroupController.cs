using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Collections;
using System.IO;
using AdminPanelDevice.Infrastructure;

namespace AdminPanelDevice.Controllers
{
    public class DeviceGroupController : Controller
    {
        DeviceContext db = new DeviceContext();
        DeviceType devicetype = new DeviceType();
        public static List<Group> groupList = new List<Group>();
        string _path, _FileName;
        //public List<DeviceType> DeviceInsert = new List<DeviceType>();
        public static string DeviceName;
        public static List<Countrie> countrie = new List<Countrie>();
        public static List<States> states = new List<States>();
        public static List<City> city = new List<City>();
        public static int countrieIndicator=0;
        public  List<TitleTowerName> TitleTowor = new List<TitleTowerName>();
       
        // GET: DeviceGroup
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public JsonResult GroupCreate(string GroupName)
        {
            Group group = new Group();
            group.GroupName = GroupName;
            group.GroupNameID = db.Groups.Select(g => g.GroupNameID).ToList().LastOrDefault() + 1;
            db.Groups.Add(group);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GroupShow()
        {
            groupList = db.Groups.ToList();
            return PartialView("~/Views/DeviceGroup/_Group.cshtml", groupList);
        }
        [HttpPost]
        public PartialViewResult AddDevice(string deviceName)
        {
            DeviceName = deviceName;
            return PartialView("~/Views/DeviceGroup/_AddDevice.cshtml");
        }
       // var model = new FromData();
        //model.append("type",$('#x')[0].files[0]);
        [HttpPost]
        public JsonResult DeviceCreate(DeviceType type)
        {
            string pathname = "";
            
            try
            {
                if (type.mib_file.ContentLength > 0)
                {
                     _FileName = Path.GetFileName(type.mib_file.FileName);
                     _path = Path.Combine(Server.MapPath("~"), _FileName);
                    pathname = /*"MibFiles/" +*/ _FileName;
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

                //new BuildMIBTree(pathname, dvcID); // mib file save 
                new MibTreeCreate(_FileName, dvcID);


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
            }
            catch(Exception e)
            {
              
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult Countries(string CountrieName,string StateName, string CityName)
        {
            TitleTowerName TowerName = new TitleTowerName();

            if (CountrieName == null)
            {
                countrie = db.Countries.ToList();
                TowerName.CountrieName = "Counter";
            }

            if (CountrieName != null  && StateName=="")
            {
                city = new List<City>();
                states = new List<States>();

                var counterID = db.Countries.Where(c => c.CountrieName == CountrieName).FirstOrDefault().ID;
                states = db.States.Where(s=>s.CountrieID==counterID).ToList();
                ViewBag.countrieName = CountrieName;
                TowerName.CountrieName = CountrieName;
             
            }
            if (StateName!=null && StateName!="")
            {
                var StateID = db.States.Where(s => s.StateName == StateName).FirstOrDefault().ID;
                city = db.Citys.Where(c => c.StateID == StateID).ToList();
                TowerName.StateName = StateName;
                TowerName.CountrieName = CountrieName;
            }
            else
            {
                TowerName.StateName = "State";
            }
            ViewBag.countrie = countrie;
            ViewBag.state = states;
            ViewBag.city = city;

            ViewBag.countrieName = CountrieName;
            TitleTowor.Add(TowerName);
            return PartialView("_CountriesSearch",TitleTowor);
        }

        [HttpPost]
        public PartialViewResult countrieSearch(string countrieSearchName)
        {
            if (countrieSearchName == null) { 
            countrie = db.Countries.ToList();
            }
            else
            {
                countrie = db.Countries.Where(c => c.CountrieName.Contains(countrieSearchName)).ToList();
            }
            ViewBag.countrie = countrie;
            return PartialView("_Countrie");
        }

        [HttpPost]
        public PartialViewResult stateSearch(string CountrieName, string stateSearchName)
        {
            if (stateSearchName == null && CountrieName!=null)
            {
                var countrieID = db.Countries.Where(c => c.CountrieName == CountrieName).FirstOrDefault().ID;
                states = db.States.Where(s => s.CountrieID == countrieID).ToList();
                ViewBag.countrie = states;
            }
            else
            {
               var statesSearch = states.Where(s => s.StateName.Contains(stateSearchName)).ToList();
                ViewBag.countrie = statesSearch;
            }
            
            return PartialView("_State");
        }

        [HttpPost]
        public PartialViewResult citySearch(string StateName, string citySearchName)
        {
            if (citySearchName == null & StateName!=null)
            {
                var countrieID = db.States.Where(c => c.StateName == StateName).FirstOrDefault().ID;
                 city = db.Citys.Where(c=>c.StateID == countrieID).ToList();
                 ViewBag.city = city;
            }
            else
            {
               var citySearch = city.Where(c=>c.CityName.Contains(citySearchName)).ToList();
                ViewBag.city = citySearch;
            }
            
            return PartialView("_City");
        }
    }
}