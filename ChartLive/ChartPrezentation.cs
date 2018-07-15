using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.ChartLive
{
    public class ChartPrezentation
    {
        public ChartPrezentation () { }

        private ChartLogic chartResult = new ChartLogic();
        public ChartSensorList ChartSensorResult (int DeviceID,string IP)
        {
            return chartResult.SensorListReturned(DeviceID, IP);
        }

        public List<List<AllDeviceLive>> AllSelectedSensor()
        {
            return chartResult.SelectedSensorInformation();
        }
    }
}