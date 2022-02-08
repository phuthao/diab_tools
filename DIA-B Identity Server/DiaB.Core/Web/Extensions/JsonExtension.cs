// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     JsonExtension.cs
// Created Date:  2018/10/19 2:54 PM
// ------------------------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiaB.Core.Web.Extensions
{
    public static class JsonExtension
    {
        public static T ToObject<T>(this string json, JsonSerializerSettings settings = default)
        {
            try
            {
                return json.IsValidJson() ? JsonConvert.DeserializeObject<T>(json, settings) : default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string ToJson(this object obj, JsonSerializerSettings settings = default)
        {
            return obj != null ? JsonConvert.SerializeObject(obj, settings) : null;
        }

        public static bool IsValidJson(this string json)
        {
            json = json.Trim();
            if (json.StartsWith("{") && json.EndsWith("}") || json.StartsWith("[") && json.EndsWith("]"))
            {
                try
                {
                    JToken.Parse(json);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
