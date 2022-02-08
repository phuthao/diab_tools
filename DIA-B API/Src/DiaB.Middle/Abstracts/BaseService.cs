using CpTech.Core.Common.Dtos;
using CpTech.Core.Data.Entities;
using CpTech.Core.Data.Repositories;
using CpTech.Core.Middle.Dtos;
using CpTech.Core.Middle.Services;
using System.Threading.Tasks;

namespace DiaB.Middle.Abstracts
{
    public abstract class BaseService<TEntity> : CoreService<TEntity>
        where TEntity : class, IIdentifiableEntity, ITraceableEntity, ISoftDeletableEntity, IHasCodeEntity
    {
        private readonly IModelRepo<TEntity> _repo;

        protected BaseService(IModelRepo<TEntity> repo)
            : base(repo)
        {
            _repo = repo;
        }

        protected override async Task<IResultDto> CreateEntity(TEntity entity, ICoreInsertDto input, IActionContext context)
        {
            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                entity.Code = await GenerateCode(entity, context);
            }

            var identity = context as DiaB.Common.Utilities.ActionContext;
            if (identity != null)
            {
                entity.CreatorId = identity.AccountId;
                entity.UpdaterId = identity.AccountId;
            }

            _repo.Create(entity);

            await _repo.SaveChanges();

            return new CoreResultDto<TEntity> { Id = entity.Id, Entity = entity };
        }

        protected override async Task<IResultDto> UpdateEntity(TEntity entity, TEntity preUpdateEntity, ICoreUpdateDto input, IActionContext context)
        {
            var identity = context as DiaB.Common.Utilities.ActionContext;
            if (identity != null)
            {
                if (entity.CreatorId == null) entity.CreatorId = identity.AccountId;
                entity.UpdaterId = identity.AccountId;
            }

            _repo.Update(entity);

            await _repo.SaveChanges();

            return new CoreResultDto<TEntity> { Id = entity.Id, Entity = entity };
        }
    }
}
