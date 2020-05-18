/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  System.Decimal 类型相关的扩展方法。
 * 
 * 
 * Members Index:
 *      static class DecimalExtensions
 *              public static byte[] GetBytes(this decimal value)
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Extensions
{
    using System;


    /// <summary>
    /// <see cref="Decimal"/> 类型相关的扩展方法。
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// 将 <see cref="Decimal"/> 类型的值转换成 <see cref="byte[]"/> 。字节序由运行平台的主机决定。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <returns>等效于 <see cref="Decimal"/> 类型值的 <see cref="byte[]"/>。</returns>
        public static byte[] GetBytes(this decimal value)
        {
            byte[] result = new byte[16]; //16 : intArr.Length * 4

            var intArr = Decimal.GetBits(value);           

            for (int i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] == 0) continue;

                BitConverter.GetBytes(intArr[i]).CopyTo(result, i * 4);
            }

            return result;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// 尝试将 <see cref="Decimal"/> 类型转换成 <see cref="Byte"/> 集并写入到 <paramref name="span"/>。字节序由运行平台的主机决定。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="span">接收转换后的 <see cref="byte"/> 集的容器。</param>
        /// <returns>转换成功并写入 <paramref name="span"/> 则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool TryWriteBytes(this decimal value, Span<byte> span)
        {
            var intArr = Decimal.GetBits(value);

            for (int i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] == 0) continue;

                if (!BitConverter.TryWriteBytes(span.Slice(i * 4), intArr[i])) return false;
            }

            return true;
        }
#endif
    }
}
