using DiaB.Core.Data.Uow;
using DiaB.IdentityServer.Database;
using DiaB.IdentityServer.Externals;
using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Repositories;
using DiaB.IdentityServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.IdentityServer.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection Configure(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<DbContext, MainDbContext>();
            services.AddDbContext<DbContext, PersistedGrantDbContext>();
            services.AddScoped<IUowRepository<MainDbContext>, UowRepository<MainDbContext>>();
            services.AddScoped<IUowRepository<PersistedGrantDbContext>, UowRepository<PersistedGrantDbContext>>();
            services.AddScoped<PersistedGrantRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDiabSettingService, DiabSettingService>();

            // Exernal grants
            services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor>();
            services.AddScoped<IEmailUserProcessor, EmailUserProcessor<User>>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider<User>>();
            services.AddTransient<ITwitterAuthProvider, TwitterAuthProvider<User>>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider<User>>();
            services.AddTransient<ILinkedInAuthProvider, LinkedInAuthProvider<User>>();
            services.AddTransient<IGitHubAuthProvider, GitHubAuthProvider<User>>();

            services.AddSingleton<CustomPhoneNumberTokenProvider>();

            services.AddHttpClient<IFacebookAuthProvider, FacebookAuthProvider<User>>();
            services.AddHttpClient<ITwitterAuthProvider, TwitterAuthProvider<User>>();
            services.AddHttpClient<IGoogleAuthProvider, GoogleAuthProvider<User>>();
            services.AddHttpClient<ILinkedInAuthProvider, LinkedInAuthProvider<User>>();
            services.AddHttpClient<IGitHubAuthProvider, GitHubAuthProvider<User>>();

            services.AddHttpClient();

            return services;
        }
    }
}
