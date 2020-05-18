using System;

using Xunit;

using Sweety.Common.Verification;

namespace Sweety.Common.Tests.Verification
{
    public class CharVerificationTest
    {
        [Fact]
        public void IsAscii()
        {
            for(ushort i = ushort.MinValue; i<ushort.MaxValue; i++)
            {
                if (i < 0x80)
                {
                    Assert.True(((char)i).IsAscii());
                }
                else
                {
                    Assert.False(((char)i).IsAscii());
                }
            }
        }


        [Fact]
        public void IsSingleByte()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i < 0x0100)
                {
                    Assert.True(((char)i).IsSingleByte());
                }
                else
                {
                    Assert.False(((char)i).IsSingleByte());
                }
            }
        }


        [Fact]
        public void IsSBCDigit()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 47 && i < 58)
                {
                    Assert.True(((char)i).IsSBCDigit());
                }
                else
                {
                    Assert.False(((char)i).IsSBCDigit());
                }
            }
        }


        [Fact]
        public void IsDBCDigit()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65295 && i < 65306)
                {
                    Assert.True(((char)i).IsDBCDigit());
                }
                else
                {
                    Assert.False(((char)i).IsDBCDigit());
                }
            }
        }


        [Fact]
        public void IsSBCLetter()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 64 && i < 91 || i > 96 && i < 123)
                {
                    Assert.True(((char)i).IsSBCLetter());
                }
                else
                {
                    Assert.False(((char)i).IsSBCLetter());
                }
            }
        }


        [Fact]
        public void IsDBCLetter()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65312 && i < 65339 || i > 65344 && i < 65371)
                {
                    Assert.True(((char)i).IsDBCLetter());
                }
                else
                {
                    Assert.False(((char)i).IsDBCLetter());
                }
            }
        }


        [Fact]
        public void IsBlank()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 8 && i < 14 || i == 32)
                {
                    Assert.True(((char)i).IsBlank());
                }
                else
                {
                    Assert.False(((char)i).IsBlank());
                }
            }
        }


        [Fact]
        public void CanToSBC()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65280 && i < 65375 || i == 0x3000)
                {
                    Assert.True(((char)i).CanToSBC());
                }
                else
                {
                    Assert.False(((char)i).CanToSBC());
                }
            }
        }


        [Fact]
        public void CanToDBC()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 31 && i < 127)
                {
                    Assert.True(((char)i).CanToDBC());
                }
                else
                {
                    Assert.False(((char)i).CanToDBC());
                }
            }
        }
    }
}
