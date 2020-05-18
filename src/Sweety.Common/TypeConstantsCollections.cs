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


    /// <summary>
    /// 值类型的 <see cref="Type"/> 对象实例。
    /// </summary>
    public static class ValueTypeConstants
    {
        public readonly static Type BooleanType = typeof(bool);

        public readonly static Type CharsType = typeof(char[]);
        public readonly static Type CharType = typeof(char);

        public readonly static Type BytesType = typeof(byte[]);
        public readonly static Type ByteType = typeof(byte);
        public readonly static Type SByteType = typeof(sbyte);

        public readonly static Type Int16Type = typeof(short);
        public readonly static Type UInt16Type = typeof(ushort);

        public readonly static Type Int32Type = typeof(int);
        public readonly static Type UInt32Type = typeof(uint);

        public readonly static Type Int64Type = typeof(long);
        public readonly static Type UInt64Type = typeof(ulong);

        public readonly static Type SingleType = typeof(float);
        public readonly static Type DoubleType = typeof(double);
        public readonly static Type DecimalType = typeof(decimal);

        public readonly static Type TimeSpanType = typeof(TimeSpan);

        public readonly static Type DateTimeType = typeof(DateTime);
        public readonly static Type DateTimeOffsetType = typeof(DateTimeOffset);

        public readonly static Type GuidType = typeof(Guid);
    }

    /// <summary>
    /// 引用类型的 <see cref="Type"/> 对象实例。
    /// </summary>
    public static class ReferenceTypeConstants
    {
        public readonly static Type StringType = typeof(string);

        public readonly static Type IPAddressType = typeof(System.Net.IPAddress);
    }
}
