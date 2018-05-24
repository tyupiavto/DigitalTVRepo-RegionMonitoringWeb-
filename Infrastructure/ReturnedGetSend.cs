using Dapper;
using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.Infrastructure
{
    public class ReturnedGetSend
    {
        SnmpPacket result;
        public ReturnedGetSend()
        {
            
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

            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                connection.Query<WalkTowerDevice>($"update WalkTowerDevice set Type='{result.Pdu.VbList[0].Value.ToString()}' where WalkOID='{getOid}' and IP='{IP}'");
            }
                return result.Pdu.VbList[0].Value.ToString();
        }
    }
}