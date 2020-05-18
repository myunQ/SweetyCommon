using System;
using System.Security.Cryptography;
using System.Text;

using Xunit;

using Sweety.Common.Cryptography;


namespace Sweety.Common.Tests.Cryptography
{
    public class InternalSymmetricCryptographyTest
    {
        [Fact]
        public void Ctor_BY_cryptograph()
        {
            using (InternalSymmetricCryptography cryptography = new InternalSymmetricCryptography(new AesCryptoServiceProvider()))
            {
                Assert.NotNull(cryptography);
                Assert.Equal(nameof(AesCryptoServiceProvider), cryptography.AlgorithmName);
            }
        }


        [Fact]
        public void Ctor_BY_cryptograph_key_iv()
        {
            byte[] key = new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 };
            byte[] iv = new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 };
            using (InternalSymmetricCryptography cryptography = new InternalSymmetricCryptography(new AesCryptoServiceProvider(), key, iv))
            {
                Assert.NotNull(cryptography);
                Assert.Equal(nameof(AesCryptoServiceProvider), cryptography.AlgorithmName);
                Assert.Equal(key, cryptography.Key);
                Assert.Equal(iv, cryptography.IV);
            }
        }


        [Fact]
        public void Ctor_BY_cryptograph_key_iv_mode_padding()
        {
            byte[] key = new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 };
            byte[] iv = new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 };
            CipherMode mode = CipherMode.CBC;
            PaddingMode padding = PaddingMode.ISO10126;
            using (InternalSymmetricCryptography cryptography = new InternalSymmetricCryptography(new AesCryptoServiceProvider(), key, iv, mode, padding))
            {
                Assert.NotNull(cryptography);
                Assert.Equal(nameof(AesCryptoServiceProvider), cryptography.AlgorithmName);
                Assert.Equal(key, cryptography.Key);
                Assert.Equal(iv, cryptography.IV);
                Assert.Equal(mode, cryptography.Mode);
                Assert.Equal(padding, cryptography.Padding);
            }
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 })]
        public void Key_GET_SET(SymmetricAlgorithmType algorithmType, byte[] key)
        {
            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()))
            {
                Key = key
            })
                Assert.Equal(key, cryptography.Key);
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 })]
        public void IV_GET_SET(SymmetricAlgorithmType algorithmType, byte[] iv)
        {
            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()))
            {
                IV = iv
            })

                Assert.Equal(iv, cryptography.IV);
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES, CipherMode.CBC)]
        [InlineData(SymmetricAlgorithmType.DES, CipherMode.CBC)]
        [InlineData(SymmetricAlgorithmType.RC2, CipherMode.CBC)]
        [InlineData(SymmetricAlgorithmType.Rijndael, CipherMode.CBC)]
        [InlineData(SymmetricAlgorithmType.TripleDES, CipherMode.CBC)]
        public void Mode_GET_SET(SymmetricAlgorithmType algorithmType, CipherMode mode)
        {
            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()))
            {
                Mode = mode
            })
                Assert.Equal(mode, cryptography.Mode);
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES, PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.DES, PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.RC2, PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.Rijndael, PaddingMode.ISO10126)]
        [InlineData(SymmetricAlgorithmType.TripleDES, PaddingMode.ISO10126)]
        public void Padding_GET_SET(SymmetricAlgorithmType algorithmType, PaddingMode padding)
        {
            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()))
            {
                Padding = padding
            })
                Assert.Equal(padding, cryptography.Padding);
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES, nameof(AesCryptoServiceProvider))]
        [InlineData(SymmetricAlgorithmType.DES, nameof(DESCryptoServiceProvider))]
        [InlineData(SymmetricAlgorithmType.RC2, nameof(RC2CryptoServiceProvider))]
        [InlineData(SymmetricAlgorithmType.Rijndael, nameof(RijndaelManaged))]
        [InlineData(SymmetricAlgorithmType.TripleDES, nameof(TripleDESCryptoServiceProvider))]
        public void AlgorithmName_GET(SymmetricAlgorithmType algorithmType, string expected)
        {
            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString())))
            {
                var actual = cryptography.AlgorithmName;

                Assert.Equal(expected, actual);
            }
        }

