/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  哈希工厂
 * 
 * 
 * Members Index:
 *      class
 *          HashAlgorithmFactory
 *          
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Cryptography
{
    using System.Security.Cryptography;

    /// <summary>
    /// 哈希工厂。
    /// </summary>
    public static class HashAlgorithmFactory
    {
        /// <summary>
        /// 创建一个哈希对象实例。
        /// </summary>
        /// <param name="algorithmType">要创建的哈希算法类型。</param>
        /// <returns>返回一个哈希算法对象实例。</returns>
        public static IHash Create(HashAlgorithmType algorithmType)
        {
            return new InternalHash(HashAlgorithm.Create(algorithmType.ToString()));
        }

        /// <summary>
        /// 创建一个基于哈希的消息验证代码 (HMAC) 的对象实例。
        /// </summary>
        /// <param name="algorithmType">要创建的哈希算法类型。</param>
        /// <param name="key">加密的机密密钥。</param>
        /// <returns>返回一个哈希算法对象实例。</returns>
        public static IHash Create(HMACHashAlgorithmType algorithmType, byte[] key)
        {
            var obj = KeyedHashAlgorithm.Create(algorithmType.ToString());
            obj.Key = key;
            return new InternalHash(obj);
        }


#if !(NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
        /// <summary>
        /// 创建一个哈希对象实例。
        /// </summary>
        /// <param name="algorithmName">要创建的哈希算法名称。</param>
        /// <returns>返回一个哈希算法对象实例。</returns>
        public static IHash Create(HashAlgorithmName algorithmName)
        {
            return new InternalHash(HashAlgorithm.Create(algorithmName.Name));
        }

        /// <summary>
        /// 创建一个基于哈希的消息验证代码 (HMAC) 的对象实例。
        /// </summary>
        /// <param name="algorithmName">要创建的哈希算法名称。</param>
        /// <param name="key">加密的机密密钥。</param>
        /// <returns>返回一个哈希算法对象实例。</returns>
        public static IHash Create(HashAlgorithmName algorithmName, byte[] key)
        {
            var obj = KeyedHashAlgorithm.Create("HMAC" + algorithmName.Name);
            obj.Key = key;
            return new InternalHash(obj);
        }
#endif
    }
}
