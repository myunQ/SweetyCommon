/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  将数据读取器 IDataReader 里的数据，转换成对应的模型。
 *  将数据读取器 DataTable 里的数据，转换成对应的模型。
 * 
 * 
 * Members Index:
 *      static class
 *          ConvertToModel
 *              public static T ToModel<T>(this IDataReader reader)
 *              public static void ToModel<T>(this IDataReader reader, T model)
 *              private static void ToModel<T>(IDataReader reader, InternalDynamicPropertiesAssignLambda<T> dynamicAssign, ref T model)
 *              
 *              public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, Func<T> funBuildModel = null)
 *              
 *              public static T ToModel<T>(this DataRow row, DataColumnCollection columns)
 *              public static IList<T> ToModel<T>(this DataTable table)
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Converter
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// 数据读取器转换到模型对象。
    /// </summary>
    public static class ConvertToModel
    {
        /// <summary>
        /// 动态对象属性赋值拉姆达表达式集合（key:Model类型；Value:<see cref="InternalDynamicPropertiesAssignLambda{T}"/> 对象的实例）。
        /// </summary>
        static readonly ConcurrentDictionary<Type, Object> DYNAMIC_ASSIGN_LAMBDA = new ConcurrentDictionary<Type, Object>();

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <returns>返回一个模型。</returns>
        public static T ToModel<T>(this IDataReader reader)
        {
            Type modelType = typeof(T);
            InternalDynamicPropertiesAssignLambda<T> dynamicAssign;

#if NETSTANDARD2_0
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object obj))
#else
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object? obj))
#endif
            {
                dynamicAssign = (InternalDynamicPropertiesAssignLambda<T>)obj;
            }
            else
            {
                dynamicAssign = new InternalDynamicPropertiesAssignLambda<T>(modelType);
                DYNAMIC_ASSIGN_LAMBDA.GetOrAdd(modelType, dynamicAssign);
            }

            T result = dynamicAssign.DefaultConstructor();
            ToModel<T>(reader, dynamicAssign, ref result);
            return result;
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="model">接受读取器数据的模型。</param>
        public static void ToModel<T>(this IDataReader reader, ref T model)
        {
            Type modelType = typeof(T);
            InternalDynamicPropertiesAssignLambda<T> dynamicAssign;

#if NETSTANDARD2_0
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object obj))
#else
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object? obj))
#endif
            {
                dynamicAssign = (InternalDynamicPropertiesAssignLambda<T>)obj;
            }
            else
            {
                dynamicAssign = new InternalDynamicPropertiesAssignLambda<T>(modelType);
                DYNAMIC_ASSIGN_LAMBDA.GetOrAdd(modelType, dynamicAssign);
            }

            ToModel<T>(reader, dynamicAssign, ref model);
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="dynamicAssign"></param>
        /// <param name="model"></param>
        private static void ToModel<T>(IDataReader reader, InternalDynamicPropertiesAssignLambda<T> dynamicAssign, ref T model)
        {
            for (int i = reader.FieldCount - 1; i >= 0; i--)
            {
                if (reader.IsDBNull(i)) continue;

#if NETSTANDARD2_0
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out Action<T, object> assignAction))
#else
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out Action<T, object>? assignAction))
#endif
                {
                    assignAction(model, reader[i]);
                }
            }
        }


        /// <summary>
        /// 将 <see cref="IDataReader"/> 中的数据读取到模型集合。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="collection">存储从<paramref name="reader"/>里提取出的模型的集合。</param>
        /// <param name="funBuildModel">创建模型实例的委托。如果为<c>null</c>则使用模型的公共无参构造函数创建模型。</param>
#if NETSTANDARD2_0
        public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, Func<T> funBuildModel = null)
#else
        public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, Func<T>? funBuildModel = null)
