using System;

using Xunit;

using Sweety.Common.Extensions;


namespace Sweety.Common.Tests.Extensions
{
    public class CharExtensionsTest
    {
        const ushort DBC_SUB_SBC = 65248;


        [Fact]
        public void ToSBC()
        {
            char actual;
            char expected;
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 65280 && i < 65375)
                {
                    expected = (char)(i - DBC_SUB_SBC);
                    actual = ((char)i).ToSBC();
                }
                else if(i == 0x3000)
                {
                    expected = ' ';
                    actual = ((char)i).ToSBC();
                }
                else
                {
                    expected = (char)i;
                    actual = ((char)i).ToSBC();
                }

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ToDBC()
        {
            char actual;
            char expected;
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                if (i > 32 && i < 127)
                {
                    expected = (char)(i + DBC_SUB_SBC);
                    actual = ((char)i).ToDBC();
                }
                else if (i == 32)
                {
                    expected = '\x3000';
                    actual = ((char)i).ToDBC();
                }
                else
                {
                    expected = (char)i;
                    actual = ((char)i).ToDBC();
                }

                Assert.Equal(expected, actual);
            }
        }
    }
}
