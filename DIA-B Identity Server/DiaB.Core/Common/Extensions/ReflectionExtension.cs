// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     ReflectionExtension.cs
// Created Date:  2018/10/22 12:08 PM
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace DiaB.Core.Common.Extensions
{
    public static class ReflectionExtension
    {
        public static object GetValue(this object obj, string property)
        {
            try
            {
                return obj.GetType().GetProperty(property)?.GetValue(obj, null);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Invoke(this object obj, string method, params object[] agrs)
        {
            obj.GetType().GetMethod(method)?.Invoke(obj, agrs);
        }

        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            return (from x in obj.GetType().GetProperties() where !x.GetCustomAttributes<JsonIgnoreAttribute>().Any() select x).ToDictionary(x => x.GetCustomAttributes<JsonPropertyAttribute>().Any()
                                                                                                                                                      ? x.GetCustomAttributes<JsonPropertyAttribute>().SingleOrDefault()?.PropertyName
                                                                                                                                                      : x.Name,
                                                                                                                                             x => x.GetGetMethod().Invoke(obj, null) == null
                                                                                                                                                      ? ""
                                                                                                                                                      : x.GetGetMethod().Invoke(obj, null).ToString());
        }

        //public static TDestination Map<TDestination>(this object obj, Action<IMapperConfigurationExpression> action = null)
        //{
        //    var configuration = new MapperConfiguration(cfg =>
        //                                                {
        //                                                    cfg.CreateMap(obj.GetType(), typeof(TDestination));
        //                                                    action?.Invoke(cfg);
        //                                                });

        //    return configuration.CreateMapper().Map<TDestination>(obj);
        //}

        public static bool IsSubclassOfGenericType(this Type type, Type genericType)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (genericType == cur)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }
}
