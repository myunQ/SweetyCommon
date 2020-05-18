using System;
using System.Security.Cryptography;

using Xunit;

using Sweety.Common.Cryptography;

namespace Sweety.Common.Tests
{
    public class HashAlgorithmFactoryTest
    {
        [Theory]
        [InlineData(HashAlgorithmType.MD5)]
        [InlineData(HashAlgorithmType.SHA1)]
        [InlineData(HashAlgorithmType.SHA256)]
        [InlineData(HashAlgorithmType.SHA384)]
        [InlineData(HashAlgorithmType.SHA512)]
        public void Create_HASH_BY_algorithmType(HashAlgorithmType hashType)
        {
            using (IHash hash = HashAlgorithmFactory.Create(hashType))
                Assert.NotNull(hash);
        }

        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5)]
        [InlineData(HMACHashAlgorithmType.HMACSHA1)]
        [InlineData(HMACHashAlgorithmType.HMACSHA256)]
        [InlineData(HMACHashAlgorithmType.HMACSHA384)]
        [InlineData(HMACHashAlgorithmType.HMACSHA512)]
        public void Create_HMACHASH_BY_algorithmType_key(HMACHashAlgorithmType hashType)
        {
            byte[] key = null;
            switch (hashType)
            {
                case HMACHashAlgorithmType.HMACMD5:
                case HMACHashAlgorithmType.HMACSHA1:
                case HMACHashAlgorithmType.HMACSHA256:
                    key = new byte[64]; break;
                case HMACHashAlgorithmType.HMACSHA384:
                case HMACHashAlgorithmType.HMACSHA512:
                    key = new byte[128]; break;
            }

            using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
                rngCrypto.GetBytes(key);

            using (IHash hash = HashAlgorithmFactory.Create(hashType, key))
                Assert.NotNull(hash);
        }


//#if !(NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
        [Theory]
        [InlineData("MD5")]
        [InlineData("SHA1")]
        [InlineData("SHA256")]
        [InlineData("SHA384")]
        [InlineData("SHA512")]
        public void Create_HASH_BY_algorithmName(string hashName)
        {
            /*
            var haName = (hashName switch
            {
                "MD5" => HashAlgorithmName.MD5,
                "SHA1" => HashAlgorithmName.SHA1,
                "SHA256" => HashAlgorithmName.SHA256,
                "SHA384" => HashAlgorithmName.SHA384,
                "SHA512" => HashAlgorithmName.SHA512,
                _ => throw new ArgumentException("unknown value.", nameof(hashName)),
            });
            */

            HashAlgorithmName haName;

            switch (hashName)
            {
                case "MD5":
                    haName = HashAlgorithmName.MD5; break;
                case "SHA1":
                    haName = HashAlgorithmName.SHA1; break;
                case "SHA256":
                    haName = HashAlgorithmName.SHA256; break;
                case "SHA384":
                    haName = HashAlgorithmName.SHA384; break;
                case "SHA512":
                    haName = HashAlgorithmName.SHA512; break;
                default:
                    throw new ArgumentException("unknown value.", nameof(hashName));
            }

            using (IHash hash = HashAlgorithmFactory.Create(haName))
                Assert.NotNull(hash);
        }

        [Theory]
        [InlineData("MD5")]
        [InlineData("SHA1")]
        [InlineData("SHA256")]
        [InlineData("SHA384")]
        [InlineData("SHA512")]
        public void Create_HMACHASH_BY_algorithmName_key(string hashName)
        {
            byte[] key = null;
            HashAlgorithmName haName;
            switch (hashName)
            {
                case "MD5":
                    haName = HashAlgorithmName.MD5;
                    key = new byte[64];
                    break;
                case "SHA1":
                    haName = HashAlgorithmName.SHA1;
                    key = new byte[64];
                    break;
                case "SHA256":
                    haName = HashAlgorithmName.SHA256;
                    key = new byte[64];
                    break;
                case "SHA384":
                    haName = HashAlgorithmName.SHA384;
                    key = new byte[128];
                    break;
                case "SHA512":
                    haName = HashAlgorithmName.SHA512;
                    key = new byte[128];
                    break;
                default:
                    throw new ArgumentException("unknown value.", nameof(hashName));
            }

            using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
                rngCrypto.GetBytes(key);

            using (IHash hash = HashAlgorithmFactory.Create(haName, key))
                Assert.NotNull(hash);
        }
    }
}