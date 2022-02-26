/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      Long 类型扩展。
 * 
 * Members Index:
 *      static class LongExtensions
 *          DateTime FromUnixTimestamp(this long timestamp)
 *          DateTime FromUnixTimestamp(this long timestamp, TimeZoneInfo destinationTimeZone)
 *          string ToString(this long i, int radix)
 *          bool TryToDateTime(this long value, out DateTime dt)
 *          bool TryToDateTime(this long value, DateTimeKind kind, out DateTime dt)
 *      
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Extensions
{
    using System;


    /// <summary>
    /// <see cref="Int64"/> 类型的扩展方法。
    /// </summary>
    public static class LongExtensions
    {
        /// <summary>
        /// 表示 66 进制的字符集，这是一个 <c>URI</c> 安全的字符集。
        /// </summary>
        internal static readonly char[] URI_SAFE_CHARS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_', '$', '!' };

        /// <summary>
        /// 将 <c>Unix</c> 时间戳 转换成 <see cref="DateTime"/> 类型。
        /// </summary>
        /// <param name="timestamp">时间戳。</param>
        /// <param name="kind"><see cref="DateTime.Kind"/></param>
        /// <returns>时间戳所表示的 <see cref="DateTime"/> 值。</returns>
        public static DateTime FromUnixTimestamp(this long timestamp, DateTimeKind kind = DateTimeKind.Local)
        {
            if (kind == DateTimeKind.Local)
            {
                return FromUnixTimestamp(timestamp, TimeZoneInfo.Local);
            }
            else
            {
                return DateTimeExtensions.UNIX_MIN_TIME.AddSeconds(timestamp);
            }
        }

        /// <summary>
        /// 获取 <c>Unix</c> 时间戳 所表示的指定时区时间。
        /// </summary>
        /// <param name="timestamp">时间戳。</param>
        /// <param name="destinationTimeZone">目标时区。</param>
        /// <returns>目标时区。</returns>
        public static DateTime FromUnixTimestamp(this long timestamp, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTimeExtensions.UNIX_MIN_TIME.AddSeconds(timestamp), destinationTimeZone);
        }

        /// <summary>
        /// 将 <c>long</c> 值转换成指定进制表示的字符串。
        /// </summary>
        /// <param name="i">数值。</param>
        /// <param name="radix">指定进制，取值范围 2~66。</param>
        /// <returns>指定进制表示法。</returns>
        public static string ToString(this long i, long radix)
        {
            if (radix == 10L) i.ToString();
            if (radix < 2L || radix > URI_SAFE_CHARS.Length) throw new ArgumentOutOfRangeException(nameof(radix), radix, String.Format(Properties.Localization.the_value_ranges_from_XXX_to_XXX, 2, URI_SAFE_CHARS.Length));

#if NETSTANDARD2_0
            char[] buf = new char[65];
#else
            Span<char> buf = stackalloc char[65]; //为什么是65：因为long类型是64bit的数据类型，其值用二进制表示时相比其它进制表示需要的字符数最多，因此64加一个符号字符就是65了。
#endif
            int charPos = 64;
            bool negative = (i < 0L);
            if (!negative)
            {
                i = -i;
            }

            while (i <= -radix)
            {
                buf[charPos--] = URI_SAFE_CHARS[(int)(-(i % radix))];
                i /= radix;
            }

            buf[charPos] = URI_SAFE_CHARS[(int)(-i)];
            if (negative)
            {
                buf[--charPos] = '-';
            }

#if NETSTANDARD2_0
            return new String(buf, charPos, (65 - charPos));
#else
            return new String(buf[charPos..^0]);
#endif
        }


        /// <summary>
        /// 尝试将<see cref="long"/>类型表示的日期时间转换成<see cref="DateTime"/>类型。
        /// </summary>
        /// <param name="value">支持的日期格式：yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH、yyyyMMddHHmm、yyyyMMddHHmmss、yyyyMMddHHmmssfff。</param>
        /// <param name="dt">转换出的值。</param>
        /// <returns>转换成功返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool TryToDateTime(this long value, out DateTime dt)
        {
            return TryToDateTime(value, DateTimeKind.Unspecified, out dt);
        }

        /// <summary>
        /// 尝试将<see cref="long"/>类型表示的日期时间转换成<see cref="DateTime"/>类型。
        /// </summary>
        /// <param name="value">支持的日期格式：yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH、yyyyMMddHHmm、yyyyMMddHHmmss、yyyyMMddHHmmssfff。</param>
        /// <param name="kind">指示指示日期时间种类。</param>
        /// <param name="dt">转换出的值。</param>
        /// <returns>转换成功返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool TryToDateTime(this long value, DateTimeKind kind, out DateTime dt)
        {
#if NET5_0_OR_GREATER
            if (value is < 1000L or > 9999L and < 100001L or > 999912L and < 10000101L or > 99991231L and < 1000010100L or > 9999123123L and < 100001010000 or > 999912312359 and < 10000101000000 or > 99991231235959 and < 10000101000000000 or > 99991231235959999)
#else
            if (value < 1000L || (value > 9999L && value < 100001L) || (value > 999912L && value < 10000101L) || (value > 99991231L && value < 1000010100L) || (value > 9999123123L && value < 100001010000) || (value > 999912312359 && value < 10000101000000) || (value > 99991231235959 && value < 10000101000000000) || value > 99991231235959999)
#endif
            {
                dt = default;
                return false;
            }

            int year;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisecond = 0;

            if (value < 9999L)
            {
                year = (int)value;
            }
            else if (value < 999912L)
            {
                year = (int)(value / 100L);
                month = (int)(value - year * 100);
            }
            else if (value < 99991231L)
            {
                year = (int)(value / 10000L);
                int r = (int)(value - year * 10000);
                month = r / 100;
                day = r - month * 100;
            }
            else if (value < 9999123123L)
            {
                year = (int)(value / 1000000L);
                int r = (int)(value - year * 1000000);
                month = r / 10000;
                r -= month * 10000;
                day = r / 100;
                hour = r - day * 100;
            }
            else if (value < 999912312359)
            {
                year = (int)(value / 100000000L);
                int r = (int)(value - year * 100000000);
                month = r / 1000000;
                r -= month * 1000000;
                day = r / 10000;
                r -= day * 10000;
                hour = r / 100;
                minute = r - hour * 100;
            }
            else if (value < 99991231235959)
            {
                year = (int)(value / 10000000000);
                int r = (int)(value - year * 10000000000);
                month = r / 100000000;
                r -= month * 100000000;
                day = r / 1000000;
                r -= day * 1000000;
                hour = r / 10000;
                r -= hour * 10000;
                minute = r / 100;
                second = r - minute * 100;
            }
            else
            {
                year = (int)(value / 10000000000000);
                long r = value - year * 10000000000000;
                month = (int)(r / 100000000000);
                r -= month * 100000000000;
                day = (int)(r / 1000000000L);
                int r2 = (int)(r - day * 1000000000L);
                hour = r2 / 10000000;
                r2 -= hour * 10000000;
                minute = r2 / 100000;
                r2 -= minute * 100000;
                second = r2 / 1000;
                millisecond = r2 - second * 1000;
            }

#if NET5_0_OR_GREATER
            if (month is < 1 or > 12 || day is < 1 or > 31 || hour is < 0 or > 23 || minute is < 0 or > 59 || second is < 0 or > 59 || millisecond is < 0 or > 999
#else
            if (month < 1 || month > 12 || day < 1 || day > 31 || hour < 0 || hour > 23 || minute < 0 || minute > 59 || second < 0 || second > 59 || millisecond < 0 || millisecond > 999
#endif
                || (month == 2 && (day > 29 || (day == 29 && !DateTime.IsLeapYear(year))))
                || (day == 31 && ((month & 9) == 0 || (month & 9) == 9)))
            //(month & 9) 等于 0 是 2、4、6月，等于 9 是 9 月 11 月。
            {
                dt = default;
                return false;
            };

            dt = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

            return true;
        }
    }
}
