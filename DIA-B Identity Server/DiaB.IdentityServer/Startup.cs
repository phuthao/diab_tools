using System;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DiaB.Core.Web.Attributes;
using DiaB.Core.Web.Extensions;
using DiaB.IdentityServer.Configurations;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Database;
using DiaB.IdentityServer.Externals;
using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace DiaB.IdentityServer
{
    public class Startup
    {
        public const string IdentityServerSection = "IdentityServer";

        public const string CorsPolicy = "CorsPolicy";

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ExceptionAttribute>();
            }).AddNewtonsoftJson();

            services.AddSameSiteCookiePolicy();

            var settings = services.Bind<IdentityServerSettings>(Configuration.GetSection(IdentityServerSection));

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<MainDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<DiabDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("DiabConnection")));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.Lockout = new Microsoft.AspNetCore.Identity.LockoutOptions
                        {
                            AllowedForNewUsers = settings.LockoutOptions.AllowedForNewUsers,
                            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(settings.LockoutOptions.DefaultLockoutTimeSpan),
                            MaxFailedAccessAttempts = settings.LockoutOptions.MaxFailedAccessAttempts
                        };
                        options.Password = new Microsoft.AspNetCore.Identity.PasswordOptions
                        {
                            RequiredLength = settings.PasswordOptions.RequiredLength,
                            RequireDigit = settings.PasswordOptions.RequireDigit,
                            RequiredUniqueChars = settings.PasswordOptions.RequiredUniqueChars,
                            RequireLowercase = settings.PasswordOptions.RequireLowercase,
                            RequireNonAlphanumeric = settings.PasswordOptions.RequireNonAlphanumeric,
                            RequireUppercase = settings.PasswordOptions.RequireUppercase
                        };
                    })
                    .AddEntityFrameworkStores<MainDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

            services.AddCors(options => options.AddPolicy(CorsPolicy,
                                                          policyBuilder => policyBuilder
                                                                           .AllowAnyMethod()
                                                                           .AllowAnyHeader()
                                                                           .AllowCredentials()
                                                                           .WithOrigins(Configuration.GetSection("AllowedHosts").Get<string[]>())));

            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;


                    if (!string.IsNullOrEmpty(settings.IssuerUri))
                    {
                        options.IssuerUri = settings.IssuerUri;
                    }

                    //options.Discovery.CustomEntries.Add();
                    options.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                    {
                        LogoutUrl = "/Account/Logout",
                        LoginUrl = "/Account/Login",
                        LoginReturnUrlParameter = "returnUrl"
                    };
                })
                .AddInMemoryCaching()
                //.AddClientStoreCache<IdentityServer4.EntityFramework.Stores.ClientStore>()
                //.AddResourceStoreCache<IdentityServer4.EntityFramework.Stores.ResourceStore>()
                //.AddConfigurationStoreCache()
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryApiResources(Config.GetApis())
                //.AddInMemoryClients(Config.GetClients())
                //.AddClientStore<ClientStore>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptionsBuilder => sqlOptionsBuilder.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptionsBuilder => sqlOptionsBuilder.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = settings.EnableTokenCleanup;
                    options.TokenCleanupInterval = settings.TokenCleanupInterval;
                })
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddExtensionGrantValidator<PhoneNumberPasswordValidator>()
                .AddExtensionGrantValidator<ExternalValidator>()
                .AddExtensionGrantValidator<AppleValidator>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                builder.AddSigningCredential(new X509Certificate2(settings.PfxFilePath, settings.PfxFilePassword));
            }

            //services.AddAuthentication()
            //        .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            //         {
            //             options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //             options.ClientId = settings.Google.ClientId;
            //             options.ClientSecret = settings.Google.ClientSecret;
            //             options.SaveTokens = true;
            //         })
            //        .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
            //        {
            //            options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //            options.AppId = settings.Facebook.AppId;
            //            options.AppSecret = settings.Facebook.AppSecret;
            //            options.SaveTokens = true;
            //        })
            //        .AddOpenIdConnect("Apple", async options =>
            //        {
            //            options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //            options.Configuration = new OpenIdConnectConfiguration
            //            {
            //                AuthorizationEndpoint = AppleGrant.AuthorizationEndpoint,
            //                TokenEndpoint = AppleGrant.TokenEndpoint
            //            };

            //            options.DisableTelemetry = true;

            //            options.Scope.Clear();
            //            options.Scope.Add("name");
            //            options.Scope.Add("email");
            //            options.Scope.Add("openid");

            //            options.ResponseType = "code";

            //            options.CallbackPath = "/signin-apple";

            //            options.ClientId = settings.Apple.ClientId;
            //            options.Events.OnAuthorizationCodeReceived = context =>
            //            {
            //                context.TokenEndpointRequest.ClientSecret = AppleTokenGenerator.CreateNewToken();

            //                return Task.CompletedTask;
            //            };

            //            // Expected identity token iss value
            //            options.TokenValidationParameters.ValidIssuer = AppleGrant.Issuer;

            //            // Expected identity token signing key
            //            var jwks = await new HttpClient().GetStringAsync(AppleGrant.JwksEndpoint);
            //            options.TokenValidationParameters.IssuerSigningKeys = new JsonWebKeySet(jwks).Keys;

            //            // Disable nonce validation (not supported by Apple)
            //            options.ProtocolValidator.RequireNonce = false;

            //            options.SaveTokens = true;
            //        });
            services.AddLocalApiAuthentication();

            // Sample external authentications
            //services.AddAuthentication()
            //        .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            //        {
            //            options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //            options.ClientId = "<insert here>";
            //            options.ClientSecret = "<insert here>";
            //        })
            //        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, "Demo IdentityServer", options =>
            //        {
            //            options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //            options.SignOutScheme = IdentityServer4.IdentityServerConstants.SignoutScheme;
            //            options.SaveTokens = true;

            //            options.Authority = "https://demo.identityserver.io/";
            //            options.ClientId = "native.code";
            //            options.ClientSecret = "secret";
            //            options.ResponseType = "code";

            //            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //            {
            //                NameClaimType = "name",
            //                RoleClaimType = "role"
            //            };
            //        });

            DependencyInjection.Configure(services);
            SwaggerConfiguration.Configure(services);

            //services.Configure<SmsSettings>(Configuration.GetSection(nameof(SmsSettings)));
            services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
        }

        public void Configure(IApplicationBuilder app)
        {
            //_ = Config.InitializeDatabase(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                   .UseSwagger()
                   .UseSwaggerUI(options => { options.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointName); });
            }

            app.UseForwardedHeaders();

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseDefaultFiles();

            // uncomment if you want to support static files
            app.UseStaticFiles();

            // Runs matching. An endpoint is selected and set on the HttpContext if a match is found.
            app.UseRouting();

            app.UseIdentityServer();

            app.UseCors(CorsPolicy);

            app.UseAuthorization();

            app.UseMustChangePassword();

            // uncomment, if you want to add an MVC-based UI
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });

            app.UseServiceLocator();
        }
    }
}
