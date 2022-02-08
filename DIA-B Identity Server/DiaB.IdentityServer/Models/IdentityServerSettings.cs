namespace DiaB.IdentityServer.Models
{
    public class IdentityServerSettings
    {
        public string PfxFilePath { get; set; }

        public string PfxFilePassword { get; set; }

        public string BasicAuthCredential { get; set; }

        public string TokenEndpoint { get; set; }

        public string UserEndpoint { get; set; }

        public string IssuerUri { get; set; }

        public bool EnableTokenCleanup { get; set; }

        public int TokenCleanupInterval { get; set; }

        public LockoutOptions LockoutOptions { get; set; }

        public PasswordOptions PasswordOptions { get; set; }

        public GoogleSettings Google { get; set; }

        public FacebookSettings Facebook { get; set; }

        public AppleSettings Apple { get; set; }
    }
}
