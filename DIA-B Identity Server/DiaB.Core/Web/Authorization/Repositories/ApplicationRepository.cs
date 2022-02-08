using DiaB.Core.Data.Repositories;
using DiaB.Core.Data.Uow;
using DiaB.Core.Web.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiaB.Core.Web.Authorization.Repositories
{
    public class ApplicationRepository : BaseRepository<Application, DbContext>
    {
        public ApplicationRepository(IUowRepository<DbContext> uow) : base(uow)
        {
        }
    }
}
