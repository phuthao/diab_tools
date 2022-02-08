using AutoMapper;
using CpTech.Core.Middle.Mappers;
using DiaB.Data.Database.Entities.Common;
using DiaB.Middle.Dtos.ConfigurationDtos;

namespace DiaB.Middle.Mappers
{
    public sealed class CommonConfigureMapper : CoreMapper<CommonConfigureEntity, ConfigurationDtos.Item,
        ConfigurationDtos.View, ConfigurationDtos.Insert, ConfigurationDtos.Update>
    {
        public CommonConfigureMapper()
        {
            this.CreateMap<ConfigurationDtos.AppFilter, ConfigurationDtos.Filter>(MemberList.None);

            this.CreateMapFromEntity<ConfigurationDtos.AppItem>()
                .ForMember(dest => dest.Group, opt => opt.Ignore())
                .ForMember(dest => dest.Updater, opt => opt.Ignore());
        }
    }
}
