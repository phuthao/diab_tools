using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.WebApi.Configurations
{
    public static class MvcConfiguration
    {
        public static IServiceCollection ConfigureMvcOptions(this IServiceCollection services)
        {
            services.AddMvc(mvcOpts =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                mvcOpts.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });

            return services;
        }
    }
}
