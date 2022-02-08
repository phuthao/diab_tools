using AutoMapper;
using CpTech.Core.Middle.Mappers;
using DiaB.Data.Database.Entities.Account;
using DiaB.Middle.Dtos.ProvinceDtos;

namespace DiaB.Middle.Mappers
{
    public class ProvinceMapper : CoreMapper<ProvinceEntity>
    {
        public ProvinceMapper()
        {
            this.CreateMapFromEntity<ProvinceDtos.AppItem>();

            this.CreateMap<ProvinceDtos.AppFilter, ProvinceDtos.Filter>(MemberList.Source);
        }
    }
}
