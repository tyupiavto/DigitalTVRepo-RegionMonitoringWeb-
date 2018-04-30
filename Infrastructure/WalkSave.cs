using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.Infrastructure
{
    public class WalkSave
    {
        DeviceContext db = new DeviceContext();
        public WalkSave(List<WalkTowerDevice> walkList)
        {
            db.WalkTowerDevices.AddRange(walkList);
            db.SaveChanges();
        }
    }
}