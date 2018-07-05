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
        public List<DeviceType> deviceLiveAll = new List<DeviceType>();
        public string devicename = "";
        // GET: LiveTrapGet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LiveGet()
        {

            return View();
        }

        [HttpPost]
        public PartialViewResult GetLiveSensor()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var allDevice = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                var deviceType = connection.Query<DeviceType>("select * from DeviceType").ToList();
              //  deviceType = deviceType.OrderByDescending(t => t.Model).ToList();
                string tower = "";
                allDevice.ForEach(it =>
                {
                    if (tower != it.TowerName)
                    {
                        tower = it.TowerName;
                        towerDevice.Add(it.TowerName);
                    }
                });

                deviceType.ForEach(type =>
                {
                    getdevice.Clear();
                    towerDevice.ForEach(device =>
                    {
                        var Devices = connection.Query<TowerDevices>($"select * from TowerDevices where TowerName='{device}'").ToList();
                        count = 0;
                        Devices.ForEach(t =>
                        {
                            if (type.Name == t.DeviceName)
                            {
                                count++;
                                devicename = t.DeviceName;
                            }
                        });
                        getdevice.Add(count);
                    });
                    getdevicelive.Add(getdevice.Max());
                });

                for (int i = 0; i < getdevicelive.Count; i++)
                {
                    for (int j = 0; j < getdevicelive[i]; j++)
                    {
                        deviceLiveAll.Add(deviceType[i]);
                    }
                }

                towerDevice.ForEach(device =>
                {
                    List<DeviceSensorList> Sensors = new List<DeviceSensorList>();
                    List<TowerDevices> tw = new List<TowerDevices>();
                    List<List<WalkTowerDevice>> SensorLive = new List<List<WalkTowerDevice>>();
                    AllDeviceLive sen = new AllDeviceLive();
                    List<AllDeviceLive> allDeviceLive = new List<AllDeviceLive>();
                    var Devices = connection.Query<TowerDevices>($"select * from TowerDevices where TowerName='{device}'").ToList();

                    deviceLiveAll.ForEach(dl =>
                    {
                        var dv = Devices.Where(d => d.DeviceName == dl.Name).FirstOrDefault();
                        if (dv != null)
                        {
                            Devices.Remove(dv);
                            var sensor = connection.Query<WalkTowerDevice>($"select * from  WalkTowerDevice where IP='{dv.IP}' and DeviceName=N'{dv.DeviceName}' and LogID<>0").ToList();
                            if (sensor.Count != 0)
                            {
                                DeviceSensorList deviceSensors = new DeviceSensorList();
                                deviceSensors.Sensors = sensor;
                                deviceSensors.DeviceSensor = dv;
                                Sensors.Add(deviceSensors);
                                tw.Add(dv);
                            }
                            else
                            {
                                DeviceSensorList deviceSensors = new DeviceSensorList();
                                deviceSensors.Sensors = new List<WalkTowerDevice>();
                                deviceSensors.DeviceSensor = new TowerDevices();
                                Sensors.Add(deviceSensors);
                                TowerDevices tow = new TowerDevices();
                                tow.DeviceName = dl.Name;
                                tow.TowerName = device;
                                tw.Add(tow);
                            }
                        }
                        else
                        {
                            DeviceSensorList dvcsensor = new DeviceSensorList();
                            TowerDevices tow = new TowerDevices();
                            tow.DeviceName = dl.Name;
                            tow.TowerName = device;
                            dvcsensor.DeviceSensor = tow;
                            dvcsensor.Sensors = new List<WalkTowerDevice>();
                            Sensors.Add(dvcsensor);
                            tw.Add(tow);
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

                        sen.SensorDevice = SensorLive;
                        allDeviceLive.Add(sen);
                    }
                    if (allDeviceLive[0].SensorDevice.Count != 0)
                    {
                        deviceSensorLive.Add(allDeviceLive);
                    }
                });
                ViewBag.Sensor = deviceSensorLive;
                return PartialView("_GetLiveInformation");
            }
        }
    }
}