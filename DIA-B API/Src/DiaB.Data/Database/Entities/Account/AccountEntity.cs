using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using DiaB.Common.Enums;
using DiaB.Data.Database.Entities.FireBase;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Account
{
    public class AccountEntity : ModelEntity<AccountEntity>
    {
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }

        [MaxLength(250)]
        public string FirstName { get; set; }

        [MaxLength(250)]
        public string MiddleName { get; set; }

        [MaxLength(250)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string FullNameSearch { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderTypes? Gender { get; set; }

        [Required]
        [MaxLength(100)]
        public override string Code { get; set; }

        public string FacebookUserId { get; set; }

        public string GoogleUserId { get; set; }

        [MaxLength(13)]
        public string PhoneNumber { get; set; }

        [MaxLength(13)]
        public string SecondPhoneNumber { get; set; }

        public bool PhoneNumberVerified { get; set; }

        public bool ForceChangePassword { get; set; }

        [Required]
        [MaxLength(500)]
        public string HashedPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordSalt { get; set; }

        public Guid? NationId { get; set; }

        public Guid? ProvinceId { get; set; }

        public Guid? DistrictId { get; set; }

        public Guid? WardId { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public Guid? CoverId { get; set; }

        public bool Active { get; set; }

        public AccountTypes AccountType { get; set; }

        public virtual NationEntity Nation { get; set; }

        public virtual ProvinceEntity Province { get; set; }

        public virtual DistrictEntity District { get; set; }

        public virtual WardEntity Ward { get; set; }

        public virtual IList<AccountRoleEntity> AccountRoles { get; set; }

        public virtual IList<UserForgotPasswordTokenEntity> UserForgotPasswordTokens { get; set; }

        public virtual IList<DeviceEntity> Devices { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<AccountEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne(x => x.Nation)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.NationId);

            builder.HasOne(x => x.Province)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.ProvinceId);

            builder.HasOne(x => x.District)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.DistrictId);

            builder.HasOne(x => x.Ward)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.WardId);
        }
    }
}
