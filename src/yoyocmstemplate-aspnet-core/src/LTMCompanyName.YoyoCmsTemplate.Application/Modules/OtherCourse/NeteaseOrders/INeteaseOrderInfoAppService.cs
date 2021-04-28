using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders
{
    /// <summary>
    /// OrderInfo应用层服务的接口方法
    ///</summary>
    public interface INeteaseOrderInfoAppService : IApplicationService
    {
        /// <summary>
		/// 获取OrderInfo的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<NeteaseOrderInfoListDto>> GetPaged(QueryInput input);


        /// <summary>
        /// 查看实体详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<NeteaseOrderInfoEditDto> GetDetailsById(NullableIdDto<long> input);

        /// <summary>
        /// 修改QQ群审核状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        Task UpdateChecked(NullableIdDto<long> input, bool isChecked);

        /// <summary>
        /// 批量修改QQ群审核状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        Task BatchUpdateChecked(List<long> input, bool isChecked);

        /// <summary>
        /// 修改码云审核状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isChecked"></param>
        Task UpdateGiteeChecked(NullableIdDto<long> input, bool isChecked);

        /// <summary>
        /// 批量修改码云审核状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        Task BatchUpdateGiteeChecked(List<long> input, bool isChecked);

        /// <summary>
        /// 刷新某个订单前后一天的订单数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task RefreshOrderInfo(NullableIdDto<long> input);

        /// <summary>
        /// 根据课程名称统计实际收益
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<NeteaseOrderInfoStatisticsDto>> GetStatistics();

        /// <summary>
        /// 立即同步网易云课堂最新的数据
        /// </summary>
        /// <returns></returns>
        Task Synchronize();

      
    }
}
