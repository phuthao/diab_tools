using DiaB.IdentityServer.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Configurations
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = string.Format(ErrorConstant.ShortPassword.Value, length),
                Description = ErrorConstant.ShortPassword.Key,
            };
        }
    }
}
