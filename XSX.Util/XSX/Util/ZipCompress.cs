using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Checksum;
using XSX.Extension;

namespace XSX.Util
{

    /// <summary>
    /// 标准ZIP压缩文件处理
    /// </summary>
    public class ZipCompress : ICompress
    {
        #region 文件压缩

        ///// <summary>   
        ///// 压缩文件   
        ///// </summary>   
        ///// <param name="files">要压缩的文件路径列表</param>   
        ///// <param name="zipedFile">压缩后的文件路径</param>    
        //public void CompressFiles(IEnumerable<string> files, string zipedFile)
        //{
        //    CompressFiles(files, zipedFile, false);
        //}

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <param name="zipedFile">压缩后的文件路径</param>
        /// <param name="isGC">压缩完成后是否执行GC操作，压缩大文件时执行gc可以释放一些内存</param>    
        public void CompressFiles(IEnumerable<string> files, string zipedFile, bool isGC)
        {
            ZipEntry ent = null;
            //FileStream fs = null;
            Crc32 crc = new Crc32();
            if (!files.Any())
            {
                ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
                zipStream.Finish();
                zipStream.Close();
                return;
            }
            try
            {
                ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
                var offset = 0;
                foreach (string file in files)
                {
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        ent = new ZipEntry(Path.GetFileName(file));
                        ent.IsUnicodeText = true;
                        ent.DateTime = DateTime.Now;
                        ent.Size = fs.Length;

                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        ent.Crc = crc.Value;
                        zipStream.PutNextEntry(ent);
                        zipStream.Write(buffer, 0, buffer.Length);
                        offset += buffer.Length;
                    }
                }
                zipStream.Finish();
                zipStream.Close();
            }
            finally
            {
                //if (fs != null)
                //{
                //    fs.Close();
                //    fs.Dispose();
                //}
                //if (ent != null)
                //{
                //    ent = null;
                //}
                if (isGC)
                {
                    GC.Collect();
                }
            }
        }

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <param name="zipedFile">压缩后的文件路径</param>   
        public void CompressFiles(IEnumerable<string> files, string zipedFile)
        {
            using ZipFile zip = ZipFile.Create(zipedFile);
            zip.BeginUpdate();
            foreach (string file in files)
            {
                zip.Add(file, Path.GetFileName(file));
            }
            zip.CommitUpdate();
        }

        /// <summary>
        /// zip压缩文件
        /// </summary>
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <returns>返回压缩文件内存流</returns>
        public MemoryStream CompressFiles(IEnumerable<string> files)
        {
            return CompressFiles(files, false);
        }
        /// <summary>
        /// zip压缩文件
        /// </summary>
        /// <param name="files">要压缩的文件路径列表</param>   
        /// <param name="isGC">压缩完成后是否执行GC操作，压缩大文件时执行gc可以释放一些内存</param>   
        /// <returns>返回压缩文件内存流</returns>
        public MemoryStream CompressFiles(IEnumerable<string> files, bool isGC)
        {
            using (var memoryStream = new MemoryStream())
            {
                ZipEntry ent = null;
                //FileStream fs = null;
                Crc32 crc = new Crc32();
                if (!files.Any())
                {
                    return memoryStream;
                }
                try
                {
                    ZipOutputStream zipStream = new ZipOutputStream(memoryStream);
                    var offset = 0;
                    foreach (string file in files)
                    {
                        using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {

                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            ent = new ZipEntry(Path.GetFileName(file));
                            ent.IsUnicodeText = true;
                            ent.DateTime = DateTime.Now;
                            ent.Size = fs.Length;

                            fs.Close();

                            crc.Reset();
                            crc.Update(buffer);

                            ent.Crc = crc.Value;
                            zipStream.PutNextEntry(ent);
                            zipStream.Write(buffer, 0, buffer.Length);
                            offset += buffer.Length;
                        }
                    }
                    zipStream.Finish();
                    zipStream.Close();
                }
                finally
                {
                    //if (fs != null)
                    //{
                    //    fs.Close();
                    //    fs.Dispose();
                    //}
                    //if (ent != null)
                    //{
                    //    ent = null;
                    //}
                    if (isGC)
                    {
                        GC.Collect();
                    }
                }

                return memoryStream;
            }
        }
        /// <summary>
        /// 压缩指定文件夹内的所有文件和文件夹
        /// </summary>
        /// <param name="sourceFilePath">要压缩的文件夹路径</param>
        /// <param name="destinationZipFilePath">压缩文件目标路径</param>
        public void CreateZip(string sourceFilePath, string destinationZipFilePath)
        {
            sourceFilePath = sourceFilePath.EnsureEndsWith(Path.DirectorySeparatorChar);
            var dir = Path.GetDirectoryName(destinationZipFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath)))
            {
                zipStream.SetLevel(6);
                CreateZipFiles(sourceFilePath, zipStream, sourceFilePath);
                zipStream.Finish();
                zipStream.Close();
            }
        }

        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream, string staticFile)
        {
            var crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                if (Directory.Exists(file))
                {
                    //如果当前是文件夹，递归
                    CreateZipFiles(file, zipStream, staticFile);
                }
                else
                {
                    //如果是文件，开始压缩
                    using (var fs = File.OpenRead(file))
                    {
                        var buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        crc.Reset();
                        crc.Update(buffer);
                        fs.Close();

                        string tempFile = Path.GetRelativePath(staticFile,file);
                        ZipEntry entry = new ZipEntry(tempFile)
                        {
                            IsUnicodeText = true,
                            DateTime = DateTime.Now,
                            Size = buffer.Length,
                            Crc = crc.Value
                        };
                        zipStream.PutNextEntry(entry);

                        zipStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        #endregion 文件压缩

        #region 文件解压

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="zipFile">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public void UnCompress(string zipFile, string zipedFolder, string password)
        {
            //FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            if (!File.Exists(zipFile))
                throw new ArgumentException("zipFile文件不存在。");

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try
            {
                zipStream = new ZipInputStream(File.OpenRead(zipFile));
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }
                        var filePathDirectory = Path.GetDirectoryName(fileName);
                        if (!Directory.Exists(filePathDirectory))
                            Directory.CreateDirectory(filePathDirectory);

                        using (var fs = File.Create(fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[size];
                            while (true)
                            {
                                size = zipStream.Read(data, 0, data.Length);
                                if (size > 0)
                                    fs.Write(data, 0, size);
                                else
                                    break;
                            }
                        }
                    }
                }
            }
            finally
            {
                //if (fs != null)
                //{
                //    fs.Flush();
                //    fs.Close();
                //    fs.Dispose();
                //}
                if (ent != null)
                {
                    ent = null;
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                GC.Collect();
            }
        }

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <returns>解压结果</returns>   
        public void UnCompress(string fileToUnZip, string zipedFolder)
        {
            UnCompress(fileToUnZip, zipedFolder, null);
        }

        #endregion
    }

}
