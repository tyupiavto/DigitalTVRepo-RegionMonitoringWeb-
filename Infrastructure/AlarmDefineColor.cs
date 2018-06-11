﻿using AdminPanelDevice.Models;
using Dapper;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class AlarmDefineColor
    {
        public AlarmDefineColor ()
        {

        }
        MapViewInformation mapinformation = new MapViewInformation();
        MapTowerLineInformation mapline = new MapTowerLineInformation();
        public string AlarmColorDefines(string value, List<AlarmLogStatus> alarmLog, TowerDevices tDevice)
        {
            bool status = false; string statuscolor = "white";
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                alarmLog.ForEach(item =>
                {

                    status = Regex.IsMatch(value, item.AlarmText);
                    if (status == true)
                    {
                        statuscolor = item.AlarmStatus;
                        var towerID = Convert.ToInt32(tDevice.TowerID.Substring(6, tDevice.TowerID.Length - 6));
                        var context = GlobalHost.ConnectionManager.GetHubContext<HubMessage>();
                        mapinformation.Value = "";
                        mapinformation.TowerID = towerID;
                        mapinformation.TowerLine = mapline.LinesCordinate(towerID);
                        var cord = connection.Query<TowerGps>($"select * from TowerGps where TowerID='{towerID}'").FirstOrDefault();
                        mapinformation.StartLattitube = Double.Parse(cord.Lattitube.Remove(cord.Lattitube.Length - 2), CultureInfo.InvariantCulture);
                        mapinformation.StartLongitube = Double.Parse(cord.Longitube.Remove(cord.Longitube.Length - 2), CultureInfo.InvariantCulture);
                        mapinformation.MapColor = item.AlarmStatus;
                        mapinformation.LineColor = item.AlarmStatus;
                        mapinformation.TextColor = "white";
                        context.Clients.All.onHitRecorded(mapinformation);
                    }
                });
            }
            return statuscolor;
                 
        }
    }
}