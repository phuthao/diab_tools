using DiaB.Core.Common.Helpers;

namespace DiaB.Core.Common.Extensions
{
    public static class SecurityExtension
    {
        public static string Sha256(this string text)
        {
            return SecurityHelper.Sha256(text);
        }

        public static string Sha512(this string text)
        {
            return SecurityHelper.Sha512(text);
        }

        public static string Md5(this string text)
        {
            return SecurityHelper.Md5(text);
        }
    }
}
