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
        /// <seealso cref="LongExtensions.ToString(this long, int)"/>
        /// <param name="i">数值</param>
        /// <param name="radix">指定进制，取值范围 2~66。</param>
        /// <returns>指定进制表示法。</returns>
        public static string ToString(this int i, int radix)
        {
            return LongExtensions.ToString(i, radix);
        }
    }
}
