using System;

using Xunit;

using Sweety.Common.Extensions;

namespace Sweety.Common.Tests.Extensions
{
    public class IntExtensionsTest
    {
        [Fact]
        public void ToString_BY_i_radix()
        {
            string[] expectedArr = {
                "1111111111111111111111111111111"
                ,"12112122212110202101"
                ,"1333333333333333"
                ,"13344223434042"
                ,"553032005531"
                ,"104134211161"
                ,"17777777777"
                ,"5478773671"
                ,"2147483647"
                ,"a02220281"
                ,"4bb2308a7"
                ,"282ba4aaa"
                ,"1652ca931"
                ,"c87e66b7"
                ,"7fffffff"
                ,"53g7f548"
                ,"3928g3h1"
                ,"27c57h32"
                ,"1db1f927"
                ,"140h2d91"
                ,"ikf5bf1"
                ,"ebelf95"
                ,"b5gge57"
                ,"8jmdnkm"
                ,"6oj8ion"
                ,"5ehncka"
                ,"4clm98f"
                ,"3hk7987"
                ,"2sb6cs7"
                ,"2d09uc1"
                ,"1vvvvvv"
                ,"1lsqtl1"
                ,"1d8xqrp"
                ,"15v22um"
                ,"zik0zj"
                ,"uzuAbl"
                ,"r3y91l"
                ,"nvabca"
                ,"kCyhb7"
                ,"ilDpqC"
                ,"gi5of1"
                ,"eq5Gx7"
                ,"d0FFin"
                ,"bsvfxB"
                ,"ajsqD5"
                ,"9h43Ik"
                ,"8kq3qv"
                ,"7tpf8H"
                ,"6HtHmL"
                ,"6blNzp"
                ,"5xAHCn"
                ,"578t6k"
                ,"4AtOnB"
                ,"4eBqQc"
                ,"3Okgif"
                ,"3woQwE"
                ,"3fIo47"
                ,"30dbGS"
                ,"2JG3e7"
                ,"2x64oW"
                ,"2lkCB1"
                ,"2akka1"
                ,"1_____"
                ,"1TjJ9-"
                ,"1LbEr1" };


            for (int i = 2; i < 67; i++)
            {
                string actual = int.MaxValue.ToString(i);

                Assert.Equal(expectedArr[i - 2], actual);
            }


            expectedArr = new string[] {
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
                ,"-1LbEr2"};

            for (int i = 2; i < 67; i++)
            {
                string actual = int.MinValue.ToString(i);

                Assert.Equal(expectedArr[i - 2], actual);
            }


            expectedArr = new string[]
            {
                "-10000100111100"
                ,"-102200010"
                ,"-2010330"
                ,"-233013"
                ,"-103220"
                ,"-33543"
                ,"-20474"
                ,"-12603"
                ,"-8508"
                ,"-6435"
                ,"-4b10"
                ,"-3b46"
                ,"-315a"
                ,"-27c3"
                ,"-213c"
                ,"-1c78"
                ,"-184c"
                ,"-14af"
                ,"-1158"
                ,"-j63"
                ,"-hcg"
                ,"-g1l"
                ,"-eic"
                ,"-df8"
                ,"-cf6"
                ,"-bi3"
                ,"-ano"
                ,"-a3b"
                ,"-9di"
                ,"-8qe"
                ,"-89s"
                ,"-7qr"
                ,"-7c8"
                ,"-6x3"
                ,"-6kc"
                ,"-67z"
                ,"-5xy"
                ,"-5n6"
                ,"-5cs"
                ,"-52l"
                ,"-4yo"
                ,"-4pB"
                ,"-4hg"
                ,"-493"
                ,"-40I"
                ,"-3E1"
                ,"-3xc"
                ,"-3qv"
                ,"-3k8"
                ,"-3dG"
                ,"-37w"
                ,"-31s"
                ,"-2Nu"
                ,"-2IC"
                ,"-2DQ"
                ,"-2zf"
                ,"-2uE"
                ,"-2qc"
                ,"-2lM"
                ,"-2ht"
                ,"-2de"
                ,"-293"
                ,"-24Y"
                ,"-20W"
                ,"-1-Y"
            };

            for (int i = 2; i < 67; i++)
            {
                string actual = (-8508).ToString(i);

                Assert.Equal(expectedArr[i - 2], actual);
            }
        }
    }
}
