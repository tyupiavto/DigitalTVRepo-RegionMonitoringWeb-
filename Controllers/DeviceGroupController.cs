using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Collections;
using System.IO;
using AdminPanelDevice.Infrastructure;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Xml;
using System.Web.Routing;
using PagedList;

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
        public List<PointConnection> pointConnection = new List<PointConnection>();
        public static List<States> states = new List<States>();
        public static List<City> city = new List<City>();
        public List<DeviceType> GroupListView = new List<DeviceType>();
        public static List<MibTreeInformation> mibInformation = new List<MibTreeInformation>();
        public ArrayList PointConnect = new ArrayList();
        public static int countrieIndicator=0;
        public  List<TitleTowerName> TitleTowor = new List<TitleTowerName>();
        public List<ScanningInterval> intervalTime = new List<ScanningInterval>();
        string searchName;
        public static string countrieName;
        public static int countrieID;
        public Tower tower = new Tower();
        public static int CountriesListID;
        public static string Html;
        public static int DeviceGroupID;
        public static int viewSearch = 20;
        // GET: DeviceGroup
        public ActionResult Index(int ? page)
        {
            //page = 1;

            return View(mibInformation.ToPagedList(page ?? 1, viewSearch));
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
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                groupList = connection.Query<Group>("Select * From [Group] ").ToList();
            }
            //groupList = db.Groups.ToList();
            return PartialView("~/Views/DeviceGroup/_Group.cshtml", groupList);
        }
        [HttpPost]
        public PartialViewResult AddDevice(string deviceName, int deviceGroupID)
        {
            DeviceGroupID = deviceGroupID;
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
                devicetype.DeviceGroupID = DeviceGroupID;
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
        public PartialViewResult Countries(string CountrieName,string StateName, string CityName, int parentId, string countrieSettingName, string stateSettingName)
        {
            CountriesListID = parentId;

            TitleTowerName TowerName = new TitleTowerName();

          
            ViewBag.countrie = countrie;
            ViewBag.countrieName = CountrieName;
            ViewBag.CountrieListID = CountriesListID;
           
            if (countrieName == null && stateSettingName == null)
            {
                TowerName.CountrieName = "Countrie";
                TowerName.StateName = "State";
                TowerName.CityName = "City";
            }
            else
            {
                TowerName.CountrieName = countrieSettingName;
                TowerName.StateName = stateSettingName;
                TowerName.CityName = "City";
            }
            TitleTowor.Add(TowerName);
            return PartialView("_CountriesSearch",TitleTowor);
        }

        [HttpPost]
        public PartialViewResult countrieSearch(string countrieSearchName)
        {
            if (countrieSearchName == null)
            {

                using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                {
                    countrie = connection.Query<Countrie>("Select * From  Countrie ").ToList();
                }
                //countrie = db.Countries.ToList();
            }
            else
            {
                if (countrieSearchName.Length >= 1)
                {
                    searchName = countrieSearchName.First().ToString().ToUpper() + countrieSearchName.Substring(1);
                    countrie = db.Countries.Where(c => c.CountrieName.Contains(countrieSearchName) || c.CountrieName.Contains(searchName)).ToList();

                    //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                    //{
                    //    countrie = connection.Query<Countrie>(@"Select * From  Countrie Where CONTAINS CountrieName=countrieSearchName").ToList();
                    //}

                }
                else
                {
                    //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                    //{
                    //    countrie = connection.Query<Countrie>("Select * From  Countrie Where CountrieName.Contains(countrieSearchName) ").ToList();
                    //}
                    countrie = db.Countries.Where(c => c.CountrieName.Contains(countrieSearchName)).ToList();
                }
            }
            states.Clear();
            city.Clear();

            ViewBag.countrie = countrie;
            return PartialView("_Countrie");
        }

        [HttpPost]
        public PartialViewResult stateSearch(string CountrieName, string stateSearchName)
        {
            countrieName = CountrieName;
            if (stateSearchName == null && CountrieName != null)
            {
                countrieID = db.Countries.Where(c => c.CountrieName == CountrieName).FirstOrDefault().ID;
                states = db.States.Where(s => s.CountrieID == countrieID).ToList();
                ViewBag.countrie = states;
            }
            if (stateSearchName != null && CountrieName != null)
            {
                if (stateSearchName.Length >= 1)
                {
                    searchName = stateSearchName.First().ToString().ToUpper() + stateSearchName.Substring(1);
                  var state = states.Where(s => s.StateName.Contains(stateSearchName) || s.StateName.Contains(searchName)).ToList();
                    ViewBag.countrie = state;
                }
                else {
                    states = db.States.Where(s => s.CountrieID == countrieID).ToList();
                    ViewBag.countrie = states;
                }
               
            }
            city.Clear();
            return PartialView("_State");
        }

        [HttpPost]
        public PartialViewResult citySearch(string CountrieName , string StateName, string citySearchName)
        {
            var countrieID = db.Countries.Where(c => c.CountrieName == CountrieName).FirstOrDefault().ID;
            var stateID = db.States.Where(s => s.StateName == StateName).FirstOrDefault().ID;
            var cityChecked = db.towers.Where(t => t.CountriesListID == CountriesListID && t.CountriesID==countrieID && t.StateID==stateID).ToList().Select(t=>t.CityCheckedID).ToList();

            if (citySearchName == null & StateName!=null && StateName!="")
            {
               var statesID = db.States.Where(c => c.StateName == StateName).FirstOrDefault().ID;
                 city = db.Citys.Where(c=>c.StateID == statesID).ToList();
                 ViewBag.city = city;
            }
            if (citySearchName != null & StateName != null && StateName != "")
            {
                if (citySearchName.Length >= 1)
                {
                    searchName = citySearchName.First().ToString().ToUpper() + citySearchName.Substring(1);
                    var citys = city.Where(c => c.CityName.Contains(citySearchName) || c.CityName.Contains(searchName)).ToList();
                    ViewBag.city = citys;
                }
                else
                {
                    city = db.Citys.Where(c => c.StateID == countrieID).ToList();
                    ViewBag.city = city;
                }
            }
            ViewBag.DiagramID = CountriesListID;
            return PartialView("_City",cityChecked);
        }

        [HttpPost]
        public PartialViewResult CityAdd (string StateName, string addcityName)
        {
            var countrieID = db.Countries.Where(c => c.CountrieName == countrieName).FirstOrDefault().ID;
            var stateID = db.States.Where(s => s.StateName == StateName).FirstOrDefault().ID;
            var cityChecked = db.towers.Where(t => t.CountriesListID == CountriesListID && t.CountriesID == countrieID && t.StateID == stateID).ToList().Select(t => t.CityCheckedID).ToList();

            City citys = new City();
            citys.CityName = addcityName;
            citys.StateID = stateID;
            db.Citys.Add(citys);
            db.SaveChanges();

            city = db.Citys.Where(c => c.StateID == stateID).ToList();
            ViewBag.city = city;

            return PartialView("_City", cityChecked);
        }

        [HttpPost]
        public JsonResult TowerInsert(string countrieName,string stateName, string cityName, int cityid)
        {
            var countrieID = db.Countries.Where(c => c.CountrieName == countrieName).FirstOrDefault().ID;
            var stateID = db.States.Where(s => s.StateName == stateName).FirstOrDefault().ID;

            tower.Name = cityName;
            tower.NumberID = db.towers.Select(s => s.NumberID).ToList().LastOrDefault() + 1;
            tower.CountriesID = countrieID;
            tower.StateID = stateID;
            tower.CityCheckedID = cityid;
            tower.CountriesListID = CountriesListID;

            db.towers.Add(tower);
            db.SaveChanges();
            return Json("");
        }

        [HttpPost]
        public JsonResult TowerDelete(int towerDeleteID, int cityid)
        {
            var deleteTower = db.towers.Where(t => t.CountriesListID == towerDeleteID).Where(c => c.CityCheckedID == cityid).FirstOrDefault();

            db.towers.Remove(deleteTower); 
            db.SaveChanges();
            return Json("");
        }

        [HttpPost]
        public PartialViewResult SelectAll(string selectallName, string StateName)
        {
            
            Tower tw = new Tower();
            var countrieID = db.Countries.Where(c => c.CountrieName == countrieName).FirstOrDefault().ID;
            var stateID = db.States.Where(s => s.StateName == StateName).FirstOrDefault().ID;
            var cityChecked = db.towers.Where(t => t.CountriesListID == CountriesListID && t.CountriesID == countrieID && t.StateID == stateID).ToList().Select(t => t.CityCheckedID).ToList();
            if (selectallName == "All")
            {
                city.Clear();
                city = db.Citys.Where(c => c.StateID == stateID).ToList();
                ViewBag.city = city;
            }
            else
            {
                city.Clear();
                foreach (var item in cityChecked)
                {
                    City ct = new City();
                    tw = db.towers.Where(t => t.CityCheckedID == item).FirstOrDefault();
                    ct.CityName = tw.Name;
                    ct.StateID = tw.StateID;
                    ct.CheckedID = tw.CityCheckedID;
                    city.Add(ct);
                }
                ViewBag.city = city;
            }
            return PartialView("_City", cityChecked);
        }
         
        [HttpPost]
        public JsonResult SaveDiagram (ReturnedHtml files)
        {
            Html = files.Html;
            string text = files.Html;
            string pointXml = files.Xml;
            System.IO.File.WriteAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\HtmlText\html.txt", text);
            //System.IO.File.WriteAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\HtmlText\PointXml.xml", pointXml);
            return Json("");
        }


        [HttpPost]
        public JsonResult PointConnections(Array[] connections)
        {
            try
            {
                List<PointConnection> point = new List<PointConnection>();
                //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                //{
                //    point = connection.Query<PointConnection>("Select * From  PointConnection ").ToList();
                //}
              
                if (connections != null)
                {
                    var points = db.PointConnections.ToList();
                    db.PointConnections.RemoveRange(points);
                    db.SaveChanges();

                    PointConnect.AddRange(connections);
                    PointConnection pointConnection = new PointConnection();
                    foreach (string[] item in PointConnect)
                    {
                        pointConnection.GetUuids = item[0];
                        pointConnection.SourceId = item[1];
                        pointConnection.TargetId = item[2];
                        pointConnection.PointRight = item[3];
                        pointConnection.PointLeft = item[4];

                        db.PointConnections.Add(pointConnection);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e) { } 

            return Json("");
        }

        [HttpPost]
        public JsonResult LoadDiagram ()
        {

            string html = "";
            string pointXml = "";
            Html = null;
            //HtmlSave html = new HtmlSave();
            //html.HtmlFile = db.HtmlSaves.Select(s => s.HtmlFile).FirstOrDefault();
            if (Html == null)
            {
             html = System.IO.File.ReadAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\HtmlText\html.txt");
             pointXml= System.IO.File.ReadAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\HtmlText\PointXml.xml");
                Html = "";
            }
            else
            {
                html = Html;
            }
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                pointConnection = connection.Query<PointConnection>("Select * From  PointConnection ").ToList();
            }
            //string html = db.HtmlSaves.Select(s => s.HtmlFile).FirstOrDefault();
            return Json(new { htmlData= html , pointData= pointConnection });
        }

        [HttpPost]
        public PartialViewResult DeviceList (int deviceGroupList)
        {
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
            //    GroupList = connection.Query<DeviceType>("Select * From DeviceType ").ToList();
            //}

            GroupListView = db.devicesTypes.Where(d => d.DeviceGroupID == deviceGroupList).ToList();

            return PartialView("_DeviceListView",GroupListView);
        }

        [HttpPost]
        public PartialViewResult DeviceManegeSetting (int dvcID , string DeviceName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.DeviceName = DeviceName;
            return PartialView("_DeviceSettings");
        }

        //[HttpPost]
        //public PartialViewResult ScanIntervalDvc()
        //{
        //    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
        //    {
        //        intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
        //    }
        //    return PartialView("_ScaninningInterval", intervalTime);
        //}
        [HttpPost]
        public PartialViewResult WalkMib (int? page)
        {
            //page = 1;
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                mibInformation = connection.Query<MibTreeInformation>("Select * From  [TreeInformation]").ToList();
            }
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.IntervalTime = intervalTime;
            return PartialView("_DeviceSettings",mibInformation.ToPagedList(page ?? 1, viewSearch));
        }

        public PartialViewResult PageNumber(int? page)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.IntervalTime = intervalTime;
            return PartialView("_DeviceSettings", mibInformation.ToPagedList(page ?? 1, viewSearch));
        }
    }
}