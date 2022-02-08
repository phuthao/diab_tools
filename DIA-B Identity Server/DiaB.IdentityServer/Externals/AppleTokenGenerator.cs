using DiaB.Core.Common.Helpers;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DiaB.IdentityServer.Externals
{
    public static class AppleTokenGenerator
    {
        public static string CreateNewToken()
        {
            var settings = IoCHelper.GetInstance<IOptions<IdentityServerSettings>>().Value;

            var ecdsa = ECDsa.Create();
            ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(settings.Apple.AuthKey), out _);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(
                issuer: settings.Apple.AppId,
                audience: AppleGrant.Issuer,
                subject: new ClaimsIdentity(new List<Claim> { new Claim("sub", settings.Apple.ClientId) }),
                expires: DateTime.UtcNow.AddMinutes(5), // expiry can be a maximum of 6 months
                issuedAt: DateTime.UtcNow,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256));

            return handler.WriteToken(token);
        }
    }
}
