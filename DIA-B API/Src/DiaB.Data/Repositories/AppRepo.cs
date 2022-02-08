using CpTech.Core.Data.Entities;
using CpTech.Core.Data.Repositories;
using DiaB.Data.Database;
using DiaB.Data.Repositories.Interfaces;

namespace DiaB.Data.Repositories
{
    public class AppRepo<TEntity> : ModelRepo<TEntity>, IAppRepo<TEntity>
        where TEntity : class, IIdentifiableEntity, ITraceableEntity, ISoftDeletableEntity, IHasCodeEntity
    {
        public AppRepo(DataContext context)
            : base(context)
        {
        }
    }
}
