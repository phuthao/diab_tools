using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;

namespace DiaB.Data.Database.Entities.Account
{
    public class RoleEntity : ModelEntity<RoleEntity>
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public override string Code { get; set; }

        public virtual IList<AccountRoleEntity> AccountRoles { get; set; }
    }
}
