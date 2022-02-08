// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  Cactus.Web
// File Name:     SwaggerConfiguration.cs
// Created Date:  2018/10/24 4:50 PM
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiaB.IdentityServer.Configurations
{
    public static class SwaggerConfiguration
    {
        public const string Name = "v1";
        public const string Title = "Cactus NVE Identity Server API";
        public const string Version = "v1";
        public const string Description = "ASP.NET Core 3.1 Web API";
        public const string TermsOfService = "https://www.cactusoftware.com";
        public const string LicenseName = "All Rights Reserved by Cactus";
        public const string LicenseUrl = "https://www.cactusoftware.com";
        public const string ContactName = "vietlp92@gmail.com";
        public const string ContactEmail = "Le Phuoc Viet";
        public const string ContactUrl = "https://www.cactusoftware.com";
        public const string EndpointUrl = "/swagger/v1/swagger.json";
        public const string EndpointName = "Cactus NVE Identity Server v1";

        public static IServiceCollection Configure(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
                                   {
                                       options.SwaggerDoc(Name,
                                                          new OpenApiInfo
                                                          {
                                                              Title = Title,
                                                              Version = Version,
                                                              Description = Description,
                                                              TermsOfService = new Uri(TermsOfService),
                                                              License = new OpenApiLicense
                                                              {
                                                                  Name = LicenseName,
                                                                  Url = new Uri(LicenseUrl)
                                                              },
                                                              Contact = new OpenApiContact
                                                              {
                                                                  Email = ContactEmail,
                                                                  Name = ContactName,
                                                                  Url = new Uri(ContactUrl)
                                                              }
                                                          });
                                       options.OperationFilter<SecurityOperationFilter>();
                                       options.CustomSchemaIds(type => type.FullName);
                                       options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                                       {
                                           Name = HeaderNames.Authorization,
                                           In = ParameterLocation.Header,
                                           Type = SecuritySchemeType.ApiKey
                                       });
                                   });

            return services;
        }
    }

    public class SecurityOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = HeaderNames.Authorization,
                In = ParameterLocation.Header,
                Required = false
            });
        }
    }
}
