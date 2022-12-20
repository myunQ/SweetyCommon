/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 *
 *     分布式缓存服务 RedisCache 的操作类。
 *     
 *  StackExchange.Redis：
 *     https://github.com/StackExchange/StackExchange.Redis
 *     https://stackexchange.github.io/StackExchange.Redis/
 *     https://stackexchange.github.io/StackExchange.Redis/ReleaseNotes
 *     https://www.nuget.org/packages/StackExchange.Redis/
 * 
 * Members Index:
 *      class RedisCache
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Caching
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using StackExchange.Redis;

    using Newtonsoft.Json;

    using Sweety.Common.Compression;
    using Sweety.Common.Extensions;


    /// <summary>
    /// 分布式缓存服务 <c>Redis</c> 的操作类。
    /// </summary>
    public class RedisCache : ICache, IDisposable
    {
        const byte BOOLEAN_TYPE_INDEX = 1;
        const byte CHAR_TYPE_INDEX = 2;
        const byte BYTE_TYPE_INDEX = 3;
        const byte SBYTE_TYPE_INDEX = 4;
        const byte INT16_TYPE_INDEX = 5;
        const byte UINT16_TYPE_INDEX = 6;
        const byte INT32_TYPE_INDEX = 7;
        const byte UINT32_TYPE_INDEX = 8;
        const byte INT64_TYPE_INDEX = 9;
        const byte UINT64_TYPE_INDEX = 10;
        const byte SINGLE_TYPE_INDEX = 11;
        const byte DOUBLE_TYPE_INDEX = 12;
        const byte DECIMAL_TYPE_INDEX = 13;
        const byte TIMESPAN_TYPE_INDEX = 14;
        const byte DATETIME_TYPE_INDEX = 15;
        const byte DATETIMEOFFSET_TYPE_INDEX = 16;
        const byte GUID_TYPE_INDEX = 17;
        const byte STRING_TYPE_INDEX = 18;
        const byte IP_TYPE_INDEX = 19;
        const byte BYTEARRAY_TYPE_INDEX = 20;
        const byte OBJECT_TYPE_INDEX = 21;

        const int ExpirationTypeMask = 0b_0100_0000;

        object _lock = new object();

        ConfigurationOptions _configuration = null;

        ConnectionMultiplexer _conn;

        IDatabase _cache;

        /// <summary>
        /// 缓存内容长度大于此值则压缩后在存储
        /// </summary>
        long _compressionThreshold = 102400L; //100KB

        ICompression _compression = null;

        /// <summary>
        /// 在序列化和反序列化类型为接口或基类的集合时可以得出每一个元素的正确类型。
        /// </summary>
        static JsonSerializerSettings __jsonSerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


        public RedisCache(string configuration)
        {
            _configuration = ConfigurationOptions.Parse(configuration);

            _cache = RedisConn.GetDatabase();
        }

        ~RedisCache()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;
            }
        }


        /// <summary>
        /// 压缩/解压缩工具
        /// </summary>
        private ICompression Compression
        {
            get
            {
                if (_compression == null)
                {
                    _compression = new Deflate();
                }
                return _compression;
            }
        }

        private ConnectionMultiplexer RedisConn
        {
            get
            {
                if (_conn == null)
                {
                    lock (_lock)
                    {
                        if (_conn == null || !_conn.IsConnected)
                        {
                            _conn = ConnectionMultiplexer.Connect(_configuration);
                        }
                    }
                }
                return _conn;
            }
        }


        public bool Add(string key, object value)
        {
            CheckKey(key);
            if (value == null) throw new ArgumentNullException(nameof(value));

            RedisKey redisKey = new RedisKey(key);

            if (_cache.KeyExists(redisKey)) return false;

            var redisValue = BeforeStorageValueHandler(value);

            return _cache.StringSet(redisKey, redisValue, when: When.NotExists);
        }

        public bool Add(string key, object value, DateTimeOffset absoluteExpiration)
        {
            CheckKey(key);
            if (value == null) throw new ArgumentNullException(nameof(value));

            RedisKey redisKey = new RedisKey(key);

            if (_cache.KeyExists(redisKey)) return false;

            var redisValue = BeforeStorageValueHandler(value);

            return _cache.StringSet(redisKey, redisValue, absoluteExpiration.Subtract(DateTimeOffset.UtcNow), when: When.NotExists);
        }

        public bool Add(string key, object value, TimeSpan slidingExpiration)
        {
            CheckKey(key);
            if (value == null) throw new ArgumentNullException(nameof(value));

            RedisKey redisKey = new RedisKey(key);

            if (_cache.KeyExists(redisKey)) return false;

            var redisValue = BeforeStorageValueHandler(value, slidingExpiration);

            return _cache.StringSet(redisKey, redisValue, slidingExpiration, when: When.NotExists);
        }




        public void Set(string key, object value)
        {
            CheckKey(key);

            var redisValue = BeforeStorageValueHandler(value);

            _cache.StringSet(new RedisKey(key), redisValue, flags: CommandFlags.FireAndForget);
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration)
        {
            CheckKey(key);

            var redisValue = BeforeStorageValueHandler(value);

            _cache.StringSet(new RedisKey(key), redisValue, absoluteExpiration.Subtract(DateTimeOffset.UtcNow), flags: CommandFlags.FireAndForget);
        }

        public void Set(string key, object value, TimeSpan slidingExpiration)
        {
            CheckKey(key);

            var redisValue = BeforeStorageValueHandler(value, slidingExpiration);

            _cache.StringSet(new RedisKey(key), redisValue, slidingExpiration, flags: CommandFlags.FireAndForget);
        }



        public void Remove(string key)
        {
            CheckKey(key);

            _cache.KeyDelete(new RedisKey(key), CommandFlags.FireAndForget);
        }

        public void Remove(params string[] keys)
        {
            Remove((IEnumerable<string>)keys);
        }

        public void Remove(IEnumerable<string> keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));

            foreach (var key in keys)
            {
                if (String.IsNullOrEmpty(key)) throw new ArgumentException();
            }

            _cache.KeyDelete(keys.Select(key => new RedisKey(key)).ToArray(), CommandFlags.FireAndForget);
        }



        public bool Contains(string key)
        {
            CheckKey(key);

            return _cache.KeyExists(new RedisKey(key));
        }


        public bool ContainsAll(params string[] keys)
        {
            return ContainsAll((IEnumerable<string>)keys);
        }

        public bool ContainsAll(IEnumerable<string> keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));

            foreach (var key in keys)
            {
                if (String.IsNullOrEmpty(key)) throw new ArgumentException();
            }

            foreach (var key in keys)
            {
                if (!_cache.KeyExists(new RedisKey(key))) return false;
            }

            return true;
        }


        public bool ContainsAny(params string[] keys)
        {
            return ContainsAny((IEnumerable<string>)keys);
        }

        public bool ContainsAny(IEnumerable<string> keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));

            foreach (var key in keys)
            {
                if (String.IsNullOrEmpty(key)) throw new ArgumentException();
            }

            foreach (var key in keys)
            {
                if (_cache.KeyExists(new RedisKey(key))) return true;
            }

            return false;
        }



        public object Get(string key)
        {
            CheckKey(key);

            RedisKey redisKey = new RedisKey(key);
            var redisValue = _cache.StringGet(redisKey);
            if (redisValue.HasValue)
            {
                var raw = (byte[])redisValue;
                if ((raw[0] & ExpirationTypeMask) == ExpirationTypeMask)
                {
                    _cache.KeyExpire(redisKey, TimeSpan.FromMilliseconds(BitConverter.ToUInt32(raw, 1)), CommandFlags.FireAndForget);
                }

                return BeforeReturningValueHandler(raw);
            }
            return null;
        }

        public T Get<T>(string key)
        {
            object result = Get(key);

            return (result == null) ? default : (T)result;
        }

        public IDictionary<string, object> GetValues(string[] keys)
        {
            return GetValues((IEnumerable<string>)keys);
        }

        public IDictionary<string, object> GetValues(IEnumerable<string> keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));

            int i = 0;
            RedisKey[] redisKeys = new RedisKey[keys.Count()];
            foreach (var key in keys)
            {
                if (String.IsNullOrEmpty(key)) throw new ArgumentException();
                redisKeys[i++] = key;
            }

            IDictionary<string, object> result = new Dictionary<string, object>(redisKeys.Length);

            i = 0;
            var redisValues = _cache.StringGet(redisKeys);
            foreach (var key in keys)
            {
                ref RedisValue redisValue = ref redisValues[i];

                if (redisValue.HasValue)
                {
                    var raw = (byte[])redisValue;
                    if ((raw[0] & ExpirationTypeMask) == ExpirationTypeMask)
                    {
                        ref RedisKey redisKey = ref redisKeys[i];
                        _cache.KeyExpire(redisKey, TimeSpan.FromMilliseconds(BitConverter.ToUInt32(raw, 1)), CommandFlags.FireAndForget);
                    }

                    result.Add(key, BeforeReturningValueHandler(raw));
                }
                i++;
            }

            return result;
        }




        /// <summary>
        /// 检测 Key 是否有效
        /// </summary>
        /// <param name="key">缓存 Key</param>
        private void CheckKey(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (key == String.Empty) throw new ArgumentException();
        }


        /// <summary>
        /// 缓存之前对缓存内容进行处理
        /// </summary>
        /// <param name="value">要缓存的内容</param>
        /// <param name="slidingExpiration">缓存项是否是滑动过期。</param>
        /// <returns>经过处理后的缓存内容</returns>
        private RedisValue BeforeStorageValueHandler(object value, TimeSpan? slidingExpiration = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (slidingExpiration.HasValue && slidingExpiration.Value == TimeSpan.Zero) slidingExpiration = null;

            Type type = value.GetType();

            byte[] buffer = FoundationToBytes(value, type, slidingExpiration);

            if (buffer == null)
            {
                /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
                 * 对象类型的 byte[] 格式：
                 * 当数据类型是 对象类型 时第 2~3 个字节存储 对象类型的实际类型名称的长度，从第四个字节开始存储实例类型名称对应的 UTF-8 码，之后才是实际内容的序列化结果。
                 * 如果是滑动过期时间的话从第 2 个字节开始往后移 4 个字节，空出的 4 个字节存储滑动过期的毫秒数。
                 * 如果需要压缩的话再从第 2 个字节开往后移 4 个字节，空出的 4 个字节存储压缩前的 byte 数。
                 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

                string typeName = TypeNameHandler(type);
                string valueJson = JsonConvert.SerializeObject(value, Formatting.None, __jsonSerSettings);

                int typeNameByteLength = Encoding.UTF8.GetByteCount(typeName);
                int valueByteLength = Encoding.UTF8.GetByteCount(valueJson);

                if (slidingExpiration.HasValue)
                {
                    if ((7 + typeNameByteLength + valueByteLength) < _compressionThreshold)
                    {
                        buffer = new byte[7 + typeNameByteLength + valueByteLength];
                        //5
                        //7
                        //7 + typeNameByteLength
                    }
                    else
                    {
                        buffer = new byte[11 + typeNameByteLength + valueByteLength];
                        //9
                        //11
                        //11 + typeNameByteLength
                    }
                }
                else
                {
                    if ((typeNameByteLength + valueByteLength + 3) < _compressionThreshold)
                    {
                        buffer = new byte[3 + typeNameByteLength + valueByteLength];
                        //1
                        //3
                        //3 + typeNameByteLength
                    }
                    else
                    {
                        buffer = new byte[7 + typeNameByteLength + valueByteLength];
                        //5
                        //7
                        //7 + typeNameByteLength
                    }
                }

                BitConverter.GetBytes((ushort)typeNameByteLength).CopyTo(buffer, buffer.Length - (2 + typeNameByteLength + valueByteLength));
                Encoding.UTF8.GetBytes(typeName, 0, typeName.Length, buffer, buffer.Length - (typeNameByteLength + valueByteLength));
                Encoding.UTF8.GetBytes(valueJson, 0, valueJson.Length, buffer, buffer.Length - valueByteLength);

                SetDataTypeAndExpirationType(buffer, slidingExpiration, OBJECT_TYPE_INDEX);
            }


            if (buffer.LongLength < _compressionThreshold)
            {
                return buffer;
            }

            if (slidingExpiration.HasValue)
            {
                Array.Copy(buffer, 1, buffer, 5, 4);

#if NETSTANDARD2_0
                BitConverter.GetBytes(buffer.Length).CopyTo(buffer, 1);
#else
                BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, 4), buffer.Length);
