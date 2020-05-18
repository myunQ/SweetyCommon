/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      Char 类型扩展。
 * 
 * Members Index:
 *      static class CharExtensions
 *          char ToSBC(this char c)
 *          char ToDBC(this char c)
 *          
 *      
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Extensions
{
    using System;


    /// <summary>
    /// <see cref="Char"/> 以及 <see cref="Char[]"/> 类型的扩展方法。
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// 全角空格。
        /// </summary>
        public const char DBC_SPACE = '\x3000';
        /// <summary>
        /// 某个全角字符与它对应的半角字符的编码差。
        /// </summary>
        public const int DBC_SUB_SBC = 65248;


        /// <summary>
        /// 将全角字符转换成半角字符。
        /// </summary>
        /// <remarks>
        /// 只对 <c>Ascii</c> 码在 32~126 之间的字符进行全/半角转换操作。
        /// </remarks>
        /// <param name="c">要转换的全角字符，如果没有对应的半角字符则直接返回。</param>
        /// <returns>返回对应的半角字符，如果没有对应的半角字符则返回原字符。</returns>
        public static char ToSBC(this char c)
        {
            if (c == DBC_SPACE) return ' ';

            if (c > 65280 && c < 65375)
            {
                return (char)(c - DBC_SUB_SBC);
            }
            return c;
        }

        /// <summary>
        /// 将半角字符转换成全角字符。
        /// </summary>
        /// <remarks>
        /// 只对 <c>Ascii</c> 码在 32~126 之间的字符进行全/半角转换操作。
        /// </remarks>
        /// <param name="c">要转换的半角字符，如果没有对应的全角字符则直接返回。</param>
        /// <returns>返回对应的全角字符，如果没有对应的全角字符则返回原字符。</returns>
        public static char ToDBC(this char c)
        {
            if (c > 126 || c < 32) return c;

            if (c == ' ') return DBC_SPACE;

            return (char)(c + DBC_SUB_SBC);
        }
    }
}
