using System;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.NationDtos;

namespace DiaB.Middle.Services.Interfaces
{
    public interface INationService : ICoreService
    {
        Task<IPagingData<NationDtos.AppItem>> GetPage(NationDtos.AppFilter input, ActionContext context);

        Task<NationDtos.AppItem> GetNationById(Guid id, ActionContext context);
    }
}
