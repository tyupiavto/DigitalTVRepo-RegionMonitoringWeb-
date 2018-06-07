using AdminPanelDevice.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class GetCorrectError
    {
        public GetCorrectError() { }

        public string  CompareCorrectError (string value, string StartCorrect, string EndCorrect, string OneStartError, string OneEndError, string OneStartCrash, string OneEndCrash, string TwoStartError, string TwoEndError, string TwoStartCrash, string TwoEndCrash)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<HubMessage>();
            //context.Clients.All.onHitRecorded("1");
       
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(OneStartError, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(OneEndError, CultureInfo.InvariantCulture))
            {
                context.Clients.All.onHitRecorded("red");
                return "Red";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(OneStartCrash, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <Convert.ToDouble(OneEndCrash, CultureInfo.InvariantCulture))
            {
                context.Clients.All.onHitRecorded("yellow");
                return "Yellow";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(StartCorrect , CultureInfo.InvariantCulture) && Convert.ToDouble(value , CultureInfo.InvariantCulture) <= Convert.ToDouble(EndCorrect, CultureInfo.InvariantCulture))
            {
                context.Clients.All.onHitRecorded("green");
                return "Green";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >Convert.ToDouble(TwoStartError, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(TwoEndError, CultureInfo.InvariantCulture))
            {
                context.Clients.All.onHitRecorded("yellow");
                return "Yellow";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(TwoStartCrash, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(TwoEndCrash, CultureInfo.InvariantCulture))
            {
                context.Clients.All.onHitRecorded("red");
                return "Red";
            }
         
            return "";
            
        }

    }

    public class GameHub : Hub
    {
       public GameHub ()
        {

        }
    }
}