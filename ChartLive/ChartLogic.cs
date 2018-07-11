using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.ChartLive
{
    public class ChartLogic
    {
        private ChartSensorList chartSensorList = new ChartSensorList();
        private ChartData chartData = new ChartData();
       
        public ChartLogic () { }

        public ChartSensorList SensorListReturned (int DeviceID, string IP)
        {
            // chartSensorList.SensorDeviceCount=chartData.DeviceChartList(DeviceID, IP);
            chartSensorList.SensorDeviceCount.AddRange(chartData.DeviceChartList(DeviceID, IP));

            chartSensorList.SensorDeviceCount.ForEach(dv => {
                chartSensorList.SensorGetResult.AddRange(chartData.SensorGetList(dv.DeviceID, dv.IP, dv.WalkOID));
            });
            return chartSensorList;
        }
    }
}