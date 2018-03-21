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
using SnmpSharpNet;
using System.Net;
using System.Drawing;

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
        public static List<ScanningInterval> intervalTime = new List<ScanningInterval>();
        public static List<WalkDevice> walkList = new List<WalkDevice>();
        public static List<WalkDevice> walkSearch = new List<WalkDevice>();
        public  List<WalkDevice> PresetList = new List<WalkDevice>();
        public List<ScanningInterval> intervalList = new List<ScanningInterval>();
        public static List<int> GPSCoordinate = new List<int>();
        string searchName;
        public static string countrieName;
        public static int countrieID;
        public Tower tower = new Tower();
        public static int CountriesListID;
        public static string Html;
        public static int DeviceGroupID;
        public static int pageListNumber = 20;
        public static int DefaultInterval = 60;
        public static List<int> CheckedLog = new List<int>();
        public static List<int> CheckedMap = new List<int>();
        public static ArrayList Interval = new ArrayList();
        public List<int> CheckedPreset = new List<int>();
        public ArrayList TimePreset = new ArrayList();
        public static int deviceTypeID;
        public int ID = 0;
        public static string IPadrress;
        public static int EditInd = 0;
        public static int PresetIND = 0;
        public static string PresetEditName;
        public static string Select;
        public static string All;
        public static string SearchNameWalk;
        public bool search = false;
        public static int QuerydeviceID;
        public static bool MibWalkIndicator =true;

        public SnmpPacket result;

        public struct intervalValue
        {
          public  int intervalID { get; set; }
          public  int intervalVal { get; set; }
        }

        // GET: DeviceGroup
        public ActionResult Index(int ? page)
        {
            //page = 1;
            ViewBag.Preset = "";
            return View(mibInformation.ToPagedList(page ?? 1, pageListNumber));
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
            }
            else
            {
                if (countrieSearchName.Length >= 1)
                {
                    searchName = countrieSearchName.First().ToString().ToUpper() + countrieSearchName.Substring(1);
                    string queryCuntries = "Select * From Countrie Where CountrieName Like N'" + countrieSearchName + "%'";

                    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                    {
                        countrie = connection.Query<Countrie>(queryCuntries).ToList();
                    }

                }
                else
                {
                    string queryCuntries = "Select * From Countrie Where CountrieName Like N'" + countrieSearchName + "%'";
                    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                    {
                        countrie = connection.Query<Countrie>(queryCuntries).ToList();
                    }
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
                using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                {
                    string queryCountrie = "Select * from Countrie where CountrieName = '" + CountrieName + "'";
                    countrieID = connection.Query<Countrie>(queryCountrie).FirstOrDefault().ID;
                    string queryState = "select * from States where CountrieID='" + countrieID + "'";
                    states = connection.Query<States>(queryState).ToList();
                }

                ViewBag.countrie = states;
            }
            if (stateSearchName != null && CountrieName != null)
            {
                if (stateSearchName.Length >= 1)
                {
                    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                    {
                        string queryState = "select * from States where CountrieID='" + countrieID + "' Select * From States where  StateName Like N'" + stateSearchName + "%'";
                        states = connection.Query<States>(queryState).ToList();
                    }
                        searchName = stateSearchName.First().ToString().ToUpper() + stateSearchName.Substring(1);
                        var state = states.Where(s => s.StateName.Contains(stateSearchName) || s.StateName.Contains(searchName)).ToList();
                        ViewBag.countrie = state;
                }
                else {
                    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                    {
                        string querySearch = "select * from States where CountrieID='" + countrieID +"'";
                        states = connection.Query<States>(querySearch).ToList();
                    }
                    ViewBag.countrie = states;
                }
               
            }
            city.Clear();
            return PartialView("_State");
        }

        [HttpPost]
        public PartialViewResult citySearch(string CountrieName, string StateName, string citySearchName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var countrieID = connection.Query<Countrie>("select * from Countrie where CountrieName='" + CountrieName + "'").FirstOrDefault().ID;
                var stateID = connection.Query<States>("select * from States where StateName='" + StateName + "'").FirstOrDefault().ID;
                var cityChecked = connection.Query<Tower>("select * from Tower where CountriesListID='" + CountriesListID + "' and CountriesID='" + countrieID + "' and StateID='" + stateID + "'").ToList().Select(t => t.CityCheckedID).ToList();

                if (citySearchName == null & StateName != null && StateName != "")
                {
                   stateID = connection.Query<States>("select * from States where StateName='" + StateName + "'").FirstOrDefault().ID;
                   city=connection.Query<City>("Select * from City where  StateID='" + stateID + "'").ToList();

                    ViewBag.city = city;
                }
                if (citySearchName != null & StateName != null && StateName != "")
                {
                    if (citySearchName.Length >= 1)
                    {
                        var citys = connection.Query<City>("Select * from City where CityName like N'" + citySearchName + "%'");
                        ViewBag.city = citys;
                    }
                    else
                    {
                        city = connection.Query<City>("Select * From City where StateID='" + countrieID + "'").ToList();
                        ViewBag.city = city;
                    }
                }
                ViewBag.DiagramID = CountriesListID;
                return PartialView("_City", cityChecked);
            }
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
             //pointXml= System.IO.File.ReadAllText(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\HtmlText\PointXml.xml");
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
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                GroupListView = connection.Query<DeviceType>("Select * From DeviceType").ToList();
                ////GroupListView = db.devicesTypes.Where(d => d.DeviceGroupID == deviceGroupList).ToList();
                return PartialView("_DeviceListView", GroupListView);
            }
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


        [HttpPost]
        public PartialViewResult LoadMib (int? page, string DeviceName)
        {
            //page = 1;
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                string cmdString = "Select * From DeviceType where Name='"+DeviceName+"'";
                QuerydeviceID = connection.Query<DeviceType>(cmdString).FirstOrDefault().ID;
            }

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                string cmdString = "Select * From  [TreeInformation] where DeviceID=" + QuerydeviceID;

                mibInformation = connection.Query<MibTreeInformation>(cmdString).ToList();
            }

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            return PartialView("_DeviceMibSetting",mibInformation.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public PartialViewResult WalkSend(int? page, string IP, int Port, string Version , string communityRead)
        {
            MibWalkIndicator = false;
            
            //if (presetName == "Preset")
            //{
            PresetIND = 0;
            walkList.Clear();
            IPadrress = IP;
            ViewBag.IP = IPadrress;
           
            // var devicename = db.devicesTypes.Where(d => d.Name == DeviceName).FirstOrDefault();
            //var walkOid = db.MibTreeInformations.Where(m => m.DeviceID == devicename.ID).FirstOrDefault();
            // deviceTypeID = devicename.ID;
            OctetString community = new OctetString(communityRead);

            AgentParameters param = new AgentParameters(community);
            if (Version == "V1")
            {
                param.Version = SnmpVersion.Ver1;
            }
            if (Version == "V2")
            {
                param.Version = SnmpVersion.Ver2;
            }
            IpAddress agent = new IpAddress(IP);

                UdpTarget target = new UdpTarget((IPAddress)agent, Port, 6000, 1);
                Oid rootOid = new Oid(".1.3.6.1.4.1"); // ifDescr
                                                       //Oid rootOid = new Oid(".1.3.6.1.4.1.23180.2.1.1.1"); // ifDescr

                Oid lastOid = (Oid)rootOid.Clone();

                Pdu pdu = new Pdu(PduType.GetNext);

                while (lastOid != null)
                {
                    try
                    {
                        if (pdu.RequestId != 0)
                        {
                            pdu.RequestId += 1;
                        }
                        pdu.VbList.Clear();
                        pdu.VbList.Add(lastOid);
                 
                    if (Version == "V1")
                    {
                         result = (SnmpV1Packet)target.Request(pdu, param);
                    }
                    if (Version == "V2")
                    {
                         result =(SnmpV2Packet)target.Request(pdu, param);
                    }

                    if (result != null)
                        {
                            if (result.Pdu.ErrorStatus != 0)
                            {
                                lastOid = null;
                                break;
                            }
                            else
                            {
                                foreach (Vb v in result.Pdu.VbList)
                                {

                                if (rootOid.IsRootOf(v.Oid))
                                {
                                    using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
                                    {
                                        WalkDevice walk = new WalkDevice();
                                        ID++;
                                        walk.ID = ID;
                                        walk.WalkID = ID;
                                        string oid = v.Oid.ToString();
                                        var OidMibdescription = db.MibTreeInformations.Where(m => m.OID == oid).FirstOrDefault();
                                        if (OidMibdescription == null)
                                        {
                                            oid = oid.Remove(oid.Length - 1);
                                            oid = oid.Remove(oid.Length - 1);
                                            OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
                                        }
                                        if (OidMibdescription == null)
                                        {
                                            oid = oid.Remove(oid.Length - 1);
                                            oid = oid.Remove(oid.Length - 1);
                                            OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
                         
                                            if (OidMibdescription != null)
                                                walk.WalkDescription = OidMibdescription.Description;
                                        }
                                        else
                                        {
                                            walk.WalkDescription = OidMibdescription.Description;
                                        }
                                        if (OidMibdescription != null)
                                            walk.WalkDescription = OidMibdescription.Description;

                                        walk.WalkOID = v.Oid.ToString();
                                        walk.Type = v.Value.ToString();
                                        walk.value = SnmpConstants.GetTypeName(v.Value.Type);
                                        walk.Time = 60;
                                        walkList.Add(walk);
                                        lastOid = v.Oid;
                                    }
                                }
                                else
                                {
                                    lastOid = null;
                                }
                                }
                            }
                        }
                    }
                    catch (Exception e) { }
                }

                target.Close();
            
            EditInd = 0;
            ViewBag.Edit = EditInd;
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            ViewBag.defaultInterval = DefaultInterval;
            return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public JsonResult SetSend(string IP,string SetOID, int SetValue,string communityWrite)
        {


            IpAddress agent = new IpAddress(IP);

            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 4000, 1);
            Pdu.SetPdu();
            Pdu pdu = new Pdu(PduType.Set);
            pdu.VbList.Add(new Oid(SetOID), new Integer32(SetValue));

            AgentParameters aparam = new AgentParameters(SnmpVersion.Ver2, new OctetString(communityWrite), true);
            SnmpV2Packet response;
            try
            {
                response = target.Request(pdu, aparam) as SnmpV2Packet;
            }
            catch (Exception ex)
            {
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public PartialViewResult WalkSearchList(int? page, string SearchName)
        {
            page = 1;
            search = true;
            SearchNameWalk = SearchName;

            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            ViewBag.CheckedMap = CheckedMap;
            ViewBag.CheckedLog = CheckedLog;
            ViewBag.Interval = Interval;
            ViewBag.defaultInterval = DefaultInterval;

            if (MibWalkIndicator == true)
            {
                if (SearchName.Length >= 1)
                {
                    walkSearch.Clear();
                    walkSearch = walkList.Where(x => x.WalkDescription.Contains(SearchName) || x.Type.Contains(SearchName)).ToList();

                    return PartialView("_DeviceMibSetting", walkSearch.ToPagedList(page ?? 1, pageListNumber));
                }

                else
                {
                    walkSearch.Clear();
                    return PartialView("_DeviceMibSetting", walkList.ToPagedList(page ?? 1, pageListNumber));
                }
            }
            else
            {
                if (SearchName.Length >= 1)
                {
                    walkSearch.Clear();
                    walkSearch = walkList.Where(x => x.WalkDescription.Contains(SearchName) || x.Type.Contains(SearchName)).ToList();

                    return PartialView("_DeviceSettings", walkSearch.ToPagedList(page ?? 1, pageListNumber));
                }

                else
                {
                    walkSearch.Clear();
                    return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
                }
            }

        }

        [HttpPost]
        public PartialViewResult PageList(int? page, int pageList) // page list number search
        {
            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            ViewBag.CheckedLog = CheckedLog;
            ViewBag.CheckedMap = CheckedMap;
            ViewBag.Interval = Interval;
            ViewBag.defaultInterval = DefaultInterval;
            page = 1;
            pageListNumber = pageList;

            if (MibWalkIndicator == true)
            {
                return PartialView("_DeviceMibSetting", mibInformation.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
            }
        }

        public PartialViewResult PageNumber(int? page) // pagelistView number click
        {
            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            ViewBag.CheckedLog = CheckedLog;
            ViewBag.CheckedMap = CheckedMap;
            ViewBag.Interval = Interval;
            ViewBag.defaultInterval = DefaultInterval;

            if (MibWalkIndicator==true)
            {
                return PartialView("_DeviceMibSetting", mibInformation.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
            }
        }
        [HttpPost]
        public JsonResult UncheckLog(int unChechkID) // uncheckd log
        {
            CheckedLog.Remove(unChechkID);
            ViewBag.CheckedLog = CheckedLog;
            return Json("");
        }

        [HttpPost]
        public JsonResult CheckLog(int chechkID) // checked log
        {
            CheckedLog.Add(chechkID);
            ViewBag.CheckedLog = CheckedLog;
            return Json("");
        }

        [HttpPost] 
        public JsonResult UncheckMap(int unChechkID) // unchecked map
        {
            CheckedMap.Remove(unChechkID);
            ViewBag.CheckedMap = CheckedMap;
            return Json("");
        }

        [HttpPost]
        public JsonResult CheckMap(int chechkID) // checked map
        {
            CheckedMap.Add(chechkID);
            ViewBag.CheckedMap = CheckedMap;
            return Json("");
        }
        
        [HttpPost]
        public PartialViewResult ViewAddInterval()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.IntervalTime = intervalTime;
            return PartialView("_ScaninningInterval", intervalTime);
        }

        [HttpPost]
        public JsonResult IntervalSearch(int intervalID , int Interval)
        {
            intervalValue intr=new intervalValue();
            intr.intervalID = intervalID;
            intr.intervalVal = Interval;

            DeviceGroupController.Interval.Add(intr);
            ViewBag.Interval = DeviceGroupController.Interval;
            return Json("");
        }

        [HttpPost]
        public JsonResult PresetSave (string presetName, string IpAddress, string towerNameID)
        {
            Preset preset = new Preset();
            preset.DeviceID = db.Presets.Select(d => d.DeviceID).ToList().LastOrDefault() + 1;
            preset.PresetName = presetName;
            preset.DeviceIP = IpAddress;
            preset.DeviceTypeID = QuerydeviceID;
            db.Presets.Add(preset);
          

            TowerGps gps = new TowerGps();
            gps.Lattitube = walkList[GPSCoordinate[0]-1].Type;
            gps.Longitude = walkList[GPSCoordinate[1]-1].Type;
            gps.Altitude = walkList[GPSCoordinate[2]-1].Type;
            gps.PresetName = presetName;
            gps.TowerNameID = towerNameID;
            gps.DeviceID = QuerydeviceID;

            db.towerGps.Add(gps);

            db.SaveChanges();

            foreach (intervalValue tim in Interval)
            {
                int id =tim.intervalID;
                walkList[id - 1].Time = Convert.ToInt32(tim.intervalVal);
            }

            PresetFill pre = new PresetFill();
            PresetList = pre.presetFillLoad(walkList, PresetList, CheckedLog, CheckedMap,presetName);

            db.WalkDevices.AddRange(PresetList);
            db.SaveChanges();
            return Json("");
        }

        [HttpPost]
        public PartialViewResult PresetListName()
        {
            List<Preset> presetList = new List<Preset>();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                presetList = connection.Query<Preset>("Select * From  Preset ").ToList();
            }
            return PartialView("_Preset",presetList);
        }

        [HttpPost]
        public PartialViewResult PresetRemove(string presetName)
        {
            Preset presetremove = new Preset();
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
            //    string cmdString = "Select * From Preset where PresetName='" + presetName+"'"; 
            //    presetremove = connection.Query<Preset>(cmdString).FirstOrDefault();
            //}
            presetremove = db.Presets.Where(p => p.PresetName == presetName).FirstOrDefault();
            db.Presets.Remove(presetremove);
            db.SaveChanges();
            List<Preset> presetList = new List<Preset>();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                presetList = connection.Query<Preset>("Select * From  Preset ").ToList();
            }
            return PartialView("_Preset", presetList);
        }

        [HttpPost]
        public PartialViewResult intervalListView()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalList = connection.Query<ScanningInterval>("Select * From  ScanningInterval ").ToList();
            }
            return PartialView("_Interval", intervalList);
        }

        [HttpPost]
        public PartialViewResult IntervalRemove(int intervalID)
        {
            ScanningInterval intervalremove = new ScanningInterval();
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
            //    string cmdString = "Select * From Preset where PresetName='" + presetName+"'"; 
            //    presetremove = connection.Query<Preset>(cmdString).FirstOrDefault();
            //}
            intervalremove = db.ScanningIntervals.Where(p => p.IntervalID == intervalID).FirstOrDefault();
            db.ScanningIntervals.Remove(intervalremove);
            db.SaveChanges();

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalList = connection.Query<ScanningInterval>("Select * From  ScanningInterval ").ToList();
            }
            return PartialView("_Interval", intervalList);
        }

        [HttpPost]
        public PartialViewResult IntervalAdd (int second)
        {
            ScanningInterval sc = new ScanningInterval();
            sc.Interval = second;
            sc.IntervalID = db.ScanningIntervals.Select(s => s.IntervalID).ToList().LastOrDefault() + 1;
            db.ScanningIntervals.Add(sc);
            db.SaveChanges();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalList = connection.Query<ScanningInterval>("Select * From  ScanningInterval ").ToList();
            }
            return PartialView("_Interval", intervalList);
        }

        [HttpPost]
        public PartialViewResult PresetSearch(int ? page ,string presetSearchName)
        {
            page = 1;
            int presetID;

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                string cmdString = "Select * From Preset where PresetName = '" + presetSearchName + "'";
                presetID = connection.Query<Preset>(cmdString).FirstOrDefault().ID;
                string log = "Select * From WalkDevice where PresetID = '" + presetID+"'";
                CheckedLog = connection.Query<WalkDevice>(log).ToList().Select(s=>s.LogID).ToList();

                string map = "Select * From WalkDevice where PresetID = '" + presetID + "'";
                CheckedMap = connection.Query<WalkDevice>(map).ToList().Select(s => s.MapID).ToList();

                string interval = "Select * From WalkDevice where PresetID = '" + presetID + "'";
                var query = from pro in db.WalkDevices
                            select new TimList() { intervalID = pro.CheckID, intervalVal = pro.Time };
                var tm = query.ToList();
                Interval.AddRange(tm);
            }

            ViewBag.IntervalTime = intervalTime;
            ViewBag.CheckedLog = CheckedLog;
            ViewBag.CheckedMap = CheckedMap;
            ViewBag.Interval = Interval;
            ViewBag.defaultInterval = DefaultInterval;
            return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public PartialViewResult DefaultIntervalSearch (int ? page , int intervalNumber)
        {
            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            ViewBag.CheckedLog = CheckedLog;
            ViewBag.CheckedMap = CheckedMap;
            ViewBag.Interval = Interval;
            DefaultInterval = intervalNumber;
            ViewBag.defaultInterval = DefaultInterval;

           return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public PartialViewResult TowerGpsSetting(List<string> devicetype)
        {
            List<DeviceType> gpsDevice = new List<DeviceType>();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                foreach (var item in devicetype)
                {
                    gpsDevice.Add( connection.Query<DeviceType>("Select * from DeviceType where Name like N'"+item+"%'").FirstOrDefault());
                }
            }
            return PartialView("_Gps", gpsDevice);
        }

        [HttpPost]
        public JsonResult GpsSelect(int GpsID)
        {
            GPSCoordinate.Add(GpsID);
            return Json("");
        }

        [HttpPost]
        public JsonResult GpsUnSelect(int GpsID)
        {
            GPSCoordinate.Remove(GpsID);
            return Json("");
        }

        [HttpPost]
        public JsonResult CheckGps(string deviceGpsName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var deviceID = connection.Query<DeviceType>("Select * from DeviceType where Name like N'" + deviceGpsName + "%'").FirstOrDefault().ID;
                var gpsCordinate = connection.Query<TowerGps>("Select * from TowerGps where DeviceID='" + deviceID + "'").ToList();
                return Json(gpsCordinate);
            }
        }

    }
}