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
 *          string ToString(this long i, int radix)
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
        /// 获取 <c>Unix</c> 时间戳 所表示的本地时间。
        /// </summary>
        /// <param name="timestamp">时间戳。</param>
        /// <returns>本地时间</returns>
        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTimeExtensions.UNIX_MIN_TIME.AddSeconds(timestamp), TimeZoneInfo.Local);
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
            return new String(buf.Slice(charPos, (65 - charPos)));
#endif
        }
    }
}
