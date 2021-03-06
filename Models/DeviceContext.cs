﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class DeviceContext:DbContext
    {
        public DeviceContext() : base("DeviceConnection")
        {

        }

        public DbSet<Tower> towers { get; set; }
        public DbSet<Devices> devices { get; set; }
        public DbSet<DeviceType> devicesTypes { get; set; }
        public DbSet<MibTreeInformation> MibTreeInformations { get; set; }
        public DbSet<TreeStructure> TreeStructure { get; set; }
        public DbSet<WalkDevice> WalkDevices { get; set; }
        public DbSet<ScanningInterval> ScanningIntervals { get; set; }
        public DbSet<Preset> Presets { get; set; }
        public DbSet<MibGet> MibGets { get; set; }
        public DbSet<LiveValue> LiveValues { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Countrie> Countries { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<HtmlSave> HtmlSaves { get; set; }
        public DbSet<PointConnection> PointConnections { get; set; }
        public DbSet<TowerGps> towerGps { get; set; }
        public DbSet <LineConnection> LineConnections { get; set; }
        public DbSet<Trap> Traps { get; set; }
        public DbSet<TowerDevices> TowerDevices { get; set; }
        public DbSet<PointConnectionPreset> PointConnectionPresets { get; set; }
        public DbSet<PresetDiagramName> PresetDiagramNames { get; set; }
        public DbSet<TowerDevicesPreset> TowerDevicesPresets { get; set; }
        public DbSet<WalkTowerDevice> WalkTowerDevices { get; set; }
        public DbSet<WalkPreset> WalkPresets { get; set; }
        public DbSet<TowerPreset> TowerPreset { get; set; }
        public DbSet<DeviceThreadOnOff> DeviceThreadOnOff { get; set; }
        public DbSet<GetSleepThread> GetSleepThread { get; set; }
        public DbSet<AlarmLogStatus> AlarmLogStatus { get; set; }
        public DbSet<TrapListNameCheck> TrapListNameCheck { get; set; }

        public System.Data.Entity.DbSet<AdminPanelDevice.Models.WalkInformation> WalkInformations { get; set; }


    }
}