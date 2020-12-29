/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          TextContrast
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// 文本差异比较
    /// </summary>
    public class TextContrast
    {
        public TextContrast()
        {
        }

        /// <summary>
        /// 获取或设置需要复查的差异字符串长度。
        /// </summary>
        public int DifferenceStringLengthThresholdToBeReviewed { get; set; } = 10;
        /// <summary>
        /// 获取或设置比较是否忽略英文字母的大小写。
        /// </summary>
        public bool IgnoreCase { get; set; } = false;
        /// <summary>
        /// 在两个字符串中的子字符串的相同字符数量大于等于此属性的值，则认为是相同的子字符串。
        /// </summary>
        public int AffirmSameMinLength { get; set; } = 1;

        /// <summary>
        /// 比较两个字符串之间的差异。
        /// </summary>
        /// <param name="a">要进行比较的其中一个字符串。</param>
        /// <param name="b">要进行比较的另一个字符串。</param>
        /// <returns>两个字符串之间的差异。如果 <paramref name="a"/> 和 <paramref name="b"/> 相同则返回零长度的列表。</returns>
        public IList<TextDifference> Contrast(string a, string b)
        {
            if (Object.ReferenceEquals(a, b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a)) return new List<TextDifference>() { new TextDifference(bLength: b.Length) };
            if (String.IsNullOrEmpty(b)) return new List<TextDifference>() { new TextDifference(aLength: a.Length) };

            IList<TextDifference> result = new List<TextDifference>();
            int acur = 0;
            int bcur = 0;
            int aIndex = 0;
            int bIndex = 0;

            for (; acur < a.Length; acur++)
            {
                if (bcur == b.Length)
                {
                    if (a[acur] == a[acur - 1]) continue;

                    bcur = bIndex;
                }

                for (; bcur < b.Length; bcur++)
                {
                    if (a[acur] == b[bcur] || (IgnoreCase && Char.ToUpperInvariant(a[acur]) == Char.ToUpperInvariant(b[bcur])))
                    {
                        int m = acur + 1;
                        int n = bcur + 1;
                        while (m != a.Length && n != b.Length && (a[m] == b[n] || (IgnoreCase && Char.ToUpperInvariant(a[m]) == Char.ToUpperInvariant(b[n]))))
                        {
                            m++;
                            n++;
                        }

                        if ((m - acur) >= AffirmSameMinLength)
                        {
                            tryAddDifference(a, b, ref aIndex, ref acur, ref bIndex, ref bcur, DifferenceStringLengthThresholdToBeReviewed, result, IgnoreCase, AffirmSameMinLength);

                            aIndex = acur + 1;
                            bIndex = bcur + 1;
                            bcur++;
                            break;
                        }
                    }
                }
            }

            tryAddDifference(a, b, ref aIndex, ref acur, ref bIndex, ref bcur, DifferenceStringLengthThresholdToBeReviewed, result, IgnoreCase, AffirmSameMinLength);

            return result;


            /// <summary>
            /// 如果 <paramref name=sub_a/> 和 <paramref name=sub_b/> 在当前位置存在差异则将差异信息添加到 <paramref name="differences"/> 里。
            /// </summary>
            /// <param name=sub_a>要进行比较的其中一个字符串。</param>
            /// <param name=sub_b>要进行比较的另一个字符串。</param>
            /// <param name="sub_aIndex"><paramref name=sub_a/> 相比 <paramref name=sub_b/> 的最近一个未添加的差一点起始字符索引。</param>
            /// <param name="sub_acur"><paramref name=sub_a/> 相比 <paramref name=sub_b/> 的最近一个未添加的差一点的结束字符索引 <c>+ 1</c>。</param>
            /// <param name="sub_bIndex"><paramref name=sub_b/> 相比 <paramref name=sub_a/> 的最近一个未添加的差一点起始字符索引。</param>
            /// <param name="sub_bcur"><paramref name=sub_b/> 相比 <paramref name=sub_a/> 的最近一个未添加的差一点的结束字符索引 <c>+ 1</c>。</param>
            /// <param name="sub_differenceLengthThreshold">当一个地方的差异字符串长度超过此值，则进行反向复查。</param>
            /// <param name="sub_differences">存储差异点信息的列表。</param>
#if NETSTANDARD2_0
            void tryAddDifference(string sub_a, string sub_b, ref int sub_aIndex, ref int sub_acur, ref int sub_bIndex, ref int sub_bcur, int sub_differenceLengthThreshold, IList<TextDifference> sub_differences, bool ignoreCase, int affirmSameMinLength)
#else
            static void tryAddDifference(string sub_a, string sub_b, ref int sub_aIndex, ref int sub_acur, ref int sub_bIndex, ref int sub_bcur, int sub_differenceLengthThreshold, IList<TextDifference> sub_differences, bool ignoreCase, int affirmSameMinLength)
#endif
            {
                if (sub_aIndex < sub_acur || sub_bIndex < sub_bcur)
                {
                    if ((sub_acur < sub_a.Length && sub_a[sub_acur] == ' ') || sub_acur - sub_aIndex > sub_differenceLengthThreshold || sub_bcur - sub_bIndex > sub_differenceLengthThreshold)
                    {
                        var diff = assistContrast(sub_b, sub_a, sub_bIndex, sub_aIndex, ignoreCase, affirmSameMinLength);

                        //if ((sub_acur > (diff.BIndex + diff.BLength) || sub_bcur > (diff.AIndex + diff.ALength)) && (diff.ALength > 0 || diff.BLength > 0))
                        if (((sub_acur > (diff.BIndex + diff.BLength) && sub_acur > (diff.AIndex + diff.ALength)) || (sub_bcur > (diff.AIndex + diff.ALength) && sub_bcur > (diff.BIndex + diff.BLength))) && (diff.ALength > 0 || diff.BLength > 0))
                        {
                            sub_aIndex = diff.BIndex;
                            sub_bIndex = diff.AIndex;
                            sub_acur = sub_aIndex + diff.BLength;
                            sub_bcur = sub_bIndex + diff.ALength;
                        }
                    }

                    sub_differences.Add(new TextDifference(sub_aIndex, sub_acur - sub_aIndex, sub_bIndex, sub_bcur - sub_bIndex));
                }
                else if (sub_a.Length == sub_aIndex && sub_b.Length > sub_bIndex)
                {
                    sub_differences.Add(new TextDifference(sub_aIndex, 0, sub_bIndex, sub_b.Length - sub_bIndex));
                }
            }

            /// <summary>
            /// <see cref="Contrast(string, string)"/> 的辅助方法。目的是为了提高差异对比的准确性。
            /// </summary>
            /// <remarks>
            /// 下面说明一下这个方法的作用，假如有以下两个字符串：
            /// <code>
            /// string a = "差异是一个汉语词语，拼音为cha yi，意***思是指区别，不同。";
            /// string b = "差异是一个汉语词语，拼音为chā yì，意思是指区别，*不同。"
            /// </code>
            /// 如果只用 <see cref="Contrast(string, string)"/> 方法比较，得到的结果将会是：
            /// <code>
            /// //说明：
            /// //     <del> 标签表示相比缺少的字符串。
            /// //     <ins> 标签表示相比新增的字符串。
            /// //     <diff> 标签表示相比不同的字符串。
            /// 
            /// string a_diff_b = "差异是一个汉语词语，拼音为ch<diff>a</diff> y<diff>i</diff>，意<del>思是指区别，</del>*<ins>**思是指区别，</ins>不同。";
            /// string b_diff_a = "差异是一个汉语词语，拼音为ch<diff>ā</diff> y<diff>ì</diff>，意<ins>思是指区别，</ins>*<del>**思是指区别，</del>不同。";
            /// </code>
            /// 以上结果明显不是我们想要的。为了避免这样的错误结果，引入了此方法，
            /// 当差异字符串过长时使用此方法从差一点开始处将两个字符串互换比较关系（即：假如之前是字符串 a 和字符串 b 进行比较的话就换成字符串 b 和字符串 a 进行比较）进行比较。
            /// 引入此方法后的比较结果如下：
            /// <code>
            /// //说明：
            /// //     <del> 标签表示相比缺少的字符串。
            /// //     <ins> 标签表示相比新增的字符串。
            /// //     <diff> 标签表示相比不同的字符串。
            /// 
            /// string a_diff_b = "差异是一个汉语词语，拼音为ch<diff>a</diff> y<diff>i</diff>，意<ins>***</ins>思是指区别，<del>*</del>不同。";
            /// string b_diff_a = "差异是一个汉语词语，拼音为ch<diff>ā</diff> y<diff>ì</diff>，意<del>***</del>思是指区别，<ins>*</ins>不同。";
            /// </code>
            /// </remarks>
            /// <param name="sub_a">要进行比较的其中一个字符串。</param>
            /// <param name="sub_b">要进行比较的另一个字符串。</param>
            /// <param name="sub_acur">从<paramref name="sub_a"/>的这个字符位置开始跟 <paramref name="sub_b"/> 进行差异比较。</param>
            /// <param name="sub_bcur">从<paramref name="sub_b"/>的这个字符位置开始跟 <paramref name="sub_a"/> 进行差异比较。</param>
            /// <returns>从 <paramref name="sub_a"/> 的 <paramref name="sub_acur"/> 开始和 <paramref name="sub_b"/> 从 <paramref name="sub_bcur"/> 开始比较最近一个差异信息。 </returns>
#if NETSTANDARD2_0
            TextDifference assistContrast(string sub_a, string sub_b, int sub_acur, int sub_bcur, bool ignoreCase, int affirmSameMinLength)
#else
            static TextDifference assistContrast(string sub_a, string sub_b, int sub_acur, int sub_bcur, bool ignoreCase, int affirmSameMinLength)
#endif
            {
                int sub_aIndex = sub_acur;
                int sub_bIndex = sub_bcur;

                for (; sub_acur < sub_a.Length; sub_acur++)
                {
                    if (sub_bcur == sub_b.Length)
                    {
                        if (sub_a[sub_acur] == sub_a[sub_acur - 1]) continue;

                        sub_bcur = sub_bIndex;
                    }

                    for (; sub_bcur < sub_b.Length; sub_bcur++)
                    {
                        if (sub_a[sub_acur] == sub_b[sub_bcur] || (ignoreCase && Char.ToUpperInvariant(sub_a[sub_acur]) == Char.ToUpperInvariant(sub_b[sub_bcur])))
                        {

                            int m = sub_acur + 1;
                            int n = sub_bcur + 1;
                            while (m != sub_a.Length && n != sub_b.Length && (sub_a[m] == sub_b[n] || (ignoreCase && Char.ToUpperInvariant(sub_a[m]) == Char.ToUpperInvariant(sub_b[n]))))
                            {
                                m++;
                                n++;
                            }

                            if ((m - sub_acur) >= affirmSameMinLength)
                            {
                                if (sub_aIndex < sub_acur || sub_bIndex < sub_bcur)
                                {
                                    return new TextDifference(sub_aIndex, sub_acur - sub_aIndex, sub_bIndex, sub_bcur - sub_bIndex);
                                }
                                else if (sub_a.Length == sub_aIndex && sub_b.Length > sub_bIndex)
                                {
                                    return new TextDifference(sub_aIndex, 0, sub_bIndex, sub_b.Length - sub_bIndex);
                                }
                            }

                            sub_aIndex = sub_acur + 1;
                            sub_bIndex = sub_bcur + 1;
                            sub_bcur++;
                            break;
                        }
                    }
                }

                return default;
            }
        }


        /// <summary>
        /// 从字符串的末尾开始反向比较两个字符串之间的差异。
        /// </summary>
        /// <param name="a">要进行比较的其中一个字符串。</param>
        /// <param name="b">要进行比较的另一个字符串。</param>
        /// <returns>两个字符串之间的差异。如果 <paramref name="a"/> 和 <paramref name="b"/> 相同则返回零长度的列表。</returns>
        public IList<TextDifference> ReverseContrast(string a, string b)
        {
            if (Object.ReferenceEquals(a, b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a)) return new List<TextDifference>() { new TextDifference(bLength: b.Length) };
            if (String.IsNullOrEmpty(b)) return new List<TextDifference>() { new TextDifference(aLength: a.Length) };

            IList<TextDifference> result = new List<TextDifference>();
            int acur = a.Length - 1;
            int bcur = b.Length - 1;
            int aIndex = acur;
            int bIndex = bcur;

            for (; acur > -1; acur--)
            {
                if (bcur == -1)
                {
                    if (a[acur] == a[acur + 1]) continue;

                    bcur = bIndex;
                }

                for (; bcur > -1; bcur--)
                {
                    if (a[acur] == b[bcur])
                    {
                        tryAddDifference(a, b, ref aIndex, ref acur, ref bIndex, ref bcur, DifferenceStringLengthThresholdToBeReviewed, result);

                        aIndex = acur - 1;
                        bIndex = bcur - 1;
                        bcur--;
                        break;
                    }
                }
            }

            tryAddDifference(a, b, ref aIndex, ref acur, ref bIndex, ref bcur, DifferenceStringLengthThresholdToBeReviewed, result);
            
            return result.Reverse().ToList();



#if NETSTANDARD2_0
            void tryAddDifference(string sub_a, string sub_b, ref int sub_aIndex, ref int sub_acur, ref int sub_bIndex, ref int sub_bcur, int sub_differenceLengthThreshold, IList<TextDifference> sub_differences)
#else
            static void tryAddDifference(string sub_a, string sub_b, ref int sub_aIndex, ref int sub_acur, ref int sub_bIndex, ref int sub_bcur, int sub_differenceLengthThreshold, IList<TextDifference> sub_differences)
#endif
            {
                //有下面这句 "abcdefg" 与 "12345fg" 的比较就会因为 sub_acur 和 sub_bcur 都为 -1 而直接退出的BUG。
                //if (sub_acur < 0 && sub_bcur < 0) return;

                if (sub_aIndex > sub_acur || sub_bIndex > sub_bcur)
                {
                    if ((sub_acur > -1 && sub_a[sub_acur] == ' ') || sub_aIndex - sub_acur > sub_differenceLengthThreshold || sub_bIndex - sub_bcur > sub_differenceLengthThreshold)
                    {
                        var diff = assistContrast(sub_b, sub_a, sub_bIndex, sub_aIndex);

                        //if (sub_acur < (diff.BIndex + diff.BLength) || sub_bcur < (diff.AIndex + diff.ALength))
                        if (((sub_acur < (diff.AIndex + diff.ALength) && sub_acur < (diff.BIndex + diff.BLength)) || (sub_bcur < (diff.AIndex + diff.ALength) && sub_bcur < (diff.BIndex + diff.BLength))) && (diff.ALength > 0 || diff.BLength > 0))
                        {
                            sub_acur = diff.BIndex - 1;
                            sub_bcur = diff.AIndex - 1;
                            sub_aIndex = sub_acur + diff.BLength;
                            sub_bIndex = sub_bcur + diff.ALength;
                        }
                    }

                    sub_differences.Add(new TextDifference(sub_acur + 1, sub_aIndex - sub_acur, sub_bcur + 1, sub_bIndex - sub_bcur));
                }
                else if (-1 == sub_aIndex && -1 < sub_bIndex)
                {
                    sub_differences.Add(new TextDifference(bLength: sub_bcur + 1));
                }
            }

#if NETSTANDARD2_0
            TextDifference assistContrast(string sub_a, string sub_b, int sub_acur, int sub_bcur)
#else
            static TextDifference assistContrast(string sub_a, string sub_b, int sub_acur, int sub_bcur)
#endif
            {
                int sub_aIndex = sub_acur;
                int sub_bIndex = sub_bcur;

                for (; sub_acur > -1; sub_acur--)
                {
                    if (sub_bcur == -1)
                    {
                        if (sub_a[sub_acur] == sub_a[sub_acur + 1]) continue;

                        sub_bcur = sub_bIndex;
                    }

                    for (; sub_bcur > -1; sub_bcur--)
                    {
                        if (sub_a[sub_acur] == sub_b[sub_bcur])
                        {
                            if (sub_aIndex > sub_acur || sub_bIndex > sub_bcur)
                            {
                                return new TextDifference(sub_acur + 1, sub_aIndex - sub_acur, sub_bcur + 1, sub_bIndex - sub_bcur);
                            }
                            else if (-1 == sub_aIndex && -1 < sub_bIndex)
                            {
                                return new TextDifference(bLength: sub_bcur + 1);
                            }

                            sub_aIndex = sub_acur - 1;
                            sub_bIndex = sub_bcur - 1;
                            sub_bcur--;
                            break;
                        }
                    }
                }

                return default;
            }
        }
    }
}
