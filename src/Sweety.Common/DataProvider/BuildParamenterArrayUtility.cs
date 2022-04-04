using System;
using System.Buffers;
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
    /// using BuildParamenterArrayUtility&lt;Npgsql.NpgsqlParameter&gt; buildParamUtil = new(_db, 3);
    /// buildParamUtil.Add("@param-1", "Paramenter Value", (int)NpgsqlTypes.NpgsqlDbType.Varchar, 100)
    ///     .Add("@param-2", 20210721, (int)NpgsqlTypes.NpgsqlDbType.Integer);
    ///     .Add("@param-3", null, (int)NpgsqlTypes.NpgsqlDbType.Integer, default, ParameterDirection.Output);
    /// Npgsql.NpgsqlParameter[] paramArray = buildParamUtil.Parameters;
    /// </per>
    /// 下面的代码和上面的代码作用是相同的：
    /// <per>
    /// IRelationalDBUtility _db = RelationalDBUtilityFactory.Create("npsql"); //这一行是伪代码。
    /// IDbDataParameter? blockade = null;
    /// int paramArryIndex = 0;
    /// var paramArry = System.Buffers.ArrayPool&lt;Npgsql.NpgsqlParameter&gt;.Shared.Rent(3);
    /// if (paramArr.Length > 3)
    /// {
    ///     blockade = paramArr[3];
    ///     paramArr[3] = null!;
    /// }
    /// try
    /// {
    ///     paramArr[paramArryIndex] = _db.ResetOrBuildParameter(paramArr[paramArryIndex++], "@param-1", "Paramenter Value", (int)NpgsqlTypes.NpgsqlDbType.Varchar, 100);
    ///     paramArr[paramArryIndex] = _db.ResetOrBuildParameter(paramArr[paramArryIndex++], "@param-2", 20210721, (int)NpgsqlTypes.NpgsqlDbType.Integer);
    ///     paramArr[paramArryIndex] = _db.ResetOrBuildParameter(paramArr[paramArryIndex++], "@param-3", null, (int)NpgsqlTypes.NpgsqlDbType.Integer, default, ParameterDirection.Output);
    /// }
    /// finally
    /// {
    ///     if (blockade != null)
    ///     {
    ///         paramArr[3] = blockade;
    ///     }
    ///     System.Buffers.ArrayPool&lt;Npgsql.NpgsqlParameter&gt;.Shared.Return(paramArr);
    /// }
    /// </per>
    /// </example>
    public class BuildParamenterArrayUtility<T> : IDisposable where T : class, IDbDataParameter
    {
        readonly int _capacity;
        readonly IRelationalDBUtility _dbUtility;
        readonly T[] _paramArray;
#if NETSTANDARD2_0
        T blockade;
#else
        T? blockade;
#endif
        int _paramArrayIndex;

        /// <summary>
        /// 初始化对象实例，设定SQL参数的数量。
        /// </summary>
        /// <param name="dbUtility">数据库操作对象，用于构建SQL参数。</param>
        /// <param name="capacity">SQL参数的数量。设置后不可更改。</param>
        public BuildParamenterArrayUtility(IRelationalDBUtility dbUtility, int capacity)
        {
            _dbUtility = dbUtility;
            _capacity = capacity;
            blockade = null;
            _paramArrayIndex = 0;
            _paramArray = ArrayPool<T>.Shared.Rent(capacity);
        }

        /// <summary>
        /// SQL参数的数量。
        /// </summary>
        public int Length => _paramArrayIndex;

        /// <summary>
        /// 获取SQL参数数组。该数组的长度大于等于初始化类时指定的SQL参数的数量，且索引为指定SQL参数的数量+1处的元素为<c>null</c>。
        /// </summary>
        /// <remarks>
        /// 注意：当调用<see cref="Dispose"/>方法后禁止使用该属性返回的值。
        /// </remarks>
        public T[] Parameters
        {
            get
            {
#if NET5_0_OR_GREATER
                if (_paramArray[_paramArrayIndex] is not null && (_paramArrayIndex < _capacity || _paramArray.Length > _capacity))
                {
                    blockade = _paramArray[_paramArrayIndex];
                    _paramArray[_paramArrayIndex] = null!;
                }
#else
                if (_paramArray[_paramArrayIndex] != null && (_paramArrayIndex < _capacity || _paramArray.Length > _capacity))
                {
                    blockade = _paramArray[_paramArrayIndex];
                    _paramArray[_paramArrayIndex] = null;
                }
#endif

                return _paramArray;
            }
        }


        /// <summary>
        /// 添加SQL参数对象到<see cref="Parameters"/>属性返回的数组中。
        /// </summary>
        /// <param name="parameter">要添加的参数对象。</param>
        public BuildParamenterArrayUtility<T> Add(T parameter)
        {
            if (_paramArrayIndex == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");

#if NET5_0_OR_GREATER
            if (blockade is not null)
#else
            if (blockade != null)
#endif
            {
                _paramArray[_paramArrayIndex] = blockade;
                blockade = null;
            }

            _paramArray[_paramArrayIndex++] = parameter;

            return this;
        }

        /// <summary>
        /// 添加SQL参数对象到<see cref="Parameters"/>属性返回的数组中。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。值为各数据库客户端提供的数据库参数类型枚举值。</param>
        /// <param name="size">参数的空间大小。</param>
        /// <param name="direction">参数的方向。</param>
#if NETSTANDARD2_0
        public BuildParamenterArrayUtility<T> Add(string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#else
        public BuildParamenterArrayUtility<T> Add(string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#endif
        {
            if (_paramArrayIndex == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");

#if NET5_0_OR_GREATER
            if (blockade is not null)
#else
            if (blockade != null)
#endif
            {
                _paramArray[_paramArrayIndex] = blockade;
                blockade = null;
            }

            // 这里是先对左侧进行运算，所以不能将 _paramArrayIndex++ 放在左侧。
            _paramArray[_paramArrayIndex] = (T)_dbUtility.ResetOrBuildParameter(_paramArray[_paramArrayIndex++], parameterName, value, parameterType, size, direction);

            return this;
        }

        /// <summary>
        /// 回到新实例时的状态。
        /// </summary>
        public BuildParamenterArrayUtility<T> Reset()
        {
#if NET5_0_OR_GREATER
            if (blockade is not null)
#else
            if (blockade != null)
#endif
            {
                _paramArray[_paramArrayIndex] = blockade;
                blockade = null;
            }

            _paramArrayIndex = 0;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
#if NET5_0_OR_GREATER
            if (blockade is not null)
#else
            if (blockade != null)
#endif
            {
                _paramArray[_paramArrayIndex] = blockade;
            }
            ArrayPool<T>.Shared.Return(_paramArray);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~BuildParamenterArrayUtility()
        {
            Dispose();
        }
    }
}
