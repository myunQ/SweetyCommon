/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          GenericEqualityComparer
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Comparer
{
    using System;
    using System.Collections.Generic;



    /// <summary>
    /// 通用对象比较器。
    /// </summary>
    /// <typeparam name="T">要进行比较的对象类型。</typeparam>
    /// <example>
    /// 
    /// var comparer = new GenericEqualityComparer<CustomObject>(
    ///         (x, y) => { return x.Member == y.Member; },
    ///         obj => { return obj.Member.GetHashCode(); }
    ///     );
    ///     
    /// </example>
    public class GenericEqualityComparer<T> : EqualityComparer<T>
    {
        Func<T, int> _getHashcode;
        Func<T, T, bool> _equals;

        /// <summary>
        /// 创建通用对象比较类的实例。
        /// </summary>
        /// <param name="equals">比较两个对象的方法。</param>
        /// <param name="getHashcode">获取哈希码的方法。</param>
        public GenericEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashcode)
        {
            this._equals = equals;
            this._getHashcode = getHashcode;
        }

        public override bool Equals(T x, T y)
        {
            if (this._equals == null)
            {
                throw new NotImplementedException();
            }

            return this._equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            if (this._getHashcode == null)
            {
                throw new NotImplementedException();
            }

            return this._getHashcode(obj);
        }
    }
}
