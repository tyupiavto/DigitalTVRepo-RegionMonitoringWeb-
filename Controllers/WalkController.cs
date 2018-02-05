using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxNetwork;
using IToolS.IOServers.Snmp;
using System.Net;
using AdminPanelDevice.Models;
using PagedList;
using PagedList.Mvc;
using System.Collections;

namespace AdminPanelDevice.Controllers
{
    public class WalkController : Controller
    {
        AxNetwork.SnmpMibBrowser objSnmpMIB = new AxNetwork.SnmpMibBrowser();
        AxNetwork.NwConstants objConstants = new AxNetwork.NwConstants();
        SnmpObject objSnmp;
        public static List<WalkDevice> walkList = new List<WalkDevice>();
        public static List<WalkDevice> walkListEdit = new List<WalkDevice>();
        public static List<WalkDevice> walkSearch = new List<WalkDevice>();
        List<WalkDevice> presetSave = new List<WalkDevice>();
        DeviceContext db = new DeviceContext();
        public static List<int> Checked = new List<int>();
        public static ArrayList Time = new ArrayList();
        public List<int> CheckedPreset = new List<int>();
        public  ArrayList TimePreset = new ArrayList();
        public static int deviceTypeID;
        public int ID = 0;
        public static int viewSearch = 20;
        public static string IPadrress;
        public static int EditInd = 0;
        public static int PresetIND = 0;
        public static string PresetEditName;
        public static string Select;
        public static string All;
        public static string SearchNameWalk;
        public bool search = false;
        // GET: Walk

        public ActionResult Index(int? page,string SearchName)
        {
            ViewBag.Checke = Checked;
            ViewBag.ListView = viewSearch;
            ViewBag.Tim = Time;
            ViewBag.IP = IPadrress;
            ViewBag.Edit = EditInd;
            ViewBag.All = All;
            ViewBag.Select = Select;
            ViewBag.SearchName = SearchNameWalk;
            if (walkSearch != null && walkSearch.Count>=0 && SearchNameWalk!="" && SearchNameWalk !=null)
            {
             return View(walkSearch.ToPagedList(page ?? 1, viewSearch));
            }
            if (PresetIND == 0)
            {
                return View(walkList.ToPagedList(page ?? 1, viewSearch));
            }
            else
            {
                return View(walkListEdit.ToPagedList(page ?? 1, viewSearch));
            }
        }

