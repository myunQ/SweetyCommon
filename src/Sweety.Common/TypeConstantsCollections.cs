/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  这个文件里存放定义常用类型的类型常量。
 * 
 * Members Index:
 *      static class ValueTypeConstants
 *      static class ReferenceTypeConstants
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;
    using System.Net;


    /// <summary>
    /// 值类型的 <see cref="Type"/> 对象实例。
    /// </summary>
    public static class ValueTypeConstants
    {
        /// <summary>
        /// 表示<see cref="Boolean"/>类型。
        /// </summary>
        public readonly static Type BooleanType = typeof(bool);

        /// <summary>
        /// 表示<see cref="Char"/>[]类型。
        /// </summary>
        public readonly static Type CharsType = typeof(char[]);
        /// <summary>
        /// 表示<see cref="Char"/>类型
        /// </summary>
        public readonly static Type CharType = typeof(char);

        /// <summary>
        /// 表示<see cref="Byte"/>[]类型。
        /// </summary>
        public readonly static Type BytesType = typeof(byte[]);
        /// <summary>
        /// 表示<see cref="Byte"/>类型。
        /// </summary>
        public readonly static Type ByteType = typeof(byte);
        /// <summary>
        /// 表示<see cref="SByte"/>类型。
        /// </summary>
        public readonly static Type SByteType = typeof(sbyte);

        /// <summary>
        /// 表示<see cref="Int16"/>类型。
        /// </summary>
        public readonly static Type Int16Type = typeof(short);
        /// <summary>
        /// 表示<see cref="UInt16"/>类型。
        /// </summary>
        public readonly static Type UInt16Type = typeof(ushort);

        /// <summary>
        /// 表示<see cref="Int32"/>类型。
        /// </summary>
        public readonly static Type Int32Type = typeof(int);
        /// <summary>
        /// 表示<see cref="UInt32"/>类型。
        /// </summary>
        public readonly static Type UInt32Type = typeof(uint);

        /// <summary>
        /// 表示<see cref="Int64"/>类型。
        /// </summary>
        public readonly static Type Int64Type = typeof(long);
        /// <summary>
        /// 表示<see cref="UInt64"/>类型。
        /// </summary>
        public readonly static Type UInt64Type = typeof(ulong);

        /// <summary>
        /// 表示<see cref="Single"/>类型。
        /// </summary>
        public readonly static Type SingleType = typeof(float);
        /// <summary>
        /// 表示<see cref="Double"/>类型。
        /// </summary>
        public readonly static Type DoubleType = typeof(double);
        /// <summary>
        /// 表示<see cref="Decimal"/>类型。
        /// </summary>
        public readonly static Type DecimalType = typeof(decimal);

        /// <summary>
        /// 表示<see cref="TimeSpan"/>类型。
        /// </summary>
        public readonly static Type TimeSpanType = typeof(TimeSpan);

        /// <summary>
        /// 表示<see cref="DateTime"/>类型。
        /// </summary>
        public readonly static Type DateTimeType = typeof(DateTime);
        /// <summary>
        /// 表示<see cref="DateTimeOffset"/>类型。
        /// </summary>
        public readonly static Type DateTimeOffsetType = typeof(DateTimeOffset);

#if NET6_0_OR_GREATER
        /// <summary>
        /// 表示<see cref="DateOnly"/>类型。
        /// </summary>
        public readonly static Type DateOnlyType = typeof(DateOnly);
        /// <summary>
        /// 表示<see cref="TimeOnly"/>类型。
        /// </summary>
        public readonly static Type TimeOnlyType = typeof(TimeOnly);
#endif

        /// <summary>
        /// 表示<see cref="Guid"/>类型。
        /// </summary>
        public readonly static Type GuidType = typeof(Guid);
    }

    /// <summary>
    /// 引用类型的 <see cref="Type"/> 对象实例。
    /// </summary>
    public static class ReferenceTypeConstants
    {
        /// <summary>
        /// 表示<see cref="String"/>类型。
        /// </summary>
        public readonly static Type StringType = typeof(string);

        /// <summary>
        /// 表示<see cref="IPAddress"/>类型。
        /// </summary>
        public readonly static Type IPAddressType = typeof(IPAddress);
    }
}
