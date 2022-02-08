using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

        private readonly ILogger _logger;

        public ProfileService(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsFactory, ILogger<ProfileService> logger)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _logger = logger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            var login = await _userManager.GetLoginsAsync(user);
            // Add more claims here

            if (user.MustChangePassword)
            {
                claims.Add(new Claim(Claims.MustChangePassword, string.Empty));
            }

            var hasGoogleAccount = login.Count(l => l.ProviderDisplayName.ToLower() == "google") > 0;

            claims.Add(new Claim(Claims.IsLinkedGoogle, hasGoogleAccount.ToString()));
            claims.Add(new Claim(Claims.IsLinkedFacebook, (login.Count(l => l.ProviderDisplayName.ToLower() == "facebook") > 0).ToString()));
            claims.Add(new Claim(Claims.IsLinkedApple, (login.Count(l => l.ProviderDisplayName.ToLower() == "apple") > 0).ToString()));
            claims.Add(new Claim(Claims.IsMobileAccount, user.IsMobileAccount.ToString()));
            claims.Add(new Claim(Claims.FirstLinkedAccount, user.FirstLinkedAccount ?? string.Empty));

            if (hasGoogleAccount)
            {
                claims.Add(new Claim(Claims.GoogleEmail, user.GoogleEmail ?? string.Empty));
            }

            switch (context.Caller)
            {
                case IdentityServerConstants.ProfileDataCallers.ClaimsProviderAccessToken:
                case IdentityServerConstants.ProfileDataCallers.ClaimsProviderIdentityToken:
                case IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint:
                    context.IssuedClaims = claims;
                    break;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);

            context.IsActive = user != null && user.IsActive;
        }
    }
}
