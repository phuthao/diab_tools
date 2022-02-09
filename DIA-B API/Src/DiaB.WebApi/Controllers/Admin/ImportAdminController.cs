using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.WebApi.Results;
using DiaB.Middle.Dtos.FileUploadDtos;
using DiaB.Middle.Services.Interfaces;
using DiaB.WebApi.Controllers.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiaB.WebApi.Controllers.Admin
{
    [SwaggerTag("Account Admin")]
    [Route("App/Admin/Import")]
    [ApiExplorerSettings(GroupName = "app")]
    public class ImportAdminController : AppController
    {
        public IImportService _importService { get; set; }

        [HttpPost("staff")]
        [SwaggerResponse(200, null, typeof(DataResult<Guid>))]
        public async Task<IActionResult> CreatePortalAccount([FromForm] FileUploadDtos.AppImportStaff input)
        {
            if (!this.AdminContext.Roles.Contains("Admin"))
            {
                return Forbid();
            }

            var result = _importService.ImportStaff(input, this.AdminContext);

            return new DataResult<FileUploadDtos.StaffInfo>(result);
        }
    }
}