#if !(NETFRAMEWORK || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)
        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
            , new byte[] { 54, 184, 153, 191, 157, 29, 180, 60, 208, 185, 9, 29, 239, 39, 153, 194, 255, 95, 130, 46, 57, 236, 72, 46, 53, 87, 184, 251, 64, 60, 144, 0, 110, 41, 42, 146, 136, 184, 234, 254, 227, 68, 3, 158, 186, 161, 113, 158 })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
            , new byte[] { 33, 83, 93, 89, 98, 204, 139, 91, 105, 124, 52, 215, 117, 204, 60, 60, 32, 224, 200, 111, 224, 96, 178, 173, 193, 249, 117, 147, 221, 162, 186, 226, 163, 9, 252, 215, 67, 15, 113, 179 })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
            , new byte[] { 124, 20, 163, 88, 120, 177, 69, 69, 142, 16, 27, 212, 9, 139, 118, 68, 244, 222, 3, 59, 24, 230, 15, 177, 139, 245, 114, 158, 248, 222, 105, 227, 101, 246, 29, 240, 31, 149, 71, 162 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
            , new byte[] { 236, 158, 222, 76, 107, 82, 198, 76, 35, 73, 57, 224, 188, 193, 38, 115, 73, 22, 13, 187, 138, 148, 126, 202, 205, 248, 21, 75, 154, 152, 96, 219, 152, 218, 94, 34, 192, 225, 83, 60, 202, 10, 192, 163, 66, 115, 251, 149 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
            , new byte[] { 182, 120, 94, 180, 223, 137, 250, 2, 194, 217, 2, 86, 199, 208, 24, 152, 42, 61, 210, 82, 125, 192, 16, 124, 182, 92, 144, 116, 216, 23, 232, 121, 158, 185, 108, 179, 100, 225, 173, 141 })]
        public void Encrypt_BY_plaintext_byteSpan(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, byte[] expected)
        {
            ReadOnlySpan<byte> plaintext = stackalloc byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 };

            using ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv);

            byte[] actual = cryptography.Encrypt(plaintext);

            Assert.Equal(expected, actual);
        }
#endif


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
            , new byte[] { 54, 184, 153, 191, 157, 29, 180, 60, 208, 185, 9, 29, 239, 39, 153, 194, 255, 95, 130, 46, 57, 236, 72, 46, 53, 87, 184, 251, 64, 60, 144, 0, 110, 41, 42, 146, 136, 184, 234, 254, 227, 68, 3, 158, 186, 161, 113, 158 })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
            , new byte[] { 33, 83, 93, 89, 98, 204, 139, 91, 105, 124, 52, 215, 117, 204, 60, 60, 32, 224, 200, 111, 224, 96, 178, 173, 193, 249, 117, 147, 221, 162, 186, 226, 163, 9, 252, 215, 67, 15, 113, 179 })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
            , new byte[] { 124, 20, 163, 88, 120, 177, 69, 69, 142, 16, 27, 212, 9, 139, 118, 68, 244, 222, 3, 59, 24, 230, 15, 177, 139, 245, 114, 158, 248, 222, 105, 227, 101, 246, 29, 240, 31, 149, 71, 162 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
            , new byte[] { 236, 158, 222, 76, 107, 82, 198, 76, 35, 73, 57, 224, 188, 193, 38, 115, 73, 22, 13, 187, 138, 148, 126, 202, 205, 248, 21, 75, 154, 152, 96, 219, 152, 218, 94, 34, 192, 225, 83, 60, 202, 10, 192, 163, 66, 115, 251, 149 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
            , new byte[] { 182, 120, 94, 180, 223, 137, 250, 2, 194, 217, 2, 86, 199, 208, 24, 152, 42, 61, 210, 82, 125, 192, 16, 124, 182, 92, 144, 116, 216, 23, 232, 121, 158, 185, 108, 179, 100, 225, 173, 141 })]
        public void Encrypt_BY_plaintext_byteArray(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, byte[] expected)
        {
            byte[] plaintext = new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 };

            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv))
            {
                byte[] actual = cryptography.Encrypt(plaintext);

                Assert.Equal(expected, actual);
            }
        }

