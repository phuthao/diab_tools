using DiaB.Core.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiaB.Core.Web.Authorization.Entities
{
    public class User : BaseEntity<Guid>
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}
