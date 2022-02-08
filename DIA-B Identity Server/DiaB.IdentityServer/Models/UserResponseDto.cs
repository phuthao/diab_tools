using DiaB.IdentityServer.Enums;
using System;

namespace DiaB.IdentityServer.Models
{
    public class UserResponseDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
    }
}
