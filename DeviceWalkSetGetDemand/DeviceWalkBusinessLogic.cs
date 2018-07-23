using AdminPanelDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.DeviceWalkSetGetDemand
{
    public class DeviceWalkBusinessLogic
    {
        private DeviceWalkData deviceWalkData = new DeviceWalkData();
        public DeviceWalkBusinessLogic() { }

        public List<Group> GroupShowListReturn()
        {
            return deviceWalkData.GroupShowList();
        }

        public List<TitleTowerName> SearchCountrieStateName(string CountrieName, string StateName, string CityName, int parentId, string countrieSettingName, string stateSettingName, int CountriesListID, string countrieName, List<TitleTowerName> TitleTowor)
        {
            TitleTowerName TowerName = new TitleTowerName();

            if (countrieName == null && stateSettingName == null)
            {
                TowerName.CountrieName = "Countrie";
                TowerName.StateName = "State";
                TowerName.CityName = "City";
            }
            else
            {
                TowerName.CountrieName = countrieSettingName;
                TowerName.StateName = stateSettingName;
                TowerName.CityName = "City";
            }
            TitleTowor.Add(TowerName);
            return TitleTowor;
        }

        public List<Countrie> CountrieSearchList(string countrieSearchName, List<Countrie> countrie, List<States> states, List<City> city, string searchName)
        {
            if (countrieSearchName == null)
            {
                countrie = deviceWalkData.CountrieList();
            }
            else
            {
                if (countrieSearchName.Length >= 1)
                {
                    searchName = countrieSearchName.First().ToString().ToUpper() + countrieSearchName.Substring(1);
                    countrie = deviceWalkData.CountrieSearchNameList(countrieSearchName);
                }
                else
                {
                    countrie = deviceWalkData.CountrieSearchNameList(countrieSearchName);
                }
            }
            states.Clear();
            city.Clear();
            return countrie;
        }

        public List<States> StateSearchList(string CountrieName, string stateSearchName, List<States> states, List<City> city, int countrieID, string countrieName)
        {
            //  countrieName = CountrieName;
            if (stateSearchName == null && CountrieName != null)
            {
                countrieID = deviceWalkData.SearchCountrieID(CountrieName);
                states = deviceWalkData.SearchStateCountrieAccordisng(countrieID);
            }
            if (stateSearchName != null && CountrieName != null)
            {
                countrieID = deviceWalkData.SearchCountrieID(CountrieName);
                if (stateSearchName.Length >= 1)
                {
                    states = deviceWalkData.SearchStateList(countrieID, stateSearchName);
                    string searchName = stateSearchName.First().ToString().ToUpper() + stateSearchName.Substring(1);
                    states = states.Where(s => s.StateName.Contains(stateSearchName) || s.StateName.Contains(searchName)).ToList();
                }
                else
                {
                    states = deviceWalkData.SearchStateIDList(countrieID);
                }

            }
            city.Clear();
            return states;
        }

        public List<City> CitySearchList(string CountrieName, string StateName, string citySearchName, int CountriesListID, List<City> city, string searchName)
        {
            List<City> ct = new List<City>();

            var countrieID = deviceWalkData.SearchCountrieID(CountrieName);
            var cityChecked = deviceWalkData.SelectedCityTower(CountriesListID, countrieID);
            if (StateName != "State")
            {
                var stateID = deviceWalkData.SearchStateID(StateName);
                if (citySearchName == null & StateName != null && StateName != "")
                {
                    city = deviceWalkData.SearchCityStateIDAccordisng(stateID);
                }

                if (citySearchName != null & StateName != null && StateName != "")
                {
                    if (citySearchName.Length >= 1)
                    {
                        var citys = deviceWalkData.SearchCity(stateID, citySearchName);
                        searchName = citySearchName.First().ToString().ToUpper() + citySearchName.Substring(1);
                        citys = citys.Where(c => c.CityName.Contains(citySearchName) || c.CityName.Contains(searchName)).ToList();

                        return citys;
                    }
                    else
                    {
                        city = deviceWalkData.SearchCityStateIDAccordisng(stateID);
                    }
                }
                if (cityChecked.Count != 0)
                {
                    cityChecked.ForEach(check =>
                    {
                        city.ForEach(c =>
                        {
                            if (c.ID == check)
                            {
                                c.CheckedID = check;
                            }
                        });
                    });
                }

                return city;
            }
            else
            {
                var cityID = deviceWalkData.CitySelectStateID(countrieID);
                cityID.ForEach(cit =>
                {
                    var citys = deviceWalkData.SearchCityStateIDAccordisng(cit.ID);
                    ct.AddRange(citys);
                });

                if (citySearchName != null && citySearchName.Length >= 1)
                {
                    searchName = citySearchName.First().ToString().ToUpper() + citySearchName.Substring(1);
                    var cit = ct.Where(c => c.CityName.Contains(citySearchName) || c.CityName.Contains(searchName)).ToList();
                    if (cityChecked.Count != 0)
                    {
                        cityChecked.ForEach(check =>
                        {
                            cit.ForEach(c =>
                            {
                                if (c.ID == check)
                                {
                                    c.CheckedID = check;
                                }
                            });
                        });
                    }
                    return cit;
                }
                else
                {

                    if (cityChecked.Count != 0)
                    {
                        cityChecked.ForEach(check =>
                        {
                            ct.ForEach(c =>
                            {
                                if (c.ID == check)
                                {
                                    c.CheckedID = check;
                                }
                            });
                        });
                    }
                    return ct;
                }
            }
        }

        public List<City> CityAdd(string StateName, string addcityName, string countrieName, int CountriesListID)
        {
            var countrieID = deviceWalkData.SearchCountrieID(countrieName);
            var stateID = deviceWalkData.SearchStateID(StateName);
            var cityChecked = deviceWalkData.SelectedCityTower(CountriesListID, countrieID);

            City citys = new City();
            citys.CityName = addcityName;
            citys.StateID = stateID;

            deviceWalkData.CityAddSaveChange(citys);

            return deviceWalkData.CityList(stateID);
        }

        public void TowerInsertCity(string countrieName,string cityName, string stateName, int cityid,int CountriesListID)
        {
            Tower tower = new Tower();
            var stateID = deviceWalkData.StateID(cityName);
            stateName = deviceWalkData.StateName(stateID);
            cityid = deviceWalkData.CityID(cityName);
            deviceWalkData.PointConnectionDelete();

            var countrieID = deviceWalkData.SearchCountrieID(countrieName);
             stateID = deviceWalkData.SearchStateID(stateName);

            tower.Name = cityName;
            tower.NumberID = deviceWalkData.TowerSaveNumberID();
            tower.CountriesID = countrieID;
            tower.StateID = stateID;
            tower.CityCheckedID = cityid;
            tower.CountriesListID = CountriesListID;

            deviceWalkData.TowerCitySave(tower);
        }

        public void TowerDeleteCity (int towerDeleteID, string cityName)
        {
                try
                {
                deviceWalkData.TowerCityDelete(towerDeleteID,cityName);
                }
                catch { }
        }
        public List<City> SelectAllCity(string selectallName, string StateName,string countrieName,int CountriesListID,List<City> city)
        {
            Tower tw = new Tower();
            var countrieID = deviceWalkData.SearchCountrieID(countrieName);
            var stateID = deviceWalkData.SearchStateID(StateName);
            var cityChecked = deviceWalkData.SelectedCityState(CountriesListID, countrieID,stateID);
            if (selectallName == "All")
            {
                city.Clear();
                city = deviceWalkData.CityList(stateID);
                return city;
            }
            else
            {
                city.Clear();
                cityChecked.ForEach(item =>
                {
                    City ct = new City();
                    tw = deviceWalkData.TowerSelectCityID(item);
                    ct.CityName = tw.Name;
                    ct.StateID = tw.StateID;
                    ct.CheckedID = tw.CityCheckedID;
                    city.Add(ct);
                });
                return city;
            }
        }
    }
}