// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     IBaseRepository.cs
// Created Date:  2018/10/15 6:42 PM
// ------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiaB.Core.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        DbSet<TEntity> GetAll(bool isAsNoTracking = true);
        IQueryable<TEntity> GetAll(bool isAsNoTracking, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);
        IQueryable<TEntity> ExecuteSqlQuery(string query);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(object key, bool softDelete = true);
        Task DeleteAsync(TEntity entity, bool softDelete = true);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool softDelete = true);
        Task<bool> ExistsAsync(object key);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(object key);
    }
}
