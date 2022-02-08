using System;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.WardDtos;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IWardService : ICoreService
    {
        Task<IPagingData<WardDtos.AppItem>> GetPage(WardDtos.AppFilter input, ActionContext context);

        Task<WardDtos.AppItem> GetWardById(Guid id, ActionContext context);
    }
}
