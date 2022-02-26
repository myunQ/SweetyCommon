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
    /// using BuildParamenterMemoryUtility<Npgsql.NpgsqlParameter> buildParamUtil = new(_db, 5);
    /// buildParamUtil.Add("@param-1", "Paramenter Value", (int)NpgsqlTypes.NpgsqlDbType.Varchar, 100);
    /// buildParamUtil.Add("@param-2", 20210721, (int)NpgsqlTypes.NpgsqlDbType.Integer);
    /// buildParamUtil.Add("@param-3", null, (int)NpgsqlTypes.NpgsqlDbType.Integer, default, ParameterDirection.Output);
    /// ReadOnlyMemory&lt;IDbDataParamenter&gt; paramMemory = buildParamUtil.Parameters;
    /// </per>
    /// 下面的代码和上面的代码作用是相同的：
    /// <per>
    /// IRelationalDBUtility _db = RelationalDBUtilityFactory.Create("npsql"); //这一行是伪代码。
    /// int paramArryIndex = 0;
    /// IDbDataParamenter[] paramArry = System.Buffers.ArrayPool<Npgsql.NpgsqlParameter>.Shared.Rent(5);
    /// try
    /// {
    ///     paramArr[paramArryIndex] = _db.ResetOrBuildParameter(paramArr[paramArryIndex++], "@param-1", "Paramenter Value", (int)NpgsqlTypes.NpgsqlDbType.Varchar, 100);
    ///     paramArr[paramArryIndex] = _db.ResetOrBuildParameter(paramArr[paramArryIndex++], "@param-2", 20210721, (int)NpgsqlTypes.NpgsqlDbType.Integer);
    ///     paramArr[paramArryIndex] = _db.ResetOrBuildParameter(paramArr[paramArryIndex++], "@param-3", null, (int)NpgsqlTypes.NpgsqlDbType.Integer, default, ParameterDirection.Output);
    ///     
    ///     ReadOnlyMemory&lt;IDbDataParamenter&gt; paramMemory = new(paramArr, 0, paramArryIndex);
    /// }
    /// finally
    /// {
    ///     System.Buffers.ArrayPool<Npgsql.NpgsqlParameter>.Shared.Return((Npgsql.NpgsqlParameter[])paramArr);
    /// }
    /// </per>
    /// </example>
    public class BuildParamenterMemoryUtility<T> : IDisposable where T : class, IDbDataParameter
    {
        readonly int _capacity;
        readonly IRelationalDBUtility _dbUtility;
        readonly IDbDataParameter[] _paramArray;
        readonly T[] _forRecycling; //增加这个变量只是为了在回收数组时不用进行类型转换。
        int _paramArrayIndex;

        /// <summary>
        /// 初始化对象实例，设定SQL参数的数量。
        /// </summary>
        /// <param name="dbUtility">数据库操作对象，用于构建SQL参数。</param>
        /// <param name="capacity">SQL参数的数量。设置后不可更改。</param>
        public BuildParamenterMemoryUtility(IRelationalDBUtility dbUtility, int capacity)
        {
            _dbUtility = dbUtility;
            _capacity = capacity;
            _paramArrayIndex = 0;
            _paramArray = _forRecycling = ArrayPool<T>.Shared.Rent(capacity);

        }

        /// <summary>
        /// 获取该对象的SQL参数最大容量。创建对象是设置。
        /// </summary>
        public int Capacity => _capacity;

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
#if NET5_0_OR_GREATER
        public ReadOnlyMemory<IDbDataParameter> Parameters => new(_paramArray, 0, _paramArrayIndex);
#else
        public ReadOnlyMemory<IDbDataParameter> Parameters => new ReadOnlyMemory<IDbDataParameter>(_paramArray, 0, _paramArrayIndex);
#endif


        /// <summary>
        /// 添加SQL参数对象到<see cref="Parameters"/>属性返回的数组中。
        /// </summary>
        /// <param name="parameter">要添加的参数对象。</param>
        public void Add(T parameter)
        {
            if (_paramArrayIndex == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");

            _paramArray[_paramArrayIndex++] = parameter;
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
        public void Add(string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#else
        public void Add(string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#endif
        {
            if (_paramArrayIndex == _capacity) throw new InvalidOperationException($"超出初始化时设定的参数数量：{_capacity}。");

            // 这里是先对左侧进行运算，所以不能将 _paramArrayIndex++ 放在左侧。
            _paramArray[_paramArrayIndex] = _dbUtility.ResetOrBuildParameter(_paramArray[_paramArrayIndex++], parameterName, value, parameterType, size, direction);
        }

        /// <summary>
        /// 回到新实例时的状态。
        /// </summary>
        public void Reset() => _paramArrayIndex = 0;



        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // 少一个变量，但需要进行类型转换的方案。
            //ArrayPool<T>.Shared.Return((T[])_paramArray);

            // 多一个变量，但不需要类型转换的方案。
            ArrayPool<T>.Shared.Return(_forRecycling);
            GC.SuppressFinalize(this);
        }
    }
}
