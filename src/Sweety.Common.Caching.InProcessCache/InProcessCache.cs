/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 *
 *     使用进程内的内存资源作为缓存容器。
 * 
 * Members Index:
 *      class InProcessCache
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Caching.Memory;
    


    /// <summary>
    /// 进程内缓存。
    /// </summary>
    public class InProcessCache : ICache
    {
        static readonly object __lock = new object();
        static InProcessCache __defaultInstance = null;

        IMemoryCache _cache;

        public InProcessCache()
        {
            _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        }

        /// <summary>
        /// 获取 <see cref="InProcessCache"/> 的默认实例。
        /// </summary>
        /// <remarks>
        /// 此属性是线程安全的。
        /// </remarks>
        public static InProcessCache Default
        {
            get
            {
                if (__defaultInstance == null)
                {
                    lock (__lock)
                    {
                        if (__defaultInstance == null) __defaultInstance = new InProcessCache();
                    }
                }
                return __defaultInstance;
            }
        }


        #region ICache interface implementation.
        public bool Add(string key, object value)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (key == String.Empty) throw new ArgumentException();

            if (_cache.TryGetValue(key, out _)) return false;

            using (var entry = _cache.CreateEntry(key))
            {
                entry.Value = value;
            }
            return true;
        }

        public bool Add(string key, object value, DateTimeOffset absoluteExpiration)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (key == String.Empty) throw new ArgumentException();

            if (_cache.TryGetValue(key, out _)) return false;

            using (var entry = _cache.CreateEntry(key))
            {
                entry.AbsoluteExpiration = absoluteExpiration;
                entry.Value = value;
            }
            return true;
        }

        public bool Add(string key, object value, TimeSpan slidingExpiration)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (key == String.Empty) throw new ArgumentException();

            if (_cache.TryGetValue(key, out _)) return false;

            using (var entry = _cache.CreateEntry(key))
            {
                entry.SlidingExpiration = slidingExpiration;
                entry.Value = value;
            }
            return true;
        }



        public void Set(string key, object value)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (key == String.Empty) throw new ArgumentException();

            using (var entry = _cache.CreateEntry(key))
            {
                entry.Value = value;
            }
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (key == String.Empty) throw new ArgumentException();

            using (var entry = _cache.CreateEntry(key))
            {
                entry.AbsoluteExpiration = absoluteExpiration;
                entry.Value = value;
            }
        }

        public void Set(string key, object value, TimeSpan slidingExpiration)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (key == String.Empty) throw new ArgumentException();

            using (var entry = _cache.CreateEntry(key))
            {
                entry.SlidingExpiration = slidingExpiration;
                entry.Value = value;
            }
        }



        public void Remove(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (key == String.Empty) throw new ArgumentException();

            _cache.Remove(key);
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

            foreach (var key in keys) _cache.Remove(key);
        }



        public bool Contains(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (key == String.Empty) throw new ArgumentException();

            return _cache.TryGetValue(key, out _);
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
                if (!_cache.TryGetValue(key, out _)) return false;
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
                if (_cache.TryGetValue(key, out _)) return true;
            }

            return false;
        }



        public object Get(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (key == String.Empty) throw new ArgumentException();

            _cache.TryGetValue(key, out var result);

            return result;
        }

        public T Get<T>(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (key == String.Empty) throw new ArgumentException();

            _cache.TryGetValue(key, out var result);

            return (T)result;
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
                if (_cache.TryGetValue(key, out var value))
                {
                    result.Add(key, value);
                }
            }

            return result;
        }
        #endregion ICache interface implementation.
    }
}
