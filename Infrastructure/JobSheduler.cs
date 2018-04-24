using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.Threading.Tasks;
namespace AdminPanelDevice.Infrastructure
{
    public class JobSheduler
    {

        private static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
     
       

        public static void Start()
        {

            IJobDetail remove_trap_log = JobBuilder.Create<RemoveLog>()
               .WithIdentity("RemoveLog", "group6")
               .Build();


            //int charge_min = 0, charge_hour = 0;
            //int message_min = 0, message_hour = 0;



            //// invoisi
            if (scheduler.CheckExists(remove_trap_log.Key))
            {
                scheduler.RescheduleJob(new TriggerKey("invoice_send_trigger"), TriggerBuilder.Create().WithIdentity("invoice_send_trigger")
                .WithIdentity("Run Infinitely every 2nd day of the month", "Monthly_Day_2")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute((DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)-10),14,37))
                .Build());
            }
            else
            {
                scheduler.ScheduleJob(remove_trap_log, TriggerBuilder.Create().WithIdentity("invoice_send_trigger")
                 .WithIdentity("Run Infinitely every 2nd day of the month", "Monthly_Day_2")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute((DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)-10),19,04))
                .Build());
            }


            scheduler.Start();
        }

        //public static void RemoveTrigger(string trigger_key)
        //{
        //    ITrigger old_trigger = triggers_set.Where(x => x.Key.Name == trigger_key).FirstOrDefault();
        //    if (old_trigger != null)
        //    {
        //        scheduler.UnscheduleJob(old_trigger.Key);
        //        triggers_set.Remove(old_trigger);
        //    }

        //}

        //public static void refresh()
        //{
        //    scheduler.ScheduleJob(charge_card_job, triggers_set, true);
        //}

        //public static void getScheduleDetails()
        //{
        //    //ISchedulerFactory schedFact = new StdSchedulerFactory();
        //    //foreach (IScheduler scheduler in schedFact.AllSchedulers)
        //    //{
        //    //    var scheduler1 = scheduler;
        //    //    //var JGN = scheduler1.jobna

        //    //    foreach (var jobDetail in from jobGroupName in scheduler1.GetJobGroupNames()
        //    //                              from jobName in scheduler1.GetJobKeys(jobGroupName)
        //    //                              select scheduler1.GetJobDetail(jobName, jobGroupName))
        //    //    {
        //    //        //Get props about job from jobDetail
        //    //    }

        //    //    foreach (var triggerDetail in from triggerGroupName in scheduler1.TriggerGroupNames
        //    //                                  from triggerName in scheduler1.GetTriggerNames(triggerGroupName)
        //    //                                  select scheduler1.GetTrigger(triggerName, triggerGroupName))
        //    //    {
        //    //        //Get props about trigger from triggerDetail
        //    //    }
        //    //}
        //}

    }
}