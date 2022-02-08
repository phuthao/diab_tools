using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DiaB.Common.Helpers
{
    public static class SecurityHelper
    {
        public static string Sha256(string text)
        {
            using HashAlgorithm sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

            return bytes.ToHashedString();
        }

        private static string ToHashedString(this IEnumerable<byte> bytes)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var element in bytes)
            {
                builder.Append(element.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
