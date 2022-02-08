using System.ComponentModel.DataAnnotations;

namespace DiaB.IdentityServer.Models
{
    public class LoginEmailModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}