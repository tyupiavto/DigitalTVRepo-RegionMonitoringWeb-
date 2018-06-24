using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class AllDeviceLive
    {
        public List<TowerDevices> TowerSensor = new List<TowerDevices>();
        public List<List<WalkTowerDevice>> SensorDevice = new List<List<WalkTowerDevice>>();
    }
}