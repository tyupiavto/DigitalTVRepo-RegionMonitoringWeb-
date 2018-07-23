using AdminPanelDevice.Models;
using Dapper;
using IToolS.IOServers.Snmp;
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
        private IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString);

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

        public List<int> SelectedCityTower(int CountriesListID, int countrieID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Tower>($"select * from Tower where CountriesListID='{CountriesListID}' and CountriesID='{countrieID}'").ToList().Select(t => t.CityCheckedID).ToList();
            }
        }

        public int SearchStateID(string StateName)
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

        public List<City> SearchCity(int stateID, string citySearchName)
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
        public List<City> CityList(int StateID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<City>($"select * from City where StateID='{StateID}'").ToList();
            }
        }
        public void CityAddSaveChange(City city)
        {
            DeviceContext db = new DeviceContext();
            db.Citys.Add(city);
            db.SaveChanges();
        }

        public int StateID(string cityName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<City>($"Select * from City where CityName='{cityName}'").FirstOrDefault().StateID;
            }
        }

        public string StateName(int stateID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<States>($"Select * from States where ID='{stateID}'").FirstOrDefault().StateName;
            }
        }

        public int CityID(string cityName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<City>($"Select * from City where CityName='{cityName}'").FirstOrDefault().ID;
            }
        }
        public void PointConnectionDelete ()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<PointConnection>("delete From  PointConnection ");
            }
        }

        public int TowerSaveNumberID ()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
              return connection.Query<Tower>("select NumberID from Tower").ToList().LastOrDefault().ID+1;
            }
        }
        public void TowerCitySave (Tower tower)
        {
            DeviceContext db = new DeviceContext();
            db.towers.Add(tower);
            db.SaveChanges();
        }

        public void TowerCityDelete(int towerDeleteID, string cityName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
              var cityID= connection.Query<City>($"Select * from City where CityName='{cityName}'").FirstOrDefault().ID;
                connection.Query<Tower>($"delete from Tower where CountriesListID='{towerDeleteID}' and CityCheckedID='{cityID}'");
            }
        }

        public Tower TowerSelectCityID(int selectID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Tower>($"select * from Tower where CityCheckedID='{selectID}'").FirstOrDefault(); ;
            }
        }
        public List<int> SelectedCityState(int CountriesListID, int countrieID,int StateID)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                return connection.Query<Tower>($"select * from Tower where CountriesListID='{CountriesListID}' and CountriesID='{countrieID}' and StateID='{StateID}'").ToList().Select(t => t.CityCheckedID).ToList();
            }
        }

        public List<WalkTowerDevice> SelectedLogMapList (int deviceID,string towerName)
        {
            //using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            //{
                return connection.Query<WalkTowerDevice>($"select * from WalkTowerDevice where DeviceID='{deviceID}' and TowerName='{towerName}' and LogID<>0").ToList();
            //}
        }
        public void UpdateWalkTowerDeviceGetSend(SnmpPacket result,string getOid,string IP)
        {
            connection.Query<WalkTowerDevice>($"update WalkTowerDevice set Type='{result.Pdu.VbList[0].Value.ToString()}' where WalkOID='{getOid}' and IP='{IP}'");
        }
    }
}