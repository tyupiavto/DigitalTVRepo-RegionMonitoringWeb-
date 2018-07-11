using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class ChartSensorList
    {
        public List<WalkTowerDevice> SensorDeviceCount = new List<WalkTowerDevice>();
        public List<MibGet> SensorGetResult = new List<MibGet>();
    }
}