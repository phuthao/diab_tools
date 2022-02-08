namespace DiaB.Common.Constants
{
    public static class CommonConstant
    {
        public const long TwoMegabytes = 2 * 1024 * 1024;

        public const string HtmlContentType = "text/html";

        public const string MonthYearFormat = "MM/yy";

        public const string ImageRequestPath = "/App/Image/{0}";

        public const string ImagePath = @"Files";

        public const string Expire = "expires";

        public const string Signature = "signature";

        public const string Space = " ";

        public const string Point = ".";

        public const string SemiColon = ";";

        public const string Hyphen = "-";

        public const string Underscore = "_";

        public const int TokensLimit = 490;

        public static readonly string[] AllowedFileTypes =
        {
            "image/png",
            "image/jpeg"
        };
    }
}
