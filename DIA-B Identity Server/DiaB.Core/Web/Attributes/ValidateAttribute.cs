// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     ValidateAttribute.cs
// Created Date:  2018/10/10 12:32 PM
// ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using DiaB.Core.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DiaB.Core.Web.Attributes
{
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponseDto<IEnumerable<ModelError>>
                                                            {
                                                                Code = (int)HttpStatusCode.BadRequest + "",
                                                                Message = context.ModelState.Values.SelectMany(v => v.Errors)
                                                            });
            }
        }
    }
}
