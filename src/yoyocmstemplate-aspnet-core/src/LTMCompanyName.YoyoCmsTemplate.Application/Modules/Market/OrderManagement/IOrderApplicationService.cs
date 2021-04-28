using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Application.Services.Dto;

using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement
{
    /// <summary>
    /// Order应用层服务的接口方法
    ///</summary>
    public interface IOrderAppService : IApplicationService
    {
        /// <summary>
		/// 获取Order的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderListDto>> GetPaged(GetOrdersInput input);


        /// <summary>
        /// 通过指定id获取OrderListDto信息
        /// </summary>
        Task<OrderListDto> GetById(EntityDto<Guid> input);



        /// <summary>
        /// 删除Order信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Order
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 用户中心获取订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserCenterOrderListDto>> GetUserCenterPaged(GetOrdersInput input);


        /// <summary>
        /// 统计产品销售额度，每月统计
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<OrderStatisticsDto>> GetProductOrderStatistics();


        /// <summary>
        /// 更新订单价格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateOraderPrice(OrderEditPriceDto input);

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateOrder(OrderEditDto input);


        /// <summary>
        /// 赠送订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Present(EntityDto<Guid> input);
    }
}
