using System;
using System.Collections;
using System.Collections.Generic;

namespace DiaB.IdentityServer.Models
{
    public class UserStatus
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool ChangePassword { get; set; }
    }
}