/*
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (actual == null)
            {
                Console.WriteLine("哈希值为 null。");
            }
            else if (actual.Length == 0)
            {
                Console.WriteLine("哈希值是个空数组。");
            }
            else
            {
                Console.WriteLine($"【{hashType.ToString()}】 哈希值：");
                Console.ResetColor();
                foreach (var b in actual)
                {
                    Console.Write($" {b},");
                }
                Console.WriteLine();
            }

*/

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using Xunit;

using Sweety.Common.Cryptography;

namespace Sweety.Common.Tests.Cryptography
{
    public class InternalHashTest_Hash
    {
        [Fact]
        public void Ctor_BY_hash()
        {
            using (InternalHash hash = new InternalHash(new SHA256Managed()))
            {
                Assert.NotNull(hash);
                Assert.Equal(nameof(SHA256Managed), hash.AlgorithmName);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5, nameof(MD5CryptoServiceProvider))]
        [InlineData(HashAlgorithmType.SHA1, nameof(SHA1CryptoServiceProvider))]
        [InlineData(HashAlgorithmType.SHA256, nameof(SHA256Managed))]
        [InlineData(HashAlgorithmType.SHA384, nameof(SHA384Managed))]
        [InlineData(HashAlgorithmType.SHA512, nameof(SHA512Managed))]
        public void AlgorithmName_GET(HashAlgorithmType hashType, string expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.AlgorithmName;

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new byte[0]
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[0]
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[0]
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[0]
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[0]
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 89, 173, 178, 78, 243, 205, 190, 2, 151, 240, 91, 57, 88, 39, 69, 63 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 153, 42, 201, 233, 29, 3, 4, 139, 86, 160, 53, 117, 74, 58, 87, 144, 4, 160, 223, 181 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 50, 210, 174, 3, 184, 182, 54, 183, 61, 225, 112, 155, 108, 240, 195, 136, 10, 225, 181, 72, 81, 13, 72, 7, 252, 10, 210, 15, 160, 77, 180, 164 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 243, 45, 188, 227, 180, 227, 75, 196, 106, 194, 47, 67, 32, 80, 141, 26, 25, 113, 211, 228, 216, 31, 195, 180, 44, 26, 31, 152, 182, 144, 86, 143, 56, 30, 162, 0, 30, 253, 236, 4, 58, 132, 195, 130, 191, 14, 121, 113 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 207, 246, 11, 71, 145, 159, 25, 23, 226, 70, 152, 237, 114, 198, 199, 130, 137, 237, 191, 238, 130, 47, 30, 205, 203, 2, 154, 31, 255, 253, 90, 56, 138, 210, 82, 197, 70, 141, 219, 134, 13, 64, 111, 118, 34, 102, 88, 230, 8, 143, 250, 244, 83, 129, 76, 24, 143, 49, 20, 126, 86, 135, 52, 4 })]
        public void GetHash_BY_bytes(HashAlgorithmType hashType, byte[] bytes, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(bytes);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 86, 246, 236, 63, 191, 243, 126, 209, 252, 136, 178, 179, 108, 137, 193, 142 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 108, 21, 5, 152, 19, 24, 176, 95, 93, 11, 150, 25, 197, 156, 201, 122, 225, 108, 215, 186 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 129, 104, 26, 85, 90, 186, 156, 218, 7, 234, 168, 2, 84, 6, 35, 160, 222, 224, 216, 110, 44, 252, 65, 142, 116, 245, 216, 164, 99, 117, 104, 31 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 48, 212, 98, 196, 72, 99, 250, 222, 215, 176, 233, 60, 180, 22, 78, 179, 25, 75, 203, 83, 138, 250, 71, 139, 39, 81, 8, 80, 6, 122, 151, 105, 193, 104, 95, 76, 34, 243, 208, 244, 120, 160, 203, 47, 123, 179, 47, 40 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 15, 117, 92, 178, 69, 244, 108, 243, 154, 178, 202, 13, 108, 152, 223, 228, 18, 214, 119, 138, 124, 67, 143, 21, 195, 220, 115, 182, 177, 68, 59, 231, 155, 73, 170, 70, 93, 152, 148, 91, 21, 96, 54, 235, 213, 100, 5, 88, 80, 188, 28, 3, 168, 34, 112, 178, 97, 53, 160, 170, 62, 208, 120, 87 })]
        public void GetHash_BY_bytes_start(HashAlgorithmType hashType, byte[] bytes, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(bytes, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 86, 246, 236, 63, 191, 243, 126, 209, 252, 136, 178, 179, 108, 137, 193, 142 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 108, 21, 5, 152, 19, 24, 176, 95, 93, 11, 150, 25, 197, 156, 201, 122, 225, 108, 215, 186 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 129, 104, 26, 85, 90, 186, 156, 218, 7, 234, 168, 2, 84, 6, 35, 160, 222, 224, 216, 110, 44, 252, 65, 142, 116, 245, 216, 164, 99, 117, 104, 31 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 48, 212, 98, 196, 72, 99, 250, 222, 215, 176, 233, 60, 180, 22, 78, 179, 25, 75, 203, 83, 138, 250, 71, 139, 39, 81, 8, 80, 6, 122, 151, 105, 193, 104, 95, 76, 34, 243, 208, 244, 120, 160, 203, 47, 123, 179, 47, 40 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 15, 117, 92, 178, 69, 244, 108, 243, 154, 178, 202, 13, 108, 152, 223, 228, 18, 214, 119, 138, 124, 67, 143, 21, 195, 220, 115, 182, 177, 68, 59, 231, 155, 73, 170, 70, 93, 152, 148, 91, 21, 96, 54, 235, 213, 100, 5, 88, 80, 188, 28, 3, 168, 34, 112, 178, 97, 53, 160, 170, 62, 208, 120, 87 })]
        public void GetHash_BY_bytes_offset_count(HashAlgorithmType hashType, byte[] bytes, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(bytes, 3, 13);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new char[0]
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[0]
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[0]
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[0]
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[0]
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 165, 139, 34, 83, 49, 137, 193, 141, 197, 94, 140, 202, 211, 225, 190, 236 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 98, 60, 213, 87, 6, 149, 201, 179, 46, 122, 87, 179, 80, 111, 246, 115, 254, 92, 115, 141 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 139, 213, 58, 246, 250, 156, 185, 202, 214, 94, 33, 241, 208, 247, 157, 235, 231, 223, 4, 77, 107, 1, 34, 60, 227, 138, 111, 89, 205, 111, 126, 91 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 126, 223, 186, 250, 208, 10, 197, 86, 74, 69, 47, 37, 156, 15, 149, 215, 151, 57, 80, 180, 0, 48, 118, 220, 70, 177, 125, 26, 171, 141, 217, 240, 104, 178, 242, 75, 69, 22, 3, 61, 18, 91, 169, 75, 129, 17, 233, 50 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 206, 125, 132, 144, 54, 75, 48, 127, 100, 102, 77, 113, 32, 179, 108, 175, 181, 200, 30, 29, 47, 156, 129, 210, 192, 236, 93, 174, 233, 250, 25, 178, 110, 190, 41, 18, 85, 202, 172, 135, 200, 88, 240, 143, 74, 137, 241, 51, 195, 124, 119, 121, 141, 113, 109, 50, 62, 108, 183, 144, 79, 17, 252, 220 })]
        public void GetHash_BY_chars(HashAlgorithmType hashType, char[] chars, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(chars);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 80, 172, 236, 13, 220, 190, 212, 226, 40, 26, 213, 40, 55, 50, 200, 193 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 102, 24, 10, 161, 145, 208, 7, 108, 37, 133, 38, 181, 167, 237, 174, 98, 32, 164, 10, 206 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 251, 241, 61, 85, 12, 138, 4, 105, 46, 13, 240, 208, 91, 109, 238, 188, 154, 216, 59, 110, 64, 4, 102, 180, 46, 240, 151, 133, 146, 188, 72, 68 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 77, 215, 75, 68, 165, 235, 249, 169, 215, 66, 30, 86, 1, 47, 123, 158, 163, 89, 222, 123, 237, 183, 69, 227, 201, 174, 254, 20, 85, 82, 60, 16, 9, 173, 2, 230, 243, 32, 206, 101, 30, 30, 173, 227, 61, 124, 246, 166 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 176, 70, 16, 88, 50, 114, 78, 169, 45, 170, 234, 145, 137, 11, 87, 43, 94, 184, 224, 9, 101, 220, 231, 197, 220, 209, 164, 164, 205, 162, 156, 148, 230, 73, 7, 71, 71, 1, 197, 182, 208, 52, 115, 42, 128, 142, 22, 104, 53, 209, 34, 144, 89, 239, 207, 147, 81, 211, 176, 68, 83, 22, 224, 34 })]
        public void GetHash_BY_chars_start(HashAlgorithmType hashType, char[] chars, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(chars, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 80, 172, 236, 13, 220, 190, 212, 226, 40, 26, 213, 40, 55, 50, 200, 193 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 102, 24, 10, 161, 145, 208, 7, 108, 37, 133, 38, 181, 167, 237, 174, 98, 32, 164, 10, 206 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 251, 241, 61, 85, 12, 138, 4, 105, 46, 13, 240, 208, 91, 109, 238, 188, 154, 216, 59, 110, 64, 4, 102, 180, 46, 240, 151, 133, 146, 188, 72, 68 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 77, 215, 75, 68, 165, 235, 249, 169, 215, 66, 30, 86, 1, 47, 123, 158, 163, 89, 222, 123, 237, 183, 69, 227, 201, 174, 254, 20, 85, 82, 60, 16, 9, 173, 2, 230, 243, 32, 206, 101, 30, 30, 173, 227, 61, 124, 246, 166 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 176, 70, 16, 88, 50, 114, 78, 169, 45, 170, 234, 145, 137, 11, 87, 43, 94, 184, 224, 9, 101, 220, 231, 197, 220, 209, 164, 164, 205, 162, 156, 148, 230, 73, 7, 71, 71, 1, 197, 182, 208, 52, 115, 42, 128, 142, 22, 104, 53, 209, 34, 144, 89, 239, 207, 147, 81, 211, 176, 68, 83, 22, 224, 34 })]
        public void GetHash_BY_chars_offset_count(HashAlgorithmType hashType, char[] chars, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(chars, 3, 10);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new char[0]
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[0]
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[0]
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[0]
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[0]
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 206, 5, 2, 98, 117, 37, 228, 5, 204, 34, 113, 23, 147, 123, 146, 106 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 228, 59, 191, 12, 250, 128, 64, 65, 120, 60, 16, 161, 144, 188, 163, 132, 52, 170, 113, 202 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 31, 229, 71, 70, 62, 1, 52, 190, 153, 58, 254, 24, 141, 245, 101, 15, 123, 167, 59, 73, 94, 126, 129, 129, 134, 167, 47, 163, 67, 108, 238, 91 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 190, 40, 3, 56, 157, 186, 111, 98, 42, 148, 176, 35, 95, 20, 91, 44, 198, 110, 243, 77, 78, 226, 92, 90, 110, 208, 164, 59, 254, 89, 186, 212, 151, 187, 232, 168, 44, 250, 6, 28, 69, 227, 222, 123, 83, 79, 171, 93 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 162, 185, 23, 141, 40, 182, 86, 202, 23, 133, 20, 116, 183, 88, 21, 140, 147, 127, 250, 26, 233, 221, 91, 36, 0, 223, 59, 69, 222, 7, 168, 8, 54, 147, 16, 176, 56, 23, 243, 176, 192, 77, 69, 127, 150, 204, 225, 171, 39, 211, 50, 239, 116, 232, 198, 59, 215, 8, 66, 43, 101, 28, 125, 91 })]
        public void GetHash_BY_encoding_chars(HashAlgorithmType hashType, char[] chars, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(Encoding.UTF8, chars);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 70, 182, 40, 0, 10, 65, 26, 94, 85, 94, 230, 32, 29, 24, 42, 93 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 193, 197, 11, 64, 211, 41, 119, 193, 51, 46, 134, 10, 110, 164, 111, 130, 171, 2, 0, 195 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 163, 247, 124, 83, 32, 126, 147, 134, 235, 218, 126, 160, 105, 96, 253, 12, 53, 72, 48, 128, 61, 68, 54, 114, 61, 206, 66, 122, 65, 32, 128, 97 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 56, 107, 40, 230, 54, 223, 243, 141, 231, 192, 37, 190, 207, 75, 102, 185, 230, 72, 96, 69, 45, 46, 3, 174, 167, 186, 215, 70, 110, 79, 161, 198, 16, 100, 78, 251, 83, 72, 51, 124, 252, 92, 67, 188, 228, 27, 232, 231 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 148, 3, 180, 30, 87, 85, 21, 83, 243, 244, 127, 141, 72, 224, 88, 225, 101, 212, 103, 53, 174, 132, 224, 28, 190, 1, 140, 183, 91, 220, 74, 95, 71, 28, 138, 230, 221, 58, 179, 36, 105, 95, 250, 99, 124, 174, 184, 46, 54, 198, 222, 87, 234, 18, 184, 252, 190, 128, 155, 5, 124, 9, 254, 203 })]
        public void GetHash_BY_encoding_chars_start(HashAlgorithmType hashType, char[] chars, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(Encoding.UTF8, chars, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 70, 182, 40, 0, 10, 65, 26, 94, 85, 94, 230, 32, 29, 24, 42, 93 })]
        [InlineData(HashAlgorithmType.SHA1
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 193, 197, 11, 64, 211, 41, 119, 193, 51, 46, 134, 10, 110, 164, 111, 130, 171, 2, 0, 195 })]
        [InlineData(HashAlgorithmType.SHA256
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 163, 247, 124, 83, 32, 126, 147, 134, 235, 218, 126, 160, 105, 96, 253, 12, 53, 72, 48, 128, 61, 68, 54, 114, 61, 206, 66, 122, 65, 32, 128, 97 })]
        [InlineData(HashAlgorithmType.SHA384
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 56, 107, 40, 230, 54, 223, 243, 141, 231, 192, 37, 190, 207, 75, 102, 185, 230, 72, 96, 69, 45, 46, 3, 174, 167, 186, 215, 70, 110, 79, 161, 198, 16, 100, 78, 251, 83, 72, 51, 124, 252, 92, 67, 188, 228, 27, 232, 231 })]
        [InlineData(HashAlgorithmType.SHA512
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 148, 3, 180, 30, 87, 85, 21, 83, 243, 244, 127, 141, 72, 224, 88, 225, 101, 212, 103, 53, 174, 132, 224, 28, 190, 1, 140, 183, 91, 220, 74, 95, 71, 28, 138, 230, 221, 58, 179, 36, 105, 95, 250, 99, 124, 174, 184, 46, 54, 198, 222, 87, 234, 18, 184, 252, 190, 128, 155, 5, 124, 9, 254, 203 })]
        public void GetHash_BY_encoding_chars_offset_count(HashAlgorithmType hashType, char[] chars, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(Encoding.UTF8, chars, 3, 10);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , ""
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , ""
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , ""
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , ""
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , ""
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , "Sweety.Common"
            , new byte[] { 165, 139, 34, 83, 49, 137, 193, 141, 197, 94, 140, 202, 211, 225, 190, 236 })]
        [InlineData(HashAlgorithmType.SHA1
            , "Sweety.Common"
            , new byte[] { 98, 60, 213, 87, 6, 149, 201, 179, 46, 122, 87, 179, 80, 111, 246, 115, 254, 92, 115, 141 })]
        [InlineData(HashAlgorithmType.SHA256
            , "Sweety.Common"
            , new byte[] { 139, 213, 58, 246, 250, 156, 185, 202, 214, 94, 33, 241, 208, 247, 157, 235, 231, 223, 4, 77, 107, 1, 34, 60, 227, 138, 111, 89, 205, 111, 126, 91 })]
        [InlineData(HashAlgorithmType.SHA384
            , "Sweety.Common"
            , new byte[] { 126, 223, 186, 250, 208, 10, 197, 86, 74, 69, 47, 37, 156, 15, 149, 215, 151, 57, 80, 180, 0, 48, 118, 220, 70, 177, 125, 26, 171, 141, 217, 240, 104, 178, 242, 75, 69, 22, 3, 61, 18, 91, 169, 75, 129, 17, 233, 50 })]
        [InlineData(HashAlgorithmType.SHA512
            , "Sweety.Common"
            , new byte[] { 206, 125, 132, 144, 54, 75, 48, 127, 100, 102, 77, 113, 32, 179, 108, 175, 181, 200, 30, 29, 47, 156, 129, 210, 192, 236, 93, 174, 233, 250, 25, 178, 110, 190, 41, 18, 85, 202, 172, 135, 200, 88, 240, 143, 74, 137, 241, 51, 195, 124, 119, 121, 141, 113, 109, 50, 62, 108, 183, 144, 79, 17, 252, 220 })]
        public void GetHash_BY_str(HashAlgorithmType hashType, string str, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(str);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , "Sweety.Common"
            , new byte[] { 80, 172, 236, 13, 220, 190, 212, 226, 40, 26, 213, 40, 55, 50, 200, 193 })]
        [InlineData(HashAlgorithmType.SHA1
            , "Sweety.Common"
            , new byte[] { 102, 24, 10, 161, 145, 208, 7, 108, 37, 133, 38, 181, 167, 237, 174, 98, 32, 164, 10, 206 })]
        [InlineData(HashAlgorithmType.SHA256
            , "Sweety.Common"
            , new byte[] { 251, 241, 61, 85, 12, 138, 4, 105, 46, 13, 240, 208, 91, 109, 238, 188, 154, 216, 59, 110, 64, 4, 102, 180, 46, 240, 151, 133, 146, 188, 72, 68 })]
        [InlineData(HashAlgorithmType.SHA384
            , "Sweety.Common"
            , new byte[] { 77, 215, 75, 68, 165, 235, 249, 169, 215, 66, 30, 86, 1, 47, 123, 158, 163, 89, 222, 123, 237, 183, 69, 227, 201, 174, 254, 20, 85, 82, 60, 16, 9, 173, 2, 230, 243, 32, 206, 101, 30, 30, 173, 227, 61, 124, 246, 166 })]
        [InlineData(HashAlgorithmType.SHA512
            , "Sweety.Common"
            , new byte[] { 176, 70, 16, 88, 50, 114, 78, 169, 45, 170, 234, 145, 137, 11, 87, 43, 94, 184, 224, 9, 101, 220, 231, 197, 220, 209, 164, 164, 205, 162, 156, 148, 230, 73, 7, 71, 71, 1, 197, 182, 208, 52, 115, 42, 128, 142, 22, 104, 53, 209, 34, 144, 89, 239, 207, 147, 81, 211, 176, 68, 83, 22, 224, 34 })]
        public void GetHash_BY_str_start(HashAlgorithmType hashType, string str, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(str, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , "Sweety.Common"
            , new byte[] { 80, 172, 236, 13, 220, 190, 212, 226, 40, 26, 213, 40, 55, 50, 200, 193 })]
        [InlineData(HashAlgorithmType.SHA1
            , "Sweety.Common"
            , new byte[] { 102, 24, 10, 161, 145, 208, 7, 108, 37, 133, 38, 181, 167, 237, 174, 98, 32, 164, 10, 206 })]
        [InlineData(HashAlgorithmType.SHA256
            , "Sweety.Common"
            , new byte[] { 251, 241, 61, 85, 12, 138, 4, 105, 46, 13, 240, 208, 91, 109, 238, 188, 154, 216, 59, 110, 64, 4, 102, 180, 46, 240, 151, 133, 146, 188, 72, 68 })]
        [InlineData(HashAlgorithmType.SHA384
            , "Sweety.Common"
            , new byte[] { 77, 215, 75, 68, 165, 235, 249, 169, 215, 66, 30, 86, 1, 47, 123, 158, 163, 89, 222, 123, 237, 183, 69, 227, 201, 174, 254, 20, 85, 82, 60, 16, 9, 173, 2, 230, 243, 32, 206, 101, 30, 30, 173, 227, 61, 124, 246, 166 })]
        [InlineData(HashAlgorithmType.SHA512
            , "Sweety.Common"
            , new byte[] { 176, 70, 16, 88, 50, 114, 78, 169, 45, 170, 234, 145, 137, 11, 87, 43, 94, 184, 224, 9, 101, 220, 231, 197, 220, 209, 164, 164, 205, 162, 156, 148, 230, 73, 7, 71, 71, 1, 197, 182, 208, 52, 115, 42, 128, 142, 22, 104, 53, 209, 34, 144, 89, 239, 207, 147, 81, 211, 176, 68, 83, 22, 224, 34 })]
        public void GetHash_BY_str_offset_count(HashAlgorithmType hashType, string str, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(str, 3, 10);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , ""
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , ""
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , ""
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , ""
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , ""
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , "Sweety.Common"
            , new byte[] { 206, 5, 2, 98, 117, 37, 228, 5, 204, 34, 113, 23, 147, 123, 146, 106 })]
        [InlineData(HashAlgorithmType.SHA1
            , "Sweety.Common"
            , new byte[] { 228, 59, 191, 12, 250, 128, 64, 65, 120, 60, 16, 161, 144, 188, 163, 132, 52, 170, 113, 202 })]
        [InlineData(HashAlgorithmType.SHA256
            , "Sweety.Common"
            , new byte[] { 31, 229, 71, 70, 62, 1, 52, 190, 153, 58, 254, 24, 141, 245, 101, 15, 123, 167, 59, 73, 94, 126, 129, 129, 134, 167, 47, 163, 67, 108, 238, 91 })]
        [InlineData(HashAlgorithmType.SHA384
            , "Sweety.Common"
            , new byte[] { 190, 40, 3, 56, 157, 186, 111, 98, 42, 148, 176, 35, 95, 20, 91, 44, 198, 110, 243, 77, 78, 226, 92, 90, 110, 208, 164, 59, 254, 89, 186, 212, 151, 187, 232, 168, 44, 250, 6, 28, 69, 227, 222, 123, 83, 79, 171, 93 })]
        [InlineData(HashAlgorithmType.SHA512
            , "Sweety.Common"
            , new byte[] { 162, 185, 23, 141, 40, 182, 86, 202, 23, 133, 20, 116, 183, 88, 21, 140, 147, 127, 250, 26, 233, 221, 91, 36, 0, 223, 59, 69, 222, 7, 168, 8, 54, 147, 16, 176, 56, 23, 243, 176, 192, 77, 69, 127, 150, 204, 225, 171, 39, 211, 50, 239, 116, 232, 198, 59, 215, 8, 66, 43, 101, 28, 125, 91 })]
        public void GetHash_BY_encoding_str(HashAlgorithmType hashType, string str, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(Encoding.UTF8, str);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , "Sweety.Common"
            , new byte[] { 70, 182, 40, 0, 10, 65, 26, 94, 85, 94, 230, 32, 29, 24, 42, 93 })]
        [InlineData(HashAlgorithmType.SHA1
            , "Sweety.Common"
            , new byte[] { 193, 197, 11, 64, 211, 41, 119, 193, 51, 46, 134, 10, 110, 164, 111, 130, 171, 2, 0, 195 })]
        [InlineData(HashAlgorithmType.SHA256
            , "Sweety.Common"
            , new byte[] { 163, 247, 124, 83, 32, 126, 147, 134, 235, 218, 126, 160, 105, 96, 253, 12, 53, 72, 48, 128, 61, 68, 54, 114, 61, 206, 66, 122, 65, 32, 128, 97 })]
        [InlineData(HashAlgorithmType.SHA384
            , "Sweety.Common"
            , new byte[] { 56, 107, 40, 230, 54, 223, 243, 141, 231, 192, 37, 190, 207, 75, 102, 185, 230, 72, 96, 69, 45, 46, 3, 174, 167, 186, 215, 70, 110, 79, 161, 198, 16, 100, 78, 251, 83, 72, 51, 124, 252, 92, 67, 188, 228, 27, 232, 231 })]
        [InlineData(HashAlgorithmType.SHA512
            , "Sweety.Common"
            , new byte[] { 148, 3, 180, 30, 87, 85, 21, 83, 243, 244, 127, 141, 72, 224, 88, 225, 101, 212, 103, 53, 174, 132, 224, 28, 190, 1, 140, 183, 91, 220, 74, 95, 71, 28, 138, 230, 221, 58, 179, 36, 105, 95, 250, 99, 124, 174, 184, 46, 54, 198, 222, 87, 234, 18, 184, 252, 190, 128, 155, 5, 124, 9, 254, 203 })]
        public void GetHash_BY_encoding_str_start(HashAlgorithmType hashType, string str, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(Encoding.UTF8, str, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , "Sweety.Common"
            , new byte[] { 70, 182, 40, 0, 10, 65, 26, 94, 85, 94, 230, 32, 29, 24, 42, 93 })]
        [InlineData(HashAlgorithmType.SHA1
            , "Sweety.Common"
            , new byte[] { 193, 197, 11, 64, 211, 41, 119, 193, 51, 46, 134, 10, 110, 164, 111, 130, 171, 2, 0, 195 })]
        [InlineData(HashAlgorithmType.SHA256
            , "Sweety.Common"
            , new byte[] { 163, 247, 124, 83, 32, 126, 147, 134, 235, 218, 126, 160, 105, 96, 253, 12, 53, 72, 48, 128, 61, 68, 54, 114, 61, 206, 66, 122, 65, 32, 128, 97 })]
        [InlineData(HashAlgorithmType.SHA384
            , "Sweety.Common"
            , new byte[] { 56, 107, 40, 230, 54, 223, 243, 141, 231, 192, 37, 190, 207, 75, 102, 185, 230, 72, 96, 69, 45, 46, 3, 174, 167, 186, 215, 70, 110, 79, 161, 198, 16, 100, 78, 251, 83, 72, 51, 124, 252, 92, 67, 188, 228, 27, 232, 231 })]
        [InlineData(HashAlgorithmType.SHA512
            , "Sweety.Common"
            , new byte[] { 148, 3, 180, 30, 87, 85, 21, 83, 243, 244, 127, 141, 72, 224, 88, 225, 101, 212, 103, 53, 174, 132, 224, 28, 190, 1, 140, 183, 91, 220, 74, 95, 71, 28, 138, 230, 221, 58, 179, 36, 105, 95, 250, 99, 124, 174, 184, 46, 54, 198, 222, 87, 234, 18, 184, 252, 190, 128, 155, 5, 124, 9, 254, 203 })]
        public void GetHash_BY_encoding_str_offset_count(HashAlgorithmType hashType, string str, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.GetHash(Encoding.UTF8, str, 3, 10);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new byte[0]
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[0]
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[0]
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[0]
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[0]
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 89, 173, 178, 78, 243, 205, 190, 2, 151, 240, 91, 57, 88, 39, 69, 63 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 153, 42, 201, 233, 29, 3, 4, 139, 86, 160, 53, 117, 74, 58, 87, 144, 4, 160, 223, 181 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 50, 210, 174, 3, 184, 182, 54, 183, 61, 225, 112, 155, 108, 240, 195, 136, 10, 225, 181, 72, 81, 13, 72, 7, 252, 10, 210, 15, 160, 77, 180, 164 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 243, 45, 188, 227, 180, 227, 75, 196, 106, 194, 47, 67, 32, 80, 141, 26, 25, 113, 211, 228, 216, 31, 195, 180, 44, 26, 31, 152, 182, 144, 86, 143, 56, 30, 162, 0, 30, 253, 236, 4, 58, 132, 195, 130, 191, 14, 121, 113 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 207, 246, 11, 71, 145, 159, 25, 23, 226, 70, 152, 237, 114, 198, 199, 130, 137, 237, 191, 238, 130, 47, 30, 205, 203, 2, 154, 31, 255, 253, 90, 56, 138, 210, 82, 197, 70, 141, 219, 134, 13, 64, 111, 118, 34, 102, 88, 230, 8, 143, 250, 244, 83, 129, 76, 24, 143, 49, 20, 126, 86, 135, 52, 4 })]
        public void GetHash_BY_stream(HashAlgorithmType hashType, byte[] buffer, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                using (Stream stream = new MemoryStream(buffer))
                {
                    var actual = hash.GetHash(stream);

                    Assert.Equal(expected, actual);
                }
            }
        }


        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 86, 246, 236, 63, 191, 243, 126, 209, 252, 136, 178, 179, 108, 137, 193, 142 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 108, 21, 5, 152, 19, 24, 176, 95, 93, 11, 150, 25, 197, 156, 201, 122, 225, 108, 215, 186 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 129, 104, 26, 85, 90, 186, 156, 218, 7, 234, 168, 2, 84, 6, 35, 160, 222, 224, 216, 110, 44, 252, 65, 142, 116, 245, 216, 164, 99, 117, 104, 31 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 48, 212, 98, 196, 72, 99, 250, 222, 215, 176, 233, 60, 180, 22, 78, 179, 25, 75, 203, 83, 138, 250, 71, 139, 39, 81, 8, 80, 6, 122, 151, 105, 193, 104, 95, 76, 34, 243, 208, 244, 120, 160, 203, 47, 123, 179, 47, 40 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 15, 117, 92, 178, 69, 244, 108, 243, 154, 178, 202, 13, 108, 152, 223, 228, 18, 214, 119, 138, 124, 67, 143, 21, 195, 220, 115, 182, 177, 68, 59, 231, 155, 73, 170, 70, 93, 152, 148, 91, 21, 96, 54, 235, 213, 100, 5, 88, 80, 188, 28, 3, 168, 34, 112, 178, 97, 53, 160, 170, 62, 208, 120, 87 })]
        public void GetHash_BY_stream_Start(HashAlgorithmType hashType, byte[] buffer, byte[] expected)
        {
            using (InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString())))
            {
                using (Stream stream = new MemoryStream(buffer))
                {
                    stream.Position = 3L;

                    var actual = hash.GetHash(stream);

                    Assert.Equal(expected, actual);
                }
            }
        }

