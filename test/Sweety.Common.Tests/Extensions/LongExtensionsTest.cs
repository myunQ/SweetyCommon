using System;

using Xunit;

using Sweety.Common.Extensions;

namespace Sweety.Common.Tests.Extensions
{
    public class LongExtensionsTest
    {
        [Theory]
        [InlineData(1583136977L, DateTimeKind.Local)]
        [InlineData(1583165777L, DateTimeKind.Utc)]
        public void FromUnixTimestamp_long_DateTimeKind(long timestamp, DateTimeKind kind)
        {
            DateTime expected = new DateTime(2020, 3, 2, 16, 16, 17, kind);
            DateTime actual = timestamp.FromUnixTimestamp(kind);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1583136977L, DateTimeKind.Local)]
        [InlineData(1583165777L, DateTimeKind.Utc)]
        public void FromUnixTimestamp_long_TimeZoneInfo(long timestamp, DateTimeKind kind)
        {
            DateTime expected = new DateTime(2020, 3, 2, 16, 16, 17, kind);
            DateTime actual = timestamp.FromUnixTimestamp(kind == DateTimeKind.Utc ? TimeZoneInfo.Utc : TimeZoneInfo.Local);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_long_long()
        {
            string[] expectedArr = {
                "100011010111101111110111100010100110110000011100001100000010"
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


            for (int i = 2; i < 67; i++)
            {
                string actual = 637188397210911490L.ToString(i);
                Assert.Equal(expectedArr[i - 2], actual);
            }

            expectedArr = new string[]
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

            for (int i = 2; i < 67; i++)
            {
                string actual = (-637188397210911490L).ToString(i);

                Assert.Equal(expectedArr[i - 2], actual);
            }
        }


        [Theory]
        [InlineData(2021, "2021-01-01 00:00:00.000", true)]
        [InlineData(1000, "1000-01-01 00:00:00.000", true)]
        [InlineData(20200229, "2020-02-29 00:00:00.000", true)]
        [InlineData(202103, "2021-03-01 00:00:00.000", true)]
        [InlineData(20210421, "2021-04-21 00:00:00.000", true)]
        [InlineData(2021042113, "2021-04-21 13:00:00.000", true)]
        [InlineData(2021022800, "2021-02-28 00:00:00.000", true)]
        [InlineData(2021013109, "2021-01-31 09:00:00.000", true)]
        [InlineData(2021033116, "2021-03-31 16:00:00.000", true)]
        [InlineData(2021053123, "2021-05-31 23:00:00.000", true)]
        [InlineData(2021073103, "2021-07-31 03:00:00.000", true)]
        [InlineData(2021083111, "2021-08-31 11:00:00.000", true)]
        [InlineData(2021103106, "2021-10-31 06:00:00.000", true)]
        [InlineData(2021123102, "2021-12-31 02:00:00.000", true)]
        [InlineData(202112310218, "2021-12-31 02:18:00.000", true)]
        [InlineData(20211231021859, "2021-12-31 02:18:59.000", true)]
        [InlineData(20211231021859987, "2021-12-31 02:18:59.987", true)]
        [InlineData(20211231235959999, "2021-12-31 23:59:59.999", true)]
        [InlineData(20211231020000000, "2021-12-31 02:00:00.000", true)]
        [InlineData(20211231021800, "2021-12-31 02:18:00.000", true)]
        [InlineData(20211231021859000, "2021-12-31 02:18:59.000", true)]
        [InlineData(202, "202-01-01 00:00:00", false)]
        [InlineData(20213, null, false)]
        [InlineData(20210, null, false)]
        [InlineData(202100, null, false)]
        [InlineData(202113, null, false)]
        [InlineData(2021040, null, false)]
        [InlineData(2021045, null, false)]
        [InlineData(20210001, null, false)]
        [InlineData(20210400, null, false)]
        [InlineData(20210231, null, false)]
        [InlineData(20210431, null, false)]
        [InlineData(20210631, null, false)]
        [InlineData(20210931, null, false)]
        [InlineData(20211131, null, false)]
        [InlineData(20210450, null, false)]
        [InlineData(20210229, null, false)]
        [InlineData(202102280, null, false)]
        [InlineData(202102281, null, false)]
        [InlineData(2021022824, null, false)]
        [InlineData(2021022896, null, false)]
        [InlineData(20211231028, null, false)]
        [InlineData(2021123102189, null, false)]
        [InlineData(2021123102185997, null, false)]
        [InlineData(202112310260, null, false)]
        [InlineData(20211231021860, null, false)]
        [InlineData(202112310218590001, null, false)]
        public void ToDateTimeTest(long value, string expected, bool result)
        {
            const string format = "yyyy-MM-dd HH:mm:ss.fff";

            if (result)
            {
                Assert.True(value.TryToDateTime(DateTimeKind.Utc, out var actual));
                Assert.Equal(expected, actual.ToString(format));
                Assert.Equal(DateTimeKind.Utc, actual.Kind);

                Assert.True(value.TryToDateTime(DateTimeKind.Local, out actual));
                Assert.Equal(expected, actual.ToString(format));
                Assert.Equal(DateTimeKind.Local, actual.Kind);

                Assert.True(value.TryToDateTime(DateTimeKind.Unspecified, out actual));
                Assert.Equal(expected, actual.ToString(format));
                Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
            }
            else
            {
                Assert.False(value.TryToDateTime(DateTimeKind.Utc, out var actual));
            }
        }
    }
}
