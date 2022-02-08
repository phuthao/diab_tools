using AutoMapper;
using CpTech.Core.Middle.Mappers;
using DiaB.Data.Database.Entities.Account;
using DiaB.Middle.Dtos.NationDtos;

namespace DiaB.Middle.Mappers
{
    public class NationMapper : CoreMapper<NationEntity>
    {
        public NationMapper()
        {
            this.CreateMapFromEntity<NationDtos.AppItem>();

            this.CreateMap<NationDtos.AppFilter, NationDtos.Filter>(MemberList.Source);
        }
    }
}
