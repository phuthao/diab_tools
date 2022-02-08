using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using DiaB.Common.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace DiaB.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable(ConfigConstant.ASPNETCOREENV);
            var basePath = Environment.CurrentDirectory;

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(basePath)
                .AddJsonFile(Path.Combine(basePath, ConfigConstant.CommonConfigsRelativePath, ConfigConstant.DefaultAppSetting), optional: true)
                .AddJsonFile(Path.Combine(basePath, ConfigConstant.CommonConfigsRelativePath, string.Format(ConfigConstant.AppSettingByEnvironment, envName)), optional: true)
                .AddJsonFile(Path.Combine(ConfigConstant.Configs, ConfigConstant.DefaultAppSetting), optional: true)
                .AddJsonFile(Path.Combine(ConfigConstant.Configs, string.Format(ConfigConstant.AppSettingByEnvironment, envName)), optional: true)
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    logging.AddNLog();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((host, builder) =>
                {
                    builder.AddConfiguration(config);
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                });
        }
    }
}
