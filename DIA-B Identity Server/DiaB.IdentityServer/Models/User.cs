using Microsoft.AspNetCore.Identity;
using System;

namespace DiaB.IdentityServer.Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; }

        public int OtpRequestCount { get; set; }

        public DateTime OtpRequestDate { get; set; }

        public bool IsMobileAccount { get; set; }

        public string FirstLinkedAccount { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ActivatedDate { get; set; }

        public bool MustChangePassword { get; set; }

        public string GoogleEmail { get; set; }
    }
}
