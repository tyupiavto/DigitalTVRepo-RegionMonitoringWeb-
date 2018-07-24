using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class WalkMibSetting
    {
        public List<WalkTowerDevice> WalkList = new List<WalkTowerDevice>();
        public List<WalkTowerDevice> SelectLogList = new List<WalkTowerDevice>();
        public List<WalkTowerDevice> SelectMapList = new List<WalkTowerDevice>();
        public List<WalkTowerDevice> SelectGpsList = new List<WalkTowerDevice>();
        public List<WalkTowerDevice> SelectIntervalList = new List<WalkTowerDevice>();
        public List<ScanningInterval> intervalTime  = new List<ScanningInterval>();
        public List<MibTreeInformation> MibInformation = new List<MibTreeInformation>();
        public int deviceTypeID { get; set; }
        public string TowerIP { get; set; }
        public bool DefineWalk { get; set; }
        public bool MibWalkIndicator { get; set; }
        public int PresetInd { get; set; }
    }
}