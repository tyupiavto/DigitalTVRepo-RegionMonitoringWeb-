using AdminPanelDevice.Infrastructure;
using AdminPanelDevice.Models;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace AdminPanelDevice
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();
        private readonly object _lockObject = new object();
        private bool _started;
        public static List<GetSleepThread> getThread = new List<GetSleepThread>();
        private BackgroundJobServer _backgroundJobServer;
        public static string hung;

        private HangfireBootstrapper()
        {
        }

        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;

                HostingEnvironment.RegisterObject(this);

                GlobalConfiguration.Configuration.UseSqlServerStorage("DeviceConnection");

                Thread thread = new Thread(() => new TrapListen());
                thread.Start();

                SleepInformation sleepget = new SleepInformation();
                getThread=sleepget.SleepGetInformation(true);

                 _backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }
        public List<GetSleepThread> GetThreadStart ()
        {
            return getThread;
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}