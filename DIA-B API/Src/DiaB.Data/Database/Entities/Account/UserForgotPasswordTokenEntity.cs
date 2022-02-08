using System;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using DiaB.Common.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Account
{
    public class UserForgotPasswordTokenEntity : ModelEntity<UserForgotPasswordTokenEntity>
    {
        [Required]
        [MaxLength(250)]
        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }

        public UserForgotPasswordTokenStates Status { get; set; }

        public Guid DiaBAccountId { get; set; }

        public virtual AccountEntity DiaBAccount { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<UserForgotPasswordTokenEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasOne(x => x.DiaBAccount)
                .WithMany(x => x.UserForgotPasswordTokens)
                .HasForeignKey(x => x.DiaBAccountId)
                .IsRequired();
        }
    }
}
