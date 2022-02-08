// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     MessageHelper.cs
// Created Date:  2018/10/12 2:59 PM
// ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using DiaB.Core.Web.Models;

namespace DiaB.Core.Web.Helpers
{
    public static class MessageHelper
    {
        public static void ThrowError(KeyValuePair<string, string> error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            throw new ErrorException(error.Value, error.Key, httpStatusCode);
        }

        public static void ThrowError(string errorMessage, string errorCode, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            throw new ErrorException(errorMessage, errorCode, httpStatusCode);
        }
    }
}
