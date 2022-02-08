using DiaB.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public ResourceOwnerPasswordValidator(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            var user = await _userManager.FindByNameAsync(context.UserName) ?? await _userManager.FindByEmailAsync(context.UserName);
            
            if (user != null && user.IsActive)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, context.Password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
                }
            }

            //context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            //var response = await RestHelper.PostAsync<TokenResponseDto>(_settings.TokenEndpoint, new TokenRequestDto
            //{
            //    GrantType = "password",
            //    Username = context.UserName,
            //    Password = context.Password
            //}, new
            //{
            //    Authorization = $"Basic {_settings.BasicAuthCredential}"
            //});

            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    var userResponse = await RestHelper.PostAsync<UserResponseDto>(_settings.UserEndpoint, null, new
            //    {
            //        Cookie = $"access_token={response.Data.AccessToken}"
            //    });

            //    if (userResponse.StatusCode == System.Net.HttpStatusCode.OK)
            //    {
            //        var user = await _userManager.FindByIdAsync(userResponse.Data.Id);

            //        if (user != null)
            //        {
            //            user.Email = userResponse.Data.Email;
            //            user.UserName = userResponse.Data.Username;
            //            //user.Code = userResponse.Data.Code;
            //            //user.Gender = userResponse.Data.Gender;
            //            //user.FullName = userResponse.Data.FullName;
            //        }
            //        else
            //        {
            //            user = new User
            //            {
            //                Id = userResponse.Data.Id,
            //                Email = userResponse.Data.Email,
            //                UserName = userResponse.Data.Username,
            //                //Code = userResponse.Data.Code,
            //                //Gender = userResponse.Data.Gender,
            //                //FullName = userResponse.Data.FullName
            //            };

            //            await _userManager.CreateAsync(user);
            //        }

            //        context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
            //    }
            //}
        }
    }
}