#if !(NETFRAMEWORK || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)
        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
           , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
           , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
           , new byte[] { 157, 248, 129, 153, 61, 88, 18, 84, 194, 190, 21, 158, 245, 150, 170, 245, 180, 164, 171, 205, 55, 127, 238, 92, 176, 161, 148, 162, 115, 88, 91, 5 })]
        [InlineData(SymmetricAlgorithmType.DES
           , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
           , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
           , new byte[] { 106, 205, 246, 30, 22, 37, 235, 169, 77, 29, 246, 184, 195, 42, 183, 246, 32, 50, 220, 253, 12, 142, 51, 196, 83, 115, 207, 137, 15, 189, 210, 202 })]
        [InlineData(SymmetricAlgorithmType.RC2
           , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
           , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
           , new byte[] { 252, 65, 151, 21, 179, 185, 42, 121, 54, 118, 248, 137, 143, 127, 169, 174, 66, 149, 112, 85, 49, 216, 219, 10, 250, 252, 90, 83, 79, 178, 20, 228 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
           , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
           , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
           , new byte[] { 184, 150, 40, 191, 247, 112, 70, 190, 229, 174, 13, 216, 29, 96, 57, 204, 22, 173, 168, 10, 28, 149, 208, 209, 47, 98, 12, 129, 201, 95, 95, 186 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
           , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
           , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
           , new byte[] { 14, 159, 96, 171, 54, 142, 120, 224, 65, 79, 82, 5, 64, 171, 122, 87, 112, 38, 124, 114, 141, 47, 15, 39, 80, 114, 131, 191, 92, 239, 116, 246 })]
        public void Encrypt_BY_encoding_plaintext_charSpan(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, byte[] expected)
        {
            ReadOnlySpan<char> plaintext = stackalloc char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' };

            using ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv);

            byte[] actual = cryptography.Encrypt(Encoding.Unicode, plaintext);

            Assert.Equal(expected, actual);
        }
#endif


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
           , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
           , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
           , new byte[] { 157, 248, 129, 153, 61, 88, 18, 84, 194, 190, 21, 158, 245, 150, 170, 245, 180, 164, 171, 205, 55, 127, 238, 92, 176, 161, 148, 162, 115, 88, 91, 5 })]
        [InlineData(SymmetricAlgorithmType.DES
           , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
           , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
           , new byte[] { 106, 205, 246, 30, 22, 37, 235, 169, 77, 29, 246, 184, 195, 42, 183, 246, 32, 50, 220, 253, 12, 142, 51, 196, 83, 115, 207, 137, 15, 189, 210, 202 })]
        [InlineData(SymmetricAlgorithmType.RC2
           , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
           , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
           , new byte[] { 252, 65, 151, 21, 179, 185, 42, 121, 54, 118, 248, 137, 143, 127, 169, 174, 66, 149, 112, 85, 49, 216, 219, 10, 250, 252, 90, 83, 79, 178, 20, 228 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
           , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
           , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
           , new byte[] { 184, 150, 40, 191, 247, 112, 70, 190, 229, 174, 13, 216, 29, 96, 57, 204, 22, 173, 168, 10, 28, 149, 208, 209, 47, 98, 12, 129, 201, 95, 95, 186 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
           , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
           , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
           , new byte[] { 14, 159, 96, 171, 54, 142, 120, 224, 65, 79, 82, 5, 64, 171, 122, 87, 112, 38, 124, 114, 141, 47, 15, 39, 80, 114, 131, 191, 92, 239, 116, 246 })]
        public void Encrypt_BY_encoding_plaintext_charArray(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, byte[] expected)
        {
            char[] plaintext = new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' };

            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv))
            {
                byte[] actual = cryptography.Encrypt(Encoding.Unicode, plaintext);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
           , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
           , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
           , new byte[] { 157, 248, 129, 153, 61, 88, 18, 84, 194, 190, 21, 158, 245, 150, 170, 245, 180, 164, 171, 205, 55, 127, 238, 92, 176, 161, 148, 162, 115, 88, 91, 5 })]
        [InlineData(SymmetricAlgorithmType.DES
           , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
           , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
           , new byte[] { 106, 205, 246, 30, 22, 37, 235, 169, 77, 29, 246, 184, 195, 42, 183, 246, 32, 50, 220, 253, 12, 142, 51, 196, 83, 115, 207, 137, 15, 189, 210, 202 })]
        [InlineData(SymmetricAlgorithmType.RC2
           , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
           , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
           , new byte[] { 252, 65, 151, 21, 179, 185, 42, 121, 54, 118, 248, 137, 143, 127, 169, 174, 66, 149, 112, 85, 49, 216, 219, 10, 250, 252, 90, 83, 79, 178, 20, 228 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
           , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
           , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
           , new byte[] { 184, 150, 40, 191, 247, 112, 70, 190, 229, 174, 13, 216, 29, 96, 57, 204, 22, 173, 168, 10, 28, 149, 208, 209, 47, 98, 12, 129, 201, 95, 95, 186 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
           , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
           , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
           , new byte[] { 14, 159, 96, 171, 54, 142, 120, 224, 65, 79, 82, 5, 64, 171, 122, 87, 112, 38, 124, 114, 141, 47, 15, 39, 80, 114, 131, 191, 92, 239, 116, 246 })]
        public void Encrypt_BY_encoding_plaintext_string(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, byte[] expected)
        {
            string plaintext = "Sweety.Common";

            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv))
            {
                byte[] actual = cryptography.Encrypt(Encoding.Unicode, plaintext);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
            , new byte[] { 54, 184, 153, 191, 157, 29, 180, 60, 208, 185, 9, 29, 239, 39, 153, 194, 255, 95, 130, 46, 57, 236, 72, 46, 53, 87, 184, 251, 64, 60, 144, 0, 110, 41, 42, 146, 136, 184, 234, 254, 227, 68, 3, 158, 186, 161, 113, 158 })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
            , new byte[] { 33, 83, 93, 89, 98, 204, 139, 91, 105, 124, 52, 215, 117, 204, 60, 60, 32, 224, 200, 111, 224, 96, 178, 173, 193, 249, 117, 147, 221, 162, 186, 226, 163, 9, 252, 215, 67, 15, 113, 179 })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
            , new byte[] { 124, 20, 163, 88, 120, 177, 69, 69, 142, 16, 27, 212, 9, 139, 118, 68, 244, 222, 3, 59, 24, 230, 15, 177, 139, 245, 114, 158, 248, 222, 105, 227, 101, 246, 29, 240, 31, 149, 71, 162 })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
            , new byte[] { 236, 158, 222, 76, 107, 82, 198, 76, 35, 73, 57, 224, 188, 193, 38, 115, 73, 22, 13, 187, 138, 148, 126, 202, 205, 248, 21, 75, 154, 152, 96, 219, 152, 218, 94, 34, 192, 225, 83, 60, 202, 10, 192, 163, 66, 115, 251, 149 })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
            , new byte[] { 182, 120, 94, 180, 223, 137, 250, 2, 194, 217, 2, 86, 199, 208, 24, 152, 42, 61, 210, 82, 125, 192, 16, 124, 182, 92, 144, 116, 216, 23, 232, 121, 158, 185, 108, 179, 100, 225, 173, 141 })]
        public void Decrypt_BY_ciphertext(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, byte[] cipherText)
        {
            byte[] expected = new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 };

            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv))
            {
                byte[] actual = cryptography.Decrypt(cipherText);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
            , new char[] { 'N', 'r', 'i', 'Z', 'v', '5', '0', 'd', 't', 'D', 'z', 'Q', 'u', 'Q', 'k', 'd', '7', 'y', 'e', 'Z', 'w', 'v', '9', 'f', 'g', 'i', '4', '5', '7', 'E', 'g', 'u', 'N', 'V', 'e', '4', '+', '0', 'A', '8', 'k', 'A', 'B', 'u', 'K', 'S', 'q', 'S', 'i', 'L', 'j', 'q', '/', 'u', 'N', 'E', 'A', '5', '6', '6', 'o', 'X', 'G', 'e' })]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
            , new char[] { 'I', 'V', 'N', 'd', 'W', 'W', 'L', 'M', 'i', '1', 't', 'p', 'f', 'D', 'T', 'X', 'd', 'c', 'w', '8', 'P', 'C', 'D', 'g', 'y', 'G', '/', 'g', 'Y', 'L', 'K', 't', 'w', 'f', 'l', '1', 'k', '9', '2', 'i', 'u', 'u', 'K', 'j', 'C', 'f', 'z', 'X', 'Q', 'w', '9', 'x', 's', 'w', '=', '=' })]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
            , new char[] { 'f', 'B', 'S', 'j', 'W', 'H', 'i', 'x', 'R', 'U', 'W', 'O', 'E', 'B', 'v', 'U', 'C', 'Y', 't', '2', 'R', 'P', 'T', 'e', 'A', 'z', 's', 'Y', '5', 'g', '+', 'x', 'i', '/', 'V', 'y', 'n', 'v', 'j', 'e', 'a', 'e', 'N', 'l', '9', 'h', '3', 'w', 'H', '5', 'V', 'H', 'o', 'g', '=', '=' })]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
            , new char[] { '7', 'J', '7', 'e', 'T', 'G', 't', 'S', 'x', 'k', 'w', 'j', 'S', 'T', 'n', 'g', 'v', 'M', 'E', 'm', 'c', '0', 'k', 'W', 'D', 'b', 'u', 'K', 'l', 'H', '7', 'K', 'z', 'f', 'g', 'V', 'S', '5', 'q', 'Y', 'Y', 'N', 'u', 'Y', '2', 'l', '4', 'i', 'w', 'O', 'F', 'T', 'P', 'M', 'o', 'K', 'w', 'K', 'N', 'C', 'c', '/', 'u', 'V' })]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
            , new char[] { 't', 'n', 'h', 'e', 't', 'N', '+', 'J', '+', 'g', 'L', 'C', '2', 'Q', 'J', 'W', 'x', '9', 'A', 'Y', 'm', 'C', 'o', '9', '0', 'l', 'J', '9', 'w', 'B', 'B', '8', 't', 'l', 'y', 'Q', 'd', 'N', 'g', 'X', '6', 'H', 'm', 'e', 'u', 'W', 'y', 'z', 'Z', 'O', 'G', 't', 'j', 'Q', '=', '=' })]
        public void Decrypt_BY_base64Ciphertext_charArray(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, char[] base64Ciphertext)
        {
            byte[] expected = new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 };

            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv))
            {
                byte[] actual = cryptography.Decrypt(base64Ciphertext);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(SymmetricAlgorithmType.AES
            , new byte[] { 191, 199, 102, 200, 78, 87, 62, 199, 110, 162, 9, 33, 231, 108, 93, 35, 40, 35, 119, 155, 0, 190, 58, 74, 221, 107, 104, 172, 189, 112, 13, 6 }
            , new byte[] { 32, 71, 127, 142, 7, 169, 172, 131, 63, 214, 125, 70, 112, 11, 96, 27 }
            , "NriZv50dtDzQuQkd7yeZwv9fgi457EguNVe4+0A8kABuKSqSiLjq/uNEA566oXGe")]
        [InlineData(SymmetricAlgorithmType.DES
            , new byte[] { 195, 253, 176, 216, 47, 71, 151, 204 }
            , new byte[] { 64, 98, 146, 110, 44, 172, 87, 89 }
            , "IVNdWWLMi1tpfDTXdcw8PCDgyG/gYLKtwfl1k92iuuKjCfzXQw9xsw==")]
        [InlineData(SymmetricAlgorithmType.RC2
            , new byte[] { 223, 129, 205, 125, 205, 156, 7, 240, 29, 253, 39, 103, 53, 118, 41, 149 }
            , new byte[] { 175, 222, 68, 253, 163, 225, 26, 178 }
            , "fBSjWHixRUWOEBvUCYt2RPTeAzsY5g+xi/VynvjeaeNl9h3wH5VHog==")]
        [InlineData(SymmetricAlgorithmType.Rijndael
            , new byte[] { 122, 209, 111, 110, 193, 202, 17, 169, 207, 210, 133, 130, 130, 151, 240, 246, 234, 190, 244, 19, 195, 117, 230, 81, 230, 82, 161, 104, 195, 254, 95, 121 }
            , new byte[] { 228, 82, 6, 137, 163, 44, 134, 161, 100, 16, 49, 130, 225, 124, 24, 235 }
            , "7J7eTGtSxkwjSTngvMEmc0kWDbuKlH7KzfgVS5qYYNuY2l4iwOFTPMoKwKNCc/uV")]
        [InlineData(SymmetricAlgorithmType.TripleDES
            , new byte[] { 79, 207, 243, 151, 220, 160, 161, 154, 125, 8, 141, 146, 124, 126, 97, 85, 71, 235, 40, 35, 243, 235, 147, 224 }
            , new byte[] { 9, 229, 41, 144, 145, 42, 6, 125 }
            , "tnhetN+J+gLC2QJWx9AYmCo90lJ9wBB8tlyQdNgX6HmeuWyzZOGtjQ==")]
        public void Decrypt_BY_base64Ciphertext_string(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, string base64Ciphertext)
        {
            byte[] expected = new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 };

            using (ISymmetricCryptography cryptography = new InternalSymmetricCryptography(SymmetricAlgorithm.Create(algorithmType.ToString()), key, iv))
            {
                byte[] actual = cryptography.Decrypt(base64Ciphertext);

                Assert.Equal(expected, actual);
            }
        }




        private void Print(SymmetricAlgorithmType algorithmType, byte[] cipherText)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{algorithmType.ToString()}");
            Console.ResetColor();
            
            for (int i = 0; i < cipherText.Length; i++)
            {
                Console.Write($" ,{cipherText[i]}");
            }
            
            /*
            Console.Write(", new char[] {");
            foreach (char c in Convert.ToBase64String(cipherText).ToCharArray())
            {
                Console.Write($" ,'{c}'");
            }
            */

            Console.WriteLine("}");
            Console.WriteLine(Convert.ToBase64String(cipherText));
        }

        private void Print(ISymmetricCryptography cryptography)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{cryptography.AlgorithmName}，mode:{cryptography.Mode}，padding:{cryptography.Padding}");
            Console.ResetColor();
            Console.Write("KEY:");
            for (int i = 0; i < cryptography.Key.Length; i++)
            {
                Console.Write($" ,{cryptography.Key[i]}");
            }
            Console.WriteLine();
            Console.Write("IV:");
            for (int i = 0; i < cryptography.IV.Length; i++)
            {
                Console.Write($" ,{cryptography.IV[i]}");
            }
            Console.WriteLine();
        }
    }
}
