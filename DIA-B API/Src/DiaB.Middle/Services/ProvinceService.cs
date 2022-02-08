using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.ProvinceDtos;
using DiaB.Middle.Services.Interfaces;

namespace DiaB.Middle.Services
{
    public class ProvinceService : BaseService<ProvinceEntity>, IProvinceService
    {
        public ProvinceService(IAppRepo<ProvinceEntity> repo)
            : base(repo)
        {
        }

        public async Task<IPagingData<ProvinceDtos.AppItem>> GetPage(ProvinceDtos.AppFilter input, ActionContext context)
        {
            ProvinceDtos.Filter filter = this.Mapper.Map<ProvinceDtos.Filter>(input);

            var pagingEntities = await this.GetPage<ProvinceDtos.AppItem>(filter, context);

            return this.Mapper.Map<IPagingData<ProvinceDtos.AppItem>>(pagingEntities);
        }

        public async Task<ProvinceDtos.AppItem> GetProvinceById(Guid id, ActionContext context)
        {
            var entity = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            return this.Mapper.Map<ProvinceDtos.AppItem>(entity);
        }

        protected override IQueryable<ProvinceEntity> FilterQuery(IQueryable<ProvinceEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (!(input is ProvinceDtos.Filter filter))
            {
                return base.FilterQuery(query, input, context);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.Code.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            if (filter.NationId != null)
            {
                query = query.Where(x => x.NationId == filter.NationId);
            }

            if (string.IsNullOrEmpty(input.OrderBy))
            {
                query = query.OrderBy(r => r.Name);
            }

            return base.FilterQuery(query, input, context);
        }
    }
}
