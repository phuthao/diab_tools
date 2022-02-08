using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.ConfigurationDtos;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services.Interfaces
{
    public interface ICommonConfigureTaskService : ICoreService
    {
        Task<List<ConfigurationDtos.AppItem>> GetConfigs(ConfigurationDtos.AppFilter input, ActionContext context);

        Task<ConfigurationDtos.AppItem> GetConfigById(Guid id, ActionContext context);

        Task<ConfigurationDtos.AppItem> GetConfigByKey(string key, ActionContext context);

        Task UpdateConfig(ConfigurationDtos.AppUpdate appUpdate, ActionContext context);
    }
}