        [HttpPost]
        public JsonResult clickchecked(List<int> ChekedList, Array[] TimeChange, List<int> UnChecked)
        {

            //if (UnChecked != null)
            //{
            //    foreach (var ch in UnChecked)
            //    {
            //        if (ChekedList != null)
            //        {
            //            ChekedList.Remove(ch);
            //        }
            //        else
            //        {
            //            Checked.Remove(ch);
            //        }
            //    }
            //}
            //if (ChekedList != null)
            //{
            //    Checked.AddRange(ChekedList);
            //}
            //if (TimeChange != null)
            //{
            //    Time.AddRange(TimeChange);
            //}

            //ViewBag.Tim = Time;
            //ViewBag.Checke = Checked;

            CheckedUnchecked(ChekedList, TimeChange, UnChecked); // add checked and change time

            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult ListView(int? page, List<int> ChekedList, Array[] TimeChange, List<int> UnChecked, int listV)
        {
            page = 1;

            if (UnChecked != null)
            {
                foreach (var ch in UnChecked)
                {
                    ChekedList.Remove(ch);
                }
            }

            if (ChekedList != null)
            {
                Checked.AddRange(ChekedList);
                ViewBag.Checke = Checked;
            }
            if (TimeChange!=null)
            {
                Time.AddRange(TimeChange);
                ViewBag.Tim = TimeChange;
            }
          

            if (listV != null)
            {
                viewSearch = listV;
            }
            return PartialView("_WalkView", walkList.ToPagedList(page ?? 1, viewSearch));
        }
        [HttpPost]
        public PartialViewResult WalkSend(int? page, string presetName,string TowerName, string IP, string DeviceName)
        {
            Checked.Clear();
            Time.Clear();
            walkSearch.Clear();
            SearchNameWalk = "";

            if (presetName == "Preset")
            {
                PresetIND = 0;
                walkList.Clear();
                IPadrress = IP;
                ViewBag.IP = IPadrress;

                var devicename = db.devicesTypes.Where(d => d.Name == DeviceName).FirstOrDefault();
                var walkOid = db.MibTreeInformations.Where(m => m.DeviceID == devicename.ID).FirstOrDefault();
                deviceTypeID = devicename.ID;
                OctetString community = new OctetString("public");

                AgentParameters param = new AgentParameters(community);
                param.Version = SnmpVersion.Ver2;

                IpAddress agent = new IpAddress(IP);

                UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);
                Oid rootOid = new Oid(walkOid.OID); // ifDescr
                                                    //Oid rootOid = new Oid(".1.3.6.1.4.1.23180.2.1.1.1"); // ifDescr

                Oid lastOid = (Oid)rootOid.Clone();

                Pdu pdu = new Pdu(PduType.GetNext);

                while (lastOid != null)
                {
                    try {
                        if (pdu.RequestId != 0)
                        {
                            pdu.RequestId += 1;
                        }
                        pdu.VbList.Clear();
                        pdu.VbList.Add(lastOid);
                        SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
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
                                        string oid = "." + v.Oid.ToString();
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
            }
            else
            {
                var presetname = db.Presets.Where(p => p.PresetName == presetName).FirstOrDefault();
                walkList = db.WalkDevices.Where(w => w.PresetID == presetname.ID).ToList();
            }

            //for (int i = 0; i < 120; i++)
            //{
            //    WalkInformation walk = new WalkInformation();
            //    ID++;
            //    walk.ID = ID;
            //    walk.OIDName = "1.2.2.3.50.20.21.2.2.3.50.20.21.2.2.3.50.20.21.2.2.3.50.20.2";
            //    walk.Value = "mrt_____mrt_____mrt_____mrt_____mrt_____mrt_____";
            //    walk.Time = 60;
            //    walkList.Add(walk);
            //}
            EditInd =0;
            ViewBag.Edit = EditInd;
            return PartialView("_WalkView", walkList.ToPagedList(page ?? 1, viewSearch));
        }

        [HttpPost]
        public PartialViewResult WalkEditPreset(int? page, string presetName)
        {
            walkSearch.Clear();
            SearchNameWalk = "";

            if (presetName != "Preset" && presetName != "Select" && presetName != "All")
            {
                Checked.Clear();
                Time.Clear();
                All = "All";
                Select = "";
                ViewBag.All = All;
                ViewBag.Select = Select;

                var preset = db.Presets.Where(p => p.PresetName == presetName).FirstOrDefault();
                PresetEditName = presetName;
                PresetIND =0;
                walkList.Clear();
                ViewBag.IP = IPadrress;
                

                var devicename = db.devicesTypes.Where(d => d.ID == preset.DeviceTypeID).FirstOrDefault();
                var walkOid = db.MibTreeInformations.Where(m => m.DeviceID == devicename.ID).FirstOrDefault();

                OctetString community = new OctetString("public");

                AgentParameters param = new AgentParameters(community);
                param.Version = SnmpVersion.Ver2;

                IpAddress agent = new IpAddress(preset.DeviceIP);

                UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);
                Oid rootOid = new Oid(walkOid.OID); // ifDescr

                Oid lastOid = (Oid)rootOid.Clone();

                Pdu pdu = new Pdu(PduType.GetNext);

                while (lastOid != null)
                {

                    if (pdu.RequestId != 0)
                    {
                        pdu.RequestId += 1;
                    }
                    pdu.VbList.Clear();
                    pdu.VbList.Add(lastOid);
                    SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                    if (result != null)
                    {
                        foreach (Vb v in result.Pdu.VbList)
                        {

                            if (rootOid.IsRootOf(v.Oid))
                            {
                                WalkDevice walk = new WalkDevice();
                                ID++;
                                string oid = "." + v.Oid.ToString();
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
                                walk.ID = ID;
                                walk.WalkID = ID;
                                if (OidMibdescription!=null)
                                walk.WalkOID =v.Oid.ToString();
                                //if (OidMibdescription!=null)
                                //walk.WalkDescription = OidMibdescription.Description;
                                walk.Type = v.Value.ToString();
                              
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
                target.Close();

                var CheckedItem = db.WalkDevices.Where(w => w.PresetID == preset.ID).ToList();
                foreach (var ch in CheckedItem)
                {
                    Checked.Add(ch.WalkID);
                    string[] s = { ch.WalkID.ToString(), ch.Time.ToString() };
                    Time.Add(s);

                    ViewBag.Tim = Time;
                    ViewBag.Checke = Checked;


                }
                ViewBag.Tim = Time;
                ViewBag.Checke = Checked;
            }
            else
            {
                var preset_edit = db.Presets.Where(p => p.PresetName == PresetEditName).FirstOrDefault();

                ViewBag.Tim = Time;
                ViewBag.Checke = Checked;

                if (presetName == "All")
                {
                    All = "All";
                    Select = "";
                    ViewBag.All =All ;
                    ViewBag.Select = Select;
                    PresetIND = 0;
                    return PartialView("_WalkView", walkList.ToPagedList(page ?? 1, viewSearch));
                }
                if (presetName=="Select")
                {
                    Select= "Select";
                    All = "";
                    ViewBag.Select = Select;
                    ViewBag.All =All;
                    PresetIND = 1;
                    walkListEdit.Clear();
                    foreach (var ch in Checked)
                    {
                        walkListEdit.Add(walkList[ch-1]);
                    }
                    
                    return PartialView("_WalkView", walkListEdit.ToPagedList(page ?? 1, viewSearch));
                }
            }

     
            EditInd = 1;
            ViewBag.Edit = EditInd;
            return PartialView("_WalkView", walkList.ToPagedList(page ?? 1, viewSearch));
        }



        [HttpPost]
        public JsonResult WalkSave(List<int> ChekedList, Array[] TimeChange, List<int> UnChecked, string PresetName, string IpAddress)
        {
            if (PresetName == null || PresetName == "")
            {
                var presetID = db.Presets.Where(p => p.PresetName == PresetEditName).FirstOrDefault().ID;
                var deletePresetEdit = db.WalkDevices.Where(w => w.PresetID == presetID).ToList();

                db.WalkDevices.RemoveRange(deletePresetEdit);
                db.SaveChanges();
                PresetName = PresetEditName;
            }
            else
            {
                if (PresetEditName == null)
                    PresetEditName = PresetName;

                Preset preset = new Preset();
                preset.DeviceID = db.Presets.Select(d => d.DeviceID).ToList().LastOrDefault() + 1;
                preset.PresetName = PresetName;
                preset.DeviceIP = IpAddress;
                preset.DeviceTypeID = deviceTypeID;
                db.Presets.Add(preset);
                db.SaveChanges();
            }

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
            CheckedPreset.AddRange(Checked);
            TimePreset.AddRange(Time);

            if (ChekedList!=null)
            {
                CheckedPreset.AddRange(ChekedList);
            }
            if (TimeChange!=null)
            {
                TimePreset.AddRange(TimeChange);
            }

            foreach(string[] tim in TimePreset)
            {
                int id = Convert.ToInt32(tim[0]);
                walkList[id-1].Time= Convert.ToInt32(tim[1]);
            }
            foreach (var item in CheckedPreset)
            {
                WalkDevice walkpreset = new WalkDevice();
                string oid ="." + walkList[item-1].WalkOID;
                var OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
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
                        walkpreset.OidID = OidMibdescription.ID;
                }
                else
                {
                    walkpreset.OidID = OidMibdescription.ID;
                }

                walkpreset.WalkID = item;
                walkpreset.PresetID = db.Presets.Where(p => p.PresetName==PresetName).FirstOrDefault().ID;
                walkpreset.WalkOID = walkList[item - 1].WalkOID;
                walkpreset.WalkDescription = walkList[item - 1].WalkDescription;
                walkpreset.Type = walkList[item - 1].Type;
                walkpreset.Time = walkList[item - 1].Time;
                presetSave.Add(walkpreset);
               
            }
            db.WalkDevices.AddRange(presetSave);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
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
                // Send request and wait for response
                response = target.Request(pdu, aparam) as SnmpV2Packet;
            }
            catch (Exception ex)
            {

                //MessageBox.Show(String.Format("Request failed with exception: {0}", ex.Message));
                //target.Close();
                //return;
            }

            //if (response == null)
            //{
            //    MessageBox.Show("Error in sending SNMP request.");
            //}
            //else
            //{
            //    // Check if we received an SNMP error from the agent
            //    if (response.Pdu.ErrorStatus != 0)
            //    {
            //        MessageBox.Show(String.Format("SNMP agent returned ErrorStatus {0} on index {1}",
            //            response.Pdu.ErrorStatus, response.Pdu.ErrorIndex) + "" + response.Pdu.TrapSysUpTime);
            //    }
            //    else
            //    {
            //        // Everything is ok. Agent will return the new value for the OID we changed
            //        MessageBox.Show(String.Format("Agent response {0}: {1}",
            //            response.Pdu[0].Oid.ToString(), response.Pdu[0].Value.ToString()));
            //    }
            //}
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult SearchList(int ? page ,string SearchName, List<int> ChekedList, Array[] TimeChange, List<int> UnChecked)
        {
            page = 1;
            search = true;
            SearchNameWalk = SearchName;

            CheckedUnchecked(ChekedList, TimeChange, UnChecked); // add checked and change time

                if (SearchName.Length >= 1)
                {
                    walkSearch.Clear();
                    walkSearch = walkList.Where(x => /* x.WalkOID.Contains(SearchName) ||*/ x.Type.Contains(SearchName)).ToList();

                    return PartialView("_WalkView", walkSearch.ToPagedList(page ?? 1, viewSearch));
                }
            
            else
            {
                walkSearch.Clear();
                return PartialView("_WalkView", walkList.ToPagedList(page ?? 1, viewSearch));
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