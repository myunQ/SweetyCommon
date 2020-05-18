using System;

using Xunit;

using Sweety.Common.Extensions;

namespace Sweety.Common.Tests.Extensions
{
    public class LongExtensionsTest
    {
        [Fact]
        public void FromUnixTimestamp()
        {
            const long timestamp = 1583136977L;
            DateTime expected = new DateTime(2020, 3, 2, 16, 16, 17, DateTimeKind.Local);
            DateTime actual = timestamp.FromUnixTimestamp();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_BY_i_radix()
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
    }
}
