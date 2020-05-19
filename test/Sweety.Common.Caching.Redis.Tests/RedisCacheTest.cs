using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Xunit;
using Xunit.Extensions.Ordering;

using Sweety.Common.Caching;

[assembly:TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]

namespace Sweety.Common.Caching.Redis.Tests
{

    public class RedisCacheTest : IDisposable
    {
        const string BOOL_KEY = "bool-value-key";
        const string BYTE_KEY = "byte-value-key";
        const string SBYTE_KEY = "sbyte-value-key";
        const string CHAR_KEY = "char-value-key";
        const string INT16_KEY = "int16-value-key";
        const string UINT16_KEY = "uint16-value-key";
        const string INT32_KEY = "int32-value-key";
        const string UINT32_KEY = "uint32-value-key";
        const string INT64_KEY = "int64-value-key";
        const string UINT64_KEY = "uint64-value-key";
        const string DECIMAL_KEY = "decimal-value-key";
        const string SINGLE_KEY = "single-value-key";
        const string DOUBLE_KEY = "double-value-key";
        const string STRING_KEY = "string-value-key";
        const string BYTE_ARRAY_KEY = "byteArray-value-key";
        const string ARRAY_SEGMENT_BYTE_KEY = "arraySegment_byte-value-key";
        const string MEMORY_BYTE_KEY = "memory_byte-value-key";
        const string READONLYMEMORY_BYTE_KEY = "readOnlyMemory_byte-value-key";
        const string DATETIME_KEY = "datetime-value-key";
        const string DATETIMEOFFSET_KEY = "datetimeoffset-value-key";
        const string TIMESPAN_KEY = "timespan-value-key";
        const string GUID_KEY = "guid-value-key";
        const string IP_V4_KEY = "ipv4-value-key";
        const string IP_V6_KEY = "ipv6-value-key";
        const string OBJECT_KEY = "object-value-key";

        const bool BOOL_VALUE = true;
        const byte BYTE_VALUE = 198;
        const sbyte SBYTE_VALUE = -118;
        const char CHAR_VALUE = '\u29F3';
        const short INT16_VALUE = -29383;
        const ushort UINT16_VALUE = 65046;
        const int INT32_VALUE = -393238623;
        const uint UINT32_VALUE = 3709380392;
        const long INT64_VALUE = -892375223752938L;
        const ulong UINT64_VALUE = 3092628937620937029UL;
        const decimal DECIMAL_VALUE = 9037289037928379816984637209.329086239m;
        const float SINGLE_VALUE = 3297.62f;
        const double DOUBLE_VALUE = 39827387.93364d;
        const string STRING_VALUE = "This is \"Sweety.Common.Caching.Redis\" library of unit test.\r\n测试字符串缓存项。\n🤮(～￣(OO)￣)ブ❀";
        static readonly byte[] BYTE_ARRAY_VALUE = { 12, 3, 62, 124, 33, 97, 58 };
        static readonly ArraySegment<byte> ARRAY_SEGMENT_BYTE_VALUE = new ArraySegment<byte>(BYTE_ARRAY_VALUE, 1, 4);
        static readonly Memory<byte> MEMORY_BYTE_VALUE = new Memory<byte>(BYTE_ARRAY_VALUE, 2, 5);
        static readonly ReadOnlyMemory<byte> READONLYMEMORY_BYTE_VALUE = new ReadOnlyMemory<byte>(BYTE_ARRAY_VALUE, 2, 5);
        static readonly DateTime DATETIME_VALUE = DateTime.Now;
        static readonly DateTimeOffset DATETIMEOFFSET_VALUE = DateTimeOffset.Now;
        static readonly TimeSpan TIMESPAN_VALUE = TimeSpan.FromMilliseconds(2395823968.235732d);
        static readonly Guid GUID_VALUE = Guid.NewGuid();
        static readonly IPAddress IP_V4_VALUE = IPAddress.Parse("192.168.1.1");
        static readonly IPAddress IP_V6_VALUE = IPAddress.Parse("D9:E2::33:9F:D7");
        static readonly IReadOnlyList<CacheObjectModel> OBJECT_VALUE;

        RedisCache _cache;

        static RedisCacheTest()
        {
            OBJECT_VALUE = new List<CacheObjectModel>
            {
                new CacheObjectModel
                {
                    ID = "0001",
                    Name = "国内",
                    Children = new List<CacheObjectModel>
                    {
                        new CacheObjectModel
                        {
                            ID = "00010001",
                            Name = "经济",
                            Children = new List<CacheObjectModel>
                            {
                                new CacheObjectModel
                                {
                                    ID = "000100010001",
                                    Name = "宏观"
                                },
                                new CacheObjectModel
                                {
                                    ID = "000100010002",
                                    Name = "微观"
                                }
                            }
                        },
                        new CacheObjectModel
                        {
                            ID = "00010002",
                            Name = "政治"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010003",
                            Name = "民生"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010004",
                            Name = "社会"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010005",
                            Name = "环境"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010006",
                            Name = "其他"
                        }
                    }
                },
                new CacheObjectModel
                {
                    ID = "0002",
                    Name = "国际"
                },
                new CacheObjectModel
                {
                    ID = "0003",
                    Name = "数读"
                },
                new CacheObjectModel
                {
                    ID = "0004",
                    Name = "军事"
                },
                new CacheObjectModel
                {
                    ID = "0005",
                    Name = "航空"
                }

            }.AsReadOnly();
        }

        public RedisCacheTest()
        {
            _cache = new RedisCache("127.0.0.1:6379");
        }




