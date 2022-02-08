using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class ClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return Config.GetClients().FirstOrDefault(row => row.ClientId == clientId);
        }
    }
}
