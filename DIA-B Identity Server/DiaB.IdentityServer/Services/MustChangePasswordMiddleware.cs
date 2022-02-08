using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DiaB.IdentityServer.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DiaB.IdentityServer.Services
{
    public class MustChangePasswordMiddleware
    {
        private readonly RequestDelegate _next;

        public MustChangePasswordMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated &&
                context.Request.Path != new PathString("/account/changepassword") &&
                context.Request.Path != new PathString("/account/logout") &&
                ((ClaimsIdentity)context.User.Identity).HasClaim(c => c.Type == Claims.MustChangePassword))
            {
                var returnUrl = context.Request.Path.Value == "/" ? "" : "?returnUrl=" + HttpUtility.UrlEncode(context.Request.Path.Value);

                context.Response.Redirect("/account/changepassword" + returnUrl);
            }

            await _next(context).ConfigureAwait(true);
        }
    }

    public static class MustChangePasswordMiddlewareExtensions
    {
        public static IApplicationBuilder UseMustChangePassword(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MustChangePasswordMiddleware>();
        }
    }
}