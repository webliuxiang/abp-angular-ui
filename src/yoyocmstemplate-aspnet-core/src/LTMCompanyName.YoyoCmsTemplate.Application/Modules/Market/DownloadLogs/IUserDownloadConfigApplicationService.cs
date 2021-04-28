using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs
{
    /// <summary>
    /// UserDownloadConfig应用层服务的接口方法
    ///</summary>
    public interface IUserDownloadConfigAppService : IApplicationService
    {
        /// <summary>
		/// 获取UserDownloadConfig的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserDownloadConfigListDto>> GetPaged(GetUserDownloadConfigsInput input);


        /// <summary>
        /// 通过指定id获取UserDownloadConfigListDto信息
        /// </summary>
        Task<UserDownloadConfigListDto> GetById(EntityDto<long> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserDownloadConfigForEditOutput> GetForEdit(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改UserDownloadConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateUserDownloadConfigInput input);


        /// <summary>
        /// 删除UserDownloadConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);


        /// <summary>
        /// 批量删除UserDownloadConfig
        /// </summary>
        Task BatchDelete(List<long> input);


        ///// <summary>
        ///// 导出UserDownloadConfig为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
