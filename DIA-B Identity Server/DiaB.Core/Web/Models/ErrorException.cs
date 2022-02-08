// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     ErrorException.cs
// Created Date:  2018/11/21 5:19 PM
// ------------------------------------------------------------------------

using System;
using System.Net;

namespace DiaB.Core.Web.Models
{
    public class ErrorException : Exception
    {
        public string Code { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public ErrorException(string message, string code, HttpStatusCode httpStatusCode) : base(message)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
        }
    }
}
