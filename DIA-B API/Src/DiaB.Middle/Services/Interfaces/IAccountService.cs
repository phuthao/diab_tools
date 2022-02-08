using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Dtos;
using CpTech.Core.Middle.Services;
using DiaB.Common.Enums;
using DiaB.Middle.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ActionContext = DiaB.Common.Utilities.ActionContext;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IAccountService : ICoreService
    {
        Task<List<AccountDtos.AppItem>> GetAll(ActionContext context);

        Task<IPagingData<AccountDtos.AppItem>> GetPage(AccountDtos.AppFilter input, ActionContext context);

        Task<AccountDtos.AppItem> GetAccountById(Guid id, ActionContext context = null);

        Task<GenderTypes> GetGenderOfCurrentUser(ActionContext context);

        Task<ICoreResultDto> UpdateAccount(AccountDtos.AppUpdate input, ActionContext context);

        Task UpdateAvatar(AccountDtos.AppUpdate input, ActionContext context);

        Task<Guid> CreatePortalAccount(AccountDtos.AppInsertPortal input, ActionContext context);

        Task UpdatePortalAccount(AccountDtos.AppUpdatePortal input, ActionContext context);

        Task<AccountDtos.AppItemPortal> GetPortalAccountById(Guid id, ActionContext context);

        Task<IPagingData<AccountDtos.AppItemPortal>> GetPortalAccounts(AccountDtos.AppFilter filter, ActionContext context);

        Task ResetPassword(string id, ActionContext context);

        Task SyncStatus(ActionContext context);
        Task UpdateAvatarAdmin(Guid accountId, IFormFile image, ActionContext context);
        Task ChangePhoneNumber(ChangePhoneNumberRequestDto request, Guid id, ActionContext context);
    }
}
