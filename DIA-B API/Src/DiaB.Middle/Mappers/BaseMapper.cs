using CpTech.Core.Common.Dtos;

namespace DiaB.Middle.Mappers
{
    public class BaseMapper : CpTech.Core.Middle.Mappers.BaseMapper
    {
        public BaseMapper()
        {
            this.CreateMap(typeof(IPagingData<>), typeof(IPagingData<>));
        }
    }
}
