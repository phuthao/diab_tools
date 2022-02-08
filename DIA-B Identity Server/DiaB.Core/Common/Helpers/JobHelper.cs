// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     JobHelper.cs
// Created Date:  2018/11/05 3:08 PM
// ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace DiaB.Core.Common.Helpers
{
    public static class JobHelper
    {
        public static async void CreateSimpleJob<T>(string name, string group, int interval, IDictionary<string, object> data = null) where T : IJob
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            if (await scheduler.CheckExists(new JobKey(name, group)))
            {
                DeleteJob(name, group);
            }

            await scheduler.Start();

            var jobBuilder = JobBuilder.Create<T>().WithIdentity(name, group);
            if (data != null)
            {
                jobBuilder = jobBuilder.SetJobData(new JobDataMap(data));
            }

            var job = jobBuilder.Build();
            var trigger = TriggerBuilder.Create()
                                        .WithIdentity(name, group)
                                        .ForJob(job)
                                        .StartNow()
                                        .WithSimpleSchedule(x => x
                                                                 .WithIntervalInSeconds(interval)
                                                                 .RepeatForever())
                                        .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public static async void DeleteJob(string name, string group)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.UnscheduleJob(new TriggerKey(name, group));
            await scheduler.DeleteJob(new JobKey(name, group));
        }

        public static async void Shutdown(string name, string group)
        {
            DeleteJob(name, group);
            await (await StdSchedulerFactory.GetDefaultScheduler()).Shutdown();
        }

        public static async void Pause(string name, string group)
        {
            await (await StdSchedulerFactory.GetDefaultScheduler()).PauseJob(new JobKey(name, group));
        }

        public static async void Resume(string name, string group)
        {
            await (await StdSchedulerFactory.GetDefaultScheduler()).ResumeJob(new JobKey(name, group));
        }

        public static async Task<bool> CheckExists(string name, string group)
        {
            return await (await StdSchedulerFactory.GetDefaultScheduler()).CheckExists(new JobKey(name, group));
        }
    }
}
