using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using AdminPanelDevice.Models;
using System.Threading;

namespace AdminPanelDevice.Infrastructure
{
    public class SleepInformation
    {
        public static List<GetSleepThread> getThread = new List<GetSleepThread>();
        public SleepInformation () {}

        public List<GetSleepThread> SleepGetInformation (bool define)
        {
            if (define==true) {
                getThread.Clear();
            GetThread getThreadPreset = new GetThread();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DeviceConnection"].ConnectionString))
            {
                getThread = connection.Query<GetSleepThread>("select * from GetSleepThread").ToList();
            }

            getThread.ForEach(gt => {
                gt.thread = new Thread(() => getThreadPreset.ThreadPreset(gt.IP, gt.ScanInterval, gt.DeviceID, gt.WalkOid, gt.Version, gt.StartCorrect, gt.EndCorrect, gt.OneStartError, gt.OneEndError, gt.OneStartCrash, gt.OneEndCrash, gt.TwoStartError, gt.TwoEndError, gt.TwoStartCrash, gt.TwoEndCrash));
                gt.thread.Start();
            });
            return getThread;
            }
            else
            {
                return getThread;
            }
        }
    }
}