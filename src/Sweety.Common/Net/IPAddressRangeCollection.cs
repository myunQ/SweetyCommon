/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  表示IP地址范围集合，并提供验证某个IP是否在此范围集合内。
 * 
 * 
 * Members Index:
 *      class IPAddressRangeCollection
 *              
 * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Sweety.Common.Net
{
    /// <summary>
    /// IP 地址范围集合，支持IPv4、IPv6
    /// </summary>
    public class IPAddressRangeCollection : List<IPAddressRange>
    {
        /// <summary>
        /// 初始化 Myun.Common.Net.IPAddressRangeCollection 类的新实例。
        /// </summary>
        public IPAddressRangeCollection() { }
        /// <summary>
        /// 初始化 Myun.Common.Net.IPAddressRangeCollection 类的新实例。
        /// </summary>
        /// <param name="collection">IP地址范围集合</param>
        public IPAddressRangeCollection(IEnumerable<IPAddressRange> collection)
            : base(collection)
        { }

        /// <summary>
        /// 判断IP地址是否在范围内，支持IPv4、IPv6。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址在范围内返回true，反则返回false</returns>
        public bool InRange(IPAddress ip)
        {
            if (ip == null) throw new ArgumentNullException("ip");

            var ipValue = ip.GetAddressBytes();

            return this.Any(p => p.InRange(ipValue));
        }
        /// <summary>
        /// 判断IP地址是否在范围内，支持IPv4、IPv6。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址在范围内返回true，反则返回false</returns>
        public bool InRange(string ip)
        {
            if (ip == null)
                throw new ArgumentNullException("ip");

            if (!IPAddress.TryParse(ip, out var ipAddress))
            {
                throw new ArgumentException("无效的IP地址");
            }

            var ipValue = ipAddress.GetAddressBytes();

            return this.Any(p => p.InRange(ipValue));
        }
    }
}
