using System;
using System.Threading.Tasks;
using CpTech.Core.WebApi.Filters;
using CpTech.Core.WebApi.Results;
using DiaB.Middle.Dtos.DeviceDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiaB.WebApi.Controllers
{
    [SwaggerTag("thiết bị")]
    [ApiExplorerSettings(GroupName = "app")]
    public class DeviceController : AppController
    {
        public IDeviceService DeviceService { get; set; }

        [HttpPost]
        [SwaggerOperation(Summary = "đăng ký [controller]")]
        [SwaggerResponse(200, null, typeof(DataResult<Guid>))]
        [Transactional]
        public async Task<IActionResult> RegisterDevice([FromBody] DeviceDtos.AppInsert input)
        {
            var result = await this.DeviceService.RegisterDevice(input, this.ActionContext);
            return new DataResult<Guid>(result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "cập nhật [controller]")]
        [SwaggerResponse(200, null, typeof(OkResult))]
        [Transactional]
        public async Task<IActionResult> UpdateDevice([FromBody] DeviceDtos.AppUpdate input)
        {
            await this.DeviceService.UpdateDevice(input, this.ActionContext);
            return this.Ok();
        }

        [HttpDelete("Input/{token}")]
        [SwaggerOperation(Summary = "xóa [controller]")]
        [SwaggerResponse(200, null, typeof(SuccessResult))]
        [Transactional]
        public async Task<IActionResult> DeleteDevice([FromBody] DeviceDtos.AppUpdate input)
        {
            await this.DeviceService.DeleteDevice(input, this.ActionContext);
            return new SuccessResult();
        }
    }
}
