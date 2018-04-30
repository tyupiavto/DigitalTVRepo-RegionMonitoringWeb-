using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelDevice.Infrastructure
{
    interface UpdateCheckLogMap
    {
        void UpdateChechkLog(int checkLog, int walkCheckID);
        void UpdateChechkMap(int checkMap, int walkCheckID);
        void UpdateInterval(int intervalID, int Interval);
        void WalkPresetSave(string DeviceName, string TowerID);
    }
}