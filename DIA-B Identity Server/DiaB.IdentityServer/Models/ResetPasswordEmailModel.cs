using System.ComponentModel.DataAnnotations;

namespace DiaB.IdentityServer.Models
{
    public class ResetPasswordEmailModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        public string Email { get; set; }
    }
}