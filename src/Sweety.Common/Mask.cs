/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  此类提供敏感掩码修饰功能：
 *      如：
 *          电子邮箱地址：m****q@dhlx.cn
 *          移动电话号码：139****4377
 *          身份证号码：110***********3432
 *          银行账号：403392******0738
 * 
 * Members Index:
 *      static class
 *          Mask
 *              string EmailAddress
 *              string MobilePhoneNumber
 *              string IdentificationNumber
 *              string BankCardNumber
 *              string Custom
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common
{
    using System;


    /// <summary>
    /// 将不适合完全公开的信息的其中一部分内容替换成掩码。
    /// </summary>
    public static class Mask
    {
        /// <summary>
        /// 掩饰 <c>E-Mail</c> 地址；将名称部分第一个和最后一个字符保留，中间用同等数量的“*”号代替。
        /// </summary>
        /// <param name="str"><c>E-Mail</c> 地址。</param>
        /// <returns>返回如：将 <c>E-Mail</c> 地址 “myun168@gmail.com” 转换为 “m*****8@gmail.com”，“myun_18@126.com” 转换为 “m*****8@126.com”。</returns>
        public static string EmailAddress(string str)
        {
            if (String.IsNullOrEmpty(str)) return str;

            var atIndex = str.LastIndexOf('@');

            if (atIndex < 3) return str;

            return str.Substring(0, 1) + String.Empty.PadLeft(atIndex - 2, '*') + str.Substring(atIndex - 1);
        }

        /// <summary>
        /// 掩饰移动电话号码；将 11 位移动电话号码的第四位至第七位用“*”号代替，如：139****4377。
        /// </summary>
        /// <param name="str">11 位的移动电话号码。</param>
        /// <returns>返回如：139****4377 的 11 为电话号码，如果号码不是 11 位则返回原始内容。</returns>
        public static string MobilePhoneNumber(string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length != 11) return str;

            return str.Substring(0, 3) + String.Empty.PadLeft(str.Length - 7, '*') + str.Substring(7);
        }

        /// <summary>
        /// 掩饰身份证号码；将 15 位身份证号码的第四位至十二位用“*”号代替，将18位身份证号码的第四位至第十四位用“*”号代替。
        /// </summary>
        /// <param name="str">15 位或 18 位身份证号码。</param>
        /// <returns>15 位身份证号码返回如：130*********001；18 位身份证号码返回如：110***********3432；否则原样返回。</returns>
        public static string IdentificationNumber(string str)
        {
            if (String.IsNullOrEmpty(str)) return str;

            if (str.Length == 15)
            {
                return str.Substring(0, 3) + String.Empty.PadLeft(str.Length - 6, '*') + str.Substring(12);
            }

            if (str.Length == 18)
            {
                return str.Substring(0, 3) + String.Empty.PadLeft(str.Length - 7, '*') + str.Substring(14);
            }

            return str;
        }

        /// <summary>
        /// 掩饰银行卡号，返回卡号前 6 位数和后 4 位数，其余数字用“*”号代替。
        /// </summary>
        /// <param name="str">16 位或 19 位的银行卡号码。</param>
        /// <returns>返回如：403392******0738。</returns>
        public static string BankCardNumber(string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 16) return str;

            return str.Substring(0, 6) + String.Empty.PadLeft(str.Length - 10, '*') + str.Substring(str.Length - 4);
        }

        /// <summary>
        /// 将字符串从指定字符索引位置开始向右的n个字符替换成“*”星号。
        /// </summary>
        /// <param name="str">要进行掩码处理的字符串。</param>
        /// <param name="startIndex">字符索引位置。</param>
        /// <param name="length">要替换为星号的字符数量，如果数量超过了字符串本身的长度则会将 <paramref name="startIndex"/> 之后的所有字符替换为星号，而不是增加 <paramref name="length"/> 个星号。</param>
        /// <returns>替换后的字符串。返回的字符串长度等于传入的字符串长度。</returns>
        public static string Custom(string str, int startIndex, int length)
        {
            return Custom(str, '*', startIndex, length);
        }

        /// <summary>
        /// 将字符串从指定字符索引位置开始向右的 n 个字符替换成指定的掩码字符。
        /// </summary>
        /// <param name="str">要进行掩码处理的字符串。</param>
        /// <param name="maskChar">掩码字符。</param>
        /// <param name="startIndex">字符索引位置。</param>
        /// <param name="length">要替换为掩码字符的字符数量，如果数量超过了字符串本身的长度则会将 <paramref name="startIndex"/> 之后的所有字符替换为掩码字符，而不是增加 <paramref name="length"/> 个掩码字符。</param>
        /// <returns>替换后的字符串。返回的字符串长度等于传入的字符串长度。</returns>
        public static string Custom(string str, char maskChar, int startIndex, int length)
        {
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, Properties.Localization.the_starting_index_must_be_greater_than_or_equal_to_zero);
            if (length < 1) throw new ArgumentOutOfRangeException(nameof(length), length, Properties.Localization.the_length_must_be_greater_than_zero);
            if (String.IsNullOrEmpty(str)) return str;
            if (str.Length <= startIndex) throw new ArgumentOutOfRangeException(nameof(startIndex), Properties.Localization.the_starting_index_is_not_in_the_valid_range);

            if (str.Length <= startIndex + length)
            {
                return str.Substring(0, startIndex) + String.Empty.PadLeft(str.Length - startIndex, maskChar);
            }
            else
            {
                return str.Substring(0, startIndex) + String.Empty.PadLeft(length, maskChar) + str.Substring(startIndex + length);
            }
        }
    }
}
