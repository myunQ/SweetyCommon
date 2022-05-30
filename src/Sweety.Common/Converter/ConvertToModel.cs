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
    using System.Buffers;
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
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <returns>返回一个模型。</returns>
        public static T ToModel<T>(this IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
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
            ToModel<T>(reader, dynamicAssign, ref result, ignoreFieldName, customAssignMethod);
            return result;
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <returns>返回一个模型。</returns>
        public static T ToModel<T>(this IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
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
            ToModel<T>(reader, dynamicAssign, ref result, ignoreFieldNames, customAssignMethod);
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
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="reader">从数据源读取数据的读取器。</param>
        /// <param name="model">模型实例。</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        public static void ToModel<T>(this IDataReader reader, ref T model, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
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

            ToModel<T>(reader, dynamicAssign, ref model, ignoreFieldName, customAssignMethod);
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型
        /// </summary>
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="reader">从数据源读取数据的读取器。</param>
        /// <param name="model">模型实例。</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        public static void ToModel<T>(this IDataReader reader, ref T model, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
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

            ToModel<T>(reader, dynamicAssign, ref model, ignoreFieldNames, customAssignMethod);
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型
        /// </summary>
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="reader">从数据源读取数据的读取器。</param>
        /// <param name="dynamicAssign">动态生成对象属性赋值的拉姆达表达式。</param>
        /// <param name="model">模型实例。</param>
        private static void ToModel<T>(IDataReader reader, InternalDynamicPropertiesAssignLambda<T> dynamicAssign, ref T model)
        {
            for (int i = reader.FieldCount - 1; i >= 0; i--)
            {
                if (reader.IsDBNull(i)) continue;

#if NETSTANDARD2_0
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out var assignAction))
#else
                if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out var assignAction))
