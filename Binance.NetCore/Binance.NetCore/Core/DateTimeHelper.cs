using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Core
{
    public class DateTimeHelper
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long epochTicks = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;

        /// <summary>
        /// Constructor/Destructor
        /// </summary>
        public DateTimeHelper()
        {
        }

        /// <summary>
        /// Convert unix timestamp to UTC DateTime
        /// </summary>
        /// <param name="unixTime">Unix Timestamp</param>
        /// <returns>DateTime object</returns>
        public DateTime UnixTimeToUTC(long unixTime)
        {
            var result = epoch.AddSeconds(unixTime);

            return result;
        }

        /// <summary>
        /// Convert string of unix timestamp to UTC DateTime
        /// </summary>
        /// <param name="unixTime">string of Unix Timestamp</param>
        /// <returns>DateTime object</returns>
        public DateTime? UnixTimeToUTC(string unixTime)
        {
            if (string.IsNullOrEmpty(unixTime))
                return null;

            long unixTimeInt = Int64.Parse(unixTime);

            var result = UnixTimeToUTC(unixTimeInt);

            return result;
        }

        /// <summary>
        /// Convert unix timestamp to Local DateTime
        /// </summary>
        /// <param name="unixTime">Unix Timestamp</param>
        /// <returns>DateTime object</returns>
        public DateTime UnixTimeToLocal(long unixTime)
        {
            var result = epoch.AddSeconds(unixTime);

            return result.ToLocalTime();
        }

        /// <summary>
        /// Convert Local DateTime to unix timestamp
        /// </summary>
        /// <param name="localTime">Local DateTime object</param>
        /// <returns>unix timestamp</returns>
        public long LocalToUnixTime(DateTime localTime)
        {
            var utcTime = localTime.ToUniversalTime();

            return UTCtoUnixTime(utcTime);
        }

        /// <summary>
        /// Convert current UTC DateTime to unix timestamp
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTime()
        {
            return UTCtoUnixTime(DateTime.UtcNow);
        }

        /// <summary>
        /// Convert end of current minute to unix timestamp
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long UTCEndOfMinuteToUnixTime()
        {
            var roundedTime = RoundUp(DateTime.UtcNow, TimeSpan.FromMinutes(1));

            return UTCtoUnixTime(roundedTime);
        }

        /// <summary>
        /// Round time up
        /// </summary>
        /// <param name="dateTime">Current time</param>
        /// <param name="timeSpan">Time span to round up to</param>
        /// <returns>Rouned up time</returns>
        public DateTime RoundUp(DateTime dateTime, TimeSpan timeSpan)
        {
            return new DateTime((dateTime.Ticks + timeSpan.Ticks - 1) / timeSpan.Ticks * timeSpan.Ticks, dateTime.Kind);
        }

        /// <summary>
        /// Convert current UTC DateTime to unix timestamp milliseconds included
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTimeMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Convert current UTC DateTime to unix timestamp milliseconds included
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long LocalTimetoUnixTimeMilliseconds(DateTimeOffset dateTime)
        {
            return dateTime.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Convert UTC DateTime to unix timestamp
        /// </summary>
        /// <param name="localTime">UTC DateTime object</param>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTime(DateTime utcTimestamp)
        {
            return ((utcTimestamp.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Get seconds from a timestamp to now
        /// </summary>
        /// <param name="timeStart">DateTime to compare</param>
        /// <returns>Double of seconds</returns>
        public double CompareSeconds(DateTime timeStart)
        {
            var timeNow = DateTime.UtcNow;

            return (timeStart - timeNow).TotalSeconds;
        }

        /// <summary>
        /// Subtract hours, minutes, seconds from current UTC Time
        /// </summary>
        /// <param name="hours">Hours to subtract (optional)</param>
        /// <param name="minutes">Minutes to subtract (optional)</param>
        /// <param name="seconds">Seconds to subtract (optional)</param>
        /// <returns>Resulting DateTime value</returns>
        public DateTime SubtractFromUTCNow(int hours =0, int minutes = 0, int seconds = 0)
        {
            return SubtractTime(DateTime.UtcNow, hours, minutes, seconds);
        }

        /// <summary>
        /// Subtract hours, minutes, seconds from a time value
        /// </summary>
        /// <param name="now">Time To Subract from</param>
        /// <param name="hours">Hours to subtract (optional)</param>
        /// <param name="minutes">Minutes to subtract (optional)</param>
        /// <param name="seconds">Seconds to subtract (optional)</param>
        /// <returns>Resulting DateTime value</returns>
        public DateTime SubtractTime(DateTime now, int hours = 0, int minutes = 0, int seconds = 0)
        {
            var timeSpan = new TimeSpan(hours, minutes, seconds);

            return now.Subtract(timeSpan);
        }

        /// <summary>
        /// Subtract hours, minutes, seconds from current UTC Time
        /// </summary>
        /// <param name="hours">Hours to subtract (optional)</param>
        /// <param name="minutes">Minutes to subtract (optional)</param>
        /// <param name="seconds">Seconds to subtract (optional)</param>
        /// <returns>Resulting DateTimeOffset value</returns>
        public DateTimeOffset SubtractFromOffsetUTCNow(int hours = 0, int minutes = 0, int seconds = 0)
        {
            return SubtractOffsetTime(DateTimeOffset.UtcNow, hours, minutes, seconds);
        }

        /// <summary>
        /// Subtract hours, minutes, seconds from a time value
        /// </summary>
        /// <param name="now">Time To Subract from</param>
        /// <param name="hours">Hours to subtract (optional)</param>
        /// <param name="minutes">Minutes to subtract (optional)</param>
        /// <param name="seconds">Seconds to subtract (optional)</param>
        /// <returns>Resulting DateTimeOffset value</returns>
        public DateTimeOffset SubtractOffsetTime(DateTimeOffset now, int hours = 0, int minutes = 0, int seconds = 0)
        {
            var timeSpan = new TimeSpan(hours, minutes, seconds);

            return now.Subtract(timeSpan);
        }
    }
}
