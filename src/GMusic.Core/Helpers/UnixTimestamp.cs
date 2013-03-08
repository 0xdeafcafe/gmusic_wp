using System;

namespace GMusic.Core.Helpers
{
    public static class Time
    {
        public static Int64 CurrentUnixTimestamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (Int64)timeSpan.TotalSeconds;
        }
        public static DateTime CurrentDateTime()
        {
            return DateTime.Now;
        }

        public static class Helpers
        {
            public static bool HasPassed(Int64 unixTimestamp)
            {
                return HasPassed(Convert.ToDateTime(unixTimestamp));
            }
            public static bool HasPassed(string unixTimestamp)
            {
                return HasPassed(Convert.ToDateTime(unixTimestamp));
            }
            public static bool HasPassed(DateTime date)
            {
                return date < CurrentDateTime();
            }
        }

        public static class Convert
        {
            public static DateTime ToDateTime(Int64 unixTimestamp)
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return origin.AddSeconds(unixTimestamp);
            }
            public static DateTime ToDateTime(string unixTimestamp)
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return origin.AddSeconds(System.Convert.ToInt64(unixTimestamp));
            }

            public static Int64 ToUnixTimestamp(DateTime date)
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var diff = date - origin;
                return (Int64)Math.Floor(diff.TotalSeconds);
            }
        }
    }
}