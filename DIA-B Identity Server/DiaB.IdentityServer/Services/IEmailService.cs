using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string html);
    }
}