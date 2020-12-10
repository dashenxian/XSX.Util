using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSX.Util
{
    public interface ICompress
    {
        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <param name="zipedFile">压缩后的文件路径</param>    
        void CompressFiles(IEnumerable<string> files, string zipedFile);

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <param name="zipedFile">压缩后的文件路径</param>
        /// <param name="isGC">压缩完成后是否执行GC操作，压缩大文件时执行gc可以释放一些内存</param>    
        void CompressFiles(IEnumerable<string> files, string zipedFile, bool isGC);

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <returns>返回压缩文件内存流</returns>
        MemoryStream CompressFiles(IEnumerable<string> files);

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <param name="isGC">压缩完成后是否执行GC操作，压缩大文件时执行gc可以释放一些内存</param>   
        /// <returns>返回压缩文件内存流</returns>
        MemoryStream CompressFiles(IEnumerable<string> files, bool isGC);

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="zipFile">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        void UnCompress(string zipFile, string zipedFolder, string password);

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <returns>解压结果</returns>   
        void UnCompress(string fileToUnZip, string zipedFolder);
    }

}
