using DiaB.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiaB.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsMatch(this string input, string pattern)
        {
            return IsMatch(input, pattern, null);
        }

        public static bool IsMatch(this string input, string pattern, RegexOptions? options)
        {
            return options == null ? Regex.IsMatch(input, pattern) : Regex.IsMatch(input, pattern, options.Value);
        }

        public static string GetFirstName(this string fullname)
        {
            var words = fullname.Split(' ');

            return words.Length > 0 ? words.LastOrDefault() : string.Empty;
        }

        public static string GetLastName(this string fullname)
        {
            var words = fullname.Split(' ');

            return words.Length > 1 ? words.FirstOrDefault() : string.Empty;
        }

        public static string GetMiddleName(this string fullname)
        {
            var words = fullname.Split(' ');

            return words.Length > 2 ? string.Join(' ', words.Skip(1).Take(words.Length - 2)) : string.Empty;
        }

        public static string GetFullname(string lastName, string middleName, string firstName)
        {
            return ((!string.IsNullOrEmpty(lastName) ? lastName.Trim() + " " : string.Empty) +
                   (!string.IsNullOrEmpty(middleName) ? middleName.Trim() + " " : string.Empty) +
                   (!string.IsNullOrEmpty(firstName) ? firstName.Trim() + " " : string.Empty)).Trim();
        }

        public static IList<Guid> ConvertGuidIds(this string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new List<Guid>();
            }

            var guidIds = new List<Guid>();

            foreach (var id in ids.Split(CommonConstant.SemiColon))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    guidIds.Add(Guid.Parse(id));
                }
            }

            return guidIds;
        }

        public static bool IsGuid(this string id)
        {
            Guid x;
            return Guid.TryParse(id, out x);
        }

        public static string RemoveSign4VietnameseString(this string str)
        {
            for (var i = 1; i < VietnameseSigns.Length; i++)
            {
                for (var j = 0; j < VietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }

            return str;
        }

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
            "ÝỲỴỶỸ",
        };
    }
}
