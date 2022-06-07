using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
    }
}
