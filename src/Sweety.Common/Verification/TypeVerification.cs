/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      验证 Object 对象实例是否是某种类型。
 * 
 * Members Index:
 *      class
 *          TypeVerification
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Verification
{
    using System;


    /// <summary>
    /// 类型验证。
    /// </summary>
    public static class TypeVerification
    {
        /// <summary>
        /// 验证对象是否是整数类型（<c>byte</c>、<c>sbyte</c>、<c>short</c>、<c>ushort</c>、<c>int</c>、<c>uint</c>、<c>long</c>、<c>ulong</c>）。
        /// </summary>
        /// <param name="obj">要验证的对象实例。</param>
        /// <returns>对象实例是整数类型则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsIntegerType(this Object obj)
        {
            if (obj == null) return false;

            return obj is int
                || obj is long
                || obj is byte
                || obj is short
                || obj is uint
                || obj is ulong
                || obj is sbyte
                || obj is ushort;
        }

        /// <summary>
        /// 验证类型是否是整数类型（<c>byte</c>、<c>sbyte</c>、<c>short</c>、<c>ushort</c>、<c>int</c>、<c>uint</c>、<c>long</c>、<c>ulong</c>）。
        /// </summary>
        /// <param name="type">要验证的类型。</param>
        /// <returns>类型是整数类型则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsIntegerType(this Type type)
        {
            if (type == null) return false;

            
            return Object.ReferenceEquals(type, ValueTypeConstants.Int32Type)
                || Object.ReferenceEquals(type, ValueTypeConstants.ByteType)
                || Object.ReferenceEquals(type, ValueTypeConstants.Int16Type)
                || Object.ReferenceEquals(type, ValueTypeConstants.Int64Type)
                || Object.ReferenceEquals(type, ValueTypeConstants.UInt32Type)
                || Object.ReferenceEquals(type, ValueTypeConstants.UInt16Type)
                || Object.ReferenceEquals(type, ValueTypeConstants.UInt64Type)
                || Object.ReferenceEquals(type, ValueTypeConstants.SByteType);
        }

        /// <summary>
        /// 验证对象是否是整数类型（<c>byte</c>、<c>sbyte</c>、<c>short</c>、<c>ushort</c>、<c>int</c>、<c>uint</c>、<c>long</c>、<c>ulong</c>）或枚举类型。
        /// </summary>
        /// <param name="obj">要验证的对象实例。</param>
        /// <returns>对象实例是整数类型或枚举类型则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsIntegerOrEnumType(this Object obj)
        {
            if (obj == null) return false;

            return IsIntegerType(obj) ? true : obj.GetType().IsEnum;
        }

        /// <summary>
        /// 验证类型是否是整数类型（<c>byte</c>、<c>sbyte</c>、<c>short</c>、<c>ushort</c>、<c>int</c>、<c>uint</c>、<c>long</c>、<c>ulong</c>）或枚举类型。
        /// </summary>
        /// <param name="type">要验证的类型。</param>
        /// <returns>类型是整数类型或枚举类型则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsIntegerOrEnumType(this Type type)
        {
            if (type == null) return false;

            return IsIntegerType(type) ? true : type.IsEnum;
        }
    }
}
