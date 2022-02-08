// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     StringHelper.cs
// Created Date:  2018/10/12 3:00 PM
// ------------------------------------------------------------------------

using System.Linq;

namespace DiaB.Core.Common.Helpers
{
    public static class StringHelper
    {
        public static string Join(string separator, params string[] values)
        {
            return string.Join(separator, values.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}
