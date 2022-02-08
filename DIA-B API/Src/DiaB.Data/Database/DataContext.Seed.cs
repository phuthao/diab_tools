using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using CpTech.Core.Data.Entities;
using DiaB.Common.Constants;
using DiaB.Common.Enums;
using DiaB.Data.Database.Entities.Account;
using DiaB.Data.Database.Entities.Common;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database
{
    public partial class DataContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    continue;
                }

                EntityTypeBuilder typeBuilder = builder.Entity(entityType.ClrType);
                ParameterExpression clrParameter = Expression.Parameter(typeBuilder.Metadata.ClrType);
                MemberExpression isDeleted = Expression.Property(clrParameter, nameof(ISoftDeletableEntity.IsDeleted));
                BinaryExpression comparer = Expression.Equal(isDeleted, Expression.Constant(false));
                typeBuilder.HasQueryFilter(Expression.Lambda(comparer, clrParameter));
            }

            var envName = Environment.GetEnvironmentVariable(ConfigConstant.ASPNETCOREENV);

            if (!ConfigConstant.LocalEnv.Equals(envName, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using FileStream stream = File.Open(DataSeedConstant.DataMasterRelativePath, FileMode.Open, FileAccess.Read);
            using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            DataSet dataSet = reader.AsDataSet();

            SeedNationData(builder, dataSet);
            SeedProvinceData(builder, dataSet);
            SeedDistrictData(builder, dataSet);
            SeedWardData(builder, dataSet);
            SeedCommonConfigureData(builder, dataSet);
            SeedAccountData(builder, dataSet);
        }

        private void SeedAccountData(ModelBuilder builder, DataSet dataSet)
        {
            var entities = new List<AccountEntity>();
            DataTable dataTable = dataSet.Tables[DataSeedConstant.AccountMasterData];

            for (var i = 1; i < dataTable.Rows.Count; i++)
            {
                entities.Add(new AccountEntity
                {
                    Id = new Guid((string)dataTable.Rows[i][0]),
                    Username = (string)dataTable.Rows[i][1],
                    FirstName = (string)dataTable.Rows[i][2],
                    MiddleName = (string)dataTable.Rows[i][3],
                    LastName = (string)dataTable.Rows[i][4],
                    Code = (string)dataTable.Rows[i][5],
                    PhoneNumber = (string)dataTable.Rows[i][6],
                    PhoneNumberVerified = (bool)dataTable.Rows[i][7],
                    ForceChangePassword = (bool)dataTable.Rows[i][8],
                    HashedPassword = (string)dataTable.Rows[i][9],
                    PasswordSalt = (string)dataTable.Rows[i][10],
                    DateOfBirth = DateTime.FromOADate((double)dataTable.Rows[i][11]),
                    Gender = (GenderTypes)Convert.ToInt32(dataTable.Rows[i][12]),
                    NationId = new Guid((string)dataTable.Rows[i][13]),
                    ProvinceId = new Guid((string)dataTable.Rows[i][14]),
                    DistrictId = new Guid((string)dataTable.Rows[i][15]),
                    WardId = new Guid((string)dataTable.Rows[i][16]),
                });
            }

            builder.Entity<AccountEntity>().HasData(entities);
        }

        private void SeedNationData(ModelBuilder builder, DataSet dataSet)
        {
            var entities = new List<NationEntity>();
            DataTable dataTable = dataSet.Tables[DataSeedConstant.NationMasterData];

            for (var i = 1; i < dataTable.Rows.Count; i++)
            {
                entities.Add(new NationEntity
                {
                    Id = new Guid((string)dataTable.Rows[i][0]),
                    Name = (string)dataTable.Rows[i][1],
                });
            }

            builder.Entity<NationEntity>().HasData(entities);
        }

        private void SeedProvinceData(ModelBuilder builder, DataSet dataSet)
        {
            var entities = new List<ProvinceEntity>();
            DataTable dataTable = dataSet.Tables[DataSeedConstant.ProvinceMasterData];

            for (var i = 1; i < dataTable.Rows.Count; i++)
            {
                entities.Add(new ProvinceEntity
                {
                    Id = new Guid((string)dataTable.Rows[i][0]),
                    Name = (string)dataTable.Rows[i][2],
                    NationId = new Guid((string)dataTable.Rows[i][4]),
                });
            }

            builder.Entity<ProvinceEntity>().HasData(entities);
        }

        private void SeedDistrictData(ModelBuilder builder, DataSet dataSet)
        {
            var entities = new List<DistrictEntity>();
            DataTable dataTable = dataSet.Tables[DataSeedConstant.DistrictMasterData];

            for (var i = 1; i < dataTable.Rows.Count; i++)
            {
                entities.Add(new DistrictEntity
                {
                    Id = new Guid((string)dataTable.Rows[i][0]),
                    Name = (string)dataTable.Rows[i][2],
                    ProvinceId = new Guid((string)dataTable.Rows[i][4]),
                });
            }

            builder.Entity<DistrictEntity>().HasData(entities);
        }

        private void SeedWardData(ModelBuilder builder, DataSet dataSet)
        {
            var entities = new List<WardEntity>();
            DataTable dataTable = dataSet.Tables[DataSeedConstant.WardMasterData];

            for (var i = 1; i < dataTable.Rows.Count; i++)
            {
                entities.Add(new WardEntity
                {
                    Id = new Guid((string)dataTable.Rows[i][0]),
                    Name = (string)dataTable.Rows[i][2],
                    DistrictId = new Guid((string)dataTable.Rows[i][4]),
                });
            }

            builder.Entity<WardEntity>().HasData(entities);
        }

        private void SeedCommonConfigureData(ModelBuilder builder, DataSet dataSet)
        {
            var entities = new List<CommonConfigureEntity>();
            DataTable dataTable = dataSet.Tables[DataSeedConstant.ConfigureMasterData];

            for (var i = 1; i < dataTable.Rows.Count; i++)
            {
                entities.Add(new CommonConfigureEntity
                {
                    Id = new Guid((string)dataTable.Rows[i][0]),
                    Key = (string)dataTable.Rows[i][1],
                    Value = (string)dataTable.Rows[i][2],
                });
            }

            builder.Entity<CommonConfigureEntity>().HasData(entities);
        }
    }
}
