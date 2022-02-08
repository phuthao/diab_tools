using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.WardDtos;
using DiaB.Middle.Services.Interfaces;

namespace DiaB.Middle.Services
{
    public class WardService : BaseService<WardEntity>, IWardService
    {
        public WardService(IAppRepo<WardEntity> repo)
            : base(repo)
        {
        }

        public async Task<IPagingData<WardDtos.AppItem>> GetPage(WardDtos.AppFilter input, ActionContext context)
        {
            WardDtos.Filter filter = this.Mapper.Map<WardDtos.Filter>(input);

            var pagingEntities = await this.GetPage<WardDtos.AppItem>(filter, context);

            return this.Mapper.Map<IPagingData<WardDtos.AppItem>>(pagingEntities);
        }

        public async Task<WardDtos.AppItem> GetWardById(Guid id, ActionContext context)
        {
            var entity = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            return this.Mapper.Map<WardDtos.AppItem>(entity);
        }

        protected override IQueryable<WardEntity> FilterQuery(IQueryable<WardEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (!(input is WardDtos.Filter filter))
            {
                return base.FilterQuery(query, input, context);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.Code.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            if (filter.DistrictId != null)
            {
                query = query.Where(x => x.DistrictId == filter.DistrictId);
            }

            if (string.IsNullOrEmpty(input.OrderBy))
            {
                query = query.OrderBy(r => r.Name);
            }

            return base.FilterQuery(query, input, context);
        }
    }
}
