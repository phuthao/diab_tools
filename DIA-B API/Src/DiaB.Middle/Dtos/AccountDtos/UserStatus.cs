using System;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public class UserStatus
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }

        public bool ChangePassword { get; set; }
    }
}
