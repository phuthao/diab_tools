using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.Core.Web.Extensions
{
    public static class ServiceLocatorExtension
    {
        public static T GetService<T>(this HttpContext httpContext)
        {
            return (T)httpContext.RequestServices.GetRequiredService(typeof(T));
        }

        public static T GetService<T>(this IHttpContextAccessor httpContextAccessor)
        {
            return GetService<T>(httpContextAccessor.HttpContext);
        }

        public static T GetService<T>(this IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
