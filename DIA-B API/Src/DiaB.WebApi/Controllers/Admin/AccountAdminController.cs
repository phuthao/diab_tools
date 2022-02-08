using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.WebApi.Results;
using DiaB.Middle.Dtos.AccountDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiaB.WebApi.Controllers.Admin
{
    [SwaggerTag("Account Admin")]
    [Route("App/Admin/Account")]
    [ApiExplorerSettings(GroupName = "app")]
    public class AccountAdminController : AppController
    {
        public IAccountService _accountService { get; set; }

        [HttpPost("portal")]
        [SwaggerResponse(200, null, typeof(DataResult<Guid>))]
        public async Task<IActionResult> CreatePortalAccount([FromForm] AccountDtos.AppInsertPortal input)
        {
            if (!this.AdminContext.Roles.Contains("Admin"))
            {
                return Forbid();
            }

            var id = await _accountService.CreatePortalAccount(input, this.AdminContext);

            return new DataResult<Guid>(id);
        }

        [HttpPut("portal")]
        public async Task<IActionResult> UpdatePortalAccount([FromForm] AccountDtos.AppUpdatePortal input)
        {
            if (!this.AdminContext.Roles.Contains("Admin"))
            {
                return Forbid();
            }

            await _accountService.UpdatePortalAccount(input, this.AdminContext);

            return Ok();
        }

        [HttpGet("portal/{id}")]
        [SwaggerResponse(200, null, typeof(DataResult<AccountDtos.AppItemPortal>))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var account = await _accountService.GetPortalAccountById(id, this.AdminContext);

            return new DataResult<AccountDtos.AppItemPortal>(account);
        }

        [HttpGet("portal")]
        [SwaggerResponse(200, null, typeof(DataResult<IPagingData<AccountDtos.AppItemPortal>>))]
        public async Task<IActionResult> Get(AccountDtos.AppFilter filter)
        {
            var result = await _accountService.GetPortalAccounts(filter, this.AdminContext);

            return new DataResult<IPagingData<AccountDtos.AppItemPortal>>(result);
        }

        [HttpGet("current")]
        [SwaggerResponse(200, null, typeof(DataResult<AccountDtos.AppItem>))]
        public async Task<IActionResult> GetById()
        {
            var result = await _accountService.GetAccountById(this.AdminContext.AccountId, this.AdminContext);

            return new DataResult<AccountDtos.AppItem>(result);
        }

        [HttpPost("current/avatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile image)
        {
            await _accountService.UpdateAvatarAdmin(this.AdminContext.AccountId, image, this.AdminContext);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("testlog")]
        public void TestLog()
        {
            throw new Exception("Test log");
        }

        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (!this.AdminContext.Roles.Contains("Admin"))
            {
                return Forbid();
            }

            await _accountService.ResetPassword(id, AdminContext);

            return Ok();
        }
    }
}
