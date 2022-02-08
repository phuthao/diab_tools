using DiaB.Core.Web.Authorization.Entities;
using System.Threading.Tasks;

namespace DiaB.Core.Web.Authorization
{
    public interface IPasswordAuthenticator<TUser> where TUser : User
    {
        Task<TUser> Authenticate(string username, string password);
    }
}
