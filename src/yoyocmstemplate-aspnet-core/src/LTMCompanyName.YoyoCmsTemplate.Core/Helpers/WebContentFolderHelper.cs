using System;
using System.IO;
using System.Linq;
using Abp.Reflection.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Helpers
{
    /// <summary>
    /// This class is used to find root path of the web project in;
    /// unit tests (to find views) and entity framework core command line commands (to find conn string).
    /// </summary>
    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(YoyoCmsTemplateCoreModule).GetAssembly().Location);
            if (coreAssemblyDirectoryPath == null)
            {
                throw new Exception("找不到程序集 LTMCompanyName.YoyoCmsTemplate.Core !");
            }

            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            while (!DirectoryContains(directoryInfo.FullName, "LTMCompanyName.YoyoCmsTemplate.sln"))
            {
                if (directoryInfo.Parent == null)
                {
                    throw new Exception("找不到项目根目录!");
                }

                directoryInfo = directoryInfo.Parent;
            }

            var webHostFolder = Path.Combine(directoryInfo.FullName, "src", "LTMCompanyName.YoyoCmsTemplate.Web.Host");
            if (Directory.Exists(webHostFolder))
            {
                Console.WriteLine("当前使用 LTMCompanyName.YoyoCmsTemplate.Web.Host 项目启动");
                return webHostFolder;
            }

            var webMvcFolder = Path.Combine(directoryInfo.FullName, "src", "LTMCompanyName.YoyoCmsTemplate.Web.Mvc");
            if (Directory.Exists(webMvcFolder))
            {
                Console.WriteLine("当前使用 LTMCompanyName.YoyoCmsTemplate.Web.Mvc 项目启动");
                return webMvcFolder;
            }

            var webPortalFolder = Path.Combine(directoryInfo.FullName, "src", "LTMCompanyName.YoyoCmsTemplate.Web.Portal");
            if (Directory.Exists(webPortalFolder))
            {
                Console.WriteLine("当前使用 LTMCompanyName.YoyoCmsTemplate.Web.Portal 项目启动");
                return webPortalFolder;
            }

            throw new Exception("无法找到web项目的根文件夹!");
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }
}
