/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  定义计算哈希值的方法
 * 
 * 
 * Members Index:
 *      class
 *          InternalHash   
 *              
 * * * * * * * * * * * * * * * * * * * * */


[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Sweety.Common.Tests")]

namespace Sweety.Common.Cryptography
{
    using System;
    using System.IO;
    using System.Text;
    using System.Security.Cryptography;


    /// <summary>
    /// 计算哈希值的类。
    /// </summary>
    internal class InternalHash : IHash, IDisposable
    {
        private readonly HashAlgorithm _hash;
        private string _algorithmName = String.Empty;

        /// <summary>
        /// 构建一个哈希函数对象实例。
        /// </summary>
        /// <param name="hash">实际执行哈希算法的哈希函数类型。</param>
        internal InternalHash(HashAlgorithm hash)
        {
            _hash = hash ?? throw new ArgumentNullException(nameof(hash));
        }


        #region IHash interface implementation.
        public string AlgorithmName
        {
            get
            {
                if (_algorithmName.Length == 0)
                {
                    _algorithmName = _hash.GetType().Name;
                }
                return _algorithmName;
            }
        }

        public byte[] GetHash(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            return _hash.ComputeHash(bytes);
        }

        public byte[] GetHash(byte[] bytes, int start)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (start < 0 || start >= bytes.Length) throw new ArgumentOutOfRangeException(nameof(start));

            return _hash.ComputeHash(bytes, start, bytes.Length - start);
        }

        public byte[] GetHash(byte[] bytes, int offset, int count)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (offset < 0 || offset >= bytes.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 1 || (offset + count) > bytes.Length) throw new ArgumentOutOfRangeException(nameof(count));

            return _hash.ComputeHash(bytes, offset, count);
        }


        public byte[] GetHash(char[] chars)
        {
            if (chars == null) throw new ArgumentNullException(nameof(chars));

            return _hash.ComputeHash(Encoding.Unicode.GetBytes(chars));
        }

        public byte[] GetHash(char[] chars, int start)
        {
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (start < 0 || start >= chars.Length) throw new ArgumentOutOfRangeException(nameof(start));

            return _hash.ComputeHash(Encoding.Unicode.GetBytes(chars, start, chars.Length - start));
        }

        public byte[] GetHash(char[] chars, int offset, int count)
        {
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (offset < 0 || offset >= chars.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 1 || (offset + count) > chars.Length) throw new ArgumentOutOfRangeException(nameof(count));

            return _hash.ComputeHash(Encoding.Unicode.GetBytes(chars, offset, count));
        }


        public byte[] GetHash(Encoding encoding, char[] chars)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (chars == null) throw new ArgumentNullException(nameof(chars));

            return _hash.ComputeHash(encoding.GetBytes(chars));
        }

        public byte[] GetHash(Encoding encoding, char[] chars, int start)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (start < 0 || start >= chars.Length) throw new ArgumentOutOfRangeException(nameof(start));

            return _hash.ComputeHash(encoding.GetBytes(chars, start, chars.Length - start));
        }

        public byte[] GetHash(Encoding encoding, char[] chars, int offset, int count)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (offset < 0 || offset >= chars.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 1 || (offset + count) > chars.Length) throw new ArgumentOutOfRangeException(nameof(count));

            return _hash.ComputeHash(encoding.GetBytes(chars, offset, count));
        }


        public byte[] GetHash(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            return _hash.ComputeHash(Encoding.Unicode.GetBytes(str));
        }

        public byte[] GetHash(string str, int start)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (start < 0 || start >= str.Length) throw new ArgumentOutOfRangeException(nameof(start));

#if NETSTANDARD2_0
            return _hash.ComputeHash(Encoding.Unicode.GetBytes(str.ToCharArray(), start, str.Length - start));
#else
            return _hash.ComputeHash(Encoding.Unicode.GetBytes(str, start, str.Length - start));
#endif
        }

        public byte[] GetHash(string str, int offset, int count)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (offset < 0 || offset >= str.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 1 || (offset + count) > str.Length) throw new ArgumentOutOfRangeException(nameof(count));

#if NETSTANDARD2_0
            return _hash.ComputeHash(Encoding.Unicode.GetBytes(str.ToCharArray(), offset, count));
#else
            return _hash.ComputeHash(Encoding.Unicode.GetBytes(str, offset, count));
#endif
        }


        public byte[] GetHash(Encoding encoding, string str)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (str == null) throw new ArgumentNullException(nameof(str));

            return _hash.ComputeHash(encoding.GetBytes(str));
        }

        public byte[] GetHash(Encoding encoding, string str, int start)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (start < 0 || start >= str.Length) throw new ArgumentOutOfRangeException(nameof(start));

#if NETSTANDARD2_0
            return _hash.ComputeHash(encoding.GetBytes(str.ToCharArray(), start, str.Length - start));
#else
            return _hash.ComputeHash(encoding.GetBytes(str, start, str.Length - start));
#endif
        }

        public byte[] GetHash(Encoding encoding, string str, int offset, int count)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (offset < 0 || offset >= str.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 1 || (offset + count) > str.Length) throw new ArgumentOutOfRangeException(nameof(count));

#if NETSTANDARD2_0
            return _hash.ComputeHash(encoding.GetBytes(str.ToCharArray(), offset, count));
#else
            return _hash.ComputeHash(encoding.GetBytes(str, offset, count));
#endif
        }


        public byte[] GetHash(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (stream.CanRead == false) throw new ArgumentException(Properties.Localization.the_stream_does_not_support_read_operations, nameof(stream));

            return _hash.ComputeHash(stream);
        }

#if !(NETFRAMEWORK || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0)
        /// <summary>
        /// 尝试计算指定字节数组的哈希值。
        /// </summary>
        /// <param name="source">要计算其哈希代码的输入。</param>
        /// <param name="destination">要接收哈希值的缓冲区。</param>
        /// <param name="bytesWritten">此方法返回时，为写入 <paramref name="destination"/> 的字节总数。</param>
        /// <returns>当 <paramref name="destination"/> 的长度不足以接收哈希值时返回 <c>false</c>，否则返回 <c>true</c>。</returns>
        public bool TryGetHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
        {
            return _hash.TryComputeHash(source, destination, out bytesWritten);
        }
#endif

        public void Dispose()
        {
            _hash.Dispose();
        }
        #endregion IHash interface implementation.
    }
}
