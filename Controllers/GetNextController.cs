using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Threading;
namespace AdminPanelDevice.Controllers
{
    public class GetNextController : Controller
    {
        public List<string> getoid = new List<string>();
        public static List<WalkDevice> walkList = new List<WalkDevice>();

        public static List<Pdu> pdu = new List<Pdu>();
 
        public static List<presetThread> presetInf = new List<presetThread>();
        public static List<Thread> the = new List<Thread>();
        public  List<MibGet> mib = new List<MibGet>();
        public List<MibGet> mibs = new List<MibGet>();
        public  int count = -1;
        public  int devID = 0;
        static bool  Thr = true;
        public static List<WalkDevice> walkdvc = new List<WalkDevice>();
        public static DeviceContext db = new DeviceContext();
        public LiveValue live = new LiveValue();
        // GET: GetNext
        public ActionResult Index()
        {
            return View();
        }

        public  void ThreadPreset(string ip ,int time ,int Deviceid, List<Pdu> pd,int j)
        {
            while (Thr==true) {
                //OctetString community = new OctetString("public");
                AgentParameters param = new AgentParameters(new OctetString("public"));
                param.Version = SnmpVersion.Ver1;

                SnmpV1Packet result = (SnmpV1Packet)(new UdpTarget((IPAddress)(new IpAddress(ip)), 161, 2000, 1).Request(pd[j], param));

                int OidID;
                foreach (Vb v in result.Pdu.VbList)
                {
                    devID++;
                    MibGet get = new MibGet();
                    get.Value= v.Value.ToString();
                    get.DeviceID = Deviceid;
                    get.dateTime = DateTime.Now;
                    get.WalkOID = v.Oid.ToString();
                    var oid =v.Oid.ToString();

                    //live.liveValue = v.Value.ToString();
                    //live.Time = (time * 1000);
                    //db.LiveValues.Add(live);
                    //db.SaveChanges();
            
                    OidID = walkdvc.Where(w => w.WalkOID == v.Oid.ToString()).FirstOrDefault().OidID;

                        get.MibID = OidID;
                  
                    if (devID >= 1000)
                    {
                        devID = 0;
                        mibs.AddRange(mib);
                        mib.Clear();
                        db.MibGets.AddRange(mibs);
                        db.SaveChanges();
                        mibs.Clear();
                    }
                    else
                    {
                        if (get != null)
                        {
                            db.MibGets.Add(get);
                            db.SaveChanges();
                            //mib.Add(get);
                        }
                    }
                }
              
                Thread.Sleep((time*1000));
            }
        }

        [HttpPost]
        public JsonResult Get()
        {
            DeviceContext db = new DeviceContext();
            walkdvc = db.WalkDevices.ToList();
            Oid rootOid = new Oid(); // ifDescr
            Oid lastOid = new Oid();

            var preset = db.Presets.ToList();
            var interval = db.ScanningIntervals.ToList();
            foreach (var pre in preset)
            {
                var device = db.devices.Where(d => d.PresetName == pre.PresetName).ToList();
                foreach (var dev in device)
                {
                    var walkdvc = db.WalkDevices.Where(w => w.PresetID == pre.ID).ToList();

                    foreach (var ScanningInterval in interval)
                    {
                        var pduinformation = walkdvc.Where(p => p.Time == ScanningInterval.Interval).ToList();
                        if (pduinformation.Count != 0)
                        {
                            count++;
                            presetThread presetinf = new presetThread();
                            presetinf.Time = ScanningInterval.Interval;
                            presetinf.IP = dev.IP;
                            presetinf.DeviceID = dev.ID;

                            presetInf.Add(presetinf);

                            pdu.Add(new Pdu(PduType.GetNext));

                            foreach (var pd in pduinformation)
                            {
                                rootOid = new Oid(db.MibTreeInformations.Where(m => m.ID == pd.OidID).FirstOrDefault().OID);
                                lastOid = (Oid)rootOid.Clone();
                                pdu[count].VbList.Add(lastOid);
                            }
                        }

                    }
                }
            }

                for (int i = 0; i<=pdu.Count-1; i++)
                {
                int j = i;
                    the.Add(new Thread(() => ThreadPreset(presetInf[j].IP, presetInf[j].Time, presetInf[j].DeviceID, pdu,j)));
                    the[i].Start();
                }
                return Json("",JsonRequestBehavior.AllowGet);
        }
    }  
}