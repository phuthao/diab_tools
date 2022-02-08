using System;
using System.Security.Claims;
using DiaB.Common.Constants;
using DiaB.Middle.Services.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.WebApi.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value;
                    options.Audience = configuration.GetSection(ConfigConstant.AppSettingIdentityAudience).Value;

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async (context) =>
                        {
                            var serviceProvider = context.HttpContext.RequestServices;
                            var idText = context.Principal.FindFirstValue(JwtClaimTypes.Subject);
                            var id = Guid.TryParse(idText, out var temp) ? temp : Guid.Empty;
                            var account = await serviceProvider.GetRequiredService<IAccountService>().GetAccountById(id);

                            if (account == null)
                            {
                                return;
                            }

                            var claims = context.Principal.Identity as ClaimsIdentity;
                            if (account.Username != null)
                            {
                                claims?.TryRemoveClaim(claims.FindFirst(ClaimConstant.PatientId));
                                claims?.AddClaim(new Claim(ClaimConstant.PatientId, idText));
                            }
                        },
                    };
                });

            return services;
        }
    }
}
