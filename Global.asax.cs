using AdminPanelDevice.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AdminPanelDevice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Thread thread = new Thread(() => new TrapListen());
            thread.Start();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
