using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Enums;
using DiaB.IdentityServer.Helpers;
using DiaB.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class PhoneNumberPasswordValidator : IExtensionGrantValidator
    {
        public string GrantType => PhoneNumberGrant.PasswordGrantType;

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public PhoneNumberPasswordValidator(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(context.Request.Raw[PhoneNumberGrant.PhoneNumber]);
            var password = context.Request.Raw[PhoneNumberGrant.Password];
            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);
            var response = new Dictionary<string, object>();

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            if (user != null && await CheckRole(user))
            {
                if (user.IsActive)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);

                        return;
                    }
                    else
                    {
                        response.Add("errorCode", LoginError.WrongPassword);
                    }
                }
                else
                {
                    if (user.PhoneNumberConfirmed)
                    {
                        response.Add("errorCode", LoginError.UserBanned);
                    }
                    else
                    {
                        response.Add("errorCode", LoginError.UserInactive);
                    }
                }
            }
            else
            {
                response.Add("errorCode", LoginError.WrongUsername);
            }

            context.Result.CustomResponse = response;
        }

        private async Task<bool> CheckRole(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Contains("User");
        }
    }
}
