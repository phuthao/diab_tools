using System;
using System.IO;
using System.Security.Cryptography;
using DiaB.Common.Utilities;


namespace DiaB.Common.Extensions
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
            using HashAlgorithm md5 = HashAlgorithm.Create(nameof(MD5));
            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLowerInvariant();
        }

        public static string GetMimeType(this string fileName)
        {
            return MimeTypeMap.GetMimeType(Path.GetExtension(fileName));
        }
    }
}
