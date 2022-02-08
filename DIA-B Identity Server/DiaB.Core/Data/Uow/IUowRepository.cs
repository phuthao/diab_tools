// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     IUowRepository.cs
// Created Date:  2018/10/15 6:42 PM
// ------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;

namespace DiaB.Core.Data.Uow
{
    public interface IUowRepository<TDbContext> : IDisposable where TDbContext : DbContext
    {
        TDbContext DbContext { get; set; }

        void BeginTransaction();

        void CommitTransaction(Exception exception);

        void SaveChanges();
    }
}
