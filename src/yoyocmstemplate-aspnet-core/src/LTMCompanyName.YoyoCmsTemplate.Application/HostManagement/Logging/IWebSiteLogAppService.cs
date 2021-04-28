using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Logging.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Logging
{
    public interface IWebSiteLogAppService
    {
        /// <summary>
        /// 获取最新的网站日志信息
        /// </summary>
        /// <returns></returns>
        GetLatestWebLogsOutput GetLatestWebLogs();

        /// <summary>
        /// 下载网站日志信息
        /// </summary>
        /// <returns></returns>
        FileDto DownloadWebLogs();
    }
}