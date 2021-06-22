/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  数据转换常用操作方法。
 * 
 * Members Index:
 *      static class InternalConvertUtility
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Converter
{
    using System;
    using System.ComponentModel;
    using System.Net;

    using Sweety.Common.Verification;


    internal static class InternalConvertUtility
    {
        /// <summary>
        /// 对可空类型进行判断转换
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="conversionType">属性类型</param>
        /// <returns>属性值</returns>
        internal static object HackType(object value, Type conversionType)
        {
            /*
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) return null;

                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            */

            Type nullableUnderlyingType = Nullable.GetUnderlyingType(conversionType);
            if (nullableUnderlyingType != null)
            {
                conversionType = nullableUnderlyingType;
            }

            Type valueType = value.GetType();
            if (conversionType.IsEnum)
            {
                if (TypeVerification.IsIntegerOrEnumType(valueType))
                {
                    //value的值为枚举值
                    return Enum.ToObject(conversionType, value);
                }
                else
                {
                    //value的值为枚举名
                    return Enum.Parse(conversionType, value.ToString());
                }
            }


            if (Object.ReferenceEquals(valueType, conversionType)) return value;

            // 字符串 GUID 转 System.Guid。
            if (conversionType == ValueTypeConstants.GuidType && valueType == ReferenceTypeConstants.StringType)
            {
                return new Guid(value.ToString());
            }

            // System.Guid 转 字符串 GUID
            if (valueType == ValueTypeConstants.GuidType && conversionType == ReferenceTypeConstants.StringType)
            {
                return value.ToString();
            }

            //字符串IP地址转换成 System.Net.IPAddress 类型
            if (conversionType == ReferenceTypeConstants.IPAddressType)
            {
                if (valueType == ReferenceTypeConstants.StringType) return IPAddress.Parse(value.ToString());
                if (valueType == ValueTypeConstants.BytesType) return new IPAddress((byte[])value);
                throw new InvalidCastException(String.Format(Properties.Localization.the_value_XXX_of_type_XXXType_cannot_be_converted_to_a_value_of_type_IPAddressType, value, valueType, ReferenceTypeConstants.IPAddressType));
            }
            else
            {
                return Convert.ChangeType(value, conversionType);
            }
        }
    }
}
