namespace DiaB.IdentityServer.Constants
{
    public class AppleGrant
    {
        public const string Issuer = "https://appleid.apple.com";

        public const string JwksEndpoint = "https://appleid.apple.com/auth/keys";

        public const string AuthorizationEndpoint = "https://appleid.apple.com/auth/authorize";

        public const string TokenEndpoint = "https://appleid.apple.com/auth/token";

        public const string GrantType = "apple";

        public const string RefreshTokenParam = "refresh_token";

        public const string Provider = "Apple";
    }
}