#if !(NETFRAMEWORK || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)
        [Theory]
        [InlineData(HashAlgorithmType.MD5
            , new byte[0]
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[0]
            , new byte[] { 218, 57, 163, 238, 94, 107, 75, 13, 50, 85, 191, 239, 149, 96, 24, 144, 175, 216, 7, 9 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[0]
            , new byte[] { 227, 176, 196, 66, 152, 252, 28, 20, 154, 251, 244, 200, 153, 111, 185, 36, 39, 174, 65, 228, 100, 155, 147, 76, 164, 149, 153, 27, 120, 82, 184, 85 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[0]
            , new byte[] { 56, 176, 96, 167, 81, 172, 150, 56, 76, 217, 50, 126, 177, 177, 227, 106, 33, 253, 183, 17, 20, 190, 7, 67, 76, 12, 199, 191, 99, 246, 225, 218, 39, 78, 222, 191, 231, 111, 101, 251, 213, 26, 210, 241, 72, 152, 185, 91 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[0]
            , new byte[] { 207, 131, 225, 53, 126, 239, 184, 189, 241, 84, 40, 80, 214, 109, 128, 7, 214, 32, 228, 5, 11, 87, 21, 220, 131, 244, 169, 33, 211, 108, 233, 206, 71, 208, 209, 60, 93, 133, 242, 176, 255, 131, 24, 210, 135, 126, 236, 47, 99, 185, 49, 189, 71, 65, 122, 129, 165, 56, 50, 122, 249, 39, 218, 62 })]
        [InlineData(HashAlgorithmType.MD5
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 89, 173, 178, 78, 243, 205, 190, 2, 151, 240, 91, 57, 88, 39, 69, 63 })]
        [InlineData(HashAlgorithmType.SHA1
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 153, 42, 201, 233, 29, 3, 4, 139, 86, 160, 53, 117, 74, 58, 87, 144, 4, 160, 223, 181 })]
        [InlineData(HashAlgorithmType.SHA256
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 50, 210, 174, 3, 184, 182, 54, 183, 61, 225, 112, 155, 108, 240, 195, 136, 10, 225, 181, 72, 81, 13, 72, 7, 252, 10, 210, 15, 160, 77, 180, 164 })]
        [InlineData(HashAlgorithmType.SHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 243, 45, 188, 227, 180, 227, 75, 196, 106, 194, 47, 67, 32, 80, 141, 26, 25, 113, 211, 228, 216, 31, 195, 180, 44, 26, 31, 152, 182, 144, 86, 143, 56, 30, 162, 0, 30, 253, 236, 4, 58, 132, 195, 130, 191, 14, 121, 113 })]
        [InlineData(HashAlgorithmType.SHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 207, 246, 11, 71, 145, 159, 25, 23, 226, 70, 152, 237, 114, 198, 199, 130, 137, 237, 191, 238, 130, 47, 30, 205, 203, 2, 154, 31, 255, 253, 90, 56, 138, 210, 82, 197, 70, 141, 219, 134, 13, 64, 111, 118, 34, 102, 88, 230, 8, 143, 250, 244, 83, 129, 76, 24, 143, 49, 20, 126, 86, 135, 52, 4 })]
        public void TryGetHash_BY_source_destination_bytesWritten(HashAlgorithmType hashType, byte[] bytes, byte[] expected)
        {
            using InternalHash hash = new InternalHash(HashAlgorithm.Create(hashType.ToString()));
            
            int length = (hashType switch
            {
                HashAlgorithmType.MD5 => 128,
                HashAlgorithmType.SHA1 => 160,
                HashAlgorithmType.SHA256 => 256,
                HashAlgorithmType.SHA384 => 384,
                HashAlgorithmType.SHA512 => 512,
                _ => throw new ArgumentException("unknown item.", nameof(hashType))
            }) / 8;

            ReadOnlySpan<byte> source = new ReadOnlySpan<byte>(bytes);
            Span<byte> destination = stackalloc byte[length];

            var actual = hash.TryGetHash(source, destination, out int bytesWritten);

            Assert.True(actual);
            Assert.Equal(length, bytesWritten);
            Assert.Equal(expected, destination.ToArray());


            destination = stackalloc byte[length - 1];
            actual = hash.TryGetHash(source, destination, out bytesWritten);

            Assert.False(actual);
            Assert.Equal(0, bytesWritten);
        }
