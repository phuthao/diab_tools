using CpTech.Core.WebApi.Results;
using DiaB.Middle.Dtos.AccountDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace DiaB.WebApi.Controllers
{
    [SwaggerTag("tài khoản")]
    [ApiExplorerSettings(GroupName = "app")]
    public class AccountController : AppController
    {
        public IAccountService AccountService { get; set; }

        [HttpGet]
        [SwaggerOperation(Summary = "lấy danh sách [controller]")]
        [SwaggerResponse(200, null, typeof(PagingResult<AccountDtos.AppItem>))]
        public async Task<IActionResult> GetAll(AccountDtos.AppFilter filter)
        {
            var result = await this.AccountService.GetPage(filter, this.ActionContext);
            return new PagingResult<AccountDtos.AppItem>(result);
        }

        [HttpGet("Current")]
        [SwaggerOperation(Summary = "lấy [controller] đang đăng nhập")]
        [SwaggerResponse(200, null, typeof(DataResult<AccountDtos.AppItem>))]
        public async Task<IActionResult> GetCurrent()
        {
            var result = await this.AccountService.GetAccountById(this.ActionContext.AccountId);
            return new DataResult<AccountDtos.AppItem>(result);
        }

        [HttpPost("SyncStatus")]
        [SwaggerOperation(Summary = "sync status account")]
        [SwaggerResponse(200, null, typeof(SuccessResult))]
        public async Task<IActionResult> SyncStatus()
        {
            await this.AccountService.SyncStatus(this.ActionContext);
            return new SuccessResult();
        }

        [HttpPut("current/phone-number")]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberRequestDto request)
        {
            await this.AccountService.ChangePhoneNumber(request, this.ActionContext.AccountId, this.ActionContext);

            return new SuccessResult();
        }
    }
}
