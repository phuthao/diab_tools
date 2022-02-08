using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;

namespace DiaB.Data.Database.Entities.Account
{
    public class NationEntity : ModelEntity<NationEntity>
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public virtual IList<ProvinceEntity> Provinces { get; set; }

        public virtual IList<AccountEntity> Accounts { get; set; }
    }
}
