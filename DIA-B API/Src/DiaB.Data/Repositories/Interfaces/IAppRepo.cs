using CpTech.Core.Data.Entities;
using CpTech.Core.Data.Repositories;

namespace DiaB.Data.Repositories.Interfaces
{
    public interface IAppRepo<TEntity> : IModelRepo<TEntity>
        where TEntity : IIdentifiableEntity, ISoftDeletableEntity, IHasCodeEntity
    {
    }
}
