using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace XSX.Util.XSX.Helper
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// 从文件夹路径加载程序集
        /// </summary>
        /// <param name="folderPath">文件夹</param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<Assembly> LoadAssemblies(string folderPath, SearchOption searchOption)
        {
            return GetAssemblyFiles(folderPath, searchOption)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToList();
        }
        /// <summary>
        /// 获取所有程序集文件
        /// </summary>
        /// <param name="folderPath">文件夹</param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAssemblyFiles(string folderPath, SearchOption searchOption)
        {
            return Directory
                .EnumerateFiles(folderPath, "*.*", searchOption)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));
        }
        /// <summary>
        /// 获取程序集中的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IReadOnlyList<Type> GetAllTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
        }
    }
}
