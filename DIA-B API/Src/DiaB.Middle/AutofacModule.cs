using System.Reflection;
using Autofac;
using CpTech.Core.Common.AutofacModules;
using CpTech.Core.Common.Extensions;
using DiaB.Middle.Services;
using DiaB.Middle.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DiaB.Middle
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

            builder.RegisterAllMapper(assembly);
            builder.RegisterAllService(assembly);
        }
    }
}
