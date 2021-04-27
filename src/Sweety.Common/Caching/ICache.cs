/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  接口定义了使用缓存的基本的存取操作。
 *
 * 
 * Members Index:
 *      interface
 *          ICache
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Caching
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 缓存服务的基本存/取操作接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 将缓存项添加到缓存中，缓存的过期时间由缓存的实现来决定。如果缓存中已有相同 <paramref name="key"/> 的缓存项，则放弃添加缓存项。
        /// </summary>
        /// <param name="key">该缓存项的唯一标识符。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <returns>如果添加成功则为 <c>true</c>；如果缓存中已存在具有与 <paramref name="key"/> 相同的键的项，则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 <c>null</c> 时引发此异常。</exception>
        bool Add(string key, object value);
        /// <summary>
        /// 将缓存项添加到缓存中，并指定在固定的日期时间之后过期。如果缓存中已有相同 <paramref name="key"/> 的缓存项，则放弃添加缓存项。
        /// </summary>
        /// <param name="key">该缓存项的唯一标识符。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <param name="absoluteExpiration">在指定日期和时间之后从缓存中移除此缓存项。</param>
        /// <returns>如果添加成功则为 <c>true</c>；如果缓存中已存在具有与 <paramref name="key"/> 相同的键的项，则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 <c>null</c> 时引发此异常。</exception>
        bool Add(string key, object value, DateTimeOffset absoluteExpiration);
        /// <summary>
        /// 将缓存项添加到缓存中，并指定可调的过期时间。如果缓存中已有相同 <paramref name="key"/> 的缓存项，则放弃添加缓存项。
        /// </summary>
        /// <param name="key">该缓存项的唯一标识符。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <param name="slidingExpiration">在此时间段内此缓存项未被访问则从缓存中移除此缓存项。</param>
        /// <returns>如果添加成功则为 <c>true</c>；如果缓存中已存在具有与 <paramref name="key"/> 相同的键的项，则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 <c>null</c> 时引发此异常。</exception>
        /// <exception cref="NotSupportedException">当缓存的实现不支持可调过期时引发此异常。</exception>
        bool Add(string key, object value, TimeSpan slidingExpiration);


        /// <summary>
        /// 向缓存中插入缓存项，缓存的过期时间由缓存的实现决定。如果缓存中已有相同 <paramref name="key"/> 的缓存项，则直接覆盖已有缓存项。
        /// </summary>
        /// <param name="key">要插入的缓存项的唯一标识符。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 <c>null</c> 时引发此异常。</exception>
        void Set(string key, object value);
        /// <summary>
        /// 向缓存中插入在固定的日期时间之后过期的缓存项。如果缓存中已有相同 <paramref name="key"/> 的缓存项，则直接覆盖已有缓存项。
        /// </summary>
        /// <param name="key">要插入的缓存项的唯一标识符。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <param name="absoluteExpiration">在指定日期和时间之后从缓存中移除此缓存项。</param>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 <c>null</c> 时引发此异常。</exception>
        void Set(string key, object value, DateTimeOffset absoluteExpiration);
        /// <summary>
        /// 向缓存中插入基于可调过期时间的缓存项。如果缓存中已有相同 <paramref name="key"/> 的缓存项，则直接覆盖已有缓存项。
        /// </summary>
        /// <param name="key">要插入的缓存项的唯一标识符。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <param name="slidingExpiration">在此时间段内此缓存项未被访问则从缓存中移除此缓存项。</param>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 <c>null</c> 时引发此异常。</exception>
        /// <exception cref="NotSupportedException">当缓存的实现不支持可调过期时引发此异常。</exception>
        void Set(string key, object value, TimeSpan slidingExpiration);



        /// <summary>
        /// 从缓存中移除缓存项。
        /// </summary>
        /// <param name="key">要移除的缓存项的唯一标识符。</param>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 <c>null</c> 时引发此异常。</exception>
        void Remove(string key);
        /// <summary>
        /// 从缓存中移除一组缓存项。
        /// </summary>
        /// <param name="keys">缓存项的唯一标识符的集合。</param>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        void Remove(params string[] keys);
        /// <summary>
        /// 从缓存中移除一组缓存项。
        /// </summary>
        /// <param name="keys">缓存项的键的集合。</param>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        void Remove(IEnumerable<string> keys);



        /// <summary>
        /// 检查缓存中是否存在指定的缓存键。
        /// </summary>
        /// <param name="key">要检查的缓存项的唯一标识符。</param>
        /// <returns>如果缓存中包含 <paramref name="key"/> 指定的键则为 <c>true</c>；否则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 <c>null</c> 时引发此异常。</exception>
        bool Contains(string key);

        /// <summary>
        /// 检查缓存中是否存在指定的所有缓存键。
        /// </summary>
        /// <param name="keys">缓存项的唯一标识符的集合。</param>
        /// <returns>如果缓存中包含 <paramref name="keys"/> 中指定的任意一个键则为 <c>true</c>；否则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        bool ContainsAll(params string[] keys);
        /// <summary>
        /// 检查缓存中是否存在指定的所有缓存键。
        /// </summary>
        /// <param name="keys">缓存项的唯一标识符的集合。</param>
        /// <returns>如果缓存中包含 <paramref name="keys"/> 中指定的任意一个键则为 <c>true</c>；否则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        bool ContainsAll(IEnumerable<string> keys);

        /// <summary>
        /// 检查缓存中是否存在指定的任意一个缓存键。
        /// </summary>
        /// <param name="keys">缓存项的唯一标识符的集合。</param>
        /// <returns>如果缓存中包含 <paramref name="keys"/> 中指定的任意一个键则为 <c>true</c>；否则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        bool ContainsAny(params string[] keys);
        /// <summary>
        /// 检查缓存中是否存在指定的任意一个缓存键。
        /// </summary>
        /// <param name="keys">缓存项的唯一标识符的集合。</param>
        /// <returns>如果缓存中包含 <paramref name="keys"/> 中指定的任意一个键则为 <c>true</c>；否则为 <c>false</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        bool ContainsAny(IEnumerable<string> keys);



        /// <summary>
        /// 从缓存中返回一个缓存项。
        /// </summary>
        /// <param name="key">要获取的缓存项的唯一标识符。</param>
        /// <returns>如果缓存项存在，则返回缓存项；否则返回 <c>null</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 <c>null</c> 时引发此异常。</exception>
        object Get(string key);
        /// <summary>
        /// 从缓存中返回一个缓存项。
        /// </summary>
        /// <typeparam name="T">缓存项的数据类型。</typeparam>
        /// <param name="key">要获取的缓存项的唯一标识符。</param>
        /// <returns>如果缓存项存在并且类型为 <typeparamref name="T"/> 指定的类型或能转换成 <typeparamref name="T"/> 类型，则返回缓存项；否则返回 <c>null</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="key"/> 等于 <see cref="String.Empty"/> 时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 <c>null</c> 时引发此异常。</exception>
        T Get<T>(string key);
        /// <summary>
        /// 从缓存中返回一组缓存项。
        /// </summary>
        /// <param name="keys">要获取的缓存项的唯一标识符的集合。</param>
        /// <returns>与指定的键对应的一组缓存项。如果指定的缓存键不存在，则忽略该缓存键；如果都不存在则返回 <c>null</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        IDictionary<string, object> GetValues(string[] keys);
        /// <summary>
        /// 从缓存中返回一组缓存项。
        /// </summary>
        /// <param name="keys">要获取的缓存项的唯一标识符的集合。</param>
        /// <returns>与指定的键对应的一组缓存项。如果指定的缓存键不存在，则忽略该缓存键；如果都不存在则返回 <c>null</c>。</returns>
        /// <exception cref="ArgumentException">当 <paramref name="keys"/> 不包含任何元素或包含值为 <c>null</c> 或 <see cref="String.Empty"/> 的元素时引发此异常。</exception>
        /// <exception cref="ArgumentNullException">当 <paramref name="keys"/> 为 <c>null</c> 时引发此异常。</exception>
        IDictionary<string, object> GetValues(IEnumerable<string> keys);
    }
}
