using DiaB.Common.Constants;
using DiaB.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiaB.Common.Extensions
{
    public static class NumberExtension
    {
        public static IList<DayOfWeeks> FormatDayOfWeeks(string days)
        {
            var dayOfWeeks = new List<DayOfWeeks> { };

            if (!string.IsNullOrEmpty(days))
            {
                var splitDays = days.Split(';');

                if (splitDays.Any())
                {
                    foreach (var day in splitDays)
                    {
                        if (!string.IsNullOrEmpty(day))
                        {
                            dayOfWeeks.Add((DayOfWeeks)Convert.ToInt32(day));
                        }
                    }
                }
            }

            return dayOfWeeks;
        }
    }
}
