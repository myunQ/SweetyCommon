/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          TextComparer
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sweety.Common.Verification;


    /// <summary>
    /// 文本比较器
    /// </summary>
    public class TextComparer
    {
        public TextComparer()
        {
            
        }

        public TextComparer(int affirmSameMinLength, bool ignoreCase)
        {
            AffirmSameMinLength = affirmSameMinLength;
            IgnoreCase = ignoreCase;
        }

        /// <summary>
        /// 在两个字符串中的子字符串的相同字符数量大于等于此属性的值，则认为是相同的子字符串。
        /// </summary>
        public int AffirmSameMinLength { get; set; } = 1;
        /// <summary>
        /// 获取或设置比较是否忽略英文字母的大小写。
        /// </summary>
        public bool IgnoreCase { get; set; } = false;



        /// <summary>
        /// 从字符串 <paramref name="a"/> 和 <paramref name="b"/> 中提取相同的部分。
        /// </summary>
        /// <param name="a">要进行比较的第一个字符串。</param>
        /// <param name="b">要进行比较的第二个字符串。</param>
        /// <returns>
        /// 返回相同文本在 <paramref name="a"/> 和 <paramref name="b"/> 中的位置信息。
        /// <see cref="IList{T}"/> 的每个元素是按 <paramref name="a"/> 的顺序在 <paramref name="b"/> 中找到的相同文本的位置信息。
        /// <see cref="LinkedList{T}"/> 的每个节点表示 <paramref name="a"/> 中的同一个位置与 <paramref name="b"/> 中不同位置的相同文本的位置信息。
        /// </returns>
        public IList<LinkedList<SameTextPosition>> ExtractSameParts(string a, string b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                var tmpLinked = new LinkedList<SameTextPosition>();
                tmpLinked.AddFirst(
#if DEBUG
                    new SameTextPosition(0, 0, a.Length, a, b)
#else
                    new SameTextPosition(0, 0, a.Length)
#endif
                    );

                return new List<LinkedList<SameTextPosition>> {tmpLinked};
            }
            if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return new List<LinkedList<SameTextPosition>>();

            List<LinkedList<SameTextPosition>> result = new List<LinkedList<SameTextPosition>>();

            for (int i = 0, j = a.Length; i < j; i++)
            {
                LinkedList<SameTextPosition> linkedList = null;

                for (int x = 0, y = b.Length; x < y; x++)
                {
                    if (a[i] == b[x] || (IgnoreCase && Char.ToUpperInvariant(a[i]) == Char.ToUpperInvariant(b[x])))
                    {
                        int m = i + 1;
                        int n = x + 1;
                        while (m != j && n != y && (a[m] == b[n] || (IgnoreCase && Char.ToUpperInvariant(a[m]) == Char.ToUpperInvariant(b[n]))))
                        {
                            m++;
                            n++;
                        }

                        if ((m - i) >= AffirmSameMinLength)
                        {
                            if (result.Count > 1 || (result.Count == 1 && linkedList == null))
                            {
                                /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
                                 * 避免已经被提取的字符串的末尾再被提取。例如："Sweety"、"weety"、"eety" 等等。
                                 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

                                bool isDiscard = false;
                                //var tmpNode = result[result.Count - (linkedList == null ? 1 : 2)].First;
                                foreach (var tmpLinked in result)
                                {
                                    var tmpNode = tmpLinked.First;
                                    while (tmpNode != null)
                                    {
                                        if (tmpNode.Value.BIndex + tmpNode.Value.Length == n)
                                        {
                                            isDiscard = true;
                                            break;
                                        }
                                        tmpNode = tmpNode.Next;
                                    }
                                    if (isDiscard) break;
                                }
                                
                                if (isDiscard) continue;
                            }

                            if (linkedList == null)
                            {
                                linkedList = new LinkedList<SameTextPosition>();
                                result.Add(linkedList);
                            }
#if DEBUG
                            linkedList.AddLast(new SameTextPosition(i, x, m - i, a.Substring(i, m - i), b.Substring(x, n - x)));
#else
                            linkedList.AddLast(new SameTextPosition(i, x, m - i));
#endif
                        }
                    }
                }
            }

            return result;
        }


        public IList<TextDifference> ExtractDifferenceParts(string a, string b)
        {
            if (Object.ReferenceEquals(a, b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a)) return new List<TextDifference>() { new TextDifference(bLength: b.Length) };
            if (String.IsNullOrEmpty(b)) return new List<TextDifference>() { new TextDifference(aLength: a.Length) };

            IList<TextDifference> result = new List<TextDifference>();
            var sameParts = ExtractSameParts(a, b);
            if (sameParts.Count > 1)
            {
                SameTextPosition cur;
                SameTextPosition next;
                int i = 0;
                int j = sameParts.Count - 1;
                int aIndex = 0;
                int bIndex = 0;

                foreach (var ss in sameParts)
                {
                    while (ss.First.Next != null)
                    {
                        if (ss.First.Value.Length < ss.First.Next.Value.Length)
                        {
                            ss.RemoveFirst();
                        }
                        else
                        {
                            ss.Remove(ss.First.Next);
                        }
                    }
                }

                do
                {
                    cur = sameParts[i].First.Value;
                    next = sameParts[i + 1].First.Value;
                    
                    bool iscontinue = true;
                    for (int k = i + 1; k <= j; k++)
                    {
                        var tmp = sameParts[k].First.Value;
                        /*
                         *B文本下一个相同点的位置在当前相同点的左边或之内   并且    A文本下一个相同点在当前相同点的右边   这样的话就形成了交叉
                         */
                        if (cur.BIndex + cur.Length > tmp.BIndex && cur.AIndex + cur.Length <= tmp.AIndex)
                        {
                            if ((cur.Length < tmp.Length)
                                || (aIndex > cur.AIndex || bIndex > cur.BIndex)
                                || (cur.AIndex < tmp.AIndex && tmp.AIndex <= tmp.BIndex && tmp.BIndex < cur.BIndex))
                            {
                                iscontinue = false;
                                break;
                            }
                        }
                    }

                    /*
                     * B文本下一相同点在当前相同点的右边  或者   A文本下一相同点在当前想同点的左边或之内。
                     * cur.BIndex + cur.Length <= next.BIndex 这个条件排除了 文本A 靠左的字符 跟 文本B 靠右的字符相同，导致错误的略过了中间的相同部分。
                     * cur.AIndex + cur.Length > next.AIndex 这个条件排除了 文本A 和 文本B 中相同的一段短语的中部或末尾跟 文本B 中短语左边的字符相同，导致略过了相同的短语。
                     * 
                     * 这种只跟下一个相同点比较的算法的缺陷是遇到连续满足条件的相同点就会得到错误的文本差异结果。
                     */
                    //if (cur.BIndex + cur.Length <= next.BIndex || cur.AIndex + cur.Length > next.AIndex)
                    if (iscontinue)
                    {
                        if (bIndex > cur.BIndex) continue;

                        if (cur.AIndex > 0 && cur.BIndex > 0)
                        {
                            result.Add(new TextDifference(aIndex, cur.AIndex - aIndex, bIndex, cur.BIndex - bIndex));
                            aIndex = cur.AIndex + cur.Length;
                            bIndex = cur.BIndex + cur.Length;
                        }
                        else if (cur.AIndex == 0 && cur.BIndex == 0)
                        {
                            aIndex = bIndex = cur.Length;
                        }
                        else if (cur.AIndex == 0)
                        {
                            result.Add(new TextDifference(aIndex, 0, bIndex, cur.BIndex - bIndex));
                            aIndex = cur.Length;
                            bIndex = cur.BIndex + cur.Length;
                        }
                        else
                        {
                            result.Add(new TextDifference(aIndex, cur.AIndex - aIndex, bIndex, 0));
                            aIndex = cur.AIndex + cur.Length;
                            bIndex = cur.Length;
                        }
                    }
                }
                while (++i < j && (a.Length > aIndex || b.Length > bIndex));

                //这里 cur 存储的是倒数第二个元素，next 指向的是最后一个元素。
                if (cur.BIndex + cur.Length < next.BIndex)
                {
                    cur = next;
                    
                    result.Add(new TextDifference(aIndex, cur.AIndex - aIndex, bIndex, cur.BIndex - bIndex));
                    aIndex = cur.AIndex + cur.Length;
                    bIndex = cur.BIndex + cur.Length;
                }

                if (a.Length > aIndex || b.Length > bIndex)
                {
                    result.Add(new TextDifference(
                        aIndex,
                        a.Length - aIndex,
                        bIndex,
                        b.Length - bIndex));
                }
            }
            else if (sameParts.Count == 1)
            {
                SameTextPosition position = sameParts[0].First.Value;

                if (position.AIndex > 0 && position.BIndex > 0)
                {
                    result.Add(new TextDifference(0, position.AIndex, 0, position.BIndex));
                    if (a.Length > (position.AIndex + position.Length) || b.Length > (position.BIndex + position.Length))
                    {
                        result.Add(new TextDifference(
                        position.AIndex + position.Length,
                        a.Length - (position.AIndex + position.Length),
                        position.BIndex + position.Length,
                        b.Length - (position.BIndex + position.Length)));
                    }
                }
                else if (position.AIndex == 0 && position.BIndex == 0)
                {
                    result.Add(new TextDifference(
                        position.Length,
                        a.Length - position.Length,
                        position.Length,
                        b.Length - position.Length));
                }
                else if (position.AIndex == 0)
                {
                    result.Add(new TextDifference(0, 0, 0, position.BIndex));
                    result.Add(new TextDifference(
                        position.Length,
                        a.Length - position.Length,
                        position.BIndex + position.Length,
                        b.Length - (position.BIndex + position.Length)));
                }
                else
                {
                    result.Add(new TextDifference(0, position.AIndex, 0, 0));
                    result.Add(new TextDifference(
                        position.AIndex + position.Length,
                        a.Length - (position.AIndex + position.Length),
                        position.Length,
                        b.Length - position.Length));
                }
            }
            else
            {
                result.Add(new TextDifference(0, a.Length, 0, b.Length));
            }

            return result;
        }

        public IList<TextDifference> ExtractDifferenceParts_备用(string a, string b)
        {
            if (Object.ReferenceEquals(a, b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return new List<TextDifference>();
            if (String.IsNullOrEmpty(a)) return new List<TextDifference>() { new TextDifference(bLength: b.Length) };
            if (String.IsNullOrEmpty(b)) return new List<TextDifference>() { new TextDifference(aLength: a.Length) };

            IList<TextDifference> result = new List<TextDifference>();
            var sameParts = ExtractSameParts(a, b);
            if (sameParts.Count > 1)
            {
                SameTextPosition cur;
                SameTextPosition next;
                int i = 0;
                int j = sameParts.Count - 1;
                int aIndex = 0;
                int bIndex = 0;

                foreach (var ss in sameParts)
                {
                    while (ss.First.Next != null)
                    {
                        if (ss.First.Value.Length < ss.First.Next.Value.Length)
                        {
                            ss.RemoveFirst();
                        }
                        else
                        {
                            ss.Remove(ss.First.Next);
                        }
                    }
                }

                do
                {
                    cur = sameParts[i].First.Value;
                    next = sameParts[i + 1].First.Value;
                    
                    bool iscontinue = true;
                    for (int k = i + 1; k <= j; k++)
                    {
                        var tmp = sameParts[k].First.Value;
                        /*
                         *B文本下一个相同点的位置在当前相同点的左边或之内   并且    A文本下一个相同点在当前相同点的右边   这样的话就形成了交叉
                         */
                        if (cur.BIndex + cur.Length > tmp.BIndex && cur.AIndex + cur.Length <= tmp.AIndex)
                        {
                            /*
                             * A文本已收录当先相同点  并且  B文本正准备收录当前相同点（当前相同点还未收录）  这样的话放弃当前相同点
                             * 
                             * A文本如果收录当前相同点后依然在下一相同点左边    并且     B文本如果收录当前相同点后依然在下一相同点左边  这样的话放弃当前相同点
                             */
                            if ((aIndex > cur.AIndex && bIndex == cur.BIndex)
                                || (cur.AIndex < tmp.AIndex && tmp.AIndex <= tmp.BIndex && tmp.BIndex < cur.BIndex)
                                || (aIndex + cur.Length < tmp.AIndex && bIndex + cur.Length < tmp.BIndex))
                            {
                                iscontinue = false;
                                break;
                            }
                        }
                    }

                    /*
                     * B文本下一相同点在当前相同点的右边  或者   A文本下一相同点在当前想同点的左边或之内。
                     * cur.BIndex + cur.Length <= next.BIndex 这个条件排除了 文本A 靠左的字符 跟 文本B 靠右的字符相同，导致错误的略过了中间的相同部分。
                     * cur.AIndex + cur.Length > next.AIndex 这个条件排除了 文本A 和 文本B 中相同的一段短语的中部或末尾跟 文本B 中短语左边的字符相同，导致略过了相同的短语。
                     * 
                     * 这种只跟下一个相同点比较的算法的缺陷是遇到连续满足条件的相同点就会得到错误的文本差异结果。
                     */
                    //if (cur.BIndex + cur.Length <= next.BIndex || cur.AIndex + cur.Length > next.AIndex)
                    if (iscontinue)
                    {
                        if (bIndex > cur.BIndex) continue;

                        if (cur.AIndex > 0 && cur.BIndex > 0)
                        {
                            result.Add(new TextDifference(aIndex, cur.AIndex - aIndex, bIndex, cur.BIndex - bIndex));
                            aIndex = cur.AIndex + cur.Length;
                            bIndex = cur.BIndex + cur.Length;
                        }
                        else if (cur.AIndex == 0 && cur.BIndex == 0)
                        {
                            aIndex = bIndex = cur.Length;
                        }
                        else if (cur.AIndex == 0)
                        {
                            result.Add(new TextDifference(aIndex, 0, bIndex, cur.BIndex - bIndex));
                            aIndex = cur.Length;
                            bIndex = cur.BIndex + cur.Length;
                        }
                        else
                        {
                            result.Add(new TextDifference(aIndex, cur.AIndex - aIndex, bIndex, 0));
                            aIndex = cur.AIndex + cur.Length;
                            bIndex = cur.Length;
                        }
                    }
                }
                while (++i < j && (a.Length > aIndex || b.Length > bIndex));

                //这里 cur 存储的是倒数第二个元素，next 指向的是最后一个元素。
                if (cur.BIndex + cur.Length < next.BIndex)
                {
                    cur = next;
                    
                    result.Add(new TextDifference(aIndex, cur.AIndex - aIndex, bIndex, cur.BIndex - bIndex));
                    aIndex = cur.AIndex + cur.Length;
                    bIndex = cur.BIndex + cur.Length;
                }

                if (a.Length > aIndex || b.Length > bIndex)
                {
                    result.Add(new TextDifference(
                        aIndex,
                        a.Length - aIndex,
                        bIndex,
                        b.Length - bIndex));
                }
            }
            else if (sameParts.Count == 1)
            {
                SameTextPosition position = sameParts[0].First.Value;

                if (position.AIndex > 0 && position.BIndex > 0)
                {
                    result.Add(new TextDifference(0, position.AIndex, 0, position.BIndex));
                    if (a.Length > (position.AIndex + position.Length) || b.Length > (position.BIndex + position.Length))
                    {
                        result.Add(new TextDifference(
                        position.AIndex + position.Length,
                        a.Length - (position.AIndex + position.Length),
                        position.BIndex + position.Length,
                        b.Length - (position.BIndex + position.Length)));
                    }
                }
                else if (position.AIndex == 0 && position.BIndex == 0)
                {
                    result.Add(new TextDifference(
                        position.Length,
                        a.Length - position.Length,
                        position.Length,
                        b.Length - position.Length));
                }
                else if (position.AIndex == 0)
                {
                    result.Add(new TextDifference(0, 0, 0, position.BIndex));
                    result.Add(new TextDifference(
                        position.Length,
                        a.Length - position.Length,
                        position.BIndex + position.Length,
                        b.Length - (position.BIndex + position.Length)));
                }
                else
                {
                    result.Add(new TextDifference(0, position.AIndex, 0, 0));
                    result.Add(new TextDifference(
                        position.AIndex + position.Length,
                        a.Length - (position.AIndex + position.Length),
                        position.Length,
                        b.Length - position.Length));
                }
            }
            else
            {
                result.Add(new TextDifference(0, a.Length, 0, b.Length));
            }

            return result;
        }
    }
}
