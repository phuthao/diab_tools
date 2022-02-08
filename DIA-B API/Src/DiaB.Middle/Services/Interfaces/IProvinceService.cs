using System;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.ProvinceDtos;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IProvinceService : ICoreService
    {
        Task<IPagingData<ProvinceDtos.AppItem>> GetPage(ProvinceDtos.AppFilter input, ActionContext context);

        Task<ProvinceDtos.AppItem> GetProvinceById(Guid id, ActionContext context);
    }
}
