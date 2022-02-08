// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     TransactionalAttribute.cs
// Created Date:  2018/10/10 12:32 PM
// ------------------------------------------------------------------------

using DiaB.Core.Data.Uow;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using DiaB.Core.Common.Extensions;

namespace DiaB.Core.Web.Attributes
{
    public class TransactionalAttribute : ActionFilterAttribute
    {
        public Type DbContextType { get; }

        public TransactionalAttribute(Type dbContextType)
        {
            DbContextType = dbContextType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.RequestServices
                .GetRequiredService(typeof(IUowRepository<>).MakeGenericType(DbContextType))
                .Invoke(nameof(IUowRepository<DbContext>.BeginTransaction));

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.RequestServices
                .GetRequiredService(typeof(IUowRepository<>).MakeGenericType(DbContextType))
                .Invoke(nameof(IUowRepository<DbContext>.CommitTransaction), context.Exception);
        }
    }
}
