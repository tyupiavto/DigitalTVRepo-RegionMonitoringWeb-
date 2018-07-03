using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Web;
using AdminPanelDevice.Models;
using System.Web.Mvc;
using AdminPanelDevice.Infrastructure;

namespace AdminPanelDevice.Controllers
{
    public class LiveGetController : Controller
    {

        public List<TowerListLive> tow = new List<TowerListLive>();
        public List<WalkTowerDevice> Sensor = new List<WalkTowerDevice>();
        public List<TowerDevices> deviceSensor = new List<TowerDevices>();
        public List<List<AllDeviceLive>> deviceSensorLive = new List<List<AllDeviceLive>>();
        public List<string> towerDevice = new List<string>();
        public static int deviceCount = 0;
        public MaxCount maxDevice = new MaxCount();
        public int count;
        public List<int> getdevice = new List<int>();
        public List<int> getdevicelive = new List<int>();
        // GET: LiveTrapGet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LiveGet()
        {

            return View();
        }

        public PartialViewResult GetLiveSensor()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var allDevice = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                var deviceType = connection.Query<DeviceType>("select * from DeviceType").ToList();
               // deviceType = deviceType.OrderByDescending(t => t.Name).ToList();
                string tower = "";
                allDevice.ForEach(it =>
                {
                    if (tower != it.TowerName)
                    {
                        tower = it.TowerName;
                        towerDevice.Add(it.TowerName);
                    }
                });

                towerDevice.ForEach(device =>
                {
                    List<DeviceSensorList> Sensors = new List<DeviceSensorList>();
                    List<TowerDevices> tw = new List<TowerDevices>();
                    List<List<WalkTowerDevice>> SensorLive = new List<List<WalkTowerDevice>>();
                    AllDeviceLive sen = new AllDeviceLive();
                    List<AllDeviceLive> allDeviceLive = new List<AllDeviceLive>();
                    var Devices = connection.Query<TowerDevices>($"select * from TowerDevices where TowerName='{device}'").ToList();
                    Devices.ForEach(t =>
                    {
                        var sensor = connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where IP='{t.IP}' and DeviceName=N'{t.DeviceName}' and MapID<>0 and LogID<>0").ToList();
                        if (sensor.Count != 0)
                        {
                            DeviceSensorList deviceSensors = new DeviceSensorList();
                            deviceSensors.Sensors = sensor;
                            deviceSensors.DeviceSensor = t;
                            Sensors.Add(deviceSensors);
                            deviceSensor.Add(t);
                            tw.Add(t);
                        }
                    });
                    if (Sensors.Count != 0)
                    {
                        sen.TowerSensor = tw;
                        deviceCount = maxDevice.maxCountReturn(deviceCount, tw.Count);

                        int maxCount = Sensors.Max(s => s.Sensors.Count);

                        Sensors.ForEach(s =>
                        {
                            int sensorCount = s.Sensors.Count;
                            if (sensorCount < maxCount)
                            {
                                int cou = maxCount - sensorCount;
                                for (int itm = 0; itm < cou; itm++)
                                {
                                    s.Sensors.Add(new WalkTowerDevice());
                                }
                            }
                        });
                        int count = Sensors.Max(s => s.Sensors.Count);
                        for (int i = 0; i < count; i++)
                        {
                            List<WalkTowerDevice> Sen = new List<WalkTowerDevice>();
                            for (int j = 0; j < Sensors.Count; j++)
                            {
                                Sen.Add(Sensors[j].Sensors[i]);
                            }
                            SensorLive.Add(Sen);

                        }

                        sen.SensorDevice = SensorLive; ;
                        allDeviceLive.Add(sen);
                    }
                    if (allDeviceLive.Count != 0)
                    {
                        deviceSensorLive.Add(allDeviceLive);
                    }

                });

                //deviceType.ForEach(type =>
                //{
                //    getdevice.Clear();
                //    deviceSensorLive.ForEach(dev =>
                //    {
                //        dev.ForEach(d =>
                //        {
                //            int countdevice=0;
                //            d.TowerSensor.ForEach(t =>
                //            {
                //                if (type.Name==t.DeviceName)
                //                {
                //                    countdevice++;
                //                }
                //            });
                //            getdevice.Add(countdevice);
                //        });
                //    });
                //    getdevicelive.Add(getdevice.Max());
                //});

                //getdevicelive.ForEach(gt =>
                //{
                //    for (int j = 0; j < gt; j++)
                //    {
                //        deviceSensorLive.ForEach(df =>
                //        {
                //            df.ForEach(d =>
                //            {
                //                d.TowerSensor.ForEach(t =>
                //                {
                //                 deviceType[j]
                //                });
           
                //            });
                //    });
                //    }
                //});

                deviceSensorLive.ForEach(df =>
            {
                count = deviceCount - df[0].TowerSensor.Count();
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        df[0].TowerSensor.Add(new TowerDevices());
                        df[0].SensorDevice.ForEach(dv =>
                        {
                            dv.Add(new WalkTowerDevice());
                        });
                    }
                }
            });
                ViewBag.Sensor = deviceSensorLive;
                return PartialView("_GetLiveInformation");
            }
        }
    }
}