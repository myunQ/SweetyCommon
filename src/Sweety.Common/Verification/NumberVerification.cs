/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          NumberVerification
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Verification
{
    using System;


    /// <summary>
    /// 号码验证。
    /// </summary>
    public static class NumberVerification
    {
        /// <summary>
        /// 是否是移动电话的号码。
        /// </summary>
        /// <param name="number">号码。</param>
        /// <returns>如果是移动电话的号码则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsMobilePhoneNumber(this string number)
        {
            if (String.IsNullOrEmpty(number)) return false;

            //TODO:myun，目前已中国移动电话号码为标准，有机会增加区域性验证。
            if (number.Length != 11 || number[0] != '1') return false;

            return StringVerification.IsSBCDigit(number);
        }

        /// <summary>
        /// 是否是固定电话的号码。
        /// </summary>
        /// <param name="number">号码。</param>
        /// <returns>如果是固定电话的号码则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsTelephoneNumber(this string number)
        {
            //TODO:myun
            throw new NotImplementedException();
        }
    }
}
