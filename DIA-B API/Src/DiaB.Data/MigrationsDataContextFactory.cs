using System;
using System.IO;
using DiaB.Common.Constants;
using DiaB.Data.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DiaB.Data
{
    public class MigrationsDataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable(ConfigConstant.ASPNETCOREENV);

            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstant.CommonConfigsRelativePath, ConfigConstant.DefaultAppSetting))
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstant.CommonConfigsRelativePath, string.Format(ConfigConstant.AppSettingByEnvironment, env)), optional: true)
                .Build();

            return new DataContext(config, null);
        }
    }
}
