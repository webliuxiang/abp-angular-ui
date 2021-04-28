using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Abp.Domain.Services;

using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService
{
    public interface IOrderManager : I52AbpDomainService<Order, Guid>
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitOrder();


        /// <summary>
        /// 根据OrderId获取实体
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<Order> GetById(Guid orderId);


        /// <summary>
        /// 创建订单并获取订单Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Guid> CreateOrderAndGetId(Order entity);

        /// <summary>
        /// 创建订单并保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Order> CreateOrderAndSave(Order entity);

        /// 修改订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        Task ChangeStatus(Guid orderId, OrderStatusEnum orderStatus);

        /// <summary>
        /// 获取用户的状态匹配的第一个订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatusEnum"></param>
        /// <returns></returns>
        Task<Order> GetUserFirstOrderByStatus(long? userId, OrderStatusEnum orderStatusEnum);

        /// <summary>
        /// 根据用户Id和日期获取用户订单数量
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        Task<int> GetOrderCountByUserIdAndDateRang(long? userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 根据订单编码获取订单
        /// </summary>
        /// <param name="orderCode">订单编码</param>
        /// <returns></returns>
        Task<Order> GetOrderByOrderCode(string orderCode);

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Order> CreateOrder(Order entity);


        /// <summary>
        /// 根据订单状态获取
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        Task<List<Order>> GetListByStatus(OrderStatusEnum orderStatus);


        /// <summary>
        /// 根据产品类型的不同修改订单状态，开通对应的课程等内容
        /// </summary>
        /// <returns></returns>
        Task<Order> UpdateOrderStateByProductType(Order order);

        /// <summary>
        /// 获取订单号码
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="payType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        string GetOrderMaxCode(OrderSourceType sourceType, PayTypeEnum payType, DateTime? time = null);


        /// <summary>
        /// 创建课程订单
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        Task<Order> CreateCourseOrder(Course course);

        /// <summary>
        /// 创建课程订单 (赠送)
        /// </summary>
        /// <param name="course">课程信息</param>
        /// <param name="userName">用户名</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<Order> CreateCourseOrderWithPresented(Course course, long userId, string userName);
    }
}
