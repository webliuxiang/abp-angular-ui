using System.IO;
using System.IO.Compression;
using Abp.UI;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.AbpProject
{
    public class AbpProjectAppService : YoyoCmsTemplateAppServiceBase, IAbpProjectAppService
    {
        /// <summary>
        /// 生成ABP项目的ZIP文件
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="filePath"></param>
        public void GenerateAbpProjectZip(string dirPath, string filePath)
        {
            //如果待压缩的目录不存在，则报错
            if (!Directory.Exists(dirPath)) throw new UserFriendlyException($"指定的目录:  {dirPath}  不存在!");

            ZipFile.CreateFromDirectory(dirPath, filePath, CompressionLevel.Fastest, true);
        }
    }
}