#endif
            }

            return RedisValue.CreateFrom(CompressHandler(buffer));
        }


        /// <summary>
        /// 获取缓存内容时，在返回内容之前对内容进行处理
        /// </summary>\
        /// <param name="value">要返回的缓存内容</param>
        /// <returns>经过处理后的缓存的内容</returns>
        private object BeforeReturningValueHandler(byte[] value)
        {
            if (value == null) return null;

            if ((value[0] & 0x80) == 0x80)
            {
                value = DecompressHandler(value);
            }

            int startIndex = ((value[0] & ExpirationTypeMask) == ExpirationTypeMask) ? 5 : 1;

#if NETSTANDARD2_0
            switch (value[0] & 0x3f)
            {
                case INT32_TYPE_INDEX: return BitConverter.ToInt32(value, startIndex);
                case INT16_TYPE_INDEX: return BitConverter.ToInt16(value, startIndex);
                case INT64_TYPE_INDEX: return BitConverter.ToInt64(value, startIndex);
                case DATETIME_TYPE_INDEX: return ToDateTime(value, startIndex);
                case UINT32_TYPE_INDEX: return BitConverter.ToUInt32(value, startIndex);
                case UINT16_TYPE_INDEX: return BitConverter.ToUInt16(value, startIndex);
                case UINT64_TYPE_INDEX: return BitConverter.ToUInt64(value, startIndex);
                case SINGLE_TYPE_INDEX: return BitConverter.ToSingle(value, startIndex);
                case DOUBLE_TYPE_INDEX: return BitConverter.ToDouble(value, startIndex);
                case BOOLEAN_TYPE_INDEX: return BitConverter.ToBoolean(value, startIndex);
                case CHAR_TYPE_INDEX: return BitConverter.ToChar(value, startIndex);
                case BYTE_TYPE_INDEX: return value[startIndex];
                case SBYTE_TYPE_INDEX: return unchecked((SByte)value[startIndex]);
                case DECIMAL_TYPE_INDEX: return value.ToDecimal(startIndex);
                case TIMESPAN_TYPE_INDEX: return new TimeSpan(BitConverter.ToInt64(value, startIndex));
                case DATETIMEOFFSET_TYPE_INDEX: return new DateTimeOffset(BitConverter.ToInt64(value, startIndex), new TimeSpan(BitConverter.ToInt64(value, startIndex + 8)));
                case GUID_TYPE_INDEX: return ToGuid(value, startIndex);
                case IP_TYPE_INDEX: return ToIPAddress(value, startIndex);
                case BYTEARRAY_TYPE_INDEX: return new Span<byte>(value, startIndex, value.Length - startIndex).ToArray();
                case STRING_TYPE_INDEX: return Encoding.UTF8.GetString(value, startIndex, value.Length - startIndex);
                case OBJECT_TYPE_INDEX: return DeserializeObject(value, startIndex);
                default: throw new NotSupportedException();
            }
#else
            return (value[0] & 0x3f) switch
            {
                INT32_TYPE_INDEX => BitConverter.ToInt32(value, startIndex),
                INT16_TYPE_INDEX => BitConverter.ToInt16(value, startIndex),
                INT64_TYPE_INDEX => BitConverter.ToInt64(value, startIndex),
                DATETIME_TYPE_INDEX => ToDateTime(value, startIndex),
                UINT32_TYPE_INDEX => BitConverter.ToUInt32(value, startIndex),
                UINT16_TYPE_INDEX => BitConverter.ToUInt16(value, startIndex),
                UINT64_TYPE_INDEX => BitConverter.ToUInt64(value, startIndex),
                SINGLE_TYPE_INDEX => BitConverter.ToSingle(value, startIndex),
                DOUBLE_TYPE_INDEX => BitConverter.ToDouble(value, startIndex),
                BOOLEAN_TYPE_INDEX => BitConverter.ToBoolean(value, startIndex),
                CHAR_TYPE_INDEX => BitConverter.ToChar(value, startIndex),
                BYTE_TYPE_INDEX => value[startIndex],
                SBYTE_TYPE_INDEX => unchecked((SByte)value[startIndex]),
                DECIMAL_TYPE_INDEX => value.ToDecimal(startIndex),

                TIMESPAN_TYPE_INDEX => new TimeSpan(BitConverter.ToInt64(value, startIndex)),
                DATETIMEOFFSET_TYPE_INDEX => new DateTimeOffset(BitConverter.ToInt64(value, startIndex), new TimeSpan(BitConverter.ToInt64(value, startIndex + 8))),
                GUID_TYPE_INDEX => new Guid(new ReadOnlySpan<byte>(value, startIndex, 16)),
                STRING_TYPE_INDEX => Encoding.UTF8.GetString(value, startIndex, value.Length - startIndex),
                IP_TYPE_INDEX => new System.Net.IPAddress(new ReadOnlySpan<byte>(value, startIndex, value.Length - startIndex)),
                BYTEARRAY_TYPE_INDEX => new Span<byte>(value, startIndex, value.Length - startIndex).ToArray(),
                OBJECT_TYPE_INDEX => DeserializeObject(value, startIndex),
                _ => throw new NotSupportedException()
            };
#endif

#if NETSTANDARD2_0
            object DeserializeObject(byte[] buffer, int sub_startIndex)
#else
            static object DeserializeObject(byte[] buffer, int sub_startIndex)
#endif
            {
                int typeNameLength = BitConverter.ToUInt16(buffer, sub_startIndex);

                Type type = Type.GetType(
                    Encoding.UTF8.GetString(buffer, sub_startIndex + 2, typeNameLength)
                    , true);

                sub_startIndex += 2 + typeNameLength;
                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(buffer, sub_startIndex, buffer.Length - sub_startIndex), type, __jsonSerSettings);
            }

