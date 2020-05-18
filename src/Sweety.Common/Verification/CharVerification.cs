/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          CharVerification
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Verification
{
    using System;

    using Sweety.Common.Extensions;


    /// <summary>
    /// 字符验证类。
    /// </summary>
    public static class CharVerification
    {
        /// <summary>
        /// 验证字符是否是 <c>Ascii</c> 字符集中的字符。
        /// </summary>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是 <c>Ascii</c> 字符集中的字符则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsAscii(this char c)
        {
            return c < '\x0080';
        }

        /// <summary>
        /// 验证字符是否是单字节字符。
        /// </summary>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是单字节字符则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSingleByte(this char c)
        {
            return c < '\x0100';
        }

        /// <summary>
        /// 验证字符是否是半角的数字“0  1   2   3   4   5   6   7   8   9”。
        /// </summary>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是半角的数字字符则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCDigit(this char c)
        {
            return '0' <= c && c <= '9';
        }

        /// <summary>
        /// 验证字符是否是全角的数字“０	１	２	３	４	５	６	７	８	９”。
        /// </summary>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是全角的数字字符则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCDigit(this char c)
        {
            return '０' <= c && c <= '９';
        }

        /// <summary>
        /// 验证字符是否是半角的大写或小写字母。
        /// </summary>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是半角字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLetter(this char c)
        {
            if (IsAscii(c))
            {
                c = (char)(c | 0x20); //大写字母转小写字母
                //c = (char)(c & ~0x20); //小写字母转大写字母

                return c >= 'a' && c <= 'z';
            }
            return false;
        }

        /// <summary>
        /// 验证字符是否是全角的大写或小写字母。
        /// </summary>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是全角字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCLetter(this char c)
        {
            if (c >= 'Ａ' && c <= 'ｚ')
            {
                c = (char)(c & 0xFFDF | 0x40); //大写字母转小写字母
                //c = (char)(c & ~0x40 | ~0xFFDF); //小写字母转大写字母

                return c >= 'ａ' && c <= 'ｚ';
            }
            return false;
        }

        /// <summary>
        /// 验证字符是否是空白字符，包括空格、水平制表符、垂直制表符、换行符、分页符、回车符。
        /// </summary>
        /// <remarks>
        /// 此方法仅验证字符是不是 <c>Ascii</c> 码里的空白符，而 <see cref="Char.IsWhiteSpace(char)"/> 验证字符是不是 <c>Unicode</c> 码里的空白符。
        /// </remarks>
        /// <param name="c">需要验证的字符。</param>
        /// <returns>如果是空白字符则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsBlank(this char c)
        {
            //0x09 水平制表符
            //0x0A 换行符
            //0x0B 垂直制表符
            //0x0C 分页符
            //0x0D 回车符
            //0x20 空格
            return c == ' ' || c >= 0x09 && c <= 0x0D;
        }

        /// <summary>
        /// 获取一个值表示参数 <paramref name="c"/> 是否可以被转换成半角字符。
        /// </summary>
        /// <remarks>
        /// 如果参数<paramref name="c"/>的值的 <c>Unicode</c> 编码等于 12288（全角空格）或在(65281 ~ 65374)(含)之间则认为是可以转换成半角的字符。
        /// 注意：全角字符与半角字符转换跟中文字母和英文字符转换无关，比如并不会将中文句号“。”转换成英文句号“.”以此类推。
        /// </remarks>
        /// <param name="c">预转换成半角的字符。</param>
        /// <returns>字符可以被转换成半角则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool CanToSBC(this char c)
        {
            return c == CharExtensions.DBC_SPACE || (c > 65280 && c < 65375);
        }

        /// <summary>
        /// 获取一个值表示参数 <paramref name="c"/> 是否可以被转换成全角字符。
        /// </summary>
        /// <remarks>
        /// 如果参数 <paramref name="c"/> 的值的 <c>Unicode</c> 编码在(32 ~ 126)(含)之间则认为是可以转换成全角的字符。
        /// 注意：全角字符与半角字符转换跟中文字母和英文字符转换无关，比如并不会将中文句号“。”转换成英文句号“.”以此类推。
        /// </remarks>
        /// <param name="c">预转换成全角的字符</param>
        /// <returns>字符可以被转换成全角则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool CanToDBC(this char c)
        {
            return !(c > 126 || c < 32);
        }
    }
}
