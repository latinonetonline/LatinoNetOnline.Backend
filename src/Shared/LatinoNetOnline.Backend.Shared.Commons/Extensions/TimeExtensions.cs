using System;

namespace LatinoNetOnline.Backend.Shared.Commons.Extensions
{
    public static class TimeExtensions
    {
        public static bool IsWithin24Hours(this TimeSpan time) => time >= new TimeSpan(0, 0, 0) && time <= new TimeSpan(23, 59, 59);

        public static bool IsWithin24Hours(this DateTime dateTime) => dateTime.TimeOfDay.IsWithin24Hours();
    }
}
