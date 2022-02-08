// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     DateTimeExtension.cs
// Created Date:  2018/10/22 12:09 PM
// ------------------------------------------------------------------------

using System;
using System.Globalization;

namespace DiaB.Core.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime? ToDate(this string input, string dateFormat = "dd/MM/yyyy")
        {
            var check = DateTime.TryParseExact(input,
                                               dateFormat,
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out var output);

            if (check)
            {
                return output;
            }

            return null;
        }

        public static DateTime TrimTime(this DateTime inputDateTime)
        {
            return inputDateTime.Date;
        }

        public static DateTime TrimDate(this DateTime inputDateTime)
        {
            return default(DateTime).Add(inputDateTime.TimeOfDay);
        }

        public static string FormatDate(this DateTime inputDateTime, string dateFormat = "dd/MM/yyyy")
        {
            try
            {
                return inputDateTime.ToString(dateFormat);
            }
            catch (FormatException)
            {
            }

            return null;
        }
    }
}
