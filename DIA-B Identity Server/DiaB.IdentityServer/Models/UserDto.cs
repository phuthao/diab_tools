﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DiaB.IdentityServer.Models
{
    public class UserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool ChangePassword { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
