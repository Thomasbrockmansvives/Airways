using System;

namespace Airways.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToDateTime(this DateOnly date, TimeOnly time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }
    }
}