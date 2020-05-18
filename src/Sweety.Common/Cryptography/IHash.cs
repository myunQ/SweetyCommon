/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      定义计算哈希的方法。
 * 
 * 
 * Members Index:
 *      interface
 *          IHash   
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Cryptography
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 哈希计算器接口。
    /// </summary>
    public interface IHash : IDisposable
    {
        /// <summary>
        /// 获取当前实例的哈希算法名称。
        /// </summary>
        string AlgorithmName { get; }

        /// <summary>
        /// 获取指定字节数组的哈希值。
        /// </summary>
        /// <param name="bytes">要计算哈希值的数据。</param>
        /// <returns><paramref name="bytes"/> 的哈希值。</returns>
        byte[] GetHash(Byte[] bytes);
        /// <summary>
        /// 获取指定字节数组的哈希值。
        /// </summary>
        /// <param name="bytes">要计算哈希值的数据。</param>
        /// <param name="start">从此索引处开始计算哈希值。</param>
        /// <returns><paramref name="bytes"/> 的哈希值。</returns>
        byte[] GetHash(Byte[] bytes, int start);
        /// <summary>
        /// 获取指定字节数组的指定区域的哈希值。
        /// </summary>
        /// <param name="bytes">要计算哈希值的数据。</param>
        /// <param name="offset"><paramref name="bytes"/> 中的偏移量，从该位置开始对计算哈希值。</param>
        /// <param name="count"><paramref name="bytes"/> 中计算哈希值的字节数。</param>
        /// <returns><paramref name="bytes"/> 的哈希值。</returns>
        byte[] GetHash(byte[] bytes, int offset, int count);

        /// <summary>
        /// 获取指定字符数组的哈希值。
        /// </summary>
        /// <param name="chars">要计算哈希值的字符数组。将使用 <c>UTF-16</c> 对这些字符进行编码。</param>
        /// <returns><paramref name="chars"/> 的哈希值。</returns>
        byte[] GetHash(char[] chars);
        /// <summary>
        /// 获取指定字符数组的哈希值。
        /// </summary>
        /// <param name="chars">要计算哈希值的数据。将使用 <c>UTF-16</c> 对这些字符进行编码。</param>
        /// <param name="start">从此索引处开始计算哈希值。</param>
        /// <returns><paramref name="chars"/> 的哈希值。</returns>
        byte[] GetHash(char[] chars, int start);
        /// <summary>
        /// 获取指定字符数组的哈希值。
        /// </summary>
        /// <param name="chars">要计算哈希值的数据。将使用 <c>UTF-16</c> 对这些字符进行编码。</param>
        /// <param name="offset"><paramref name="chars"/> 中的偏移量，从该位置开始对计算哈希值。</param>
        /// <param name="count"><paramref name="chars"/> 中计算哈希值的字符数。</param>
        /// <returns><paramref name="chars"/> 的哈希值。</returns>
        byte[] GetHash(char[] chars, int offset, int count);

        /// <summary>
        /// 获取指定字符数组的哈希值。
        /// </summary>
        /// <param name="encoding"><paramref name="chars"/> 的编码。</param>
        /// <param name="chars">要计算哈希值的数据。</param>
        /// <returns><paramref name="chars"/> 的哈希值。</returns>
        byte[] GetHash(Encoding encoding, char[] chars);
        /// <summary>
        /// 获取指定字符数组的哈希值。
        /// </summary>
        /// <param name="encoding"><paramref name="chars"/> 的编码。</param>
        /// <param name="chars">要计算哈希值的数据。</param>
        /// <param name="start">从此索引处开始计算哈希值。</param>
        /// <returns><paramref name="chars"/> 的哈希值。</returns>
        byte[] GetHash(Encoding encoding, char[] chars, int start);
        /// <summary>
        /// 获取指定字符数组的哈希值。
        /// </summary>
        /// <param name="encoding"><paramref name="chars"/> 的编码。</param>
        /// <param name="chars">要计算哈希值的数据。</param>
        /// <param name="offset"><paramref name="chars"/> 中的偏移量，从该位置开始对计算哈希值。</param>
        /// <param name="count"><paramref name="chars"/> 中计算哈希值的字符数。</param>
        /// <returns><paramref name="chars"/> 的哈希值。</returns>
        byte[] GetHash(Encoding encoding, char[] chars, int offset, int count);

        /// <summary>
        /// 获取指定字符串的哈希值。
        /// </summary>
        /// <param name="str">要计算哈希值的字符串。将使用 <c>UTF-16</c> 对字符串进行编码。</param>
        /// <returns><paramref name="str"/> 的哈希值。</returns>
        byte[] GetHash(string str);
        /// <summary>
        /// 获取指定字符串的哈希值。
        /// </summary>
        /// <param name="str">要计算哈希值的字符串。将使用 <c>UTF-16</c> 对字符串进行编码。</param>
        /// <param name="start">从此索引开始计算哈希值。</param>
        /// <returns><paramref name="str"/> 的哈希值。</returns>
        byte[] GetHash(string str, int start);
        /// <summary>
        /// 获取指定字符串的哈希值。
        /// </summary>
        /// <param name="str">要计算哈希值的字符串。将使用 <c>UTF-16</c> 对字符串进行编码。</param>
        /// <param name="offset"><paramref name="str"/> 中的偏移量，从该位置开始对计算哈希值。</param>
        /// <param name="count"><paramref name="str"/> 中计算哈希值的字符数。</param>
        /// <returns><paramref name="str"/> 的哈希值。</returns>
        byte[] GetHash(string str, int offset, int count);

        /// <summary>
        /// 获取指定字符串的哈希值。
        /// </summary>
        /// <param name="encoding"><paramref name="str"/> 的编码。</param>
        /// <param name="str">要计算哈希值的字符串。</param>
        /// <returns><paramref name="str"/> 的哈希值。</returns>
        byte[] GetHash(Encoding encoding, string str);
        /// <summary>
        /// 获取指定字符串的哈希值。
        /// </summary>
        /// <param name="encoding"><paramref name="str"/> 的编码。</param>
        /// <param name="str">要计算哈希值的字符串。</param>
        /// <param name="start">从此索引开始计算哈希值。</param>
        /// <returns><paramref name="str"/> 的哈希值。</returns>
        byte[] GetHash(Encoding encoding, string str, int start);
        /// <summary>
        /// 获取指定字符串的哈希值。
        /// </summary>
        /// <param name="encoding"><paramref name="str"/> 的编码。</param>
        /// <param name="str">要计算哈希值的字符串。</param>
        /// <param name="offset"><paramref name="str"/> 中的偏移量，从该位置开始对计算哈希值。</param>
        /// <param name="count"><paramref name="str"/> 中计算哈希值的字符数。</param>
        /// <returns><paramref name="str"/> 的哈希值。</returns>
        byte[] GetHash(Encoding encoding, string str, int offset, int count);

        /// <summary>
        /// 获取指定流的哈希值。
        /// </summary>
        /// <param name="stream">要计算哈希值的流。请确保 <see cref="Stream.Position"/> 属性设置了正确的值。</param>
        /// <returns><paramref name="stream"/> 的哈希值。</returns>
        byte[] GetHash(Stream stream);

#if !(NETFRAMEWORK || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0)
        /// <summary>
        /// 尝试计算指定字节数组的哈希值。
        /// </summary>
        /// <param name="source">要计算其哈希代码的输入。</param>
        /// <param name="destination">要接收哈希值的缓冲区。</param>
        /// <param name="bytesWritten">此方法返回时，为写入 <paramref name="destination"/> 的字节总数。</param>
        /// <returns>当 <paramref name="destination"/> 的长度不足以接收哈希值时返回 <c>false</c>，否则返回 <c>true</c>。</returns>
        bool TryGetHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten);
#endif
    }
}