#if NETSTANDARD2_0
            Guid ToGuid(byte[] buffer, int sub_startIndex)
            {
                byte[] bits = new byte[16];
                Array.Copy(buffer, sub_startIndex, bits, 0, 16);
                return new Guid(bits);
            }

            System.Net.IPAddress ToIPAddress(byte[] buffer, int sub_startIndex)
            {
                byte[] bits = new byte[buffer.Length - sub_startIndex == 16 ? 16 : 4];
                Array.Copy(buffer, sub_startIndex, bits, 0, bits.Length);
                return new System.Net.IPAddress(bits);
            }
#endif

            DateTime ToDateTime(byte[] buffer, int sub_startIndex)
            {
                long dateData = BitConverter.ToInt64(value, startIndex);

                return new DateTime(dateData & 0x3FFFFFFFFFFFFFFF, (DateTimeKind)((ulong)dateData >> 62));
            }
        }

        /// <summary>
        /// 将 <c>.net</c> 常用基础类型的值转换成 <see cref="Byte[]"/>。
        /// </summary>
        /// <remarks>
        /// 这个方法里不做压缩数据的操作，即使遇到超过 <see cref="_compressionThreshold"/> 长度的 <see cref="string"/> 或 <see cref="byte[]"/> 值。
        /// 转换的基础类型包括：
        /// <see cref="Boolean"/>、
        /// <see cref="Char"/>、
        /// <see cref="Byte"/>、
        /// <see cref="SByte"/>、
        /// <see cref="Byte[]"/>、
        /// <see cref="ArraySegment{byte}"/>（当做 <see cref="Byte[]"/> 类型处理）、
        /// <see cref="Memory{byte}"/>（当做 <see cref="Byte[]"/> 类型处理）、
        /// <see cref="ReadOnlyMemory{byte}"/>（当做 <see cref="Byte[]"/> 类型处理）、
        /// <see cref="Int16"/>、
        /// <see cref="UInt16"/>、
        /// <see cref="Int32"/>、
        /// <see cref="UInt32"/>、
        /// <see cref="Int64"/>、
        /// <see cref="UInt64"/>、
        /// <see cref="Decimal"/>、
        /// <see cref="Single"/>、
        /// <see cref="Double"/>、
        /// <see cref="TimeSpan"/>、
        /// <see cref="DateTime"/>、
        /// <see cref="DateTimeOffset"/>、
        /// <see cref="Guid"/>、
        /// <see cref="System.Net.IPAddress"/>、
        /// <see cref="String"/>。
        /// </remarks>
        /// <param name="value">要缓存的对象。</param>
        /// <param name="valueType"><paramref name="value"/> 的类型。</param>
        /// <param name="slidingExpiration">缓存项是否是使用滑动过期策略</param>
        /// <returns>
        /// 如果 <paramref name="value"/> 是此方法能处理的基础数据类型则返回其值的 <see cref="Byte[]"/> 表现形式，否则返回 <c>null</c>。
        /// 返回的 <see cref="Byte[]"/> 的第一个字节（即 <c><see cref="byte"/>[0]</c>）的
        ///     第 1 个 bit 表示数据是否经过压缩（0：未压缩；1：压缩），
        ///     第 2 个 bit 表示缓存过期策略（0：固定日期时间过期；1：滑动过期时间）
        ///     第 3~8 个 bit 表示数据的原类型。
        /// 如果 <paramref name="slidingExpiration"/> 的值非 <c>null</c> 的话，
        /// 第 <c><see cref="byte"/>[1~4]</c> 共 4 个字节存储的是滑动过期时间（单位：毫秒）。
        /// </returns>
        private byte[] FoundationToBytes(object value, Type valueType, TimeSpan? slidingExpiration)
        {
            if (valueType.IsValueType)
            {
                if (Type.GetTypeCode(valueType) != TypeCode.Object)
                {
#if NETSTANDARD2_0
                    switch (value)
                    {
                        case int v: return ToBytes(v, slidingExpiration);
                        case short v: return ToBytes(v, slidingExpiration);
                        case long v: return ToBytes(v, slidingExpiration);
                        case DateTime v: return ToBytes(v, slidingExpiration);
                        case uint v: return ToBytes(v, slidingExpiration);
                        case ushort v: return ToBytes(v, slidingExpiration);
                        case ulong v: return ToBytes(v, slidingExpiration);
                        case float v: return ToBytes(v, slidingExpiration);
                        case double v: return ToBytes(v, slidingExpiration);
                        case bool v: return ToBytes(v, slidingExpiration);
                        case char v: return ToBytes(v, slidingExpiration);
                        case byte v: return ToBytes(v, slidingExpiration);
                        case sbyte v: return ToBytes(v, slidingExpiration);
                        case decimal v: return ToBytes(v, slidingExpiration);
                        default: return null;
                    }
#else
                    return value switch
                    {
                        int v => ToBytes(v, slidingExpiration),
                        short v => ToBytes(v, slidingExpiration),
                        long v => ToBytes(v, slidingExpiration),
                        DateTime v => ToBytes(v, slidingExpiration),
                        uint v => ToBytes(v, slidingExpiration),
                        ushort v => ToBytes(v, slidingExpiration),
                        ulong v => ToBytes(v, slidingExpiration),
                        float v => ToBytes(v, slidingExpiration),
                        double v => ToBytes(v, slidingExpiration),
                        bool v => ToBytes(v, slidingExpiration),
                        char v => ToBytes(v, slidingExpiration),
                        byte v => ToBytes(v, slidingExpiration),
                        sbyte v => ToBytes(v, slidingExpiration),
                        decimal v => ToBytes(v, slidingExpiration),
                        _ => null
                    };
#endif
                }
                else
                {
#if NETSTANDARD2_0
                    switch (value)
                    {
                        case Guid v: return ToBytes(v, slidingExpiration);
                        case TimeSpan v: return ToBytes(v, slidingExpiration);
                        case DateTimeOffset v: return ToBytes(v, slidingExpiration);
                        case ArraySegment<byte> v: return ToBytes(v, slidingExpiration);
                        default: return null;
                    }
#else
                    return value switch
                    {
                        Guid v => ToBytes(v, slidingExpiration),
                        TimeSpan v => ToBytes(v, slidingExpiration),
                        DateTimeOffset v => ToBytes(v, slidingExpiration),
                        ArraySegment<byte> v => ToBytes(v, slidingExpiration),
                        Memory<byte> v => ToBytes(v, slidingExpiration),
                        ReadOnlyMemory<byte> v => ToBytes(v, slidingExpiration),
                        _ => null
                    };
#endif
                }
            }

#if NETSTANDARD2_0
            switch (value)
            {
                case string v: return ToBytes(v, slidingExpiration);
                case byte[] v: return ToBytes(new ArraySegment<byte>(v), slidingExpiration);
                case System.Net.IPAddress v: return ToBytes(v, slidingExpiration);
                default: return null;
            }
#else
            return value switch
            {
                string v => ToBytes(v, slidingExpiration),
                byte[] v => ToBytes(new ReadOnlyMemory<byte>(v), slidingExpiration),
                System.Net.IPAddress v => ToBytes(v, slidingExpiration),
                _ => null
            };
#endif
        }

        /// <summary>
        /// 将缓存项的过期类型和数据类型赋值给 <c><paramref name="data"/>[0]</c>，
        /// 如果 <paramref name="slidingExpiration"/> 非 <c>null</c> 则将表示的时间（单位：毫秒）写入到 <c><paramref name="data"/>[1~4]</c>。
        /// </summary>
        /// <param name="data">缓存项。</param>
        /// <param name="slidingExpiration">滑动过期时间。</param>
        /// <param name="dataType">在 <see cref="RedisCache"/> 内部定义的基础数据类型索引。</param>
        /// <exception cref="ArgumentOutOfRangeException">如果 <paramref name="slidingExpiration"/> 表示的时间范围超出 10~4294967295 毫秒之间时引发此异常。</exception>
        private void SetDataTypeAndExpirationType(byte[] data, TimeSpan? slidingExpiration, byte dataType)
        {
            if (slidingExpiration.HasValue)
            {
                long totalMilliseconds = (long)slidingExpiration.Value.TotalMilliseconds;
                if (totalMilliseconds < 10L || totalMilliseconds > uint.MaxValue) throw new ArgumentOutOfRangeException(nameof(slidingExpiration), String.Format(Common.Properties.Localization.the_value_ranges_from_XXX_to_XXX_milliseconds, 10, uint.MaxValue));

                data[0] = (byte)(ExpirationTypeMask | dataType);
#if NETSTANDARD2_0
                BitConverter.GetBytes((uint)totalMilliseconds).CopyTo(data, 1);
#else
                BitConverter.TryWriteBytes(new Span<byte>(data, 1, 4), (uint)totalMilliseconds);
#endif
            }
            else
            {
                data[0] = (byte)(~ExpirationTypeMask & dataType);
            }
        }

        /// <summary>
        /// 如果流的长度达到压缩阈值则对流进行压缩。
        /// </summary>
        /// <param name="buffer">要压缩的数据，数据格式：第 1 个字节存储标识信息，第 2~4 个字节存 <paramref name="buffer"/> 的字节数。</param>
        private MemoryStream CompressHandler(byte[] buffer)
        {
            //流的最高位为 1 表压缩过的流，0表示位压缩过的流
            buffer[0] |= 0x80;

            MemoryStream compressed = new MemoryStream(buffer.Length);
            compressed.Write(buffer, 0, 5); //前 5 个字节存储的数据：1：标识字节，2~4：压缩前的 byte 数。

            using (MemoryStream memStream = new MemoryStream(buffer))
            {
                memStream.Position = 5L;
                Compression.Compress(memStream, compressed);
            }

            compressed.Position = 0L;
            return compressed;
        }



        /// <summary>
        /// 解压缩。
        /// </summary>
        /// <param name="value">要解压缩的数据，数据格式：第 1 个字节存储标识信息，第 2~4 字节存 <paramref name="buffer"/> 的字节数。</param>
        /// <returns>解压缩后的数据。返回的数据不包括 <paramref name="value"/> 中第 2~4 字节的数据（表示压缩前的字节长度）。</returns>
        private byte[] DecompressHandler(byte[] value)
        {
            byte[] result = new byte[BitConverter.ToInt32(value, 1) - 4];

            result[0] = (byte)(value[0] & 0x7f);

            using (MemoryStream memStream = new MemoryStream(value))
            {
                using (MemoryStream decompressed = new MemoryStream(result))
                {
                    memStream.Position = 5L;
                    decompressed.Position = 1L;
                    Compression.Decompression(memStream, decompressed);
                }
            }

            return result;
        }

        /// <summary>
        /// 类型名称处理程序，防止强名时因版本号变更导致缓存项无法还原为类型的问题。
        /// </summary>
        /// <param name="t">类型对象</param>
        /// <returns>返回的类型名称仅包括“命名空间”、“类型名称”、“程序集名称”。不包括版本号信息、公钥信息、区域信息。</returns>
        private string TypeNameHandler(Type t)
        {
            if (t.IsGenericType)
            {
                StringBuilder typeName = new StringBuilder(t.AssemblyQualifiedName.Substring(0, t.AssemblyQualifiedName.IndexOf('[')));
                typeName.Append('[');
                foreach (var type in t.GetGenericArguments())
                {
                    typeName.Append('[');
                    typeName.Append(type.AssemblyQualifiedName.Substring(0, type.AssemblyQualifiedName.IndexOf(',', type.FullName.Length + 1)));
                    typeName.Append("],");
                }
                typeName.Length--;
                typeName.Append(']');
                typeName.Append(t.AssemblyQualifiedName.Substring(t.FullName.Length, t.AssemblyQualifiedName.IndexOf(',', t.FullName.Length + 1) - t.FullName.Length));

                return typeName.ToString();
            }
            else
            {
                return t.AssemblyQualifiedName.Substring(0, t.AssemblyQualifiedName.IndexOf(',', t.FullName.Length + 1));
            }
        }




        private byte[] ToBytes(bool value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[6];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[2];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[6];
                span = new Span<byte>(result, 5, 1);
            }
            else
            {
                result = new byte[2];
                span = new Span<byte>(result, 1, 1);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, BOOLEAN_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(byte value, TimeSpan? slidingExpiration)
        {
            byte[] result;
            if (slidingExpiration.HasValue)
            {
                result = new byte[6];
                result[5] = value;
            }
            else
            {
                result = new byte[] { byte.MinValue, value };
            }
            SetDataTypeAndExpirationType(result, slidingExpiration, BYTE_TYPE_INDEX);
            return result;

        }

        private byte[] ToBytes(sbyte value, TimeSpan? slidingExpiration)
        {
            byte[] result;
            if (slidingExpiration.HasValue)
            {
                result = new byte[6];
                result[5] = unchecked((byte)value);
            }
            else
            {
                result = new byte[] { byte.MinValue, unchecked((byte)value) };
            }
            SetDataTypeAndExpirationType(result, slidingExpiration, SBYTE_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(char value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[7];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[3];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[7];
                span = new Span<byte>(result, 5, 2);
            }
            else
            {
                result = new byte[3];
                span = new Span<byte>(result, 1, 2);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, CHAR_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(short value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[7];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[3];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[7];
                span = new Span<byte>(result, 5, 2);
            }
            else
            {
                result = new byte[3];
                span = new Span<byte>(result, 1, 2);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, INT16_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(ushort value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[7];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[3];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[7];
                span = new Span<byte>(result, 5, 2);
            }
            else
            {
                result = new byte[3];
                span = new Span<byte>(result, 1, 2);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, UINT16_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(int value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[9];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[5];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[9];
                span = new Span<byte>(result, 5, 4);
            }
            else
            {
                result = new byte[5];
                span = new Span<byte>(result, 1, 4);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, INT32_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(uint value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[9];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[5];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[9];
                span = new Span<byte>(result, 5, 4);
            }
            else
            {
                result = new byte[5];
                span = new Span<byte>(result, 1, 4);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, UINT32_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(long value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[9];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                span = new Span<byte>(result, 5, 8);
            }
            else
            {
                result = new byte[9];
                span = new Span<byte>(result, 1, 8);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, INT64_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(ulong value, TimeSpan? slidingExpiration)
        {
            byte[] result;
#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[9];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                span = new Span<byte>(result, 5, 8);
            }
            else
            {
                result = new byte[9];
                span = new Span<byte>(result, 1, 8);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, UINT64_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(float value, TimeSpan? slidingExpiration)
        {
            byte[] result;
#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[9];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[5];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[9];
                span = new Span<byte>(result, 5, 4);
            }
            else
            {
                result = new byte[5];
                span = new Span<byte>(result, 1, 4);
            }
            BitConverter.TryWriteBytes(span, value);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, SINGLE_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(double value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                BitConverter.GetBytes(value).CopyTo(result, 5);
            }
            else
            {
                result = new byte[9];
                BitConverter.GetBytes(value).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                span = new Span<byte>(result, 5, 8);
            }
            else
            {
                result = new byte[9];
                span = new Span<byte>(result, 1, 8);
            }
            BitConverter.TryWriteBytes(span, value);
#endif

            SetDataTypeAndExpirationType(result, slidingExpiration, DOUBLE_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(decimal value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[21];
                value.GetBytes().CopyTo(result, 5);
            }
            else
            {
                result = new byte[17];
                value.GetBytes().CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[21];
                span = new Span<byte>(result, 5, 16);
            }
            else
            {
                result = new byte[17];
                span = new Span<byte>(result, 1, 16);
            }
            value.TryWriteBytes(span);
#endif

            SetDataTypeAndExpirationType(result, slidingExpiration, DECIMAL_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(Guid value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[21];
                value.ToByteArray().CopyTo(result, 5);
            }
            else
            {
                result = new byte[17];
                value.ToByteArray().CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[21];
                span = new Span<byte>(result, 5, 16);
            }
            else
            {
                result = new byte[17];
                span = new Span<byte>(result, 1, 16);
            }
            value.TryWriteBytes(span);
#endif

            SetDataTypeAndExpirationType(result, slidingExpiration, GUID_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(TimeSpan value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                BitConverter.GetBytes(value.Ticks).CopyTo(result, 5);
            }
            else
            {
                result = new byte[9];
                BitConverter.GetBytes(value.Ticks).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                span = new Span<byte>(result, 5, 8);
            }
            else
            {
                result = new byte[9];
                span = new Span<byte>(result, 1, 8);
            }
            BitConverter.TryWriteBytes(span, value.Ticks);
#endif

            SetDataTypeAndExpirationType(result, slidingExpiration, TIMESPAN_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(DateTime value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                BitConverter.GetBytes((ulong)value.Ticks | (ulong)value.Kind << 62).CopyTo(result, 5);
            }
            else
            {
                result = new byte[9];
                BitConverter.GetBytes((ulong)value.Ticks | (ulong)value.Kind << 62).CopyTo(result, 1);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[13];
                span = new Span<byte>(result, 5, 8);
            }
            else
            {
                result = new byte[9];
                span = new Span<byte>(result, 1, 8);
            }
            BitConverter.TryWriteBytes(span, ((ulong)value.Ticks | (ulong)value.Kind << 62));
#endif

            SetDataTypeAndExpirationType(result, slidingExpiration, DATETIME_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(DateTimeOffset value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (slidingExpiration.HasValue)
            {
                result = new byte[21];
                BitConverter.GetBytes(value.Ticks).CopyTo(result, 5);
                BitConverter.GetBytes(value.Offset.Ticks).CopyTo(result, 13);
            }
            else
            {
                result = new byte[17];
                BitConverter.GetBytes(value.Ticks).CopyTo(result, 1);
                BitConverter.GetBytes(value.Offset.Ticks).CopyTo(result, 9);
            }
#else
            Span<byte> span;
            if (slidingExpiration.HasValue)
            {
                result = new byte[21];
                span = new Span<byte>(result, 5, 16);
                BitConverter.TryWriteBytes(span.Slice(8), value.Offset.Ticks);
            }
            else
            {
                result = new byte[17];
                span = new Span<byte>(result, 1, 16);
                BitConverter.TryWriteBytes(span.Slice(8), value.Offset.Ticks);
            }
            BitConverter.TryWriteBytes(span, value.Ticks);
#endif

            SetDataTypeAndExpirationType(result, slidingExpiration, DATETIMEOFFSET_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(ArraySegment<byte> value, TimeSpan? slidingExpiration)
        {
            byte[] result;
            if (slidingExpiration.HasValue)
            {
                if ((value.Count + 5) < _compressionThreshold)
                {
                    result = new byte[value.Count + 5];
                    Array.Copy(value.Array, value.Offset, result, 5, value.Count);
                }
                else
                {
                    result = new byte[value.Count + 9];
                    Array.Copy(value.Array, value.Offset, result, 9, value.Count);
                }
            }
            else
            {
                if ((value.Count + 1) < _compressionThreshold)
                {
                    result = new byte[value.Count + 1];
                    Array.Copy(value.Array, value.Offset, result, 1, value.Count);
                }
                else
                {
                    result = new byte[value.Count + 5];
                    Array.Copy(value.Array, value.Offset, result, 5, value.Count);
                }
            }
            SetDataTypeAndExpirationType(result, slidingExpiration, BYTEARRAY_TYPE_INDEX);
            return result;
        }

#if !NETSTANDARD2_0
        private byte[] ToBytes(ReadOnlyMemory<byte> value, TimeSpan? slidingExpiration)
        {
            byte[] result;
            if (slidingExpiration.HasValue)
            {
                if ((value.Length + 5) < _compressionThreshold)
                {
                    result = new byte[value.Length + 5];
                    value.CopyTo(new Memory<byte>(result, 5, result.Length - 5));
                }
                else
                {
                    result = new byte[value.Length + 9];
                    value.CopyTo(new Memory<byte>(result, 9, result.Length - 9));
                }
            }
            else
            {
                if ((value.Length + 1) < _compressionThreshold)
                {
                    result = new byte[value.Length + 1];
                    value.CopyTo(new Memory<byte>(result, 1, result.Length - 1));
                }
                else
                {
                    result = new byte[value.Length + 5];
                    value.CopyTo(new Memory<byte>(result, 5, result.Length - 5));
                }
            }
            SetDataTypeAndExpirationType(result, slidingExpiration, BYTEARRAY_TYPE_INDEX);
            return result;
        }
#endif

        private byte[] ToBytes(string value, TimeSpan? slidingExpiration)
        {
            byte[] result;
            int byteLength = Encoding.UTF8.GetByteCount(value);
            if (slidingExpiration.HasValue)
            {
                if ((byteLength + 5) < _compressionThreshold)
                {
                    result = new byte[byteLength + 5];
                    Encoding.UTF8.GetBytes(value, 0, value.Length, result, 5);
                }
                else
                {
                    result = new byte[byteLength + 9];
                    Encoding.UTF8.GetBytes(value, 0, value.Length, result, 9);
                }
            }
            else
            {
                if ((byteLength + 1) < _compressionThreshold)
                {
                    result = new byte[byteLength + 1];
                    Encoding.UTF8.GetBytes(value, 0, value.Length, result, 1);
                }
                else
                {
                    result = new byte[byteLength + 5];
                    Encoding.UTF8.GetBytes(value, 0, value.Length, result, 5);
                }
            }

            SetDataTypeAndExpirationType(result, slidingExpiration, STRING_TYPE_INDEX);
            return result;
        }

        private byte[] ToBytes(System.Net.IPAddress value, TimeSpan? slidingExpiration)
        {
            byte[] result;

#if NETSTANDARD2_0
            if (value.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                if (slidingExpiration.HasValue)
                {
                    result = new byte[21];
                    value.GetAddressBytes().CopyTo(result, 5);
                }
                else
                {
                    result = new byte[17];
                    value.GetAddressBytes().CopyTo(result, 1);
                }
            }
            else
            {
                if (slidingExpiration.HasValue)
                {
                    result = new byte[9];
                    value.GetAddressBytes().CopyTo(result, 5);
                }
                else
                {
                    result = new byte[5];
                    value.GetAddressBytes().CopyTo(result, 1);
                }
            }
#else
            Span<byte> span;
            if (value.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                if (slidingExpiration.HasValue)
                {
                    result = new byte[21];
                    span = new Span<byte>(result, 5, 16);
                }
                else
                {
                    result = new byte[17];
                    span = new Span<byte>(result, 1, 16);
                }
            }
            else
            {
                if (slidingExpiration.HasValue)
                {
                    result = new byte[9];
                    span = new Span<byte>(result, 5, 4);
                }
                else
                {
                    result = new byte[5];
                    span = new Span<byte>(result, 1, 4);
                }
            }
            value.TryWriteBytes(span, out _);
#endif
            SetDataTypeAndExpirationType(result, slidingExpiration, IP_TYPE_INDEX);
            return result;
        }
    }
}
