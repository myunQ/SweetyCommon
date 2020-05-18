/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      DateTime 类型扩展。
 * 
 * Members Index:
 *      static class DateTimeExtensions
 *          DateTime UNIX_MIN_TIME
 *          DateTime FirstDayOfMonth(this DateTime datetime)
 *          DateTime LastDayOfMonth(this DateTime datetime)
 *          long NowUnixTimestamp { get; }
 *          long UnixTimestamp(this DateTime datetime)
 *          long UnixTimestamp(this DateTime datetime, long offsetSeconds)
 *          long UnixTimestamp(this DateTime datetime, TimeSpan offsetTime)
 *      
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Extensions
{
    using System;


    /// <summary>
    /// <see cref="DateTime"/> 类型的扩展方法。
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 表示一天中的秒数。
        /// </summary>
        public const long SecondsPerDay = 86400;
        /// <summary>
        /// 表示 1 小时的秒数。
        /// </summary>
        public const long SecondsPerHour = 3600;
        /// <summary>
        /// 表示 1 分钟的秒数。
        /// </summary>
        public const long SecondsPerMinute = 60;
        /// <summary>
        /// 表示协调世界时（UTC）1970-01-01 00:00:00。
        /// </summary>
        public static readonly DateTime UNIX_MIN_TIME = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 表示当前日期时间的 <c>Unix</c> 时间戳。
        /// </summary>
        public static long NowUnixTimestamp
        {
            get
            {
                return DateTime.UtcNow.Subtract(UNIX_MIN_TIME).Ticks / TimeSpan.TicksPerSecond;
            }
        }

        /// <summary>
        /// 获取月份的第一天零点整的日期时间对象。
        /// </summary>
        /// <param name="datetime">日期时间。</param>
        /// <returns>返回当前 <paramref name="datetime"/> 参数所表示日期的当月第一天零点整的日期时间对象。</returns>
        public static DateTime FirstDayOfMonth(this DateTime datetime)
        {
            return datetime.Date.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 获取月份的最后一天零点整的日期时间对象。
        /// </summary>
        /// <param name="datetime">日期时间。</param>
        /// <returns>返回当前 <paramref name="datetime"/> 参数所表示日期的当月最后一天零点整的日期时间对象。</returns>
        public static DateTime LastDayOfMonth(this DateTime datetime)
        {
            int year = datetime.Year;
            int month = datetime.Month;
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 0, 0, 0, datetime.Kind);
        }

        /// <summary>
        /// 获取 <paramref name="datetime"/> 参数的 <c>Unix</c> 时间戳。
        /// </summary>
        /// <param name="datetime">日期时间，如果日期时间早于。</param>
        /// <returns><c>UTC</c> 时间1970年1月1日零点整 与 <paramref name="datetime"/> 参数表示的日期时间相差的秒数。</returns>
        public static long UnixTimestamp(this DateTime datetime)
        {
            if (datetime.Kind != DateTimeKind.Utc)
            {
                return datetime.ToUniversalTime().Subtract(UNIX_MIN_TIME).Ticks / TimeSpan.TicksPerSecond;
            }

            return datetime.Subtract(UNIX_MIN_TIME).Ticks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// 获取 <paramref name="datetime"/> 参数的 <c>Unix</c> 时间戳。
        /// </summary>
        /// <param name="datetime">日期时间，如果日期时间早于。</param>
        /// <param name="offsetSeconds">偏移秒数。</param>
        /// <returns><c>UTC</c> 时间1970年1月1日零点整 与 <paramref name="datetime"/> 参数表示的日期时间相差的秒数。</returns>
        public static long UnixTimestamp(this DateTime datetime, long offsetSeconds)
        {
            return UnixTimestamp(datetime) + offsetSeconds;
        }

        /// <summary>
        /// 获取 <paramref name="datetime"/> 参数的 <c>Unix</c> 时间戳。
        /// </summary>
        /// <param name="datetime">日期时间，如果日期时间早于。</param>
        /// <param name="offsetTime">偏移时间。</param>
        /// <returns><c>UTC</c> 时间1970年1月1日零点整 与 <paramref name="datetime"/> 参数表示的日期时间相差的秒数。</returns>
        public static long UnixTimestamp(this DateTime datetime, TimeSpan offsetTime)
        {
            return UnixTimestamp(datetime) + (long)offsetTime.TotalSeconds;
        }

        /// <summary>
        /// 获取月份属于哪个季度。
        /// </summary>
        /// <param name="date">日期。</param>
        /// <returns>月份所属季度。</returns>
        public static int GetQuarter(this DateTime date)
        {
            var q = date.Month / 3f;
            int result = Convert.ToInt32(q);

            if (q > result) result++;

            return result;
        }
    }
}
