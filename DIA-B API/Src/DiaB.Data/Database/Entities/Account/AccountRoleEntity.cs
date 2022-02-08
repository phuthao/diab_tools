using System;
using CpTech.Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Account
{
    public class AccountRoleEntity : BaseEntity<AccountRoleEntity>
    {
        public Guid AccountId { get; set; }

        public Guid RoleId { get; set; }

        public virtual AccountEntity Account { get; set; }

        public virtual RoleEntity Role { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<AccountRoleEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasKey(x => new { DiaBAccountId = x.AccountId, x.RoleId });

            builder.HasOne(src => src.Account)
                .WithMany(desc => desc.AccountRoles)
                .HasForeignKey(x => x.AccountId)
                .IsRequired();

            builder.HasOne(src => src.Role)
                .WithMany(desc => desc.AccountRoles)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
        }
    }
}
