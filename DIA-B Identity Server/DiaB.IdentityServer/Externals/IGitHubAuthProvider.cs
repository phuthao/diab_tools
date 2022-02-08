namespace DiaB.IdentityServer.Externals
{
    public interface IGitHubAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
