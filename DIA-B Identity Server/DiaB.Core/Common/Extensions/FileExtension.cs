// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     FileExtension.cs
// Created Date:  2019/05/17 1:25 PM
// ------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;
using DiaB.Core.Web.Helpers;

namespace DiaB.Core.Common.Extensions
{
    public static class FileExtension
    {
        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(Path.GetFileNameWithoutExtension(fileName),
                                 "-",
                                 DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
                                 Path.GetExtension(fileName)
                                );
        }

        public static string GetMd5Hash(this Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string GetMimeType(this string fileName)
        {
            return MimeTypeMap.GetMimeType(Path.GetExtension(fileName));
        }

        public static bool IsImage(this string fileName)
        {
            return MimeTypeMap.GetMimeType(Path.GetExtension(fileName)).Contains("image");
        }
    }
}
