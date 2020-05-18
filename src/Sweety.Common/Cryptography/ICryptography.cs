/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      定义加密/解密的方法。
 * 
 * 
 * Members Index:
 *      interface
 *          ICryptography   
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Cryptography
{
    using System;
    using System.Text;


    /// <summary>
    /// 定义加密/解密的方法。
    /// </summary>
    public interface ICryptography : IDisposable
    {
        /// <summary>
        /// 获取当前实例的对称加密算法名称。
        /// </summary>
        string AlgorithmName { get; }

        /// <summary>
        /// 对数据进行加密。
        /// </summary>
        /// <param name="plaintext">要加密的数据。</param>
        /// <returns>加密后的数据。</returns>
        byte[] Encrypt(byte[] plaintext);
#if !NETSTANDARD2_0
        /// <summary>
        /// 对数据进行加密。
        /// </summary>
        /// <param name="plaintext">要加密的数据。</param>
        /// <returns>加密后的数据。</returns>
        byte[] Encrypt(ReadOnlySpan<byte> plaintext);
#endif
        /// <summary>
        /// 对数据进行加密。
        /// </summary>
        /// <param name="encoding"><paramref name="plaintext"/> 的编码。</param>
        /// <param name="plaintext">要加密的数据。</param>
        /// <returns>加密后的数据。</returns>
        byte[] Encrypt(Encoding encoding, char[] plaintext);

#if !NETSTANDARD2_0
        /// <summary>
        /// 对数据进行加密。
        /// </summary>
        /// <param name="encoding"><paramref name="plaintext"/> 的编码。</param>
        /// <param name="plaintext">要加密的数据。</param>
        /// <returns>加密后的数据。</returns>
        byte[] Encrypt(Encoding encoding, ReadOnlySpan<char> plaintext);
#endif
        /// <summary>
        /// 对数据进行加密。
        /// </summary>
        /// <param name="encoding"><paramref name="plaintext"/> 的编码。</param>
        /// <param name="plaintext">要加密的数据。</param>
        /// <returns>加密后的数据。</returns>
        byte[] Encrypt(Encoding encoding, string plaintext);

        /// <summary>
        /// 对数据进行解密。
        /// </summary>
        /// <param name="ciphertext">要解密的密文。</param>
        /// <returns>解密后的数据。</returns>
        byte[] Decrypt(byte[] ciphertext);
        /// <summary>
        /// 对数据进行解密。
        /// </summary>
        /// <param name="base64Ciphertext">要解密的密文。该密文是经过 <c>base64</c> 编码过的。</param>
        /// <returns>解密后的数据。</returns>
        byte[] Decrypt(char[] base64Ciphertext);
        /// <summary>
        /// 对数据进行解密。
        /// </summary>
        /// <param name="base64Ciphertext">要解密的密文。该密文是经过 <c>base64</c> 编码的。</param>
        /// <returns>解密后的数据。</returns>
        byte[] Decrypt(string base64Ciphertext);
    }
}
