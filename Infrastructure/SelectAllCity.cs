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
    public class SelectAllCity
    {
        DeviceContext db = new DeviceContext();
        public SelectAllCity(List<City> city, string countrieName, string StateName, int CountriesListID, string selectallName)
        {
            //List<City> city = new List<City>();
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                Tower tw = new Tower();
                var countrieID = connection.Query<Countrie>("Select * From Countries where CountrieName='" + countrieName + "'").FirstOrDefault().ID;
                var stateID = connection.Query<States>("Select * From States Where StateName='" + StateName + "'").FirstOrDefault().ID;
                //db.States.Where(s => s.StateName == StateName).FirstOrDefault().ID;
                var cityChecked = connection.Query<Tower>("Select * From towers where CountriesListID='" + CountriesListID + "' and CountriesID='" + countrieID + "'and StateID='" + stateID + "'").ToList().Select(t => t.CityCheckedID).ToList();
                    //db.towers.Where(t => t.CountriesListID == CountriesListID && t.CountriesID == countrieID && t.StateID == stateID).ToList().Select(t => t.CityCheckedID).ToList();
                if (selectallName == "All")
                {
                    city.Clear();
                    city = db.Citys.Where(c => c.StateID == stateID).ToList();
                    // ViewBag.city = city;
                }
                else
                {
                    city.Clear();
                    foreach (var item in cityChecked)
                    {
                        City ct = new City();
                        tw = db.towers.Where(t => t.CityCheckedID == item).FirstOrDefault();
                        ct.CityName = tw.Name;
                        ct.StateID = tw.StateID;
                        ct.CheckedID = tw.CityCheckedID;
                        city.Add(ct);
                    }
                    //  ViewBag.city = city;
                }
            }
        }
    }
}