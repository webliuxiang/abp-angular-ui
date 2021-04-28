using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using L._52ABP.Application.Dtos;
using L._52ABP.Common.Net.MimeTypes;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Logging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.IO;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Logging
{
    /// <summary>
    ///网站日志
    /// </summary>
    public class WebSiteLogAppService : YoyoCmsTemplateAppServiceBase, IWebSiteLogAppService
    {
        private readonly IAppFolder _appFolders;
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;
        public WebSiteLogAppService(IAppFolder appFolders, IDataTempFileCacheManager dataTempFileCacheManager)
        {
            _appFolders = appFolders;
            _dataTempFileCacheManager = dataTempFileCacheManager;
        }
        /// <summary>
        /// 获取最新的网站日志信息
        /// </summary>
        /// <returns></returns>
        public GetLatestWebLogsOutput GetLatestWebLogs()
        {
            var directory = new DirectoryInfo(_appFolders.WebSiteLogsFolder);
            if (!directory.Exists)
            {
                return new GetLatestWebLogsOutput { LatestWebLogLines = new List<string>() };
            }
            var lastLogFile = directory.GetFiles("*.txt", SearchOption.AllDirectories)
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();

            if (lastLogFile == null) return new GetLatestWebLogsOutput();

            var lines = AppFileHelper.ReadLines(lastLogFile.FullName).Reverse().Take(1000).ToList();
            var logLineCount = 0;
            var lineCount = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("DEBUG") ||
                    line.StartsWith("INFO") ||
                    line.StartsWith("WARN") ||
                    line.StartsWith("ERROR") ||
                    line.StartsWith("FATAL"))
                    logLineCount++;

                lineCount++;

                if (logLineCount == 100) break;
            }

            return new GetLatestWebLogsOutput
            {
                LatestWebLogLines = lines.Take(lineCount).Reverse().ToList()
            };
        }
        /// <summary>
        ///下载日志文件压缩包
        /// </summary>
        /// <returns></returns>
        public FileDto DownloadWebLogs()
        {
            //获取所有的日志文件的路径信息
            var logFiles = GetAllLogFiles();

            //生成名为WebSiteLogs.zip的压缩包
            var zipFileDto = new FileDto("WebSiteLogs.zip", MimeTypeNames.ApplicationZip);

            using (var outputZipFileStream = new MemoryStream())
            {
                using (var zipStream = new ZipArchive(outputZipFileStream, ZipArchiveMode.Create))
                {
                    foreach (var logFile in logFiles)
                    {
                        var entry = zipStream.CreateEntry(logFile.Name);
                        using (var entryStream = entry.Open())
                        {
                            using (var fs = new FileStream(logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
                            {
                                fs.CopyTo(entryStream);
                                entryStream.Flush();
                            }
                        }
                    }
                }
                _dataTempFileCacheManager.SetFile(zipFileDto.FileToken, outputZipFileStream.ToArray());
            }



            return zipFileDto;
        }



        private List<FileInfo> GetAllLogFiles()
        {
            var directory = new DirectoryInfo(_appFolders.WebSiteLogsFolder);
            return directory.GetFiles("*.*", SearchOption.TopDirectoryOnly).ToList();
        }
    }
}