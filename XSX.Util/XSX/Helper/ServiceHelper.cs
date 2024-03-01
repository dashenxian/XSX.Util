using System;
using System.Runtime.InteropServices;
//using System.ServiceProcess;

namespace XSX.Helper
{
    public class ServiceHelper
    {
        /// <summary>
        /// 服务是否存在
        /// </summary>
        /// <param name="serviceName"></param>
        public static bool IsServiceExisted(string serviceName)
        {
            //判断当前操作系统是否是windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //判断windows服务是否存在
                var str = CmdHelper.RunCmdAndGetOutput($"sc query {serviceName}");
                return str.Contains("STATE");
            }
            //TODO 其他操作系统未实现
            throw new NotImplementedException("其他操作系统未实现");
        }
    }

}
