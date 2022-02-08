using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.DistrictDtos;
using DiaB.Middle.Services.Interfaces;

namespace DiaB.Middle.Services
{
    public class DistrictService : BaseService<DistrictEntity>, IDistrictService
    {
        public DistrictService(IAppRepo<DistrictEntity> repo)
            : base(repo)
        {
        }

        public async Task<IPagingData<DistrictDtos.AppItem>> GetPage(DistrictDtos.AppFilter input, ActionContext context)
        {
            DistrictDtos.Filter filter = this.Mapper.Map<DistrictDtos.Filter>(input);

            var pagingEntities = await this.GetPage<DistrictDtos.AppItem>(filter, context);

            return this.Mapper.Map<IPagingData<DistrictDtos.AppItem>>(pagingEntities);
        }

        public async Task<DistrictDtos.AppItem> GetDistrictById(Guid id, ActionContext context)
        {
            var entity = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            return this.Mapper.Map<DistrictDtos.AppItem>(entity);
        }

        protected override IQueryable<DistrictEntity> FilterQuery(IQueryable<DistrictEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (!(input is DistrictDtos.Filter filter))
            {
                return base.FilterQuery(query, input, context);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.Code.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            if (filter.ProvinceId != null)
            {
                query = query.Where(x => x.ProvinceId == filter.ProvinceId);
            }

            if (string.IsNullOrEmpty(input.OrderBy))
            {
                query = query.OrderBy(r => r.Name);
            }

            return base.FilterQuery(query, input, context);
        }
    }
}
