﻿using AdminPanelDevice.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class GetCorrectError
    {
        //MapViewInformation mapinf = new MapViewInformation();
        MapTowerLineInformation mapline = new MapTowerLineInformation();

        public GetCorrectError() { }

        public string  CompareCorrectError (int WalkID,string values,string DivideMultiply, int DeviceID,int ID,int TowerID,string value, string StartCorrect, string EndCorrect, string OneStartError, string OneEndError, string OneStartCrash, string OneEndCrash, string TwoStartError, string TwoEndError, string TwoStartCrash, string TwoEndCrash)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<HubMessage>();

            MapViewInformation mapinf = new MapViewInformation();
            if (values != "")
            {
                mapinf.Value = values;
            }
            else
            {
                mapinf.Value = value;
            }
            mapinf.TowerID = TowerID;
            mapinf.TowerLine= mapline.LinesCordinate(TowerID);
            mapinf.GetTrap = "get";
            mapinf.ID = ID;
            mapinf.DeviceID = DeviceID;
            mapinf.DivideMultiply = DivideMultiply;
            mapinf.WalkID = WalkID;

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                var cord = connection.Query<TowerGps>($"select * from TowerGps where TowerID='{TowerID}'").FirstOrDefault();
                mapinf.StartLattitube = Double.Parse(cord.Lattitube.Remove(cord.Lattitube.Length - 2), CultureInfo.InvariantCulture);
                mapinf.StartLongitube = Double.Parse(cord.Longitube.Remove(cord.Longitube.Length - 2), CultureInfo.InvariantCulture);
            }
            double number;
            var convertationDouble = Double.TryParse(value, out number);

            if (convertationDouble != false && StartCorrect != "" && EndCorrect != "")
            {

                if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(OneStartError, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) < Convert.ToDouble(OneEndError, CultureInfo.InvariantCulture))
                {
                    mapinf.MapColor = "red";
                    mapinf.LineColor = "red";
                    mapinf.TextColor = "white";
                    context.Clients.All.onHitRecorded(mapinf);
                    return "Red";
                }
                if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(OneStartCrash, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) < Convert.ToDouble(OneEndCrash, CultureInfo.InvariantCulture))
                {
                    mapinf.MapColor = "yellow";
                    mapinf.LineColor = "yellow";
                    mapinf.TextColor = "black";
                    context.Clients.All.onHitRecorded(mapinf);
                    return "Yellow";
                }
                if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(StartCorrect, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(EndCorrect, CultureInfo.InvariantCulture))
                {
                    mapinf.MapColor = "rgb(51, 51, 51);";
                    mapinf.LineColor = "#006699";
                    mapinf.TextColor = "white";
                    context.Clients.All.onHitRecorded(mapinf);
                    return "Green";
                }
                if (Convert.ToDouble(value, CultureInfo.InvariantCulture) > Convert.ToDouble(EndCorrect, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) < Convert.ToDouble(TwoStartError, CultureInfo.InvariantCulture))
                {
                    mapinf.MapColor = "yellow";
                    mapinf.LineColor = "yellow";
                    mapinf.TextColor = "black";
                    context.Clients.All.onHitRecorded(mapinf);
                    return "Yellow";
                }
                if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(TwoStartCrash, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(TwoEndError, CultureInfo.InvariantCulture) || Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(TwoEndError, CultureInfo.InvariantCulture))
                {
                    mapinf.MapColor = "red";
                    mapinf.LineColor = "red";
                    mapinf.TextColor = "white";
                    context.Clients.All.onHitRecorded(mapinf);
                    return "Red";
                }
            }
            context.Clients.All.onHitRecorded(mapinf);
            return "";
        }

    }

    public class GameHub : Hub
    {
       public GameHub ()
        {

        }
    }
}