/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  对称加密工厂
 * 
 * 
 * Members Index:
 *      sealed class SymmetricAlgorithmFactory
 *              
 * 
AesCryptoServiceProvider
BlockSize:128bit
FeedbackSize:8bit
KeySize:256bit
LegalBlockSizes:Min128, Skip:0,Max:128
LegalKeySizes:Min128, Skip:64,Max:256
Mode:CBC，Padding:PKCS7
Key:236,67,109,24,205,104,117,52,84,242,143,97,139,223,98,74,138,182,48,55,72,194,74,195,4,201,18,193,154,237,4,213,IV:53,216,210,121,103,166,148,194,98,14,174,42,129,117,156,180

AesManaged
BlockSize:128bit
FeedbackSize:128bit
KeySize:256bit
LegalBlockSizes:Min128, Skip:0,Max:128
LegalKeySizes:Min128, Skip:64,Max:256
Mode:CBC，Padding:PKCS7
Key:201,221,172,97,127,153,83,103,139,58,111,146,246,59,253,1,19,54,157,190,191,106,115,76,98,243,200,172,215,144,169,49,IV:225,27,147,8,82,249,183,192,197,94,217,213,100,52,116,133

DESCryptoServiceProvider
BlockSize:64bit
FeedbackSize:8bit
KeySize:64bit
LegalBlockSizes:Min64, Skip:0,Max:64
LegalKeySizes:Min64, Skip:0,Max:64
Mode:CBC，Padding:PKCS7
Key:43,241,21,157,10,227,169,231,IV:192,243,122,233,149,58,64,18

RC2CryptoServiceProvider
BlockSize:64bit
FeedbackSize:8bit
KeySize:128bit
LegalBlockSizes:Min64, Skip:0,Max:64
LegalKeySizes:Min40, Skip:8,Max:128
Mode:CBC，Padding:PKCS7
Key:42,112,109,24,88,232,115,18,219,89,180,21,107,205,93,155,IV:245,55,236,73,3,247,172,234

RijndaelManaged
BlockSize:128bit
FeedbackSize:128bit
KeySize:256bit
LegalBlockSizes:Min128, Skip:64,Max:256
LegalKeySizes:Min128, Skip:64,Max:256
Mode:CBC，Padding:PKCS7
Key:187,124,119,52,118,32,186,205,61,79,87,219,125,54,206,208,73,209,234,136,43,211,245,94,224,139,150,96,62,56,66,175,IV:56,61,106,145,29,127,175,114,74,104,60,194,1,67,145,8

TripleDESCryptoServiceProvider
BlockSize:64bit
FeedbackSize:8bit
KeySize:192bit
LegalBlockSizes:Min64, Skip:0,Max:64
LegalKeySizes:Min128, Skip:64,Max:192
Mode:CBC，Padding:PKCS7
Key:123,184,231,72,67,248,24,143,85,136,247,50,6,179,157,40,69,209,1,202,149,154,234,143,IV:228,142,89,42,152,20,37,143
 * 
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Cryptography
{
    using System;
    using System.Security.Cryptography;



    /// <summary>
    /// 对称算法工厂
    /// </summary>
    public static class SymmetricAlgorithmFactory
    {
        /// <summary>
        /// 创建一个高级加密标准 (AES) 算法来执行对称加密和解密。
        /// </summary>
        /// <returns>返回一个默认的对称加密和解密的对象实例。</returns>
        public static ISymmetricCryptography Create()
        {
            return Create(SymmetricAlgorithmType.AES);
        }
        /// <summary>
        /// 创建一个对称加密和解密对象实例。
        /// </summary>
        /// <param name="algorithmType">要创建的对称加密算法类型。</param>
        /// <returns>返回一个对称加密和解密的对象实例。</returns>
        public static ISymmetricCryptography Create(SymmetricAlgorithmType algorithmType)
        {
            return new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()));
        }
        /// <summary>
        /// 创建一个对称加密和解密对象实例。
        /// </summary>
        /// <param name="algorithmType">要创建的对称加密算法类型。</param>
        /// <param name="key">密钥；必须符合 <paramref name="algorithmType"/> 参数指定的加密算法类型。</param>
        /// <param name="iv">初始化向量；必须符合 <paramref name="algorithmType"/> 参数指定的加密算法类型。</param>
        /// <returns>返回一个对称加密和解密的对象实例。</returns>
        public static ISymmetricCryptography Create(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv)
        {
            return new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv);
        }
        /// <summary>
        /// 创建一个对称加密和解密对象实例。
        /// </summary>
        /// <param name="algorithmType">要创建的对称加密算法类型。</param>
        /// <param name="key">密钥；必须符合 <paramref name="algorithmType"/> 参数指定的加密算法类型。</param>
        /// <param name="iv">初始化向量；必须符合 <paramref name="algorithmType"/> 参数指定的加密算法类型。</param>
        /// <param name="mode">对称算法的运算模式；必须符合 <paramref name="algorithmType"/> 参数指定的加密算法类型。</param>
        /// <param name="padding">对称算法中使用的填充模式；必须符合 <paramref name="algorithmType"/> 参数指定的加密算法类型。</param>
        /// <returns>返回一个对称加密和解密的对象实例。</returns>
        public static ISymmetricCryptography Create(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding)
        {
            return new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv, mode, padding);
        }
    }
}
