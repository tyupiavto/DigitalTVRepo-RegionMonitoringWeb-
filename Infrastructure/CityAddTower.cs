using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class CityAddTower
    {
        DeviceContext db = new DeviceContext();
        private Tower tower = new Tower();
        public CityAddTower(string countrieName, string stateName,string cityName, int cityid, int CountriesListID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<PointConnection>("delete From  PointConnection ");

                var countrieID = connection.Query<Countrie>("select * from Countries where CountrieName='" + countrieName + "'").FirstOrDefault().ID;
                var stateID = connection.Query<States>("select * from States StateName='" + stateName + "'").FirstOrDefault().ID;

                tower.Name = cityName;
                tower.NumberID = db.towers.Select(s => s.NumberID).ToList().LastOrDefault() + 1;
                tower.CountriesID = countrieID;
                tower.StateID = stateID;
                tower.CityCheckedID = cityid;
                tower.CountriesListID = CountriesListID;

                db.towers.Add(tower);
                db.SaveChanges();
            }
        }
    }
}