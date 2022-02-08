using System.ComponentModel;
using CpTech.Core.Common.Attributes;

namespace DiaB.Data
{
    public class RepositoryException
        : CpTech.Core.Common.Exceptions.RepositoryException
    {
        public RepositoryException(
            string code = "exception",
            int httpCode = 500,
            string message = "unexpected exception",
            object data = null)
            : base(code, httpCode, message, data)
        {
        }

        public RepositoryException(
            RepositoryExceptions code,
            object data = null)
            : base(code, data)
        {
        }
    }

#pragma warning disable SA1201 // ElementsMustAppearInTheCorrectOrder
    public enum RepositoryExceptions
    {
        [ErrorData]
        [Description("mật khẩu không đúng cấu trúc cho phép")]
        PasswordInvalid,
    }
}
