// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     AuthenticationHelper.cs
// Created Date:  2018/11/05 2:50 PM
// ------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using DiaB.Core.Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace DiaB.Core.Common.Helpers
{
    public static class SecurityHelper
    {
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);

            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var src = Convert.FromBase64String(hashedPassword);
            if (src.Length != 0x31 || src[0] != 0)
            {
                return false;
            }

            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (var bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return StructuralComparisons.StructuralEqualityComparer.Equals(buffer3, buffer4);
        }

        public static string GenerateJwtToken(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static JwtSecurityToken ReadJwtToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        public static string GenerateJwtToken(JwtSettings jwtSettings)
        {
            var header = new JwtHeader(jwtSettings.SigningCredentials);
            var payload = new JwtPayload(jwtSettings.Issuer, jwtSettings.Audience, jwtSettings.Claims, jwtSettings.NotBefore, jwtSettings.Expires, DateTime.UtcNow);

            if (jwtSettings != null)
            {
                foreach (var pair in jwtSettings.Payload)
                {
                    payload[pair.Key] = pair.Value;
                }
            }

            return GenerateJwtToken(new JwtSecurityToken(header, payload));
        }

        public static string Sha256(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                return bytes.ToHashedString();
            }
        }

        public static string Sha512(string text)
        {
            using (var sha512 = SHA512.Create())
            {
                var bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(text));

                return bytes.ToHashedString();
            }
        }

        public static string Md5(string text)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));

                return bytes.ToHashedString();
            }
        }

        public static string GenerateCriptoKey(int keyLength = 32)
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var secretKeyByteArray = new byte[keyLength];

                cryptoProvider.GetBytes(secretKeyByteArray);

                return Convert.ToBase64String(secretKeyByteArray);
            }
        }

        public static SymmetricSecurityKey GenerateSymmetricSecurityKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        private static string ToHashedString(this IEnumerable<byte> bytes)
        {
            var builder = new StringBuilder();

            foreach (var element in bytes)
            {
                builder.Append(element.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
