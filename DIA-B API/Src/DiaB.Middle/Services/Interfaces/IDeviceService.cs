using System;
using System.Threading.Tasks;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.DeviceDtos;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IDeviceService : ICoreService
    {
        Task<Guid> RegisterDevice(DeviceDtos.AppInsert input, ActionContext context);

        Task UpdateDevice(DeviceDtos.AppUpdate input, ActionContext context);

        Task DeleteDevice(DeviceDtos.AppUpdate input, ActionContext context);
    }
}
