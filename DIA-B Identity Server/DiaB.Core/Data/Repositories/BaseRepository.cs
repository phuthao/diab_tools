// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseRepository.cs
// Created Date:  2018/10/15 6:42 PM
// ------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DiaB.Core.Data.Entities;
using DiaB.Core.Data.Uow;
using DiaB.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiaB.Core.Data.Repositories
{
    public abstract class BaseRepository<TEntity, TDbContext> : IBaseRepository<TEntity> where TEntity : class
                                                                                         where TDbContext : DbContext
    {
        public IUowRepository<TDbContext> Uow { get; }

        protected BaseRepository(IUowRepository<TDbContext> uow)
        {
            Uow = uow;
        }

        public virtual DbSet<TEntity> GetAll(bool isAsNoTracking = true)
        {
            Uow.DbContext.ChangeTracker.QueryTrackingBehavior = isAsNoTracking ? QueryTrackingBehavior.NoTracking : QueryTrackingBehavior.TrackAll;

            return Uow.DbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll(bool isAsNoTracking, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            var query = Uow.DbContext.Set<TEntity>().AsNoTracking();

            if (!isAsNoTracking)
            {
                query = query.AsTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            return query;
        }

        public virtual IQueryable<TEntity> ExecuteSqlQuery(string query)
        {
            return Uow.DbContext.Set<TEntity>().FromSqlRaw(query);
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = Uow.DbContext.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            return await query.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = Uow.DbContext.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            Uow.DbContext.Set<TEntity>().Add(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (Uow.DbContext.Entry(entity).State == EntityState.Detached)
            {
                Uow.DbContext.Set<TEntity>().Attach(entity);
            }

            Uow.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task DeleteAsync(object key, bool softDelete = true)
        {
            await DeleteAsync(await Uow.DbContext.Set<TEntity>().FindAsync(key), softDelete);
        }

        public virtual async Task DeleteAsync(TEntity entity, bool softDelete = true)
        {
            if (Uow.DbContext.Entry(entity).State == EntityState.Detached)
            {
                Uow.DbContext.Set<TEntity>().Attach(entity);
            }

            if (!softDelete && entity.GetType().IsSubclassOfGenericType(typeof(BaseEntity<>)))
            {
                Uow.DbContext.Entry(entity).CurrentValues["IsDeleted"] = true;
            }

            Uow.DbContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool softDelete = true)
        {
            var entities = await Uow.DbContext.Set<TEntity>().Where(predicate).ToListAsync();

            foreach (var entity in entities)
            {
                await DeleteAsync(entity, softDelete);
            }

            return entities.Count;
        }

        public virtual async Task<bool> ExistsAsync(object primaryKey)
        {
            return await Uow.DbContext.Set<TEntity>().FindAsync(primaryKey) != null;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Uow.DbContext.Set<TEntity>().AnyAsync(predicate);
        }

        public virtual async Task<TEntity> FindAsync(object primaryKey)
        {
            return await Uow.DbContext.Set<TEntity>().FindAsync(primaryKey);
        }
    }
}
