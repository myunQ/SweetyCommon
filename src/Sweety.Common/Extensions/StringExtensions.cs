/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      String 类型扩展。
 * 
 * Members Index:
 *      static class StringExtensions
 *          int ByteLength(this string str)
 *          int ByteLengthComparer(this string str, int length)
 *          string SubstringByte(this string str, int length)
 *          string SubstringByte(this string str, Encoding encoding, int length)
 *          string SubstringByte(this string str, int length, string append)
 *          string SubstringByte(this string str, Encoding encoding, int length, string append)
 *          string SubstringRetainHTML(this string str, int length, string append)
 *          bool StartsAndEndsWith(this string str, char value)
 *          bool StartsAndEndsWith(this string str, string value)
 *          bool StartsAndEndsWith(this string str, string value, StringComparison comparisonType)
 *          bool StartsAndEndsWith(this string str, string value, bool ignoreCase, CultureInfo culture)
 *          bool StartsAndEndsWith(this string str, char starts, char ends)
 *          bool StartsAndEndsWith(this string str, string starts, string ends)
 *          bool StartsAndEndsWith(this string str, string starts, string ends, StringComparison comparisonType)
 *          bool StartsAndEndsWith(this string str, string starts, string ends, bool ignoreCase, CultureInfo culture)
 *          int ToInt(this string s, int radix)
 *          long ToLong(this string s, int radix)
 *          Guid ToGuid(this string value, Guid defaultValue)
 *          byte[] HexToBytes(this string value)
 *          string ToURLSafeBase64(this string value)
 *          string ToNormalBase64(this string value)
 *          bool TryFromBase64String(this string value, out byte[] result)
 *          string ToSBC(this string str)
 *          string ToDBC(this string str)
 *          
 *      
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Extensions
{
    using System;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    using Sweety.Common.Verification;

    /// <summary>
    /// <see cref="String"/> 类型的扩展方法。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串字节长度。
        /// </summary>
        /// <remarks>
        /// 扩展 <c>ASCII</c> 码被视为 2 个字节，即：(<c>ushort</c>)<c>char</c> 大于 127 的字符都视为 2 个字节。
        /// </remarks>
        /// <param name="str">字符串。</param>
        /// <returns>字符串字节长度。</returns>
        public static int ByteLength(this string str)
        {
            if (String.IsNullOrEmpty(str)) return 0;

            int result = 0;

            foreach (var ch in str)
            {
                result += ((ch & 0xFF80) == 0) ? 1 : 2;
            }

            return result;
        }

        /// <summary>
        /// 按字节比较字符串的长度。
        /// </summary>
        /// <param name="str">要比较的字符串。</param>
        /// <param name="length">被比较的字符串字节长度。</param>
        /// <returns>
        /// 一个有符号整数，指示 <paramref name="str"/> 的字节长度和 <paramref name="length"/> 的相对值，如下表所示。
        /// 值含义：
        /// 小于零 <paramref name="str"/> 的字节长度 小于 <paramref name="length"/>。
        /// 零 <paramref name="str"/> 的字节长度 等于 <paramref name="length"/>。
        /// 大于零 <paramref name="str"/> 的字节长度 大于 <paramref name="length"/>。
        /// </returns>
        public static int ByteLengthComparer(this string str, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), length, Properties.Localization.the_byte_length_of_the_compared_strings_must_not_be_less_than_zero);

            int strByteLength = 0;
            if (!String.IsNullOrEmpty(str))
            {
                foreach (var ch in str)
                {
                    strByteLength += ((ch & 0xFF80) == 0) ? 1 : 2;

                    if (strByteLength > length) return 1;
                }
            }

            if (strByteLength < length)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 按字节数截取字符串。
        /// </summary>
        /// <remarks>
        /// 扩展 <c>ASCII</c> 码被视为 2 个字节，即：(<c>ushort</c>)<c>char</c> 大于 127 的字符都视为 2 个字节。
        /// </remarks>
        /// <param name="str">字符串。</param>
        /// <param name="length">保留的字节长度（截取后字符串总字节数小于等于该参数的值）。</param>
        /// <returns>总字节长度不超过 <paramref name="length"/> 的字符串。</returns>
        public static string SubstringByte(this string str, int length)
        {
            if (String.IsNullOrEmpty(str) || length <= 0 || str.ByteLengthComparer(length) < 1) return str;

            int i = 0, j = 0;

            do
            {
                i += ((str[j] & 0xFF80) == 0) ? 1 : 2;
                j++;
            } while (i < length);

            if (i > length)
            {
                return str.Substring(0, j - 1);
            }

            return str.Substring(0, j);
        }

        /// <summary>
        /// 按字节数截取字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="encoding">字符编码。</param>
        /// <param name="length">保留的字节长度（截取后字符串总字节数小于等于该参数的值）。</param>
        /// <returns>总字节长度不超过 <paramref name="length"/> 的字符串。</returns>
        public static string SubstringByte(this string str, Encoding encoding, int length)
        {
            if (String.IsNullOrEmpty(str) || length <= 0 || encoding.GetByteCount(str) <= length) return str;

            int i = 0, j = 0;

#if NETSTANDARD2_0
            char[] ch = new char[1];
#else
            Span<char> ch = stackalloc char[1];
#endif
            StringBuilder result = new StringBuilder(str.Length);

            do
            {
                ch[0] = str[j++];
                i += encoding.GetByteCount(ch);
                result.Append(ch[0]);
            } while (i < length);

            if (i > length)
            {
                result.Length--;
            }

            return result.ToString();
        }



        /// <summary>
        /// 按字节数截取字符串。
        /// </summary>
        /// <remarks>
        /// 扩展 <c>ASCII</c> 码被视为 2 个字节，即：(<c>ushort</c>)<c>char</c> 大于 127 的字符都视为 2 个字节。
        /// </remarks>
        /// <param name="str">字符串。</param>
        /// <param name="length">返回值的字节长度（截取后字符串总字节数加上 <paramref name="append"/> 参数值的总字节数小于等于该参数的值）。</param>
        /// <param name="append">字符串被截取后在末尾追加的字符串。</param>
        /// <returns>总字节长度不超过 <paramref name="length"/> 的字符串。</returns>
        public static string SubstringByte(this string str, int length, string append)
        {
            if (String.IsNullOrEmpty(str) || length <= 0 || str.ByteLengthComparer(length) < 1) return str;

            int i = 0, j = 0;

            if (!String.IsNullOrEmpty(append))
            {
                i = append.ByteLength();

                if (i >= length) throw new ArgumentException(Properties.Localization.the_length_of_the_string_to_be_appended_to_the_end_exceeds_the_length_to_be_intercepted_or_equal_to_the_length_to_be_intercepted, nameof(append));
            }

            do
            {
                i += ((str[j] & 0xFF80) == 0) ? 1 : 2;
                j++;
            } while (i < length);

            if (i > length)
            {
                if (j == 1) throw new OverflowException(Properties.Localization.the_length_of_bytes_after_the_intercepted_string_plus_the_appended_string_exceeds_the_expected_length);

                return str.Substring(0, j - 1) + append;
            }

            return str.Substring(0, j) + append;
        }

        /// <summary>
        /// 按字节数截取字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="encoding">字符编码。</param>
        /// <param name="length">返回值的字节长度（截取后字符串总字节数加上 <paramref name="append"/> 参数值的总字节数小于等于该参数的值）。</param>
        /// <param name="append">字符串被截取后在末尾追加的字符串。</param>
        /// <returns>总字节长度不超过 <paramref name="length"/> 的字符串</returns>
        public static string SubstringByte(this string str, Encoding encoding, int length, string append)
        {
            if (String.IsNullOrEmpty(str) || length <= 0 || encoding.GetByteCount(str) <= length) return str;

            int i = 0, j = 0;
            if (!String.IsNullOrEmpty(append))
            {
                i = encoding.GetByteCount(append);

                if (i >= length) throw new ArgumentException(Properties.Localization.the_length_of_the_string_to_be_appended_to_the_end_exceeds_the_length_to_be_intercepted_or_equal_to_the_length_to_be_intercepted, nameof(append));
            }

#if NETSTANDARD2_0
            char[] ch = new char[1];
#else
            Span<char> ch = stackalloc char[1];
#endif
            StringBuilder result = new StringBuilder(str.Length);

            do
            {
                ch[0] = str[j++];
                i += encoding.GetByteCount(ch);
                result.Append(ch[0]);
            } while (i < length);

            if (i > length)
            {
                if (result.Length == 1) throw new OverflowException(Properties.Localization.the_length_of_bytes_after_the_intercepted_string_plus_the_appended_string_exceeds_the_expected_length);

                result.Length--;
            }

            result.Append(append);
            return result.ToString();
        }

        /// <summary>
        /// 按字节截取 <c>HTML</c> 文档的可视部分，该方法不计算 <c>HTML</c> 标签的字节数，截取后的字符串包含 <c>HTML</c> 标签。
        /// </summary>
        /// <param name="str"><c>HTML</c> 文档字符串。</param>
        /// <param name="length">需要截取的长度（单位：<c>byte</c>）。</param>
        /// <param name="append">截取后需要追加的字符串。</param>
        /// <returns>可视内容字节长度不超过<c><paramref name="length"/> + <paramref name="append"/>.length</c> 的字符串的 <c>HTML</c> 文档。</returns>
        public static string SubstringRetainHTML(this string str, int length, string append)
        {
            if (length <= 0) throw new ArgumentException("length 必须大于 0。", "length");
            if (String.IsNullOrEmpty(str) || str.ByteLengthComparer(length) < 1) return str;

            char c;
            bool isLabel = false; //当前索引处于标签内
            bool isEndLable = false; //当前索引处于结束标签内
            bool isLabelName = false; //当前索引处于标签名内
            bool isComment = false; //当前索引处于注释内
            string labelName = null;
            Stack<string> stack = new Stack<string>();
            StringBuilder buffer = new StringBuilder(16); //记录HTML标签名，不包含 “<” 和 “>”
            StringBuilder result = new StringBuilder(length);

            for (int i = 0, len = str.Length - 1; i <= len && length > 0; i++)
            {
                c = str[i];

                if (isLabelName)
                {
                    if (c == '>' || c == '/' || CharVerification.IsBlank(c))
                    {
                        isLabelName = false;
                        labelName = buffer.ToString();
                        buffer.Clear();
                        if (!("br".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "img".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "hr".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "input".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "meta".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "link".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "param".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "source".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "base".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "area".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "col".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "embed".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "keygen".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                            || "track".Equals(labelName, StringComparison.OrdinalIgnoreCase)))
                        {
                            if (isEndLable)
                            {
                                isEndLable = false;
                                if (stack.Count > 0 && labelName.Equals(stack.Peek(), StringComparison.OrdinalIgnoreCase))
                                {
                                    stack.Pop();
                                }
                            }
                            else
                            {
                                stack.Push(labelName);
                            }
                        }
                    }
                    else
                    {
                        buffer.Append(c);
                    }
                }

                if (c == '<' && !isComment)
                {
                    if (i < len)
                    {
                        char nextChar = str[i + 1];
                        if (CharVerification.IsSBCLetter(nextChar))
                        {
                            isLabel = isLabelName = true;
                            result.Append(c);
                            continue;
                        }
                        else if (nextChar == '/') //结束标签中的斜杠
                        {
                            isLabel = isLabelName = isEndLable = true;
                            result.Append(c);
                            result.Append(nextChar);
                            i++;
                            continue;
                        }
                        else if (nextChar == '!')
                        {
                            if ((i < len - 3) && (str[i + 2] == '-') && (str[i + 3] == '-'))
                            {
                                isComment = true;
                                result.Append("<!--");
                                i += 3;
                            }
                            else if (i < len - 7 && (str[i + 2] == 'D' || str[i + 2] == 'd')) //<!DOCTYPE HTML>
                            {
                                isLabel = true;
                                result.Append(c);
                                result.Append(nextChar);
                                i++;
                            }
                            continue;
                        }
                    }

                    length--;
                    result.Append("&lt;");
                }
                else if (c == '>')
                {
                    if (isLabel)
                    {
                        isLabel = false;
                        result.Append(c);
                    }
                    else if (isComment)
                    {
                        if ((i > 2) && (str[i - 1] == '-') && (str[i - 2] == '-'))
                        {
                            isComment = false;
                        }
                        result.Append(c);
                    }
                    else
                    {
                        length--;
                        result.Append("&gt;");
                    }
                }
                else
                {
                    if (!(isLabel || isComment)
                         && !"audio".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                         && !"noframes".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                         && !"noscript".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                         && !"script".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                         && !"style".Equals(labelName, StringComparison.OrdinalIgnoreCase)
                         && !"video".Equals(labelName, StringComparison.OrdinalIgnoreCase))
                    {
                        if ((c & 0xFF80) != 0)
                        {
                            length -= 2;
                        }
                        else
                        {
                            length--;
                        }
                    }
                    result.Append(c);
                }
            }

            if (length < 1 && !String.IsNullOrEmpty(append))
            {
                result.Append(append);
            }

            while (stack.Count > 0)
            {
                result.Append("</" + stack.Pop() + ">");
            }

            return result.ToString();
        }

        /// <summary>
        /// 确定此字符串实例的开头与结尾是否与指定的字符匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="value">要比较的字符。</param>
        /// <returns>如果 <paramref name="value"/> 与此字符串的开头与结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, char value)
        {
            return String.IsNullOrEmpty(str) == false && str[0] == value && str[str.Length - 1] == value;
        }
        /// <summary>
        /// 确定此字符串实例的开头与结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="value">要比较的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 与此字符串的开头与结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, string value)
        {
            return String.IsNullOrEmpty(str) == false && str.StartsWith(value) && str.EndsWith(value);
        }
        /// <summary>
        /// 确定在使用指定的比较选项进行比较时此字符串实例的开头与结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="value">要比较的字符串。</param>
        /// <param name="comparisonType">枚举值之一，用于确定如何比较此字符串与 <paramref name="value"/>。</param>
        /// <returns>如果 <paramref name="value"/> 参数与此字符串的开头与结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, string value, StringComparison comparisonType)
        {
            return String.IsNullOrEmpty(str) == false && str.StartsWith(value, comparisonType) && str.EndsWith(value, comparisonType);
        }
        /// <summary>
        /// 确定在使用指定的区域性进行比较时此字符串实例的开头与结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="value">要比较的字符串。</param>
        /// <param name="ignoreCase">要在比较过程中忽略大小写，则为 <c>true</c>；否则为 <c>false</c>。</param>
        /// <param name="culture">确定如何对此字符串与 <paramref name="value"/> 进行比较的区域性信息。如果 <paramref name="culture"/> 为 <c>null</c>，则使用当前区域性。</param>
        /// <returns>如果 <paramref name="value"/> 参数与此字符串的开头与结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, string value, bool ignoreCase, CultureInfo culture)
        {
            return String.IsNullOrEmpty(str) == false && str.StartsWith(value, ignoreCase, culture) && str.EndsWith(value, ignoreCase, culture);
        }
        /// <summary>
        /// 确定此字符串实例的开头与结尾是否与指定的字符匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="starts">要与开头比较的字符。</param>
        /// <param name="ends">要与结尾比较的字符。</param>
        /// <returns>如果 <paramref name="starts"/> 参数与此字符串的开头匹配并且 <paramref name="ends"/> 参数与此字符串的结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, char starts, char ends)
        {
            return String.IsNullOrEmpty(str) == false && str[0] == starts && str[str.Length - 1] == ends;
        }
        /// <summary>
        /// 确定此字符串实例的开头与结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="starts">要与开头比较的字符串。</param>
        /// <param name="ends">要与结尾比较的字符串。</param>
        /// <returns>如果 <paramref name="starts"/> 参数与此字符串的开头匹配并且 <paramref name="ends"/> 参数与此字符串的结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, string starts, string ends)
        {
            return String.IsNullOrEmpty(str) == false && str.StartsWith(starts) && str.EndsWith(ends);
        }
        /// <summary>
        /// 确定在使用指定的比较选项进行比较时此字符串实例的开头与结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="starts">要与开头比较的字符串。</param>
        /// <param name="ends">要与结尾比较的字符串。</param>
        /// <param name="comparisonType">枚举值之一，用于确定如何比较此字符串。</param>
        /// <returns>如果 <paramref name="starts"/> 参数与此字符串的开头匹配并且 <paramref name="ends"/> 参数与此字符串的结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, string starts, string ends, StringComparison comparisonType)
        {
            return String.IsNullOrEmpty(str) == false && str.StartsWith(starts, comparisonType) && str.EndsWith(ends, comparisonType);
        }
        /// <summary>
        /// 确定在使用指定的区域性进行比较时此字符串实例的开头与结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="starts">要与开头比较的字符串。</param>
        /// <param name="ends">要与结尾比较的字符串。</param>
        /// <param name="ignoreCase">要在比较过程中忽略大小写，则为 <c>true</c>；否则为 <c>false</c>。</param>
        /// <param name="culture">确定如何对此字符串，进行比较的区域性信息。如果 <paramref name="culture"/> 为 <c>null</c>，则使用当前区域性。</param>
        /// <returns>如果 <paramref name="starts"/> 参数与此字符串的开头匹配并且 <paramref name="ends"/> 参数与此字符串的结尾匹配，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool StartsAndEndsWith(this string str, string starts, string ends, bool ignoreCase, CultureInfo culture)
        {
            return String.IsNullOrEmpty(str) == false && str.StartsWith(starts, ignoreCase, culture) && str.EndsWith(ends, ignoreCase, culture);
        }

        /// <summary>
        /// 将 <see cref="IntExtensions.ToString(int, int)"/> 转换的字符串换还原成 <c>long</c> 类型。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="radix">指定进制，取值范围 2~66。</param>
        /// <returns>返回字符串表示的 <c>int</c> 类型的值。</returns>
        public static int ToInt(this string str, int radix)
        {
            return (int)ToLong(str, radix);
        }

        /// <summary>
        /// 将 <see cref="LongExtensions.ToString(long, int)"/> 转换的字符串换还原成 <c>long</c> 类型。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="radix">指定进制，取值范围 2~66。</param>
        /// <returns>返回字符串表示的 <c>long</c> 类型的值。</returns>
        public static long ToLong(this string str, int radix)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));
            if (radix < 2 || radix > LongExtensions.URI_SAFE_CHARS.Length) throw new ArgumentOutOfRangeException(nameof(radix), radix, String.Format(Properties.Localization.the_value_ranges_from_XXX_to_XXX, 2, LongExtensions.URI_SAFE_CHARS.Length));

            long result = 0L;
            int i = 0, len = str.Length;
            int exponent = len - 1;
            if (str[0] == '-')
            {
                exponent--;
                i++;
            }

            for (; i < len; i++)
            {
                long p = Array.IndexOf(LongExtensions.URI_SAFE_CHARS, str[i]);
                if (p < 0) throw new FormatException(String.Format(Properties.Localization.invalid_character_XXX, str[i]));

                if (p > 0)
                {
                    result += (p * SubMethodPow(radix, exponent--));
                }
                else
                {
                    exponent--;
                }
            }

            return str[0] == '-' ? -result : result;

            //没有使用 Math.Pow(...) 方法的原因：当数字巨大的时候 double 将使用科学计数法表示，导致丢失精度无法准确还原出原始数值。
            //这个方法 SubMethodPow(...) 不是完整的幂运算方法，因此仅用于此方法，不可公开给别的调用者。
