using System;

namespace Sweety.Common.Converter
{
    /// <summary>
    /// 用于指定存储媒介中存储的 <see cref="DateTime"/> 类型的数据 <see cref="DateTime.Kind"/> 属性值。
    /// 以便从存储媒介读取数据时可以正确的设置 <see cref="DateTime.Kind"/> 属性值。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SpecifierStoredDateTimeKindAttribute : Attribute
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="kind">存储的<see cref="DateTime"/>类型的<see cref="DateTime.Kind"/>属性值。</param>
        public SpecifierStoredDateTimeKindAttribute(DateTimeKind kind)
        {
            Kind = kind;
        }

        /// <summary>
        /// 获取存储的<see cref="DateTime"/>类型的<see cref="DateTime.Kind"/>属性值。
        /// </summary>
        public DateTimeKind Kind { get; }
    }
}
