using System;
using System.Collections.Generic;
using System.Data;

namespace Sweety.Common.DataProvider
{
    /// <summary>
    /// SQL参数列表。
    /// </summary>
    public interface IParameterList<T> : IEnumerable<T>, IDisposable where T : IDataParameter
    {
        /// <summary>
        /// 获取参数总数。
        /// </summary>
        int Length { get; set; }
        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">索引位置。</param>
        /// <returns>返回指定索引处的元素。</returns>
#if NETCOREAPP3_1_OR_GREATER
        T? this[int index] { get; set; }
#else
        T this[int index] { get; set; }
#endif
        /// <summary>
        /// 获取或设置指定SQL参数名称的元素。
        /// </summary>
        /// <param name="parameterName">参数名。</param>
        /// <returns>返回指定索引处的元素。</returns>
#if NETCOREAPP3_1_OR_GREATER
        T? this[string parameterName] { get; set; }
#else
        T this[string parameterName] { get; set; }
#endif

        /// <summary>
        /// 添加SQL参数对象。
        /// </summary>
        /// <param name="parameter">要添加的参数对象。</param>
        /// <returns>返回当前对象实例。</returns>
        IParameterList<T> Add(T parameter);
        /// <summary>
        /// 添加SQL参数对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数的方向。</param>
#if NETSTANDARD2_0
        IParameterList<T> Add(string parameterName, object value, ParameterDirection direction = ParameterDirection.Input);
#else
        IParameterList<T> Add(string parameterName, object? value, ParameterDirection direction = ParameterDirection.Input);
#endif
        /// <summary>
        /// 添加SQL参数对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="field">参数类型与参数的空间大小。类型的值为各数据库客户端提供的数据库参数类型枚举值。</param>
        /// <param name="direction">参数的方向。</param>
        /// <returns>返回当前对象实例。</returns>
#if NETSTANDARD2_0
        IParameterList<T> Add(string parameterName, object value, (int parameterType, int size) field, ParameterDirection direction = ParameterDirection.Input);
#else
        IParameterList<T> Add(string parameterName, object? value, (int parameterType, int size) field, ParameterDirection direction = ParameterDirection.Input);
#endif
        /// <summary>
        /// 添加SQL参数对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。值为各数据库客户端提供的数据库参数类型枚举值。</param>
        /// <param name="size">参数的空间大小。</param>
        /// <param name="direction">参数的方向。</param>
        /// <returns>返回当前对象实例。</returns>
#if NETSTANDARD2_0
        IParameterList<T> Add(string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#else
        IParameterList<T> Add(string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#endif

        /// <summary>
        /// 重置SQL参数列表对象，回到新实例时的状态。
        /// </summary>
        IParameterList<T> Reset();
    }
}
