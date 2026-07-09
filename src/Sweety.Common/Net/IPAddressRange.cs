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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace Sweety.Common.Net
{
    /// <summary>
    /// IP 地址范围，支持IPv4、IPv6
    /// </summary>
    public class IPAddressRange
    {
        byte[] _start;
        byte[] _end;
        uint? _ipv4Count = null;
        BigInteger? _count = null;


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

            Start = ip;

            if (startIP != endIP)
            {
                if (!IPAddress.TryParse(endIP, out ip))
                {
                    throw new ArgumentException("结束IP地址“" + endIP + "”无效。", CanOutputException.DefaultInstance);
                }

                if (ip.AddressFamily != Start.AddressFamily) throw new ArgumentException("起始IP与结束IP版本不同。", CanOutputException.DefaultInstance);
            }

            End = ip;
            Initialize();
        }
        /// <summary>
        /// 初始化 Myun.Common.Net.IPAddressRange 类的新实例。
        /// </summary>
        /// <param name="start">起始IP地址，支持IPv4、IPv6两种格式</param>
        /// <param name="end">结束IP地址，支持IPv4、IPv6两种格式</param>
        public IPAddressRange(IPAddress start, IPAddress end)
        {
            Start = start ?? throw new ArgumentNullException("起始IP地址不能为 null。", CanOutputException.DefaultInstance);

            if (end == null) throw new ArgumentNullException("结束IP地址不能为 null。", CanOutputException.DefaultInstance);

            // 从执行效率出发 IPv4 使用 Equals() 比较，IPv6 使用 GetHashCode() 比较。
            if ((start.AddressFamily == AddressFamily.InterNetwork && start.Equals(end)) || (start.AddressFamily == AddressFamily.InterNetworkV6 && start.GetHashCode() == end.GetHashCode()))
            {
                End = start;
            }
            else
            {
                if (start.AddressFamily != end.AddressFamily) throw new ArgumentException("起始IP与结束IP版本不同。", CanOutputException.DefaultInstance);
                End = end;
            }
            Initialize();
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
        /// 获取 IP v4 地址的数量，如果不是IP v4 地址则返回<see cref="uint.MinValue"/>。
        /// </summary>
        public uint IPv4Count
        {
            get
            {
                if (!_ipv4Count.HasValue)
                {
                    if (Start.AddressFamily != AddressFamily.InterNetwork)
                    {
                        _ipv4Count = uint.MinValue;
                    }
                    else
                    {
                        _ipv4Count = BitConverter.ToUInt32(_end, 0) - BitConverter.ToUInt32(_start, 0) + 1U;
                    }
                }
                
                return _ipv4Count.Value;
            }
        }

        /// <summary>
        /// 获取IP地址范围包含的IP地址数量。
        /// </summary>
        public BigInteger Count
        {
            get
            {
                if (!_count.HasValue)
                {
                    _count = BytesToBigInteger(_end) - BytesToBigInteger(_start) + 1;
                }

                return _count.Value;
            }
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        private static BigInteger BytesToBigInteger(ReadOnlySpan<byte> bytes)
        {
            Span<byte> buf = stackalloc byte[bytes.Length + 1];
            bytes.CopyTo(buf[1..]);
            if (BitConverter.IsLittleEndian)
            {
                buf.Reverse();
            }

            return new BigInteger(buf);
        }
#else
        private static BigInteger BytesToBigInteger(byte[] bytes)
        {
            byte[] buf = new byte[bytes.Length + 1];
            bytes.CopyTo(buf, 1);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }

            return new BigInteger(buf);
        }
#endif


        /// <summary>
        /// 生成一个列表，列表中按顺序包含从<see cref="Start"/>（含）至 <see cref="End"/>（含）的每一个IP地址。
        /// </summary>
        /// <param name="maxAllowCount">如果<see cref="Count"/>大于此值将抛出异常。</param>
        /// <returns>返回按顺序包含从<see cref="Start"/>（含）至 <see cref="End"/>（含）的所有IP的列表。</returns>
        /// <exception cref="InvalidOperationException">当IP范围内的IP数量超过<paramref name="maxAllowCount"/>时抛出此异常。</exception>
        public IList<IPAddress> GenerateList(int maxAllowCount = 256)
        {
            if (Count > maxAllowCount)
            {
                throw new InvalidOperationException($"IP区间总量{Count}超过最大允许上限{maxAllowCount}。");
            }

#if NETSTANDARD2_0
            var current = (byte[])_start.Clone();
#else
            Span<byte> current = stackalloc byte[_start.Length];
            _start.AsSpan().CopyTo(current);
#endif

            IList<IPAddress> list = new List<IPAddress>();
            while (true)
            {
                list.Add(new IPAddress(current));

                // 判断是否到达终点
                if (current.SequenceEqual<byte>(_end))
                {
                    return list;
                }


                for (int i = current.Length - 1; i >= 0; i--)
                {
                    if (current[i] == 255)
                        current[i] = 0;
                    else
                    {
                        current[i]++;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 判断IP地址是否在范围内，支持IPv4、IPv6。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址在范围内返回true，反则返回false</returns>
        public bool InRange(IPAddress ip)
        {
            if (ip == null)
                throw new ArgumentNullException("请传入IP地址。", CanOutputException.DefaultInstance);

            if (ip.AddressFamily != Start.AddressFamily) throw new ArgumentException("IP版本不一致。", nameof(ip));

            return InRange(ip.GetAddressBytes());
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

            return InRange(ipvalue);
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
            _start = Start.GetAddressBytes();
            if (Object.ReferenceEquals(Start, End))
            {
                _end = _start;
                return;
            }
            _end = End.GetAddressBytes();

            for (int i = 0; i < _start.Length; i++)
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

        public class Enumerator
        {
            IPAddressRange _range;
            IPAddress _current = IPAddress.None;
            byte[] _currentBytes = Array.Empty<byte>();

            public Enumerator(IPAddressRange range)
            {
                _range = range;
            }

            public bool MoveNext()
            {
                if (_currentBytes.Length == 0)
                {
                    _currentBytes = (byte[])_range._start.Clone();
                }
                else
                {
                    // 判断是否到达终点
                    if (_currentBytes.SequenceEqual<byte>(_range._end))
                    {
                        _current = IPAddress.None;
                        return false;
                    }

                    for (int i = _currentBytes.Length - 1; i >= 0; i--)
                    {
                        if (_currentBytes[i] == 255)
                            _currentBytes[i] = 0;
                        else
                        {
                            _currentBytes[i]++;
                            break;
                        }
                    }
                }

                _current = new IPAddress(_currentBytes);
                return true;
            }

            public IPAddress Current => _current;
        }
    }
}
