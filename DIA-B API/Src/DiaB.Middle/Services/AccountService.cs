using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using DiaB.Common.Constants;
using DiaB.Common.Enums;
using DiaB.Common.Extensions;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Repositories.Interfaces;
using DiaB.Middle.Abstracts;
using DiaB.Middle.Dtos.AccountDtos;
using DiaB.Middle.Dtos.ImageDtos;
using DiaB.Middle.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services
{
    public class AccountService : BaseService<AccountEntity>, IAccountService
    {
        public AccountService(IAppRepo<AccountEntity> repo)
            : base(repo)
        {
        }

        public IConfiguration Configuration { get; set; }

        public IImageService ImageService { get; set; }

        protected override IQueryable<AccountEntity> FilterQuery(IQueryable<AccountEntity> query, ICoreFilterDto input, IActionContext context)
        {
            if (!(input is AccountDtos.Filter filter))
            {
                return base.FilterQuery(query, input, context);
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                var textList = filter.Name.Split(' ');

                foreach (var text in textList)
                {
                    query = query.Where(x => x.FirstName.Contains(text, StringComparison.CurrentCultureIgnoreCase) ||
                                             x.MiddleName.Contains(text, StringComparison.CurrentCultureIgnoreCase) ||
                                             x.LastName.Contains(text, StringComparison.CurrentCultureIgnoreCase));
                }
            }

            if (!string.IsNullOrEmpty(filter.Code))
            {
                query = query.Where(x => x.Code.Contains(filter.Code, StringComparison.CurrentCultureIgnoreCase));
            }

            if (filter.AgeFrom.HasValue && filter.AgeTo.HasValue)
            {
                var dateFrom = DateTime.Now.AddYears(-filter.AgeFrom.Value);
                var dateTo = DateTime.Now.AddYears(-filter.AgeTo.Value);

                query = query.Where(x => dateFrom >= x.DateOfBirth && x.DateOfBirth >= dateTo);
            }

            if (filter.Gender.HasValue)
            {
                query = query.Where(x => x.Gender == filter.Gender);
            }

            if (filter.NationId.HasValue)
            {
                query = query.Where(x => x.NationId == filter.NationId);
            }

            if (filter.ProvinceId.HasValue)
            {
                query = query.Where(x => x.ProvinceId == filter.ProvinceId);
            }

            if (filter.DistrictId.HasValue)
            {
                query = query.Where(x => x.DistrictId == filter.DistrictId);
            }

            if (filter.WardId.HasValue)
            {
                query = query.Where(x => x.WardId == filter.WardId);
            }

            if (filter.ExcludeInactive)
            {
                query = query.Where(x => x.Active);
            }

            if (filter.AccountType.HasValue)
            {
                query = query.Where(x => x.AccountType == filter.AccountType);
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));
            }

            return base.FilterQuery(query, input, context);
        }

        public async Task<List<AccountDtos.AppItem>> GetAll(ActionContext context)
        {
            var entities = await this.GetListEntity(del => del.Where(r => !r.IsDeleted), context);

            return this.Mapper.Map<List<AccountDtos.AppItem>>(entities);
        }

        public async Task<IPagingData<AccountDtos.AppItem>> GetPage(AccountDtos.AppFilter input, ActionContext context)
        {
            var filter = this.Mapper.Map<AccountDtos.Filter>(input);

            return await this.GetPage<AccountDtos.AppItem>(filter, context);
        }

        public async Task<AccountDtos.AppItem> GetAccountById(Guid id, ActionContext context = null)
        {
            var account = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            var result = this.Mapper.Map<AccountDtos.AppItem>(account);

            if (result is null)
            {
                return null;
            }

            result.Roles = context?.Roles;

            if (account.CoverId.HasValue && context != null)
            {
                result.Avatar = ImageService.GetImageUrl(account.CoverId.Value, context);
            }

            return result;
        }

        public async Task<AccountDtos.AppItemPortal> GetPortalAccountById(Guid id, ActionContext context)
        {
            var account = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id && r.AccountType == AccountTypes.User), context);
            var result = this.Mapper.Map<AccountDtos.AppItemPortal>(account);

            if (account.CoverId.HasValue)
            {
                result.Avatar = ImageService.GetImageUrl(account.CoverId.Value, context);
            }

            await PopulateStatus(context, account, result);

            result.Creator = await GetEditor(result.CreatorId, context);
            result.Updater = await GetEditor(result.UpdaterId, context);

            return result;
        }

        private async Task<AccountDtos.Editor> GetEditor(Guid? id, ActionContext context)
        {
            if (id is null) return default;

            var entity = await this.GetEntity(del => del.Where(r => !r.IsDeleted && r.Id == id), context);

            if (entity is null) return default;

            var result = new AccountDtos.Editor
            {
                Username = entity.Username,
                FullName = StringExtension.GetFullname(entity.LastName, entity.MiddleName, entity.FirstName),
                Avatar = entity.CoverId.HasValue ? ImageService.GetImageUrl(entity.CoverId.Value, context) : new ImageDtos.AppUrl(),
            };

            return result;
        }

        private async Task PopulateStatus(ActionContext context, AccountEntity account, AccountDtos.AppItemPortal result)
        {
            var statusList = await GetStatus(new List<Guid> { account.Id }, context);

            if (statusList != null && statusList.Count() > 0)
            {
                result.IsActive = statusList.FirstOrDefault().IsActive;
                result.ChangePassword = statusList.FirstOrDefault().ChangePassword;
            }
        }

        public async Task<IPagingData<AccountDtos.AppItemPortal>> GetPortalAccounts(AccountDtos.AppFilter input, ActionContext context)
        {
            var filter = Mapper.Map<AccountDtos.Filter>(input);

            filter.AccountType = AccountTypes.User;

            var pageResult = await this.GetPage<AccountDtos.AppItemPortal>(filter, context);
            var items = pageResult.Items;

            foreach (var item in items)
            {
                item.Avatar = item.CoverId.HasValue ? ImageService.GetImageUrl(item.CoverId.Value, context) : new ImageDtos.AppUrl();
            }

            var ids = items.Select(i => i.Id);

            var statusList = await GetStatus(ids, context);

            foreach (var item in items)
            {
                var userStatus = statusList.FirstOrDefault(s => s.Id == item.Id);

                if (userStatus != null)
                {
                    item.IsActive = userStatus.IsActive;
                }
            }

            return pageResult;
        }

        public async Task<GenderTypes> GetGenderOfCurrentUser(ActionContext context)
        {
            var currentUser = await GetAccountById(context.AccountId);
            var gender = currentUser.Gender == "Nam" ? GenderTypes.Male : GenderTypes.Female;

            return gender;
        }

        public async Task<ICoreResultDto> UpdateAccount(AccountDtos.AppUpdate input, ActionContext context)
        {
            var entity = await this.GetEntity(
                del => del.Where(r => !r.IsDeleted && r.Id == input.Id),
                context);

            if (entity == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            input.PhoneNumber = GetFormatedPhoneNumber(input.PhoneNumber);
            input.SecondPhoneNumber = GetFormatedPhoneNumber(input.SecondPhoneNumber);

            await CheckUpdatedAccount(input, context);

            var result = await this.UpdateEntity(
                entity,
                del =>
                {
                    del.Code = input.Code ?? entity.Code;
                    del.Email = input.Email ?? entity.Email;
                    del.FirstName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetFirstName() : entity.FirstName;
                    del.MiddleName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetMiddleName() : entity.MiddleName;
                    del.LastName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetLastName() : entity.LastName;
                    del.DateOfBirth = input.DateOfBirth ?? entity.DateOfBirth;
                    del.Gender = input.Gender ?? entity.Gender;
                    del.NationId = input.NationId ?? entity.NationId;
                    del.ProvinceId = input.ProvinceId ?? entity.ProvinceId;
                    del.DistrictId = input.DistrictId ?? entity.DistrictId;
                    del.WardId = input.WardId ?? entity.WardId;
                    del.Address = input.Address ?? entity.Address;
                    del.PhoneNumber = input.PhoneNumber ?? entity.PhoneNumber;
                    del.Email = input.Email ?? entity.Email;
                    del.Active = input.Active.HasValue ? input.Active.Value : entity.Active;
                    del.FullNameSearch = input.FullName;
                },
                context) as ICoreResultDto;

            if (input.Active.HasValue)
            {
                using (var client = new HttpClient())
                {
                    var isBase = this.Configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value;
                    var isRoute = string.Format(ConfigConstant.ApiUser, entity.Id, input.Active.Value ? "unblock" : "block");
                    var url = $"{isBase}/{isRoute}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", context.AccessToken.Contains(CommonConstant.Space) ? context.AccessToken.Split(CommonConstant.Space)[1] : context.AccessToken);

                    if (input.Active.Value)
                    {
                        await client.PutAsync(url, null);
                    }
                    else
                    {
                        await client.DeleteAsync(url);
                    }
                }
            }

            return result;
        }

        public async Task ChangePhoneNumber(ChangePhoneNumberRequestDto request, Guid id, ActionContext context)
        {
            var phoneNumber = GetFormatedPhoneNumber(request.PhoneNumber);

            await CheckPhoneNumber(phoneNumber, context, id);

            var entity = await this.GetEntity(
                del => del.Where(r => !r.IsDeleted && r.Id == id),
                context);

            if (entity == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            await SendUpdatePhoneNumberRequest(phoneNumber, context);

            var result = await UpdateEntity(entity,
                e =>
                {
                    e.PhoneNumber = phoneNumber;
                },
                context);
        }

        public async Task UpdateAvatar(AccountDtos.AppUpdate input, ActionContext context)
        {
            var entity = await this.GetEntity(
               del => del.Where(r => !r.IsDeleted && r.Id == input.Id),
               context);

            if (entity == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            var result = await this.UpdateEntity(
                entity,
                del =>
                {
                    del.CoverId = input.CoverId;
                },
                context);
        }

        public async Task UpdatePortalAccount(AccountDtos.AppUpdatePortal input, ActionContext context)
        {
            var entity = await this.GetEntity(
                del => del.Where(r => !r.IsDeleted && r.Id == input.Id),
                context);

            if (entity == null)
            {
                throw new ServiceException(ServiceExceptions.ObjectNotFound);
            }

            FormatUpdatePhoneNumbers(input);
            await CheckUpdatedPortalAccount(input, context);

            Guid? coverId = null;

            if (input.Image != null)
            {
                coverId = await UploadImage(input.Image, context, entity.CoverId);
            }

            await SendCreateRequest(
                new AccountDtos.AppInsertPortal
                {
                    Id = input.Id,
                    Username = input.Username ?? entity.Username,
                    PhoneNumber = input.PhoneNumber ?? entity.PhoneNumber,
                    Email = input.Email ?? entity.Email,
                    ChangePassword = input.ChangePassword.GetValueOrDefault(),
                    Password = "dummy",
                }, context);

            var result = await this.UpdateEntity(
                entity,
                del =>
                {
                    del.Code = input.Code ?? entity.Code;
                    del.Username = !string.IsNullOrEmpty(input.Username) ? input.Username : entity.Username;
                    del.FirstName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetFirstName() : entity.FirstName;
                    del.MiddleName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetMiddleName() : entity.MiddleName;
                    del.LastName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetLastName() : entity.LastName;
                    del.DateOfBirth = input.DateOfBirth ?? entity.DateOfBirth;
                    del.Gender = input.Gender ?? entity.Gender;
                    del.NationId = input.NationId ?? entity.NationId;
                    del.ProvinceId = input.ProvinceId ?? entity.ProvinceId;
                    del.DistrictId = input.DistrictId ?? entity.DistrictId;
                    del.WardId = input.WardId ?? entity.WardId;
                    del.Address = input.Address ?? entity.Address;
                    del.Active = input.Active.HasValue ? input.Active.Value : entity.Active;
                    del.CoverId = coverId ?? del.CoverId;
                    del.Email = input.Email ?? del.Email;
                    del.Username = input.Username ?? del.Username;
                    del.SecondPhoneNumber = input.SecondPhoneNumber ?? del.SecondPhoneNumber;
                    del.PhoneNumber = input.PhoneNumber ?? del.PhoneNumber;
                    del.FullNameSearch = input.FullName ?? del.FullNameSearch;
                },
                context) as ICoreResultDto;

            await UpdateAccountStatus(input, context, entity);
        }

        private async Task UpdateAccountStatus(AccountDtos.AppUpdatePortal input, ActionContext context, AccountEntity entity)
        {
            if (input.Active.HasValue)
            {
                using (var client = new HttpClient())
                {
                    var isBase = this.Configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value;
                    var isRoute = string.Format(ConfigConstant.ApiUser, entity.Id, input.Active.Value ? "unblock" : "block");
                    var url = $"{isBase}/{isRoute}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", context.AccessToken.Contains(CommonConstant.Space) ? context.AccessToken.Split(CommonConstant.Space)[1] : context.AccessToken);

                    if (input.Active.Value)
                    {
                        await client.PutAsync(url, null);
                    }
                    else
                    {
                        await client.DeleteAsync(url);
                    }
                }
            }
        }

        private async Task CheckUpdatedPortalAccount(AccountDtos.AppUpdatePortal input, ActionContext context)
        {
            await CheckUsername(input.Username, context, input.Id);
            await CheckPhoneNumber(input.PhoneNumber, context, input.Id);
            await CheckSecondPhoneNumber(input.SecondPhoneNumber, context, input.Id);
            await CheckEmail(input.Email, context, input.Id);
        }

        private async Task CheckUpdatedAccount(AccountDtos.AppUpdate input, ActionContext context)
        {
            await CheckPhoneNumber(input.PhoneNumber, context, input.Id);
            await CheckSecondPhoneNumber(input.SecondPhoneNumber, context, input.Id);
        }

        public async Task<Guid> CreatePortalAccount(AccountDtos.AppInsertPortal input, ActionContext context)
        {
            FormatPhoneNumbers(input);
            await CheckAccount(input, context);

            input.Id = Guid.NewGuid();

            await SendCreateRequest(input, context);

            Guid? imageId = null;

            if (input.Image != null)
            {
                imageId = await UploadImage(input.Image, context);
            }

            var phoneNumberAccount = await GetEntity(
                del => del.Where(e => e.PhoneNumber == input.PhoneNumber),
                context);

            if (phoneNumberAccount != null)
            {
                var result = await this.UpdateEntity(
                    phoneNumberAccount,
                    del =>
                    {
                        del.Email = input.Email ?? phoneNumberAccount.Email;
                        del.Username = !string.IsNullOrEmpty(input.Username) ? input.Username : phoneNumberAccount.Username;
                        del.FirstName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetFirstName() : phoneNumberAccount.FirstName;
                        del.MiddleName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetMiddleName() : phoneNumberAccount.MiddleName;
                        del.LastName = !string.IsNullOrEmpty(input.FullName) ? input.FullName.GetLastName() : phoneNumberAccount.LastName;
                        del.FullNameSearch = !string.IsNullOrEmpty(input.FullName) ? input.FullName : phoneNumberAccount.FullNameSearch;
                        del.DateOfBirth = input.DateOfBirth;
                        del.Gender = input.Gender ?? phoneNumberAccount.Gender;
                        del.NationId = input.NationId ?? phoneNumberAccount.NationId;
                        del.ProvinceId = input.ProvinceId ?? phoneNumberAccount.ProvinceId;
                        del.DistrictId = input.DistrictId ?? phoneNumberAccount.DistrictId;
                        del.WardId = input.WardId ?? phoneNumberAccount.WardId;
                        del.Address = input.Address ?? phoneNumberAccount.Address;
                        del.SecondPhoneNumber = input.SecondPhoneNumber ?? phoneNumberAccount.SecondPhoneNumber;
                        del.Active = true;
                        del.CoverId = imageId ?? del.CoverId;
                        del.AccountType = AccountTypes.User;
                        del.FullNameSearch = input.FullName;
                    },
                    context) as ICoreResultDto;

                return phoneNumberAccount.Id;
            }

            await this.CreateEntity(
                new AccountEntity
                {
                    Code = $"Coach {input.Username}",
                    Id = input.Id,
                    FirstName = input.FullName.GetFirstName(),
                    MiddleName = input.FullName.GetMiddleName(),
                    LastName = input.FullName.GetLastName(),
                    FullNameSearch = input.FullName,
                    PhoneNumber = input.PhoneNumber,
                    SecondPhoneNumber = input.SecondPhoneNumber,
                    Gender = input.Gender,
                    NationId = input.NationId,
                    ProvinceId = input.ProvinceId,
                    DistrictId = input.DistrictId,
                    WardId = input.WardId,
                    AccountType = AccountTypes.User,
                    CoverId = imageId,
                    HashedPassword = "dummy",
                    PasswordSalt = "dummy",
                    DateOfBirth = input.DateOfBirth,
                    Username = input.Username,
                    Email = input.Email,
                    Address = input.Address,
                    CreatorId = context.AccountId,
                    Active = true,
                }, context);

            return input.Id;
        }

        public async Task ResetPassword(string id, ActionContext context)
        {
            using (var client = new HttpClient())
            {
                var resetPasswordPath = string.Format(ConfigConstant.IsResetPasswordApi, id);
                var isUrl = $"{this.Configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value}/{resetPasswordPath}";
                var token = context.AccessToken.Contains(CommonConstant.Space) ? context.AccessToken.Split(CommonConstant.Space)[1] : context.AccessToken;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var postTask = await client.PostAsync(isUrl, null);

                if (!postTask.IsSuccessStatusCode)
                {
                    throw new ServiceException($"could not send request to identity server {postTask.Content}");
                }
            }
        }

        private async Task CheckAccount(AccountDtos.AppInsertPortal input, ActionContext context)
        {
            await CheckUsername(input.Username, context);
            await CheckSecondPhoneNumber(input.SecondPhoneNumber, context);
            await CheckEmail(input.Email, context);
        }

        private static void FormatPhoneNumbers(AccountDtos.AppInsertPortal input)
        {
            input.PhoneNumber = GetFormatedPhoneNumber(input.PhoneNumber);
            input.SecondPhoneNumber = GetFormatedPhoneNumber(input.SecondPhoneNumber);
        }

        private static void FormatUpdatePhoneNumbers(AccountDtos.AppUpdatePortal input)
        {
            input.PhoneNumber = GetFormatedPhoneNumber(input.PhoneNumber);
            input.SecondPhoneNumber = GetFormatedPhoneNumber(input.SecondPhoneNumber);
        }

        private async Task CheckSecondPhoneNumber(string number, ActionContext context, Guid? id = null)
        {
            if (!string.IsNullOrEmpty(number))
            {
                var secondPhonenumberAccount = await GetEntity(
                    del => del.Where(e => e.SecondPhoneNumber == number && (id.HasValue ? e.Id != id : true)),
                    context);

                if (secondPhonenumberAccount != null)
                {
                    throw new ServiceException(ServiceExceptions.SecondPhoneNumberExists);
                }
            }
        }

        private async Task CheckPhoneNumber(string number, ActionContext context, Guid? id = null)
        {
            if (!string.IsNullOrEmpty(number))
            {
                var phoneNumberAccount = await GetEntity(
                    del => del.Where(e => e.PhoneNumber == number && (id.HasValue ? e.Id != id : true)),
                    context);

                if (phoneNumberAccount != null)
                {
                    throw new ServiceException(ServiceExceptions.PhoneNumberExists);
                }
            }
        }

        private async Task CheckUsername(string username, ActionContext context, Guid? id = null)
        {
            var usernameAccount = await this.GetEntity(
                del => del.Where(e => e.Username == username && (id.HasValue ? e.Id != id : true)),
                context);

            if (usernameAccount != null)
            {
                throw new ServiceException(ServiceExceptions.UsernameExists);
            }
        }

        private async Task CheckEmail(string email, ActionContext context, Guid? id = null)
        {
            if (string.IsNullOrEmpty(email)) return;

            var emailAccount = await this.GetEntity(
                del => del.Where(e => e.Email == email && (id.HasValue ? e.Id != id : true)),
                context);

            if (emailAccount != null)
            {
                throw new ServiceException(ServiceExceptions.EmailExists);
            }
        }

        private static string GetFormatedPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return null;
            }

            var value = phoneNumber.Replace("-", string.Empty);

            if (value.StartsWith("0"))
            {
                var regex = new Regex("0");

                return regex.Replace(value, "+84", 1);
            }

            if (!value.StartsWith("+84"))
            {
                return "+84" + value;
            }

            return value;
        }

        private async Task SendCreateRequest(AccountDtos.AppInsertPortal input, ActionContext context)
        {
            using (var client = new HttpClient())
            {
                var isUrl = $"{this.Configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value}/{ConfigConstant.IsCreateUserApi}";
                var requestObject = new
                {
                    id = input.Id,
                    phoneNumber = input.PhoneNumber,
                    username = input.Username,
                    email = input.Email,
                    password = input.Password,
                    changePassword = input.ChangePassword,
                };
                var content = new StringContent(JsonSerializer.Serialize(requestObject), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", context.AccessToken.Contains(CommonConstant.Space) ? context.AccessToken.Split(CommonConstant.Space)[1] : context.AccessToken);

                var response = await client.PostAsync(isUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServiceException($"could not send request to identity server {response.Content}");
                }
            }
        }

        private async Task SendUpdatePhoneNumberRequest(string phoneNumber, ActionContext context)
        {
            using (var client = new HttpClient())
            {
                var isUrl = $"{this.Configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value}/{ConfigConstant.IsUserPhoneNumberApi}";
                var requestObject = new { phoneNumber };
                var content = new StringContent(JsonSerializer.Serialize(requestObject), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    context.AccessToken.Contains(CommonConstant.Space) ? context.AccessToken.Split(CommonConstant.Space)[1] : context.AccessToken);

                var response = await client.PutAsync(isUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServiceException($"could not send request to identity server {response.Content}");
                }
            }
        }

        private async Task<IEnumerable<UserStatus>> GetStatus(IEnumerable<Guid> ids, ActionContext context)
        {
            if (ids.Count() == 0)
            {
                return new List<UserStatus>();
            }

            using (var client = new HttpClient())
            {
                var isUrl = $"{this.Configuration.GetSection(ConfigConstant.AppSettingIdentityAuthority).Value}/{ConfigConstant.IsUserStatusApi}?";
                var urlBuilder = new StringBuilder(isUrl);

                foreach (var id in ids)
                {
                    urlBuilder.Append($"ids={id}&");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", context.AccessToken.Contains(CommonConstant.Space) ? context.AccessToken.Split(CommonConstant.Space)[1] : context.AccessToken);

                var response = await client.GetAsync(urlBuilder.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServiceException($"could not send request to identity server {response.Content}");
                }

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserStatus>>();

                return result;
            }
        }

        private async Task<Guid?> UploadImage(IFormFile image, ActionContext context, Guid? deleteId = null)
        {
            if (deleteId.HasValue)
            {
                await this.DeleteImage(deleteId.Value, context);
            }

            var imageInput = new ImageDtos.AppInsert
            {
                Type = ImageTypes.Activity,
                images = new List<IFormFile> { image },
            };

            var imageId = await this.ImageService.UploadSingleImageAndGetId(imageInput, context);

            return imageId;
        }

        private async Task DeleteImage(Guid id, ActionContext context)
        {
            await this.ImageService.DeleteImages(new List<Guid> { id }, context);
        }

        public async Task SyncStatus(ActionContext context)
        {
            var entities = await this.GetListEntity(del => del.Where(r => !r.IsDeleted), context);

            if (entities != null && entities.Any())
            {
                var ids = entities.Select(i => i.Id);

                var statusList = await GetStatus(ids, context);

                foreach (var item in entities)
                {
                    var userStatus = statusList.FirstOrDefault(s => s.Id == item.Id);

                    if (userStatus != null)
                    {
                        await this.UpdateEntity(
                            item,
                            del =>
                            {
                                del.Active = userStatus.IsActive;
                            },
                            context);
                    }
                }
            }
        }

        public async Task UpdateAvatarAdmin(Guid accountId, IFormFile image, ActionContext context)
        {
            if (image is null) return;

            var entity = await this.GetEntity(
                del => del.Where(r => !r.IsDeleted && r.Id == accountId),
                context);

            if (entity is null) return;

            Guid? coverId = await UploadImage(image, context, entity.CoverId);

            await this.UpdateEntity(
                entity,
                del =>
                {
                    del.CoverId = coverId;
                },
                context);
        }
    }
}
