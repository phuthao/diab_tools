// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     AuthorizeAttribute.cs
// Created Date:  2018/10/17 6:36 PM
// ------------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiaB.Core.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }

        public virtual void Authorize(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (!context.Filters.Any(row => row is AllowAnonymousFilter) &&
                    !(context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttributes(true).Any(row => row is AllowAnonymousAttribute) == true)
                {
                    if (context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        Authorize(context);
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
            }
            catch (Exception)
            {
                context.Result = new BadRequestResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
