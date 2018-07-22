﻿using AdminPanelDevice.Models;
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
    }
}