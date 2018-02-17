using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class CountrieStateCityModel
    {
        public List<Countrie> countrie = new List<Countrie>();
        public List<States> state = new List<States>();
        public List<City> city = new List<City>();
    }
}