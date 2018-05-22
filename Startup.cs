using System;
using System.Threading;
using System.Threading.Tasks;
using AdminPanelDevice.Infrastructure;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AdminPanelDevice.Startup))]

namespace AdminPanelDevice
{
 
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);

            //GlobalConfiguration.Configuration.UseSqlServerStorage("DeviceConnection");

            //app.UseHangfireDashboard();
            //// var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            ////  var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"),TimeSpan.FromDays(1));
            //// RecurringJob.AddOrUpdate(() => Console.WriteLine("semovida"), Cron.MinuteInterval(4));
            //var options = new DashboardOptions
            //{
            //    AuthorizationFilters = new[]
            //  {
            //    new LocalRequestsOnlyAuthorizationFilter()
            //}
            //};

            //app.UseHangfireDashboard("/hangfire", options);
           // app.UseHangfireServer();
        }
    }
}
