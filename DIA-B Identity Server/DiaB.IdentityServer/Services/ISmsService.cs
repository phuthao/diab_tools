using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public interface ISmsService
    {
        Task SendMessage(string phoneNumber, string token);
    }
}
