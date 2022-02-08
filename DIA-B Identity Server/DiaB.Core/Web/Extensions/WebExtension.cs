using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace DiaB.Core.Web.Extensions
{
    public static class WebExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                return new Guid(principal.FindFirstValue(JwtClaimTypes.Subject));
            }

            throw new UnauthorizedAccessException();
        }

        public static Guid GetUserId(this HttpContext context)
        {
            return context.User.GetUserId();
        }
    }
}
