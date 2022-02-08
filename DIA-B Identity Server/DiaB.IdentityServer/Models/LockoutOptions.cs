using System;

namespace DiaB.IdentityServer.Models
{
    public class LockoutOptions
    {
        public bool AllowedForNewUsers { get; set; }

        public int DefaultLockoutTimeSpan { get; set; }

        public int MaxFailedAccessAttempts { get; set; }
    }
}
