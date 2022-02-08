using DiaB.Core.Web.Extensions;
using DiaB.Core.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DiaB.Core.Web.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<RequestLoggingMiddleware> logger)
        {
            var stopwatch = new Stopwatch();
            var eventTime = DateTime.UtcNow;

            stopwatch.Start();

            context.Request.EnableBuffering();

            await _next(context);

            stopwatch.Stop();

            // Only log fail requests
            if (200 <= context.Response.StatusCode && context.Response.StatusCode <= 299)
            {
                return;
            }

            var requestDetails = new RequestDetailsDto
            {
                RequestName = $"{context.Request.Method} {context.Request.Path}",
                RequestUrl = context.Request.GetDisplayUrl(),
                RequestMethod = context.Request.Method,
                EventTime = eventTime,
                ExecutionTime = stopwatch.ElapsedMilliseconds,
                ResponseCode = context.Response.StatusCode
            };

            using (var reader = new StreamReader(context.Request.Body))
            {
                requestDetails.RequestPayload = (await reader.ReadToEndAsync()).ToObject<object>();
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            logger.LogInformation(requestDetails.ToJson());
        }
    }
}
