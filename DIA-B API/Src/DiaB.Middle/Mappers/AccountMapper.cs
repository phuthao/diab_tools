using System.Linq;
using AutoMapper;
using CpTech.Core.Common.Helpers;
using CpTech.Core.Middle.Mappers;
using DiaB.Common.Extensions;
using DiaB.Data.Database.Entities.Account;
using DiaB.Middle.Dtos.AccountDtos;

namespace DiaB.Middle.Mappers
{
    public sealed class AccountMapper : CoreMapper<AccountEntity>
    {
        public AccountMapper()
        {
            this.CreateMapFromEntity<AccountDtos.AppItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => StringExtension.GetFullname(src.LastName, src.MiddleName, src.FirstName)))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetAge()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null ? src.Gender.Value.GetDescription() : string.Empty))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.AccountRoles.Any()))
                .ForMember(dest => dest.Nation, opt => opt.MapFrom(src => src.Nation.Name))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province.Name))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District.Name))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward.Name))
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Avatar, opt => opt.Ignore());

            this.CreateMapFromEntity<AccountDtos.AppItemPortal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => StringExtension.GetFullname(src.LastName, src.MiddleName, src.FirstName)))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetAge()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null ? src.Gender.Value.GetDescription() : string.Empty))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Active))
                .ForMember(dest => dest.Nation, opt => opt.MapFrom(src => src.Nation.Name))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province.Name))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District.Name))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward.Name))
                .ForMember(dest => dest.Avatar, opt => opt.Ignore())
                .ForMember(dest => dest.ChangePassword, opt => opt.Ignore())
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.Updater, opt => opt.Ignore());

            this.CreateMap<AccountDtos.AppFilter, AccountDtos.Filter>(MemberList.Source)
                .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => src.SortBy));
        }
    }
}
