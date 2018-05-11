using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using AdminPanelDevice.Models;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class CityDeleteTower
    {
        DeviceContext db = new DeviceContext();
        public CityDeleteTower(int towerDeleteID, int cityID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                try
                {
                    connection.Query<Tower>("delete from Tower where CountriesListID='" + towerDeleteID + "' and CityCheckedID='" + cityID + "'");
                    //var deleteTower = connection.Query<Tower>("Select * towers where CountriesListID='" + towerDeleteID + "'").Where(c => c.CityCheckedID == cityID).FirstOrDefault();
                    //db.towers.Remove(deleteTower);
                    //db.SaveChanges();
                }
                catch { }
            }
        }
    }
}