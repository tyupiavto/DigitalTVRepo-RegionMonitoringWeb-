using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class DeviceSensorList
    {
        public List<WalkTowerDevice> Sensors { get; set; }
        public TowerDevices DeviceSensor { get; set; }
    }
}