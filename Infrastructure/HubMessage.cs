using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AdminPanelDevice.Infrastructure
{
    [HubName("hubMessage")]
    public class HubMessage : Hub
    {
        public void Send(string ColorMap)
        {
         //  this.Clients.All.onHitRecorded("2");
        }
    }
}