#endif
    }





    public class InternalHashTest_HMACHash
    {
        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5, nameof(HMACMD5))]
        [InlineData(HMACHashAlgorithmType.HMACSHA1, nameof(HMACSHA1))]
        [InlineData(HMACHashAlgorithmType.HMACSHA256, nameof(HMACSHA256))]
        [InlineData(HMACHashAlgorithmType.HMACSHA384, nameof(HMACSHA384))]
        [InlineData(HMACHashAlgorithmType.HMACSHA512, nameof(HMACSHA512))]
        public void AlgorithmName_GET(HMACHashAlgorithmType hashType, string expected)
        {
            using (InternalHash hash = new InternalHash(KeyedHashAlgorithm.Create(hashType.ToString())))
            {
                var actual = hash.AlgorithmName;

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[0]
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[0]
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 179, 120, 31, 11, 1, 2, 156, 182, 46, 174, 149, 58, 221, 97, 147, 103 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 47, 147, 186, 146, 122, 133, 0, 229, 211, 182, 11, 34, 138, 78, 207, 92, 75, 240, 102, 151 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 14, 92, 73, 199, 48, 38, 19, 173, 8, 162, 192, 112, 205, 43, 24, 134, 186, 42, 127, 147, 63, 114, 208, 141, 54, 194, 221, 68, 111, 102, 3 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 197, 94, 220, 149, 32, 175, 43, 216, 47, 194, 142, 66, 69, 241, 150, 185, 148, 124, 223, 4, 252, 102, 5, 107, 181, 253, 150, 130, 253, 147, 56, 214, 147, 237, 193, 179, 202, 208, 111, 10, 254, 95, 36, 116, 96, 251, 157, 127 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 194, 104, 116, 52, 251, 62, 187, 94, 161, 128, 189, 186, 198, 236, 0, 112, 1, 223, 163, 214, 165, 163, 179, 111, 180, 117, 54, 14, 172, 195, 50, 51, 73, 22, 89, 133, 142, 117, 220, 197, 185, 28, 235, 214, 200, 177, 191, 98, 36, 1, 171, 77, 98, 242, 77, 249, 170, 166, 11, 123, 144, 182, 185, 165 })]
        public void GetHash_BY_bytes(HMACHashAlgorithmType hashType, byte[] key, byte[] bytes, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(bytes);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 14, 99, 171, 46, 86, 77, 140, 31, 55, 47, 112, 155, 51, 59, 80, 172 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 74, 54, 2, 57, 19, 142, 126, 190, 156, 19, 126, 13, 93, 123, 63, 38, 124, 9, 110, 16 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 14, 250, 214, 139, 78, 154, 8, 123, 141, 186, 186, 125, 171, 151, 207, 118, 118, 55, 129, 234, 44, 223, 238, 63, 133, 211, 241, 72, 190, 179, 155, 153 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 112, 150, 99, 21, 54, 1, 52, 155, 140, 24, 220, 28, 89, 87, 48, 59, 255, 157, 144, 174, 60, 38, 131, 44, 64, 28, 200, 55, 217, 221, 95, 214, 58, 175, 78, 217, 103, 2, 46, 114, 51, 73, 253, 42, 162, 226, 25, 45 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 47, 203, 104, 197, 158, 244, 122, 247, 124, 236, 248, 105, 74, 67, 135, 27, 85, 36, 132, 202, 143, 202, 210, 203, 173, 48, 250, 36, 117, 159, 94, 184, 65, 78, 231, 85, 136, 103, 16, 215, 61, 251, 190, 11, 50, 26, 121, 218, 224, 240, 59, 97, 177, 131, 151, 251, 251, 25, 132, 71, 44, 242, 62, 34 })]
        public void GetHash_BY_bytes_start(HMACHashAlgorithmType hashType, byte[] key, byte[] bytes, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(bytes, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 14, 99, 171, 46, 86, 77, 140, 31, 55, 47, 112, 155, 51, 59, 80, 172 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 74, 54, 2, 57, 19, 142, 126, 190, 156, 19, 126, 13, 93, 123, 63, 38, 124, 9, 110, 16 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 14, 250, 214, 139, 78, 154, 8, 123, 141, 186, 186, 125, 171, 151, 207, 118, 118, 55, 129, 234, 44, 223, 238, 63, 133, 211, 241, 72, 190, 179, 155, 153 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 112, 150, 99, 21, 54, 1, 52, 155, 140, 24, 220, 28, 89, 87, 48, 59, 255, 157, 144, 174, 60, 38, 131, 44, 64, 28, 200, 55, 217, 221, 95, 214, 58, 175, 78, 217, 103, 2, 46, 114, 51, 73, 253, 42, 162, 226, 25, 45 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 47, 203, 104, 197, 158, 244, 122, 247, 124, 236, 248, 105, 74, 67, 135, 27, 85, 36, 132, 202, 143, 202, 210, 203, 173, 48, 250, 36, 117, 159, 94, 184, 65, 78, 231, 85, 136, 103, 16, 215, 61, 251, 190, 11, 50, 26, 121, 218, 224, 240, 59, 97, 177, 131, 151, 251, 251, 25, 132, 71, 44, 242, 62, 34 })]
        public void GetHash_BY_bytes_offset_count(HMACHashAlgorithmType hashType, byte[] key, byte[] bytes, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(bytes, 3, 13);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[0]
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[0]
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[0]
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[0]
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[0]
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 73, 234, 153, 149, 181, 139, 180, 75, 163, 224, 237, 110, 2, 90, 47, 29 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 199, 252, 53, 78, 163, 104, 183, 182, 154, 174, 190, 22, 175, 105, 15, 8, 154, 84, 8, 201 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 9, 90, 227, 34, 125, 244, 62, 170, 166, 228, 185, 102, 77, 255, 210, 158, 59, 120, 26, 236, 70, 38, 192, 62, 60, 204, 105, 45, 208, 43, 118, 167 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 238, 247, 245, 47, 212, 107, 100, 190, 49, 25, 229, 0, 198, 151, 123, 254, 166, 56, 195, 16, 181, 195, 121, 153, 163, 224, 84, 177, 175, 12, 68, 180, 231, 139, 252, 64, 191, 186, 173, 213, 9, 237, 77, 4, 226, 131, 104, 165 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 189, 102, 31, 130, 117, 56, 95, 44, 42, 49, 119, 73, 14, 234, 48, 11, 74, 194, 72, 147, 5, 14, 4, 224, 121, 173, 61, 83, 251, 103, 97, 232, 95, 247, 184, 152, 58, 170, 84, 66, 114, 18, 103, 24, 22, 45, 231, 61, 227, 104, 27, 245, 224, 178, 30, 152, 219, 183, 193, 22, 163, 172, 38, 176 })]
        public void GetHash_BY_chars(HMACHashAlgorithmType hashType, byte[] key, char[] chars, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(chars);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 248, 158, 113, 171, 92, 0, 131, 242, 108, 91, 112, 3, 31, 239, 22, 102 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 102, 3, 95, 186, 113, 200, 26, 20, 118, 103, 124, 215, 237, 245, 24, 246, 9, 249, 141, 118 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 193, 22, 191, 232, 76, 58, 133, 185, 81, 143, 90, 78, 123, 21, 211, 64, 40, 77, 146, 242, 126, 141, 221, 115, 221, 138, 208, 216, 30, 81, 198, 207 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 67, 253, 2, 1, 77, 17, 164, 24, 94, 238, 244, 209, 240, 204, 49, 57, 102, 124, 72, 97, 56, 17, 60, 172, 55, 12, 157, 71, 196, 51, 32, 189, 244, 199, 191, 25, 134, 97, 30, 253, 247, 243, 76, 45, 1, 87, 120, 146 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 60, 74, 215, 207, 175, 7, 233, 47, 216, 190, 215, 17, 161, 88, 14, 224, 220, 82, 231, 206, 240, 216, 193, 53, 119, 165, 171, 77, 132, 166, 26, 94, 83, 2, 98, 191, 22, 125, 22, 123, 242, 122, 89, 25, 196, 207, 210, 136, 32, 86, 164, 201, 208, 172, 103, 238, 140, 196, 86, 101, 6, 212, 254, 76 })]
        public void GetHash_BY_chars_start(HMACHashAlgorithmType hashType, byte[] key, char[] chars, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(chars, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 248, 158, 113, 171, 92, 0, 131, 242, 108, 91, 112, 3, 31, 239, 22, 102 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 102, 3, 95, 186, 113, 200, 26, 20, 118, 103, 124, 215, 237, 245, 24, 246, 9, 249, 141, 118 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 193, 22, 191, 232, 76, 58, 133, 185, 81, 143, 90, 78, 123, 21, 211, 64, 40, 77, 146, 242, 126, 141, 221, 115, 221, 138, 208, 216, 30, 81, 198, 207 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 67, 253, 2, 1, 77, 17, 164, 24, 94, 238, 244, 209, 240, 204, 49, 57, 102, 124, 72, 97, 56, 17, 60, 172, 55, 12, 157, 71, 196, 51, 32, 189, 244, 199, 191, 25, 134, 97, 30, 253, 247, 243, 76, 45, 1, 87, 120, 146 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 60, 74, 215, 207, 175, 7, 233, 47, 216, 190, 215, 17, 161, 88, 14, 224, 220, 82, 231, 206, 240, 216, 193, 53, 119, 165, 171, 77, 132, 166, 26, 94, 83, 2, 98, 191, 22, 125, 22, 123, 242, 122, 89, 25, 196, 207, 210, 136, 32, 86, 164, 201, 208, 172, 103, 238, 140, 196, 86, 101, 6, 212, 254, 76 })]
        public void GetHash_BY_chars_offset_count(HMACHashAlgorithmType hashType, byte[] key, char[] chars, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(chars, 3, 10);

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[0]
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[0]
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[0]
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[0]
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[0]
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 73, 225, 31, 28, 7, 18, 175, 137, 8, 74, 190, 124, 9, 160, 162, 6 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 55, 32, 9, 115, 146, 42, 32, 217, 148, 239, 116, 173, 155, 27, 48, 58, 29, 235, 23, 218 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 104, 17, 38, 63, 232, 195, 185, 233, 217, 198, 228, 117, 161, 181, 95, 103, 129, 94, 237, 57, 65, 0, 168, 244, 21, 171, 115, 166, 191, 187, 169, 230 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 131, 220, 55, 213, 175, 38, 19, 234, 173, 75, 39, 151, 25, 208, 70, 185, 33, 190, 79, 108, 100, 183, 192, 9, 205, 214, 9, 29, 114, 190, 67, 210, 115, 132, 122, 21, 165, 153, 93, 114, 90, 249, 234, 137, 50, 10, 135, 176 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 61, 136, 112, 70, 182, 230, 116, 14, 220, 247, 9, 108, 138, 221, 222, 0, 82, 142, 246, 168, 22, 177, 59, 97, 119, 137, 76, 32, 218, 241, 146, 110, 242, 17, 238, 215, 84, 209, 3, 197, 118, 207, 226, 13, 5, 54, 218, 159, 181, 97, 59, 79, 185, 240, 157, 226, 115, 241, 15, 198, 149, 71, 36, 190 })]
        public void GetHash_BY_encoding_chars(HMACHashAlgorithmType hashType, byte[] key, char[] chars, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(Encoding.UTF8, chars);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 107, 118, 42, 124, 90, 122, 42, 174, 27, 216, 68, 139, 72, 35, 52, 216 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 180, 186, 12, 89, 220, 172, 10, 118, 29, 215, 29, 77, 169, 2, 63, 176, 93, 0, 201, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 225, 254, 3, 174, 184, 223, 207, 5, 109, 210, 234, 47, 252, 52, 239, 33, 24, 199, 229, 53, 4, 187, 212, 39, 12, 55, 47, 225, 246, 118, 81, 94 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 199, 135, 214, 98, 102, 235, 255, 86, 110, 251, 134, 136, 128, 109, 206, 211, 246, 65, 193, 171, 232, 233, 123, 189, 225, 69, 63, 111, 52, 139, 26, 19, 38, 124, 126, 205, 121, 39, 204, 232, 80, 66, 211, 85, 128, 233, 113, 133 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 237, 181, 100, 42, 219, 174, 111, 193, 85, 214, 181, 120, 133, 218, 93, 105, 90, 233, 197, 126, 47, 139, 85, 114, 253, 92, 145, 200, 29, 15, 197, 98, 174, 133, 93, 51, 75, 34, 70, 65, 47, 180, 46, 171, 231, 221, 216, 197, 184, 25, 90, 199, 202, 39, 25, 246, 27, 22, 92, 84, 90, 34, 207, 13 })]
        public void GetHash_BY_encoding_chars_start(HMACHashAlgorithmType hashType, byte[] key, char[] chars, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(Encoding.UTF8, chars, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 107, 118, 42, 124, 90, 122, 42, 174, 27, 216, 68, 139, 72, 35, 52, 216 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 180, 186, 12, 89, 220, 172, 10, 118, 29, 215, 29, 77, 169, 2, 63, 176, 93, 0, 201, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 225, 254, 3, 174, 184, 223, 207, 5, 109, 210, 234, 47, 252, 52, 239, 33, 24, 199, 229, 53, 4, 187, 212, 39, 12, 55, 47, 225, 246, 118, 81, 94 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 199, 135, 214, 98, 102, 235, 255, 86, 110, 251, 134, 136, 128, 109, 206, 211, 246, 65, 193, 171, 232, 233, 123, 189, 225, 69, 63, 111, 52, 139, 26, 19, 38, 124, 126, 205, 121, 39, 204, 232, 80, 66, 211, 85, 128, 233, 113, 133 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new char[] { 'S', 'w', 'e', 'e', 't', 'y', '.', 'C', 'o', 'm', 'm', 'o', 'n' }
            , new byte[] { 237, 181, 100, 42, 219, 174, 111, 193, 85, 214, 181, 120, 133, 218, 93, 105, 90, 233, 197, 126, 47, 139, 85, 114, 253, 92, 145, 200, 29, 15, 197, 98, 174, 133, 93, 51, 75, 34, 70, 65, 47, 180, 46, 171, 231, 221, 216, 197, 184, 25, 90, 199, 202, 39, 25, 246, 27, 22, 92, 84, 90, 34, 207, 13 })]
        public void GetHash_BY_encoding_chars_offset_count(HMACHashAlgorithmType hashType, byte[] key, char[] chars, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(Encoding.UTF8, chars, 3, 10);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , ""
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , ""
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , ""
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , ""
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , ""
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 73, 234, 153, 149, 181, 139, 180, 75, 163, 224, 237, 110, 2, 90, 47, 29 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 199, 252, 53, 78, 163, 104, 183, 182, 154, 174, 190, 22, 175, 105, 15, 8, 154, 84, 8, 201 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 9, 90, 227, 34, 125, 244, 62, 170, 166, 228, 185, 102, 77, 255, 210, 158, 59, 120, 26, 236, 70, 38, 192, 62, 60, 204, 105, 45, 208, 43, 118, 167 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 238, 247, 245, 47, 212, 107, 100, 190, 49, 25, 229, 0, 198, 151, 123, 254, 166, 56, 195, 16, 181, 195, 121, 153, 163, 224, 84, 177, 175, 12, 68, 180, 231, 139, 252, 64, 191, 186, 173, 213, 9, 237, 77, 4, 226, 131, 104, 165 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 189, 102, 31, 130, 117, 56, 95, 44, 42, 49, 119, 73, 14, 234, 48, 11, 74, 194, 72, 147, 5, 14, 4, 224, 121, 173, 61, 83, 251, 103, 97, 232, 95, 247, 184, 152, 58, 170, 84, 66, 114, 18, 103, 24, 22, 45, 231, 61, 227, 104, 27, 245, 224, 178, 30, 152, 219, 183, 193, 22, 163, 172, 38, 176 })]
        public void GetHash_BY_str(HMACHashAlgorithmType hashType, byte[] key, string str, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(str);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 248, 158, 113, 171, 92, 0, 131, 242, 108, 91, 112, 3, 31, 239, 22, 102 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 102, 3, 95, 186, 113, 200, 26, 20, 118, 103, 124, 215, 237, 245, 24, 246, 9, 249, 141, 118 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 193, 22, 191, 232, 76, 58, 133, 185, 81, 143, 90, 78, 123, 21, 211, 64, 40, 77, 146, 242, 126, 141, 221, 115, 221, 138, 208, 216, 30, 81, 198, 207 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 67, 253, 2, 1, 77, 17, 164, 24, 94, 238, 244, 209, 240, 204, 49, 57, 102, 124, 72, 97, 56, 17, 60, 172, 55, 12, 157, 71, 196, 51, 32, 189, 244, 199, 191, 25, 134, 97, 30, 253, 247, 243, 76, 45, 1, 87, 120, 146 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 60, 74, 215, 207, 175, 7, 233, 47, 216, 190, 215, 17, 161, 88, 14, 224, 220, 82, 231, 206, 240, 216, 193, 53, 119, 165, 171, 77, 132, 166, 26, 94, 83, 2, 98, 191, 22, 125, 22, 123, 242, 122, 89, 25, 196, 207, 210, 136, 32, 86, 164, 201, 208, 172, 103, 238, 140, 196, 86, 101, 6, 212, 254, 76 })]
        public void GetHash_BY_str_start(HMACHashAlgorithmType hashType, byte[] key, string str, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(str, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 248, 158, 113, 171, 92, 0, 131, 242, 108, 91, 112, 3, 31, 239, 22, 102 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 102, 3, 95, 186, 113, 200, 26, 20, 118, 103, 124, 215, 237, 245, 24, 246, 9, 249, 141, 118 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 193, 22, 191, 232, 76, 58, 133, 185, 81, 143, 90, 78, 123, 21, 211, 64, 40, 77, 146, 242, 126, 141, 221, 115, 221, 138, 208, 216, 30, 81, 198, 207 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 67, 253, 2, 1, 77, 17, 164, 24, 94, 238, 244, 209, 240, 204, 49, 57, 102, 124, 72, 97, 56, 17, 60, 172, 55, 12, 157, 71, 196, 51, 32, 189, 244, 199, 191, 25, 134, 97, 30, 253, 247, 243, 76, 45, 1, 87, 120, 146 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 60, 74, 215, 207, 175, 7, 233, 47, 216, 190, 215, 17, 161, 88, 14, 224, 220, 82, 231, 206, 240, 216, 193, 53, 119, 165, 171, 77, 132, 166, 26, 94, 83, 2, 98, 191, 22, 125, 22, 123, 242, 122, 89, 25, 196, 207, 210, 136, 32, 86, 164, 201, 208, 172, 103, 238, 140, 196, 86, 101, 6, 212, 254, 76 })]
        public void GetHash_BY_str_offset_count(HMACHashAlgorithmType hashType, byte[] key, string str, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(str, 3, 10);

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , ""
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , ""
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , ""
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , ""
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , ""
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 73, 225, 31, 28, 7, 18, 175, 137, 8, 74, 190, 124, 9, 160, 162, 6 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 55, 32, 9, 115, 146, 42, 32, 217, 148, 239, 116, 173, 155, 27, 48, 58, 29, 235, 23, 218 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 104, 17, 38, 63, 232, 195, 185, 233, 217, 198, 228, 117, 161, 181, 95, 103, 129, 94, 237, 57, 65, 0, 168, 244, 21, 171, 115, 166, 191, 187, 169, 230 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 131, 220, 55, 213, 175, 38, 19, 234, 173, 75, 39, 151, 25, 208, 70, 185, 33, 190, 79, 108, 100, 183, 192, 9, 205, 214, 9, 29, 114, 190, 67, 210, 115, 132, 122, 21, 165, 153, 93, 114, 90, 249, 234, 137, 50, 10, 135, 176 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 61, 136, 112, 70, 182, 230, 116, 14, 220, 247, 9, 108, 138, 221, 222, 0, 82, 142, 246, 168, 22, 177, 59, 97, 119, 137, 76, 32, 218, 241, 146, 110, 242, 17, 238, 215, 84, 209, 3, 197, 118, 207, 226, 13, 5, 54, 218, 159, 181, 97, 59, 79, 185, 240, 157, 226, 115, 241, 15, 198, 149, 71, 36, 190 })]
        public void GetHash_BY_encoding_str(HMACHashAlgorithmType hashType, byte[] key, string str, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(Encoding.UTF8, str);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 107, 118, 42, 124, 90, 122, 42, 174, 27, 216, 68, 139, 72, 35, 52, 216 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 180, 186, 12, 89, 220, 172, 10, 118, 29, 215, 29, 77, 169, 2, 63, 176, 93, 0, 201, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 225, 254, 3, 174, 184, 223, 207, 5, 109, 210, 234, 47, 252, 52, 239, 33, 24, 199, 229, 53, 4, 187, 212, 39, 12, 55, 47, 225, 246, 118, 81, 94 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 199, 135, 214, 98, 102, 235, 255, 86, 110, 251, 134, 136, 128, 109, 206, 211, 246, 65, 193, 171, 232, 233, 123, 189, 225, 69, 63, 111, 52, 139, 26, 19, 38, 124, 126, 205, 121, 39, 204, 232, 80, 66, 211, 85, 128, 233, 113, 133 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 237, 181, 100, 42, 219, 174, 111, 193, 85, 214, 181, 120, 133, 218, 93, 105, 90, 233, 197, 126, 47, 139, 85, 114, 253, 92, 145, 200, 29, 15, 197, 98, 174, 133, 93, 51, 75, 34, 70, 65, 47, 180, 46, 171, 231, 221, 216, 197, 184, 25, 90, 199, 202, 39, 25, 246, 27, 22, 92, 84, 90, 34, 207, 13 })]
        public void GetHash_BY_encoding_str_start(HMACHashAlgorithmType hashType, byte[] key, string str, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(Encoding.UTF8, str, 3);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 107, 118, 42, 124, 90, 122, 42, 174, 27, 216, 68, 139, 72, 35, 52, 216 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 180, 186, 12, 89, 220, 172, 10, 118, 29, 215, 29, 77, 169, 2, 63, 176, 93, 0, 201, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , "Sweety.Common"
            , new byte[] { 225, 254, 3, 174, 184, 223, 207, 5, 109, 210, 234, 47, 252, 52, 239, 33, 24, 199, 229, 53, 4, 187, 212, 39, 12, 55, 47, 225, 246, 118, 81, 94 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 199, 135, 214, 98, 102, 235, 255, 86, 110, 251, 134, 136, 128, 109, 206, 211, 246, 65, 193, 171, 232, 233, 123, 189, 225, 69, 63, 111, 52, 139, 26, 19, 38, 124, 126, 205, 121, 39, 204, 232, 80, 66, 211, 85, 128, 233, 113, 133 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , "Sweety.Common"
            , new byte[] { 237, 181, 100, 42, 219, 174, 111, 193, 85, 214, 181, 120, 133, 218, 93, 105, 90, 233, 197, 126, 47, 139, 85, 114, 253, 92, 145, 200, 29, 15, 197, 98, 174, 133, 93, 51, 75, 34, 70, 65, 47, 180, 46, 171, 231, 221, 216, 197, 184, 25, 90, 199, 202, 39, 25, 246, 27, 22, 92, 84, 90, 34, 207, 13 })]
        public void GetHash_BY_encoding_str_offset_count(HMACHashAlgorithmType hashType, byte[] key, string str, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                var actual = hash.GetHash(Encoding.UTF8, str, 3, 10);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[0]
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[0]
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 179, 120, 31, 11, 1, 2, 156, 182, 46, 174, 149, 58, 221, 97, 147, 103 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 47, 147, 186, 146, 122, 133, 0, 229, 211, 182, 11, 34, 138, 78, 207, 92, 75, 240, 102, 151 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 14, 92, 73, 199, 48, 38, 19, 173, 8, 162, 192, 112, 205, 43, 24, 134, 186, 42, 127, 147, 63, 114, 208, 141, 54, 194, 221, 68, 111, 102, 3 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 197, 94, 220, 149, 32, 175, 43, 216, 47, 194, 142, 66, 69, 241, 150, 185, 148, 124, 223, 4, 252, 102, 5, 107, 181, 253, 150, 130, 253, 147, 56, 214, 147, 237, 193, 179, 202, 208, 111, 10, 254, 95, 36, 116, 96, 251, 157, 127 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 194, 104, 116, 52, 251, 62, 187, 94, 161, 128, 189, 186, 198, 236, 0, 112, 1, 223, 163, 214, 165, 163, 179, 111, 180, 117, 54, 14, 172, 195, 50, 51, 73, 22, 89, 133, 142, 117, 220, 197, 185, 28, 235, 214, 200, 177, 191, 98, 36, 1, 171, 77, 98, 242, 77, 249, 170, 166, 11, 123, 144, 182, 185, 165 })]
        public void GetHash_BY_stream(HMACHashAlgorithmType hashType, byte[] key, byte[] bytes, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    var actual = hash.GetHash(stream);

                    Assert.Equal(expected, actual);
                }
            }
        }


        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 14, 99, 171, 46, 86, 77, 140, 31, 55, 47, 112, 155, 51, 59, 80, 172 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 74, 54, 2, 57, 19, 142, 126, 190, 156, 19, 126, 13, 93, 123, 63, 38, 124, 9, 110, 16 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 14, 250, 214, 139, 78, 154, 8, 123, 141, 186, 186, 125, 171, 151, 207, 118, 118, 55, 129, 234, 44, 223, 238, 63, 133, 211, 241, 72, 190, 179, 155, 153 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 112, 150, 99, 21, 54, 1, 52, 155, 140, 24, 220, 28, 89, 87, 48, 59, 255, 157, 144, 174, 60, 38, 131, 44, 64, 28, 200, 55, 217, 221, 95, 214, 58, 175, 78, 217, 103, 2, 46, 114, 51, 73, 253, 42, 162, 226, 25, 45 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 47, 203, 104, 197, 158, 244, 122, 247, 124, 236, 248, 105, 74, 67, 135, 27, 85, 36, 132, 202, 143, 202, 210, 203, 173, 48, 250, 36, 117, 159, 94, 184, 65, 78, 231, 85, 136, 103, 16, 215, 61, 251, 190, 11, 50, 26, 121, 218, 224, 240, 59, 97, 177, 131, 151, 251, 251, 25, 132, 71, 44, 242, 62, 34 })]
        public void GetHash_BY_stream_start(HMACHashAlgorithmType hashType, byte[] key, byte[] bytes, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using (InternalHash hash = new InternalHash(obj))
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    stream.Position = 3L;

                    var actual = hash.GetHash(stream);

                    Assert.Equal(expected, actual);
                }
            }
        }



