// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseController.cs
// Created Date:  2018/10/25 3:00 PM
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DiaB.Core.Web.Mvc
{
    public class BaseController : Controller
    {
        public string Token => HttpContext.GetTokenAsync("access_token").Result;
    }
}
