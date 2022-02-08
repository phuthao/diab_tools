// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseRevokableJwtSecurityTokenHandler.cs
// Created Date:  2018/11/15 1:08 AM
// ------------------------------------------------------------------------

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DiaB.Core.Web.Authorization
{
    public abstract class BaseRevokableJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {        
        public virtual void ValidateToken(JwtSecurityToken jwtToken)
        {
            throw new NotImplementedException();
        }

        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ValidateToken(ReadJwtToken(token));

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
