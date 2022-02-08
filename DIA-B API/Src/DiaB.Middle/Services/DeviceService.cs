using System;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Middle.Dtos;
using DiaB.Data.Database.Entities.FireBase;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.DeviceDtos;
using DiaB.Middle.Services.Interfaces;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services
{
    public class DeviceService : BaseService<DeviceEntity>, IDeviceService
    {
        public DeviceService(IAppRepo<DeviceEntity> repo)
            : base(repo)
        {
        }

        public async Task<Guid> RegisterDevice(DeviceDtos.AppInsert input, ActionContext context)
        {
            var result = await this.CreateEntity(
                    new DeviceEntity
                    {
                        AccountId = context.AccountId,
                        DeviceType = input.DeviceType,
                        DeviceInformation = input.DeviceInformation,
                    }, context);

            return (result as ICoreResultDto).Id;
        }

        public async Task UpdateDevice(DeviceDtos.AppUpdate input, ActionContext context)
        {
            var device = await this.GetEntityById(input.Id, context);

            if (device == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            await this.UpdateEntity(
                device,
                entity =>
                {
                    entity.DeviceType = input.DeviceType;
                    entity.DeviceInformation = input.DeviceInformation;
                },
                context);
        }

        public async Task DeleteDevice(DeviceDtos.AppUpdate input, ActionContext context)
        {
            var device = await this.GetEntityById(input.Id, context);

            if (device == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            await this.Delete(device.Id, context);
        }
    }
}
