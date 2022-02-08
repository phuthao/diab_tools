using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Autofac;
using CpTech.Core.WebApi;
using DiaB.Common.Constants;
using DiaB.WebApi.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.ReDoc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiaB.WebApi
{
    public class Startup : BaseStartup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
            : base(Assembly.GetExecutingAssembly(), env, configuration)
        {
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterModule(new Common.AutofacModule(this.Configuration));
            builder.RegisterModule(new Data.AutofacModule(this.Configuration));
            builder.RegisterModule(new Middle.AutofacModule(this.Configuration));
            QuartzConfiguration.RegisterScheduler(builder);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services
                .ConfigureMvcOptions()
                .ConfigureAuthentication(this.Configuration);
        }

        public override void ConfigurationDefaultCors(CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }

        public override void ConfigureSwagger(SwaggerGenOptions options)
        {
            base.ConfigureSwagger(options);

            options.SwaggerDoc("app", new OpenApiInfo
            {
                Title = "App API",
                Description = "API dành cho app nhân viên",
                Version = "v1" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            });

            options.AddSecurityDefinition("AccountToken", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "token",
                Description = $"Xác thực bằng access token, <br/> Token được lấy từ {this.Configuration.GetValue<string>("Identity:Host")}",
            });
        }

        public override void ConfigureReDoc(ReDocOptions options)
        {
            base.ConfigureReDoc(options);
            options.IndexStream = () => this.GetType().GetTypeInfo().Assembly
                .GetManifestResourceStream("DiaB.WebApi.ReDoc.html");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.BaseConfigure(app, env);

            if (env.IsEnvironment(ConfigConstant.DevelopmentEnv))
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseSwagger().UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/app/swagger.json", "App API"); });

            app.UseRouting();
            app.UseCors();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            //this.AutofacContainer.Resolve<QuartzConfiguration>().Start();
        }
    }
}
