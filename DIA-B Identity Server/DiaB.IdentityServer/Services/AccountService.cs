using DiaB.Core.Web.Helpers;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Helpers;
using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace DiaB.IdentityServer.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        private readonly PersistedGrantRepository _persistedGrantRepository;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserRepository _userRepository;

        private readonly ISmsService _smsService;

        private static Random random = new Random();

        private readonly IEmailService _emailService;

        public AccountService(
            UserManager<User> userManager,
            PersistedGrantRepository persistedGrantRepository,
            RoleManager<IdentityRole> roleManager,
            UserRepository userRepository,
            ISmsService smsService, IEmailService emailService)
        {
            _userManager = userManager;
            _persistedGrantRepository = persistedGrantRepository;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _smsService = smsService;
            _emailService = emailService;
        }

        public async Task<IdentityResult> ChangePassword(string userId, string newPassword, bool sendEmail = false)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded && sendEmail)
                {
                    await _emailService.Send(user.Email, "Reset mật khẩu", $"Mật khẩu mới của bạn là {newPassword}");
                }

                user.MustChangePassword = false;

                await _userManager.UpdateAsync(user);

                return result;
            }

            return null;
        }

        public async Task<IdentityResult> ChangePassword(string userId, ChangePasswordRequestDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                return await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            }

            return null;
        }

        public async Task<IdentityResult> ChangePhoneNumber(string userId, ChangePhoneNumberRequestDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber((request.PhoneNumber));
            var phoneNumberUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.IsActive);

            if (phoneNumberUser != null)
            {
                MessageHelper.ThrowError(ErrorConstant.PhoneNumberExists);
            }

            if (user != null)
            {
                user.PhoneNumber = phoneNumber;

                return await _userManager.UpdateAsync(user);
            }

            return null;
        }

        public async Task SetChangePassword(string userId, bool value)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return;

            user.MustChangePassword = value;

            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> BlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.IsActive = false;

                await _persistedGrantRepository.DeleteAsync(row => row.SubjectId == userId, false);

                return await _userManager.UpdateAsync(user);
            }

            return null;
        }

        public async Task<IdentityResult> UnblockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.IsActive = true;

                return await _userManager.UpdateAsync(user);
            }

            return null;
        }

        public async Task CreateUser(UserDto user)
        {
            user.PhoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(user.PhoneNumber);

            await CheckUser(user);

            var phoneNumberUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber && u.Id != user.Id.ToString());;

            if (phoneNumberUser != null)
            {
                await UpdatePhoneNumberUser(user, phoneNumberUser);

                return;
            }

            await CreateUserImp(user);

            //await _smsService.SendMessage(user.PhoneNumber, password);
        }

        private async Task UpdatePhoneNumberUser(UserDto user, User phoneNumberUser)
        {
            phoneNumberUser.UserName = user.UserName;
            phoneNumberUser.Email = user.Email;
            phoneNumberUser.MustChangePassword = user.ChangePassword;

            await _userManager.UpdateAsync(phoneNumberUser);

            await _userManager.AddToRoleAsync(phoneNumberUser, Role.User);

            await ChangePassword(phoneNumberUser.Id, user.Password);
        }

        private async Task CreateUserImp(UserDto user)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id.ToString());

            if (dbUser != null)
            {
                dbUser.PhoneNumber = user.PhoneNumber;
                dbUser.UserName = user.UserName;
                dbUser.Email = user.Email;
                dbUser.MustChangePassword = user.ChangePassword;

                await _userManager.UpdateAsync(dbUser);
                return;
            }

            var newUser = new User
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = true,
                EmailConfirmed = true,
                LockoutEnabled = true,
                CreatedDate = DateTime.UtcNow,
                MustChangePassword = user.ChangePassword
            };

            await _userManager.CreateAsync(newUser, user.Password);
            await _userManager.AddToRoleAsync(newUser, Role.User);
        }

        private async Task CheckUser(UserDto user)
        {
            var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Id != user.Id.ToString());

            if (dbUser != null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserDuplicated);
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return;
            }

            dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Id != user.Id.ToString());

            if (dbUser != null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserDuplicated);
            }
        }

        private async Task<bool> IsPhoneNumberExist(UserDto user)
        {
            User dbUser;
            dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);

            return dbUser != null;
        }

        public async Task<IdentityResult> UpdateUser(string userId, UserDto user)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.UserName = user.UserName;
                existingUser.PhoneNumber = user.PhoneNumber;
            }

            return await _userManager.UpdateAsync(existingUser);
        }

        public async Task<IdentityResult> ResetPassword(string userId)
        {
            return await ChangePassword(userId, GetRandomString(), sendEmail: true);
        }

        public static string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<IEnumerable<UserStatus>> GetUserStatus(IEnumerable<string> ids)
        {
            var statusList = _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => new UserStatus
                    {Id = u.Id, IsActive = u.IsActive, ChangePassword = u.MustChangePassword});

            return statusList;
        }
    }
}
