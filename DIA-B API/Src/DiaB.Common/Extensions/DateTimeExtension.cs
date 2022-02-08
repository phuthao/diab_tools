using System;
using System.Globalization;
using DiaB.Common.Enums;

namespace DiaB.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static int? GetAge(this DateTime? birthDate)
        {
           return birthDate.GetAge(DateTime.Now);
        }

        public static int GetQuarterMonth(this DateTime dateTime)
        {
            var quarterGroupNo = (dateTime.Month - 1) / 3;

            return quarterGroupNo switch
            {
                0 => 3,
                1 => 6,
                2 => 9,
                3 => 12,
                _ => throw new ArgumentOutOfRangeException(quarterGroupNo.ToString()),
            };
        }

        public static (DateTime fdow, DateTime ldow) GetDateRangeOfWeek(this DateTime currentDateTime)
        {
            int thisWeekNumber = GetIso8601WeekOfYear(currentDateTime);
            DateTime firstDayOfWeek = FirstDateOfWeek(currentDateTime.Year, thisWeekNumber, CultureInfo.InvariantCulture);

            return (firstDayOfWeek, firstDayOfWeek.AddDays(6));
        }

        private static int? GetAge(this DateTime? birthDate, DateTime laterDate)
        {
            if (birthDate == null)
            {
                return null;
            }

            var age = laterDate.Year - birthDate.Value.Year;

            return age > 0
                ? age - Convert.ToInt32(laterDate.Date < birthDate.Value.Date.AddYears(age))
                : 0;
        }

        private static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private static DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
    }
}
