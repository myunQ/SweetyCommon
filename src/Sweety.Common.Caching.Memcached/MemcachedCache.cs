/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 *
 *     Memcached 客户端使用的是 博客园 提供的 EnyimMemcachedCore ，https://www.cnblogs.com/cmt/p/6163455.html
 * 
 * Members Index:
 *      class MemcachedCache
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Caching
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    using Enyim.Caching;
    using Enyim.Caching.Configuration;
    using Enyim.Caching.Memcached;

    using Newtonsoft.Json;

    using Sweety.Common.Compression;


    /// <summary>
    /// 分布式缓存服务 <c>Memcached</c> 的操作类。
    /// </summary>
    public class MemcachedCache : ICache
    {
        /// <summary>
        /// 每个 chunk 键的后缀
        /// </summary>
        const string CHUNK_KEY_SUFFIX = "-PART";


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

        /// <summary>
        /// 缓存内容长度大于此值则压缩后在存储
        /// </summary>
        long _compressionThreshold = 102400L; //100KB
        /// <summary>
        /// 缓存的内容长度超过此值将会被切分成多块进行存储。
        /// </summary>
        long _chunkSizeThreshold = 252400L; //byte 预留出 key 和 flags 的空间

        ICompression _compression = null;

        IMemcachedClient _cache;


        public MemcachedCache(IConfiguration configuration)
        {
            var nullLoggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();
            _cache = new MemcachedClient(nullLoggerFactory, new MemcachedClientConfiguration(nullLoggerFactory, null, configuration));
        }

        public MemcachedCache(IMemcachedClientConfiguration configuration)
        {
            _cache = new MemcachedClient(new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory(), configuration);
        }

        public MemcachedCache(IOptions<MemcachedClientOptions> options)
        {
            var nullLoggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();
            _cache = new MemcachedClient(nullLoggerFactory, new MemcachedClientConfiguration(nullLoggerFactory, options));
        }

        public MemcachedCache(IEnumerable<(string host, int port)> endPoints)
        {
            MemcachedClientOptions options = new MemcachedClientOptions();
            foreach (var endPoint in endPoints)
            {
                options.AddServer(endPoint.host, endPoint.port);
            }

            var nullLoggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();

            _cache = new MemcachedClient(nullLoggerFactory, new MemcachedClientConfiguration(nullLoggerFactory, options));
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


        public bool Add(string key, object value)
        {
            return Store(StoreMode.Add, key, value, TimeSpan.Zero);
        }

        public bool Add(string key, object value, DateTimeOffset absoluteExpiration)
        {
            // 如果换成 absoluteExpiration.Subtract(DateTimeOffset.Now) 则会导致 TotalSeconds 为 (int)XXX.999999 == XXX 这样会导致5秒后超时的实际4秒后就超时了。
            return Store(StoreMode.Add, key, value, TimeSpan.FromSeconds(absoluteExpiration.ToUnixTimeSeconds() - DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        }

        public bool Add(string key, object value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }




        public void Set(string key, object value)
        {
            Store(StoreMode.Set, key, value, TimeSpan.Zero);
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration)
        {
            // 如果换成 absoluteExpiration.Subtract(DateTimeOffset.Now) 则会导致 TotalSeconds 为 (int)XXX.999999 == XXX 这样会导致5秒后超时的实际4秒后就超时了。
            Store(StoreMode.Set, key, value, TimeSpan.FromSeconds(absoluteExpiration.ToUnixTimeSeconds() - DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        }

        public void Set(string key, object value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }



        public void Remove(string key)
        {
            CheckKey(key);

            if (_cache.Remove(key))
            {
                int i = 1;
                while (_cache.Remove(key + CHUNK_KEY_SUFFIX + i))
                {
                    i++;
                }
            }
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

            foreach (var key in keys) Remove(key);
        }



        public bool Contains(string key)
        {
            CheckKey(key);

            return _cache.TryGet(key, out _);
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
                if (!_cache.TryGet(key, out _)) return false;
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
                if (_cache.TryGet(key, out _)) return true;
            }

            return false;
        }



        public object Get(string key)
        {
            CheckKey(key);

            if (_cache.TryGet(key, out var result))
            {
                return BeforeReturningValueHandler(key, result);
            }
            return result;
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

            foreach (var key in keys)
            {
                if (String.IsNullOrEmpty(key)) throw new ArgumentException();
            }
            
            IDictionary<string, object> result = new Dictionary<string, object>(keys.Count());
            foreach (var key in keys)
            {
                object item = Get(key);
                if (item != null)
                {
                    result.Add(key, item);
                }
            }

            return result;
        }









        /// <summary>
        /// 把数据进行缓存
        /// </summary>
        /// <remarks>
        /// <c>Memcached Client</c> 存在超时时间计算的BUG，如果 <paramref name="validFor"/> 使用 <see cref="DateTime"/> 类型，
        /// 在 <c>Memcached Client</c> 内部将被转换成 <c>UnixTimestamp</c>，
        /// 但 <c>Memcached Server</c> 接受的过期时间是经过多少秒之后过期，且过期时间取值范围是 0 ~ 60 * 60 * 24 * 30 (即 30 天）。0 表示不限。
        /// 过期时间超过取值范围的 <c>Memcached Server</c> 将直接丢弃缓存项。
        /// </remarks>
        /// <param name="mode">存储模式</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">要缓存的数据</param>
        /// <param name="validFor">超时时间</param>
        /// <returns>缓存是否成功。true表示缓存成功。</returns>
        private bool Store(StoreMode mode, string key, object value, TimeSpan validFor)
        {
            this.CheckKey(key);

            var data = this.BeforeStorageValueHandler(value);

            bool result = _cache.Store(mode, key, data[0], validFor);

            if (result && data.Length > 1)
            {
                string key2 = key + CHUNK_KEY_SUFFIX;
                for (int i = data.Length - 1; i > 0; i--)
                {
                    if (!_cache.Store(mode, key2 + i.ToString(), data[i], validFor))
                    {
                        //将结果置为 false，并删除已缓存的 chunk。
                        result = false;
                        this.Remove(key);

                        for (int j = data.Length - 1; j > i; j--)
                        {
                            this.Remove(key2 + j.ToString());
                        }

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 把数据进行缓存
        /// </summary>
        /// <remarks>
        /// <c>Memcached Client</c> 存在超时时间计算的BUG，如果 <paramref name="validFor"/> 使用 <see cref="DateTime"/> 类型，
        /// 在 <c>Memcached Client</c> 内部将被转换成 <c>UnixTimestamp</c>，
        /// 但 <c>Memcached Server</c> 接受的过期时间是经过多少秒之后过期，且过期时间取值范围是 0 ~ 60 * 60 * 24 * 30 (即 30 天）。0 表示不限。
        /// 过期时间超过取值范围的 <c>Memcached Server</c> 将直接丢弃缓存项。
        /// </remarks>
        /// <param name="key">缓存键</param>
        /// <param name="value">要缓存的数据</param>
        /// <param name="version">缓存版本，只有与缓存服务器的值一致时才会修改缓存；传入 0 时始终进行更新缓存操作并返回新的版本值</param>
        /// <param name="validFor">超时时间</param>
        /// <returns>缓存是否成功。true表示缓存成功。</returns>
        private bool CheckAndStore(string key, object value, ref ulong version, TimeSpan validFor)
        {
            this.CheckKey(key);
            StoreMode mode = StoreMode.Set;
            var data = this.BeforeStorageValueHandler(value);

            CasResult<bool> result = _cache.Cas(mode, key, data[0], validFor, version);

            if (result.Result)
            {
                version = result.Cas;

                if (data.Length > 1)
                {
                    string key2 = key + CHUNK_KEY_SUFFIX;
                    for (int i = data.Length - 1; i > 0; i--)
                    {
                        if (!_cache.Store(mode, key2 + i.ToString(), data[i], validFor))
                        {
                            //将结果置为 false，并删除已缓存的 chunk。
                            result.Result = false;
                            this.Remove(key);

                            for (int j = data.Length - 1; j > i; j--)
                            {
                                this.Remove(key2 + j.ToString());
                            }

                            break;
                        }
                    }
                }
            }

            return result.Result;
        }


        /// <summary>
        /// 缓存之前对缓存内容进行处理
        /// </summary>
        /// <param name="value">要缓存的内容</param>
        /// <returns>经过处理后的缓存内容</returns>
        private object[] BeforeStorageValueHandler(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Type t = value.GetType();
            var typeCode = Type.GetTypeCode(t);

            //如果是字符串，并且长度超过 65536 个字符（视英文与中文字符数量，使用UTF-8编码占用空间在 64K 至 192K 之间），则当作对象一样进行压缩、分割处理。
            if ((t.IsValueType && typeCode != TypeCode.Object) || (typeCode == TypeCode.String && ((string)value).Length < 65537)) return new Object[] { value };

            //流的最高位表示是否经过压缩，1压缩、0未压缩。
            //第一个字节的右边七个位与第二个字节表示内容的类型完整名称（命名空间.类型名, 程序集名称）字节长度。
            //第三至第四个字节表示内容被切分成多少个 chunk 进行存储。
            //第五位开始是内容。
            MemoryStream memStream = new MemoryStream();
            memStream.Position = 4L;
            byte[] buffer = value as byte[];

            if (buffer != null)
            {
                memStream.Write(buffer, 0, buffer.Length);
            }
            else if (value is ArraySegment<byte>)
            {
                var tmp = (ArraySegment<byte>)value;

                memStream.Write(tmp.Array, tmp.Offset, tmp.Count);
            }
            else if (typeCode == TypeCode.Object || typeCode == TypeCode.String)
            {
                buffer = Encoding.UTF8.GetBytes(this.TypeNameHandler(t));
                memStream.Write(buffer, 0, buffer.Length);

                memStream.Position = 0L;
                memStream.Write(BitConverter.GetBytes((ushort)buffer.Length), 0, 2);

                if (typeCode == TypeCode.String)
                {
                    buffer = Encoding.UTF8.GetBytes(value.ToString());
                }
                else
                {
                    buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, Formatting.None, __jsonSerSettings));
                }

                memStream.Seek(0L, SeekOrigin.End);
                memStream.Write(buffer, 0, buffer.Length);
            }

            this.CompressHandler(memStream);

            return this.Split(memStream);
        }

        /// <summary>
        /// 获取缓存内容时，在返回内容之前对内容进行处理
        /// </summary>
        /// <param name="key">当前缓存键</param>
        /// <param name="value">要返回的缓存内容</param>
        /// <returns>经过处理后的缓存的内容</returns>
        private object BeforeReturningValueHandler(string key, object value)
        {
            if (value == null) return null;

            var tmp = value as byte[];
            if (tmp == null) return value;

            MemoryStream memStream = new MemoryStream();
            memStream.Write(tmp, 0, tmp.Length);
            memStream = this.Merge(key, memStream);

            if (memStream == null) return null;

            this.DecompressHandler(memStream);

            int typeLength = BitConverter.ToUInt16(memStream.GetBuffer(), 0);
            memStream.Position = 4L;

            if (typeLength == 0)
            {
                byte[] result = new byte[tmp.Length - 4];
                memStream.Read(result, 0, result.Length);
                return result;
            }
            else
            {
                byte[] buffer = new byte[typeLength];
                memStream.Read(buffer, 0, buffer.Length);
                Type t = Type.GetType(Encoding.UTF8.GetString(buffer), true);

                buffer = new byte[memStream.Length - memStream.Position];
                memStream.Read(buffer, 0, buffer.Length);

                if (Type.GetTypeCode(t) == TypeCode.String)
                {
                    return Encoding.UTF8.GetString(buffer);
                }
                else
                {
                    return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(buffer), t, __jsonSerSettings);
                }
            }
        }
        /// <summary>
        /// 获取缓存内容时，在返回内容之前对内容进行处理
        /// </summary>
        /// <typeparam name="T">预返回的内容类型</typeparam>
        /// <param name="key">当前缓存键</param>
        /// <param name="value">要返回的缓存内容</param>
        /// <returns>经过处理后的缓存的内容</returns>
        private T BeforeReturningValueHandler<T>(string key, object value)
        {
            if (value == null) return default(T);

            return (T)this.BeforeReturningValueHandler(key, value);
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
        /// 如果流的长度达到压缩阈值则对流进行压缩。
        /// </summary>
        /// <param name="memStream">数据流</param>
        private void CompressHandler(MemoryStream memStream)
        {
            if (memStream.Length > _compressionThreshold)
            {
                using (MemoryStream compressed = new MemoryStream())
                {
                    //流的最高位为 1 表压缩过的流，0表示位压缩过的流
                    memStream.GetBuffer()[BitConverter.IsLittleEndian ? 1 : 0] |= 0x80;

                    memStream.Position = 4L;

                    this.Compression.Compress(memStream, compressed);

                    compressed.Position = 0L;
                    memStream.Position = 4L;
                    memStream.SetLength(memStream.Position + compressed.Length);
                    compressed.CopyTo(memStream);
                }
            }
        }

        /// <summary>
        /// 如果是压缩过（流的最高位为 1 表示压缩过的流，0表示未压缩过的流）的流则进行解压缩处理。
        /// </summary>
        /// <param name="memStream">数据流</param>
        private void DecompressHandler(MemoryStream memStream)
        {
            if (memStream.Position > 0L) memStream.Position = BitConverter.IsLittleEndian ? 1L : 0L;

            if ((memStream.ReadByte() & 0x0080) == 0x0080)
            {
                using (MemoryStream decompressed = new MemoryStream())
                {
                    memStream.GetBuffer()[BitConverter.IsLittleEndian ? 1 : 0] &= 0x7F;

                    memStream.Position = 4L;

                    this.Compression.Decompression(memStream, decompressed);

                    decompressed.Position = 0L;
                    memStream.Position = 4L;
                    memStream.SetLength(memStream.Position + decompressed.Length);
                    decompressed.CopyTo(memStream);
                }
            }
        }

        /// <summary>
        /// 如果流的长度达到分块存储的阈值则对流进行分块。
        /// </summary>
        /// <param name="memStream">数据流</param>
        /// <returns>数据块</returns>
        private object[] Split(MemoryStream memStream)
        {
            memStream.Position = 2L;

            if (memStream.Length < _chunkSizeThreshold)
            {
                memStream.Write(BitConverter.GetBytes((ushort)1), 0, 2);
                return new object[] { memStream.ToArray() };
            }

            int chunkCount = 2;
            int chunkSize;
            int streamLength = checked((int)memStream.Length);

            while ((chunkSize = streamLength / chunkCount) > _chunkSizeThreshold)
            {
                chunkCount++;
            }

            int remainder = streamLength % chunkCount;
            if (remainder > 100) chunkCount++;


            object[] result = new object[chunkCount];
            memStream.Write(BitConverter.GetBytes(checked((ushort)chunkCount)), 0, 2);

            byte[] buffer = memStream.GetBuffer();
            int offset = 0;

            if (remainder > 100)
            {
                result[--chunkCount] = new ArraySegment<byte>(buffer, streamLength - remainder, remainder);
                remainder = 0;
            }

            result[0] = new ArraySegment<byte>(buffer, offset, chunkSize + remainder);
            offset = chunkSize + remainder;

            for (int i = 1; i < chunkCount; i++)
            {
                result[i] = new ArraySegment<byte>(buffer, offset, chunkSize);
                offset += chunkSize;
            }

            return result;
        }

        /// <summary>
        /// 如果数据被分块存储，则读取剩余块合并数据。
        /// </summary>
        /// <param name="key">当前缓存键</param>
        /// <param name="memStream">数据流</param>
        /// <returns>数据流，如果某块数据丢失则返回 null。</returns>
        private MemoryStream Merge(string key, MemoryStream memStream)
        {
            int chunkCount = BitConverter.ToUInt16(memStream.GetBuffer(), 2);
            if (chunkCount > 1)
            {
                byte[] chunk;
                object tmp;
                key += CHUNK_KEY_SUFFIX;
                memStream.Seek(0L, SeekOrigin.End);

                for (int i = 1; i < chunkCount; i++)
                {
                    if (!_cache.TryGet(key + i.ToString(), out tmp))
                    {
                        memStream.Dispose();
                        memStream = null;
                    }

                    chunk = (byte[])tmp;
                    memStream.Write(chunk, 0, chunk.Length);
                }
            }
            return memStream;
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
    }
}
