﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;

namespace DiaB.IdentityServer.Externals
{
    public class TwitterAuthProvider<TUser> : ITwitterAuthProvider where TUser : IdentityUser, new()
    {
        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _httpClient;
        public TwitterAuthProvider(
             IProviderRepository providerRepository,
             HttpClient httpClient
             )
        {
            _providerRepository = providerRepository;
            _httpClient = httpClient;
        }
        public Provider Provider => _providerRepository.Get()
                                    .FirstOrDefault(x => x.Name.ToLower() == ProviderType.Twitter.ToString().ToLower());

        public JObject GetUserInfo(string accessToken)
        {
            if (Provider == null)
            {
                throw new ArgumentNullException(nameof(Provider));
            }

            var request = new Dictionary<string, string>();
            request.Add("tokenString", accessToken);
            request.Add("endpoint", Provider.UserInfoEndPoint);

            var authorizationHeaderParams = QueryBuilder.GetQuery(request, ProviderType.Twitter);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeaderParams);

            var result = _httpClient.GetAsync(Provider.UserInfoEndPoint).Result;

            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }
            return null;
        }
    }
}
