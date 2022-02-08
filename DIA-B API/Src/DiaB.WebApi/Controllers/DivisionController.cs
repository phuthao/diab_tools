using System.Threading.Tasks;
using CpTech.Core.WebApi.Results;
using DiaB.Middle.Dtos.DistrictDtos;
using DiaB.Middle.Dtos.NationDtos;
using DiaB.Middle.Dtos.ProvinceDtos;
using DiaB.Middle.Dtos.WardDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiaB.WebApi.Controllers
{
    [SwaggerTag("quốc gia/tỉnh/thành phố/quận/huyện")]
    [ApiExplorerSettings(GroupName = "app")]
    public class DivisionController : AppController
    {
        public INationService NationService { get; set; }

        public IProvinceService ProvinceService { get; set; }

        public IDistrictService DistrictService { get; set; }

        public IWardService WardService { get; set; }

        [HttpGet("Nations")]
        [SwaggerOperation(Summary = "lấy danh sách quốc gia")]
        [SwaggerResponse(200, null, typeof(PagingResult<NationDtos.AppItem>))]
        public async Task<IActionResult> GetNations(NationDtos.AppFilter filter)
        {
            var result = await this.NationService.GetPage(filter, this.ActionContext);
            return new PagingResult<NationDtos.AppItem>(result);
        }

        [HttpGet("Provinces")]
        [SwaggerOperation(Summary = "lấy danh sách tỉnh/thành phố")]
        [SwaggerResponse(200, null, typeof(PagingResult<ProvinceDtos.AppItem>))]
        public async Task<IActionResult> GetProvinces(ProvinceDtos.AppFilter filter)
        {
            var result = await this.ProvinceService.GetPage(filter, this.ActionContext);
            return new PagingResult<ProvinceDtos.AppItem>(result);
        }

        [HttpGet("Dictricts")]
        [SwaggerOperation(Summary = "lấy danh sách quận/huyện")]
        [SwaggerResponse(200, null, typeof(PagingResult<DistrictDtos.AppItem>))]
        public async Task<IActionResult> GetDictricts(DistrictDtos.AppFilter filter)
        {
            var result = await this.DistrictService.GetPage(filter, this.ActionContext);
            return new PagingResult<DistrictDtos.AppItem>(result);
        }

        [HttpGet("Wards")]
        [SwaggerOperation(Summary = "lấy danh sách phường/xã")]
        [SwaggerResponse(200, null, typeof(PagingResult<WardDtos.AppItem>))]
        public async Task<IActionResult> GetWards(WardDtos.AppFilter filter)
        {
            var result = await this.WardService.GetPage(filter, this.ActionContext);
            return new PagingResult<WardDtos.AppItem>(result);
        }
    }
}
