using System.Collections.Generic;

namespace DiaB.IdentityServer.Externals
{
    public interface IProviderRepository
    {
        IEnumerable<Provider> Get();
    }
}
