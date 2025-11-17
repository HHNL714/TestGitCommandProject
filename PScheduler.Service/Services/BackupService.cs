using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PScheduler.Domain.IRepository;
using PScheduler.Domain.IService;
using PScheduler.ExtentionClass;
using PScheduler.Service;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PScheduler.Service.Services
{
    public class BackupService : IHostedService
    {
        public BackupService()
        { 
        
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var scheduler = await GetScheduler();
                var serviceProvider = GetConfiguredServiceProvider();
                scheduler.JobFactory = new CustomJobFactory(serviceProvider);
                await scheduler.Start();
                await ConfigureDailyJob(scheduler);
            }
            catch (Exception ex)
            {
                var errMessage = ex.Message.ToString();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #region common functions

        private ITrigger GetDailyJobTrigger()
        {
            return TriggerBuilder.Create()
                 .WithIdentity("dailytrigger", "dailygroup")
                 .StartNow()
                 .WithSimpleSchedule(x => x
                     .WithIntervalInHours(24)
                     .RepeatForever())
                 .Build();
        }

        private async Task ConfigureDailyJob(IScheduler scheduler)
        {
            var dailyJob = GetScheduleJob();
            if (await scheduler.CheckExists(dailyJob.Key))
            {
                await scheduler.ResumeJob(dailyJob.Key);
            }
            else
            {
                await scheduler.ScheduleJob(dailyJob, GetDailyJobTrigger());
            }
        }

        private IJobDetail GetScheduleJob()
        {
            return JobBuilder.Create<IScheduleJobService>()
                .WithIdentity("dailyjob", "dailygroup")
                .Build();
        }

        private static async Task<IScheduler> GetScheduler()
        {
            // Comment this if you don't want to use database start
            var config = (NameValueCollection)ConfigurationManager.GetSection("quartz");
            var factory = new StdSchedulerFactory(config);
            // Comment this if you don't want to use database end

            // Uncomment this if you want to use RAM instead of database start
            //var props = new NameValueCollection { { "quartz.serializer.type", "binary" } };
            //var factory = new StdSchedulerFactory(props);
            // Uncomment this if you want to use RAM instead of database end
            var scheduler = await factory.GetScheduler();
            return scheduler;
        }


        private IServiceProvider GetConfiguredServiceProvider()
        {
            var services = new ServiceCollection()
                .AddScoped<IScheduleJobService, ScheduleJobService>()
                .AddScoped<IHelperService, HelperService>();
            return services.BuildServiceProvider();
        }

        #endregion


    }
}
