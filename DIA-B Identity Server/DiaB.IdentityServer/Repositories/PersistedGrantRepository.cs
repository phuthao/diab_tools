using DiaB.Core.Data.Repositories;
using DiaB.Core.Data.Uow;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace DiaB.IdentityServer.Repositories
{
    public class PersistedGrantRepository : BaseRepository<PersistedGrant, PersistedGrantDbContext>
    {
        public PersistedGrantRepository(IUowRepository<PersistedGrantDbContext> uow) : base(uow)
        {
        }
    }
}
