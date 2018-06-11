using AdminPanelDevice.Models;
using Dapper;
using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class TrapListen
    {
        DeviceContext db = new DeviceContext();
        Hexstring hex = new Hexstring();
        List<MibTreeInformation> mibTreeInformation = new List<MibTreeInformation>();
        List<TowerDevices> towerDevices = new List<TowerDevices>();
        List<AlarmLogStatus> alarmLog = new List<AlarmLogStatus>();
        public TrapListen()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                 mibTreeInformation = connection.Query<MibTreeInformation>("select * from TreeInformation").ToList();
                 towerDevices = connection.Query<TowerDevices>("select * from TowerDevices").ToList();
                 alarmLog = connection.Query<AlarmLogStatus>("select * from AlarmLogStatus").ToList();
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 162);
            EndPoint ep = (EndPoint)ipep;
            socket.Bind(ep);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

            bool run = true;
            int inlen = -1;
            while (run)
            {
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