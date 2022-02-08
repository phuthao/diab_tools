using AutoMapper;
using CpTech.Core.Middle.Mappers;
using DiaB.Data.Database.Entities.Account;
using DiaB.Middle.Dtos.WardDtos;

namespace DiaB.Middle.Mappers
{
    public class WardMapper : CoreMapper<WardEntity>
    {
        public WardMapper()
        {
            this.CreateMapFromEntity<WardDtos.AppItem>();

            this.CreateMap<WardDtos.AppFilter, WardDtos.Filter>(MemberList.Source);
        }
    }
}
