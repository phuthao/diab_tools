using DiaB.Core.Web.Helpers;
using DiaB.IdentityServer.Constants;
using DiaB.IdentityServer.Helpers;
using DiaB.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly CustomPhoneNumberTokenProvider _phoneNumberTokenProvider;

        private readonly UserManager<User> _userManager;

        private readonly ISmsService _smsService;

        public AuthService(
            CustomPhoneNumberTokenProvider phoneNumberTokenProvider,
            UserManager<User> userManager, ISmsService smsService = null)
        {
            _phoneNumberTokenProvider = phoneNumberTokenProvider;
            _userManager = userManager;
            _smsService = smsService;
        }

        public async Task<bool> ExistPhoneNumber(string phoneNumber)
        {
            phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(phoneNumber);

            return await _userManager.Users.AnyAsync(row => row.PhoneNumber == phoneNumber);
        }

        public async Task<OtpRequestResult> RegisterUser(PhoneNumberRegisterRequestDto request)
        {
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(request.PhoneNumber);
            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);
            string token;

            if (user != null)
            {
                if (user.PhoneNumberConfirmed)
                {
                    MessageHelper.ThrowError(ErrorConstant.UserDuplicated);
                }

                if (!CheckAndUpdateOtpCount(user))
                {
                    return new OtpRequestResult
                    {
                        Message = "Bạn đã yêu cầu otp quá số lần quy định trong ngày.",
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    MessageHelper.ThrowError(ErrorConstant.InvalidPassword);
                }

                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var updateResult = await _userManager.ResetPasswordAsync(user, resetPasswordToken, request.Password.Trim());

                await _userManager.UpdateAsync(user);

                token = await GeneratePhoneNumberToken(PhoneNumberGrant.RegisterPurpose, phoneNumber);

                return new OtpRequestResult
                {
                    IsSuccess = true,
                    Token = token,
                    RemainingRequestCount = 5 - user.OtpRequestCount,
                };
            }

            User newUser = new User
            {
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                IsActive = false,
                IsMobileAccount = true,
                EmailConfirmed = false,
                LockoutEnabled = false,
                OtpRequestCount = 1,
                OtpRequestDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createResult.Succeeded && createResult.Errors.Any())
            {
                var error = createResult.Errors.FirstOrDefault();
                MessageHelper.ThrowError(error?.Code, error?.Description);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(newUser, Role.User);

            if (!addRoleResult.Succeeded && addRoleResult.Errors.Any())
            {
                var error = addRoleResult.Errors.FirstOrDefault();
                MessageHelper.ThrowError(error?.Code, error?.Description);
            }

            token = await GeneratePhoneNumberToken(PhoneNumberGrant.RegisterPurpose, phoneNumber);

            return new OtpRequestResult
            {
                IsSuccess = true,
                Token = token,
                RemainingRequestCount = 4,
            };
        }

        public async Task ResetUserPassword(UserPasswordResetRequestDto request)
        {
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(request.PhoneNumber);

            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            if (!await _phoneNumberTokenProvider.ValidateAsync(PhoneNumberGrant.RecoverPurpose, request.Token, _userManager, user))
            {
                MessageHelper.ThrowError(ErrorConstant.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, request.Password.Trim());

            if (!resetResult.Succeeded && resetResult.Errors.Any())
            {
                var error = resetResult.Errors.FirstOrDefault();
                MessageHelper.ThrowError(error?.Code, error?.Description);
            }
        }

        public async Task<OtpRequestResult> GetResetPasswordToken(string phoneNumber)
        {
            phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(phoneNumber);

            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            if (!CheckAndUpdateOtpCount(user))
            {
                return new OtpRequestResult
                {
                    Message = "Bạn đã yêu cầu otp quá số lần quy định trong ngày.",
                };
            }

            await _userManager.UpdateAsync(user);

            var token = await _phoneNumberTokenProvider.GenerateAsync(PhoneNumberGrant.RecoverPurpose, _userManager, user);

            await _smsService.SendMessage(phoneNumber, token);

            return new OtpRequestResult
            {
                IsSuccess = true,
                Token = token,
                RemainingRequestCount = 5 - user.OtpRequestCount,
            };
        }

        public async Task<string> GeneratePhoneNumberToken(string purpose, string phoneNumber)
        {
            phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(phoneNumber);

            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            var token = await _phoneNumberTokenProvider.GenerateAsync(purpose, _userManager, user);

            await _smsService.SendMessage(phoneNumber, token);

            return token;
        }

        public async Task VerifyRecoverToken(string phoneNumber, string token)
        {
            phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(phoneNumber);

            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            if (!await _phoneNumberTokenProvider.ValidateAsync(PhoneNumberGrant.RecoverPurpose, token, _userManager, user))
            {
                MessageHelper.ThrowError(ErrorConstant.InvalidToken, HttpStatusCode.Unauthorized);
            }
        }

        public async Task VerifyRegisterToken(string phoneNumber, string token)
        {
            phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(phoneNumber);

            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            if (!await _phoneNumberTokenProvider.ValidateAsync(PhoneNumberGrant.RegisterPurpose, token, _userManager, user))
            {
                MessageHelper.ThrowError(ErrorConstant.InvalidToken, HttpStatusCode.Unauthorized);
            }

            user.IsActive = true;
            user.PhoneNumberConfirmed = true;
            user.ActivatedDate = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded && updateResult.Errors.Any())
            {
                var error = updateResult.Errors.FirstOrDefault();
                MessageHelper.ThrowError(error?.Code, error?.Description);
            }
        }

        public async Task<string> ValidatePhoneNumberToken(string purpose, string phoneNumber, string token)
        {
            phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(phoneNumber);

            var user = await _userManager.Users.SingleOrDefaultAsync(row => row.PhoneNumber == phoneNumber);

            if (user != null && await _phoneNumberTokenProvider.ValidateAsync(purpose, token, _userManager, user))
            {
                return user.Id;
            }

            return null;
        }

        public async Task LinkAccount(LinkAccountRequestDto request)
        {
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(request.PhoneNumber);
            var externalUser = await _userManager.FindByLoginAsync(request.ProviderName, request.ProviderKey);
            var phoneNumberUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            var emailUser = string.IsNullOrEmpty(request.Email) ? null :
                _userManager.Users.FirstOrDefault(u => u.GoogleEmail.ToLower() == request.Email.ToLower());

            CheckUserForLinkAccount(externalUser, phoneNumberUser, emailUser);

            if (!await _phoneNumberTokenProvider.ValidateAsync(ExternalGrant.LinkAccountPurpose, request.Token, _userManager, phoneNumberUser))
            {
                MessageHelper.ThrowError(ErrorConstant.InvalidToken, HttpStatusCode.Unauthorized);
            }

            await _userManager.AddLoginAsync(phoneNumberUser, new UserLoginInfo(request.ProviderName, request.ProviderKey, request.ProviderName));

            if (request.ProviderName.ToLower() == "google")
            {
                phoneNumberUser.GoogleEmail = request.Email;

                await _userManager.UpdateAsync(phoneNumberUser);
            }
        }

        private static void CheckUserForLinkAccount(User externalUser, User phoneNumberUser, User emailUser)
        {
            if (phoneNumberUser is null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            if (externalUser != null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserDuplicated);
            }

            if (emailUser != null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserDuplicated);
            }
        }

        public async Task<OtpRequestResult> GetLinkAccountOtp(RegisterExternalPhoneNumberRequestDto request)
        {
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(request.PhoneNumber);
            var externalUser = await _userManager.FindByLoginAsync(request.ProviderName, request.ProviderKey);
            var phoneNumberUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            var emailUser = string.IsNullOrEmpty(request.Email) ? null :
                _userManager.Users.FirstOrDefault(u => u.GoogleEmail.ToLower() == request.Email.ToLower());

            CheckUserForLinkAccount(externalUser, phoneNumberUser, emailUser);

            if (!CheckAndUpdateOtpCount(phoneNumberUser))
            {
                return new OtpRequestResult
                {
                    Message = "Bạn đã yêu cầu otp quá số lần quy định trong ngày.",
                };
            }

            await _userManager.UpdateAsync(phoneNumberUser);

            var token = await _phoneNumberTokenProvider.GenerateAsync(ExternalGrant.LinkAccountPurpose, _userManager, phoneNumberUser);

            await _smsService.SendMessage(phoneNumber, token);

            return new OtpRequestResult
            {
                IsSuccess = true,
                Token = token,
                RemainingRequestCount = 5 - phoneNumberUser.OtpRequestCount
            };
        }

        public async Task CreatExternalUser(RegisterExternalPhoneNumberRequestDto request)
        {
            var user = await _userManager.FindByLoginAsync(request.ProviderName, request.ProviderKey);

            if (user != null)
            {
                MessageHelper.ThrowError(ErrorConstant.ExternalUserExists);
            }

            user = await CreateExternalUserImp(request);

            user.IsActive = true;

            await _userManager.UpdateAsync(user);
        }

        public async Task<OtpRequestResult> GetExternalRegisterOtp(RegisterExternalPhoneNumberRequestDto request)
        {
            var phoneNumber = PhoneNumberHelper.GetFormatedPhoneNumber(request.PhoneNumber);

            await CheckPhoneNumber(phoneNumber);

            var user = await _userManager.FindByLoginAsync(request.ProviderName, request.ProviderKey);

            if (user is null)
            {
                user = await CreateExternalUserImp(request);
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumberConfirmed && user.IsActive)
            {
                MessageHelper.ThrowError(ErrorConstant.UserVerified);
            }

            if (!CheckAndUpdateOtpCount(user))
            {
                return new OtpRequestResult
                {
                    Message = "Bạn đã yêu cầu otp quá số lần quy định trong ngày.",
                };
            }

            user.PhoneNumber = phoneNumber;
            user.FirstLinkedAccount = request.ProviderName;

            await _userManager.UpdateAsync(user);

            var token = await _phoneNumberTokenProvider.GenerateAsync(ExternalGrant.RegisterPurpose, _userManager, user);

            await _smsService.SendMessage(phoneNumber, token);

            return new OtpRequestResult
            {
                IsSuccess = true,
                Token = token,
                RemainingRequestCount = 5 - user.OtpRequestCount
            };
        }

        private async Task<User> CreateExternalUserImp(RegisterExternalPhoneNumberRequestDto request)
        {
            var id = Guid.NewGuid().ToString();
            var phoneNumber = string.Empty;

            phoneNumber = !string.IsNullOrEmpty(request.PhoneNumber)
                ? PhoneNumberHelper.GetFormatedPhoneNumber(request.PhoneNumber)
                : GetNewUsername();

            var newUser = new User
            {
                Id = id,
                PhoneNumber = phoneNumber,
                UserName = phoneNumber,
                CreatedDate = DateTime.UtcNow
            };

            await _userManager.CreateAsync(newUser);

            var user = await _userManager.FindByIdAsync(id);

            await _userManager.AddLoginAsync(user, new UserLoginInfo(request.ProviderName, request.ProviderKey, request.ProviderName));
            await _userManager.AddToRoleAsync(user, Role.User);

            return user;
        }

        private string GetNewUsername()
        {
            var count = _userManager.Users.Count();
            var username = $"User_{++count}";

            while (true)
            {
                if (!_userManager.Users.Any(u => u.UserName == username))
                    break;

                username = $"User_{++count}";
            }

            return username;
        }

        public async Task UnlinkAccount(string id, string providerName)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                MessageHelper.ThrowError(ErrorConstant.UserNotFound);
            }

            if (!user.IsMobileAccount)
            {
                MessageHelper.ThrowError(ErrorConstant.CannotUnlink);
            }

            var logins = await _userManager.GetLoginsAsync(user);
            var login = logins.FirstOrDefault(l => l.LoginProvider == providerName);

            if (login == null) return;

            await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);

            if (login.LoginProvider.ToLower() == "google")
            {
                user.GoogleEmail = string.Empty;

                await _userManager.UpdateAsync(user);
            }
        }

        private static bool CheckAndUpdateOtpCount(User user)
        {
            var totalHours = DateTime.UtcNow.Subtract(user.OtpRequestDate).TotalHours;

            if (totalHours > 24)
            {
                user.OtpRequestCount = 1;
                user.OtpRequestDate = DateTime.UtcNow;

                return true;
            }

            if (user.OtpRequestCount == 5)
            {
                if (totalHours < 24)
                {
                    return false;
                }

                user.OtpRequestCount = 0;
            }

            user.OtpRequestCount++;
            user.OtpRequestDate = DateTime.UtcNow;

            return true;
        }

        private async Task CheckPhoneNumber(string phoneNumber)
        {
            var phoneUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (phoneUser != null)
            {
                if (phoneUser.PhoneNumberConfirmed && phoneUser.IsActive)
                {
                    MessageHelper.ThrowError(ErrorConstant.PhoneNumberExists);
                }

                phoneUser.PhoneNumber = null;

                await _userManager.UpdateAsync(phoneUser);
            }
        }
    }
}
