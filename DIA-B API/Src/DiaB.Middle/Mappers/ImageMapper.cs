using CpTech.Core.Middle.Mappers;
using DiaB.Data.Database.Entities.Common;
using DiaB.Middle.Dtos.ImageDtos;

namespace DiaB.Middle.Mappers
{
    public sealed class ImageMapper : CoreMapper<ImageEntity,
        ImageDtos.Item, ImageDtos.View, ImageDtos.Insert, ImageDtos.Update>
    {
        public ImageMapper()
        {
            this.CreateMap<ImageDtos.AppInsert, ImageDtos.Insert>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.PhysicalPath, opt => opt.Ignore())
                .ForMember(dest => dest.Size, opt => opt.Ignore())
                .ForMember(dest => dest.Checksum, opt => opt.Ignore());

            this.CreateMapFromEntity<ImageDtos.AppItem>()
                .ForMember(dest => dest.Url, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
