using System.ComponentModel;

namespace DiaB.Common.Enums
{
    [Description("trạng thái mã quên mật khẩu")]
    public enum UserForgotPasswordTokenStates
    {
        [Description("đã sử dụng")]
        Used = 0,
        [Description("cấp mới")]
        New = 1,
    }
}
