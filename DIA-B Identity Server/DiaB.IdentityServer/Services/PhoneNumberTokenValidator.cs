using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DiaB.Core.Web.Attributes;
using DiaB.IdentityServer.Database;
using DiaB.IdentityServer.Helpers;

namespace DiaB.IdentityServer.Services
{
    [Transactional(typeof(MainDbContext))]
    public class PhoneNumberTokenValidator : IExtensionGrantValidator
    {
        public string GrantType => PhoneNumberGrant.TokenGrantType;

        private readonly PhoneNumberTokenProvider<User> _phoneNumberTokenProvider;

        private readonly UserManager<User> _userManager;

        public PhoneNumberTokenValidator(
            PhoneNumberTokenProvider<User> phoneNumberTokenProvider,
            UserManager<User> userManager)
        {
            _phoneNumberTokenProvider = phoneNumberTokenProvider;
            _userManager = userManager;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(context.Request.Raw[PhoneNumberGrant.PhoneNumber]);
            var token = context.Request.Raw[PhoneNumberGrant.Token];
            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user != null && await _phoneNumberTokenProvider.ValidateAsync(PhoneNumberGrant.LoginPurpose, token, _userManager, user))
            {
                user.IsActive = true;
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);

                context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.ConfirmationBySms);
            }
        }
    }
}
