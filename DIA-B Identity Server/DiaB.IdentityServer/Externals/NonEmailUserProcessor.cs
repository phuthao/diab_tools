using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using IdentityServer4.Models;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Enums;

namespace DiaB.IdentityServer.Externals
{
    public class NonEmailUserProcessor : INonEmailUserProcessor
    {
        // private readonly IExternalUserRepository _externalUserRepository;
        private readonly UserManager<User> _userManager;

        public NonEmailUserProcessor(
            //   IExternalUserRepository externalUserRepository,
            UserManager<User> userManager
            )
        {
            //  _externalUserRepository = externalUserRepository ?? throw new ArgumentNullException(nameof(externalUserRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider)
        {
            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
            {
                userEmail = userInfo.Value<string>("emailAddress");
            }

            if (provider.ToLower() == "facebook")
            {
                userEmail = userInfo.Value<string>("id");
            }

            var userExternalId = userInfo.Value<string>("id");

            if (userEmail == null)
            {
                var existingUser = await _userManager.FindByLoginAsync(provider, userExternalId);

                if (existingUser == null)
                {
                    var customResponse = new Dictionary<string, object>
                    {
                        { "userInfo", userInfo }
                    };

                    return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user's email from the given provider, include email paramater and send request again.", customResponse);
                }
                else
                {
                    existingUser = await _userManager.FindByIdAsync(existingUser.Id);

                    if (existingUser == null || !existingUser.IsActive)
                    {
                        var result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                        result.CustomResponse.Add("errorCode", LoginError.UserInactive);

                        return result;
                    }

                    var userClaims = await _userManager.GetClaimsAsync(existingUser);

                    return new GrantValidationResult(existingUser.Id, provider, userClaims, provider, null);
                }
            }
            else
            {
                var newUser = new User { Email = userEmail, UserName = userEmail, IsActive = true };
                var result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Role.User);
                    await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, userExternalId, provider));
                    var userClaims = await _userManager.GetClaimsAsync(newUser);

                    return new GrantValidationResult(newUser.Id, provider, userClaims, provider, null);
                }

                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
            }
        }
    }
}
