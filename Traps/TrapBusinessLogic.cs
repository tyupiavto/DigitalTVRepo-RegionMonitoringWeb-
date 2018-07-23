using AdminPanelDevice.Infrastructure;
using AdminPanelDevice.Models;
using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web;

namespace AdminPanelDevice.Traps
{
    public class TrapBusinessLogic
    {
        public TrapBusinessLogic() { }
        private TrapData trapData = new TrapData();
        private TrapLogInformationList trapLogInformarion = new TrapLogInformationList();

        public List<Trap> TrapLogLoad(List<Trap> TrapLogList, int LogInd)
        {
            if (LogInd == 0)
            {
                DateTime start = DateTime.Now;
                DateTime end = start.Add(new TimeSpan(-24, 0, 0));
                TrapLogList = trapData.DateTimeSearchLog(end, start);
                TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();
            }
            return TrapLogList;
        }

        public TrapLogInformationList LogLoad(string SearchName, int SearchClear, string startTime, string endTime, List<Trap> TrapLogList, List<Trap> TrapLogListSearch, int SearchIndicator, string mapTowerDeviceName, int LogInd)
        {
            DateTime start = DateTime.Now;
            DateTime end = start.Add(new TimeSpan(-1, 0, 0));
            // ViewBag.pageNumber = pageListNumber;
            int trapID;
            if (SearchName == "" && startTime != "" && startTime != null && endTime != "" && endTime != null)
            {
                //   ViewBag.ColorDefine = 1;

                var startTm = DateTime.ParseExact(startTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                var EndTm = DateTime.ParseExact(endTime, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                TrapLogList = trapData.DateTimeSearchLog(EndTm, startTm);
                TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();

                trapLogInformarion.ErrorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                trapLogInformarion.CorrectCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                trapLogInformarion.CrashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                trapLogInformarion.WhiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                trapLogInformarion.AllCount = TrapLogList.Count;
                trapLogInformarion.TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();

                return trapLogInformarion;
            }
            else
            {
                if (SearchName == "" && startTime == "" && endTime == "")
                {
                    if (SearchIndicator == 2)
                    {
                        TrapLogList = trapData.MapTowerLog(end, start, mapTowerDeviceName);
                        LogInd = 1;
                    }
                    else
                    {
                        SearchIndicator = 0;
                        LogInd = 0;
                        TrapLogList = trapData.DateTimeSearchLog(end, start);
                    }

                    trapLogInformarion.ErrorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                    trapLogInformarion.CorrectCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                    trapLogInformarion.CrashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                    trapLogInformarion.WhiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                    trapLogInformarion.AllCount = TrapLogList.Count;
                    trapLogInformarion.TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();

                    return trapLogInformarion;
                }
                else
                {

                    if (SearchClear == 0)
                    {
                        SearchIndicator = 0;

                        trapLogInformarion.ErrorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                        trapLogInformarion.CorrectCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                        trapLogInformarion.CrashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                        trapLogInformarion.WhiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                        trapLogInformarion.AllCount = TrapLogList.Count;

                        return trapLogInformarion;
                    }
                    else
                    {
                        SearchIndicator = 1;
                        TrapLogListSearch.Clear();
                        int number;
                        var convertationInt = int.TryParse(SearchName, out number);
                        if (convertationInt != false)
                        {
                            trapID = Convert.ToInt32(SearchName);
                        }
                        else
                        {
                            trapID = -1;
                        }


                        var searchName = SearchName.First().ToString().ToUpper() + SearchName.Substring(1);
                        TrapLogListSearch = TrapLogList.Where(s => s.Countrie.Contains(SearchName) || s.States.Contains(SearchName) || s.City.Contains(SearchName) || s.TowerName.Contains(SearchName) || s.DeviceName.Contains(SearchName) || s.Description != null && s.Description.Contains(SearchName) || s.OIDName != null && s.OIDName.Contains(SearchName) || s.IpAddres.Contains(SearchName) || s.CurrentOID.Contains(SearchName) || s.ReturnedOID.Contains(SearchName) || s.Value.Contains(SearchName) || s.AlarmDescription != null && s.AlarmDescription.Contains(SearchName) || s.Countrie.Contains(searchName) || s.States.Contains(searchName) || s.City.Contains(searchName) || s.TowerName.Contains(searchName) || s.DeviceName.Contains(searchName) || s.Description != null && s.Description.Contains(searchName) || s.OIDName != null && s.OIDName.Contains(searchName) || s.IpAddres.Contains(searchName) || s.CurrentOID.Contains(searchName) || s.ReturnedOID.Contains(searchName) || s.Value.Contains(searchName) || s.AlarmDescription != null && s.AlarmDescription.Contains(searchName) || s.ID == trapID).ToList();
                        TrapLogListSearch = TrapLogListSearch.OrderByDescending(t => t.dateTimeTrap).ToList();

                        trapLogInformarion.ErrorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                        trapLogInformarion.CorrectCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                        trapLogInformarion.CrashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                        trapLogInformarion.WhiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                        trapLogInformarion.AllCount = TrapLogList.Count;

                        return trapLogInformarion;
                    }
                }
            }
        }
        public List<Trap> AlarmColorSearch(string correctColor, string errorColor, string crashColor, string whiteColor, int all, List<Trap> TrapLogListSearch, List<Trap> TrapLogList, int SearchIndicator)
        {
            if (correctColor == " " && errorColor == " " && crashColor == " " && whiteColor == " ")
            {
                all = 1;
            }
            if (all == 0)
            {
                SearchIndicator = 1;

                return TrapLogList.Where(s => s.AlarmStatus != null && s.AlarmStatus.Contains(correctColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(errorColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(crashColor) || s.AlarmStatus != null && s.AlarmStatus.Contains(whiteColor)).ToList();
            }
            else
            {
                SearchIndicator = 0;
                return TrapLogList;
            }
        }

        public List<Trap> AlarmLogStatus(string alarmColor, string deviceName, string alarmText, string returnOidText, string currentOidText, string alarmDescription, List<Trap> TrapLogList, DeviceContext db)
        {
            var alarmtextdecode = System.Uri.UnescapeDataString(alarmText);

            if (alarmColor != "white")
            {
                trapData.AlarmLogStatusUpdate(alarmColor, returnOidText, currentOidText, alarmDescription, alarmtextdecode);
            }
            else
            {
                alarmDescription = "";
                trapData.AlarmLogStatusUpdate(alarmColor, returnOidText, currentOidText, alarmDescription, alarmtextdecode);
            }
            trapData.AlarmLogStatusDelete(alarmColor, returnOidText, currentOidText, alarmtextdecode);

            if (alarmColor != "white")
            {
                AlarmLogStatus alarmlog = new AlarmLogStatus();
                alarmlog.AlarmStatus = alarmColor;
                alarmlog.AlarmText = alarmtextdecode;
                alarmlog.DeviceName = deviceName;
                alarmlog.ReturnOidText = returnOidText;
                alarmlog.CurrentOidText = currentOidText;
                alarmlog.AlarmDescription = alarmDescription;

                trapData.AlarmLogStatusSave(db, alarmlog);
            }
            TrapLogList.ForEach(item =>
            {
                bool status = Regex.IsMatch(item.Value, alarmtextdecode);
                if (status == true && item.CurrentOID == currentOidText && item.ReturnedOID == returnOidText)
                {
                    item.AlarmStatus = alarmColor;
                    item.AlarmDescription = alarmDescription;
                }
            });
            return TrapLogList;
        }

        public void TrapNameSelected(string trapListName, string check)
        {
            trapData.TrapNameSelectUpdate(trapListName, check);
        }

        public List<TrapListNameCheck> TrapTitleSelectedList()
        {
            return trapData.TrapTitleSelectedListReturn();
        }

        public List<Trap> PageLogNumberGoto(List<Trap> TrapLogList, List<Trap> TrapLogListSearch, int SearchIndicator)
        {
            if (SearchIndicator == 0 || SearchIndicator == 2)
            {
                return TrapLogList;
            }
            else
            {
                return TrapLogListSearch;
            }
        }

        public TrapLogInformationList TrapLogListSize(List<Trap> TrapLogList, List<Trap> TrapLogListSearch, int SearchIndicator)
        {
            if (SearchIndicator == 0 || SearchIndicator == 2)
            {
                trapLogInformarion.ErrorCount = TrapLogList.Where(t => t.AlarmStatus == "red").ToList().Count;
                trapLogInformarion.CorrectCount = TrapLogList.Where(t => t.AlarmStatus == "green").ToList().Count;
                trapLogInformarion.CrashCount = TrapLogList.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                trapLogInformarion.WhiteCount = TrapLogList.Where(t => t.AlarmStatus == "white").ToList().Count;
                trapLogInformarion.AllCount = TrapLogList.Count;
                trapLogInformarion.TrapLogList = TrapLogList.OrderByDescending(t => t.dateTimeTrap).ToList();

                return trapLogInformarion;
            }
            else
            {
                trapLogInformarion.ErrorCount = TrapLogListSearch.Where(t => t.AlarmStatus == "red").ToList().Count;
                trapLogInformarion.CorrectCount = TrapLogListSearch.Where(t => t.AlarmStatus == "green").ToList().Count;
                trapLogInformarion.CrashCount = TrapLogListSearch.Where(t => t.AlarmStatus == "yellow").ToList().Count;
                trapLogInformarion.WhiteCount = TrapLogListSearch.Where(t => t.AlarmStatus == "white").ToList().Count;
                trapLogInformarion.AllCount = TrapLogListSearch.Count;
                trapLogInformarion.TrapLogList = TrapLogListSearch.OrderByDescending(t => t.dateTimeTrap).ToList();

                return trapLogInformarion;
            }
        }

        public void SendTrapListen(bool trapInd)
        {
            if (trapInd == true)
            {
                trapInd = false;

                TrapListen();
            }
        }

        public void TrapListen()
        {
            List<MibTreeInformation> mibTreeInformation = new List<MibTreeInformation>();
            List<TowerDevices> towerDevices = new List<TowerDevices>();
            List<AlarmLogStatus> alarmLog = new List<AlarmLogStatus>();

            mibTreeInformation = trapData.MibTreeInformationList();
            towerDevices = trapData.TowerDevicesList();
            alarmLog = trapData.AlarmLogStatusList();

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 162);
            EndPoint ep = (EndPoint)ipep;
            socket.Bind(ep);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

            bool run = true;
            int inlen = -1;
            while (run)
            {

                //  alarmLog = connection.Query<AlarmLogStatus>("select * from AlarmLogStatus").ToList();

                byte[] indata = new byte[16 * 1024];

                IPEndPoint peer = new IPEndPoint(IPAddress.Any, 0);
                EndPoint inep = (EndPoint)peer;
                try
                {
                    inlen = socket.ReceiveFrom(indata, ref inep);
                }
                catch (Exception ex)
                {
                    inlen = -1;
                }
                if (inlen > 0)
                {
                    int ver = SnmpPacket.GetProtocolVersion(indata, inlen);
                    if (ver == 0)
                    {
                        try
                        {
                            SnmpV1TrapPacket pkt = new SnmpV1TrapPacket();
                            pkt.decode(indata, inlen);
                            new SnmpVersionOne(pkt, inep, mibTreeInformation, towerDevices, alarmLog);
                        }
                        catch (Exception e)
                        {

                        }
                    }

                    if (ver == 2 || ver == 1)
                    {
                        try
                        {
                            SnmpV2Packet pkt = new SnmpV2Packet();
                            pkt.decode(indata, inlen);
                            new SnmpVersionTwo(pkt, inep, mibTreeInformation, towerDevices, alarmLog);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
        }
    }
}