using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSX.Extension
{
    public static class StreamExtensions
    {
        /// <summary>
        /// 读取stream中的字节
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] GetAllBytes(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }


    }
}
