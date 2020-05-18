using System;
using System.Security.Cryptography;

using Xunit;

using Sweety.Common.Cryptography;


namespace Sweety.Common.Tests.Cryptography
{
    public class SymmetricAlgorithmFactoryTest
    {
        [Fact]
        public void Create()
        {
            using (ISymmetricCryptography cryptography = SymmetricAlgorithmFactory.Create())
                Assert.NotNull(cryptography);
        }

        [Theory]
        [InlineData(SymmetricAlgorithmType.AES)]
        [InlineData(SymmetricAlgorithmType.DES)]
        [InlineData(SymmetricAlgorithmType.RC2)]
        [InlineData(SymmetricAlgorithmType.Rijndael)]
        [InlineData(SymmetricAlgorithmType.TripleDES)]
        public void Create_BY_algorithmType(SymmetricAlgorithmType algorithmType)
        {
            using (ISymmetricCryptography cryptography = SymmetricAlgorithmFactory.Create(algorithmType))
                Assert.NotNull(cryptography);
        }

        
        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 })]
        public void Create_BY_algorithmType_key_iv(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv)
        {
            using (ISymmetricCryptography cryptography = SymmetricAlgorithmFactory.Create(algorithmType, key, iv))
                Assert.NotNull(cryptography);
        }

        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 150, 16, 72, 230, 255, 223, 164, 247, 150, 136, 75, 45, 54, 133, 230, 187, 221, 234, 222, 233, 204, 103, 123, 115, 105, 237, 171, 182, 119, 183, 198, 242 }
            , new byte[] { 51, 129, 130, 36, 241, 29, 213, 239, 157, 185, 79, 157, 111, 101, 182, 45 }
            , CipherMode.CBC
            , PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 79, 235, 27, 122, 232, 201, 245, 231 }
            , new byte[] { 182, 177, 251, 7, 75, 228, 111, 9 }
            , CipherMode.CBC
            , PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 238, 222, 163, 226, 122, 69, 248, 117, 58, 76, 192, 14, 64, 159, 37, 58 }
            , new byte[] { 29, 45, 12, 194, 227, 189, 223, 247 }
            , CipherMode.CBC
            , PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 182, 195, 68, 141, 62, 74, 3, 187, 189, 150, 197, 1, 235, 213, 195, 17, 98, 49, 74, 229, 153, 46, 208, 93, 184, 62, 0, 23, 101, 248, 167, 238 }
            , new byte[] { 167, 129, 47, 86, 245, 41, 252, 17, 107, 252, 172, 255, 20, 220, 230, 245 }
            , CipherMode.CBC
            , PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 56, 189, 35, 224, 206, 31, 18, 93, 196, 231, 141, 26, 217, 46, 151, 131, 237, 194, 7, 0, 161, 211, 124, 240 }
            , new byte[] { 223, 213, 201, 58, 96, 206, 46, 37}
            , CipherMode.CBC
            , PaddingMode.ISO10126)]
        public void Create_BY_algorithmType_key_iv_mode_padding(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding)
        {
            using (ISymmetricCryptography cryptography = SymmetricAlgorithmFactory.Create(algorithmType, key, iv, mode, padding))
                Assert.NotNull(cryptography);
        }
    }
}
