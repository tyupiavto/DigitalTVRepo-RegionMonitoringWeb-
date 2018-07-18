using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AdminPanelDevice.Infrastructure
{
    public class WalkPresetImportExport
    {
        public WalkPresetImportExport () { }

        public void ExportPreset (List<WalkPreset> ExportPresets ,string presetName)
        {
            var xEleExport = new XElement("WalkPreset",
                                from emp in ExportPresets
                                select new XElement("WalkPreset",
                                             new XAttribute("PresetID", emp.ID),
                                               new XElement("PresetID", emp.PresetID),
                                               new XElement("LogID", emp.LogID),
                                               new XElement("MapID", emp.MapID),
                                               new XElement("IntervalID", emp.IntervalID),
                                               new XElement("Interval", emp.Interval),
                                               new XElement("GpsID", emp.GpsID),
                                               new XElement("DeviceName", emp.DeviceName),
                                               new XElement("DeviceID", emp.DeviceID),
                                               new XElement("OneStartError", emp.OneStartError),
                                               new XElement("OneEndError", emp.OneEndError),
                                               new XElement("OneStartCrash", emp.OneStartCrash),
                                               new XElement("OneEndCrash", emp.OneEndCrash),
                                               new XElement("StartCorrect", emp.StartCorrect),
                                               new XElement("EndCorrect", emp.EndCorrect),
                                               new XElement("TwoStartError", emp.TwoStartError),
                                               new XElement("TwoEndError", emp.TwoEndError),
                                               new XElement("TwoStartCrash", emp.TwoStartCrash),
                                               new XElement("TwoEndCrash", emp.TwoEndCrash),
                                               new XElement("OIDName", emp.OIDName),
                                               new XElement("Description", emp.Description),
                                               new XElement("WalkOID", emp.WalkOID),
                                               new XElement("MyDescription", emp.MyDescription),
                                               new XElement("DivideMultiply", emp.DivideMultiply),
                                               new XElement("StringParserInd", emp.StringParserInd)
                                           ));
            xEleExport.Save("D:\\" + presetName + ".xml");
        }

        public List<WalkPreset> ImportPreset(string FileName)
        {
            XDocument xmlDoc = XDocument.Load(FileName);

           return xmlDoc.Root.Elements("WalkPreset").Select(element => new WalkPreset
            {
                PresetID = (int)element.Element("PresetID"),
                LogID = (int)element.Element("LogID"),
                MapID = (int)element.Element("MapID"),
                IntervalID = (int)element.Element("IntervalID"),
                Interval = (int)element.Element("Interval"),
                GpsID = (int)element.Element("GpsID"),
                DeviceName = (string)element.Element("DeviceName"),
                DeviceID = (int)element.Element("DeviceID"),
                OneStartError = (string)element.Element("OneStartError"),
                OneEndError = (string)element.Element("OneEndError"),
                OneStartCrash = (string)element.Element("OneStartCrash"),
                OneEndCrash = (string)element.Element("OneEndCrash"),
                StartCorrect = (string)element.Element("StartCorrect"),
                EndCorrect = (string)element.Element("EndCorrect"),
                TwoStartError = (string)element.Element("TwoStartError"),
                TwoEndError = (string)element.Element("TwoEndError"),
                TwoStartCrash = (string)element.Element("TwoStartCrash"),
                TwoEndCrash = (string)element.Element("TwoEndCrash"),
                OIDName = (string)element.Element("OIDName"),
                Description = (string)element.Element("Description"),
                WalkOID = (string)element.Element("WalkOID"),
                MyDescription = (string)element.Element("MyDescription"),
                DivideMultiply = (string)element.Element("DivideMultiply"),
                StringParserInd = (int)element.Element("StringParserInd")
            }).ToList();
        }
    }
}