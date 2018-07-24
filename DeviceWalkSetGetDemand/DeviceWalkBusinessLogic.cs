using AdminPanelDevice.Models;
using IToolS.IOServers.Snmp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AdminPanelDevice.DeviceWalkSetGetDemand
{
    public class DeviceWalkBusinessLogic
    {
        private DeviceWalkData deviceWalkData = new DeviceWalkData();
        private SnmpPacket result;
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

        public void TowerInsertCity(string countrieName, string cityName, string stateName, int cityid, int CountriesListID)
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

        public void TowerDeleteCity(int towerDeleteID, string cityName)
        {
            try
            {
                deviceWalkData.TowerCityDelete(towerDeleteID, cityName);
            }
            catch { }
        }
        public List<City> SelectAllCity(string selectallName, string StateName, string countrieName, int CountriesListID, List<City> city)
        {
            Tower tw = new Tower();
            var countrieID = deviceWalkData.SearchCountrieID(countrieName);
            var stateID = deviceWalkData.SearchStateID(StateName);
            var cityChecked = deviceWalkData.SelectedCityState(CountriesListID, countrieID, stateID);
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

        public List<WalkTowerDevice> GetPlaySelectList(int deviceID, string towerName)
        {
            var getList = deviceWalkData.SelectedLogMapList(deviceID, towerName);
            getList.ForEach(item =>
            {
                item.Type = GetSend(item.WalkOID, item.Version, "public", item.IP, 161);
            });
            return getList;
        }

        public string GetSend(string getOid, string Version, string communityRead, string IP, int Port)
        {
            OctetString community = new OctetString(communityRead);

            AgentParameters param = new AgentParameters(community);
            if (Version == "V1")
            {
                param.Version = SnmpVersion.Ver1;
            }
            if (Version == "V2")
            {
                param.Version = SnmpVersion.Ver2;
            }
            IpAddress agent = new IpAddress(IP);

            UdpTarget target = new UdpTarget((IPAddress)agent, Port, 2000, 1);

            Pdu pdu = new Pdu(PduType.Get);
            try
            {
                if (pdu.RequestId != 0)
                {
                    pdu.RequestId += 1;
                }
                pdu.VbList.Clear();
                pdu.VbList.Add(getOid);

                if (Version == "V1")
                {
                    result = (SnmpV1Packet)target.Request(pdu, param);
                }
                if (Version == "V2")
                {
                    result = (SnmpV2Packet)target.Request(pdu, param);
                }
            }
            catch (Exception e) { }
            target.Close();
            deviceWalkData.UpdateWalkTowerDeviceGetSend(result, getOid, IP);
            return result.Pdu.VbList[0].Value.ToString();
        }

        //public void SaveDiagram (ReturnedHtml files,string Html)
        // {
        //     Html = files.Html;
        //     string text = files.Html;

        //     if (text == "undefined")
        //         text = "";
        //     try
        //     {
        //         var path = Server.MapPath(@"~/HtmlText/html.txt");
        //         System.IO.StreamWriter htmlText = new StreamWriter(path);

        //         htmlText.Write(text);
        //         htmlText.Close();

        //     }
        //     catch { }
        // }

        public void CreatePointConnections(Array[] connections)
        {
            try
            {
                List<PointConnection> point = new List<PointConnection>();

                deviceWalkData.PointConnectionDelete();

                ArrayList PointConnect = new ArrayList();

                PointConnect.AddRange(connections);

                PointConnection pointConnection = new PointConnection();
                foreach (string[] item in PointConnect)
                {
                    pointConnection.GetUuids = item[0];
                    pointConnection.SourceId = item[1];
                    pointConnection.TargetId = item[2];
                    pointConnection.PointRight = item[3];
                    pointConnection.PointLeft = item[4];
                    deviceWalkData.PointConnectionSave(pointConnection);
                }
            }
            catch (Exception e)
            {
                int ss;
            }
        }

        public List<DeviceType> DeviceGroupListReturn(int deviceGroupID)
        {
            return deviceWalkData.DeviceGroupList(deviceGroupID);
        }

        public WalkMibSetting LoadWalkMibSettingInformation(string deviceName, string towerName, int deviceID, int defineWalk, List<WalkTowerDevice> walkList)
        {
            WalkMibSetting walkMibSetting = new WalkMibSetting();
            walkMibSetting.deviceTypeID = deviceWalkData.DeviceTypeID(deviceName);
            walkMibSetting.intervalTime = deviceWalkData.ScanIntervalList();
            walkMibSetting.TowerIP = deviceWalkData.TowerIP(walkMibSetting.deviceTypeID, towerName, deviceID);

            walkMibSetting.WalkList = deviceWalkData.WalkTowerDeviceList(deviceName, towerName, deviceID);

            if (walkMibSetting.WalkList.Count >= 1 && defineWalk == 1)
            {
                walkMibSetting.MibWalkIndicator = false;
                walkMibSetting.DefineWalk = true;
                walkMibSetting.SelectLogList = deviceWalkData.SelectLogList(deviceName, towerName, deviceID);
                walkMibSetting.SelectMapList = deviceWalkData.SelectMapList(deviceName, towerName, deviceID);
                walkMibSetting.SelectIntervalList = deviceWalkData.SelectIntervalList(deviceName, towerName, deviceID);
                walkMibSetting.SelectGpsList = deviceWalkData.SelectGpsList(deviceName, towerName, deviceID);
                walkMibSetting.PresetInd = 1;

                return walkMibSetting;
            }
            else
            {
                walkMibSetting.MibInformation = deviceWalkData.MibTreeInformationList(walkMibSetting.deviceTypeID);
                return walkMibSetting;
            }
        }
    }
}