using AdminPanelDevice.Models;
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
          
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(OneStartError, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(OneEndError, CultureInfo.InvariantCulture))
            {
                return "Red";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(OneStartCrash, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <Convert.ToDouble(OneEndCrash, CultureInfo.InvariantCulture))
            {
                return "Yellow";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(StartCorrect , CultureInfo.InvariantCulture) && Convert.ToDouble(value , CultureInfo.InvariantCulture) <= Convert.ToDouble(EndCorrect, CultureInfo.InvariantCulture))
            {
                return "Green";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >Convert.ToDouble(TwoStartError, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(TwoEndError, CultureInfo.InvariantCulture))
            {
                return "Yellow";
            }
            if (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= Convert.ToDouble(TwoStartCrash, CultureInfo.InvariantCulture) && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= Convert.ToDouble(TwoEndCrash, CultureInfo.InvariantCulture))
            {
                return "Red";
            }

            return "";
        }

    }
}