namespace DiaB.IdentityServer.Externals
{
    public interface IGoogleAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
