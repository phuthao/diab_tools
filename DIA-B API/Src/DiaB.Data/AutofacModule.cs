using System.Reflection;
using Autofac;
using CpTech.Core.Common.AutofacModules;
using CpTech.Core.Common.Extensions;
using DiaB.Data.Database;
using DiaB.Data.Repositories;
using DiaB.Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DiaB.Data
{
    public class AutofacModule : BaseModule
    {
        public AutofacModule(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterDbContext(c =>
            {
                var dbLoggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
                return new DataContext(this.Configuration, dbLoggerFactory);
            });

            builder.RegisterAllRepository(assembly);
            builder.RegisterGeneric(typeof(AppRepo<>))
                .As(typeof(IAppRepo<>))
                .InstancePerLifetimeScope();
        }
    }
}
