// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     LinqExtension.cs
// Created Date:  2018/11/03 11:20 PM
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiaB.Core.Data.Entities;
using DiaB.Core.Web.Models;
using DiaB.Core.Common.Extensions;
using DiaB.Core.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;

namespace DiaB.Core.Data.Extensions
{
    public static class LinqExtension
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int skip, int take, string key, string order, out int total) where T : class
        {
            total = query.Count();

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(order))
            {
                query = query.OrderBy($"{key} {order}".Trim()).Skip(skip).Take(take);
            }

            return query;
        }

        public static async Task<BootstrapTableResponseDto<TRow>> Paginate<TEntity, TRow>(this IQueryable<TEntity> query,
                                                                                          BootstrapTableRequestDto request) where TEntity : class where TRow : class
        {
            var response = new BootstrapTableResponseDto<TRow>
            {
                Total = await query.CountAsync(),
                Rows = default,
                Data = default
            };

            if (!string.IsNullOrWhiteSpace(request.SortKey) && !string.IsNullOrWhiteSpace(request.SortDirection))
            {
                query = query.OrderBy($"{request.SortKey} {request.SortDirection}".Trim());
            }

            query = query.Skip(request.Offset).Take(request.Limit);

            response.Rows = await query.ProjectTo<TRow>().ToListAsync();

            return response;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string sort, bool isDescending = false)
        {
            return sort != null ? source.OrderBy($"{sort} {(isDescending ? "DESC" : "ASC")}".Trim()) : source;
        }

        public static List<T> SortBy<T>(this List<T> source, string sort, bool isDescending = false)
        {
            return source.AsQueryable().SortBy(sort, isDescending).ToList();
        }

        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> queryable, string searchKey, bool exactMatch)
        {
            if (string.IsNullOrEmpty(searchKey))
            {
                return queryable;
            }

            searchKey = searchKey.Trim();

            var parameter = Expression.Parameter(typeof(T), "c");
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var publicProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.PropertyType == typeof(string));
            var searchKeyParts = exactMatch ? new[] { searchKey } : searchKey.Split(' ');
            Expression orExpressions = null;

            foreach (var property in publicProperties)
            {
                Expression nameProperty = Expression.Property(parameter, property);
                foreach (var searchKeyPart in searchKeyParts)
                {
                    Expression searchKeyExpression = Expression.Constant(searchKeyPart);
                    Expression callContainsMethod = Expression.Call(nameProperty, containsMethod, searchKeyExpression);
                    orExpressions = orExpressions == null ? callContainsMethod : Expression.Or(orExpressions, callContainsMethod);
                }
            }

            var whereCallExpression = Expression.Call(typeof(Queryable),
                                                      "WHERE",
                                                      new[] { queryable.ElementType },
                                                      queryable.Expression,
                                                      Expression.Lambda<Func<T, bool>>(orExpressions, parameter));

            return queryable.Provider.CreateQuery<T>(whereCallExpression);
        }

        public static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade.GetService<IRawSqlCommandBuilder>()
                                                  .Build(sql, parameters);

                return rawSqlCommand
                  .RelationalCommand
                  .ExecuteReader(new RelationalCommandParameterObject(connection: databaseFacade.GetService<IRelationalConnection>(), parameterValues: rawSqlCommand.ParameterValues, null, null, null));
            }
        }

        public static IEnumerable<T> ExecuteSqlQuery<T>(this DatabaseFacade databaseFacade, string sql, IMapper mapper, params object[] parameters)
        {
            using (var reader = ExecuteSqlQuery(databaseFacade, sql, parameters).DbDataReader)
            {
                var data = mapper.Map<IDataReader, IEnumerable<T>>(reader);

                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                return data;
            }
        }

        public static async Task<RelationalDataReader> ExecuteSqlQueryAsync(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade.GetService<IRawSqlCommandBuilder>()
                                                  .Build(sql, parameters);

                return await rawSqlCommand
                    .RelationalCommand
                    .ExecuteReaderAsync(new RelationalCommandParameterObject(connection: databaseFacade.GetService<IRelationalConnection>(), parameterValues: rawSqlCommand.ParameterValues, null, null, null));
            }
        }

        public static async Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(this DatabaseFacade databaseFacade, string sql, IMapper mapper, params object[] parameters)
        {
            using (var reader = (await ExecuteSqlQueryAsync(databaseFacade, sql, parameters)).DbDataReader)
            {
                var data = mapper.Map<IDataReader, IEnumerable<T>>(reader);

                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                return data;
            }
        }

        public static DataBuilder<TEntity> HasData<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder) where TEntity : BaseEnumEntity<TEnum>
                                                                                                            where TEnum : struct, IConvertible
        {
            var data = Enum.GetValues(typeof(TEnum))
                           .Cast<TEnum>()
                           .Select(element =>
                                   {
                                       var item = Activator.CreateInstance<TEntity>();
                                       item.Id = element;
                                       item.Name = (element as Enum)?.ToString();
                                       item.Description = (element as Enum).ToDescription();
                                       return item;
                                   });

            return builder.HasData(data);
        }

        public static IQueryable<T> ProjectTo<T>(this IQueryable query)
        {
            return query.ProjectTo<T>(IoCHelper.GetInstance<IConfigurationProvider>());
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, object parameter, Expression<Func<T, bool>> predicate)
        {
            if (parameter != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }
    }
}
