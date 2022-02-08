using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Enums;
using DiaB.IdentityServer.Externals;
using DiaB.IdentityServer.Helpers;
using DiaB.IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DiaB.Core.Web.Helpers;
using IdentityModel;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Json;

namespace DiaB.IdentityServer.Services
{
    public class ExternalValidator : IExtensionGrantValidator
    {
        private readonly UserManager<User> _userManager;
        private readonly CustomPhoneNumberTokenProvider _phoneNumberTokenProvider;
        private readonly IProviderRepository _providerRepository;
        private readonly IFacebookAuthProvider _facebookAuthProvider;
        private readonly IGoogleAuthProvider _googleAuthProvider;
        private readonly ITwitterAuthProvider _twitterAuthProvider;
        private readonly ILinkedInAuthProvider _linkedAuthProvider;
        private readonly IGitHubAuthProvider _githubAuthProvider;
        private readonly INonEmailUserProcessor _nonEmailUserProcessor;
        private readonly IEmailUserProcessor _emailUserProcessor;
        private readonly IdentityServerSettings _settings;

        public ExternalValidator(
            UserManager<User> userManager,
            CustomPhoneNumberTokenProvider phoneNumberTokenProvider,
            IProviderRepository providerRepository,
            IFacebookAuthProvider facebookAuthProvider,
            IGoogleAuthProvider googleAuthProvider,
            ITwitterAuthProvider twitterAuthProvider,
            ILinkedInAuthProvider linkeInAuthProvider,
            IGitHubAuthProvider githubAuthProvider,
            INonEmailUserProcessor nonEmailUserProcessor,
            IEmailUserProcessor emailUserProcessor,
            IOptions<IdentityServerSettings> options
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _phoneNumberTokenProvider = phoneNumberTokenProvider ?? throw new ArgumentNullException(nameof(CustomPhoneNumberTokenProvider));
            _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
            _facebookAuthProvider = facebookAuthProvider ?? throw new ArgumentNullException(nameof(facebookAuthProvider));
            _googleAuthProvider = googleAuthProvider ?? throw new ArgumentNullException(nameof(googleAuthProvider));
            _twitterAuthProvider = twitterAuthProvider ?? throw new ArgumentNullException(nameof(twitterAuthProvider));
            _linkedAuthProvider = linkeInAuthProvider ?? throw new ArgumentNullException(nameof(linkeInAuthProvider));
            _githubAuthProvider = githubAuthProvider ?? throw new ArgumentNullException(nameof(githubAuthProvider));
            _nonEmailUserProcessor = nonEmailUserProcessor ?? throw new ArgumentNullException(nameof(nonEmailUserProcessor));
            _emailUserProcessor = emailUserProcessor ?? throw new ArgumentNullException(nameof(nonEmailUserProcessor));
            _settings = options.Value;

            _providers = new Dictionary<ProviderType, IExternalAuthProvider>
            {
                 {ProviderType.Facebook, _facebookAuthProvider},
                 {ProviderType.Google, _googleAuthProvider},
                 {ProviderType.Twitter, _twitterAuthProvider},
                 {ProviderType.LinkedIn, _linkedAuthProvider}
            };
        }

        private Dictionary<ProviderType, IExternalAuthProvider> _providers;

        public string GrantType => ExternalGrant.ExternalGrantType;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            var provider = context.Request.Raw.Get(ExternalGrant.Provider);

            if (string.IsNullOrWhiteSpace(provider))
            {
                context.Result.ErrorDescription = "invalid_provider";

                return;
            }

            if (provider.ToLower() == "apple")
            {
                await ValidateAppleAsync(context);

                return;
            }

            var token = context.Request.Raw.Get(ExternalGrant.ExternalToken);

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result.ErrorDescription = "invalid_external_token";

                return;
            }

            var providerType = (ProviderType)Enum.Parse(typeof(ProviderType), provider, true);

            if (!Enum.IsDefined(typeof(ProviderType), providerType))
            {
                context.Result.ErrorDescription = "invalid_provider";

                return;
            }

            var userInfo = _providers[providerType].GetUserInfo(token);

            if (userInfo == null)
            {
                context.Result.ErrorDescription = "Couldn't retrieve user info from specified provider, please make sure that access token is not expired.";

                return;
            }

            var externalId = userInfo.Value<string>("id");
            var user = await _userManager.FindByLoginAsync(provider, externalId);

            if (user is null)
            {
                var customResponse = new Dictionary<string, object>();

                customResponse.Add("errorCode", LoginError.UserNotLinkedToPhoneNumber);
                customResponse.Add("message", "User is not linked to a phone number");

                context.Result.CustomResponse = customResponse;

                return;
            }

            if (!user.IsActive)
            {
                var customResponse = new Dictionary<string, object>();

                customResponse.Add("errorCode", LoginError.UserInactive);
                customResponse.Add("message", "User is inactive");

                context.Result.CustomResponse = customResponse;

                return;
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            context.Result = new GrantValidationResult(user.Id, provider, userClaims, provider, null);
        }

        public async Task<bool> ValidateIdToken(string idToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var kid = jwt.Header.Kid;
            var keys = await GetKeys();
            var key = keys.Where(k => k.Kid == kid).FirstOrDefault();

            try
            {
                IDictionary<string, object> claims = Decode(idToken, key.Mondulus, key.Exponent);
            }
            catch (SignatureVerificationException)
            {
                return false;
            }

            return true;
        }

        public async Task ValidateAppleAsync(ExtensionGrantValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            var idToken = context.Request.Raw[ExternalGrant.ExternalToken];

            if (await ValidateIdToken(idToken))
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
                var sub = jwt.Claims.Where(row => row.Type == JwtClaimTypes.Subject || row.Type == ClaimTypes.NameIdentifier).Select(row => row.Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(sub))
                {
                    var user = await _userManager.FindByLoginAsync(AppleGrant.Provider, sub);

                    if (user == null)
                    {
                        var customResponse = new Dictionary<string, object>();

                        customResponse.Add("errorCode", LoginError.UserNotLinkedToPhoneNumber);
                        customResponse.Add("message", "User is not linked to a phone number");

                        context.Result.CustomResponse = customResponse;

                        return;
                    }

                    user = await _userManager.FindByIdAsync(user.Id);

                    context.Result = new GrantValidationResult(user.Id, AppleGrant.Provider, await _userManager.GetClaimsAsync(user), AppleGrant.Provider, null);

                    return;
                }
            }

            return;
        }

        private static IDictionary<string, object> Decode(string token, string modulus, string exponent)
        {
            var urlEncoder = new JwtBase64UrlEncoder();

            var rsaKey = RSA.Create();
            rsaKey.ImportParameters(new RSAParameters() {
                Modulus = urlEncoder.Decode(modulus),
                Exponent = urlEncoder.Decode(exponent)
            });

            var claims = new JwtBuilder()
                .WithAlgorithm(new RS256Algorithm(rsaKey))
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(token);

            return claims;
        }

        private static async Task<IEnumerable<PublicKey>> GetKeys()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://appleid.apple.com/auth/keys");

            var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

            return result.Keys;
        }

        public class ResponseModel
        {
            public IEnumerable<PublicKey> Keys { get; set; }
        }
        public class PublicKey
        {
            [JsonPropertyName("n")]
            public string Mondulus { get; set; }
            [JsonPropertyName("e")]
            public string Exponent  { get; set; }

            public string Kid { get; set; }
        }
    }
}
