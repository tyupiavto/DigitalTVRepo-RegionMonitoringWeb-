using IToolS.IOServers.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.Controllers
{
    public class TrapController : Controller
    {
        DeviceContext db = new DeviceContext();
        // GET: Trap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendTrap(string FileName)
        {

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

                IPEndPoint peer = new IPEndPoint(IPAddress.Any, 162);
                EndPoint inep = (EndPoint)peer;
                try
                {
                    inlen = socket.ReceiveFrom(indata, ref inep);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception {0}", ex.Message);
                    inlen = -1;
                }
                if (inlen > 0)
                {
                    // Check protocol version int 
                    int ver = SnmpPacket.GetProtocolVersion(indata, inlen);
                    SnmpV2Packet pkt = new SnmpV2Packet();
                    pkt.decode(indata, inlen);

                    //MessageBox.Show(" Community: {0}" + " " + pkt.Community.ToString());
                    //MessageBox.Show(" VarBind count: {0}" + "  " + pkt.Pdu.VbList.Count.ToString());

                    foreach (Vb v in pkt.Pdu.VbList)
                    {
                        Trap trap = new Trap();
                        string IP = inep.ToString();
                        trap.IpAddres = IP.Remove(13, (IP.Length - 13));
                        trap.CurrentOID = pkt.Pdu.TrapObjectID.ToString();
                        trap.ReturnedOID = v.Oid.ToString();
                        trap.Value = v.Value.ToString();
                        db.Traps.Add(trap);
                        db.SaveChanges();
                        
                    }
                }
                else
                {
                    if (inlen == 0)
                        Console.WriteLine("Zero length packet received.");

                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}