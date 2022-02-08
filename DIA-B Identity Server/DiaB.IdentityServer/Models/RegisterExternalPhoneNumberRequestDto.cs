using System.ComponentModel.DataAnnotations;

namespace DiaB.IdentityServer.Models
{
    public class RegisterExternalPhoneNumberRequestDto
    {
        [Required]
        public string ProviderName { get; set; }

        [Required]
        public string ProviderKey { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }

    public class LinkAccountRequestDto : RegisterExternalPhoneNumberRequestDto
    {
        public string Token { get; set; }
    }

    public class RegisterExternalDto : RegisterExternalPhoneNumberRequestDto
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }

    public class UnlinkAccountRequestDto
    {
        public string ProviderName { get; set; }
    }
}
