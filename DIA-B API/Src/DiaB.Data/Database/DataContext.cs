using CpTech.Core.Data.Contexts;
using DiaB.Data.Database.Entities;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Database.Entities.Common;
using DiaB.Data.Database.Entities.FireBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DiaB.Data.Database
{
    public partial class DataContext : MySqlContext
    {
        public DataContext(IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(Assembly.GetExecutingAssembly(), configuration, loggerFactory)
        {
        }

        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<AccountRoleEntity> AccountRoles { get; set; }

        public DbSet<UserForgotPasswordTokenEntity> UserForgotPasswordTokens { get; set; }

        public DbSet<CommonConfigureEntity> CommonConfigures { get; set; }

        public DbSet<NationEntity> Nations { get; set; }

        public DbSet<ProvinceEntity> Provinces { get; set; }

        public DbSet<DistrictEntity> Districts { get; set; }

        public DbSet<WardEntity> Wards { get; set; }

        public DbSet<TermAndConditionEntity> TermAndConditions { get; set; }

        public DbSet<DeviceEntity> Devices { get; set; }
    }
}
