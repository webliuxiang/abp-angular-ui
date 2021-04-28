using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs
{
    /// <summary>
    /// DownloadLog应用层服务的接口方法
    ///</summary>
    public interface IDownloadLogAppService : IApplicationService
    {
        /// <summary>
		/// 获取DownloadLog的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DownloadLogListDto>> GetPaged(GetDownloadLogsInput input);


        /// <summary>
        /// 通过指定id获取DownloadLogListDto信息
        /// </summary>
        Task<DownloadLogListDto> GetById(EntityDto<long> input);

        ///// <summary>
        ///// 导出DownloadLog为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
