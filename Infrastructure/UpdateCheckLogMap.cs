using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdminPanelDevice.Models;
using System.Threading.Tasks;

namespace AdminPanelDevice.Infrastructure
{
    interface UpdateCheckLogMap
    {
        void UpdateChechkLog(int checkLog, int walkCheckID,string towerName,string IP);
        void UpdateChechkMap(int checkMap, int walkCheckID,string towerName, string IP);
        void UpdateInterval(int intervalID, int Interval,string towerName, string IP);
        void WalkPresetSave(List<WalkTowerDevice> walkList,int presetID,string DeviceName, string TowerID, string IP);
    }
}