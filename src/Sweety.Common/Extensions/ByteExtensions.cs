/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  字节类型到其它类型的转换
 * 
 * 
 * Members Index:
 *      static class ByteConvert
 *              public static decimal ToDecimal(this byte[] bytes)
 *              public static string ToHex(this byte[] value)
 *              public static string ToHEX(this byte[] value)
 *              public static bool FastEquals(this byte[] a, byte[] b)
 *              public static bool FastEquals(this ArraySegment<byte> a, ArraySegment<byte> b)
 *              public static bool SlowEquals(this byte[] a, byte[] b)
 *              public static bool SlowEquals(this ArraySegment<byte> a, ArraySegment<byte> b)
 *              private static string ToHex(byte[] value, string format)
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Extensions
{
    using System;


    /// <summary>
    /// <see cref="Byte"/ >以及 <see cref="Byte[]"/> 类型的扩展方法。
    /// </summary>
    public static class ByteExtensions
    {
        private static readonly char[] HEX_DIGITS_LOWER = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private static readonly char[] HEX_DIGITS_UPPER = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };



        /// <summary>
        /// 将 <see cref="byte[]"/> 转换成 <see cref="Decimal"/> 类型的值。
        /// </summary>
        /// <param name="bytes">字节数组。</param>
        /// <param name="startIndex">从 <paramref name="bytes"/> 的什么位置开始读。</param>
        /// <returns><see cref="Decimal"/> 类型的值。</returns>
        /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 等于 <c>null</c> 时引发此异常。</exception>
        /// <exception cref="ArgumentException">当 <paramref name="bytes"/> 的长度小于 16 时引发此异常。</exception>
        public static decimal ToDecimal(this byte[] bytes, int startIndex = 0)
        {
            const int byteArrLen = 16;
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if ((bytes.Length - startIndex) < byteArrLen) throw new ArgumentException(String.Format(Properties.Localization.at_least_XXX_bytes, byteArrLen + startIndex));

            const int intArrLen = 4;
            int[] bits = new int[intArrLen];

            for (int i = 0; i < intArrLen; i++)
            {
                bits[i] = BitConverter.ToInt32(bytes, i * intArrLen + startIndex);
            }

            return new decimal(bits);
        }


        /// <summary>
        /// 将 <c>byte</c> 数组转换成十六进制字符串（小写字母）。
        /// </summary>
        /// <param name="value"><c>byte</c> 数组。</param>
        /// <returns><c>byte</c> 数组转换的十六进制字符串，如果 <c>byte</c> 数组为 <c>null</c> 或零长度则返回 <see cref="String.Empty"/>。</returns>
        public static string ToHex(this byte[] value)
        {
            return ToHex(value, HEX_DIGITS_LOWER);
        }

        /// <summary>
        /// 将 <c>byte</c> 数组转换成十六进制字符串（大写字母）。
        /// </summary>
        /// <param name="value"><c>byte</c> 数组。</param>
        /// <returns><c>byte</c> 数组转换的十六进制字符串，如果 <c>byte</c> 数组为 <c>null</c> 或零长度则返回 <see cref="String.Empty"/>。</returns>
        public static string ToHEX(this byte[] value)
        {
            return ToHex(value, HEX_DIGITS_UPPER);
        }

        /// <summary>
        /// 比较两个 <c>byte[]</c> 是否相等。
        /// </summary>
        /// <param name="a">要进行比较的第一个 <c>byte</c> 数组。</param>
        /// <param name="b">要进行比较的第二个 <c>byte</c> 数组。</param>
        /// <returns>当两个 <c>byte[]</c> 都不为 <c>null</c> 并且长度一致、每个元素的值也一致时返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool FastEquals(this byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            if ((object)a == (object)b) return true;
            if (a.Length != b.Length) return false;

            for (int i = 0, j = a.Length; i < j; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// 比较两个 <see cref="ArraySegment{T}"/> 的每个元素是否相等。
        /// </summary>
        /// <param name="a">要进行比较的第一个 <see cref="ArraySegment{T}"/>。</param>
        /// <param name="b">要进行比较的第二个 <see cref="ArraySegment{T}"/>。</param>
        /// <returns>当两个 <see cref="ArraySegment{T}.Array"/> 都不为 <c>null</c> 并且长度一致、每个元素的值也一致时返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool FastEquals(this ArraySegment<byte> a, ArraySegment<byte> b)
        {
            if (a.Array == null || b.Array == null) return false;
            if (a.Count != b.Count) return false;

            for (int i = 0, j = a.Count; i < j; i++)
            {
                if (a.Array[a.Offset + i] != b.Array[b.Offset + i]) return false;
            }

            return true;
        }

        /// <summary>
        /// 时间恒定的比较两个 <c>byte[]</c> 是否相等。
        /// </summary>
        /// <remarks>
        /// 使用时间恒定的方式比较可以防止根据验证的时间长短来判断前几位是否正确，然后逐步修正最终得到正确的结果。
        /// </remarks>
        /// <param name="a">要进行比较的第一个 <c>byte</c> 数组。</param>
        /// <param name="b">要进行比较的第二个 <c>byte</c> 数组。</param>
        /// <returns>当两个 <c>byte[]</c> 都不为 <c>null</c> 并且长度一致、每个元素的值也一致时返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool SlowEquals(this byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;

            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }

            return diff == 0;
        }

        /// <summary>
        /// 时间恒定的比较两个 <see cref="ArraySegment{T}"/> 是否相等。
        /// </summary>
        /// <remarks>
        /// 使用时间恒定的方式比较两个 <see cref="ArraySegment{T}"/> 可以防止根据验证的时间长短来判断前几位是否正确，然后逐步修正最终得到正确的结果。
        /// </remarks>
        /// <param name="a">要进行比较的第一个 <see cref="ArraySegment{T}"/>。</param>
        /// <param name="b">要进行比较的第二个 <see cref="ArraySegment{T}"/>。</param>
        /// <returns>当两个 <see cref="ArraySegment{T}.Array"/> 都不为 <c>null</c> 并且长度一致、每个元素的值也一致时返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool SlowEquals(this ArraySegment<byte> a, ArraySegment<byte> b)
        {
            if (a.Array == null || b.Array == null) return false;

            int diff = a.Count ^ b.Count;
            for (int i = 0; i < a.Count && i < b.Count; i++)
            {
                diff |= a.Array[a.Offset + i] ^ b.Array[b.Offset + i];
            }

            return diff == 0;
        }



        /// <summary>
        /// 将 <c>byte</c> 数组转换成十六进制字符串。
        /// </summary>
        /// <param name="value"><c>byte</c> 数组。</param>
        /// <param name="hexChar">十六进制字符集合。只能是 <see cref="HEX_DIGITS_LOWER"/> 或 <see cref="HEX_DIGITS_UPPER"/>。</param>
        /// <returns>byte 数组转换的十六进制字符串，如果 <c>byte</c> 数组为 <c>null</c> 或零长度则返回 <see cref="String.Empty"/>。</returns>
        private static string ToHex(this byte[] value, char[] hexChar)
        {
            if (value == null || value.Length == 0) return String.Empty;

#if NETSTANDARD2_0
            char[] hex = new char[value.Length * 2];
#else
            Span<char> hex = stackalloc char[value.Length * 2];
#endif

            int hexIndex = 0;
            for (int i = 0, l = value.Length; i < l; i++)
            {
                int b = value[i];
                hex[hexIndex++] = hexChar[b >> 4];
                hex[hexIndex++] = hexChar[b & 0xf];
            }
            return new String(hex);
        }
    }
}
