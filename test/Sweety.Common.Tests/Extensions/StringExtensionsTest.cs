using System;
using System.Globalization;
using System.Text;

using Xunit;

using Sweety.Common.Extensions;

namespace Sweety.Common.Tests.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("sweety.common", 13)]
        [InlineData("汉语。", 6)]
        [InlineData("🌔Emoji", 9)]
        public void ByteLength(string str, int expected)
        {
            int actual = str.ByteLength();

            Assert.Equal(expected, actual);
        }
        

        [Theory]
        [InlineData("sweety.common", 13, 0)]
        [InlineData("sweety.common", 12, 1)]
        [InlineData("sweety.common", 14, -1)]
        public void ByteLengthComparer(string str, int length, int expected)
        {
            int actual = str.ByteLengthComparer(length);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("sweety.common", 7, "sweety.")]
        [InlineData("碎骨浑不怕，要留青白在人间。", 10, "碎骨浑不怕")]
        [InlineData("碎骨浑不怕，要留青白在人间。", 3, "碎")]
        public void SubstringByte_BY_str_length(string str, int length, string expected)
        {
            string actual = str.SubstringByte(length);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("碎骨浑不怕", 4, "..", "碎..")]
        [InlineData("sweety.common", 10, "...", "sweety....")]
        [InlineData("碎骨浑不怕，要留青白在人间。", 10, "..", "碎骨浑不..")]
        [InlineData("碎骨浑不怕，要留青白在人间。", 30, "...", "碎骨浑不怕，要留青白在人间。")]
        public void SubstringByte_BY_str_length_append(string str, int length, string append, string expected)
        {
            string actual = str.SubstringByte(length, append);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("sweety.common", "utf-8", 7, "sweety.")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-8", 10, "碎骨浑")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-8", 3, "碎")]
        [InlineData("sweety.common", "unicode", 7, "swe")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "unicode", 10, "碎骨浑不怕")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "unicode", 3, "碎")]
        [InlineData("sweety.common", "utf-32", 8, "sw")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-32", 20, "碎骨浑不怕")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-32", 7, "碎")]
        public void SubstringByte_BY_str_encoding_length(string str, string characterSet, int length, string expected)
        {
            string actual = str.SubstringByte(Encoding.GetEncoding(characterSet), length);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("sweety.common", "utf-8", 7, "...", "swee...")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-8", 10, "...", "碎骨...")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-8", 42, "...", "碎骨浑不怕，要留青白在人间。")]
        [InlineData("sweety.common", "unicode", 11, "...", "sw...")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "unicode", 10, "...", "碎骨...")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "unicode", 42, "...", "碎骨浑不怕，要留青白在人间。")]
        [InlineData("sweety.common", "utf-32", 16, "...", "s...")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-32", 28, "...", "碎骨浑不...")]
        [InlineData("碎骨浑不怕，要留青白在人间。", "utf-32", 56, "...", "碎骨浑不怕，要留青白在人间。")]
        public void SubstringByte_BY_str_encoding_length_append(string str, string characterSet, int length, string append, string expected)
        {
            string actual = str.SubstringByte(Encoding.GetEncoding(characterSet), length, append);

            Assert.Equal(expected, actual);
        }

        
        [Theory]
        [InlineData("<div class=\"container\" id=\"mainContent\"><h1 class=\"x-hidden-focus\">ASP.NET</h1><p class=\"lead\">Free. Cross-platform. Open source.<br>A framework for building web apps and services with .NET and C#.</p><p><a class=\"btn btn-white btn-lg btn-scale\" href=\"/learn/aspnet/hello-world-tutorial/intro\" role=\"button\">Get Started</a><a class=\"btn btn-purple btn-lg btn-scale btn-normal-weight\" href=\"/download\" role=\"button\">Download</a></p><p>Supported on macOS, Windows, and Linux</p></div>"
            , 30
            , "<div class=\"container\" id=\"mainContent\"><h1 class=\"x-hidden-focus\">ASP.NET</h1><p class=\"lead\">Free. Cross-platform. O...</p></div>")]
        [InlineData("<p><span data-ttu-id=\"f252d-104\">本文提供 .NET Core 入门的相关信息。</span><span data-ttu-id=\"f252d-105\">可在 Windows、Linux 和 macOS 上安装 .NET Core。</span><span data-ttu-id=\"f252d-106\">你可在最喜欢的文本编辑器中编写代码并生成跨平台的库和应用程序。</span></p>"
            , 40
            , "<p><span data-ttu-id=\"f252d-104\">本文提供 .NET Core 入门的相关信息。</span><span data-ttu-id=\"f252d-105\">可在 ...</span></p>")]
        public void SubstringRetainHTML(string str, int length, string expected)
        {
            string actual = str.SubstringRetainHTML(length, "...");

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, '0', false)]
        [InlineData("", '0', false)]
        [InlineData("001", '0', false)]
        [InlineData("Sweety", 'S', false)]
        [InlineData("Sweety.C", 'C', false)]
        [InlineData("Sweety.S", 's', false)]
        [InlineData("sweety.s", 'S', false)]
        [InlineData("Sweety.s", 's', false)]
        [InlineData("sweety.S", 's', false)]
        [InlineData("Sweety.S", 'S', true)]
        [InlineData("010", '0', true)]
        public void StartsAndEndsWith_BY_str_value_char(string str, char value, bool expected)
        {
            bool actual = str.StartsAndEndsWith(value);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, "", false)]
        [InlineData("", "0", false)]
        [InlineData("001", "0", false)]
        [InlineData("Sweety", "S", false)]
        [InlineData("Sweety.C", "C", false)]
        [InlineData("Sweety.Common.sweety", "sweety", false)]
        [InlineData("sweety.Common.Sweety", "sweety", false)]
        [InlineData("Sweety.Common.Sweety", "sweety", false)]
        [InlineData("Sweety.Common.Sweety", "Sweety", true)]
        [InlineData("010123456789010", "010", true)]
        public void StartsAndEndsWith_BY_str_value_string(string str, string value, bool expected)
        {
            bool actual = str.StartsAndEndsWith(value);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, "", false)]
        [InlineData("", "0", false)]
        [InlineData("001", "0", false)]
        [InlineData("Sweety", "S", false)]
        [InlineData("Sweety.C", "C", false)]
        [InlineData("Sweety.Common.sweety", "sweety", true)]
        [InlineData("sweety.Common.Sweety", "sweety", true)]
        [InlineData("Sweety.Common.Sweety", "sweety", true)]
        [InlineData("Sweety.Common.Sweety", "Sweety", true)]
        [InlineData("010123456789010", "010", true)]
        public void StartsAndEndsWith_BY_str_value_string_comparisonType(string str, string value, bool expected)
        {
            bool actual = str.StartsAndEndsWith(value, StringComparison.OrdinalIgnoreCase);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, "", false)]
        [InlineData("", "0", false)]
        [InlineData("001", "0", false)]
        [InlineData("Sweety", "S", false)]
        [InlineData("Sweety.C", "C", false)]
        [InlineData("Sweety.Common.sweety", "sweety", true)]
        [InlineData("sweety.Common.Sweety", "sweety", true)]
        [InlineData("Sweety.Common.Sweety", "sweety", true)]
        [InlineData("Sweety.Common.Sweety", "Sweety", true)]
        [InlineData("010123456789010", "010", true)]
        public void StartsAndEndsWith_BY_str_value_string_ignoreCase_culture(string str, string value, bool expected)
        {
            bool actual = str.StartsAndEndsWith(value, true, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, '0', 'p', false)]
        [InlineData("", '0', 'p', false)]
        [InlineData("001", '0', '2', false)]
        [InlineData("Sweety", 'S', 't', false)]
        [InlineData("Sweety.Common", 's', 'n', false)]
        [InlineData("Sweety.Common", 'S', 'N', false)]
        [InlineData("Sweety.Common", 'S', 'n', true)]
        [InlineData("0123456789", '0', '9', true)]
        public void StartsAndEndsWith_BY_str_starts_char_ends_char(string str, char starts, char ends, bool expected)
        {
            bool actual = str.StartsAndEndsWith(starts, ends);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, "00", "pp", false)]
        [InlineData("", "00", "pp", false)]
        [InlineData("001", "00", "22", false)]
        [InlineData("Sweety", "S", "t", false)]
        [InlineData("Sweety.Common", "s", "n", false)]
        [InlineData("Sweety.Common", "S", "N", false)]
        [InlineData("Sweety.Common", "S", "n", true)]
        [InlineData("0123456789", "0", "9", true)]
        public void StartsAndEndsWith_BY_str_starts_string_ends_string(string str, string starts, string ends, bool expected)
        {
            bool actual = str.StartsAndEndsWith(starts, ends);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, "00", "pp", false)]
        [InlineData("", "00", "pp", false)]
        [InlineData("001", "00", "22", false)]
        [InlineData("Sweety", "S", "t", false)]
        [InlineData("Sweety.Common", "s", "n", true)]
        [InlineData("Sweety.Common", "S", "N", true)]
        [InlineData("Sweety.Common", "S", "n", true)]
        [InlineData("0123456789", "0", "9", true)]
        public void StartsAndEndsWith_BY_str_starts_string_ends_string_comparisonType(string str, string starts, string ends, bool expected)
        {
            bool actual = str.StartsAndEndsWith(starts, ends, StringComparison.OrdinalIgnoreCase);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, "00", "pp", false)]
        [InlineData("", "00", "pp", false)]
        [InlineData("001", "00", "22", false)]
        [InlineData("Sweety", "S", "t", false)]
        [InlineData("Sweety.Common", "s", "n", true)]
        [InlineData("Sweety.Common", "S", "N", true)]
        [InlineData("Sweety.Common", "S", "n", true)]
        [InlineData("0123456789", "0", "9", true)]
        public void StartsAndEndsWith_BY_str_starts_string_ends_string_ignoreCase_culture(string str, string starts, string ends, bool expected)
        {
            bool actual = str.StartsAndEndsWith(starts, ends, true, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ToInt()
        {
            string[] data = {
                "1111111111111111111111111111110"
                ,"12112122212110202100"
                ,"1333333333333332"
                ,"13344223434041"
                ,"553032005530"
                ,"104134211160"
                ,"17777777776"
                ,"5478773670"
                ,"2147483646"
                ,"a02220280"
                ,"4bb2308a6"
                ,"282ba4aa9"
                ,"1652ca930"
                ,"c87e66b6"
                ,"7ffffffe"
                ,"53g7f547"
                ,"3928g3h0"
                ,"27c57h31"
                ,"1db1f926"
                ,"140h2d90"
                ,"ikf5bf0"
                ,"ebelf94"
                ,"b5gge56"
                ,"8jmdnkl"
                ,"6oj8iom"
                ,"5ehnck9"
                ,"4clm98e"
                ,"3hk7986"
                ,"2sb6cs6"
                ,"2d09uc0"
                ,"1vvvvvu"
                ,"1lsqtl0"
                ,"1d8xqro"
                ,"15v22ul"
                ,"zik0zi"
                ,"uzuAbk"
                ,"r3y91k"
                ,"nvabc9"
                ,"kCyhb6"
                ,"ilDpqB"
                ,"gi5of0"
                ,"eq5Gx6"
                ,"d0FFim"
                ,"bsvfxA"
                ,"ajsqD4"
                ,"9h43Ij"
                ,"8kq3qu"
                ,"7tpf8G"
                ,"6HtHmK"
                ,"6blNzo"
                ,"5xAHCm"
                ,"578t6j"
                ,"4AtOnA"
                ,"4eBqQb"
                ,"3Okgie"
                ,"3woQwD"
                ,"3fIo46"
                ,"30dbGR"
                ,"2JG3e6"
                ,"2x64oV"
                ,"2lkCB0"
                ,"2akka0"
                ,"1____-"
                ,"1TjJ9Z"
                ,"1LbEr0"};


            int expected = int.MaxValue - 1;
            for (int i = 0; i < data.Length; i++)
            {
                int actual = data[i].ToInt(i + 2);

                Assert.Equal(expected, actual);
            }

            data = new string[]
            {
                "-10000000000000000000000000000000"
                ,"-12112122212110202102"
                ,"-2000000000000000"
                ,"-13344223434043"
                ,"-553032005532"
                ,"-104134211162"
                ,"-20000000000"
                ,"-5478773672"
                ,"-2147483648"
                ,"-a02220282"
                ,"-4bb2308a8"
                ,"-282ba4aab"
                ,"-1652ca932"
                ,"-c87e66b8"
                ,"-80000000"
                ,"-53g7f549"
                ,"-3928g3h2"
                ,"-27c57h33"
                ,"-1db1f928"
                ,"-140h2d92"
                ,"-ikf5bf2"
                ,"-ebelf96"
                ,"-b5gge58"
                ,"-8jmdnkn"
                ,"-6oj8ioo"
                ,"-5ehnckb"
                ,"-4clm98g"
                ,"-3hk7988"
                ,"-2sb6cs8"
                ,"-2d09uc2"
                ,"-2000000"
                ,"-1lsqtl2"
                ,"-1d8xqrq"
                ,"-15v22un"
                ,"-zik0zk"
                ,"-uzuAbm"
                ,"-r3y91m"
                ,"-nvabcb"
                ,"-kCyhb8"
                ,"-ilDpqD"
                ,"-gi5of2"
                ,"-eq5Gx8"
                ,"-d0FFio"
                ,"-bsvfxC"
                ,"-ajsqD6"
                ,"-9h43Il"
                ,"-8kq3qw"
                ,"-7tpf8I"
                ,"-6HtHmM"
                ,"-6blNzq"
                ,"-5xAHCo"
                ,"-578t6l"
                ,"-4AtOnC"
                ,"-4eBqQd"
                ,"-3Okgig"
                ,"-3woQwF"
                ,"-3fIo48"
                ,"-30dbGT"
                ,"-2JG3e8"
                ,"-2x64oX"
                ,"-2lkCB2"
                ,"-2akka2"
                ,"-200000"
                ,"-1TjJ9_"
                ,"-1LbEr2"
            };

            expected = int.MinValue;
            for (int i = 0; i < data.Length; i++)
            {
                int actual = data[i].ToInt(i + 2);

                Assert.Equal(expected, actual);
            }
        }


        [Fact]
        public void ToLong()
        {
            string[] data = { "100011010111101111110111100010100110110000011100001100000010"
                    ,"11020121210010212212110020012020002221"
                    ,"203113233313202212300130030002"
                    ,"20321120024144441443131430"
                    ,"45014002340343425401254"
                    ,"1066202553343145356000"
                    ,"43275767424660341402"
                    ,"4217703785406166087"
                    ,"637188397210911490"
                    ,"1295a095079134645a"
                    ,"35434b0425219922a"
                    ,"c5aa50a05746c929"
                    ,"414b150d96d97a70"
                    ,"16c607272981157a"
                    ,"8d7bf78a6c1c302"
                    ,"3d5b2abfca6e5b2"
                    ,"1cae8e39a3hf6hg"
                    ,"f2ggfded3aff76"
                    ,"7fb58bf6883iea"
                    ,"42d206kbeg1597"
                    ,"25cad2i2d3fk8a"
                    ,"161h406ld65m1h"
                    ,"hahi2ahb78fda"
                    ,"ah6a2loo9n89f"
                    ,"6hfih1h9n4lkm"
                    ,"46gl3nnc6562p"
                    ,"2kn6f11hpr63e"
                    ,"1n6gb1jj9qngc"
                    ,"15t2fls15knqa"
                    ,"p2cnbqj9uus5"
                    ,"hltvf2jc3go2"
                    ,"ck13laike2va"
                    ,"92mdk61d61e2"
                    ,"6kyiysr37fe0"
                    ,"4ua0fommho8y"
                    ,"3lix0eebAtel"
                    ,"2piqi2xds3w6"
                    ,"20arzBfv8pmm"
                    ,"1kur9dlkkjra"
                    ,"16jcBqBscp61"
                    ,"Bcz97urarps"
                    ,"tkymse3iFDw"
                    ,"niB9xw1qwqa"
                    ,"iw3yhqeBccp"
                    ,"f0HyxyG3snE"
                    ,"c5gHAjK8tHd"
                    ,"9D3Jm14K3Gy"
                    ,"7MejCpmxqG0"
                    ,"6qc17l3BetE"
                    ,"5hO7IMxIuHj"
                    ,"4lb1HCpGInm"
                    ,"3y5jigM6Heo"
                    ,"31aLnuELMFQ"
                    ,"2sjBjfnxr1a"
                    ,"25A8ubQkOtG"
                    ,"1HiiBwIdelp"
                    ,"1rJxIFmlrBc"
                    ,"1ewAApP9J2P"
                    ,"13dDiH9HOWa"
                    ,"StK8l3FyBB"
                    ,"L4kr3vCKtA"
                    ,"ELHO6Zo9X7"
                    ,"znLTyCMsc2"
                    ,"uNImOTcBQz"
                    ,"qROK3ICWwa"};

            long expected = 637188397210911490L;
            for (int i = 0; i < data.Length; i++)
            {
                long actual = data[i].ToLong(i + 2);

                Assert.Equal(expected, actual);
            }

            data = new string[]
            {
                "-100011010111101111110111100010100110110000011100001100000010"
                ,"-11020121210010212212110020012020002221"
                ,"-203113233313202212300130030002"
                ,"-20321120024144441443131430"
                ,"-45014002340343425401254"
                ,"-1066202553343145356000"
                ,"-43275767424660341402"
                ,"-4217703785406166087"
                ,"-637188397210911490"
                ,"-1295a095079134645a"
                ,"-35434b0425219922a"
                ,"-c5aa50a05746c929"
                ,"-414b150d96d97a70"
                ,"-16c607272981157a"
                ,"-8d7bf78a6c1c302"
                ,"-3d5b2abfca6e5b2"
                ,"-1cae8e39a3hf6hg"
                ,"-f2ggfded3aff76"
                ,"-7fb58bf6883iea"
                ,"-42d206kbeg1597"
                ,"-25cad2i2d3fk8a"
                ,"-161h406ld65m1h"
                ,"-hahi2ahb78fda"
                ,"-ah6a2loo9n89f"
                ,"-6hfih1h9n4lkm"
                ,"-46gl3nnc6562p"
                ,"-2kn6f11hpr63e"
                ,"-1n6gb1jj9qngc"
                ,"-15t2fls15knqa"
                ,"-p2cnbqj9uus5"
                ,"-hltvf2jc3go2"
                ,"-ck13laike2va"
                ,"-92mdk61d61e2"
                ,"-6kyiysr37fe0"
                ,"-4ua0fommho8y"
                ,"-3lix0eebAtel"
                ,"-2piqi2xds3w6"
                ,"-20arzBfv8pmm"
                ,"-1kur9dlkkjra"
                ,"-16jcBqBscp61"
                ,"-Bcz97urarps"
                ,"-tkymse3iFDw"
                ,"-niB9xw1qwqa"
                ,"-iw3yhqeBccp"
                ,"-f0HyxyG3snE"
                ,"-c5gHAjK8tHd"
                ,"-9D3Jm14K3Gy"
                ,"-7MejCpmxqG0"
                ,"-6qc17l3BetE"
                ,"-5hO7IMxIuHj"
                ,"-4lb1HCpGInm"
                ,"-3y5jigM6Heo"
                ,"-31aLnuELMFQ"
                ,"-2sjBjfnxr1a"
                ,"-25A8ubQkOtG"
                ,"-1HiiBwIdelp"
                ,"-1rJxIFmlrBc"
                ,"-1ewAApP9J2P"
                ,"-13dDiH9HOWa"
                ,"-StK8l3FyBB"
                ,"-L4kr3vCKtA"
                ,"-ELHO6Zo9X7"
                ,"-znLTyCMsc2"
                ,"-uNImOTcBQz"
                ,"-qROK3ICWwa"
            };
            expected = -expected;
            for (int i = 0; i < data.Length; i++)
            {
                long actual = data[i].ToLong(i + 2);

                Assert.Equal(expected, actual);
            }
        }


        [Theory]
        [InlineData("76b0826a-504e-4009-bc37-4022f41f1444", "8f513aca-35c9-4bc8-b7e2-2ebd108c1147", "76b0826a-504e-4009-bc37-4022f41f1444")]
        [InlineData("82E2F8AA-AA0A-4DDF-ACD2-B31A33090325", "8f513aca-35c9-4bc8-b7e2-2ebd108c1147", "82E2F8AA-AA0A-4DDF-ACD2-B31A33090325")]
        [InlineData("g2E2F8AA-AA0A-4DDF-ACD2-B31A33090325", "8f513aca-35c9-4bc8-b7e2-2ebd108c1147", "8f513aca-35c9-4bc8-b7e2-2ebd108c1147")]
        public void ToGuid(string guid, string defaultGuid, string expectedGuid)
        {
            Guid expected = Guid.Parse(expectedGuid);

            Guid actual = guid.ToGuid(Guid.Parse(defaultGuid));

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("0AEE02F65B", new byte[] { 10, 238, 2, 246, 91 })]
        [InlineData("0aee02f65b", new byte[] { 10, 238, 2, 246, 91 })]
        public void HexToBytes(string hex, byte[] expected)
        {
            byte[] actual = hex.HexToBytes();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ToURLSafeBase64()
        {
            string str = "bMvYht8g7tsxs6m2/eJDHKx/R5TMURtxM0tOcPMjgKRzyPS63AVFTWg0ThyZNE0BTWi2xxc+XnVlu54siHXcsg==";

            string expected = "bMvYht8g7tsxs6m2_eJDHKx_R5TMURtxM0tOcPMjgKRzyPS63AVFTWg0ThyZNE0BTWi2xxc-XnVlu54siHXcsg==";

            string actual = str.ToURLSafeBase64();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ToNormalBase64()
        {

            string str = "bMvYht8g7tsxs6m2_eJDHKx_R5TMURtxM0tOcPMjgKRzyPS63AVFTWg0ThyZNE0BTWi2xxc-XnVlu54siHXcsg==";

            string expected = "bMvYht8g7tsxs6m2/eJDHKx/R5TMURtxM0tOcPMjgKRzyPS63AVFTWg0ThyZNE0BTWi2xxc+XnVlu54siHXcsg==";

            string actual = str.ToNormalBase64();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void TryFromBase64String()
        {
            string str = "bMvYht8g7tsxs6m2/eJDHKx/R5TMURtxM0tOcPMjgKRzyPS63AVFTWg0ThyZNE0BTWi2xxc+XnVlu54siHXcsg==";

            bool actual = str.TryFromBase64String(out byte[] bytes);

            Assert.True(actual);

            actual = str.Substring(0, str.Length - 1).TryFromBase64String(out bytes);

            Assert.False(actual);
        }


        [Fact]
        public void ToSBC()
        {
            string str = "ａｂｃｄ　ｅｆｇｈｉ　ｊｋｌｍｎ　ｏｐｑｒｓｔ　ｕｖｗｘｙｚ　０１２３４　５６７８９　，．；：＇＂［］＜＞｀～！＠＃＄％＾＆＊（）－＿＋＝";
            
            string expected = "abcd efghi jklmn opqrst uvwxyz 01234 56789 ,.;:'\"[]<>`~!@#$%^&*()-_+=";

            string actual = str.ToSBC();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ToDBC()
        {
            string str = "abcd efghi jklmn opqrst uvwxyz 01234 56789 ,.;:'\"[]<>`~!@#$%^&*()-_+=";

            string expected = "ａｂｃｄ　ｅｆｇｈｉ　ｊｋｌｍｎ　ｏｐｑｒｓｔ　ｕｖｗｘｙｚ　０１２３４　５６７８９　，．；：＇＂［］＜＞｀～！＠＃＄％＾＆＊（）－＿＋＝";

            string actual = str.ToDBC();
            
            Assert.Equal(expected, actual);
        }
    }
}
