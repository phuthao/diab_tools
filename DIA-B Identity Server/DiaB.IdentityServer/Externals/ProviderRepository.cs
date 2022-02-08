using System.Collections.Generic;

namespace DiaB.IdentityServer.Externals
{
    public class ProviderRepository : IProviderRepository
    {
        public IEnumerable<Provider> Get()
        {
            return ProviderDataSource.GetProviders();
        }
    }
}
