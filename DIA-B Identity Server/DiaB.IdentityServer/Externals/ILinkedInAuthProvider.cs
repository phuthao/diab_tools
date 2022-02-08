namespace DiaB.IdentityServer.Externals
{
    public interface ILinkedInAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
