using System.ComponentModel.DataAnnotations;

namespace DiaB.IdentityServer.Models
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận không trùng khớp")]
        public string RepeatPassword { get; set; }
    }
}
