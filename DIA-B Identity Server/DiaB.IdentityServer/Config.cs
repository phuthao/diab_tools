using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Database;
using DiaB.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static IdentityServer4.IdentityServerConstants;

namespace DiaB.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                {
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.PreferredUserName,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Address,
                        JwtClaimTypes.Picture,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Role
                    }
                },
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource(LocalApi.ScopeName),
                new ApiResource
                {
                    Name = "diabapi",
                    ApiSecrets =
                    {
                        new Secret("E2ZZzPcD8rMjEn5LKOjIqG6ZRYwax4Ptv+KCFByV6n0=".Sha256())
                    },
                    DisplayName = "DiaB APIs",
                    Enabled = true,
                    Scopes =
                    {
                        Scopes.DiaB
                    },
                    UserClaims = new List<string>
                    {
                        JwtClaimTypes.Id,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Email
                    }
                },
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientName = "DiaB Web Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedCorsOrigins = new List<string>
                    {
	                    "http://localhost:8080",
	                    "https://localhost:8080"
                    },
					RedirectUris =
					{
						"http://localhost:8080/callback.html",
						"https://localhost:8080/callback.html"
                    },
                    PostLogoutRedirectUris =
                    {
	                    "http://localhost:8080",
	                    "https://localhost:8080"
                    },
					RequireClientSecret = true,
					RequirePkce = true,
					RequireConsent = false,
                    AccessTokenLifetime = Convert.ToInt32(TimeSpan.FromMinutes(30).TotalSeconds),
                    ClientId = "6718E4F1-0EBC-4607-896D-DB0B27C85F23",
                    ClientSecrets =
                    {
                        new Secret("9eEvPYDR2Q+S32kIx1BV7qh6qUHmyWMwjxcfHkK0sxI=".Sha256())
                    },
                    AllowedScopes =
                    {
	                    StandardScopes.OpenId,
	                    StandardScopes.Profile,
	                    StandardScopes.Email,
	                    StandardScopes.Phone,
	                    StandardScopes.Address,
                        Scopes.DiaB,
                        LocalApi.ScopeName
                    },
                    AccessTokenType = AccessTokenType.Jwt,
					AllowAccessTokensViaBrowser = true,
					AlwaysIncludeUserClaimsInIdToken = true,
					AllowOfflineAccess = false
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
            {
                new ApiScope(Scopes.DiaB, "DiaB Scope"),
                new ApiScope(LocalApi.ScopeName, LocalApi.ScopeName, new string[]
                {
                    JwtClaimTypes.Role
                })
            };
        }

        public static async Task InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mainDbContext = serviceScope.ServiceProvider.GetRequiredService<MainDbContext>();
                var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await mainDbContext.Database.EnsureDeletedAsync();
                await configurationDbContext.Database.EnsureDeletedAsync();
                await persistedGrantDbContext.Database.EnsureDeletedAsync();
                mainDbContext.Database.Migrate();
                configurationDbContext.Database.Migrate();
                persistedGrantDbContext.Database.Migrate();

                // Clients
                foreach (var client in GetClients())
                {
                    var existingClient = configurationDbContext.Clients.SingleOrDefault(row => row.ClientId == client.ClientId);

                    if (existingClient != null)
                    {
                        configurationDbContext.Clients.Remove(existingClient);
                    }

                    configurationDbContext.Clients.Add(client.ToEntity());
                }

                // APIs
                foreach (var api in GetApis())
                {
                    var existingApi = configurationDbContext.ApiResources.SingleOrDefault(row => row.Name == api.Name);

                    if (existingApi != null)
                    {
                        configurationDbContext.ApiResources.Remove(existingApi);
                    }

                    configurationDbContext.ApiResources.Add(api.ToEntity());
                }

                // Identity
                foreach (var identity in GetIdentityResources())
                {
                    var existingIdentity = configurationDbContext.IdentityResources.SingleOrDefault(row => row.Name == identity.Name);

                    if (existingIdentity != null)
                    {
                        configurationDbContext.IdentityResources.Remove(existingIdentity);
                    }

                    configurationDbContext.IdentityResources.Add(identity.ToEntity());
                }

                // Scopes
                foreach (var scope in GetApiScopes())
                {
                    var existingScope = configurationDbContext.ApiScopes.SingleOrDefault(row => row.Name == scope.Name);

                    if (existingScope != null)
                    {
                        configurationDbContext.ApiScopes.Remove(existingScope);
                    }

                    configurationDbContext.ApiScopes.Add(scope.ToEntity());
                }

                 configurationDbContext.SaveChanges();

                // Roles
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
                await roleManager.CreateAsync(new IdentityRole(Role.User));

                // Users
                var user = new User
                {
                    Id = new Guid("cb356d0b-b62b-4418-a295-b2b71393fba6").ToString(),
                    Email = "admin01@gmail.com",
                    UserName = "admin01",
                    PhoneNumber = "000000000",
                    IsActive = true,
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };
                await userManager.CreateAsync(user, "admin01");
                await userManager.AddToRoleAsync(user, Role.Admin);

                //Users
                var user1 = new User
                {
                    Id = new Guid("a8b898c7-0dfd-438f-96e9-b9b9e3e18f37").ToString(),
                    Email = "user01@gmail.com",
                    UserName = "user01",
                    PhoneNumber = "000000000",
                    IsActive = true,
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };
                await userManager.CreateAsync(user1, "user01");
                await userManager.AddToRoleAsync(user1, Role.User);

            }
        }
    }
}

//dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb

//dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
