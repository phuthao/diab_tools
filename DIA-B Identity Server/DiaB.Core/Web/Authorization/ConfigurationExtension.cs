using DiaB.Core.Web.Authorization.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.Core.Web.Authorization
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddAuthenticator<T, TUser>(this IServiceCollection services) where T : class, IPasswordAuthenticator<TUser> where TUser : User
        {
            services.AddScoped(typeof(IPasswordAuthenticator<TUser>), typeof(T));

            return services;
        }
    }
}
