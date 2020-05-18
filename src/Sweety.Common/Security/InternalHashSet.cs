/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  自定义一个内部Set类型，用于存储白名单的每一个元素（HTML标签和属性、样式表属性）。
 *  此Set对象可以表示所包含的元素是否是默认的，可以把Set设置位只读的。
 * 
 * Members Index:
 *      class
 *          InternalHashSet
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Security
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// <c>XSS</c> 防御允许的白名单集。
    /// </summary>
    internal class InternalHashSet : ISet<string>
    {
        /// <summary>
        /// <c>true</c> 表示 <see cref="_set"/> 是 <see cref="XssSanitizerBase"/> 里定义的默认集合，不要修改这些默认集合；
        /// </summary>
        bool _isDefault = false;
        bool _isReadOnly = false;
        ISet<string> _set;

        internal InternalHashSet()
        {
            _set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public InternalHashSet(IEnumerable<string> enumerable)
        {
            _set = new HashSet<string>(enumerable, StringComparer.OrdinalIgnoreCase);
        }


        internal bool IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                CheckIsReadOnly();

                _isDefault = value;
            }
        }

        /// <summary>
        /// 将集合设置为只读集合。
        /// </summary>
        internal void SetReadOnly()
        {
            _isReadOnly = true;
        }

        private void ChangeNonDefault()
        {
            if (_isDefault)
            {
                _isDefault = false;
                _set = new HashSet<string>(_set, StringComparer.OrdinalIgnoreCase);
            }
        }

        private void CheckIsReadOnly()
        {
            if (_isReadOnly) throw new NotSupportedException(Properties.Localization.the_read_only_collection_cannot_be_modified);
        }


        public bool Add(string item)
        {
            CheckIsReadOnly();

            ChangeNonDefault();

            return _set.Add(item);
        }

        public void ExceptWith(IEnumerable<string> other)
        {
            CheckIsReadOnly();

            if (other == null || !other.Any()) return;

            ChangeNonDefault();

            _set.ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<string> other)
        {
            CheckIsReadOnly();

            if (other == null || !other.Any()) return;

            ChangeNonDefault();

            _set.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<string> other)
        {
            return _set.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<string> other)
        {
            return _set.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<string> other)
        {
            return _set.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<string> other)
        {
            return _set.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<string> other)
        {
            return _set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<string> other)
        {
            return _set.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<string> other)
        {
            CheckIsReadOnly();

            if (other == null || !other.Any()) return;

            ChangeNonDefault();

            _set.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<string> other)
        {
            CheckIsReadOnly();

            if (other == null || !other.Any()) return;

            ChangeNonDefault();

            _set.UnionWith(other);
        }

        void ICollection<string>.Add(string item)
        {
            CheckIsReadOnly();

            ChangeNonDefault();

            _set.Add(item);
        }

        public void Clear()
        {
            CheckIsReadOnly();

            if (_isDefault)
            {
                _set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                _set.Clear();
            }
        }

        public bool Contains(string item)
        {
            return _set.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _set.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        public bool Remove(string item)
        {
            ChangeNonDefault();

            return _set.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _set.GetEnumerator();
        }
    }
}
