using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class Hexstring
    {
        public Hexstring() { }

        public string Hexstrings(string hex)
        {
            string strvalue = "";
            string[] strsplit = hex.Split(' ');
            foreach (String hexa in strsplit)
            {
                strvalue = strvalue + char.ConvertFromUtf32(Convert.ToInt32(hexa, 16));
            }
            return strvalue;
        }
    }
}