/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          SameTextPosition
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Comparer
{
    using System;


    /// <summary>
    /// 表示两个文本相同地方。
    /// </summary>
    //public readonly struct SameTextPosition
    public class SameTextPosition
    {
#if DEBUG
        public SameTextPosition(int aIndex, int bIndex, int length, string astr, string bstr)
        {
            AIndex = aIndex;
            BIndex = bIndex;
            Length = length;
            AString = astr;
            BString = bstr;
        }
#else
        public SameTextPosition(int aIndex, int bIndex, int length)
        {
            AIndex = aIndex;
            BIndex = bIndex;
            Length = length;
        }
#endif

        /// <summary>
        /// 字符串 <c>a</c> 与字符串 <c>b</c> 相同的字符的起始位置索引。
        /// </summary>
        public int AIndex { get; }
        /// <summary>
        /// 字符串 <c>b</c> 与字符串 <c>a</c> 相同的字符的起始位置索引。
        /// </summary>
        public int BIndex { get; }
        /// <summary>
        /// 相同的字符数量。
        /// </summary>
        public int Length { get; }

#if DEBUG
        public string AString { get; }

        public string BString { get; }

        public bool Same => AString == BString;
#endif

        /// <summary>
        /// 重写了父类方法。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return AIndex.GetHashCode() + BIndex.GetHashCode() + Length.GetHashCode();
        }
        /// <summary>
        /// 重写了父类方法。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
#if DEBUG
            return $"Index:[{AIndex},{BIndex}],Length:{Length},Same:{Same},Str:{AString}";
#else
            return $"Index:[{AIndex},{BIndex}],Length:{Length}";
#endif
        }
        /// <summary>
        /// 重写了父类方法。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SameTextPosition);
        }
        /// <summary>
        /// 比较结构体是否相等。
        /// </summary>
        /// <param name="b">要比较的结构体。</param>
        /// <returns>如果相等则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public bool Equals(SameTextPosition b)
        {
            if (b == null) return false;
            if (Object.ReferenceEquals(this, b)) return true;

            return AIndex == b.AIndex
                && BIndex == b.BIndex
                && Length == b.Length;
        }
    }
}
