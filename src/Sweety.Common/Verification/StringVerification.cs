/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          StringVerification
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Verification
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 字符串验证类。
    /// </summary>
    public static class StringVerification
    {
        /// <summary>
        /// 验证字符串是否仅包含字母，不区分大小写，也不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < 'ａ' || c > 'ｚ') && (c < 'Ａ' || c > 'Ｚ'))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含半角字母，不区分大小写。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含半角字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z'))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含全角字母，不区分大小写。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <param name="ignoreFullAndHalf">忽略全/半角。</param>
        /// <returns>如果字符串仅包含全角字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'ａ' || c > 'ｚ') && (c < 'Ａ' || c > 'Ｚ'))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含小写字母，不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串</param>
        /// <returns>如果字符串仅包含小写字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsLowercaseLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'a' || c > 'z') && (c < 'ａ' || c > 'ｚ'))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含半角小写字母。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含半角小写字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLowercaseLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if (c < 'a' || c > 'z')
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含全角小写字母。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含全角小写字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCLowercaseLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if (c < 'ａ' || c > 'ｚ')
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含大写字母，不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含大写字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsUppercaseLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'A' || c > 'Z') && (c < 'Ａ' || c > 'Ｚ'))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含半角大写字母。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含半角大写字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCUppercaseLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if (c < 'A' || c > 'Z')
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含全角大写字母。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含全角大写字母则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCUppercaseLetters(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if (c < 'Ａ' || c > 'Ｚ')
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含数字，不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含数字则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDigit(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < '0' || c > '9') && (c < '０' || c > '９')) return false;
            }

            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含半角数字。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果符串仅包含半角数字则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCDigit(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含全角数字。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含全角数字则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCDigit(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if (c < '０' || c > '９') return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个的正整数或负整数，不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个正整数或负整数则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsInteger(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            int i = (str[0] == '-' || str[0] == '－' || str[0] == '+' || str[0] == '＋') ? 1 : 0;

            if (i == str.Length) return false;

            for (; i < str.Length; i++)
            {
                if ((str[i] < '0' || str[i] > '9') && (str[i] < '０' || str[i] > '９')) return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个半角的正整数或负整数。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个半角的正整数或负整数则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCInteger(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            int i = (str[0] == '-' || str[0] == '+') ? 1 : 0;

            if (i == str.Length) return false;

            for (; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9') return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个全角的正整数或负整数。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个全角的正整数或负整数则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCInteger(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            int i = (str[0] == '－' || str[0] == '＋') ? 1 : 0;

            if (i == str.Length) return false;

            for (; i < str.Length; i++)
            {
                if (str[i] < '０' || str[i] > '９') return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个正/负小数，不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个正/负小数则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDecimal(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 3) return false;

            int i = (str[0] == '-' || str[0] == '－' || str[0] == '+' || str[0] == '＋') ? 1 : 0;

#if NETSTANDARD2_0
            if (str[i] == '.' || str[str.Length - 1] == '.') return false;
#else
            if (str[i] == '.' || str[^1] == '.') return false;
#endif

            bool decimalPoint = false;

            for (; i < str.Length; i++)
            {
                if (str[i] == '.')
                {
                    if (decimalPoint) return false;

                    decimalPoint = true;
                    continue;
                }
                if ((str[i] < '0' || str[i] > '9') && (str[i] < '０' || str[i] > '９')) return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个半角的正/负小数。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个半角的正/负小数则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCDecimal(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 3) return false;

            int i = (str[0] == '-' || str[0] == '+') ? 1 : 0;

#if NETSTANDARD2_0
            if (str[i] == '.' || str[str.Length - 1] == '.') return false;
#else
            if (str[i] == '.' || str[^1] == '.') return false;
#endif

            bool decimalPoint = false;

            for (; i < str.Length; i++)
            {
                if (str[i] == '.')
                {
                    if (decimalPoint) return false;

                    decimalPoint = true;
                    continue;
                }
                if (str[i] < '0' || str[i] > '9') return false;
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个全角的正/负小数。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个全角的正/负小数则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCDecimal(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 3) return false;

            int i = (str[0] == '－' || str[0] == '＋') ? 1 : 0;

#if NETSTANDARD2_0
            if (str[i] == '.' || str[str.Length - 1] == '.') return false;
#else
            if (str[i] == '.' || str[^1] == '.') return false;
#endif

            bool decimalPoint = false;

            for (; i < str.Length; i++)
            {
                if (str[i] == '.')
                {
                    if (decimalPoint) return false;

                    decimalPoint = true;
                    continue;
                }
                if (str[i] < '０' || str[i] > '９') return false;
            }
            return true;
        }



        /// <summary>
        /// 验证字符串是否仅包含字母或数字或两者的混合，不区分全/半角。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含字母或数字或两者的混合则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsLetterOrDigit(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < 'ａ' || c > 'ｚ') && (c < 'Ａ' || c > 'Ｚ') && (c < '0' || c > '9') && (c < '０' || c > '９'))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含半角字母及半角数字或两者的混合。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含半角字母及半角数字或两者的混合则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLetterOrDigit(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9'))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含全角字母或全角数或两者的混合。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含全角字母及全角数字或两者的混合则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsDBCLetterOrDigit(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 'ａ' || c > 'ｚ') && (c < 'Ａ' || c > 'Ｚ') && (c < '０' || c > '９'))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含半角字母或半角数字或其它字符。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <param name="otherChars">还允许包含的其它字符。</param>
        /// <returns>如果字符串仅包含半角字母或半角数字或其它字符则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSBCLetterOrSBCDigitOrOther(this string str, params char[] otherChars)
        {
            if (String.IsNullOrEmpty(str)) return false;
            if (otherChars == null || otherChars.Length == 0)
            {
                return StringVerification.IsSBCLetterOrDigit(str);
            }

            int charlength = otherChars.Length;

            foreach (char c in str)
            {
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9'))
                {
                    if (charlength == 1)
                    {
                        if (c != otherChars[0]) return false;
                    }
                    else if (charlength == 2)
                    {
                        if (c != otherChars[0] && c != otherChars[1]) return false;
                    }
                    else if (charlength == 3)
                    {
                        if (c != otherChars[0] && c != otherChars[1] && c != otherChars[2]) return false;
                    }
                    else if (charlength == 4)
                    {
                        if (c != otherChars[0] && c != otherChars[1] && c != otherChars[2] && c != otherChars[3]) return false;
                    }
                    else if (charlength == 5)
                    {
                        if (c != otherChars[0] && c != otherChars[1] && c != otherChars[2] && c != otherChars[3] && c != otherChars[4]) return false;
                    }
                    else
                    {
                        bool hit = false;
                        foreach (char otherChar in otherChars)
                        {
                            if (c == otherChar)
                            {
                                hit = !hit;
                                break;
                            }
                        }
                        if (!hit) return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 验证字符串是否是一个符合规范的电子邮件地址。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串是一个符合规范的电子邮件地址则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsEmailAddress(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            string[] split = str.Split('@');
            if (split.Length != 2 || split[0].Length == 0 || split[1].Length < 3) return false;

            if (!StringVerification.IsSBCLetterOrSBCDigitOrOther(split[0], '_', '-', '.') || !StringVerification.IsSBCLetterOrSBCDigitOrOther(split[1], '-', '.')) return false;

            string[] splitdomain = split[1].Split('.');
            if (splitdomain.Length == 1) return false;

            foreach (var segment in splitdomain)
            {
#if NETSTANDARD2_0
                if (segment.Length == 0 || segment[0] == '-' || segment[segment.Length - 1] == '-') return false;
#else
                if (segment.Length == 0 || segment[0] == '-' || segment[^1] == '-') return false;
#endif
            }

            return true;
        }


        /// <summary>
        /// 验证字符串是否仅包含中文，包括简/繁体。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含中文，包括简/繁体则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsChinese(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            foreach (char c in str)
            {
                if ((c < 0x4E00 || c > 0x9FFF) && (c < 0x3400 || c > 0x4DB5) && (c < 0xE050 || c > 0xE1EC) && (c < 0xE815 || c > 0xE864) && (c < 0xF900 || c > 0xFA2D))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证字符串是否仅包含简体中文。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含简体中文则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsSimplifiedChinese(this string str)
        {
            //TODO:myun
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证字符串是否仅包含繁体中文。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含繁体中文则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsTraditionalChinese(this string str)
        {
            //TODO:myun
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证字符串是否仅包含中文生僻字。
        /// </summary>
        /// <param name="str">需要验证的字符串。</param>
        /// <returns>如果字符串仅包含中文生僻字则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsUncommonChinese(this string str)
        {
            //TODO:myun
            throw new NotImplementedException();
        }


        /// <summary>
        /// 验证字符串是否是不安全密码字符串。
        /// </summary>
        /// <param name="str">密码字符串。</param>
        /// <param name="minLength">密码最小长度，如果密码小于此长度则认定为不安全密码。</param>
        /// <param name="allowPureDigit">允许纯数字被认定为安全密码。</param>
        /// <param name="allowPureLetter">允许纯字母被认定为安全密码（注意：大小写混合不会被认定为纯字母）。</param>
        /// <returns>如果密码字符串被认定为不安全密码则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsUnsafePassword(this string str, byte minLength, bool allowPureDigit, bool allowPureLetter)
        {
            if (String.IsNullOrEmpty(str) || str.Length < minLength) return true;

            if (!allowPureDigit && StringVerification.IsSBCDigit(str)) return true;
            if (!allowPureLetter && (StringVerification.IsSBCLowercaseLetters(str) || StringVerification.IsSBCUppercaseLetters(str))) return true;

            int length = str.Length - 1;
            float lengthf = length;
            float sameCount = 0f; //相同字符统计
            float sequentialCount = 0f; //连续性字符统计
            HashSet<char> hashSet = new HashSet<char>();
            char a, b;

            for (int i = length; i > 0; i--)
            {
                a = str[i];
                b = str[i - 1];

                if (hashSet.Count < 5 && !hashSet.Contains(a))
                {
                    hashSet.Add(a);
                }

                if (a == b)
                {
                    sameCount++;
                }
                else if ((a == (b + 1) || a == (b - 1)))
                {
                    sequentialCount++;
                }
            }

            return (hashSet.Count < 5 && hashSet.Count != minLength) || sameCount / lengthf > 0.35f || sequentialCount / lengthf > 0.4f;
        }


        /// <summary>
        /// 验证字符串是否是 <c>GUID</c>（xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx）字符串。
        /// </summary>
        /// <param name="str">需要验证的字符串，不区分大小写。</param>
        /// <returns>字符串符合 <c>GUID</c> 的字符表现形式则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsGUID(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length != 36) return false;

            int[] lineIndexs = { 8, 13, 18, 23 }; //GUID字符串表现形式中几个横线“-”的索引位置。
            int index = lineIndexs.Length - 1; //表示 lineIndexs 数组的下标。
            int lineIndex = lineIndexs[index--]; //表示下一个横线“-”的索引位置。
            char c;

            for (int i = str.Length - 1; i > -1; i--)
            {
                c = str[i];
                if (i == lineIndex)
                {
                    if (c == '-')
                    {
                        if (index > -1) lineIndex = lineIndexs[index--];
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

#if NET5_0_OR_GREATER
                if (c is (< 'a' or > 'f') and (< 'A' or > 'F') and (< '0' or > '9'))
#else
                if ((c < 'a' || c > 'f') && (c < 'A' || c > 'F') && (c < '0' || c > '9'))
#endif
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 验证字符串是否是十六进制字符串。
        /// </summary>
        /// <param name="str">需要验证的字符串，不区分大小写。</param>
        /// <returns>字符串是有效的十六进制字符串则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsHex(this string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            if (str.Length % 2 != 0) return false;

            for (int i = str.Length - 1; i > -1; i--)
            {
                char c = str[i];
#if NET5_0_OR_GREATER
                if (c is (< 'a' or > 'f') and (< 'A' or > 'F') and (< '0' or > '9'))
#else
                if ((c < 'a' || c > 'f') && (c < 'A' || c > 'F') && (c < '0' || c > '9'))
#endif
                {
                    return false;
                }
            }

            return true;
        }
    }
}
