/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          TextDifferenceResultProcessing
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 文本差异结果加工程序
    /// </summary>
    public class TextDifferenceResultProcessing
    {
        /// <summary>
        /// 获取或设置，不同文本的起始标签。默认值：&lt;diff&gt;。
        /// </summary>
        public string DifferentStartLable { get; set; } = "<diff>";
        /// <summary>
        /// 获取或设置，不同文本的结束标签。默认值：&lt;/diff&gt;。
        /// </summary>
        public string DifferentEndLable { get; set; } = "</diff>";

        /// <summary>
        /// 获取或设置，多出的文本的起始标签。默认值：&lt;extra&gt;。
        /// </summary>
        public string ExtraStartLabel { get; set; } = "<extra>";
        /// <summary>
        /// 获取或设置，多出的文本的结束标签。默认值：&lt;/extra&gt;。
        /// </summary>
        public string ExtraEndLabel { get; set; } = "</extra>";

        /// <summary>
        /// 获取或设置，缺少的文本的起始标签。默认值：&lt;lack&gt;。
        /// </summary>
        public string LackingStartLabel { get; set; } = "<lack>";
        /// <summary>
        /// 获取或设置，缺少的文本的结束标签。默认值：&lt;/lack&gt;。
        /// </summary>
        public string LackingEndLabel { get; set; } = "</lack>";

        /// <summary>
        /// 根据差异信息将字符串的差异标记出来。
        /// </summary>
        /// <param name="differences"><paramref name="a"/> 和 <paramref name="b"/> 的差异信息。</param>
        /// <param name="a">被比较的字符串。</param>
        /// <param name="b">比较的字符串。</param>
        /// <param name="aDiff">被标记出与 <paramref name="b"/> 存在差异之处的 <paramref name="a"/>。</param>
        /// <returns><paramref name="aDiff"/> 被赋予有效值则为 <c>true</c>，<paramref name="aDiff"/> 被赋予 <c>null</c> 则为 <c>false</c>。</returns>
        public bool TryMarkDifferences(IEnumerable<TextDifference> differences, string a, string b, out string aDiff)
        {
            if (differences == null || !differences.Any() || String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                aDiff = null;
                return false;
            }

            if (String.IsNullOrEmpty(a))
            {
                if (differences.Count() == 1)
                {
                    var tmpDiff = differences.First();
                    if (tmpDiff.BIndex == 0 && tmpDiff.BLength == b.Length)
                    {
                        aDiff = LackingStartLabel + b + LackingEndLabel;
                        return true;
                    }
                }
                aDiff = null;
                return false;
            }

            if (String.IsNullOrEmpty(b))
            {
                if (differences.Count() == 1)
                {
                    var tmpDiff = differences.First();
                    if (tmpDiff.AIndex == 0 && tmpDiff.ALength == a.Length)
                    {
                        aDiff = ExtraStartLabel + a + ExtraEndLabel;
                        return true;
                    }
                }
                
                aDiff = null;
                return false;
            }


            StringBuilder buildA = new StringBuilder(a.Length + b.Length + (DifferentStartLable?.Length + DifferentEndLable?.Length + ExtraStartLabel?.Length + ExtraEndLabel?.Length + LackingStartLabel?.Length + LackingEndLabel?.Length) ?? 0);
            
            var diff = differences.First();
            if (diff.AIndex > 0) buildA.Append(a.Substring(0, diff.AIndex));

            bool notFirst = false;
            foreach (var di in differences)
            {
                if (notFirst)
                {
                    buildA.Append(a.Substring(diff.AIndex + diff.ALength, di.AIndex - (diff.AIndex + diff.ALength)));
                }
                else
                {
                    notFirst = true;
                }

                if (di.ALength > 0 && di.BLength > 0)
                {
                    buildA.Append(DifferentStartLable);
                    buildA.Append(a.Substring(di.AIndex, di.ALength));
                    buildA.Append(DifferentEndLable);
                }
                else if (di.ALength > 0)
                {
                    buildA.Append(ExtraStartLabel);
                    buildA.Append(a.Substring(di.AIndex, di.ALength));
                    buildA.Append(ExtraEndLabel);
                }
                else
                {
                    buildA.Append(LackingStartLabel);
                    buildA.Append(b.Substring(di.BIndex, di.BLength));
                    buildA.Append(LackingEndLabel);
                }

                diff = di;
            }

            diff = differences.Last();
            if (a.Length > (diff.AIndex + diff.ALength)) buildA.Append(a.Substring(diff.AIndex + diff.ALength));


            aDiff = buildA.ToString();
            return true;
        }

        /// <summary>
        /// 根据差异信息将字符串的差异标记出来。
        /// </summary>
        /// <param name="differences"><paramref name="a"/> 和 <paramref name="b"/> 的差异信息。</param>
        /// <param name="a">被比较的字符串。</param>
        /// <param name="b">比较的字符串。</param>
        /// <param name="aDiff">被标记出与 <paramref name="b"/> 存在差异之处的 <paramref name="a"/>。</param>
        /// <param name="bDiff">被标记出与 <paramref name="a"/> 存在差异之处的 <paramref name="b"/>。</param>
        /// <returns><paramref name="aDiff"/> 和 <paramref name="bDiff"/> 被赋予有效值则为 <c>true</c>，<paramref name="aDiff"/> 和 <paramref name="bDiff"/> 被赋予 <c>null</c> 则为 <c>false</c>。</returns>
        public bool TryMarkDifferences(IEnumerable<TextDifference> differences, string a, string b, out string aDiff, out string bDiff)
        {
            if (differences == null || !differences.Any() || String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                aDiff = bDiff = null;
                return false;
            }

            if (String.IsNullOrEmpty(a))
            {
                aDiff = LackingStartLabel + b + LackingEndLabel;
                bDiff = ExtraStartLabel + b + ExtraEndLabel;
                return true;
            }

            if (String.IsNullOrEmpty(b))
            {
                aDiff = ExtraStartLabel + a + ExtraEndLabel;
                bDiff = LackingStartLabel + a + LackingEndLabel;
                return true;
            }


            StringBuilder buildA = new StringBuilder(a.Length + b.Length + (DifferentStartLable?.Length + DifferentEndLable?.Length + ExtraStartLabel?.Length + ExtraEndLabel?.Length + LackingStartLabel?.Length + LackingEndLabel?.Length) ?? 0);
            StringBuilder buildB = new StringBuilder(buildA.Capacity);
            
            var diff = differences.First();
            if (diff.AIndex > 0) buildA.Append(a.Substring(0, diff.AIndex));
            if (diff.BIndex > 0) buildB.Append(b.Substring(0, diff.BIndex));

            bool notFirst = false;
            foreach (var di in differences)
            {
                if (notFirst)
                {
                    buildA.Append(a.Substring(diff.AIndex + diff.ALength, di.AIndex - (diff.AIndex + diff.ALength)));
                    buildB.Append(b.Substring(diff.BIndex + diff.BLength, di.BIndex - (diff.BIndex + diff.BLength)));
                }
                else
                {
                    notFirst = true;
                }

                if (di.ALength > 0 && di.BLength > 0)
                {
                    buildA.Append(DifferentStartLable);
                    buildA.Append(a.Substring(di.AIndex, di.ALength));
                    buildA.Append(DifferentEndLable);

                    buildB.Append(DifferentStartLable);
                    buildB.Append(b.Substring(di.BIndex, di.BLength));
                    buildB.Append(DifferentEndLable);
                }
                else if (di.ALength > 0)
                {
                    string con = a.Substring(di.AIndex, di.ALength);
                    buildA.Append(ExtraStartLabel);
                    buildA.Append(con);
                    buildA.Append(ExtraEndLabel);

                    buildB.Append(LackingStartLabel);
                    buildB.Append(con);
                    buildB.Append(LackingEndLabel);
                }
                else
                {
                    string con = b.Substring(di.BIndex, di.BLength);
                    buildA.Append(LackingStartLabel);
                    buildA.Append(con);
                    buildA.Append(LackingEndLabel);

                    buildB.Append(ExtraStartLabel);
                    buildB.Append(con);
                    buildB.Append(ExtraEndLabel);
                }

                diff = di;
            }

            

            diff = differences.Last();
            if (a.Length > (diff.AIndex + diff.ALength)) buildA.Append(a.Substring(diff.AIndex + diff.ALength));
            if (b.Length > (diff.BIndex + diff.BLength)) buildB.Append(b.Substring(diff.BIndex + diff.BLength));
            

            aDiff = buildA.ToString();
            bDiff = buildB.ToString();
            return true;
        }
    }
}
