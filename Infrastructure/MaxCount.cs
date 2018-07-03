using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Infrastructure
{
    public class MaxCount
    {
        int dvcCount;
        public MaxCount()
        {

        }

        public int maxCountReturn (int maxCount, int deviceCount)
        {
            if (maxCount<=deviceCount)
            {
                dvcCount = deviceCount;
            }
            return dvcCount;
        }
    }
}
