/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  当数据库表的字段命名与实体的属性命名不一致时使用此特性进行标记。
 * 
 * 
 * Members Index:
 *      class
 *          FieldNameAttribute
 *              
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Converter
{
    using System;


    /// <summary>
    /// 字段列名，当属性名与数据库表列名不相同时使用此特性进行标注
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class FieldNameAttribute : Attribute
    {
        /// <summary>
        /// 数据库表字段名
        /// </summary>
        public string FiledName { get; set; }

        /// <summary>
        /// 对象模型与数据库表字段名映射
        /// </summary>
        /// <param name="fieldName">数据库表字段名</param>
        public FieldNameAttribute(string fieldName)
        {
            this.FiledName = fieldName;
        }
    }
}
