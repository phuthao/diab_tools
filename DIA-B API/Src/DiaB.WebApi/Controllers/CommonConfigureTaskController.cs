using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CpTech.Core.WebApi.Filters;
using CpTech.Core.WebApi.Results;
using DiaB.Middle.Dtos.ConfigurationDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiaB.WebApi.Controllers
{
    [SwaggerTag("tham số hệ thống")]
    [ApiExplorerSettings(GroupName = "app")]
    public class CommonConfigureTaskController : AppController
    {
        public ICommonConfigureTaskService CommonConfigureTaskService { get; set; }

        [HttpGet("Parameter")]
        [SwaggerOperation(Summary = "lấy danh sách tham số hệ thống [controller]")]
        [SwaggerResponse(200, null, typeof(DataResult<List<ConfigurationDtos.AppItem>>))]
        public async Task<IActionResult> GetParameter(ConfigurationDtos.AppFilter input)
        {
            input.ConfigureSetupType = Common.Enums.ConfigureSetupTypes.Parameter;
            var result = await this.CommonConfigureTaskService.GetConfigs(input, this.ActionContext);
            return new DataResult<List<ConfigurationDtos.AppItem>>(result);
        }

        [HttpGet("Content")]
        [SwaggerOperation(Summary = "lấy danh sách [controller]")]
        [SwaggerResponse(200, null, typeof(DataResult<List<ConfigurationDtos.AppItem>>))]
        public async Task<IActionResult> GetContent(ConfigurationDtos.AppFilter input)
        {
            input.ConfigureSetupType = Common.Enums.ConfigureSetupTypes.Content;
            var result = await this.CommonConfigureTaskService.GetConfigs(input, this.ActionContext);
            return new DataResult<List<ConfigurationDtos.AppItem>>(result);
        }

        [HttpGet("Key/{key}")]
        [SwaggerOperation(Summary = "lấy chi tiết [controller] by key")]
        [SwaggerResponse(200, null, typeof(DataResult<ConfigurationDtos.AppItem>))]
        public async Task<IActionResult> GetDetailByKey([SwaggerParameter("key [controller]")] string key)
        {
            var result = await this.CommonConfigureTaskService.GetConfigByKey(key, this.ActionContext);
            return new DataResult<ConfigurationDtos.AppItem>(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "lấy chi tiết [controller]")]
        [SwaggerResponse(200, null, typeof(DataResult<ConfigurationDtos.AppItem>))]
        public async Task<IActionResult> GetDetail([SwaggerParameter("mã định danh [controller]")] Guid id)
        {
            var result = await this.CommonConfigureTaskService.GetConfigById(id, this.ActionContext);
            return new DataResult<ConfigurationDtos.AppItem>(result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "cập nhật [controller]")]
        [SwaggerResponse(200, null, typeof(SuccessResult))]
        [Transactional]
        public async Task<IActionResult> UpdateConfig([FromBody] ConfigurationDtos.AppUpdate input)
        {
            await this.CommonConfigureTaskService.UpdateConfig(input, this.ActionContext);
            return new SuccessResult();
        }
    }
}
