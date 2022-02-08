using DiaB.Core.Web.Attributes;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace DiaB.IdentityServer.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(LocalApi.PolicyName, Roles = Role.Admin)]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;

        public AdminController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }

        [Transactional(typeof(PersistedGrantDbContext))]
        [HttpDelete("v1/users/{subjectId}/block")]
        public async Task<IActionResult> BlockUser([FromRoute] string subjectId)
        {
            return Ok(await _accountService.BlockUser(subjectId));
        }

        [HttpPut("v1/users/{subjectId}/unblock")]
        public async Task<IActionResult> UnblockUser([FromRoute] string subjectId)
        {
            return Ok(await _accountService.UnblockUser(subjectId));
        }

        [HttpPost("v1/users")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _accountService.CreateUser(user);

            return Ok();
        }

        [HttpPut("v1/users/{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UserDto user)
        {
            var result = await _accountService.UpdateUser(userId, user);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPut("v1/users/{userId}/password")]
        public async Task<IActionResult> ChangePassword([FromRoute] string userId, [FromBody] string password)
        {
            var result = await _accountService.ChangePassword(userId, password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPost("v1/users/{userId}/reset-password")]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            var result = await _accountService.ResetPassword(userId);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("v1/user-status")]
        public async Task<IActionResult> GetUserStatus(UserStatusRequest request)
        {
            var statusList = await _accountService.GetUserStatus(request.Ids);

            return Ok(statusList);
        }

        [HttpPost("v1/test-email")]
        public async Task<IActionResult> TestEmail(string sendTo)
        {
            await _emailService.Send(sendTo, "Test mail", "Test mail");

            return Ok();
        }
    }
}
