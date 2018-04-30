using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class PresetFill
    {
        DeviceContext db = new DeviceContext();

        //public List<WalkTowerDevice> presetFillLoad(List<WalkDevice> walkList, List<WalkDevice> PresetList, List<int> CheckLog, List<int> checkMap , string presetName)
        //{
        //    foreach (var log in CheckLog)
        //    {
        //        WalkDevice walkpreset = new WalkDevice();
        //        Point p = new Point();
        //        if (checkMap.Contains(log))
        //        {
        //            checkMap.Remove(log);
        //            walkpreset.MapID = log;
        //            walkpreset.LogID = log;
        //        }
        //        else
        //        {
        //            walkpreset.LogID = log;
        //        }
              
        //        walkpreset.WalkID = log;
        //        walkpreset.CheckID = log;
        //        walkpreset.OidID = OidID(walkList[log - 1].WalkOID);
        //        walkpreset.PresetID = db.Presets.Where(pr=> pr.PresetName == presetName).FirstOrDefault().ID;
        //        walkpreset.WalkOID = walkList[log - 1].WalkOID;
        //        walkpreset.WalkDescription = walkList[log - 1].WalkDescription;
        //        walkpreset.Type = walkList[log - 1].Type;
        //        walkpreset.Time = walkList[log- 1].Time;

        //        PresetList.Add(walkpreset);
        //    }

        //    foreach (var map in checkMap)
        //    {
        //        WalkDevice walkpreset = new WalkDevice();
        //         string oid = walkList[map - 1].WalkOID;


        //        walkpreset.OidID = OidID(oid);
        //        walkpreset.WalkID = map;
        //        walkpreset.CheckID = map;
        //        walkpreset.PresetID = db.Presets.Where(p => p.PresetName == presetName).FirstOrDefault().ID;
        //        walkpreset.WalkOID = walkList[map - 1].WalkOID;
        //        walkpreset.WalkDescription = walkList[map - 1].WalkDescription;
        //        walkpreset.Type = walkList[map - 1].Type;
        //        walkpreset.Time = walkList[map - 1].Time;
        //        walkpreset.MapID = map;
        //        PresetList.Add(walkpreset);
        //    }
          
            
        //    return PresetList;
        //}


        public int OidID (string oid)
        {
            int ID=0;
            var OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();

            if (OidMibdescription == null)
            {
                oid = oid.Remove(oid.Length - 1);
                oid = oid.Remove(oid.Length - 1);
                OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
            }

            if (OidMibdescription == null)
            {
                oid = oid.Remove(oid.Length - 1);
                oid = oid.Remove(oid.Length - 1);
                OidMibdescription = db.MibTreeInformations.Where(o => o.OID == oid).FirstOrDefault();
                if (OidMibdescription != null)
                   ID = OidMibdescription.ID;
            }
            else
            {
               ID = OidMibdescription.ID;
            }
            return ID;
        }
    }
}