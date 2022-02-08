using DiaB.Core.Data.Repositories;
using DiaB.Core.Data.Uow;
using DiaB.IdentityServer.Database;
using DiaB.IdentityServer.Models;

namespace DiaB.IdentityServer.Repositories
{
    public class UserRepository : BaseRepository<User, MainDbContext>
    {
        public UserRepository(IUowRepository<MainDbContext> uow) : base(uow)
        {
        }
    }
}
