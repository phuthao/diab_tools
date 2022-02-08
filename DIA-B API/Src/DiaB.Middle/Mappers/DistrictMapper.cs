using AutoMapper;
using CpTech.Core.Middle.Mappers;
using DiaB.Data.Database.Entities.Account;
using DiaB.Middle.Dtos.DistrictDtos;

namespace DiaB.Middle.Mappers
{
    public class DistrictMapper : CoreMapper<DistrictEntity>
    {
        public DistrictMapper()
        {
            this.CreateMapFromEntity<DistrictDtos.AppItem>();

            this.CreateMap<DistrictDtos.AppFilter, DistrictDtos.Filter>(MemberList.Source);
        }
    }
}
