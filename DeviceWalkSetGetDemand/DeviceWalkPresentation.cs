using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.DeviceWalkSetGetDemand
{
    public class DeviceWalkPresentation
    {
        private  DeviceWalkBusinessLogic deviceWalkBusinessLogic = new DeviceWalkBusinessLogic(); 
      public  DeviceWalkPresentation() {  }

        public List<Group> GroupShowList ()
        {
            return deviceWalkBusinessLogic.GroupShowListReturn();
        }

        public List<TitleTowerName> SearchCountrieStateNameReturn (string CountrieName, string StateName, string CityName, int parentId, string countrieSettingName, string stateSettingName, int CountriesListID, string countrieName, List<TitleTowerName> TitleTowor)
        {
            return deviceWalkBusinessLogic.SearchCountrieStateName(CountrieName, StateName, CityName, parentId, countrieSettingName, stateSettingName, CountriesListID, countrieName, TitleTowor);
        }

        public List<Countrie> CountrieSearch (string countrieSearchName, List<Countrie> countrie, List<States> states, List<City> city, string searchName)
        {
            return deviceWalkBusinessLogic.CountrieSearchList(countrieSearchName, countrie, states, city, searchName);
        }
        public List<States> StateSearchListReturn(string CountrieName, string stateSearchName, List<States> states, List<City> city, int countrieID, string countrieName)
        {
            return deviceWalkBusinessLogic.StateSearchList(countrieName, stateSearchName,states, city, countrieID, countrieName);
        }

        public List<City> SearchCity (string CountrieName, string StateName, string citySearchName, int CountriesListID, List<City> city, string searchName)
        {
            return deviceWalkBusinessLogic.CitySearchList(CountrieName, StateName, citySearchName, CountriesListID, city, searchName);
        }
        public List<City> CityAddShow (string StateName, string addcityName, string countrieName, int CountriesListID)
        {
            return deviceWalkBusinessLogic.CityAdd(StateName, addcityName, countrieName, CountriesListID);
        }

        public void TowerInsertCity(string countrieName, string cityName, string stateName, int cityid, int CountriesListID)
        {
            deviceWalkBusinessLogic.TowerInsertCity(countrieName,cityName,stateName,cityid,CountriesListID);
        }
        public void TowerDeleteCity (int towerDeleteID, string cityName)
        {
            deviceWalkBusinessLogic.TowerDeleteCity(towerDeleteID, cityName);
        }

        public List<City> SelectAllCityResult (string selectallName, string StateName, string countrieName, int CountriesListID, List<City> city)
        {
            return deviceWalkBusinessLogic.SelectAllCity(selectallName, StateName, countrieName, CountriesListID, city);
        } 
        public List<WalkTowerDevice> GetPlaySelectListResult(int deviceiID,string towerName)
        {
            return deviceWalkBusinessLogic.GetPlaySelectList(deviceiID, towerName);
        }
    }
}