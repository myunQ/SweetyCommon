using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace Sweety.Common.DataProvider
{
    /// <summary>
    /// 这个类旨在简化操作数据库时SQL参数的编码工作。
    /// </summary>
    /// <typeparam name="T">SQL参数对象类型。这里应该指定具体的对象类型，而不是抽象类或接口。</typeparam>
    /// <example>
    /// <h2>创建三个SQL参数对象：</h2>
    /// <per>
    /// IRelationalDBUtility _db = RelationalDBUtilityFactory.Create("npsql"); //这一行是伪代码。
    /// using FixedCapacityParameterList&lt;Npgsql.NpgsqlParameter&gt; parameters = new(_db, 3);
    /// parameters.Add("@param-1", "Parameter Value", (int)NpgsqlTypes.NpgsqlDbType.Varchar, 100)
    ///     .Add("@param-2", 20210721, (int)NpgsqlTypes.NpgsqlDbType.Integer);
    ///     .Add("@param-3", null, (int)NpgsqlTypes.NpgsqlDbType.Integer, default, ParameterDirection.Output);
    ///     
    /// Npgsql.NpgsqlCommand cmd = new();
    /// foreach(var p in parameters)
    /// {
    ///     cmd.Parameters.Add(p);
    /// }
    /// </per>
    /// </example>
    internal class FixedCapacityParameterList<T> : IParameterList<T>, IEnumerable<T>, IDisposable where T : class, IDbDataParameter
    {
        readonly int _capacity;
        readonly IRelationalDBUtility _dbUtility;
#if NETSTANDARD2_0
        readonly T[] _paramArray;
#else
        readonly T?[] _paramArray;
#endif

        /// <summary>
        /// 如果从外部传入SQL参数，则实例化该字段，用于存储<see cref="_paramArray"/>对应位置的原始值，在调用实例的<see cref="Reset"/>或<see cref="Dispose()"/>方法时恢复。
        /// key:<see cref="_paramArray"/>的索引位置。
        /// value:<see cref="_paramArray"/>对应索引位置的值。
        /// </summary>
#if NETSTANDARD2_0
        Dictionary<int, T> _paramArrayOriginalElementDictionary;
#else
        Dictionary<int, T?>? _paramArrayOriginalElementDictionary;
#endif


        /// <summary>
        /// 指向 <see cref="_paramArray"/> 的空位（不一定是<c>null</c>）的索引，下次添加元素时就添加到该指定的索引位。
        /// </summary>
        int _length;

        /// <summary>
        /// 初始化对象实例，设定SQL参数的数量。
        /// </summary>
        /// <param name="dbUtility">数据库操作对象，用于构建SQL参数。</param>
        /// <param name="capacity">SQL参数的数量。设置后不可更改。</param>
        /// <exception cref="ArgumentNullException"><paramref name="dbUtility"/>为null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/>小于1。</exception>
        public FixedCapacityParameterList(IRelationalDBUtility dbUtility, int capacity)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity), Properties.Localization.a_value_greater_than_0_is_required);

            _dbUtility = dbUtility ?? throw new ArgumentNullException(nameof(dbUtility));
            _capacity = capacity;
            _length = 0;
            _paramArrayOriginalElementDictionary = null;
#if NETSTANDARD2_0
            _paramArray = ArrayPool<T>.Shared.Rent(capacity);
#else
            _paramArray = ArrayPool<T?>.Shared.Rent(capacity);
#endif
        }

        /// <summary>
        /// SQL参数的数量。
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// 注意：如果设置了SQL参数对象，则需要主动将其设置为null，否则该当前对象实例释放后，传入的参数对象将被其它 Command 对象（可能是不同线程的）使用。
        /// </summary>
        /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> 小于 0。-或- <paramref name="index"/> 等于或大于 <see cref="Length"/>。</exception>
        /// <exception cref="ValueNullException">将null赋值给索引器。</exception>
#if NETSTANDARD2_0
        public T this[int index]
#else
        public T? this[int index]
#endif
        {
            get
            {
                if (index >= _length) throw new IndexOutOfRangeException();

                return _paramArray[index];
            }
            set
            {
                if (value == null) throw new ValueNullException(Properties.Localization.value_cannot_be_null);
                if (index >= _length) throw new IndexOutOfRangeException();

                SaveOriginalElementDictionary(index, _paramArray[index]);

                _paramArray[index] = value;
            }
        }

        /// <summary>
        /// 获取或设置指定SQL参数名称的元素。
        /// 注意：如果设置了SQL参数对象，则需要主动将其设置为null，否则该当前对象实例释放后，传入的参数对象将被其它 Command 对象（可能是不同线程的）使用。
        /// </summary>
        /// <param name="parameterName">参数名。</param>
        /// <returns>返回指定索引处的元素。</returns>
        /// <exception cref="ArgumentException">key为空字符串。</exception>
        /// <exception cref="ArgumentNullException">key为null。</exception>
        /// <exception cref="ValueNullException">将null赋值给索引器。</exception>
#if NETSTANDARD2_0
        public T this[string parameterName]
#else
        public T? this[string parameterName]
