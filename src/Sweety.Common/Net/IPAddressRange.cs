/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  表示IP地址范围，并提供验证某个IP是否在此范围内。
 * 
 * 
 * Members Index:
 *      class IPAddressRange
 *              
 * * * * * * * * * * * * * * * * * * * * */


using System;
using System.Net;
using System.Net.Sockets;

namespace Sweety.Common.Net
{
    /// <summary>
    /// IP 地址范围，支持IPv4、IPv6
    /// </summary>
    public class IPAddressRange
    {
        byte[] _start;
        byte[] _end;

        /// <summary>
        /// 初始化 Myun.Common.Net.IPAddressRange 类的新实例。
        /// </summary>
        private IPAddressRange() { }
        /// <summary>
        /// 初始化 Myun.Common.Net.IPAddressRange 类的新实例。
        /// </summary>
        /// <param name="startIP">起始IP地址，支持IPv4、IPv6两种格式</param>
        /// <param name="endIP">结束IP地址，支持IPv4、IPv6两种格式</param>
        public IPAddressRange(string startIP, string endIP)
        {
            if (String.IsNullOrEmpty(startIP)) throw new ArgumentNullException("请输入起始IP地址。", CanOutputException.DefaultInstance);
            if (String.IsNullOrEmpty(endIP)) throw new ArgumentNullException("请输入结束IP地址。", CanOutputException.DefaultInstance);

            if (!IPAddress.TryParse(startIP, out var ip))
            {
                throw new ArgumentException("起始IP地址“" + startIP + "”无效。", CanOutputException.DefaultInstance);
            }

            this.Start = ip;

            if (startIP != endIP)
            {
                if (!IPAddress.TryParse(endIP, out ip))
                {
                    throw new ArgumentException("结束IP地址“" + endIP + "”无效。", CanOutputException.DefaultInstance);
                }

                if (ip.AddressFamily != this.Start.AddressFamily) throw new ArgumentException("起始IP与结束IP版本不同。", CanOutputException.DefaultInstance);
            }

            this.End = ip;
            this.Initialize();
        }
        /// <summary>
        /// 初始化 Myun.Common.Net.IPAddressRange 类的新实例。
        /// </summary>
        /// <param name="start">起始IP地址，支持IPv4、IPv6两种格式</param>
        /// <param name="end">结束IP地址，支持IPv4、IPv6两种格式</param>
        public IPAddressRange(IPAddress start, IPAddress end)
        {
            this.Start = start ?? throw new ArgumentNullException("起始IP地址不能为 null。", CanOutputException.DefaultInstance);

            if (end == null) throw new ArgumentNullException("结束IP地址不能为 null。", CanOutputException.DefaultInstance);

            // 从执行效率触发 IPv4 使用 Equals() 比较，IPv6 使用 GetHashCode() 比较。
            if ((start.AddressFamily == AddressFamily.InterNetwork && start.Equals(end)) || (start.AddressFamily == AddressFamily.InterNetworkV6 && start.GetHashCode() == end.GetHashCode()))
            {
                this.End = start;
            }
            else
            {
                if (start.AddressFamily != end.AddressFamily) throw new ArgumentException("起始IP与结束IP版本不同。", CanOutputException.DefaultInstance);
                this.End = end;
            }
            this.Initialize();
        }

        /// <summary>
        /// 获取起始IP地址
        /// </summary>
        public IPAddress Start { get; private set; }
        /// <summary>
        /// 获取结束IP地址
        /// </summary>
        public IPAddress End { get; private set; }

        /// <summary>
        /// 判断IP地址是否在范围内，支持IPv4、IPv6。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址在范围内返回true，反则返回false</returns>
        public bool InRange(IPAddress ip)
        {
            if (ip == null)
                throw new ArgumentNullException("请传入IP地址。", CanOutputException.DefaultInstance);

            if (ip.AddressFamily != this.Start.AddressFamily) return false;

            return this.InRange(ip.GetAddressBytes());
        }

        /// <summary>
        /// 判断IP地址是否在范围内，支持IPv4、IPv6。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址在范围内返回true，反则返回false</returns>
        public bool InRange(string ip)
        {
            if (String.IsNullOrEmpty(ip))
                throw new ArgumentNullException("请传入IP地址。", CanOutputException.DefaultInstance);

            if (!IPAddress.TryParse(ip, out var ipvalue))
            {
                throw new ArgumentException(ip + "不是一个有效的IP地址。", CanOutputException.DefaultInstance);
            }

            return this.InRange(ipvalue);
        }
        /// <summary>
        /// 判断IP地址是否在范围内，支持IPv4、IPv6。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址在范围内返回true，反则返回false</returns>
        internal bool InRange(byte[] ip)
        {
            if (_start.Length != ip.Length) return false;

            bool greaterStart = false;
            bool lessEnd = false;
            int length = ip.Length;

            for (int i = 0; i < length; i++)
            {
                if (_start[i] < _end[i])
                {
                    if (!greaterStart && _start[i] < ip[i]) greaterStart = true;
                    if (!lessEnd && ip[i] < _end[i]) lessEnd = true;

                    if (greaterStart && lessEnd)
                    {
                        return true;
                    }
                    else if (!greaterStart && _start[i] != ip[i])
                    {
                        return false;
                    }
                    else if (!lessEnd && _end[i] != ip[i])
                    {
                        return false;
                    }
                }
                else if (_start[i] != ip[i]) //已经校验了_start不会小于_end，这里就不再重复 (_start[i] == _end[i] && _start[i] != ipValue[i]) 的判断。
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将起始IP地址与结束IP地址转换成数字类型，方便比对。
        /// 对起始地址是否大于结束地址进行校验。
        /// </summary>
        private void Initialize()
        {
            _start = this.Start.GetAddressBytes();
            if (Object.ReferenceEquals(this.Start, this.End))
            {
                _end = _start;
                return;
            }
            _end = this.End.GetAddressBytes();

            int length = _start.Length;
            for (int i = 0; i < length; i++)
            {
                if (_start[i] < _end[i])
                {
                    break;
                }
                else if (_start[i] == _end[i])
                {
                    continue;
                }
                throw new ArgumentException("起始IP地址大于结束IP地址。", CanOutputException.DefaultInstance);
            }
        }
    }
}
