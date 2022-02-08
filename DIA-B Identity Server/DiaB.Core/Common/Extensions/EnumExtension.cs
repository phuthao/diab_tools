// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     EnumExtension.cs
// Created Date:  2019/03/24 10:29 PM
// ------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;

namespace DiaB.Core.Common.Extensions
{
    public static class EnumExtension
    {
        public static string ToDescription(this Enum value)
        {
            return value.GetType()
                        .GetField(value.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .Cast<DescriptionAttribute>()
                        .FirstOrDefault()
                        ?.Description;
        }
    }
}
