using Autofac;
using CpTech.Core.Common.AutofacModules;
using CpTech.Core.Common.Extensions;
using Microsoft.Extensions.Configuration;

namespace DiaB.Common
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

            builder.RegisterEventPublisher();
        }
    }
}
