// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseDbContext.cs
// Created Date:  2018/11/02 12:02 PM
// ------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DiaB.Core.Data.Entities;
using DiaB.Core.Web.Extensions;
using DiaB.Core.Common.Extensions;
using DiaB.Core.Data.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DiaB.Core.Data.Database
{
    public class BaseDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public bool EnableEntityLog { get; set; }

        public BaseDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DoRelationshipMappings(modelBuilder);

            AddSoftDeleteFilter(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            SaveEntityLog();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            SaveEntityLog();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected void AddSoftDeleteFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsSubclassOfGenericType(typeof(BaseEntity<>)))
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                    var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                    var compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
                    var lambda = Expression.Lambda(compareExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        protected void OnBeforeSaving()
        {
            ChangeTracker.DetectChanges();

            var baseEntityEntries = ChangeTracker.Entries().Where(entry => entry.Entity.GetType().IsSubclassOfGenericType(typeof(BaseEntity<>))).ToList();

            if (baseEntityEntries.Any())
            {
                Guid? userId = null;

                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    userId = _httpContextAccessor.HttpContext.GetUserId();
                }

                foreach (var entry in baseEntityEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            entry.CurrentValues["CreatedOn"] = DateTime.UtcNow;
                            entry.CurrentValues["CreatedBy"] = userId;
                            break;
                        case EntityState.Deleted:
                            if (!(bool)entry.Property("IsDeleted").CurrentValue)
                            {
                                entry.State = EntityState.Modified;
                                entry.CurrentValues["IsDeleted"] = true;
                                entry.CurrentValues["DeletedOn"] = DateTime.UtcNow;
                                entry.CurrentValues["DeletedBy"] = userId;
                            }
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Modified:
                            entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
                            entry.CurrentValues["ModifiedBy"] = userId;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        protected void SaveEntityLog()
        {
            if (!EnableEntityLog)
            {
                return;
            }

            var entries = ChangeTracker.Entries()
                                       .Where(entry => entry.State == EntityState.Modified &&
                                                       entry.OriginalValues.Properties.Any(property => property.PropertyInfo.GetCustomAttributes(true).Any(attribute => attribute.GetType() == typeof(LogAttribute))))
                                       .ToList();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                var logEvent = new EntityLogEvent
                {
                    CreatedOn = now,
                    EntityKey = GetKey(entry).ToString(),
                    EntityName = entry.Entity.GetType().Name
                };

                foreach (var property in entry.OriginalValues.Properties)
                {
                    if (property.PropertyInfo.GetCustomAttributes(true).Any(attribute => attribute.GetType() == typeof(LogAttribute)))
                    {
                        var originalValue = entry.OriginalValues[property].ToString();
                        var currentValue = entry.CurrentValues[property].ToString();
                        if (originalValue != currentValue)
                        {
                            logEvent.EntityLogs.Add(new EntityLog
                            {
                                CreatedOn = now,
                                PropertyName = property.Name,
                                OldValue = originalValue,
                                NewValue = currentValue
                            });
                        }
                    }
                }

                if (logEvent.EntityLogs.Any())
                {
                    EntityLogEvents.Add(logEvent);
                }
            }
        }

        protected object GetKey(EntityEntry entityEntry)
        {
            var entityType = entityEntry.Entity.GetType().BaseType;
            var keyName = Model.FindEntityType(entityType).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            return entityEntry.Entity.GetValue(keyName);
        }

        public virtual DbSet<EntityLog> EntityLogs { get; set; }

        public virtual DbSet<EntityLogEvent> EntityLogEvents { get; set; }

        private static void DoRelationshipMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityLog>()
                        .HasOne(entityLog => entityLog.EntityLogEvent)
                        .WithMany(entityLogEvent => entityLogEvent.EntityLogs)
                        .HasForeignKey(entityLog => entityLog.EntityLogEventId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
        }
    }
}