#endif
                {
                    assignAction(ref model, reader[i]);
                }
            }
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型
        /// </summary>
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="reader">从数据源读取数据的读取器。</param>
        /// <param name="dynamicAssign">动态生成对象属性赋值的拉姆达表达式。</param>
        /// <param name="model">模型实例。</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        private static void ToModel<T>(IDataReader reader, InternalDynamicPropertiesAssignLambda<T> dynamicAssign, ref T model, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            for (int i = reader.FieldCount - 1; i >= 0; i--)
            {
                if (reader.IsDBNull(i)) continue;

                string fieldName = reader.GetName(i);

                if (fieldName == ignoreFieldName)
                {
                    customAssignMethod(ref model, reader, i);
                }
                else
                {
#if NETSTANDARD2_0
                    if (dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#else
                    if (dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#endif
                    {
                        assignAction(ref model, reader[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 将<see cref="IDataReader"/>中的数据赋值给对应的模型
        /// </summary>
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="reader">从数据源读取数据的读取器。</param>
        /// <param name="dynamicAssign">动态生成对象属性赋值的拉姆达表达式。</param>
        /// <param name="model">模型实例。</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        private static void ToModel<T>(IDataReader reader, InternalDynamicPropertiesAssignLambda<T> dynamicAssign, ref T model, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            for (int i = reader.FieldCount - 1; i >= 0; i--)
            {
                if (reader.IsDBNull(i)) continue;

                string fieldName = reader.GetName(i);

#if NETSTANDARD2_0
                if (!ignoreFieldNames.Contains(fieldName) && dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#else
                if (!ignoreFieldNames.Contains(fieldName) && dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#endif
                {
                    assignAction(ref model, reader[i]);
                }
            }

            customAssignMethod(ref model, reader);
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
            RefT1Action<T, object>[] actions = ArrayPool<RefT1Action<T, object>>.Shared.Rent(fieldCount);

            try
            {
                // 获取对象属性赋值的函数。
                for (int i = 0; i < fieldCount; i++)
                {
#if NETSTANDARD2_0
                    if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out var assignAction))
#else
                    if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out var assignAction))
#endif
                    {
                        actions[i] = assignAction;
                    }
                    else if (actions[i] != null)
                    {
                        actions[i] = null;
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

                        actions[i](ref model, reader[i]);
                    }

                    collection.Add(model);
                }
                while (reader.Read());
            }
            finally
            {
                ArrayPool<RefT1Action<T, object>>.Shared.Return(actions);
            }
        }


        /// <summary>
        /// 将 <see cref="IDataReader"/> 中的数据读取到模型集合。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="collection">存储从<paramref name="reader"/>里提取出的模型的集合。</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="funBuildModel">创建模型实例的委托。如果为<c>null</c>则使用模型的公共无参构造函数创建模型。</param>
#if NETSTANDARD2_0
        public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, Func<T> funBuildModel = null)
#else
        public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, Func<T>? funBuildModel = null)
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
            RefT1Action<T, object>[] actions = ArrayPool<RefT1Action<T, object>>.Shared.Rent(fieldCount);

            try
            {
                int propertyNameIndex = -1;
                // 获取对象属性赋值的函数。
                for (int i = 0; i < fieldCount; i++)
                {
                    string fieldName = reader.GetName(i);

                    if (fieldName == ignoreFieldName)
                    {
                        propertyNameIndex = i;
                    }
#if NETSTANDARD2_0
                    else if (dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#else
                    else if (dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#endif
                    {
                        actions[i] = assignAction;
                    }
                    else if (actions[i] != null)
                    {
                        actions[i] = null;
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
                        if (reader.IsDBNull(i)) continue;

                        if (propertyNameIndex == i)
                        {
                            customAssignMethod(ref model, reader, i);
                        }
                        else if (actions[i] != null)
                        {
                            actions[i](ref model, reader[i]);
                        }
                    }

                    collection.Add(model);
                }
                while (reader.Read());
            }
            finally
            {
                ArrayPool<RefT1Action<T, object>>.Shared.Return(actions);
            }
        }


        /// <summary>
        /// 将 <see cref="IDataReader"/> 中的数据读取到模型集合。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="collection">存储从<paramref name="reader"/>里提取出的模型的集合。</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="funBuildModel">创建模型实例的委托。如果为<c>null</c>则使用模型的公共无参构造函数创建模型。</param>
#if NETSTANDARD2_0
        public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, Func<T> funBuildModel = null)
#else
        public static void ToCollection<T>(this IDataReader reader, ICollection<T> collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, Func<T>? funBuildModel = null)
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
            RefT1Action<T, object>[] actions = ArrayPool<RefT1Action< T, object>>.Shared.Rent(fieldCount);
            
            try
            {
                // 获取对象属性赋值的函数。
                for (int i = 0; i < fieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
#if NETSTANDARD2_0
                    if (!ignoreFieldNames.Contains(fieldName) && dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#else
                    if (!ignoreFieldNames.Contains(fieldName) && dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#endif
                    {
                        actions[i] = assignAction;
                    }
                    else if (actions[i] != null)
                    {
                        actions[i] = null;
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
    
                        actions[i](ref model, reader[i]);
                    }

                    customAssignMethod(ref model, reader);

                    collection.Add(model);
                }
                while (reader.Read());
            }
            finally
            {
                ArrayPool<RefT1Action<T, object>>.Shared.Return(actions);
            }
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
        public static async Task ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, CancellationToken cancellationToken = default, Func<T> funBuildModel = null)
#else
        public static async ValueTask ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, CancellationToken cancellationToken = default, Func<T>? funBuildModel = null)
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
            RefT1Action<T, object>[] actions = ArrayPool<RefT1Action<T, object>>.Shared.Rent(fieldCount);

            try
            {
                // 获取对象属性赋值的函数。
                for (int i = 0; i < fieldCount; i++)
                {
#if NETSTANDARD2_0
                    if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out var assignAction))
#else
                    if (dynamicAssign.Properties.TryGetValue(reader.GetName(i), out var assignAction))
#endif
                    {
                        actions[i] = assignAction;
                    }
                    else if (actions[i] != null)
                    {
                        actions[i] = null;
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

                        actions[i](ref model, reader[i]);
                    }

                    collection.Add(model);
                }
                while (await reader.ReadAsync(cancellationToken));
            }
            finally
            {
                ArrayPool<RefT1Action<T, object>>.Shared.Return(actions);
            }
        }

        /// <summary>
        /// 将 <see cref="IDataReader"/> 中的数据读取到模型集合。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="collection">存储从<paramref name="reader"/>里提取出的模型的集合。</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="cancellationToken">用于取消异步操作的令牌。</param>
        /// <param name="funBuildModel">创建模型实例的委托。如果为<c>null</c>则使用模型的公共无参构造函数创建模型。</param>
