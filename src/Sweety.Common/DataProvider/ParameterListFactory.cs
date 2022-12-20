using System.Data;

namespace Sweety.Common.DataProvider
{
    /// <summary>
    /// 参数列表工厂。
    /// </summary>
    public class ParameterListFactory
    {
        /// <summary>
        /// 创建参数列表对象实例。
        /// </summary>
        /// <typeparam name="T">参数类型。</typeparam>
        /// <param name="dbUtility"></param>
        /// <param name="capacity">参数列表能容纳的参数数量的上限。</param>
        /// <returns></returns>
        public static IParameterList<T> Create<T>(IRelationalDBUtility dbUtility, int capacity) where T : class, IDbDataParameter
            => new FixedCapacityParameterList<T>(dbUtility, capacity);
    }
}
