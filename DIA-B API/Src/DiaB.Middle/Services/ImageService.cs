using CpTech.Core.Middle.Dtos;
using DiaB.Common.Constants;
using DiaB.Common.Extensions;
using DiaB.Common.Helpers;
using DiaB.Data.Database.Entities.Common;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.ImageDtos;
using DiaB.Middle.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services
{
    public class ImageService : BaseService<ImageEntity>, IImageService
    {
        private readonly IConfiguration _configuration;

        public ImageService(IAppRepo<ImageEntity> repo,
                            IConfiguration configuration)
            : base(repo)
        {
            _configuration = configuration;
        }

        public async Task UploadImage(ImageDtos.AppInsert input, ActionContext context)
        {
            if (input.images == null)
            {
                return;
            }

            await this.ValidateImages(input.images);

            var directoryInfo = Directory.CreateDirectory(Path.Combine(CommonConstant.ImagePath, DateTime.UtcNow.ToString("yyyyMMdd")));

            foreach (IFormFile image in input.images)
            {
                if (image.Length <= 0)
                {
                    continue;
                }

                await using var stream = image.OpenReadStream();
                var data = this.Mapper.Map<ImageDtos.Insert>(input);
                data.Title = image.FileName;
                data.Size = image.Length;
                data.PhysicalPath = Path.Combine(CommonConstant.ImagePath, directoryInfo.Name, image.FileName.AppendTimeStamp());
                data.Checksum = stream.GetMd5Hash();

                await using (FileStream fileStream = new FileStream(data.PhysicalPath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                await this.Create(data, context);
            }
        }

        public async Task<Guid> UploadSingleImageAndGetId(ImageDtos.AppInsert input, ActionContext context)
        {
            var result = await SaveImage(input, context);

            return result.Id;
        }

        public async Task<ImageDtos.AppUrl> UploadSingleImage(ImageDtos.AppInsert input, ActionContext context)
        {
            var result = await SaveImage(input, context);

            return this.GetImageUrl(result.Id, context);
        }

        private async Task<ICoreResultDto> SaveImage(ImageDtos.AppInsert input, ActionContext context)
        {
            if (input.images == null)
            {
                throw new ServiceException(string.Empty, 400, "hình ảnh upload không hợp lệ!");
            }

            await this.ValidateImages(input.images);

            var directoryInfo = Directory.CreateDirectory(Path.Combine(CommonConstant.ImagePath, DateTime.UtcNow.ToString("yyyyMMdd")));

            var image = input.images[0];

            if (image.Length <= 0)
            {
                throw new ServiceException(string.Empty, 400, "hình ảnh upload không hợp lệ!");
            }

            await using var stream = image.OpenReadStream();
            var data = this.Mapper.Map<ImageDtos.Insert>(input);
            data.Title = image.FileName;
            data.Size = image.Length;
            data.PhysicalPath = Path.Combine(CommonConstant.ImagePath, directoryInfo.Name, image.FileName.AppendTimeStamp());
            data.Checksum = stream.GetMd5Hash();

            await using (FileStream fileStream = new FileStream(data.PhysicalPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var result = await this.Create(data, context) as ICoreResultDto;

            return result;
        }

        public IList<ImageDtos.AppUrl> GetImageUrls(IList<Guid> imageIds, ActionContext context)
        {
            return imageIds.Select(imageId =>
                new ImageDtos.AppUrl
                {
                    Id = imageId,
                    Url = this.GeneratePreSignedUrl(imageId, context, 300),
                }).ToList();
        }

        public ImageDtos.AppUrl GetImageUrl(Guid imageId, ActionContext context)
        {
            return new ImageDtos.AppUrl
            {
                Id = imageId,
                Url = this.GeneratePreSignedUrl(imageId, context, 300),
            };
        }

        public async Task DeleteImages(IList<Guid> imageIds, ActionContext context)
        {
            if (imageIds == null || !imageIds.Any())
            {
                return;
            }

            await this.Delete(imageIds.ToArray(), context);
        }

        public async Task<ImageDtos.AppItem> GetImageById(Guid imageId, ActionContext context, bool includeStream = false)
        {
            ImageEntity image = await this.GetEntityById(imageId, context);

            if (image == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            ImageDtos.AppItem response = this.Mapper.Map<ImageDtos.AppItem>(image);

            if (includeStream)
            {
                response.Image = new FileStream(image.PhysicalPath, FileMode.Open, FileAccess.Read);
            }

            response.Url = this.GeneratePreSignedUrl(image.Id, context, 300);

            return response;
        }

        public string GeneratePreSignedUrl(Guid imageId, ActionContext context, long expires = 600)
        {
            var queryString = default(QueryString);

            expires = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + expires;

            queryString = queryString.Add(CommonConstant.Expire, expires + string.Empty);
            queryString = queryString.Add(CommonConstant.Signature, this.GeneratePreSignedSignature(imageId + string.Empty, expires + string.Empty));

            if (context.HttpContext != null)
            {
                return UriHelper.BuildAbsolute(
                        context.HttpContext.Request.IsHttps ? context.HttpContext.Request.Scheme : "https",
                        context.HttpContext.Request.Host,
                        context.HttpContext.Request.PathBase,
                        string.Format(CommonConstant.ImageRequestPath, imageId),
                        queryString);
            }

            return this._configuration.GetSection(ConfigConstant.Domain).Value + string.Format(CommonConstant.ImageRequestPath, imageId);
        }

        public string GeneratePreSignedSignature(string imageId, string expires)
        {
            return SecurityHelper.Sha256($"{imageId}{expires}{this.Configuration.GetValue<string>(ConfigConstant.AppSettingFileSecret)}");
        }

        private Task ValidateImages(IList<IFormFile> images)
        {
            if (images.Any(x => x.Length > CommonConstant.TwoMegabytes))
            {
                throw new ServiceException(ServiceExceptions.ImageLengthIsInvalid);
            }

            foreach (IFormFile image in images)
            {
                FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
                provider.TryGetContentType(image.FileName, out var contentType);

                if (!CommonConstant.AllowedFileTypes.Contains(
                    contentType,
                    StringComparer.InvariantCultureIgnoreCase))
                {
                    throw new ServiceException(ServiceExceptions.ImageTypeIsInvalid);
                }
            }

            return Task.CompletedTask;
        }
    }
}
