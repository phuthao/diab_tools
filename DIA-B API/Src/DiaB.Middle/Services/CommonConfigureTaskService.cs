using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using DiaB.Common.Constants;
using DiaB.Common.Enums;
using DiaB.Data.Database.Entities.Common;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.ConfigurationDtos;
using DiaB.Middle.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services
{
    public class CommonConfigureTaskService : BaseService<CommonConfigureEntity>, ICommonConfigureTaskService
    {
        public CommonConfigureTaskService(IAppRepo<CommonConfigureEntity> repo)
            : base(repo)
        {
        }

        public IAccountService AccountService { get; set; }

        protected override IQueryable<CommonConfigureEntity> FilterQuery(IQueryable<CommonConfigureEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (input is ConfigurationDtos.Filter filter)
            {
                if (!string.IsNullOrEmpty(filter.Key))
                {
                    query = query.Where(x => x.Key.Contains(filter.Key, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!string.IsNullOrEmpty(filter.Value))
                {
                    query = query.Where(x => x.Value.Contains(filter.Value, StringComparison.CurrentCultureIgnoreCase));
                }

                if (filter.ConfigureSetupType.HasValue)
                {
                    query = query.Where(x => x.ConfigureSetupType == filter.ConfigureSetupType.Value);
                }
            }

            if (string.IsNullOrEmpty(input.OrderBy))
            {
                query = query.OrderBy(r => r.Key);
            }

            return base.FilterQuery(query, input, context);
        }

        public async Task<List<ConfigurationDtos.AppItem>> GetConfigs(ConfigurationDtos.AppFilter input, ActionContext context)
        {
            var filter = this.Mapper.Map<ConfigurationDtos.Filter>(input);

            var entities = await this.GetListEntity(filter, context);

            var result = new List<ConfigurationDtos.AppItem>();

            foreach (var entity in entities)
            {

                if (entity.ConfigureSetupType.HasValue)
                {
                    var type = entity.Key.Split(CommonConstant.Point).Any() ? entity.Key.Split(CommonConstant.Point)[0] : string.Empty;

                    var configureItem = new ConfigurationDtos.AppItem
                    {
                        Id = entity.Id,
                        Key = entity.Key,
                        Value = entity.Value,
                        Name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : entity.Key,
                        ShortDescription = entity.ShortDescription,
                        Description = entity.Description,
                        UpdateDateTime = entity.UpdateDatetime,
                        Updater = entity.UpdaterId.HasValue ? (await this.AccountService.GetAccountById(entity.UpdaterId.Value, context)).FullName : string.Empty,
                    };

                    if (entity.ConfigureSetupType == ConfigureSetupTypes.Content)
                    {
                        switch (type)
                        {

                            default:
                                configureItem.Group = ContentConfigureTypes.Other.ToString();
                                break;
                        }
                    }
                    else if (entity.ConfigureSetupType == Common.Enums.ConfigureSetupTypes.Parameter)
                    {
                        switch (type)
                        {

                            default:
                                configureItem.Group = ParameterConfigureTypes.Other.ToString();
                                break;
                        }
                    }

                    result.Add(configureItem);
                }
            }

            return result;
        }

        public async Task<ConfigurationDtos.AppItem> GetConfigById(Guid id, ActionContext context)
        {
            var config = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            return this.Mapper.Map<ConfigurationDtos.AppItem>(config);
        }

        public async Task<ConfigurationDtos.AppItem> GetConfigByKey(string key, ActionContext context)
        {
            var config = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Key == key), context);

            return this.Mapper.Map<ConfigurationDtos.AppItem>(config);
        }

        public async Task UpdateConfig(ConfigurationDtos.AppUpdate appUpdate, ActionContext context)
        {
            if (string.IsNullOrEmpty(appUpdate.Key))
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            var entity = await this.GetEntity(
            del => del.Where(r => !r.IsDeleted && r.Key == appUpdate.Key),
            context);

            if (entity == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            var result = await this.UpdateEntity(
                entity,
                del =>
                {
                    del.Value = appUpdate.Value;
                    del.Name = appUpdate.Name ?? entity.Name;
                    del.ShortDescription = appUpdate.ShortDescription ?? entity.ShortDescription;
                    del.Description = appUpdate.Description ?? entity.Description;
                },
                context);
        }
    }
}
