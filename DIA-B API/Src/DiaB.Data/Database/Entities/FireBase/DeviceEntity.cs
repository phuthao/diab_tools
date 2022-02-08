using System;
using CpTech.Core.Data.Entities;
using DiaB.Common.Enums;
using DiaB.Data.Database.Entities.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.FireBase
{
    public class DeviceEntity : ModelEntity<DeviceEntity>
    {
        public DeviceTypes DeviceType { get; set; }

        public string DeviceInformation { get; set; }

        public Guid AccountId { get; set; }

        public virtual AccountEntity Account { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<DeviceEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Devices)
                .HasForeignKey(x => x.AccountId)
                .IsRequired();
        }
    }
}
