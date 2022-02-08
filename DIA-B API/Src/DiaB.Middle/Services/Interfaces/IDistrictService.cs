using System;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.DistrictDtos;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IDistrictService : ICoreService
    {
        Task<IPagingData<DistrictDtos.AppItem>> GetPage(DistrictDtos.AppFilter input, ActionContext context);

        Task<DistrictDtos.AppItem> GetDistrictById(Guid id, ActionContext context);
    }
}
