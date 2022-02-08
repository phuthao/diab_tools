// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     StringExtension.cs
// Created Date:  2018/10/22 12:08 PM
// ------------------------------------------------------------------------

using System;

namespace DiaB.Core.Common.Extensions
{
    public static class StringExtension
    {
        private static readonly string[] VietnameseSigns =
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string ToUnsign(this string input)
        {
            for (var i = 1; i < VietnameseSigns.Length; i++)
            {
                for (var j = 0; j < VietnameseSigns[i].Length; j++)
                {
                    input = input.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }

            return input;
        }

        public static string TrimLowerUnsign(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : input.Trim().ToLower().ToUnsign();
        }

        public static bool EqualsIgnoreCase(this string s1, string s2)
        {
            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        public static string CanonicalizeUrlPath(this string urlPath)
        {
            return urlPath.Trim().TrimEnd('/').Replace(" ", "").ToLower();
        }

        public static int ToInt(this string input)
        {
            int.TryParse(input, out var output);

            return output;
        }

        public static long ToLong(this string input)
        {
            long.TryParse(input, out var output);

            return output;
        }

        public static double ToDouble(this string input)
        {
            double.TryParse(input, out var output);

            return output;
        }

        public static Guid ToGuid(this string input)
        {
            return new Guid(input);
        }

        public static string AntiNullText(this string input, string defaultValue = "-")
        {
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }

        public static string TrimIfNotNull(this string input)
        {
            return string.IsNullOrEmpty(input) ? input : input.Trim();
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNumeric(this string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
