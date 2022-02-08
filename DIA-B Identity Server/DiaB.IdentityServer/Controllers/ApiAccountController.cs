using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace DiaB.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/Account")]
    public class ApiAccountController : Controller
    {
        private readonly IAccountService _accountService;

        public ApiAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPut("v1/users/current/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var result = await _accountService.ChangePassword(User.GetSubjectId(), request);

            if (result != null && result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("v1/users/current/phone-number")]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberRequestDto request)
        {
            var result = await _accountService.ChangePhoneNumber(User.GetSubjectId(), request);

            if (result != null && result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
    }
}