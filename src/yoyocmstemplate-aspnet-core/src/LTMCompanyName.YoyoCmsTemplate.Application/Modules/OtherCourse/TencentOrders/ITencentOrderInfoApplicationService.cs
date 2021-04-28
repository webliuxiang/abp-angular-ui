using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders
{
    /// <summary>
    /// TencentOrderInfo应用层服务的接口方法
    ///</summary>
    public interface ITencentOrderInfoAppService : IApplicationService
    {
        /// <summary>
		/// 获取TencentOrderInfo的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TencentOrderInfoListDto>> GetPaged(GetTencentOrderInfosInput input);


		/// <summary>
		/// 通过指定id获取TencentOrderInfoListDto信息
		/// </summary>
		Task<TencentOrderInfoListDto> GetById(EntityDto<long> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTencentOrderInfoForEditOutput> GetForEdit(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改TencentOrderInfo的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateTencentOrderInfoInput input);


        /// <summary>
        /// 删除TencentOrderInfo信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);


        /// <summary>
        /// 批量删除TencentOrderInfo
        /// </summary>
        Task BatchDelete(List<long> input);


        /// <summary>
        /// 删除同步的后台作业job信息
        /// </summary>
        /// <returns></returns>
        Task DeleteSyncBackgroundJobs();

        /// <summary>
        /// 通过腾讯课程订单号来绑定信息
        /// </summary>
        /// <returns></returns>
        Task SyncTencentOrderToL52ABPCourseOrder(EntityDto<long> input);




        ///// <summary>
        ///// 导出TencentOrderInfo为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
