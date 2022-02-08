using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Account
{
    public class ProvinceEntity : ModelEntity<ProvinceEntity>
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public Guid NationId { get; set; }

        public virtual NationEntity Nation { get; set; }

        public virtual IList<DistrictEntity> Districts { get; set; }

        public virtual IList<AccountEntity> Accounts { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<ProvinceEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasOne(x => x.Nation)
                .WithMany(x => x.Provinces)
                .HasForeignKey(x => x.NationId)
                .IsRequired();
        }
    }
}
