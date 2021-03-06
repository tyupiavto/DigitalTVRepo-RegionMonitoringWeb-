﻿using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using Microsoft.AspNet.SignalR;
using System.Text.RegularExpressions;

namespace AdminPanelDevice.Infrastructure
{
    
    public class GetThread
    {
        public GetThread() { }
        public SnmpPacket result;
        public DeviceContext db = new DeviceContext();
        public GetCorrectError correctError = new GetCorrectError();
        MapViewInformation mapinformation = new MapViewInformation();
        public void ThreadPreset(int WalkID,int StringParserInd,string DivideMultiply, int ID,int TowerID,string IP, int time, int Deviceid, string getOid, string Version,string StartCorrect, string EndCorrect,string OneStartError,string OneEndError,string OneStartCrash,string OneEndCrash, string TwoStartError,string TwoEndError,string TwoStartCrash,string TwoEndCrash)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<HubMessage>();
            // string Version = "V2";
            var values = "";
            string communityRead = "public";
            int Port = 161;
            while (true)
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

                    foreach (Vb v in result.Pdu.VbList)
                    {
                        MibGet get = new MibGet();
                        get.Value = v.Value.ToString();
                        get.DeviceID = Deviceid;
                        get.dateTime = DateTime.Now;
                        get.WalkOID = v.Oid.ToString();
                        get.IP = IP;
                        get.WalkID = WalkID;
                        if (StringParserInd == 1)
                        {
                            char[] arr = get.Value.ToCharArray();
                            get.Value = new string(Array.FindAll<char>(arr, (c => (char.IsDigit(c) || c == '.'))));
                            values = new string(Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '.'))));
                        }
                        get.ResultCorrectError = correctError.CompareCorrectError(WalkID, values,DivideMultiply,Deviceid,ID,TowerID, get.Value, StartCorrect, EndCorrect, OneStartError, OneEndError, OneStartCrash, OneEndCrash, TwoStartError, TwoEndError, TwoStartCrash, TwoEndCrash);

                        db.MibGets.Add(get);
                        db.SaveChanges();
                    }
            }
                catch (Exception e)
            {
                var ks = 1;
            }

            target.Close();
                Thread.Sleep((time * 1000));
            }
        }
    }
}