#if NETSTANDARD2_0
        public static async Task ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken = default, Func<T> funBuildModel = null)
#else
        public static async ValueTask ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken = default, Func<T>? funBuildModel = null)
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
            RefT1Action<T, object>[] actions = ArrayPool<RefT1Action<T, object>>.Shared.Rent(fieldCount);

            try
            {
                int propertyNameIndex = -1;
                // 获取对象属性赋值的函数。
                for (int i = 0; i < fieldCount; i++)
                {
                    string fieldName = reader.GetName(i);

                    if (fieldName == ignoreFieldName)
                    {
                        propertyNameIndex = i;
                    }
#if NETSTANDARD2_0
                    else if (dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#else
                    else if (dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#endif
                    {
                        actions[i] = assignAction;
                    }
                    else if (actions[i] != null)
                    {
                        actions[i] = null;
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
                        if (reader.IsDBNull(i)) continue;

                        if (propertyNameIndex == i)
                        {
                            customAssignMethod(ref model, reader, i);
                        }
                        else if (actions[i] != null)
                        {
                            actions[i](ref model, reader[i]);
                        }
                    }

                    collection.Add(model);
                }
                while (await reader.ReadAsync(cancellationToken));
            }
            finally
            {
                ArrayPool<RefT1Action<T, object>>.Shared.Return(actions);
            }
        }

        /// <summary>
        /// 将 <see cref="IDataReader"/> 中的数据读取到模型集合。
        /// </summary>
        /// <typeparam name="T">要接收数据的模型类型。</typeparam>
        /// <param name="reader">数据读取器，需先调用<see cref="IDataReader.Read()"/>方法并返回<c>true</c>时在传入。</param>
        /// <param name="collection">存储从<paramref name="reader"/>里提取出的模型的集合。</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="cancellationToken">用于取消异步操作的令牌。</param>
        /// <param name="funBuildModel">创建模型实例的委托。如果为<c>null</c>则使用模型的公共无参构造函数创建模型。</param>
#if NETSTANDARD2_0
        public static async Task ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken = default, Func<T> funBuildModel = null)
#else
        public static async ValueTask ToCollectionAsync<T>(this DbDataReader reader, ICollection<T> collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken = default, Func<T>? funBuildModel = null)
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
            RefT1Action<T, object>[] actions = ArrayPool<RefT1Action<T, object>>.Shared.Rent(fieldCount);

            try
            {
                // 获取对象属性赋值的函数。
                for (int i = 0; i < fieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
#if NETSTANDARD2_0
                    if (!ignoreFieldNames.Contains(fieldName) && dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#else
                    if (!ignoreFieldNames.Contains(fieldName) && dynamicAssign.Properties.TryGetValue(fieldName, out var assignAction))
#endif
                    {
                        actions[i] = assignAction;
                    }
                    else if (actions[i] != null)
                    {
                        actions[i] = null;
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

                        actions[i](ref model, reader[i]);
                    }

                    customAssignMethod(ref model, reader);

                    collection.Add(model);
                }
                while (await reader.ReadAsync(cancellationToken));
            }
            finally
            {
                ArrayPool<RefT1Action<T, object>>.Shared.Return(actions);
            }
        }
    }
}