#endif
        {
            Type modelType = typeof(T);
            InternalDynamicPropertiesAssignLambda<T> dynamicAssign;

#if NETSTANDARD2_0
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object obj))
#else
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object? obj))
#endif
            {
                dynamicAssign = (InternalDynamicPropertiesAssignLambda<T>)obj;
            }
            else
            {
                dynamicAssign = new InternalDynamicPropertiesAssignLambda<T>(modelType);
                DYNAMIC_ASSIGN_LAMBDA.GetOrAdd(modelType, dynamicAssign);
            }


            int fieldCount = reader.FieldCount;
            // 获取对象属性赋值的函数。
            Action<T, object>[] actions = new Action<T, object>[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
#if NETSTANDARD2_0
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out Action<T, object> assignAction))
#else
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out Action<T, object>? assignAction))
#endif
                {
                    actions[i] = assignAction;
                }
            }


            bool hasFun = funBuildModel != null;
            // 从 reader 里读取数据到集合 collection。
            do
            {
#if NETSTANDARD2_0
                T model = hasFun ? funBuildModel() : dynamicAssign.DefaultConstructor();
#else
                T model = hasFun ? funBuildModel!() : dynamicAssign.DefaultConstructor();
#endif

                for (int i = 0; i < fieldCount; i++)
                {
                    if (actions[i] == null || reader.IsDBNull(i)) continue;

                    actions[i](model, reader[i]);
                }

                collection.Add(model);
            }
            while (reader.Read());
        }


        /// <summary>
        /// 将 <see cref="IDataReader"/> 中的数据读取到模型集合。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="collection">存储从<paramref name="reader"/>里提取出的模型的集合。</param>
        /// <param name="cancellationToken">用于取消异步操作的令牌。</param>
        /// <param name="funBuildModel">创建模型实例的委托。如果为<c>null</c>则使用模型的公共无参构造函数创建模型。</param>
#if NETSTANDARD2_0
        public static async Task ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, CancellationToken? cancellationToken = null, Func<T> funBuildModel = null)
#else
        public static async ValueTask ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, CancellationToken? cancellationToken = null, Func<T>? funBuildModel = null)
#endif
        {
            Type modelType = typeof(T);
            InternalDynamicPropertiesAssignLambda<T> dynamicAssign;

#if NETSTANDARD2_0
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object obj))
#else
            if (DYNAMIC_ASSIGN_LAMBDA.TryGetValue(modelType, out object? obj))
#endif
            {
                dynamicAssign = (InternalDynamicPropertiesAssignLambda<T>)obj;
            }
            else
            {
                dynamicAssign = new InternalDynamicPropertiesAssignLambda<T>(modelType);
                DYNAMIC_ASSIGN_LAMBDA.GetOrAdd(modelType, dynamicAssign);
            }

            int fieldCount = reader.FieldCount;
            // 获取对象属性赋值的函数。
            Action<T, object>[] actions = new Action<T, object>[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
#if NETSTANDARD2_0
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out Action<T, object> assignAction))
#else
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out Action<T, object>? assignAction))
#endif
                {
                    actions[i] = assignAction;
                }
            }


            bool hasFun = funBuildModel != null;
            // 从 reader 里读取数据到集合 collection。
            if (cancellationToken.HasValue)
            {
                do
                {
#if NETSTANDARD2_0
                    T model = hasFun ? funBuildModel() : dynamicAssign.DefaultConstructor();
#else
                    T model = hasFun ? funBuildModel!() : dynamicAssign.DefaultConstructor();
#endif

                    for (int i = 0; i < fieldCount; i++)
                    {
                        if (actions[i] == null || await reader.IsDBNullAsync(i, cancellationToken.Value)) continue;

                        actions[i](model, reader[i]);
                    }

                    collection.Add(model);
                }
                while (await reader.ReadAsync(cancellationToken.Value));
            }
            else
            {
                do
                {
#if NETSTANDARD2_0
                    T model = hasFun ? funBuildModel() : dynamicAssign.DefaultConstructor();
#else
                    T model = hasFun ? funBuildModel!() : dynamicAssign.DefaultConstructor();
#endif

                    for (int i = 0; i < fieldCount; i++)
                    {
                        if (actions[i] == null || await reader.IsDBNullAsync(i)) continue;

                        actions[i](model, reader[i]);
                    }

                    collection.Add(model);
                }
                while (await reader.ReadAsync());
            }
        }
    }
}