#endif
        {
            get
            {
                if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
                if (parameterName.Length == 0) throw new ArgumentException(Properties.Localization.the_parameter_name_cannot_be_empty, nameof(parameterName));

                for (int i = 0; i < _length; i++)
                {
#if NETSTANDARD2_0
                    if (_paramArray[i].ParameterName == parameterName)
#else
                    if (_paramArray[i]!.ParameterName == parameterName)
#endif
                    {
                        return _paramArray[i];
                    }
                }
                return default;
            }
            set
            {
                if (value == null) throw new ValueNullException(Properties.Localization.value_cannot_be_null);
                if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
                if (parameterName.Length == 0) throw new ArgumentException(Properties.Localization.the_parameter_name_cannot_be_empty, nameof(parameterName));

                for (int i = 0; i < _length; i++)
                {
#if NETSTANDARD2_0
                    if (_paramArray[i].ParameterName == parameterName)
#else
                    if (_paramArray[i]!.ParameterName == parameterName)
#endif
                    {
                        SaveOriginalElementDictionary(i, _paramArray[i]);
                        _paramArray[i] = value;
                        return;
                    }
                }

                if (value != null) Add(value);
            }
        }


        /// <summary>
        /// 添加SQL参数对象。
        /// 注意：如果添加了SQL参数对象，则需要主动将其设置为null，否则该当前对象实例释放后，传入的参数对象将被其它 Command 对象（可能是不同线程的）使用。
        /// </summary>
        /// <param name="parameter">要添加的参数对象。</param>
        public IParameterList<T> Add(T parameter)
        {
            if (_length == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");

            SaveOriginalElementDictionary(_length, _paramArray[_length]);

            _paramArray[_length++] = parameter;

            return this;
        }



        /// <summary>
        /// 添加SQL参数对象到<see cref="Parameters"/>属性返回的数组中。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数的方向。</param>
#if NETSTANDARD2_0
        public IParameterList<T> Add(string parameterName, object value, ParameterDirection direction = ParameterDirection.Input)
#else
        public IParameterList<T> Add(string parameterName, object? value, ParameterDirection direction = ParameterDirection.Input)
#endif
        {
            if (_length == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");


            // 这里是先对左侧进行运算，所以不能将 _paramArrayIndex++ 放在左侧。
            _paramArray[_length] = (T)_dbUtility.ResetOrBuildParameter(_paramArray[_length++], parameterName, value, direction);

            return this;
        }

        /// <summary>
        /// 添加SQL参数对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="field">参数类型与参数的空间大小。类型的值为各数据库客户端提供的数据库参数类型枚举值。</param>
        /// <param name="direction">参数的方向。</param>
        /// <returns>返回当前对象实例。</returns>
#if NETSTANDARD2_0
        public IParameterList<T> Add(string parameterName, object value, (int parameterType, int size) field, ParameterDirection direction = ParameterDirection.Input)
#else
        public IParameterList<T> Add(string parameterName, object? value, (int parameterType, int size) field, ParameterDirection direction = ParameterDirection.Input)
#endif
            => Add(parameterName, value, field.parameterType, field.size, direction);

        /// <summary>
        /// 添加SQL参数对象到<see cref="Parameters"/>属性返回的数组中。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。值为各数据库客户端提供的数据库参数类型枚举值。</param>
        /// <param name="size">参数的空间大小。</param>
        /// <param name="direction">参数的方向。</param>
#if NETSTANDARD2_0
        public IParameterList<T> Add(string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#else
        public IParameterList<T> Add(string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#endif
        {
            if (_length == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");

            // 这里是先对左侧进行运算，所以不能将 _paramArrayIndex++ 放在左侧。
            _paramArray[_length] = (T)_dbUtility.ResetOrBuildParameter(_paramArray[_length++], parameterName, value, parameterType, size, direction);

            return this;
        }

        /// <summary>
        /// 重置SQL参数列表对象，回到新实例时的状态。
        /// </summary>
        public IParameterList<T> Reset()
        {
            RecoverOriginalElement();

            _length = 0;

            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
            {
#if NETSTANDARD2_0
                yield return _paramArray[i];
#else
                yield return _paramArray[i]!;
#endif
            }
            //yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 记录<see cref="_paramArray"/>原始元素。
        /// </summary>
        /// <param name="key">元素在<see cref="_paramArray"/>中的索引。</param>
        /// <param name="value">元素。</param>
#if NETSTANDARD2_0
        private void SaveOriginalElementDictionary(int key, T value)
        {
            if (_paramArrayOriginalElementDictionary == null) _paramArrayOriginalElementDictionary = new Dictionary<int, T>();
#else
        private void SaveOriginalElementDictionary(int key, T? value)
        {
            _paramArrayOriginalElementDictionary ??= new Dictionary<int, T?>();
#endif
            if (!_paramArrayOriginalElementDictionary.ContainsKey(key))
            {
                _paramArrayOriginalElementDictionary.Add(key, value);
            }
        }
        /// <summary>
        /// 恢复<see cref="_paramArray"/>原始元素。
        /// </summary>
        private void RecoverOriginalElement()
        {
            if (_paramArrayOriginalElementDictionary?.Count > 0)
            {
                foreach(var kv in _paramArrayOriginalElementDictionary)
                {
                    _paramArray[kv.Key] = kv.Value;
                }

                _paramArrayOriginalElementDictionary.Clear();
            }
        }

        /// <summary>
        /// 释放对象所使用的资源。
        /// </summary>
        public void Dispose()
        {
            RecoverOriginalElement();

#if NETSTANDARD2_0
            ArrayPool<T>.Shared.Return(_paramArray);
#else
            ArrayPool<T?>.Shared.Return(_paramArray);
#endif
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放对象所使用的资源。
        /// </summary>
        ~FixedCapacityParameterList()
            => Dispose();
    }
}
