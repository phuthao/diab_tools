using System;
using System.Threading.Tasks;
using DiaB.IdentityServer.Models;

namespace DiaB.IdentityServer.Services
{
    public interface IAuthService
    {
        Task<bool> ExistPhoneNumber(string phoneNumber);

        Task<OtpRequestResult> RegisterUser(PhoneNumberRegisterRequestDto request);

        Task VerifyRegisterToken(string phoneNumber, string token);

        Task VerifyRecoverToken(string phoneNumber, string token);

        Task<string> GeneratePhoneNumberToken(string purpose, string phoneNumber);

        Task ResetUserPassword(UserPasswordResetRequestDto request);

        Task<string> ValidatePhoneNumberToken(string purpose, string phoneNumber, string token);

        Task<OtpRequestResult> GetResetPasswordToken(string phoneNumber);

        Task<OtpRequestResult> GetLinkAccountOtp(RegisterExternalPhoneNumberRequestDto request);

        Task LinkAccount(LinkAccountRequestDto request);

        Task<OtpRequestResult> GetExternalRegisterOtp(RegisterExternalPhoneNumberRequestDto request);

        Task UnlinkAccount(string id, string providerName);
        Task CreatExternalUser(RegisterExternalPhoneNumberRequestDto request);
    }
}
