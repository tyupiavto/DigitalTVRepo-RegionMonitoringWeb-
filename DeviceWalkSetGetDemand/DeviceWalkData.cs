using AdminPanelDevice.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.DeviceWalkSetGetDemand
{
    public class DeviceWalkData
    {
        public DeviceWalkData() { }

        public List<Group> GroupShowList()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Group>("Select * From [Group] ").ToList();
            }
        }

        public List<Countrie> CountrieList()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Countrie>("Select * From  Countrie ").ToList();
            }
        }
        public List<Countrie> CountrieSearchNameList(string countrieSearchName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Countrie>($"Select * From Countrie Where CountrieName Like N'{countrieSearchName}%'").ToList();
            }
        }

        public int SearchCountrieID(string CountrieName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Countrie>($"Select * from Countrie where CountrieName = '{CountrieName}'").FirstOrDefault().ID;
            }
        }

        public List<States> SearchStateCountrieAccordisng(int countrieID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<States>($"select * from States where CountrieID='{countrieID} '").ToList();
            }
        }

        public List<States> SearchStateList(int countrieID, string stateSearchName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<States>($"select * from States where CountrieID='{countrieID}' Select * From States where  StateName Like N'{stateSearchName}%'").ToList();
            }
        }

        public List<States> SearchStateIDList(int countrieID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<States>($"select * from States where CountrieID='{countrieID}'").ToList();
            }
        }
         
        public List<int> SelectedCityTower (int CountriesListID, int countrieID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
              return connection.Query<Tower>("select * from Tower where CountriesListID='" + CountriesListID + "' and CountriesID='" + countrieID + "'").ToList().Select(t => t.CityCheckedID).ToList();
            }
        }

        public int SearchStateID (string StateName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<States>($"select * from States where StateName='{StateName}'").FirstOrDefault().ID;
            }
        }

        public List<City> SearchCityStateIDAccordisng(int stateID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<City>($"Select * from City where  StateID='{stateID}'").ToList();
            }
        }

        public List<City> SearchCity(int stateID,string citySearchName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<City>($"select * from City where StateID='{stateID}' Select * from City where CityName like N'{citySearchName}%'").ToList();
            }
        }
        public List<States> CitySelectStateID(int countrieID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<States>($"select ID from States where CountrieID='{countrieID}'").ToList();
            }
        }
    }
}