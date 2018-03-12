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

        string searchName;
        public static string countrieName;
        public static int countrieID;
        public Tower tower = new Tower();
        public static int CountriesListID;
        public static string Html;
        public static int DeviceGroupID;
        public static int pageListNumber = 20;
        public static List<int> Checked = new List<int>();
        public static ArrayList Time = new ArrayList();
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
        public int QuerydeviceID;
        public static bool MibWalkIndicator =true;

        public SnmpPacket result;


        // GET: DeviceGroup
        public ActionResult Index(int ? page)
        {
            //page = 1;

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
        public PartialViewResult LoadMib (int? page, string DeviceName)
        {
            //page = 1;
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                string cmdString = "Select * From DeviceType where Name = '"+DeviceName+"'";
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
            ViewBag.ViewSearch = pageListNumber;
            return PartialView("_DeviceMibSetting",mibInformation.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public PartialViewResult WalkSend(int? page, string IP, int Port, string Version)
        {
            MibWalkIndicator = false;
            
            //Checked.Clear();
            //Time.Clear();
            //walkSearch.Clear();
            //SearchNameWalk = "";

            //if (presetName == "Preset")
            //{
            PresetIND = 0;
            walkList.Clear();
            IPadrress = IP;
            ViewBag.IP = IPadrress;

            // var devicename = db.devicesTypes.Where(d => d.Name == DeviceName).FirstOrDefault();
            //var walkOid = db.MibTreeInformations.Where(m => m.DeviceID == devicename.ID).FirstOrDefault();
            // deviceTypeID = devicename.ID;
            OctetString community = new OctetString("public");

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

                UdpTarget target = new UdpTarget((IPAddress)agent, Port, 2000, 1);
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
                                        WalkDevice walk = new WalkDevice();
                                        ID++;
                                        walk.ID = ID;
                                        walk.WalkID = ID;
                                        string oid =v.Oid.ToString();
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
            //}
            //else
            //{
            //    var presetname = db.Presets.Where(p => p.PresetName == presetName).FirstOrDefault();
            //    walkList = db.WalkDevices.Where(w => w.PresetID == presetname.ID).ToList();
            //}
            EditInd = 0;
            ViewBag.Edit = EditInd;
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                intervalTime = connection.Query<ScanningInterval>("Select * From  ScanningInterval").ToList();
            }
            ViewBag.IntervalTime = intervalTime;
            ViewBag.ViewSearch = pageListNumber;
            return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
        }

        [HttpPost]
        public JsonResult SetSend(string SetOID, int SetValue)
        {


            IpAddress agent = new IpAddress("192.168.4.42");

            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 4000, 1);
            Pdu.SetPdu();
            Pdu pdu = new Pdu(PduType.Set);
            pdu.VbList.Add(new Oid(SetOID), new Integer32(SetValue));

            AgentParameters aparam = new AgentParameters(SnmpVersion.Ver2, new OctetString("private"), true);
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
        public PartialViewResult WalkSearchList(int? page, string SearchName, List<int> ChekedList, Array[] TimeChange, List<int> UnChecked)
        {
            page = 1;
            search = true;
            SearchNameWalk = SearchName;

            CheckedUnchecked(ChekedList, TimeChange, UnChecked); // add checked and change time

            ViewBag.IntervalTime = intervalTime;
            ViewBag.ViewSearch = pageListNumber;
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

        [HttpPost]
        public PartialViewResult PageList(int? page, int pageList, List<int> ChekedList, Array[] TimeChange, List<int> UnChecked) // page list number search
        {
            ViewBag.IntervalTime = intervalTime;
            ViewBag.pageListNumber = pageListNumber;
            page = 1;
            pageListNumber = pageList;
            CheckedUnchecked(ChekedList, TimeChange, UnChecked);
            if (MibWalkIndicator = true)
            {
                return PartialView("_DeviceMibSetting", mibInformation.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
            }
        }

        public PartialViewResult PageNumber(int? page , List<int> ChekedList, Array[] TimeChange, List<int> UnChecked) // pagelistView number click
        {
            ViewBag.IntervalTime = intervalTime;
            ViewBag.ViewSearch = pageListNumber;
            CheckedUnchecked(ChekedList, TimeChange, UnChecked);
            if (MibWalkIndicator==true)
            {
                return PartialView("_DeviceMibSetting", mibInformation.ToPagedList(page ?? 1, pageListNumber));
            }
            else
            {
                return PartialView("_DeviceSettings", walkList.ToPagedList(page ?? 1, pageListNumber));
            }
        }

        public void CheckedUnchecked(List<int> ChekedList, Array[] TimeChange, List<int> UnChecked) // add checked and change time
        {
            if (UnChecked != null)
            {
                foreach (var ch in UnChecked)
                {
                    if (ChekedList != null)
                    {
                        ChekedList.Remove(ch);
                    }
                    else
                    {
                        Checked.Remove(ch);
                    }
                }
            }
            if (ChekedList != null)
            {
                Checked.AddRange(ChekedList);
            }
            if (TimeChange != null)
            {
                Time.AddRange(TimeChange);
            }

            ViewBag.Tim = Time;
            ViewBag.Checke = Checked;
        }

    }
}