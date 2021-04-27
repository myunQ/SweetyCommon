/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      Int 类型扩展。
 * 
 * Members Index:
 *      static class LongExtensions
 *          string ToString(this int i, int radix)
 *          bool TryToDateTime(this int value, out DateTime dt)
 *          bool TryToDateTime(this int value, DateTimeKind kind, out DateTime dt)
 *      
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Extensions
{
    using System;


    /// <summary>
    /// <see cref="Int32"/> 类型的扩展方法。
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// 将 <c>int</c> 值转换成指定进制表示的字符串。
        /// </summary>
        /// <seealso cref="LongExtensions.ToString(long, long)"/>
        /// <param name="i">数值</param>
        /// <param name="radix">指定进制，取值范围 2~66。</param>
        /// <returns>指定进制表示法。</returns>
        public static string ToString(this int i, int radix)
        {
            return LongExtensions.ToString(i, radix);
        }


        /// <summary>
        /// 尝试将<see cref="int"/>类型表示的日期时间转换成<see cref="DateTime"/>类型。
        /// </summary>
        /// <param name="value">支持的日期格式：yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH。</param>
        /// <param name="dt">转换出的值。</param>
        /// <returns>转换成功返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool TryToDateTime(this int value, out DateTime dt)
        {
            return TryToDateTime(value, DateTimeKind.Unspecified, out dt);
        }

        /// <summary>
        /// 尝试将<see cref="int"/>类型表示的日期时间转换成<see cref="DateTime"/>类型。
        /// </summary>
        /// <param name="value">支持的日期格式：yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH。</param>
        /// <param name="kind">指示指示日期时间种类。</param>
        /// <param name="dt">转换出的值。</param>
        /// <returns>转换成功返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool TryToDateTime(this int value, DateTimeKind kind, out DateTime dt)
        {
#if NET5_0_OR_GREATER
            if (value is < 1000 or > 9999 and < 100001 or > 999912 and < 10000101 or > 99991231 and < 1000010100 or > 2147123123)
#else
            if (value < 1000 || (value > 9999 && value < 100001) || (value > 999912 && value < 10000101) || (value > 99991231 && value < 1000010100) || value > 2147123123)
#endif
            {
                dt = default;
                return false;
            }

            int year;
            int month = 1;
            int day = 1;
            int hour = 0;

            if (value < 9999)
            {
                year = value;
            }
            else if (value < 999912)
            {
                year = value / 100;
                month = value - year * 100;
            }
            else if (value < 99991231)
            {
                year = value / 10000;
                int r = (value - year * 10000);
                month = r / 100;
                day = r - month * 100;
            }
            else
            {
                year = value / 1000000;
                int r = (value - year * 1000000);
                month = r / 10000;
                r -= month * 10000;
                day = r / 100;
                hour = r - day * 100;
            }

#if NET5_0_OR_GREATER
            if (month is < 1 or > 12 || day is < 1 or > 31 || hour is < 0 or > 23
#else
            if (month < 1 || month > 12 || day < 1 || day > 31 || hour < 0 || hour > 23
#endif
                || (month == 2 && (day > 29 || (day == 29 && !DateTime.IsLeapYear(year))))
                || (day == 31 && ((month & 9) == 0 || (month & 9) == 9)))
            //(month & 9) 等于 0 是 2、4、6月，等于 9 是 9 月 11 月。
            {
                dt = default;
                return false;
            };

            dt = new DateTime(year, month, day, hour, 0, 0, kind);

            return true;
        }
    }
}
