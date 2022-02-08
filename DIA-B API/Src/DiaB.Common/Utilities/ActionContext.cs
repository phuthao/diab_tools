using System;
using System.Collections.Generic;
using System.Security.Claims;
using DiaB.Common.Constants;
using Google.Apis.Util;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DiaB.Common.Utilities
{
    public class ActionContext : CpTech.Core.Common.Dtos.ActionContext
    {
        public ActionContext(HttpContext httpContext)
            : base(httpContext)
        {
            HttpContext = httpContext;
            this.PhoneNumber = httpContext.User.FindFirstValue(JwtClaimTypes.PhoneNumber);
            this.Email = httpContext.User.FindFirstValue(JwtClaimTypes.Email);
            this.AccountId = Guid.TryParse(httpContext.User.FindFirstValue(JwtClaimTypes.Subject), out Guid accountId) ? accountId : Guid.Empty;
            this.AccessToken = httpContext.Request.Headers[HeaderNames.Authorization];
            this.IsLinkedFacebook = bool.TryParse(httpContext.User.FindFirstValue(ClaimConstant.IsLinkedFacebook), out var temp) ? temp : false;
            this.IsLinkedGoogle = bool.TryParse(httpContext.User.FindFirstValue(ClaimConstant.IsLinkedGoogle), out var temp1) ? temp1 : false;
            this.IsMobileAccount = bool.TryParse(httpContext.User.FindFirstValue(ClaimConstant.IsMobileAccount), out var temp2) ? temp2 : false;
            this.IsLinkedApple = bool.TryParse(httpContext.User.FindFirstValue(ClaimConstant.IsLinkedApple), out var temp3) ? temp1 : false;
            this.FirstLinkedAccount = httpContext.User.FindFirstValue(ClaimConstant.FirstLinkedAccount);
            this.GoogleEmail = httpContext.User.FindFirstValue(ClaimConstant.GoogleEmail);

            InitRoles(httpContext);
        }

        public ActionContext(HttpContext httpContext, HttpRequest request)
            : base(httpContext)
        {
            HttpContext = httpContext;
            this.PhoneNumber = httpContext.User.FindFirstValue(JwtClaimTypes.PhoneNumber);
            this.Email = httpContext.User.FindFirstValue(JwtClaimTypes.Email);
            this.AccountId = Guid.TryParse(httpContext.User.FindFirstValue(JwtClaimTypes.Subject), out Guid accountId) ? accountId : Guid.Empty;
            this.PatientId = Guid.TryParse(request.Query["patientId"], out Guid patientId) ? patientId : Guid.Empty;
            this.AccessToken = httpContext.Request.Headers[HeaderNames.Authorization];

            InitRoles(httpContext);
        }

        private void InitRoles(HttpContext httpContext)
        {
            var roleClaims = httpContext.User.FindAll("role");
            var roles = new List<string>();

            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }

            this.Roles = roles;
        }

        public ActionContext()
           : base()
        {
        }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Guid AccountId { get; set; }

        public Guid PatientId { get; set; }

        public string AccessToken { get; set; }

        public HttpContext HttpContext { get; set; }

        public bool IsLinkedFacebook { get; set; }

        public bool IsLinkedGoogle { get; set; }

        public bool IsLinkedApple { get; set; }

        public bool IsMobileAccount { get; set; }

        public string FirstLinkedAccount { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public string GoogleEmail { get; set; }
    }
}