#if !(NETFRAMEWORK || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)
        [Theory]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 120, 2, 193, 254, 100, 124, 137, 72, 76, 131, 138, 191, 140, 50, 224, 217 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 96, 14, 209, 71, 149, 251, 61, 208, 86, 193, 74, 222, 191, 237, 3, 179, 196, 32, 111, 159 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[0]
            , new byte[] { 104, 215, 237, 176, 28, 105, 80, 16, 154, 161, 131, 224, 83, 208, 34, 71, 55, 83, 181, 74, 166, 254, 100, 231, 91, 104, 71, 173, 80, 205, 5, 61 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[0]
            , new byte[] { 96, 230, 76, 18, 33, 34, 59, 119, 163, 202, 197, 15, 225, 143, 161, 250, 131, 127, 64, 40, 227, 61, 149, 204, 182, 84, 39, 181, 170, 135, 224, 51, 8, 108, 44, 0, 156, 167, 5, 114, 242, 160, 199, 224, 112, 209, 227, 226 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[0]
            , new byte[] { 189, 105, 83, 14, 179, 59, 54, 242, 56, 129, 18, 219, 226, 87, 188, 132, 199, 83, 79, 155, 155, 237, 115, 185, 34, 250, 179, 82, 62, 202, 127, 146, 142, 113, 102, 76, 104, 159, 103, 56, 146, 50, 0, 131, 181, 23, 59, 9, 15, 37, 15, 215, 79, 114, 203, 66, 117, 118, 27, 53, 71, 27, 236, 70 })]
        [InlineData(HMACHashAlgorithmType.HMACMD5
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 179, 120, 31, 11, 1, 2, 156, 182, 46, 174, 149, 58, 221, 97, 147, 103 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA1
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 47, 147, 186, 146, 122, 133, 0, 229, 211, 182, 11, 34, 138, 78, 207, 92, 75, 240, 102, 151 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA256
            , new byte[] { 143, 0, 178, 4, 233, 128, 9, 152 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 14, 92, 73, 199, 48, 38, 19, 173, 8, 162, 192, 112, 205, 43, 24, 134, 186, 42, 127, 147, 63, 114, 208, 141, 54, 194, 221, 68, 111, 102, 3 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA384
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 197, 94, 220, 149, 32, 175, 43, 216, 47, 194, 142, 66, 69, 241, 150, 185, 148, 124, 223, 4, 252, 102, 5, 107, 181, 253, 150, 130, 253, 147, 56, 214, 147, 237, 193, 179, 202, 208, 111, 10, 254, 95, 36, 116, 96, 251, 157, 127 })]
        [InlineData(HMACHashAlgorithmType.HMACSHA512
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 }
            , new byte[] { 194, 104, 116, 52, 251, 62, 187, 94, 161, 128, 189, 186, 198, 236, 0, 112, 1, 223, 163, 214, 165, 163, 179, 111, 180, 117, 54, 14, 172, 195, 50, 51, 73, 22, 89, 133, 142, 117, 220, 197, 185, 28, 235, 214, 200, 177, 191, 98, 36, 1, 171, 77, 98, 242, 77, 249, 170, 166, 11, 123, 144, 182, 185, 165 })]
        public void TryGetHash_BY_source_destination_bytesWritten(HMACHashAlgorithmType hashType, byte[] key, byte[] bytes, byte[] expected)
        {
            var obj = KeyedHashAlgorithm.Create(hashType.ToString());
            obj.Key = key;

            using InternalHash hash = new InternalHash(obj);
            
            int length = (hashType switch
            {
                HMACHashAlgorithmType.HMACMD5 => 128,
                HMACHashAlgorithmType.HMACSHA1 => 160,
                HMACHashAlgorithmType.HMACSHA256 => 256,
                HMACHashAlgorithmType.HMACSHA384 => 384,
                HMACHashAlgorithmType.HMACSHA512 => 512,
                _ => throw new ArgumentException("unknown item.", nameof(hashType))
            }) / 8;

            ReadOnlySpan<byte> source = new ReadOnlySpan<byte>(bytes);
            Span<byte> destination = stackalloc byte[length];

            var actual = hash.TryGetHash(source, destination, out int bytesWritten);

            Assert.True(actual);
            Assert.Equal(length, bytesWritten);
            Assert.Equal(expected, destination.ToArray());

            destination = stackalloc byte[length - 1];
            actual = hash.TryGetHash(source, destination, out bytesWritten);

            Assert.False(actual);
            Assert.Equal(0, bytesWritten);
        }
#endif
    }
}
