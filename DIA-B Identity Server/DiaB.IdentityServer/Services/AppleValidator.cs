using DiaB.Core.Web.Helpers;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Externals;
using DiaB.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class AppleValidator : IExtensionGrantValidator
    {
        public string GrantType => AppleGrant.GrantType;

        private readonly UserManager<User> _userManager;

        private readonly IdentityServerSettings _settings;

        public AppleValidator(
            UserManager<User> userManager,
            IOptions<IdentityServerSettings> options)
        {
            _userManager = userManager;
            _settings = options.Value;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", _settings.Apple.ClientId);
            request.AddParameter("client_secret", AppleTokenGenerator.CreateNewToken());
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", context.Request.Raw[AppleGrant.RefreshTokenParam]);

            var response = await RestHelper.SendAsync(AppleGrant.TokenEndpoint, request);

            if (response.IsSuccessful)
            {
                var idToken = JObject.Parse(response.Content).Value<string>("id_token");
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
                var sub = jwt.Claims.Where(row => row.Type == JwtClaimTypes.Subject || row.Type == ClaimTypes.NameIdentifier).Select(row => row.Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(sub))
                {
                    var user = await _userManager.FindByLoginAsync(AppleGrant.Provider, sub);
                    if (user == null)
                    {
                        user = new User { Email = $"{sub}@apple.com", UserName = sub, IsActive = true };
                        var result = await _userManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, Role.User);
                            await _userManager.AddLoginAsync(user, new UserLoginInfo(AppleGrant.Provider, sub, AppleGrant.Provider));

                            context.Result = new GrantValidationResult(user.Id, AppleGrant.Provider, await _userManager.GetClaimsAsync(user), AppleGrant.Provider, null);
                        }
                    }
                    else
                    {
                        user = await _userManager.FindByIdAsync(user.Id);

                        context.Result = new GrantValidationResult(user.Id, AppleGrant.Provider, await _userManager.GetClaimsAsync(user), AppleGrant.Provider, null);
                    }
                }
            }
        }
    }
}
