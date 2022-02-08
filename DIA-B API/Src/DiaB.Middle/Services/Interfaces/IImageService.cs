using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.ImageDtos;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IImageService : ICoreService
    {
        Task UploadImage(ImageDtos.AppInsert input, ActionContext context);

        Task<ImageDtos.AppUrl> UploadSingleImage(ImageDtos.AppInsert input, ActionContext context);

        IList<ImageDtos.AppUrl> GetImageUrls(IList<Guid> imageIds, ActionContext context);

        ImageDtos.AppUrl GetImageUrl(Guid imageId, ActionContext context);

        Task DeleteImages(IList<Guid> imageIds, ActionContext context);

        Task<ImageDtos.AppItem> GetImageById(Guid imageId, ActionContext context, bool includeStream = false);

        Task<Guid> UploadSingleImageAndGetId(ImageDtos.AppInsert input, ActionContext context);
    }
}
