using System;
using System.Threading.Tasks;
using CpTech.Core.WebApi.Filters;
using CpTech.Core.WebApi.Results;
using DiaB.Common.Extensions;
using DiaB.Middle.Dtos.ImageDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiaB.WebApi.Controllers
{
    [SwaggerTag("hình ảnh")]
    [ApiExplorerSettings(GroupName = "app")]
    public class ImageController : AppController
    {
        public IImageService ImageService { get; set; }

        [HttpGet("{imageId}")]
        [SwaggerOperation(Summary = "lấy [controller] theo mã định danh")]
        [SwaggerResponse(200, null, typeof(FileStreamResult))]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadImage(Guid imageId)
        {
            var result = await this.ImageService.GetImageById(imageId, this.ActionContext, true);
            return this.File(result.Image, result.Title.GetMimeType());
        }

        [HttpPost("Upload")]
        [SwaggerOperation(Summary = "upload [controller]")]
        [SwaggerResponse(200, null, typeof(DataResult<ImageDtos.AppUrl>))]
        [Transactional]
        public async Task<IActionResult> UploadImage([FromForm] ImageDtos.AppInsert input)
        {
            var result = await this.ImageService.UploadSingleImage(input, this.ActionContext);
            return new DataResult<ImageDtos.AppUrl>(result);
        }

        [HttpGet("Url/{imageId}")]
        [SwaggerOperation(Summary = "lấy url [controller] theo mã định danh")]
        [SwaggerResponse(200, null, typeof(string))]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageUrl(Guid imageId)
        {
            var result = this.ImageService.GetImageUrl(imageId, this.ActionContext);
            return new DataResult<string>(result.Url);
        }
    }
}
