using DiaB.Core.Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.Core.Web.Extensions
{
	public static class ConfigurationExtension
    {
        public static T Bind<T>(this IServiceCollection services, IConfiguration configuration) where T : class, new()
        {
            var settings = new T();

            services.Configure<T>(configuration);
            configuration.Bind(settings);

            return settings;
        }

        public static IApplicationBuilder UseServiceLocator(this IApplicationBuilder app)
        {
            IoCHelper.SetServiceProvider(app.ApplicationServices);

            return app;
        }
    }
}
