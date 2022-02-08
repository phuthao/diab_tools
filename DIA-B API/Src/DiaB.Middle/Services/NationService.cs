using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.NationDtos;
using DiaB.Middle.Services.Interfaces;

namespace DiaB.Middle.Services
{
    public class NationService : BaseService<NationEntity>, INationService
    {
        public NationService(IAppRepo<NationEntity> repo)
            : base(repo)
        {
        }

        public async Task<IPagingData<NationDtos.AppItem>> GetPage(NationDtos.AppFilter input, ActionContext context)
        {
            NationDtos.Filter filter = this.Mapper.Map<NationDtos.Filter>(input);

            var pagingEntities = await this.GetPage<NationDtos.AppItem>(filter, context);

            return this.Mapper.Map<IPagingData<NationDtos.AppItem>>(pagingEntities);
        }

        public async Task<NationDtos.AppItem> GetNationById(Guid id, ActionContext context)
        {
            var entity = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            return this.Mapper.Map<NationDtos.AppItem>(entity);
        }

        protected override IQueryable<NationEntity> FilterQuery(IQueryable<NationEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (!(input is NationDtos.Filter filter))
            {
                return base.FilterQuery(query, input, context);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.Code.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            if (string.IsNullOrEmpty(input.OrderBy))
            {
                query = query.OrderBy(r => r.Name);
            }

            return base.FilterQuery(query, input, context);
        }
    }
}
