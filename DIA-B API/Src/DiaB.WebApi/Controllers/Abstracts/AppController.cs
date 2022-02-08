using CpTech.Core.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.WebApi.Controllers.Abstracts
{
    [Route("App/[controller]")]
    [ApiExplorerSettings(GroupName = "app")]
    [Consumes("application/json", "multipart/form-data")]
    public abstract class AppController : BaseController
    {
        protected ActionContext ActionContext => new ActionContext(this.HttpContext);

        protected ActionContext AdminContext => new ActionContext(this.HttpContext, this.HttpContext.Request);
    }
}
