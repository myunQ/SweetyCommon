/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  定义对称加密/解密的方法
 * 
 * 
 * Members Index:
 *      interface
 *          ISymmetricCryptography   
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Cryptography
{
    using System;
    using System.Security.Cryptography;


    /// <summary>
    /// 定义对称加密/解密的方法。
    /// </summary>
    public interface ISymmetricCryptography : ICryptography
    {
        /// <summary>
        /// 获取或设置对称算法的密钥。
        /// </summary>
        /// <exception cref="ArgumentNullException">试图将密钥设置为 <c>null</c>。</exception>
        /// <exception cref="CryptographicException">密钥大小无效。</exception>
        byte[] Key { get; set; }
        /// <summary>
        /// 获取或设置对称算法的初始化向量
        /// </summary>
        /// <exception cref="ArgumentNullException">试图将初始化向量设置为 <c>null</c>。</exception>
        /// <exception cref="CryptographicException">试图将初始化向量设置为无效大小。</exception>
        byte[] IV { get; set; }

        /// <summary>
        /// 获取或设置对称算法的运算模式。默认值为 <see cref="CipherMode.CBC"/>。
        /// </summary>
        /// <exception cref="CryptographicException">
        /// 该密码模式不是 <see cref="CipherMode"/> 值之一。
        /// </exception>
        CipherMode Mode { get; set; }
        /// <summary>
        /// 获取或设置对称算法中使用的填充模式。默认值为 <see cref="PaddingMode.PKCS7"/>。
        /// </summary>
        /// <exception cref="CryptographicException">该填充模式不是 <see cref="PaddingMode"/> 值之一。</exception>
        PaddingMode Padding { get; set; }
    }
}
