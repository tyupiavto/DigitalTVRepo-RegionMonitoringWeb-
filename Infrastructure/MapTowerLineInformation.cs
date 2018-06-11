using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class MapTowerLineInformation
    {
        public MapTowerLineInformation () { }

        public List<mapTower> LinesCordinate (int TowerID)
        {
            List<mapTower> mapTowerCordinate = new List<mapTower>();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
              var mapConnection = connection.Query<LineConnection>($"select * from LineConnection where ParentTowerID='{TowerID}'").ToList();
                if (mapConnection.Count !=0)
                {
                    mapConnection.ForEach(item =>
                    {
                        mapTower mapcor = new mapTower();
                        var cord = connection.Query<TowerGps>($"select * from TowerGps where TowerID='{item.ChildTowerID}'").FirstOrDefault();
                        if (cord != null)
                        {
                            mapcor.lattitube = Double.Parse(cord.Lattitube.Remove(cord.Lattitube.Length - 2), CultureInfo.InvariantCulture);
                            mapcor.longitube = Double.Parse(cord.Longitube.Remove(cord.Longitube.Length - 2), CultureInfo.InvariantCulture);
                            mapcor.towerID = cord.TowerID;
                            mapTowerCordinate.Add(mapcor);
                        }
                    });
                }
            }
            return mapTowerCordinate;
        }
    }
}