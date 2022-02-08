using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Account
{
    public class WardEntity : ModelEntity<WardEntity>
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public Guid DistrictId { get; set; }

        public virtual DistrictEntity District { get; set; }

        public virtual IList<AccountEntity> Accounts { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<WardEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasOne(x => x.District)
                .WithMany(x => x.Wards)
                .HasForeignKey(x => x.DistrictId)
                .IsRequired();
        }
    }
}