#if NETSTANDARD2_0
            long SubMethodPow(int x, int y)
#else
            static long SubMethodPow(int x, int y)
#endif
            {
                //if (y < 0) throw new NotSupportedException("此方法不支持小于零的支持。");
                //if (x == 0) return 0L; 已经把 x == 0 合并到下面的 y == 1 的判断了。
                if (y == 0) return 1L;
                if (y == 1 || x == 0) return x;

                long sm_result = 1L;
                long xL = x;
                for (int sm_i = 0; sm_i < y; sm_i++)
                {
                    sm_result *= xL;
                }

                return sm_result;
            }
        }

        /// <summary>
        /// 将 <c>GUID</c> 字符串转换成 <see cref="Guid"/> 结构体。
        /// </summary>
        /// <param name="value"><c>GUID</c> 字符串。</param>
        /// <param name="defaultValue">转换失败返回的 <see cref="Guid"/> 结构体。</param>
        /// <returns><see cref="Guid"/> 结构体。</returns>
        public static Guid ToGuid(this string value, Guid defaultValue)
        {
            if (value != null && value.Length == 36)
            {
                try { return new Guid(value); }
                catch { }
            }
            return defaultValue;
        }

        /// <summary>
        /// 将 十六进制 的字符串转换为 <c>byte</c> 数组。
        /// </summary>
        /// <param name="hexString">十六进制字符串，长度应为偶数。</param>
        /// <returns>十六进制字符串对应的 <c>byte</c> 数组，如果字符串为空则返回零长度的 <c>byte</c>数组。</returns>
        public static byte[] HexToBytes(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return new byte[0];

            if ((value.Length % 2) != 0)
            {
                throw new ArgumentException(Properties.Localization.the_length_of_the_hexadecimal_string_should_be_an_even_number);
            }

            int arrLength = value.Length / 2;
            byte[] result = new byte[arrLength];

            for (int i = 0; i < arrLength; i++)
            {
                result[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            }

            return result;
        }

        /// <summary>
        /// 将 <c>Base64</c> 字符串转换为 <c>URL</c> 安全的 <c>Base64</c> 编码，适用于以 <c>URL</c> 方式传递 <c>Base64</c> 编码结果的场景。
        /// </summary>
        /// <param name="value">标准的 <c>Base64</c> 编码字符串。</param>
        /// <returns>适用于以 <c>URL</c> 方式传递 <c>Base64</c> 编码字符串，将“+”替换为“-”，将“/”替换为“_”。</returns>
        public static string ToURLSafeBase64(this string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            return value.Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// 将 <c>URL</c> 安全的 <c>Base64</c> 编码字符串转换为标准的 <c>Base64</c> 编码字符串。
        /// </summary>
        /// <param name="value"><c>URL</c> 安全的 <c>Base64</c> 编码字符串。</param>
        /// <returns>标准的 <c>Base64</c> 编码字符串。</returns>
        public static string ToNormalBase64(this string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            return value.Replace('-', '+').Replace('_', '/');
        }

        /// <summary>
        /// 尝试将字符串当作 <c>Base64</c> 编码字符串进行解码。
        /// </summary>
        /// <param name="value"><c>Base64</c> 编码的字符串。</param>
        /// <param name="result">解码后的结果。</param>
        /// <returns>解码成功返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool TryFromBase64String(this string value, out byte[] result)
        {
            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    result = Convert.FromBase64String(value);
                    return true;
                }
                catch { }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// 将全角字符转换成半角字符。
        /// </summary>
        /// <remarks>
        /// 只对 <c>Ascii</c> 码在 32~126 之间的字符进行全/半角转换操作。
        /// </remarks>
        /// <param name="str">要转换的字符串，如果没有对应的半角字符则直接返回。</param>
        /// <returns>返回对应的半角字符串，如果没有对应的半角字符则返回原字符。</returns>
        public static string ToSBC(this string str)
        {
            if (String.IsNullOrEmpty(str)) return str;

            int i = 0;
            int j = str.Length;

            char[] cArr = null;
            for (; i < j; i++)
            {
                if (str[i].CanToSBC())
                {
                    cArr = str.ToCharArray();
                    cArr[i] = cArr[i].ToSBC();
                    for (i++; i < j; i++)
                    {
                        if (str[i].CanToSBC()) cArr[i] = cArr[i].ToSBC();
                    }
                }
            }

            if (cArr == null) return str;

            return new String(cArr);
        }

        /// <summary>
        /// 将半角字符转换成全角字符。
        /// </summary>
        /// <remarks>
        /// 只对 <c>Ascii</c> 码在 32~126 之间的字符进行全/半角转换操作。
        /// </remarks>
        /// <param name="str">要转换的字符串，如果没有对应的全角字符则直接返回。</param>
        /// <returns>返回对应的全角字符串，如果没有对应的全角字符则返回原字符。</returns>
        public static string ToDBC(this string str)
        {
            if (String.IsNullOrEmpty(str)) return str;

            int i = 0;
            int j = str.Length;

            char[] cArr = null;
            for (; i < j; i++)
            {
                if (str[i].CanToDBC())
                {
                    cArr = str.ToCharArray();
                    cArr[i] = cArr[i].ToDBC();
                    for (i++; i < j; i++)
                    {
                        if (cArr[i].CanToDBC()) cArr[i] = cArr[i].ToDBC();
                    }
                }
            }

            if (cArr == null) return str;

            return new String(cArr);
        }
    }
}