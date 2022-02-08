// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     UowRepository.cs
// Created Date:  2018/10/15 6:42 PM
// ------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DiaB.Core.Data.Uow
{
    public sealed class UowRepository<TDbContext> : IUowRepository<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _dbContext;

        private IDbContextTransaction _transaction;

        private bool _disposed;

        public TDbContext DbContext
        {
            get => _dbContext;
            set => _dbContext = value ?? throw new ArgumentNullException(nameof(value));
        }

        public UowRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_dbContext != null)
                    {
                        _dbContext.Dispose();
                        _dbContext = null;
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction(Exception exception)
        {
            try
            {
                if (exception == null)
                {
                    SaveChanges();
                    _transaction.Commit();
                }
                else
                {
                    _transaction.Rollback();
                }
            }
            catch (Exception)
            {
                _transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
