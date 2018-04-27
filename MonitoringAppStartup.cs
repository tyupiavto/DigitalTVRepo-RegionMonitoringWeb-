using AdminPanelDevice.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdminPanelDevice
{
    public class MonitoringAppStartup : System.Web.Hosting.IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            try
            {
                //Thread thread = new Thread(() => new TrapListen());
                //thread.Start();
            }
            catch
            {
            }
        }
    }
}