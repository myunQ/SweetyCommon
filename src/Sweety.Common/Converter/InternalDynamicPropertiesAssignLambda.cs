/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  动态的将指定的类型生成调用无参构造函数的 Lambda 表达式 以及 为每个属性赋值的 Lambda 表达式。
 *
 *  目前近支持对 class 的公共属性动态赋值。
 *
 *  要想支持 struct 的公共属性动态赋值就要解决传址问题，
 *  由于现在表达式树不支持创建使用 ref 关键字的拉姆达表达式，
 *  所以现在没法给 struct 的动态赋值。
 * 
 * Members Index:
 *      class
 *          DynamicPropertiesAssignLambda<T>
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    

    /// <summary>
    /// 动态生成对象属性赋值的拉姆达表达式
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    internal class InternalDynamicPropertiesAssignLambda<T>
    {
        /// <summary>
        /// 初始化为对象属性赋值的拉姆达表达式集合
        /// </summary>
        internal InternalDynamicPropertiesAssignLambda() : this(typeof(T)) { }
        /// <summary>
        /// 初始化为对象属性赋值的拉姆达表达式集合
        /// </summary>
        /// <param name="modelType">对象类型</param>
        internal InternalDynamicPropertiesAssignLambda(Type modelType)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));

            PropertyInfo[] properties = modelType.GetProperties();

            DefaultConstructor = Expression.Lambda<Func<T>>(Expression.New(modelType), null).Compile();
            Properties = new Dictionary<string, Action<T, Object>>(properties.Length, StringComparer.OrdinalIgnoreCase);

            Type attribType = typeof(FieldNameAttribute);

            ParameterExpression pe_instance = Expression.Parameter(modelType, "instance");
            ParameterExpression pe_value = Expression.Parameter(typeof(Object), "value");

            MethodInfo hackType = typeof(InternalConvertUtility).GetMethod("HackType", BindingFlags.Static | BindingFlags.NonPublic);

            foreach (PropertyInfo p in properties)
            {
                if (!p.CanWrite) continue;

                var attrib = Attribute.GetCustomAttribute(p, attribType);
                string key = (attrib == null) ? p.Name : ((FieldNameAttribute)attrib).FiledName;

                var expr_method = Expression.Call(hackType, pe_value, Expression.Constant(p.PropertyType, typeof(Type)));
                
                var expr_property = Expression.Property(pe_instance, p);
                var expr_assign = Expression.Assign(expr_property, Expression.Convert(expr_method, p.PropertyType));
                var expr_lambda = Expression.Lambda<Action<T, Object>>(expr_assign, pe_instance, pe_value);

                Properties.Add(key, expr_lambda.Compile());
            }
        }
        
        /// <summary>
        /// 调用对象的无参构造函数的委托
        /// </summary>
        internal Func<T> DefaultConstructor { get; private set; }
        /// <summary>
        /// 为对象每个公开属性赋值的委托集合（key：属性名称，区分大小写；value：为该属性赋值的委托）
        /// </summary>
        internal Dictionary<string, Action<T, Object>> Properties { get; private set; }
    }
}
