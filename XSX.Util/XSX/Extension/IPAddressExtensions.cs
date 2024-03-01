using System;
using System.Net;
using System.Net.Sockets;

namespace XSX.Extension
{
    public static class IPAddressExtensions
    {
        /// <summary>
        /// 获取可用的端口
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static int GetAvailablePort(this IPAddress ip)
        {
            using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(ip, 0));
            socket.Listen(1);
            var ipEndPoint = (IPEndPoint)socket.LocalEndPoint;
            var port = ipEndPoint.Port;
            return port;
        }
        /// <summary>
        /// 判断IP地址在不在某个IP地址段
        /// </summary>
        /// <param name="input">需要判断的IP地址</param>
        /// <param name="begin">起始地址</param>
        /// <param name="ends">结束地址</param>
        /// <returns></returns>
        public static bool IpAddressInRange(this string input, string begin, string ends)
        {
            uint current = IPToID(input);
            return current >= IPToID(begin) && current <= IPToID(ends);
        }
        /// <summary>
        /// IP地址转换成数字
        /// </summary>
        /// <param name="addr">IP地址</param>
        /// <returns>数字,输入无效IP地址返回0</returns>
        private static uint IPToID(string addr)
        {
            if (!IPAddress.TryParse(addr, out var ip))
            {
                return 0;
            }

            byte[] bInt = ip.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bInt);
            }

            return BitConverter.ToUInt32(bInt, 0);
        }
    }
}
