using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanelDevice.Infrastructure;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.ChartLive
{
    public class ChartLogic
    {
        private ChartSensorList chartSensorList = new ChartSensorList();
        private ChartData chartData = new ChartData();
        private static int deviceCount = 0;
        private MaxCount maxDevice = new MaxCount();
        private List<List<AllDeviceLive>> deviceSensorLive = new List<List<AllDeviceLive>>();
        public ChartLogic() { }

        public ChartSensorList SensorListReturned(int DeviceID, string IP)
        {
            // chartSensorList.SensorDeviceCount=chartData.DeviceChartList(DeviceID, IP);
            chartSensorList.SensorDeviceCount.AddRange(chartData.DeviceChartList(DeviceID, IP));

            chartSensorList.SensorDeviceCount.ForEach(dv =>
            {
                chartSensorList.SensorGetResult.AddRange(chartData.SensorGetList(dv.DeviceID, dv.IP, dv.WalkOID));
            });
            return chartSensorList;
        }

        public List<List<AllDeviceLive>> SelectedSensorInformation()
        {
            List<string> towerDevice = new List<string>();
            List<int> getdevicelive = new List<int>();
            List<DeviceType> deviceLiveAll = new List<DeviceType>();

            var deviceType = chartData.deviceTypesList();
            towerDevice = chartData.TowerCountList();

            deviceType.ForEach(type =>
            {
                var deviceCount = chartData.DeviceMaxCount(type.ID);
                if (deviceCount.ToList().Count != 0)
                {
                    getdevicelive.Add(deviceCount.Max());
                }
                else
                {
                    getdevicelive.Add(0);
                }
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

                var Devices = chartData.TowerDevicesList(device);

                deviceLiveAll.ForEach(dl =>
                {
                    var dv = Devices.Where(d => d.DeviceName == dl.Name).FirstOrDefault();
                    if (dv != null)
                    {
                        Devices.Remove(dv);
                        var sensor = chartData.SensorSelected(dv.IP, dv.DeviceName);
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

                            var tv = Devices.Where(d => d.TowerName == device && d.DeviceName == dl.Name).FirstOrDefault();
                            if (tv != null)
                            {
                                tow.IP = tv.IP;
                            }
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
                        tw.Add(tow);
                        dvcsensor.DeviceSensor = tow;
                        dvcsensor.Sensors = new List<WalkTowerDevice>();
                        Sensors.Add(dvcsensor);
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
                            if (Sensors[j].Sensors[i].DivideMultiply != null && Sensors[j].Sensors[i].DivideMultiply != "")
                            {
                                string values = Sensors[j].Sensors[i].DivideMultiply.Substring(1, Sensors[j].Sensors[i].DivideMultiply.Length - 1);
                                var divide = (Sensors[j].Sensors[i].DivideMultiply.Substring(0, 1));
                                if (divide == "/")
                                {
                                    Sensors[j].Sensors[i].Type = (double.Parse((Sensors[j].Sensors[i].Type), System.Globalization.CultureInfo.InvariantCulture) / Convert.ToInt32(values)).ToString();
                                }
                            }
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

            return deviceSensorLive;
        }
    }
}