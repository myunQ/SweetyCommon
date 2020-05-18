/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      该文件集中存放加密相关的自定义枚举。
 *      This file stores the custom enumeration related to encryption.
 *      
 * Members Index:
 *      enum
 *          SymmetricAlgorithmType
 *          AsymmetricAlgorithmType
 *          HashAlgorithmType
 *          HMACHashAlgorithmType
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Cryptography
{
    /// <summary>
    /// 对称加密和解密算法类型
    /// </summary>
    public enum SymmetricAlgorithmType : byte
    {
        /// <summary>
        /// 表示高级加密标准 (AES) 算法来执行对称加密和解密；
        /// 密钥长度在 128 至 256 位，每次增长 64 位。加密操作的块大小为 128 位。
        /// </summary>
        AES,
        /// <summary>
        /// 表示数据加密标准 (DES) 算法来执行对称加密和解密；
        /// 密钥长度 64 位。加密操作的块大小为 64 位。
        /// </summary>
        DES,
        /// <summary>
        /// 表示 (Rijndael) 算法来执行对称加密和解密；
        /// 密钥长度在 128 至 256 位，每次增长 64 位。加密操作的块大小在 128 位 至 256 位，每次增长 64 位。
        /// </summary>
        Rijndael,
        /// <summary>
        /// 表示三重数据加密标准 (TripleDES) 算法来执行对称加密和解密；
        /// 密钥长度在 128 至 192 位，每次增长 64 位。加密操作的块大小为 64 位。
        /// </summary>
        TripleDES,
        /// <summary>
        /// 表示 (RC2) 算法来执行对称加密和解密；
        /// 密钥长度在 40 至 128 位，每次增长 8 位。加密操作的块大小为 64 位。
        /// </summary>
        RC2
    }

    /// <summary>
    /// 非对称加密和解密算法类型
    /// </summary>
    public enum AsymmetricAlgorithmType : byte
    {
        /// <summary>
        /// 表示 RSA 算法的实现执行不对称加密和解密、数字签名和验证；
        /// 密钥长度在 384 至 16384 位，每次增长 8 位。但实际有效的密钥大小取决于实例使用的加密服务提供程序 (CSP)。 
        /// 例如：windows CSPs 在 Windows 8.1之前启用 384 到 16384 位的密钥，在 Windows 8.1及之后使用的 512 到 16384 位的密钥。
        /// </summary>
        RSA,
        /// <summary>
        /// 表示椭圆曲线 Diffie-Hellman (ECDH) 算法实现。
        /// </summary>
        ECDiffieHellman,
        /// <summary>
        /// 表示椭圆曲线数字签名算法 (ECC-DSA) 的实现，该算法是使用椭圆曲线密码（ECC）对数字签名算法（DSA）的模拟。
        /// </summary>
        ECDsa,
        /// <summary>
        /// 表示数字签名算法 (DSA) 的实现。
        /// 此算法支持的密钥长度为从 512 位到 1024 位，以 64 位递增。
        /// 建议优先使用较新 RSA、ECDiffieHellman、ECDsa 算法，仅在与传统应用程序和数据的兼容性才使用 DSA 算法。
        /// </summary>
        DSA
    }

    /// <summary>
    /// 哈希函数类型
    /// </summary>
    public enum HashAlgorithmType : byte
    {
        /// <summary>
        /// 表示 MD5 哈希算法。MD5 算法的哈希值大小为 128 位。
        /// </summary>
        /// <remarks>
        /// 由于 MD5 出现冲突问题，Microsoft 建议使用基于 SHA256 或更好的安全模型。
        /// </remarks>
        MD5,
        /// <summary>
        /// 表示 SHA1 哈希算法。SHA1 算法的哈希值大小为 160 位。
        /// </summary>
        /// <remarks>
        /// 由于 SHA1 出现冲突问题，Microsoft 建议使用基于 SHA256 或更好的安全模型。
        /// </remarks>
        SHA1,
        /// <summary>
        /// 表示 SHA256 哈希算法。SHA256 算法的哈希值大小为 256 位。
        /// </summary>
        SHA256,
        /// <summary>
        /// 表示 SHA384 哈希算法。SHA384 算法的哈希值大小为 384 位。
        /// </summary>
        SHA384,
        /// <summary>
        /// 表示 SHA512 哈希算法。SHA512 算法的哈希值大小为 512 位。
        /// </summary>
        SHA512,
        ///// <summary>
        ///// 表示 MD160 哈希算法；（不建议使用）RIPEMD160 由安全哈希算法和 SHA-512 取代。
        ///// </summary>
        ///// <remarks>
        ///// RIPEMD-160 是一个 160 位加密哈希函数。 它适合用作 128 位哈希函数 MD4、MD5 和 RIPEMD 的安全替换。 RIPEMD 是在 EU 项目 RIPE（RACE Integrity Primitives Evaluation，1988-1992）的框架中开发的。
        ///// SHA256 和 SHA512 比 RIPEMD160提供更好的安全性和性能。 与传统应用程序和数据的兼容性仅使用 RIPEMD160。
        ///// </remarks>
        //RIPEMD160
    }

    /// <summary>
    /// 基于哈希值的消息验证类型
    /// </summary>
    public enum HMACHashAlgorithmType : byte
    {
        /// <summary>
        /// 表示使用 MD5 哈希函数计算基于哈希值的消息身份验证代码 (HMAC)。HMACMD5 接受任何范围键，并产生长度为 128 位的哈希序列。
        /// </summary>
        /// <remarks>
        /// 虽然密钥的长度不限，但如果超过 64 个字节，就会对其进行哈希计算（使用 SHA-1），以派生一个 64 个字节的密钥。因此，建议的密钥大小为 64 个字节。
        /// 由于 HMACMD5 出现冲突问题，Microsoft 建议使用基于 HMACSHA256 或更好的安全模型。
        /// </remarks>
        HMACMD5,
        /// <summary>
        /// 表示使用 SHA1 哈希函数计算基于哈希值的消息身份验证代码 (HMAC)。HMACSHA1 接受任何范围键，并产生长度为 160 位的哈希序列。
        /// </summary>
        /// <remarks>
        /// 虽然密钥的长度不限，但如果超过 64 个字节，就会对其进行哈希计算（使用 SHA-1），以派生一个 64 个字节的密钥。因此，建议的密钥大小为 64 个字节。
        /// 由于 HMACSHA1 出现冲突问题，Microsoft 建议使用基于 HMACSHA256 或更好的安全模型。
        /// </remarks>
        HMACSHA1,
        /// <summary>
        /// 表示使用 SHA256 哈希函数计算基于哈希值的消息身份验证代码 (HMAC)。HMACSHA256 接受任何大小的密钥，并生成长度为 256 字节的哈希序列。
        /// </summary>
        /// <remarks>
        /// 虽然密钥的长度不限，但如果超过 64 个字节，就会对其进行哈希计算（使用 SHA-1），以派生一个 64 个字节的密钥。因此，建议的密钥大小为 64 个字节。
        /// 如果少于 64 个字节，就填充到 64 个字节。
        /// </remarks>
        HMACSHA256,
        /// <summary>
        /// 表示使用 SHA384 哈希函数计算基于哈希值的消息身份验证代码 (HMAC)。HMACSHA384 接受任何大小的密钥，并生成长度为 384 字节的哈希序列。
        /// </summary>
        /// <remarks>
        /// 密钥可以是任意长度。 但是建议的大小为 128 个字节。 如果密钥长度超过 128 个字节，将对其进行哈希运算（使用 SHA-384）以派生出一个 128 字节的密钥。
        /// 如果少于 128 个字节，就填充到 128 个字节。
        /// </remarks>
        HMACSHA384,
        /// <summary>
        /// 表示使用 SHA512 哈希函数计算基于哈希值的消息身份验证代码 (HMAC)。HMACSHA512 接受任何大小的密钥，并生成长度为 512 字节的哈希序列。
        /// </summary>
        /// <remarks>
        /// 密钥可以是任意长度。 但是建议的大小为 128 个字节。 如果密钥长度超过 128 个字节，将对其进行哈希运算（使用 SHA-512）以派生出一个 128 字节的密钥。
        /// 如果少于 128 个字节，就填充到 128 个字节。
        /// </remarks>
        HMACSHA512,
        ///// <summary>
        ///// 表示使用 HMACRIPEMD160 哈希函数计算基于哈希值的消息身份验证代码 (HMAC)。HMACRIPEMD160 接受任何大小的密钥，并生成长度为 160 位的哈希序列。
        ///// </summary>
        //HMACRIPEMD160,
        ///// <summary>
        ///// 表示使用 TripleDES 哈希函数计算基于哈希值的消息身份验证代码 (MAC)。MACTripleDES 使用长为 16 个或 24 个字节的密钥，并导致长为 8 个字节的哈希序列。
        ///// </summary>
        //MACTripleDES
    }
}
