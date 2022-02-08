using System;
using System.Collections.Specialized;
using Autofac;
using Autofac.Extras.Quartz;
using DiaB.WebApi.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DiaB.WebApi.Configurations
{
    public class QuartzConfiguration
    {
        private readonly IScheduler _scheduler;

        public QuartzConfiguration(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Start()
        {
            _scheduler.Start();

            var job = JobBuilder.Create<NotificationJob>()
                .WithIdentity(new JobKey("NotificationJob", "NotificationGroup"))
                .WithDescription("Notification Job Get Schedule Setup")
                .StoreDurably()
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("NotificationTrigger")
                .WithDescription("Notification Job Get Schedule Setup Trigger")
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(1)).RepeatForever())
                .StartNow()
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        internal static void RegisterScheduler(ContainerBuilder builder)
        {
            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => new NameValueCollection
                {
                  { "quartz.threadPool.threadCount", "3" },
                  { "quartz.scheduler.threadName", "MyScheduler" },
                },
            });

            builder.RegisterType<QuartzConfiguration>().AsSelf();
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(NotificationJob).Assembly));
        }

        // Old Config
        public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
        {
            // base configuration from appsettings.json
            services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));

            // base configuration for DI
            services.AddQuartz(q =>
            {
                // handy when part of cluster or you want to otherwise identify multiple schedulers
                q.SchedulerId = "Scheduler-Core";

                // we take this from appsettings.json, just show it's possible
                // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

                // we could leave DI configuration intact and then jobs need to have public no-arg constructor
                // the MS DI is expected to produce transient job instances
                q.UseMicrosoftDependencyInjectionJobFactory(options =>
                {
                    // if we don't have the job in DI, allow fallback to configure via default constructor
                    options.AllowDefaultConstructor = true;
                });

                // or
                // q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });

                // configure jobs with code
                var jobKey = new JobKey("NotificationJob", "NotificationGroup");
                q.AddJob<NotificationJob>(j => j
                    .StoreDurably()
                    .WithIdentity(jobKey)
                    .WithDescription("Notification Job"));

                q.AddTrigger(t => t
                    .WithIdentity("Notification Trigger")
                    .ForJob(jobKey)
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(30)).WithRepeatCount(1))
                    .WithDescription("Notification trigger"));
            });

            // Quartz.Extensions.Hosting hosting
            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
