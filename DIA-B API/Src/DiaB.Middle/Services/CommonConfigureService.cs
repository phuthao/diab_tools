using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Common.Helpers;
using CpTech.Core.Middle.Dtos;
using DiaB.Common.Enums;
using DiaB.Data.Database.Entities.Common;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.ConfigurationDtos;
using DiaB.Middle.Services.Interfaces;

namespace DiaB.Middle.Services
{
    public abstract class CommonConfigureService : BaseService<CommonConfigureEntity>, ICommonConfigureService
    {
        public Dictionary<string, string> ColorConfigs { get; set; }

        protected CommonConfigureService(IAppRepo<CommonConfigureEntity> repo)
            : base(repo)
        {
            this.ColorConfigs = this.GetAllConfigs(new ConfigurationDtos.ColorFilter()).Result;
        }

        protected async Task<Dictionary<string, string>> GetAllConfigs(ConfigurationDtos.BaseFilter filter)
        {
            var configs = await this.GetListEntity(filter, null);

            return configs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        protected override IQueryable<CommonConfigureEntity> FilterQuery(IQueryable<CommonConfigureEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (input is ConfigurationDtos.BaseFilter filter)
            {
                query = query.Where(r => r.Key.Contains(filter.KeyPattern));
            }

            return base.FilterQuery(query, input, context);
        }

        protected bool Compare(double value, string comparer, double comparerValue)
        {
            switch (comparer)
            {
                case var _ when OperatorTypes.GreaterThan.GetDescription() == comparer:
                    return value > comparerValue;

                case var _ when OperatorTypes.LessThan.GetDescription() == comparer:
                    return value < comparerValue;

                case var _ when OperatorTypes.GreaterThanOrEqual.GetDescription() == comparer:
                    return value >= comparerValue;

                case var _ when OperatorTypes.LessThanOrEqual.GetDescription() == comparer:
                    return value <= comparerValue;

                default:
                    throw new ArgumentOutOfRangeException(comparer);
            }
        }
    }
}
