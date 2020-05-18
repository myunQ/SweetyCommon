using System;

using Xunit;

using Sweety.Common.Verification;

namespace Sweety.Common.Tests.Verification
{
    public class TypeVerificationTest
    {
        [Theory]
        [InlineData((byte)1,    true)]
        [InlineData((sbyte)1,   true)]
        [InlineData((short)1,   true)]
        [InlineData((ushort)1,  true)]
        [InlineData((int)1,     true)]
        [InlineData(1u,         true)]
        [InlineData(1L,         true)]
        [InlineData(1ul,        true)]
        [InlineData(StringComparison.CurrentCulture, false)]
        [InlineData(1f,         false)]
        [InlineData(1d,         false)]
        [InlineData("",         false)]
        public void IsIntegerType_BY_obj_object(Object obj, bool expected)
        {
            if (expected)
            {
                Assert.True(obj.IsIntegerType());
            }
            else
            {
                Assert.False(obj.IsIntegerType());
            }
        }


        [Theory]
        [InlineData((byte)1,    true)]
        [InlineData((sbyte)1,   true)]
        [InlineData((short)1,   true)]
        [InlineData((ushort)1,  true)]
        [InlineData((int)1,     true)]
        [InlineData(1u,         true)]
        [InlineData(1L,         true)]
        [InlineData(1ul,        true)]
        [InlineData(StringComparison.CurrentCulture, true)]
        [InlineData(1f,         false)]
        [InlineData(1d,         false)]
        [InlineData("",         false)]
        public void IsIntegerOrEnumType_BY_obj_object(Object obj, bool expected)
        {
            if (expected)
            {
                Assert.True(obj.IsIntegerOrEnumType());
            }
            else
            {
                Assert.False(obj.IsIntegerOrEnumType());
            }
        }


        [Theory]
        [InlineData(typeof(byte),       true)]
        [InlineData(typeof(sbyte),      true)]
        [InlineData(typeof(short),      true)]
        [InlineData(typeof(ushort),     true)]
        [InlineData(typeof(int),        true)]
        [InlineData(typeof(uint),       true)]
        [InlineData(typeof(long),       true)]
        [InlineData(typeof(ulong),      true)]
        [InlineData(typeof(StringComparison), false)]
        [InlineData(typeof(float),      false)]
        [InlineData(typeof(double),     false)]
        [InlineData(typeof(decimal),    false)]
        [InlineData(typeof(string),     false)]
        [InlineData(typeof(char),       false)]
        [InlineData(typeof(DateTime),   false)]
        public void IsIntegerType_BY_type_Type(Type type, bool expected)
        {
            if (expected)
            {
                Assert.True(type.IsIntegerType());
            }
            else
            {
                Assert.False(type.IsIntegerType());
            }
        }


        [Theory]
        [InlineData(typeof(byte),       true)]
        [InlineData(typeof(sbyte),      true)]
        [InlineData(typeof(short),      true)]
        [InlineData(typeof(ushort),     true)]
        [InlineData(typeof(int),        true)]
        [InlineData(typeof(uint),       true)]
        [InlineData(typeof(long),       true)]
        [InlineData(typeof(ulong),      true)]
        [InlineData(typeof(StringComparison), true)]
        [InlineData(typeof(float),      false)]
        [InlineData(typeof(double),     false)]
        [InlineData(typeof(decimal),    false)]
        [InlineData(typeof(string),     false)]
        [InlineData(typeof(char),       false)]
        [InlineData(typeof(DateTime),   false)]
        public void IsIntegerOrEnumType_BY_type_Type(Type type, bool expected)
        {
            if (expected)
            {
                Assert.True(type.IsIntegerOrEnumType());
            }
            else
            {
                Assert.False(type.IsIntegerOrEnumType());
            }
        }
    }
}
