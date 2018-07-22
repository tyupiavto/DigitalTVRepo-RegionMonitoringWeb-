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
    }
}