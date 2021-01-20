using System;

using Xunit;

using Sweety.Common.Extensions;

namespace Sweety.Common.Tests.Extensions
{
    public class ByteExtensionsTest
    {
        [Theory]
        [InlineData(new byte[] { 0x91, 0xe8, 0xa, 0x36, 0xff, 0x15, 0x20, 0, 0x40, 0x42, 0xf, 0, 0, 0, 0x6, 0 }, "18446744082740936738.269329")]
        [InlineData(new byte[] { 0x55, 0x3f, 0xf1, 0xb8, 0xaf, 0x23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "39237629067093")]
        [InlineData(new byte[] { 0x80, 0xf2, 0xe2, 0xe5, 0xee, 0x7f, 0xac, 0xce, 0x31, 0xba, 0, 0, 0, 0, 0x4, 0x80 }, "-87927894869209762096.3968")]
        [InlineData(new byte[] { 0xc9, 0x7a, 0xcc, 0x15, 0x10, 0xe4, 0x73, 0x2f, 0x10, 0xe9, 0x1, 0, 0, 0, 0, 0x80 }, "-2309535777355735702993609")]
        public void ToDecimal(byte[] bytes, string expected)
        {
            decimal actual = bytes.ToDecimal();

            Assert.Equal(decimal.Parse(expected), actual);
        }

        [Theory]
        [InlineData(new byte[] { 0, 4, 7, 9, 10, 15 }, "000407090a0f")]
        [InlineData(new byte[] { 13, 26, 98, 138, 176, 201, 233, 255 }, "0d1a628ab0c9e9ff")]
        public void ToHex(byte[] bytes, string expected)
        {
            string actual = bytes.ToHex();

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(new byte[] { 0, 4, 7, 9, 10, 15 }, "000407090A0F")]
        [InlineData(new byte[] { 13, 26, 98, 138, 176, 201, 233, 255 }, "0D1A628AB0C9E9FF")]
        public void ToHEX(byte[] bytes, string expected)
        {
            string actual = bytes.ToHEX();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void FastEquals_BY_a_byteArray_b_byteArray()
        {
            byte[] a = { 13, 26, 98, 138, 176, 201, 233, 255 };
            byte[] b = a;
            Assert.True(a.FastEquals(b));


            b = new byte[] { 13, 26, 98, 138, 176, 201, 233, 255 };
            Assert.True(a.FastEquals(b));


            b = new byte[] { 13, 26, 98, 138, 176, 201, 233, 254 };
            Assert.False(a.FastEquals(b));
        }


        [Fact]
        public void FastEquals_BY_a_ArraySegment_b_ArraySegment()
        {
            byte[] bytes = { 13, 26, 98, 138, 176, 201, 233, 255 };
            ArraySegment<byte> a = new ArraySegment<byte>(bytes);
            ArraySegment<byte> b = new ArraySegment<byte>(bytes);
            Assert.True(a.FastEquals(b));


            a = new ArraySegment<byte>(bytes, 2, 5);
            b = new ArraySegment<byte>(bytes, 2, 5);
            Assert.True(a.FastEquals(b));


            a = new ArraySegment<byte>(bytes, 1, 5);
            b = new ArraySegment<byte>(bytes, 2, 5);
            Assert.False(a.FastEquals(b));


            a = new ArraySegment<byte>(bytes, 1, 3);
            b = new ArraySegment<byte>(bytes, 1, 4);
            Assert.False(a.FastEquals(b));


            a = new ArraySegment<byte>(bytes, 3, 3);
            b = new ArraySegment<byte>(new byte[] { 13, 26, 98, 138, 176, 201, 233, 255 }, 3, 3);
            Assert.True(a.FastEquals(b));


            a = new ArraySegment<byte>(bytes);
            b = new ArraySegment<byte>(new byte[] { 13, 26, 98, 138, 176, 201, 233, 254 });
            Assert.False(a.FastEquals(b));
        }


        [Fact]
        public void SlowEquals_BY_a_byteArray_b_byteArray()
        {
            byte[] a = { 13, 26, 98, 138, 176, 201, 233, 255 };
            byte[] b = a;
            Assert.True(a.SlowEquals(b));


            b = new byte[] { 13, 26, 98, 138, 176, 201, 233, 255 };
            Assert.True(a.SlowEquals(b));


            b = new byte[] { 13, 26, 98, 138, 176, 201, 233, 254 };
            Assert.False(a.SlowEquals(b));
        }


        [Fact]
        public void SlowEquals_BY_a_ArraySegment_b_ArraySegment()
        {
            byte[] bytes = { 13, 26, 98, 138, 176, 201, 233, 255 };
            ArraySegment<byte> a = new ArraySegment<byte>(bytes);
            ArraySegment<byte> b = new ArraySegment<byte>(bytes);
            Assert.True(a.SlowEquals(b));


            a = new ArraySegment<byte>(bytes, 2, 5);
            b = new ArraySegment<byte>(bytes, 2, 5);
            Assert.True(a.SlowEquals(b));


            a = new ArraySegment<byte>(bytes, 1, 5);
            b = new ArraySegment<byte>(bytes, 2, 5);
            Assert.False(a.SlowEquals(b));


            a = new ArraySegment<byte>(bytes, 1, 3);
            b = new ArraySegment<byte>(bytes, 1, 4);
            Assert.False(a.SlowEquals(b));


            a = new ArraySegment<byte>(bytes, 3, 3);
            b = new ArraySegment<byte>(new byte[] { 13, 26, 98, 138, 176, 201, 233, 255 }, 3, 3);
            Assert.True(a.SlowEquals(b));


            a = new ArraySegment<byte>(bytes);
            b = new ArraySegment<byte>(new byte[] { 13, 26, 98, 138, 176, 201, 233, 254 });
            Assert.False(a.SlowEquals(b));
        }

        [Fact]
        public void SlowEquals_BY_a_Span_b_ReadOnlySpan()
        {
            Span<byte> a = stackalloc byte[5];
            Span<byte> b = stackalloc byte[5];

            Assert.True(a.SlowEquals(b));

            a[3] = 35;
            Assert.False(a.SlowEquals(b));

            b[3] = 35;
            Assert.True(a.SlowEquals(b));
        }

        [Fact]
        public void SlowEquals_BY_a_ReadOnlySpan_b_ReadOnlySpan()
        {
            Span<byte> a = stackalloc byte[5];
            Span<byte> b = stackalloc byte[5];

            Assert.True(((ReadOnlySpan<byte>)a).SlowEquals(b));

            a[3] = 35;
            Assert.False(((ReadOnlySpan<byte>)a).SlowEquals(b));

            b[3] = 35;
            Assert.True(((ReadOnlySpan<byte>)a).SlowEquals(b));
        }
    }
}