        [Fact, Order(0)]
        //[Fact]
        public void Add_bool()
        {
            Assert.True(_cache.Add(BOOL_KEY, BOOL_VALUE));
            Assert.False(_cache.Add(BOOL_KEY, BOOL_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(1)]
        //[Fact]
        public void Add_byte()
        {
            Assert.True(_cache.Add(BYTE_KEY, BYTE_VALUE));
            Assert.False(_cache.Add(BYTE_KEY, BYTE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(2)]
        //[Fact]
        public void Add_sbyte()
        {
            Assert.True(_cache.Add(SBYTE_KEY, SBYTE_VALUE));
            Assert.False(_cache.Add(SBYTE_KEY, SBYTE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(3)]
        //[Fact]
        public void Add_char()
        {
            Assert.True(_cache.Add(CHAR_KEY, CHAR_VALUE));
            Assert.False(_cache.Add(CHAR_KEY, CHAR_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(4)]
        //[Fact]
        public void Add_int16()
        {
            Assert.True(_cache.Add(INT16_KEY, INT16_VALUE));
            Assert.False(_cache.Add(INT16_KEY, INT16_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(5)]
        //[Fact]
        public void Add_uint16()
        {
            Assert.True(_cache.Add(UINT16_KEY, UINT16_VALUE));
            Assert.False(_cache.Add(UINT16_KEY, UINT16_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(6)]
        //[Fact]
        public void Add_int32()
        {
            Assert.True(_cache.Add(INT32_KEY, INT32_VALUE));
            Assert.False(_cache.Add(INT32_KEY, INT32_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(7)]
        //[Fact]
        public void Add_uint32()
        {
            Assert.True(_cache.Add(UINT32_KEY, UINT32_VALUE));
            Assert.False(_cache.Add(UINT32_KEY, UINT32_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(8)]
        //[Fact]
        public void Add_int64()
        {
            Assert.True(_cache.Add(INT64_KEY, INT64_VALUE));
            Assert.False(_cache.Add(INT64_KEY, INT64_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(9)]
        //[Fact]
        public void Add_uint64()
        {
            Assert.True(_cache.Add(UINT64_KEY, UINT64_VALUE));
            Assert.False(_cache.Add(UINT64_KEY, UINT64_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(10)]
        //[Fact]
        public void Add_decimal()
        {
            Assert.True(_cache.Add(DECIMAL_KEY, DECIMAL_VALUE));
            Assert.False(_cache.Add(DECIMAL_KEY, DECIMAL_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(11)]
        //[Fact]
        public void Add_float()
        {
            Assert.True(_cache.Add(SINGLE_KEY, SINGLE_VALUE));
            Assert.False(_cache.Add(SINGLE_KEY, SINGLE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(12)]
        //[Fact]
        public void Add_double()
        {
            Assert.True(_cache.Add(DOUBLE_KEY, DOUBLE_VALUE));
            Assert.False(_cache.Add(DOUBLE_KEY, DOUBLE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(13)]
        //[Fact]
        public void Add_string()
        {
            Assert.True(_cache.Add(STRING_KEY, STRING_VALUE));
            Assert.False(_cache.Add(STRING_KEY, STRING_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(14)]
        //[Fact]
        public void Add_byteArray()
        {
            Assert.True(_cache.Add(BYTE_ARRAY_KEY, BYTE_ARRAY_VALUE));
            Assert.False(_cache.Add(BYTE_ARRAY_KEY, BYTE_ARRAY_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(15)]
        //[Fact]
        public void Add_arraySegment_byte()
        {
            Assert.True(_cache.Add(ARRAY_SEGMENT_BYTE_KEY, ARRAY_SEGMENT_BYTE_VALUE));
            Assert.False(_cache.Add(ARRAY_SEGMENT_BYTE_KEY, ARRAY_SEGMENT_BYTE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(16)]
        //[Fact]
        public void Add_memory_byte()
        {
            Assert.True(_cache.Add(MEMORY_BYTE_KEY, MEMORY_BYTE_VALUE));
            Assert.False(_cache.Add(MEMORY_BYTE_KEY, MEMORY_BYTE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(17)]
        //[Fact]
        public void Add_readOnlyMemory_byte()
        {
            Assert.True(_cache.Add(READONLYMEMORY_BYTE_KEY, READONLYMEMORY_BYTE_VALUE));
            Assert.False(_cache.Add(READONLYMEMORY_BYTE_KEY, READONLYMEMORY_BYTE_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(18)]
        //[Fact]
        public void Add_dateTime()
        {
            Assert.True(_cache.Add(DATETIME_KEY, DATETIME_VALUE));
            Assert.False(_cache.Add(DATETIME_KEY, DATETIME_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(19)]
        //[Fact]
        public void Add_dateTimeOffset()
        {
            Assert.True(_cache.Add(DATETIMEOFFSET_KEY, DATETIMEOFFSET_VALUE));
            Assert.False(_cache.Add(DATETIMEOFFSET_KEY, DATETIMEOFFSET_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(20)]
        //[Fact]
        public void Add_timeSpan()
        {
            Assert.True(_cache.Add(TIMESPAN_KEY, TIMESPAN_VALUE));
            Assert.False(_cache.Add(TIMESPAN_KEY, TIMESPAN_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(21)]
        //[Fact]
        public void Add_guid()
        {
            Assert.True(_cache.Add(GUID_KEY, GUID_VALUE));
            Assert.False(_cache.Add(GUID_KEY, GUID_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(22)]
        //[Fact]
        public void Add_ipv4()
        {
            Assert.True(_cache.Add(IP_V4_KEY, IP_V4_VALUE));
            Assert.False(_cache.Add(IP_V4_KEY, IP_V4_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(23)]
        //[Fact]
        public void Add_ipv6()
        {
            Assert.True(_cache.Add(IP_V6_KEY, IP_V6_VALUE));
            Assert.False(_cache.Add(IP_V6_KEY, IP_V6_VALUE));
        }

        [Fact, Order(0)]
        //[Fact, Order(24)]
        //[Fact]
        public void Add_object()
        {
            Assert.True(_cache.Add(OBJECT_KEY, OBJECT_VALUE));
            Assert.False(_cache.Add(OBJECT_KEY, OBJECT_VALUE));
        }




        [Fact, Order(1)]
        //[Fact, Order(25)]
        //[Fact]
        public void Get_bool()
        {
            Assert.Equal(BOOL_VALUE, _cache.Get<bool>(BOOL_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(26)]
        //[Fact]
        public void Get_byte()
        {
            Assert.Equal(BYTE_VALUE, _cache.Get<byte>(BYTE_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(27)]
        //[Fact]
        public void Get_sbyte()
        {
            Assert.Equal(SBYTE_VALUE, _cache.Get<sbyte>(SBYTE_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(28)]
        //[Fact]
        public void Get_char()
        {
            Assert.Equal(CHAR_VALUE, _cache.Get<char>(CHAR_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(29)]
        //[Fact]
        public void Get_int16()
        {
            Assert.Equal(INT16_VALUE, _cache.Get<short>(INT16_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(30)]
        //[Fact]
        public void Get_uint16()
        {
            Assert.Equal(UINT16_VALUE, _cache.Get<ushort>(UINT16_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(31)]
        //[Fact]
        public void Get_int32()
        {
            Assert.Equal(INT32_VALUE, _cache.Get<int>(INT32_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(32)]
        //[Fact]
        public void Get_uint32()
        {
            Assert.Equal(UINT32_VALUE, _cache.Get<uint>(UINT32_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(33)]
        //[Fact]
        public void Get_int64()
        {
            Assert.Equal(INT64_VALUE, _cache.Get<long>(INT64_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(34)]
        //[Fact]
        public void Get_uint64()
        {
            Assert.Equal(UINT64_VALUE, _cache.Get<ulong>(UINT64_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(35)]
        //[Fact]
        public void Get_decimal()
        {
            Assert.Equal(DECIMAL_VALUE, _cache.Get<decimal>(DECIMAL_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(36)]
        //[Fact]
        public void Get_float()
        {
            Assert.Equal(SINGLE_VALUE, _cache.Get<float>(SINGLE_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(37)]
        //[Fact]
        public void Get_double()
        {
            Assert.Equal(DOUBLE_VALUE, _cache.Get<double>(DOUBLE_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(38)]
        //[Fact]
        public void Get_string()
        {
            Assert.Equal(STRING_VALUE, _cache.Get<string>(STRING_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(39)]
        //[Fact]
        public void Get_byteArray()
        {
            Assert.Equal(BYTE_ARRAY_VALUE, _cache.Get<byte[]>(BYTE_ARRAY_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(40)]
        //[Fact]
        public void Get_arraySegment_byte()
        {
            var expected = ARRAY_SEGMENT_BYTE_VALUE;
            var actual = new ArraySegment<byte>(_cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY));

            ArraySegmentEquals(expected, actual);
        }

        [Fact, Order(1)]
        //[Fact, Order(41)]
        //[Fact]
        public void Get_memory_byte()
        {
            var expected = MEMORY_BYTE_VALUE;
            var actual = new Memory<byte>(_cache.Get<byte[]>(MEMORY_BYTE_KEY));

            ReadOnlyMemoryEquals(expected, actual);
        }

        [Fact, Order(1)]
        //[Fact, Order(42)]
        //[Fact]
        public void Get_readOnlyMemory_byte()
        {
            var expected = READONLYMEMORY_BYTE_VALUE;
            var actual = new ReadOnlyMemory<byte>(_cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY));

            ReadOnlyMemoryEquals(expected, actual);
        }

        [Fact, Order(1)]
        //[Fact, Order(43)]
        //[Fact]
        public void Get_dateTime()
        {
            Assert.Equal(DATETIME_VALUE, _cache.Get<DateTime>(DATETIME_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(44)]
        //[Fact]
        public void Get_dateTimeOffset()
        {
            Assert.Equal(DATETIMEOFFSET_VALUE, _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(45)]
        //[Fact]
        public void Get_timeSpan()
        {
            Assert.Equal(TIMESPAN_VALUE, _cache.Get<TimeSpan>(TIMESPAN_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(46)]
        //[Fact]
        public void Get_guid()
        {
            Assert.Equal(GUID_VALUE, _cache.Get<Guid>(GUID_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(47)]
        //[Fact]
        public void Get_ipv4()
        {
            Assert.Equal(IP_V4_VALUE, _cache.Get<IPAddress>(IP_V4_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(48)]
        //[Fact]
        public void Get_ipv6()
        {
            Assert.Equal(IP_V6_VALUE, _cache.Get<IPAddress>(IP_V6_KEY));
        }

        [Fact, Order(1)]
        //[Fact, Order(49)]
        //[Fact]
        public void Get_object()
        {
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
        }



        [Fact, Order(2)]
        //[Fact]
        public void Remove()
        {
            _cache.Remove(BOOL_KEY);
        }

        [Fact, Order(2)]
        //[Fact]
        public void Removes()
        {
            _cache.Remove(BOOL_KEY,
                BYTE_KEY,
                SBYTE_KEY,
                CHAR_KEY,
                INT16_KEY,
                UINT16_KEY,
                INT32_KEY,
                UINT32_KEY,
                INT64_KEY,
                UINT64_KEY,
                DECIMAL_KEY,
                SINGLE_KEY,
                DOUBLE_KEY,
                STRING_KEY,
                BYTE_ARRAY_KEY,
                ARRAY_SEGMENT_BYTE_KEY,
                MEMORY_BYTE_KEY,
                READONLYMEMORY_BYTE_KEY,
                DATETIME_KEY,
                DATETIMEOFFSET_KEY,
                TIMESPAN_KEY,
                GUID_KEY,
                IP_V4_KEY,
                IP_V6_KEY,
                OBJECT_KEY);
        }



        [Fact, Order(3)]
        //[Fact]
        public void Add_bool_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(BOOL_KEY, BOOL_VALUE, expiration));
            Assert.False(_cache.Add(BOOL_KEY, BOOL_VALUE, expiration));

            Assert.Equal(BOOL_VALUE, _cache.Get<bool>(BOOL_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(BOOL_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(BOOL_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(1)]
        //[Fact]
        public void Add_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(BYTE_KEY, BYTE_VALUE, expiration));
            Assert.False(_cache.Add(BYTE_KEY, BYTE_VALUE, expiration));

            Assert.Equal(BYTE_VALUE, _cache.Get<byte>(BYTE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(BYTE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(BYTE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(2)]
        //[Fact]
        public void Add_sbyte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(SBYTE_KEY, SBYTE_VALUE, expiration));
            Assert.False(_cache.Add(SBYTE_KEY, SBYTE_VALUE, expiration));

            Assert.Equal(SBYTE_VALUE, _cache.Get<sbyte>(SBYTE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(SBYTE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(SBYTE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(3)]
        //[Fact]
        public void Add_char_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(CHAR_KEY, CHAR_VALUE, expiration));
            Assert.False(_cache.Add(CHAR_KEY, CHAR_VALUE, expiration));

            Assert.Equal(CHAR_VALUE, _cache.Get<char>(CHAR_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(CHAR_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(CHAR_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(4)]
        //[Fact]
        public void Add_int16_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(INT16_KEY, INT16_VALUE, expiration));
            Assert.False(_cache.Add(INT16_KEY, INT16_VALUE, expiration));

            Assert.Equal(INT16_VALUE, _cache.Get<short>(INT16_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(INT16_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(INT16_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(5)]
        //[Fact]
        public void Add_uint16_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(UINT16_KEY, UINT16_VALUE, expiration));
            Assert.False(_cache.Add(UINT16_KEY, UINT16_VALUE, expiration));

            Assert.Equal(UINT16_VALUE, _cache.Get<ushort>(UINT16_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(UINT16_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(UINT16_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(6)]
        //[Fact]
        public void Add_int32_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(INT32_KEY, INT32_VALUE, expiration));
            Assert.False(_cache.Add(INT32_KEY, INT32_VALUE, expiration));

            Assert.Equal(INT32_VALUE, _cache.Get<int>(INT32_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(INT32_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(INT32_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(7)]
        //[Fact]
        public void Add_uint32_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(UINT32_KEY, UINT32_VALUE, expiration));
            Assert.False(_cache.Add(UINT32_KEY, UINT32_VALUE, expiration));

            Assert.Equal(UINT32_VALUE, _cache.Get<uint>(UINT32_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(UINT32_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(UINT32_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(8)]
        //[Fact]
        public void Add_int64_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(INT64_KEY, INT64_VALUE, expiration));
            Assert.False(_cache.Add(INT64_KEY, INT64_VALUE, expiration));

            Assert.Equal(INT64_VALUE, _cache.Get<long>(INT64_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(INT64_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(INT64_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(9)]
        //[Fact]
        public void Add_uint64_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(UINT64_KEY, UINT64_VALUE, expiration));
            Assert.False(_cache.Add(UINT64_KEY, UINT64_VALUE, expiration));

            Assert.Equal(UINT64_VALUE, _cache.Get<ulong>(UINT64_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(UINT64_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(UINT64_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(10)]
        //[Fact]
        public void Add_decimal_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(DECIMAL_KEY, DECIMAL_VALUE, expiration));
            Assert.False(_cache.Add(DECIMAL_KEY, DECIMAL_VALUE, expiration));

            Assert.Equal(DECIMAL_VALUE, _cache.Get<decimal>(DECIMAL_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(DECIMAL_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DECIMAL_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(11)]
        //[Fact]
        public void Add_float_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(SINGLE_KEY, SINGLE_VALUE, expiration));
            Assert.False(_cache.Add(SINGLE_KEY, SINGLE_VALUE, expiration));

            Assert.Equal(SINGLE_VALUE, _cache.Get<float>(SINGLE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(SINGLE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(SINGLE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(12)]
        //[Fact]
        public void Add_double_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(DOUBLE_KEY, DOUBLE_VALUE, expiration));
            Assert.False(_cache.Add(DOUBLE_KEY, DOUBLE_VALUE, expiration));

            Assert.Equal(DOUBLE_VALUE, _cache.Get<double>(DOUBLE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(DOUBLE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DOUBLE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(13)]
        //[Fact]
        public void Add_string_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(STRING_KEY, STRING_VALUE, expiration));
            Assert.False(_cache.Add(STRING_KEY, STRING_VALUE, expiration));

            Assert.Equal(STRING_VALUE, _cache.Get<string>(STRING_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(STRING_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(STRING_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(14)]
        //[Fact]
        public void Add_byteArray_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(BYTE_ARRAY_KEY, BYTE_ARRAY_VALUE, expiration));
            Assert.False(_cache.Add(BYTE_ARRAY_KEY, BYTE_ARRAY_VALUE, expiration));

            Assert.Equal(BYTE_ARRAY_VALUE, _cache.Get<byte[]>(BYTE_ARRAY_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(BYTE_ARRAY_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(BYTE_ARRAY_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(15)]
        //[Fact]
        public void Add_arraySegment_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(ARRAY_SEGMENT_BYTE_KEY, ARRAY_SEGMENT_BYTE_VALUE, expiration));
            Assert.False(_cache.Add(ARRAY_SEGMENT_BYTE_KEY, ARRAY_SEGMENT_BYTE_VALUE, expiration));

            ArraySegmentEquals(ARRAY_SEGMENT_BYTE_VALUE, _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(ARRAY_SEGMENT_BYTE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(ARRAY_SEGMENT_BYTE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(16)]
        //[Fact]
        public void Add_memory_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(MEMORY_BYTE_KEY, MEMORY_BYTE_VALUE, expiration));
            Assert.False(_cache.Add(MEMORY_BYTE_KEY, MEMORY_BYTE_VALUE, expiration));

            ReadOnlyMemoryEquals(MEMORY_BYTE_VALUE, _cache.Get<byte[]>(MEMORY_BYTE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(MEMORY_BYTE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(MEMORY_BYTE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(17)]
        //[Fact]
        public void Add_readOnlyMemory_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(READONLYMEMORY_BYTE_KEY, READONLYMEMORY_BYTE_VALUE, expiration));
            Assert.False(_cache.Add(READONLYMEMORY_BYTE_KEY, READONLYMEMORY_BYTE_VALUE, expiration));

            ReadOnlyMemoryEquals(READONLYMEMORY_BYTE_VALUE, _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(READONLYMEMORY_BYTE_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(READONLYMEMORY_BYTE_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(18)]
        //[Fact]
        public void Add_dateTime_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(DATETIME_KEY, DATETIME_VALUE, expiration));
            Assert.False(_cache.Add(DATETIME_KEY, DATETIME_VALUE, expiration));

            Assert.Equal(DATETIME_VALUE, _cache.Get<DateTime>(DATETIME_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(DATETIME_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DATETIME_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(19)]
        //[Fact]
        public void Add_dateTimeOffset_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(DATETIMEOFFSET_KEY, DATETIMEOFFSET_VALUE, expiration));
            Assert.False(_cache.Add(DATETIMEOFFSET_KEY, DATETIMEOFFSET_VALUE, expiration));

            Assert.Equal(DATETIMEOFFSET_VALUE, _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(DATETIMEOFFSET_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DATETIMEOFFSET_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(20)]
        //[Fact]
        public void Add_timeSpan_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(TIMESPAN_KEY, TIMESPAN_VALUE, expiration));
            Assert.False(_cache.Add(TIMESPAN_KEY, TIMESPAN_VALUE, expiration));

            Assert.Equal(TIMESPAN_VALUE, _cache.Get<TimeSpan>(TIMESPAN_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(TIMESPAN_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(TIMESPAN_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(21)]
        //[Fact]
        public void Add_guid_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(GUID_KEY, GUID_VALUE, expiration));
            Assert.False(_cache.Add(GUID_KEY, GUID_VALUE, expiration));

            Assert.Equal(GUID_VALUE, _cache.Get<Guid>(GUID_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(GUID_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(GUID_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(22)]
        //[Fact]
        public void Add_ipv4_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(IP_V4_KEY, IP_V4_VALUE, expiration));
            Assert.False(_cache.Add(IP_V4_KEY, IP_V4_VALUE, expiration));

            Assert.Equal(IP_V4_VALUE, _cache.Get<IPAddress>(IP_V4_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(IP_V4_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(IP_V4_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(23)]
        //[Fact]
        public void Add_ipv6_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(IP_V6_KEY, IP_V6_VALUE, expiration));
            Assert.False(_cache.Add(IP_V6_KEY, IP_V6_VALUE, expiration));

            Assert.Equal(IP_V6_VALUE, _cache.Get<IPAddress>(IP_V6_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(IP_V6_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(IP_V6_KEY));
        }

        [Fact, Order(3)]
        //[Fact, Order(24)]
        //[Fact]
        public void Add_object_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Assert.True(_cache.Add(OBJECT_KEY, OBJECT_VALUE, expiration));
            Assert.False(_cache.Add(OBJECT_KEY, OBJECT_VALUE, expiration));

            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(110).Wait();
            Assert.True(_cache.Contains(OBJECT_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(OBJECT_KEY));
        }




        [Fact, Order(4)]
        //[Fact]
        public void Add_bool_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(BOOL_KEY, BOOL_VALUE, expiration));
            Assert.False(_cache.Add(BOOL_KEY, BOOL_VALUE, expiration));

            Assert.Equal(BOOL_VALUE, _cache.Get<bool>(BOOL_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(BOOL_VALUE, _cache.Get<bool>(BOOL_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(BOOL_VALUE, _cache.Get<bool>(BOOL_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(BOOL_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(1)]
        //[Fact]
        public void Add_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(BYTE_KEY, BYTE_VALUE, expiration));
            Assert.False(_cache.Add(BYTE_KEY, BYTE_VALUE, expiration));

            Assert.Equal(BYTE_VALUE, _cache.Get<byte>(BYTE_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(BYTE_VALUE, _cache.Get<byte>(BYTE_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(BYTE_VALUE, _cache.Get<byte>(BYTE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(BYTE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(2)]
        //[Fact]
        public void Add_sbyte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(SBYTE_KEY, SBYTE_VALUE, expiration));
            Assert.False(_cache.Add(SBYTE_KEY, SBYTE_VALUE, expiration));

            Assert.Equal(SBYTE_VALUE, _cache.Get<sbyte>(SBYTE_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(SBYTE_VALUE, _cache.Get<sbyte>(SBYTE_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(SBYTE_VALUE, _cache.Get<sbyte>(SBYTE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(SBYTE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(4)]
        //[Fact]
        public void Add_char_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(CHAR_KEY, CHAR_VALUE, expiration));
            Assert.False(_cache.Add(CHAR_KEY, CHAR_VALUE, expiration));

            Assert.Equal(CHAR_VALUE, _cache.Get<char>(CHAR_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(CHAR_VALUE, _cache.Get<char>(CHAR_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(CHAR_VALUE, _cache.Get<char>(CHAR_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(CHAR_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(4)]
        //[Fact]
        public void Add_int16_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(INT16_KEY, INT16_VALUE, expiration));
            Assert.False(_cache.Add(INT16_KEY, INT16_VALUE, expiration));

            Assert.Equal(INT16_VALUE, _cache.Get<short>(INT16_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(INT16_VALUE, _cache.Get<short>(INT16_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(INT16_VALUE, _cache.Get<short>(INT16_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(INT16_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(5)]
        //[Fact]
        public void Add_uint16_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(UINT16_KEY, UINT16_VALUE, expiration));
            Assert.False(_cache.Add(UINT16_KEY, UINT16_VALUE, expiration));

            Assert.Equal(UINT16_VALUE, _cache.Get<ushort>(UINT16_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(UINT16_VALUE, _cache.Get<ushort>(UINT16_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(UINT16_VALUE, _cache.Get<ushort>(UINT16_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(UINT16_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(6)]
        //[Fact]
        public void Add_int32_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(INT32_KEY, INT32_VALUE, expiration));
            Assert.False(_cache.Add(INT32_KEY, INT32_VALUE, expiration));

            Assert.Equal(INT32_VALUE, _cache.Get<int>(INT32_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(INT32_VALUE, _cache.Get<int>(INT32_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(INT32_VALUE, _cache.Get<int>(INT32_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(INT32_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(7)]
        //[Fact]
        public void Add_uint32_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(UINT32_KEY, UINT32_VALUE, expiration));
            Assert.False(_cache.Add(UINT32_KEY, UINT32_VALUE, expiration));

            Assert.Equal(UINT32_VALUE, _cache.Get<uint>(UINT32_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(UINT32_VALUE, _cache.Get<uint>(UINT32_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(UINT32_VALUE, _cache.Get<uint>(UINT32_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(UINT32_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(8)]
        //[Fact]
        public void Add_int64_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(INT64_KEY, INT64_VALUE, expiration));
            Assert.False(_cache.Add(INT64_KEY, INT64_VALUE, expiration));

            Assert.Equal(INT64_VALUE, _cache.Get<long>(INT64_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(INT64_VALUE, _cache.Get<long>(INT64_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(INT64_VALUE, _cache.Get<long>(INT64_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(INT64_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(9)]
        //[Fact]
        public void Add_uint64_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(UINT64_KEY, UINT64_VALUE, expiration));
            Assert.False(_cache.Add(UINT64_KEY, UINT64_VALUE, expiration));

            Assert.Equal(UINT64_VALUE, _cache.Get<ulong>(UINT64_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(UINT64_VALUE, _cache.Get<ulong>(UINT64_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(UINT64_VALUE, _cache.Get<ulong>(UINT64_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(UINT64_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(10)]
        //[Fact]
        public void Add_decimal_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(DECIMAL_KEY, DECIMAL_VALUE, expiration));
            Assert.False(_cache.Add(DECIMAL_KEY, DECIMAL_VALUE, expiration));

            Assert.Equal(DECIMAL_VALUE, _cache.Get<decimal>(DECIMAL_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(DECIMAL_VALUE, _cache.Get<decimal>(DECIMAL_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(DECIMAL_VALUE, _cache.Get<decimal>(DECIMAL_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(DECIMAL_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(11)]
        //[Fact]
        public void Add_float_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(SINGLE_KEY, SINGLE_VALUE, expiration));
            Assert.False(_cache.Add(SINGLE_KEY, SINGLE_VALUE, expiration));

            Assert.Equal(SINGLE_VALUE, _cache.Get<float>(SINGLE_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(SINGLE_VALUE, _cache.Get<float>(SINGLE_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(SINGLE_VALUE, _cache.Get<float>(SINGLE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(SINGLE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(12)]
        //[Fact]
        public void Add_double_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(DOUBLE_KEY, DOUBLE_VALUE, expiration));
            Assert.False(_cache.Add(DOUBLE_KEY, DOUBLE_VALUE, expiration));

            Assert.Equal(DOUBLE_VALUE, _cache.Get<double>(DOUBLE_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(DOUBLE_VALUE, _cache.Get<double>(DOUBLE_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(DOUBLE_VALUE, _cache.Get<double>(DOUBLE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(DOUBLE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(13)]
        //[Fact]
        public void Add_string_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(STRING_KEY, STRING_VALUE, expiration));
            Assert.False(_cache.Add(STRING_KEY, STRING_VALUE, expiration));

            Assert.Equal(STRING_VALUE, _cache.Get<string>(STRING_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(STRING_VALUE, _cache.Get<string>(STRING_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(STRING_VALUE, _cache.Get<string>(STRING_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(STRING_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(14)]
        //[Fact]
        public void Add_byteArray_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(BYTE_ARRAY_KEY, BYTE_ARRAY_VALUE, expiration));
            Assert.False(_cache.Add(BYTE_ARRAY_KEY, BYTE_ARRAY_VALUE, expiration));

            Assert.Equal(BYTE_ARRAY_VALUE, _cache.Get<byte[]>(BYTE_ARRAY_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(BYTE_ARRAY_VALUE, _cache.Get<byte[]>(BYTE_ARRAY_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(BYTE_ARRAY_VALUE, _cache.Get<byte[]>(BYTE_ARRAY_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(BYTE_ARRAY_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(15)]
        //[Fact]
        public void Add_arraySegment_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(ARRAY_SEGMENT_BYTE_KEY, ARRAY_SEGMENT_BYTE_VALUE, expiration));
            Assert.False(_cache.Add(ARRAY_SEGMENT_BYTE_KEY, ARRAY_SEGMENT_BYTE_VALUE, expiration));

            ArraySegmentEquals(ARRAY_SEGMENT_BYTE_VALUE, _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY));
            Task.Delay(30).Wait();
            ArraySegmentEquals(ARRAY_SEGMENT_BYTE_VALUE, _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY));
            Task.Delay(40).Wait();
            ArraySegmentEquals(ARRAY_SEGMENT_BYTE_VALUE, _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(ARRAY_SEGMENT_BYTE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(16)]
        //[Fact]
        public void Add_memory_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(MEMORY_BYTE_KEY, MEMORY_BYTE_VALUE, expiration));
            Assert.False(_cache.Add(MEMORY_BYTE_KEY, MEMORY_BYTE_VALUE, expiration));

            ReadOnlyMemoryEquals(MEMORY_BYTE_VALUE, _cache.Get<byte[]>(MEMORY_BYTE_KEY));
            Task.Delay(30).Wait();
            ReadOnlyMemoryEquals(MEMORY_BYTE_VALUE, _cache.Get<byte[]>(MEMORY_BYTE_KEY));
            Task.Delay(40).Wait();
            ReadOnlyMemoryEquals(MEMORY_BYTE_VALUE, _cache.Get<byte[]>(MEMORY_BYTE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(MEMORY_BYTE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(17)]
        //[Fact]
        public void Add_readOnlyMemory_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(READONLYMEMORY_BYTE_KEY, READONLYMEMORY_BYTE_VALUE, expiration));
            Assert.False(_cache.Add(READONLYMEMORY_BYTE_KEY, READONLYMEMORY_BYTE_VALUE, expiration));

            ReadOnlyMemoryEquals(READONLYMEMORY_BYTE_VALUE, _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY));
            Task.Delay(30).Wait();
            ReadOnlyMemoryEquals(READONLYMEMORY_BYTE_VALUE, _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY));
            Task.Delay(40).Wait();
            ReadOnlyMemoryEquals(READONLYMEMORY_BYTE_VALUE, _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(READONLYMEMORY_BYTE_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(18)]
        //[Fact]
        public void Add_dateTime_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(DATETIME_KEY, DATETIME_VALUE, expiration));
            Assert.False(_cache.Add(DATETIME_KEY, DATETIME_VALUE, expiration));

            Assert.Equal(DATETIME_VALUE, _cache.Get<DateTime>(DATETIME_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(DATETIME_VALUE, _cache.Get<DateTime>(DATETIME_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(DATETIME_VALUE, _cache.Get<DateTime>(DATETIME_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(DATETIME_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(19)]
        //[Fact]
        public void Add_dateTimeOffset_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(DATETIMEOFFSET_KEY, DATETIMEOFFSET_VALUE, expiration));
            Assert.False(_cache.Add(DATETIMEOFFSET_KEY, DATETIMEOFFSET_VALUE, expiration));

            Assert.Equal(DATETIMEOFFSET_VALUE, _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(DATETIMEOFFSET_VALUE, _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(DATETIMEOFFSET_VALUE, _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(DATETIMEOFFSET_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(20)]
        //[Fact]
        public void Add_timeSpan_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(TIMESPAN_KEY, TIMESPAN_VALUE, expiration));
            Assert.False(_cache.Add(TIMESPAN_KEY, TIMESPAN_VALUE, expiration));

            Assert.Equal(TIMESPAN_VALUE, _cache.Get<TimeSpan>(TIMESPAN_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(TIMESPAN_VALUE, _cache.Get<TimeSpan>(TIMESPAN_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(TIMESPAN_VALUE, _cache.Get<TimeSpan>(TIMESPAN_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(TIMESPAN_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(21)]
        //[Fact]
        public void Add_guid_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(GUID_KEY, GUID_VALUE, expiration));
            Assert.False(_cache.Add(GUID_KEY, GUID_VALUE, expiration));

            Assert.Equal(GUID_VALUE, _cache.Get<Guid>(GUID_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(GUID_VALUE, _cache.Get<Guid>(GUID_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(GUID_VALUE, _cache.Get<Guid>(GUID_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(GUID_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(22)]
        //[Fact]
        public void Add_ipv4_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(IP_V4_KEY, IP_V4_VALUE, expiration));
            Assert.False(_cache.Add(IP_V4_KEY, IP_V4_VALUE, expiration));

            Assert.Equal(IP_V4_VALUE, _cache.Get<IPAddress>(IP_V4_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(IP_V4_VALUE, _cache.Get<IPAddress>(IP_V4_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(IP_V4_VALUE, _cache.Get<IPAddress>(IP_V4_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(IP_V4_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(23)]
        //[Fact]
        public void Add_ipv6_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(IP_V6_KEY, IP_V6_VALUE, expiration));
            Assert.False(_cache.Add(IP_V6_KEY, IP_V6_VALUE, expiration));

            Assert.Equal(IP_V6_VALUE, _cache.Get<IPAddress>(IP_V6_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(IP_V6_VALUE, _cache.Get<IPAddress>(IP_V6_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(IP_V6_VALUE, _cache.Get<IPAddress>(IP_V6_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(IP_V6_KEY));
        }

        [Fact, Order(4)]
        //[Fact, Order(24)]
        //[Fact]
        public void Add_object_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Assert.True(_cache.Add(OBJECT_KEY, OBJECT_VALUE, expiration));
            Assert.False(_cache.Add(OBJECT_KEY, OBJECT_VALUE, expiration));

            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(30).Wait();
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(60).Wait();
            Assert.Null(_cache.Get(OBJECT_KEY));
        }




        [Fact, Order(5)]
        //[Fact]
        public void Set_bool()
        {
            bool expected = false;
            _cache.Set(BOOL_KEY, expected);
            Assert.Equal(expected, _cache.Get<bool>(BOOL_KEY));

            expected = BOOL_VALUE;
            _cache.Set(BOOL_KEY, expected);
            var actual = _cache.Get(BOOL_KEY);
            Assert.NotNull(actual);
            Assert.True((bool)actual);
            _cache.Remove(BOOL_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(1)]
        //[Fact]
        public void Set_byte()
        {
            byte expected = 28;
            _cache.Set(BYTE_KEY, expected);
            byte actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);

            expected = BYTE_VALUE;
            _cache.Set(BYTE_KEY, expected);
            actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(BYTE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(2)]
        //[Fact]
        public void Set_sbyte()
        {
            sbyte expected = 28;
            _cache.Set(SBYTE_KEY, expected);
            sbyte actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);

            expected = SBYTE_VALUE;
            _cache.Set(SBYTE_KEY, expected);
            actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(SBYTE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(3)]
        //[Fact]
        public void Set_char()
        {
            char expected = 'q';
            _cache.Set(CHAR_KEY, expected);
            char actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);

            expected = CHAR_VALUE;
            _cache.Set(CHAR_KEY, expected);
            actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(CHAR_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(4)]
        //[Fact]
        public void Set_int16()
        {
            short expected = 8267;
            _cache.Set(INT16_KEY, expected);
            short actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);

            expected = INT16_VALUE;
            _cache.Set(INT16_KEY, expected);
            actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(INT16_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(5)]
        //[Fact]
        public void Set_uint16()
        {
            ushort expected = 8267;
            _cache.Set(UINT16_KEY, expected);
            ushort actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);

            expected = UINT16_VALUE;
            _cache.Set(UINT16_KEY, expected);
            actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(UINT16_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(6)]
        //[Fact]
        public void Set_int32()
        {
            int expected = 827;
            _cache.Set(INT32_KEY, expected);
            int actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);

            expected = INT32_VALUE;
            _cache.Set(INT32_KEY, expected);
            actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(INT32_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(7)]
        //[Fact]
        public void Set_uint32()
        {
            uint expected = 2;
            _cache.Set(UINT32_KEY, expected);
            uint actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);

            expected = UINT32_VALUE;
            _cache.Set(UINT32_KEY, expected);
            actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(UINT32_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(8)]
        //[Fact]
        public void Set_int64()
        {
            long expected = 2L;
            _cache.Set(INT64_KEY, expected);
            long actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);

            expected = INT64_VALUE;
            _cache.Set(INT64_KEY, expected);
            actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(INT64_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(9)]
        //[Fact]
        public void Set_uint64()
        {
            ulong expected = 2UL;
            _cache.Set(UINT64_KEY, expected);
            ulong actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);

            expected = UINT64_VALUE;
            _cache.Set(UINT64_KEY, expected);
            actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(UINT64_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(10)]
        //[Fact]
        public void Set_decimal()
        {
            decimal expected = -3232.336m;
            _cache.Set(DECIMAL_KEY, expected);
            decimal actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);

            expected = DECIMAL_VALUE;
            _cache.Set(DECIMAL_KEY, expected);
            actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(DECIMAL_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(11)]
        //[Fact]
        public void Set_float()
        {
            float expected = -3232.336f;
            _cache.Set(SINGLE_KEY, expected);
            float actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);

            expected = SINGLE_VALUE;
            _cache.Set(SINGLE_KEY, expected);
            actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(SINGLE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(12)]
        //[Fact]
        public void Set_double()
        {
            double expected = -3232.336d;
            _cache.Set(DOUBLE_KEY, expected);
            double actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);

            expected = DOUBLE_VALUE;
            _cache.Set(DOUBLE_KEY, expected);
            actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(DOUBLE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(13)]
        //[Fact]
        public void Set_string()
        {
            string expected = "初闻不知曲中意，再闻已是曲中人。";
            _cache.Set(STRING_KEY, expected);
            string actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);

            expected = STRING_VALUE;
            _cache.Set(STRING_KEY, expected);
            actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(STRING_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(14)]
        //[Fact]
        public void Set_byteArray()
        {
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 255 };
            _cache.Set(BYTE_ARRAY_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);

            expected = BYTE_ARRAY_VALUE;
            _cache.Set(BYTE_ARRAY_KEY, expected);
            actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(BYTE_ARRAY_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(15)]
        //[Fact]
        public void Set_arraySegment_byte()
        {
            ArraySegment<byte> expected = new ArraySegment<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(ARRAY_SEGMENT_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = ARRAY_SEGMENT_BYTE_VALUE;
            _cache.Set(ARRAY_SEGMENT_BYTE_KEY, expected);
            actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            _cache.Remove(ARRAY_SEGMENT_BYTE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(16)]
        //[Fact]
        public void Set_memory_byte()
        {
            Memory<byte> expected = new Memory<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(MEMORY_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = MEMORY_BYTE_VALUE;
            _cache.Set(MEMORY_BYTE_KEY, expected);
            actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            _cache.Remove(MEMORY_BYTE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(17)]
        //[Fact]
        public void Set_readOnlyMemory_byte()
        {
            ReadOnlyMemory<byte> expected = new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(READONLYMEMORY_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = READONLYMEMORY_BYTE_VALUE;
            _cache.Set(READONLYMEMORY_BYTE_KEY, expected);
            actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            _cache.Remove(READONLYMEMORY_BYTE_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(18)]
        //[Fact]
        public void Set_dateTime()
        {
            DateTime expected = new DateTime(2020, 5, 22, 20, 25, 07);
            _cache.Set(DATETIME_KEY, expected);
            DateTime actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);

            expected = new DateTime(2020, 5, 22, 12, 25, 07, DateTimeKind.Utc);
            _cache.Set(DATETIME_KEY, expected);
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);

            expected = DATETIME_VALUE;
            _cache.Set(DATETIME_KEY, expected);
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(DATETIME_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(19)]
        //[Fact]
        public void Set_dateTimeOffset()
        {
            DateTimeOffset expected = new DateTimeOffset(2020, 5, 22, 20, 25, 07, TimeSpan.FromMinutes(-210d));
            _cache.Set(DATETIMEOFFSET_KEY, expected);
            DateTimeOffset actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);

            expected = DATETIMEOFFSET_VALUE;
            _cache.Set(DATETIMEOFFSET_KEY, expected);
            actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(DATETIMEOFFSET_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(20)]
        //[Fact]
        public void Set_timeSpan()
        {
            TimeSpan expected = TimeSpan.FromDays(12.872938372335d);
            _cache.Set(TIMESPAN_KEY, expected);
            TimeSpan actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);

            expected = TIMESPAN_VALUE;
            _cache.Set(TIMESPAN_KEY, expected);
            actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(TIMESPAN_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(21)]
        //[Fact]
        public void Set_guid()
        {
            Guid expected = Guid.Empty;
            _cache.Set(GUID_KEY, expected);
            Guid actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);

            expected = GUID_VALUE;
            _cache.Set(GUID_KEY, expected);
            actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(GUID_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(22)]
        //[Fact]
        public void Set_ipv4()
        {
            IPAddress expected = IPAddress.Parse("10.206.0.33");
            _cache.Set(IP_V4_KEY, expected);
            IPAddress actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);

            expected = IP_V4_VALUE;
            _cache.Set(IP_V4_KEY, expected);
            actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(IP_V4_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(23)]
        //[Fact]
        public void Set_ipv6()
        {
            IPAddress expected = IPAddress.Parse("E3::01");
            _cache.Set(IP_V6_KEY, expected);
            IPAddress actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);

            expected = IP_V6_VALUE;
            _cache.Set(IP_V6_KEY, expected);
            actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);
            _cache.Remove(IP_V6_KEY);
        }

        [Fact, Order(5)]
        //[Fact, Order(24)]
        //[Fact]
        public void Set_object()
        {
            CacheObjectModel expected = new CacheObjectModel
            {
                ID = "325",
                Name = "海峡两岸"
            };

            _cache.Set(OBJECT_KEY, expected);
            CacheObjectModel actual = _cache.Get<CacheObjectModel>(OBJECT_KEY);
            Assert.Equal(expected, actual);

            
            _cache.Set(OBJECT_KEY, OBJECT_VALUE);
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            _cache.Remove(OBJECT_KEY);
        }








        [Fact, Order(6)]
        //[Fact]
        public void Set_bool_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            bool expected = false;
            _cache.Set(BOOL_KEY, expected);
            Assert.Equal(expected, _cache.Get<bool>(BOOL_KEY));

            expected = BOOL_VALUE;
            _cache.Set(BOOL_KEY, expected, expiration);
            Task.Delay(110).Wait();
            var actual = _cache.Get(BOOL_KEY);
            Assert.NotNull(actual);
            Assert.True((bool)actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(BOOL_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(1)]
        //[Fact]
        public void Set_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            byte expected = 28;
            _cache.Set(BYTE_KEY, expected);
            byte actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);

            expected = BYTE_VALUE;
            _cache.Set(BYTE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(BYTE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(2)]
        //[Fact]
        public void Set_sbyte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            sbyte expected = 28;
            _cache.Set(SBYTE_KEY, expected);
            sbyte actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);

            expected = SBYTE_VALUE;
            _cache.Set(SBYTE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(SBYTE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(3)]
        //[Fact]
        public void Set_char_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            char expected = 'q';
            _cache.Set(CHAR_KEY, expected);
            char actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);

            expected = CHAR_VALUE;
            _cache.Set(CHAR_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(CHAR_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(4)]
        //[Fact]
        public void Set_int16_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            short expected = 8267;
            _cache.Set(INT16_KEY, expected);
            short actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);

            expected = INT16_VALUE;
            _cache.Set(INT16_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(INT16_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(6)]
        //[Fact]
        public void Set_uint16_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            ushort expected = 8267;
            _cache.Set(UINT16_KEY, expected);
            ushort actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);

            expected = UINT16_VALUE;
            _cache.Set(UINT16_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(UINT16_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(6)]
        //[Fact]
        public void Set_int32_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            int expected = 827;
            _cache.Set(INT32_KEY, expected);
            int actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);

            expected = INT32_VALUE;
            _cache.Set(INT32_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(INT32_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(7)]
        //[Fact]
        public void Set_uint32_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            uint expected = 2;
            _cache.Set(UINT32_KEY, expected);
            uint actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);

            expected = UINT32_VALUE;
            _cache.Set(UINT32_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(UINT32_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(8)]
        //[Fact]
        public void Set_int64_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            long expected = 2L;
            _cache.Set(INT64_KEY, expected);
            long actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);

            expected = INT64_VALUE;
            _cache.Set(INT64_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(INT64_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(9)]
        //[Fact]
        public void Set_uint64_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            ulong expected = 2UL;
            _cache.Set(UINT64_KEY, expected);
            ulong actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);

            expected = UINT64_VALUE;
            _cache.Set(UINT64_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(UINT64_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(10)]
        //[Fact]
        public void Set_decimal_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            decimal expected = -3232.336m;
            _cache.Set(DECIMAL_KEY, expected);
            decimal actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);

            expected = DECIMAL_VALUE;
            _cache.Set(DECIMAL_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DECIMAL_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(11)]
        //[Fact]
        public void Set_float_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            float expected = -3232.336f;
            _cache.Set(SINGLE_KEY, expected);
            float actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);

            expected = SINGLE_VALUE;
            _cache.Set(SINGLE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(SINGLE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(12)]
        //[Fact]
        public void Set_double_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            double expected = -3232.336d;
            _cache.Set(DOUBLE_KEY, expected);
            double actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);

            expected = DOUBLE_VALUE;
            _cache.Set(DOUBLE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DOUBLE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(13)]
        //[Fact]
        public void Set_string_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            string expected = "初闻不知曲中意，再闻已是曲中人。";
            _cache.Set(STRING_KEY, expected);
            string actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);

            expected = STRING_VALUE;
            _cache.Set(STRING_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(STRING_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(14)]
        //[Fact]
        public void Set_byteArray_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 255 };
            _cache.Set(BYTE_ARRAY_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);

            expected = BYTE_ARRAY_VALUE;
            _cache.Set(BYTE_ARRAY_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(BYTE_ARRAY_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(15)]
        //[Fact]
        public void Set_arraySegment_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            ArraySegment<byte> expected = new ArraySegment<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(ARRAY_SEGMENT_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = ARRAY_SEGMENT_BYTE_VALUE;
            _cache.Set(ARRAY_SEGMENT_BYTE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(ARRAY_SEGMENT_BYTE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(16)]
        //[Fact]
        public void Set_memory_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Memory<byte> expected = new Memory<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(MEMORY_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = MEMORY_BYTE_VALUE;
            _cache.Set(MEMORY_BYTE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(MEMORY_BYTE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(17)]
        //[Fact]
        public void Set_readOnlyMemory_byte_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            ReadOnlyMemory<byte> expected = new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(READONLYMEMORY_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = READONLYMEMORY_BYTE_VALUE;
            _cache.Set(READONLYMEMORY_BYTE_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(READONLYMEMORY_BYTE_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(18)]
        //[Fact]
        public void Set_dateTime_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            DateTime expected = new DateTime(2020, 5, 22, 20, 25, 07);
            _cache.Set(DATETIME_KEY, expected);
            DateTime actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);

            expected = new DateTime(2020, 5, 22, 12, 25, 07, DateTimeKind.Utc);
            _cache.Set(DATETIME_KEY, expected);
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);

            expected = DATETIME_VALUE;
            _cache.Set(DATETIME_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DATETIME_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(19)]
        //[Fact]
        public void Set_dateTimeOffset_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            DateTimeOffset expected = new DateTimeOffset(2020, 5, 22, 20, 25, 07, TimeSpan.FromMinutes(-210d));
            _cache.Set(DATETIMEOFFSET_KEY, expected);
            DateTimeOffset actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);

            expected = DATETIMEOFFSET_VALUE;
            _cache.Set(DATETIMEOFFSET_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(DATETIMEOFFSET_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(20)]
        //[Fact]
        public void Set_timeSpan_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            TimeSpan expected = TimeSpan.FromDays(12.872938372335d);
            _cache.Set(TIMESPAN_KEY, expected);
            TimeSpan actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);

            expected = TIMESPAN_VALUE;
            _cache.Set(TIMESPAN_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(TIMESPAN_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(21)]
        //[Fact]
        public void Set_guid_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            Guid expected = Guid.Empty;
            _cache.Set(GUID_KEY, expected);
            Guid actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);

            expected = GUID_VALUE;
            _cache.Set(GUID_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(GUID_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(22)]
        //[Fact]
        public void Set_ipv4_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            IPAddress expected = IPAddress.Parse("10.206.0.33");
            _cache.Set(IP_V4_KEY, expected);
            IPAddress actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);

            expected = IP_V4_VALUE;
            _cache.Set(IP_V4_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(IP_V4_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(23)]
        //[Fact]
        public void Set_ipv6_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            IPAddress expected = IPAddress.Parse("E3::01");
            _cache.Set(IP_V6_KEY, expected);
            IPAddress actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);

            expected = IP_V6_VALUE;
            _cache.Set(IP_V6_KEY, expected, expiration);
            Task.Delay(110).Wait();
            actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(IP_V6_KEY));
        }

        [Fact, Order(6)]
        //[Fact, Order(24)]
        //[Fact]
        public void Set_object_absolute_expiration()
        {
            var expiration = DateTimeOffset.Now.AddMilliseconds(200);
            CacheObjectModel expected = new CacheObjectModel
            {
                ID = "325",
                Name = "海峡两岸"
            };

            _cache.Set(OBJECT_KEY, expected);
            CacheObjectModel actual = _cache.Get<CacheObjectModel>(OBJECT_KEY);
            Assert.Equal(expected, actual);


            _cache.Set(OBJECT_KEY, OBJECT_VALUE, expiration);
            Task.Delay(110).Wait();
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(100).Wait();
            Assert.False(_cache.Contains(OBJECT_KEY));
        }








        
        [Fact, Order(7)]
        //[Fact]
        public void Set_bool_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            bool expected = false;
            _cache.Set(BOOL_KEY, expected);
            Assert.Equal(expected, _cache.Get<bool>(BOOL_KEY));

            expected = BOOL_VALUE;
            _cache.Set(BOOL_KEY, expected, expiration);
            Task.Delay(30).Wait();
            var actual = _cache.Get(BOOL_KEY);
            Assert.NotNull(actual);
            Assert.True((bool)actual);
            Task.Delay(40).Wait();
            actual = _cache.Get(BOOL_KEY);
            Assert.NotNull(actual);
            Assert.True((bool)actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(BOOL_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(1)]
        //[Fact]
        public void Set_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            byte expected = 28;
            _cache.Set(BYTE_KEY, expected);
            byte actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);

            expected = BYTE_VALUE;
            _cache.Set(BYTE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<byte>(BYTE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(BYTE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(2)]
        //[Fact]
        public void Set_sbyte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            sbyte expected = 28;
            _cache.Set(SBYTE_KEY, expected);
            sbyte actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);

            expected = SBYTE_VALUE;
            _cache.Set(SBYTE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<sbyte>(SBYTE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(SBYTE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(3)]
        //[Fact]
        public void Set_char_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            char expected = 'q';
            _cache.Set(CHAR_KEY, expected);
            char actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);

            expected = CHAR_VALUE;
            _cache.Set(CHAR_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<char>(CHAR_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(CHAR_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(4)]
        //[Fact]
        public void Set_int16_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            short expected = 8267;
            _cache.Set(INT16_KEY, expected);
            short actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);

            expected = INT16_VALUE;
            _cache.Set(INT16_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<short>(INT16_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(INT16_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(7)]
        //[Fact]
        public void Set_uint16_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            ushort expected = 8267;
            _cache.Set(UINT16_KEY, expected);
            ushort actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);

            expected = UINT16_VALUE;
            _cache.Set(UINT16_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<ushort>(UINT16_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(UINT16_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(7)]
        //[Fact]
        public void Set_int32_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            int expected = 827;
            _cache.Set(INT32_KEY, expected);
            int actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);

            expected = INT32_VALUE;
            _cache.Set(INT32_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<int>(INT32_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(INT32_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(7)]
        //[Fact]
        public void Set_uint32_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            uint expected = 2;
            _cache.Set(UINT32_KEY, expected);
            uint actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);

            expected = UINT32_VALUE;
            _cache.Set(UINT32_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<uint>(UINT32_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(UINT32_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(8)]
        //[Fact]
        public void Set_int64_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            long expected = 2L;
            _cache.Set(INT64_KEY, expected);
            long actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);

            expected = INT64_VALUE;
            _cache.Set(INT64_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<long>(INT64_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(INT64_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(9)]
        //[Fact]
        public void Set_uint64_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            ulong expected = 2UL;
            _cache.Set(UINT64_KEY, expected);
            ulong actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);

            expected = UINT64_VALUE;
            _cache.Set(UINT64_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<ulong>(UINT64_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(UINT64_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(10)]
        //[Fact]
        public void Set_decimal_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            decimal expected = -3232.336m;
            _cache.Set(DECIMAL_KEY, expected);
            decimal actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);

            expected = DECIMAL_VALUE;
            _cache.Set(DECIMAL_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<decimal>(DECIMAL_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(DECIMAL_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(11)]
        //[Fact]
        public void Set_float_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            float expected = -3232.336f;
            _cache.Set(SINGLE_KEY, expected);
            float actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);

            expected = SINGLE_VALUE;
            _cache.Set(SINGLE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<float>(SINGLE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(SINGLE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(12)]
        //[Fact]
        public void Set_double_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            double expected = -3232.336d;
            _cache.Set(DOUBLE_KEY, expected);
            double actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);

            expected = DOUBLE_VALUE;
            _cache.Set(DOUBLE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<double>(DOUBLE_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(DOUBLE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(13)]
        //[Fact]
        public void Set_string_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            string expected = "初闻不知曲中意，再闻已是曲中人。";
            _cache.Set(STRING_KEY, expected);
            string actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);

            expected = STRING_VALUE;
            _cache.Set(STRING_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<string>(STRING_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(STRING_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(14)]
        //[Fact]
        public void Set_byteArray_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 255 };
            _cache.Set(BYTE_ARRAY_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);

            expected = BYTE_ARRAY_VALUE;
            _cache.Set(BYTE_ARRAY_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<byte[]>(BYTE_ARRAY_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(BYTE_ARRAY_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(15)]
        //[Fact]
        public void Set_arraySegment_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            ArraySegment<byte> expected = new ArraySegment<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(ARRAY_SEGMENT_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = ARRAY_SEGMENT_BYTE_VALUE;
            _cache.Set(ARRAY_SEGMENT_BYTE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<byte[]>(ARRAY_SEGMENT_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(ARRAY_SEGMENT_BYTE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(16)]
        //[Fact]
        public void Set_memory_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Memory<byte> expected = new Memory<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(MEMORY_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = MEMORY_BYTE_VALUE;
            _cache.Set(MEMORY_BYTE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<byte[]>(MEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(MEMORY_BYTE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(17)]
        //[Fact]
        public void Set_readOnlyMemory_byte_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            ReadOnlyMemory<byte> expected = new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 255 }, 3, 2);
            _cache.Set(READONLYMEMORY_BYTE_KEY, expected);
            byte[] actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);

            expected = READONLYMEMORY_BYTE_VALUE;
            _cache.Set(READONLYMEMORY_BYTE_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<byte[]>(READONLYMEMORY_BYTE_KEY);
            ReadOnlyMemoryEquals(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(READONLYMEMORY_BYTE_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(18)]
        //[Fact]
        public void Set_dateTime_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            DateTime expected = new DateTime(2020, 5, 22, 20, 25, 07);
            _cache.Set(DATETIME_KEY, expected);
            DateTime actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);

            expected = new DateTime(2020, 5, 22, 12, 25, 07, DateTimeKind.Utc);
            _cache.Set(DATETIME_KEY, expected);
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);

            expected = DATETIME_VALUE;
            _cache.Set(DATETIME_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<DateTime>(DATETIME_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(DATETIME_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(19)]
        //[Fact]
        public void Set_dateTimeOffset_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            DateTimeOffset expected = new DateTimeOffset(2020, 5, 22, 20, 25, 07, TimeSpan.FromMinutes(-210d));
            _cache.Set(DATETIMEOFFSET_KEY, expected);
            DateTimeOffset actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);

            expected = DATETIMEOFFSET_VALUE;
            _cache.Set(DATETIMEOFFSET_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<DateTimeOffset>(DATETIMEOFFSET_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(DATETIMEOFFSET_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(20)]
        //[Fact]
        public void Set_timeSpan_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            TimeSpan expected = TimeSpan.FromDays(12.872938372335d);
            _cache.Set(TIMESPAN_KEY, expected);
            TimeSpan actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);

            expected = TIMESPAN_VALUE;
            _cache.Set(TIMESPAN_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<TimeSpan>(TIMESPAN_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(TIMESPAN_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(21)]
        //[Fact]
        public void Set_guid_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            Guid expected = Guid.Empty;
            _cache.Set(GUID_KEY, expected);
            Guid actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);

            expected = GUID_VALUE;
            _cache.Set(GUID_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<Guid>(GUID_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(GUID_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(22)]
        //[Fact]
        public void Set_ipv4_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            IPAddress expected = IPAddress.Parse("10.206.0.33");
            _cache.Set(IP_V4_KEY, expected);
            IPAddress actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);

            expected = IP_V4_VALUE;
            _cache.Set(IP_V4_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<IPAddress>(IP_V4_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(IP_V4_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(23)]
        //[Fact]
        public void Set_ipv6_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            IPAddress expected = IPAddress.Parse("E3::01");
            _cache.Set(IP_V6_KEY, expected);
            IPAddress actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);

            expected = IP_V6_VALUE;
            _cache.Set(IP_V6_KEY, expected, expiration);
            Task.Delay(30).Wait();
            actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(40).Wait();
            actual = _cache.Get<IPAddress>(IP_V6_KEY);
            Assert.Equal(expected, actual);
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(IP_V6_KEY));
        }

        [Fact, Order(7)]
        //[Fact, Order(24)]
        //[Fact]
        public void Set_object_sliding_expiration()
        {
            var expiration = TimeSpan.FromMilliseconds(50);
            CacheObjectModel expected = new CacheObjectModel
            {
                ID = "325",
                Name = "海峡两岸"
            };

            _cache.Set(OBJECT_KEY, expected);
            CacheObjectModel actual = _cache.Get<CacheObjectModel>(OBJECT_KEY);
            Assert.Equal(expected, actual);


            _cache.Set(OBJECT_KEY, OBJECT_VALUE, expiration);
            Task.Delay(30).Wait();
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(40).Wait();
            Assert.Equal(OBJECT_VALUE, _cache.Get<IReadOnlyList<CacheObjectModel>>(OBJECT_KEY));
            Task.Delay(60).Wait();
            Assert.False(_cache.Contains(OBJECT_KEY));
        }



        public void Dispose()
        {
            _cache.Dispose();
        }





        private void ArraySegmentEquals(ArraySegment<byte> expected, ArraySegment<byte> actual)
        {
            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected.Array[expected.Offset + i], actual.Array[actual.Offset + i]);
            }
        }

        private void ReadOnlyMemoryEquals(ReadOnlyMemory<byte> expected, ReadOnlyMemory<byte> actual)
        {
            Assert.Equal(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
            {
                Assert.Equal(expected.Span[i], actual.Span[i]);
            }
        }
    }
}
