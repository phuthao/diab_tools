// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     ExceptionAttribute.cs
// Created Date:  2018/10/10 12:32 PM
// ------------------------------------------------------------------------

using System.Net;
using DiaB.Core.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DiaB.Core.Web.Attributes
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ExceptionAttribute(ILogger<ExceptionAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

            if (exception is ErrorException errorException)
            {
                filterContext.Result = new ObjectResult(new ErrorResponseDto<string>
                                                        {
                                                            Code = errorException.Code,
                                                            Message = errorException.Message
                                                        })
                                       {
                                           StatusCode = (int)errorException.HttpStatusCode
                                       };
            }
            else
            {
                filterContext.Result = new ObjectResult(new ErrorResponseDto<string>
                                                        {
                                                            Code = $"{(int)HttpStatusCode.InternalServerError}",
                                                            Message = exception.Message,
                                                            Trace = exception.StackTrace
                                                        })
                                       {
                                           StatusCode = StatusCodes.Status500InternalServerError
                };

                _logger.LogError(exception, exception.Message);
            }
        }
    }
}
