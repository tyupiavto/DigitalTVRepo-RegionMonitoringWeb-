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
        int UpdateChechkLog(int checkLog, int walkCheckID,string towerName, int deviceID);
        int UpdateChechkMap(int checkMap, int walkCheckID,string towerName, int deviceID);
        void UpdateChechkGps(int checkGps, int walkCheckID, string towerName, int deviceID);
        void UpdateInterval(int intervalID, int Interval,string towerName, int deviceID);
        void UpdateLogMapSetting(LogMapSettingValue value, string towerName, int deviceID);
        void WalkPresetSave(List<WalkTowerDevice> walkList,int presetID,string DeviceName, string TowerID, int deviceID);
    }
}