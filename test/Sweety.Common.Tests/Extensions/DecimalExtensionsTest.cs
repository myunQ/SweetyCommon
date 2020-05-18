using System;

using Xunit;

using Sweety.Common.Extensions;

namespace Sweety.Common.Tests.Extensions
{
    public class DecimalExtensionsTest
    {
        [Theory]
        [InlineData("18446744082740936738.269329", new byte[] { 0x91, 0xe8, 0xa, 0x36, 0xff, 0x15, 0x20, 0, 0x40, 0x42, 0xf, 0, 0, 0, 0x6, 0 })]
        [InlineData("39237629067093", new byte[] { 0x55, 0x3f, 0xf1, 0xb8, 0xaf, 0x23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData("-87927894869209762096.3968", new byte[] { 0x80, 0xf2, 0xe2, 0xe5, 0xee, 0x7f, 0xac, 0xce, 0x31, 0xba, 0, 0, 0, 0, 0x4, 0x80 })]
        [InlineData("-2309535777355735702993609", new byte[] { 0xc9, 0x7a, 0xcc, 0x15, 0x10, 0xe4, 0x73, 0x2f, 0x10, 0xe9, 0x1, 0, 0, 0, 0, 0x80 })]
        public void GetBytes(string value, byte[] expected)
        {
            var actual = decimal.Parse(value).GetBytes();

            Assert.Equal(expected, actual);
        }

#if !NETCOREAPP2_0
        [Theory]
        [InlineData("18446744082740936738.269329", new byte[] { 0x91, 0xe8, 0xa, 0x36, 0xff, 0x15, 0x20, 0, 0x40, 0x42, 0xf, 0, 0, 0, 0x6, 0 })]
        [InlineData("39237629067093", new byte[] { 0x55, 0x3f, 0xf1, 0xb8, 0xaf, 0x23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData("-87927894869209762096.3968", new byte[] { 0x80, 0xf2, 0xe2, 0xe5, 0xee, 0x7f, 0xac, 0xce, 0x31, 0xba, 0, 0, 0, 0, 0x4, 0x80 })]
        [InlineData("-2309535777355735702993609", new byte[] { 0xc9, 0x7a, 0xcc, 0x15, 0x10, 0xe4, 0x73, 0x2f, 0x10, 0xe9, 0x1, 0, 0, 0, 0, 0x80 })]
        public void TryWriteBytes_for_stackalloc(string value, byte[] expected)
        {
            Span<byte> actual = stackalloc byte[16];

            Assert.True( decimal.Parse(value).TryWriteBytes(actual));
            Assert.Equal(expected, actual.ToArray());
        }

        [Theory]
        [InlineData("18446744082740936738.269329", new byte[] { 0x91, 0xe8, 0xa, 0x36, 0xff, 0x15, 0x20, 0, 0x40, 0x42, 0xf, 0, 0, 0, 0x6, 0 })]
        [InlineData("39237629067093", new byte[] { 0x55, 0x3f, 0xf1, 0xb8, 0xaf, 0x23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData("-87927894869209762096.3968", new byte[] { 0x80, 0xf2, 0xe2, 0xe5, 0xee, 0x7f, 0xac, 0xce, 0x31, 0xba, 0, 0, 0, 0, 0x4, 0x80 })]
        [InlineData("-2309535777355735702993609", new byte[] { 0xc9, 0x7a, 0xcc, 0x15, 0x10, 0xe4, 0x73, 0x2f, 0x10, 0xe9, 0x1, 0, 0, 0, 0, 0x80 })]
        public void TryWriteBytes_for_bytes(string value, byte[] expected)
        {
            byte[] actual = new byte[16];

            Span<byte> span = new Span<byte>(actual);

            Assert.True(decimal.Parse(value).TryWriteBytes(span));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryWriteBytes_for_stackalloc_return_false()
        {
            Span<byte> span = stackalloc byte[15];

            Assert.False(18446744082740936738.269329m.TryWriteBytes(span));
        }

        [Fact]
        public void TryWriteBytes_for_bytes_return_false()
        {
            Span<byte> span = new Span<byte>(new byte[15]);

            Assert.False(18446744082740936738.269329m.TryWriteBytes(span));
        }
#endif
    }
}
