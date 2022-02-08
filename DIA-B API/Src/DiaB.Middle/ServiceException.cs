using System.ComponentModel;
using CpTech.Core.Common.Attributes;

namespace DiaB.Middle
{
    public class ServiceException
        : CpTech.Core.Common.Exceptions.ServiceException
    {
        public ServiceException(
            string code = "exception",
            int httpCode = 500,
            string message = "unexpected exception",
            object data = null)
            : base(code, httpCode, message, data)
        {
        }

        public ServiceException(
            ServiceExceptions code,
            object data = null)
            : base(code, data)
        {
        }
    }

#pragma warning disable SA1201 // ElementsMustAppearInTheCorrectOrder
    public enum ServiceExceptions
    {
        [ErrorData(httpCode: 404)]
        [Description("không tìm thấy đối tượng")]
        ObjectNotFound,

        [ErrorDataAttribute(httpCode: 404)]
        [Description("đối tượng đã bị xóa")]
        ObjectIsDeleted,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("đối tượng không hợp lệ")]
        ObjectInvalid,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("đối tượng đã tồn tại")]
        ObjectIsExisted,

        [ErrorDataAttribute(httpCode: 403)]
        [Description("đối tượng không thể bị chỉnh sửa")]
        ObjectUneditable,

        [ErrorDataAttribute(httpCode: 404)]
        [Description("đối tượng không còn khả dụng")]
        ObjectNotAvailable,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("thông tin xác thực không chính xác")]
        AuthInputInvalid,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("dung lượng hình ảnh quá lớn")]
        ImageLengthIsInvalid,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("chuẩn hình ảnh không phù hợp")]
        ImageTypeIsInvalid,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("đối tượng đã tồn tại")]
        ObjectIsDuplicated,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("username đã tồn tại")]
        UsernameExists,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("số điện thoại đã tồn tại")]
        PhoneNumberExists,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("số điện thoại thứ 2 đã tồn tại")]
        SecondPhoneNumberExists,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("code đã tồn tại")]
        CodeExists,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("email đã tồn tại")]
        EmailExists,

        [ErrorDataAttribute(httpCode: 400)]
        [Description("có món ăn trong group đang active")]
        ActiveFoodInCategory,
    }
}
