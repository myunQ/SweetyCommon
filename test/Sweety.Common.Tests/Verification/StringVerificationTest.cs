using System;

using Xunit;

using Sweety.Common.Verification;

namespace Sweety.Common.Tests.Verification
{
    public class StringVerificationTest
    {
        [Fact]
        public void IsLetters()
        {
            Assert.True("SweetyａＺ".IsLetters());
            Assert.False("sweety.common".IsLetters());
            Assert.False("sweety2".IsLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 64 && i < 91 || i > 96 && i < 123 || i > 65312 && i < 65339 || i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).ToString().IsLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsLetters());
                }
            }
        }

        [Fact]
        public void IsSBCLetters()
        {
            Assert.True("Sweety".IsSBCLetters());
            Assert.False("SweetyａＺ".IsSBCLetters());
            Assert.False("sweety.common".IsSBCLetters());
            Assert.False("sweety2".IsSBCLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 64 && i < 91 || i > 96 && i < 123)
                {
                    Assert.True(((char)i).ToString().IsSBCLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCLetters());
                }
            }
        }

        [Fact]
        public void IsDBCLetters()
        {
            Assert.True("ａＺ".IsDBCLetters());
            Assert.False("ａＺ０".IsDBCLetters());
            Assert.False("ａＺ.".IsDBCLetters());
            Assert.False("ａＺ2".IsDBCLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65312 && i < 65339 || i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).ToString().IsDBCLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDBCLetters());
                }
            }
        }

        [Fact]
        public void IsLowercaseLetters()
        {
            Assert.True("aａ".IsLowercaseLetters());
            Assert.False("ａＺ".IsLowercaseLetters());
            Assert.False("aZ".IsLowercaseLetters());
            Assert.False("a2".IsLowercaseLetters());
            Assert.False("sweety.common".IsLowercaseLetters());
            Assert.False("ａ2".IsLowercaseLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 96 && i < 123 || i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).ToString().IsLowercaseLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsLowercaseLetters());
                }
            }
        }

        [Fact]
        public void IsSBCLowercaseLetters()
        {
            Assert.True("sweety".IsSBCLowercaseLetters());
            Assert.False("sweety2".IsSBCLowercaseLetters());
            Assert.False("Sweety".IsSBCLowercaseLetters());
            Assert.False("sweety.".IsSBCLowercaseLetters());
            Assert.False("sweetyａ".IsSBCLowercaseLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 96 && i < 123)
                {
                    Assert.True(((char)i).ToString().IsSBCLowercaseLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCLowercaseLetters());
                }
            }
        }

        [Fact]
        public void IsDBCLowercaseLetters()
        {
            Assert.True("ａａ".IsDBCLowercaseLetters());
            Assert.False("ａ2ａ".IsDBCLowercaseLetters());
            Assert.False("sweety".IsDBCLowercaseLetters());
            Assert.False("sweetyａ".IsDBCLowercaseLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).ToString().IsDBCLowercaseLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDBCLowercaseLetters());
                }
            }
        }

        [Fact]
        public void IsUppercaseLetters()
        {
            Assert.True("SWEETYＡＺ".IsUppercaseLetters());
            Assert.False("ａａ".IsUppercaseLetters());
            Assert.False("sweety".IsUppercaseLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 64 && i < 91 || i > 65312 && i < 65339)
                {
                    Assert.True(((char)i).ToString().IsUppercaseLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsUppercaseLetters());
                }
            }
        }

        [Fact]
        public void IsSBCUppercaseLetters()
        {
            Assert.True("SWEETY".IsSBCUppercaseLetters());
            Assert.False("SWEETYＡＺ".IsSBCUppercaseLetters());
            Assert.False("ａａ".IsSBCUppercaseLetters());
            Assert.False("sweety".IsSBCUppercaseLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 64 && i < 91)
                {
                    Assert.True(((char)i).ToString().IsSBCUppercaseLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCUppercaseLetters());
                }
            }
        }

        [Fact]
        public void IsDBCUppercaseLetters()
        {
            Assert.True("ＡＺ".IsDBCUppercaseLetters());
            Assert.False("SWEETYＡＺ".IsDBCUppercaseLetters());
            Assert.False("ａａ".IsDBCUppercaseLetters());
            Assert.False("sweety".IsDBCUppercaseLetters());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65312 && i < 65339)
                {
                    Assert.True(((char)i).ToString().IsDBCUppercaseLetters());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDBCUppercaseLetters());
                }
            }
        }

        [Fact]
        public void IsDigit()
        {
            Assert.True("12０９".IsDigit());
            Assert.False("12ＡＺ".IsDigit());
            Assert.False("12SWEETYＡＺ".IsDigit());
            Assert.False("12ａａ".IsDigit());
            Assert.False("12sweety".IsDigit());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58 || i > 65295 && i < 65306)
                {
                    Assert.True(((char)i).ToString().IsDigit());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDigit());
                }
            }
        }

        [Fact]
        public void IsSBCDigit()
        {
            Assert.True("12".IsSBCDigit());
            Assert.False("12０９".IsSBCDigit());
            Assert.False("12ＡＺ".IsSBCDigit());
            Assert.False("12sweety".IsSBCDigit());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58)
                {
                    Assert.True(((char)i).ToString().IsSBCDigit());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCDigit());
                }
            }
        }

        [Fact]
        public void IsDBCDigit()
        {
            Assert.True("０９".IsDBCDigit());
            Assert.False("12ＡＺ".IsDBCDigit());
            Assert.False("12０９".IsDBCDigit());
            Assert.False("12ａａ".IsDBCDigit());
            Assert.False("12sweety".IsDBCDigit());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65295 && i < 65306)
                {
                    Assert.True(((char)i).ToString().IsDBCDigit());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDBCDigit());
                }
            }
        }

        [Fact]
        public void IsInteger()
        {
            Assert.True("12".IsInteger());
            Assert.True("+12".IsInteger());
            Assert.True("-12".IsInteger());
            Assert.True("－12".IsInteger());
            Assert.True("＋12".IsInteger());

            Assert.True("９０".IsInteger());
            Assert.True("+９０".IsInteger());
            Assert.True("-９０".IsInteger());
            Assert.True("－９０".IsInteger());
            Assert.True("＋９０".IsInteger());

            Assert.True("1９".IsInteger());
            Assert.True("+1９".IsInteger());
            Assert.True("-1９".IsInteger());
            Assert.True("－1９".IsInteger());
            Assert.True("＋1９".IsInteger());

            Assert.False("12.0".IsInteger());
            Assert.False("12f".IsInteger());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58 || i > 65295 && i < 65306)
                {
                    Assert.True(((char)i).ToString().IsInteger());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsInteger());
                }
            }
        }

        [Fact]
        public void IsSBCInteger()
        {
            Assert.True("12".IsSBCInteger());
            Assert.True("+12".IsSBCInteger());
            Assert.True("-12".IsSBCInteger());
            Assert.False("－12".IsSBCInteger());
            Assert.False("＋12".IsSBCInteger());

            Assert.False("９０".IsSBCInteger());
            Assert.False("+９０".IsSBCInteger());
            Assert.False("-９０".IsSBCInteger());
            Assert.False("－９０".IsSBCInteger());
            Assert.False("＋９０".IsSBCInteger());

            Assert.False("1９".IsSBCInteger());
            Assert.False("+1９".IsSBCInteger());
            Assert.False("-1９".IsSBCInteger());
            Assert.False("－1９".IsSBCInteger());
            Assert.False("＋1９".IsSBCInteger());

            Assert.False("12.0".IsSBCInteger());
            Assert.False("12f".IsSBCInteger());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58)
                {
                    Assert.True(((char)i).ToString().IsSBCInteger());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCInteger());
                }
            }
        }

        [Fact]
        public void IsDBCInteger()
        {
            Assert.True("９０".IsDBCInteger());
            Assert.True("－９０".IsDBCInteger());
            Assert.True("＋９０".IsDBCInteger());
            Assert.False("+９０".IsDBCInteger());
            Assert.False("-９０".IsDBCInteger());

            Assert.False("12".IsDBCInteger());
            Assert.False("+12".IsDBCInteger());
            Assert.False("-12".IsDBCInteger());
            Assert.False("－12".IsDBCInteger());
            Assert.False("＋12".IsDBCInteger());


            Assert.False("1９".IsDBCInteger());
            Assert.False("+1９".IsDBCInteger());
            Assert.False("-1９".IsDBCInteger());
            Assert.False("－1９".IsDBCInteger());
            Assert.False("＋1９".IsDBCInteger());

            Assert.False("12.0".IsDBCInteger());
            Assert.False("12f".IsDBCInteger());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65295 && i < 65306)
                {
                    Assert.True(((char)i).ToString().IsDBCInteger());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDBCInteger());
                }
            }
        }

        [Theory]
        [InlineData("12.3", true)]
        [InlineData("+12.0", true)]
        [InlineData("-12.0", true)]
        [InlineData("－12.6", true)]
        [InlineData("＋12.2", true)]
        [InlineData("９.０", true)]
        [InlineData("+９.０", true)]
        [InlineData("-９.０", true)]
        [InlineData("－９.０", true)]
        [InlineData("＋９.０", true)]
        [InlineData("1.９", true)]
        [InlineData("+1.９", true)]
        [InlineData("-1.９", true)]
        [InlineData("－1.９", true)]
        [InlineData("＋1.９", true)]
        [InlineData("12", false)]
        [InlineData("９０", false)]
        [InlineData("12.3f", false)]
        [InlineData("12.3.4", false)]
        public void IsDecimal(string str, bool expected)
        {
            if (expected)
            {
                Assert.True(str.IsDecimal());
            }
            else
            {
                Assert.False(str.IsDecimal());
            }
        }

        [Theory]
        [InlineData("12.3", true)]
        [InlineData("+12.0", true)]
        [InlineData("-12.0", true)]
        [InlineData("－12.6", false)]
        [InlineData("＋12.2", false)]
        [InlineData("９.０", false)]
        [InlineData("+９.０", false)]
        [InlineData("-９.０", false)]
        [InlineData("－９.０", false)]
        [InlineData("＋９.０", false)]
        [InlineData("1.９", false)]
        [InlineData("+1.９", false)]
        [InlineData("-1.９", false)]
        [InlineData("－1.９", false)]
        [InlineData("＋1.９", false)]
        [InlineData("12", false)]
        [InlineData("９０", false)]
        [InlineData("12.3f", false)]
        [InlineData("12.3.4", false)]
        public void IsSBCDecimal(string str, bool expected)
        {
            if (expected)
            {
                Assert.True(str.IsSBCDecimal());
            }
            else
            {
                Assert.False(str.IsSBCDecimal());
            }
        }

        [Theory]
        [InlineData("９.０", true)]
        [InlineData("－９.０", true)]
        [InlineData("＋９.０", true)]
        [InlineData("12.3", false)]
        [InlineData("+12.0", false)]
        [InlineData("-12.0", false)]
        [InlineData("－12.6", false)]
        [InlineData("＋12.2", false)]
        [InlineData("+９.０", false)]
        [InlineData("-９.０", false)]
        [InlineData("1.９", false)]
        [InlineData("+1.９", false)]
        [InlineData("-1.９", false)]
        [InlineData("－1.９", false)]
        [InlineData("＋1.９", false)]
        [InlineData("12", false)]
        [InlineData("９０", false)]
        [InlineData("12.3f", false)]
        [InlineData("９.０.９", false)]
        public void IsDBCDecimal(string str, bool expected)
        {
            if (expected)
            {
                Assert.True(str.IsDBCDecimal());
            }
            else
            {
                Assert.False(str.IsDBCDecimal());
            }
        }

        [Fact]
        public void IsLetterOrDigit()
        {
            Assert.True("Sweety12９ａＺ".IsLetterOrDigit());
            Assert.True("sweety2".IsLetterOrDigit());
            Assert.False("sweety.common".IsLetterOrDigit());
            

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58 || i > 64 && i < 91 || i > 96 && i < 123 || i > 65295 && i < 65306 || i > 65312 && i < 65339 || i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).ToString().IsLetterOrDigit());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsLetterOrDigit());
                }
            }
        }

        [Fact]
        public void IsSBCLetterOrDigit()
        {
            Assert.True("sweety2".IsSBCLetterOrDigit());
            Assert.False("Sweety12９ａＺ".IsSBCLetterOrDigit());
            Assert.False("sweety.common".IsSBCLetterOrDigit());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58 || i > 64 && i < 91 || i > 96 && i < 123)
                {
                    Assert.True(((char)i).ToString().IsSBCLetterOrDigit());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCLetterOrDigit());
                }
            }
        }

        [Fact]
        public void IsDBCLetterOrDigit()
        {
            Assert.True("９ａＺ".IsDBCLetterOrDigit());
            Assert.False("sweety2".IsDBCLetterOrDigit());
            Assert.False("Sweety12９ａＺ".IsDBCLetterOrDigit());
            Assert.False("sweety.common".IsDBCLetterOrDigit());

            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65295 && i < 65306 || i > 65312 && i < 65339 || i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).ToString().IsDBCLetterOrDigit());
                }
                else
                {
                    Assert.False(((char)i).ToString().IsDBCLetterOrDigit());
                }
            }
        }

        [Fact]
        public void IsSBCLetterOrSBCDigitOrOther()
        {
            Assert.True("sweety2".IsSBCLetterOrSBCDigitOrOther());
            Assert.True("Sweety12ａ".IsSBCLetterOrSBCDigitOrOther('#', 'ａ'));
            Assert.True("９ａＺ".IsSBCLetterOrSBCDigitOrOther('９','ａ','Ｚ'));
            Assert.True("sweety.common".IsSBCLetterOrSBCDigitOrOther('.'));
            Assert.True("C#".IsSBCLetterOrSBCDigitOrOther('#'));
            
            Assert.False("C#".IsSBCLetterOrSBCDigitOrOther('&'));
            Assert.False("９ａＺ".IsSBCLetterOrSBCDigitOrOther('#'));
            Assert.False("Sweety12９ａＺ".IsSBCLetterOrSBCDigitOrOther());
            Assert.False("sweety.common".IsSBCLetterOrSBCDigitOrOther());
            
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 45 && i < 58 || i > 64 && i < 91 || i > 96 && i < 123)
                {
                    Assert.True(((char)i).ToString().IsSBCLetterOrSBCDigitOrOther('.', '/'));
                }
                else
                {
                    Assert.False(((char)i).ToString().IsSBCLetterOrSBCDigitOrOther());
                }
            }
        }

        [Theory]
        [InlineData("myun_18@126.com", true)]
        [InlineData("test@126.com.cn", true)]
        [InlineData("test-23@126.com", true)]
        [InlineData("test#23@126.com", false)]
        [InlineData("test#23@com", false)]
        [InlineData("23123.com", false)]
        public void IsEmailAddress(string email, bool expected)
        {
            if (expected)
            {
                Assert.True(email.IsEmailAddress());
            }
            else
            {
                Assert.False(email.IsEmailAddress());
            }
        }

        [Fact]
        public void IsChinese()
        {

        }

        [Theory]
        [InlineData(null, 6, true, true, true)]
        [InlineData("", 6, true, true, true)]
        public void IsUnsafePassword(string pwd, byte minLength, bool allowPureDigit, bool allowPureLetter, bool expected)
        {
            if (expected)
            {
                Assert.True(pwd.IsUnsafePassword(minLength, allowPureDigit, allowPureLetter));
            }
            else
            {
                Assert.False(pwd.IsUnsafePassword(minLength, allowPureDigit, allowPureLetter));
            }
        }

        [Theory]
        [InlineData("6F9619FF-8B86-D011-B42D-00C04FC964FF", true)]
        [InlineData("d7fcb515-810d-4e1b-87ed-7c54f8bd12c2", true)]
        [InlineData("e4a494bb-6cdc-4f6g-970c-77626b740226", false)]
        [InlineData("e4a494bb-6cdc4f6f-970c-77626b740226", false)]
        [InlineData("e4a494bb6cdc4f6f970c77626b740226", false)]
        public void IsGUID(string str, bool expected)
        {
            if (expected)
            {
                Assert.True(str.IsGUID());
            }
            else
            {
                Assert.False(str.IsGUID());
            }
        }
    }
}
