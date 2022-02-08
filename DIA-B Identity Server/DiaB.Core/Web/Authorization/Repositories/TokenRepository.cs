// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     TokenRepository.cs
// Created Date:  2019/07/09 12:14 PM
// ------------------------------------------------------------------------

using DiaB.Core.Data.Repositories;
using DiaB.Core.Data.Uow;
using DiaB.Core.Web.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiaB.Core.Web.Authorization.Repositories
{
    public class TokenRepository : BaseRepository<Token, DbContext>
    {
        public TokenRepository(IUowRepository<DbContext> uow) : base(uow)
        {
        }
    }
}
