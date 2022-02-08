using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Account
{
    public class DistrictEntity : ModelEntity<DistrictEntity>
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public Guid ProvinceId { get; set; }

        public virtual ProvinceEntity Province { get; set; }

        public virtual IList<WardEntity> Wards { get; set; }

        public virtual IList<AccountEntity> Accounts { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<DistrictEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasOne(x => x.Province)
                .WithMany(x => x.Districts)
                .HasForeignKey(x => x.ProvinceId)
                .IsRequired();
        }
    }
}
