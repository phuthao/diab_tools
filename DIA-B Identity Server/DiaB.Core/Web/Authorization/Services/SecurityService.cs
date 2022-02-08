// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     SecurityService.cs
// Created Date:  2018/11/16 3:25 PM
// ------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using LazyCache;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace DiaB.Core.Web.Authorization.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IAntiforgery _antiforgery;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAppCache _cache;

        public SecurityService(
            IAntiforgery antiforgery,
            IHttpContextAccessor httpContextAccessor,
            IAppCache cache)
        {
            _antiforgery = antiforgery;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        //[Obsolete]
        //public async Task<T> Authenticate<T>(string username, string password, params string[] columns) where T : class
        //{
        //    var query = $"SELECT TOP 1 * FROM [Users] WHERE {string.Join(" OR ", columns.Select((row, index) => $"[{row}] = " + "{" + index + "}"))}";
        //    var parameters = columns.Select(row => username).ToArray();
        //    var user = (await _dbContext.Database.ExecuteSqlQueryAsync<T>(query, _mapper, parameters)).SingleOrDefault();

        //    if (user != null && SecurityHelper.VerifyHashedPassword(user.GetValue("Password")?.ToString(), password))
        //    {
        //        return user;
        //    }

        //    return null;
        //}

        public async Task GenerateAntiforegyToken(string cookieName)
        {
            var tokens = _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, tokens.RequestToken, new CookieOptions
            {
                HttpOnly = false,
                Path = "/",
                SameSite = SameSiteMode.None
            });
        }

        public async Task<string> RequestClientCredentialsTokenAsync(string authority, string clientId, string clientSecret, string scope)
        {
            const string key = "access_token";

            var token = await _cache.GetAsync<string>(key);

            if (string.IsNullOrEmpty(token))
            {
                using (var httpClient = new HttpClient())
                {
                    var docs = await httpClient.GetDiscoveryDocumentAsync(authority);

                    if (!docs.IsError)
                    {
                        var result = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                        {
                            Address = docs.TokenEndpoint,
                            ClientId = clientId,
                            ClientSecret = clientSecret,
                            Scope = scope
                        });

                        if (!result.IsError)
                        {
                            token = result.AccessToken;
                            _cache.Add(key, token, TimeSpan.FromSeconds(result.ExpiresIn));
                        }
                        else
                        {
                            throw new UnauthorizedAccessException(result.Error);
                        }
                    }
                }
            }

            return token;
        }

        public async Task<string> RequestPasswordTokenAsync(string authority, string clientId, string clientSecret, string username, string password, string scope)
        {
            const string key = "access_token";

            var token = await _cache.GetAsync<string>(key);

            if (string.IsNullOrEmpty(token))
            {
                using (var httpClient = new HttpClient())
                {
                    var docs = await httpClient.GetDiscoveryDocumentAsync(authority);

                    if (!docs.IsError)
                    {
                        var result = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                        {
                            Address = docs.TokenEndpoint,
                            ClientId = clientId,
                            ClientSecret = clientSecret,
                            UserName = username,
                            Password = password,
                            Scope = scope
                        });

                        if (!result.IsError)
                        {
                            token = result.AccessToken;
                            _cache.Add(key, token, TimeSpan.FromSeconds(result.ExpiresIn));
                        }
                        else
                        {
                            throw new UnauthorizedAccessException(result.Error);
                        }
                    }
                }
            }

            return token;
        }
    }
}
