/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      Char 类型扩展。
 * 
 * Members Index:
 *      static class CharExtensions
 *          bool IsCurrencySymbol(this char c)
 *          bool IsSBCLetter(this char c)
 *          bool IsSBCLetterOrDigit(this char c)
 *          char ToSBC(this char c)
 *          char ToDBC(this char c)
 *          
 *      
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Extensions
{
    using System;
    using System.Collections.Generic;


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
        /// 货币符号集
        /// </summary>
        /// <see cref="https://unicode-table.com/cn/blocks/currency-symbols/"/>
        /// <see cref="http://www.52unicode.com/currency-symbols-zifu"/>
        static HashSet<char> __currencySymbol = new HashSet<char>
        {
            '￥',    //人民币
            '$',    //美元
            '€',    //欧元
            '₠',    //欧洲
            '₡',    //科朗
            '₢',    //克鲁塞罗
            '₣',    //法国法郎
            '₤',    //里拉
            '₺',    //土耳其里拉（Turkish Lira）
            '₥',    //密尔
            '₦',    //奈拉
            '₧',    //比塞塔
            '₨',    //卢比
            '₹',    //印度卢比
            '₩',    //韩元
            '₪',    //新谢克尔
            '₫',    //盾
            '₭',    //基普标记
            '₮',    //图格里克
            '₯',    //德拉克马
            '₰',    //德国便士
            '₱',    //比索
            '₲',    //瓜拉尼
            '₳',    //澳大利亚
            '₴',    //格里夫纳
            '₵',    //塞地
            '₶',    //Livre Tournois
            '₷',    //Spesmilo
            '₸',    //Tenge
            '₻',    //北欧马克（Nordic Mark）
            '₼',    //马纳特（Manat）
            '₽',    //俄罗斯卢布
            '₾',    //拉里（Lari）
            '₿'     //比特币（Bitcoin）
        };

        /// <summary>
        /// 指示 <c>Unicod</c> 是否是货币符号。（未完全收录世界各地的货币符号）
        /// </summary>
        /// <param name="c">要检查的字符。</param>
        /// <returns>如果 <paramref name="c"/> 是已知的货币符号则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsCurrencySymbol(this char c)
        {
            return __currencySymbol.Contains(c);
        }

        /// <summary>
        /// 指示 <c>Unicod</c> 是否是半角英文字母。
        /// </summary>
        /// <param name="c">要检查的字符。</param>
        /// <returns>如果 <paramref name="c"/> 是半角英文字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLetter(this char c)
        {
            int unicodeValue = c;

            //大写字母
            if (unicodeValue > 64 && unicodeValue < 91)
            {
                return true;
            }

            //小写字母
            if (unicodeValue > 96 && unicodeValue < 123)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 指示 <c>Unicod</c> 是否是半角英文字母或数字。
        /// </summary>
        /// <param name="c">要检查的字符。</param>
        /// <returns>如果 <paramref name="c"/> 是半角英文字母或数字则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLetterOrDigit(this char c)
        {
            int unicodeValue = c;

            //大写字母
            if (unicodeValue > 64 && unicodeValue < 91) return true;

            //小写字母
            if (unicodeValue > 96 && unicodeValue < 123) return true;

            //数字
            if (unicodeValue > 47 && unicodeValue < 58) return true;
            
            return false;
        }

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