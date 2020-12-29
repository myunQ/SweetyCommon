/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          TextDifference
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Comparer
{
    using System;


    /// <summary>
    /// 表示两个文本内容的差异之处。
    /// </summary>
    //public readonly struct TextDifference
    public class TextDifference
    {
        public TextDifference(int aIndex = 0, int aLength = 0, int bIndex = 0, int bLength = 0)
        {
            AIndex = aIndex;
            ALength = aLength;
            BIndex = bIndex;
            BLength = bLength;
        }

        /// <summary>
        /// 字符串 <c>a</c> 与字符串 <c>b</c> 不同的字符的位置索引。
        /// </summary>
        public int AIndex { get; }
        /// <summary>
        /// 字符串 <c>a</c> 不同于字符串 <c>b</c> 的字符数。
        /// 当值为 <c>0</c> 时表示字符串 <c>b</c> 里有，但是在字符串 <c>a</c> 里没有的字符串。
        /// </summary>
        public int ALength { get; }
        /// <summary>
        /// 字符串 <c>b</c> 与字符串 <c>a</c> 不同的字符的位置索引。
        /// </summary>
        public int BIndex { get; }
        /// <summary>
        /// 字符串 <c>b</c> 不同于字符串 <c>a</c> 的字符数。
        /// 当值为 <c>0</c> 时表示字符串 <c>a</c> 里有，但是在字符串 <c>b</c> 里没有的字符串。
        /// </summary>
        public int BLength { get; }

        /// <summary>
        /// 重写了父类方法。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return AIndex.GetHashCode() + ALength.GetHashCode() + BIndex.GetHashCode() + BLength.GetHashCode();
        }
        /// <summary>
        /// 重写了父类方法。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Index:[{AIndex},{BIndex}],Length:[{ALength},{BLength}]";
        }
        /// <summary>
        /// 重写了父类方法。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TextDifference);
        }
        /// <summary>
        /// 比较结构体是否相等。
        /// </summary>
        /// <param name="b">要比较的结构体。</param>
        /// <returns>如果相等则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public bool Equals(TextDifference b)
        {
            if (b == null) return false;
            if (Object.ReferenceEquals(this, b)) return true;

            return AIndex == b.AIndex
                && BIndex == b.BIndex
                && ALength == b.ALength
                && BLength == b.BLength;
        }
    }
}
