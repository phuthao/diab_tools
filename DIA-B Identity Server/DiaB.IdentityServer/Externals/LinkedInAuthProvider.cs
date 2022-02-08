using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;

namespace DiaB.IdentityServer.Externals
{
    public class LinkedInAuthProvider<TUser> : ILinkedInAuthProvider where TUser : IdentityUser, new()
    {

        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _httpClient;
        public LinkedInAuthProvider(

             IProviderRepository providerRepository,
             HttpClient httpClient
             )
        {

            _providerRepository = providerRepository;
            _httpClient = httpClient;
        }
        public Provider Provider => _providerRepository.Get()
                                    .FirstOrDefault(x => x.Name.ToLower() == ProviderType.LinkedIn.ToString().ToLower());

        public JObject GetUserInfo(string accessToken)
        {
            var urlParams = $"oauth2_access_token={accessToken}&format=json";

            var result = _httpClient.GetAsync($"{Provider.UserInfoEndPoint}{urlParams}").Result;
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }
            return null;
        }
    }
}
