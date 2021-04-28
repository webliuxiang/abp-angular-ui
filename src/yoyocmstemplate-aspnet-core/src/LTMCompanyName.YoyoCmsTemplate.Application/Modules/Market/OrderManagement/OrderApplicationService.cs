using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement
{
    /// <summary>
    ///     Order应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class OrderAppService : YoyoCmsTemplateAppServiceBase, IOrderAppService
    {
        private readonly IOrderManager _orderManager;

        /// <summary>
        ///     构造函数
        /// </summary>
        public OrderAppService(
            IOrderManager orderManager
        )
        {
            _orderManager = orderManager;
        }

        /// <summary>
        ///     获取Order的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(OrderPermissions.Query)]
        public async Task<PagedResultDto<OrderListDto>> GetPaged(GetOrdersInput input)
        {
            var query = _orderManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    o => o.ProductName.Contains(input.FilterText)
                         || o.ProductCode.Contains(input.FilterText)
                         || o.UserName.Contains(input.FilterText)
                         || o.OrderCode.Contains(input.FilterText)
                );
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var entityList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<OrderListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<OrderListDto>>();
            foreach (var item in entityListDtos)
            {
                item.StatusStr = GetStatusString(item.Status);
            }

            return new PagedResultDto<OrderListDto>(count, entityListDtos);
        }

        /// <summary>
        ///     通过指定id获取OrderListDto信息
        /// </summary>
        [AbpAuthorize(OrderPermissions.Query)]
        public async Task<OrderListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _orderManager.EntityRepo.GetAsync(input.Id);

            return ObjectMapper.Map<OrderListDto>(entity);

            //return entity.MapTo<OrderListDto>();
        }

        /// <summary>
        ///     删除Order信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(OrderPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _orderManager.EntityRepo.DeleteAsync(input.Id);
        }

        /// <summary>
        ///     批量删除Order的方法
        /// </summary>
        [AbpAuthorize(OrderPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _orderManager.EntityRepo.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        ///     获取Order的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<UserCenterOrderListDto>> GetUserCenterPaged(GetOrdersInput input)
        {




            var query = _orderManager.QueryAsNoTracking
                .Where(o => o.UserId == AbpSession.UserId)
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    o => o.ProductName.Contains(input.FilterText)
                );

            var count = await query.CountAsync();

            // 设置排序，根据创建时间倒叙
            input.Sorting = "CreationTime Desc";
            var entityList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();
            var entityListDtos = ObjectMapper.Map<List<UserCenterOrderListDto>>(entityList);

            //var entityListDtos = entityList.MapTo<List<UserCenterOrderListDto>>();
            foreach (var item in entityListDtos)
            {
                item.StatusStr = GetStatusString(item.Status);
            }

            return new PagedResultDto<UserCenterOrderListDto>(count, entityListDtos);
        }

        /// <summary>
        ///     获取产品分类的收益统计表
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(OrderPermissions.Query)]
        public async Task<ListResultDto<OrderStatisticsDto>> GetProductOrderStatistics()
        {
            var query = await _orderManager.QueryAsNoTracking.Where(a => a.Status == OrderStatusEnum.ChangeHands).ToListAsync();

            var dtos = from order in query
                       group order by new
                       {
                           order.CreationTime.Month,
                           order.ProductName,
                           order.CreationTime.Year,
                           order.OrderSourceType
                       }
                into g
                       select g;

            var resultList = dtos.Select(dto => new OrderStatisticsDto
                {
                    YearMonth = dto.Key.Year + dto.Key.Month.ToString(),
                    ActualPayment = dto.Sum(a => a.ActualPayment),
                    ProductName = dto.Key.ProductName,
                    BuyCount = dto.Count(),
                    OrderSourceType = dto.Key.OrderSourceType
                })
                .ToList();

            return new ListResultDto<OrderStatisticsDto>(resultList);
        }

        [AbpAuthorize(OrderPermissions.EditPrice)]
        public async Task UpdateOraderPrice(OrderEditPriceDto input)
        {
            var order = await _orderManager.QueryAsNoTracking
                .Where(o => o.Id == input.Id)
                .FirstOrDefaultAsync();

            if (order.Status == OrderStatusEnum.ChangeHands
                || order.Status == OrderStatusEnum.TradingClosed)
            {
                return;
            }

            if (input.ActualPayment < 0)
            {
                input.ActualPayment = 0.1m;
            }

            order.Discounts = order.Price - input.ActualPayment;
            order.ActualPayment = input.ActualPayment;

            await _orderManager.Update(order);
        }

        /// <summary>
        /// 赠送订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(OrderPermissions.Present)]
        public async Task Present(EntityDto<Guid> input)
        {
            var order = await _orderManager.QueryAsNoTracking
                .Where(o => o.Id == input.Id)
                .FirstOrDefaultAsync();

            if (order.Status == OrderStatusEnum.ChangeHands
                || order.Status == OrderStatusEnum.TradingClosed)
            {
                return;
            }

            if (order != null)
            {
                await _orderManager.UpdateOrderStateByProductType(order);
            }
        }

        [AbpAuthorize(OrderPermissions.Update)]
        public async Task UpdateOrder(OrderEditDto input)
        {
            var entity = await _orderManager.QueryAsNoTracking
                .Where(o => o.Id == input.Id)
                .FirstOrDefaultAsync();

            entity.Status = input.Status;
            entity.ProductIndate = input.ProductIndate;
            entity.OrderSourceType = input.OrderSourceType;

            await _orderManager.Update(entity);
        }

        /// <summary>
        ///     导出Order为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}
        private static string GetStatusString(OrderStatusEnum orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatusEnum.WaitForPayment:
                    return "等待支付";

                case OrderStatusEnum.WaitForDelivery:
                    return "等待发货";

                case OrderStatusEnum.ChangeHands:
                    return "交易成功";

                case OrderStatusEnum.TradingClosed:
                    return "交易关闭";

                default:
                    return "未知";
            }
        }
    }
}
