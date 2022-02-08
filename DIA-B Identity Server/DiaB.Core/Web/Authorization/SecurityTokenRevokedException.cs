// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     SecurityTokenRevokedException.cs
// Created Date:  2018/11/15 6:50 PM
// ------------------------------------------------------------------------

using System;
using Microsoft.IdentityModel.Tokens;

namespace DiaB.Core.Web.Authorization
{
    public class SecurityTokenRevokedException : SecurityTokenException
    {
        public SecurityTokenRevokedException()
        {
        }

        public SecurityTokenRevokedException(string message) : base(message)
        {
        }

        public SecurityTokenRevokedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
