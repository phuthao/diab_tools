using System.Collections.Generic;
using DiaB.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> ChangePassword(string userId, string newPassword, bool sendEmail = false);

        Task<IdentityResult> BlockUser(string userId);

        Task<IdentityResult> UnblockUser(string userId);

        Task CreateUser(UserDto user);

        Task<IdentityResult> UpdateUser(string userId, UserDto user);
        Task<IdentityResult> ResetPassword(string userId);
        Task<IEnumerable<UserStatus>> GetUserStatus(IEnumerable<string> ids);
        Task<IdentityResult> ChangePassword(string userId, ChangePasswordRequestDto request);

        Task SetChangePassword(string userId, bool value);
        Task<IdentityResult> ChangePhoneNumber(string userId, ChangePhoneNumberRequestDto request);
    }
}
