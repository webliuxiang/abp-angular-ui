using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp;
using Abp.Domain.Repositories;

using L._52ABP.Common.Extensions;

using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

using Microsoft.EntityFrameworkCore;

using NPOI.SS.Formula.Functions;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService
{
    /// <summary>
    /// Order领域层的业务管理
    ///</summary>
    public class OrderManager : AbpDomainService<Order, Guid>, IOrderManager
    {

        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Course, long> _courseRepository;
        private readonly UserManager _userManager;
        private readonly IUserDownloadConfigManager _userDownloadConfigManager;


        /// <summary>
        /// Order的构造方法
        ///</summary>
        public OrderManager(
            IRepository<Order, Guid> repository,
            IRepository<Product, Guid> productRepository,
            UserManager userManager,
            IRepository<Course, long> courseRepository,
            IUserDownloadConfigManager userDownloadConfigManager
            )
            : base(repository)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _courseRepository = courseRepository;
            _userDownloadConfigManager = userDownloadConfigManager;

        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitOrder()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码



        public async Task<Order> GetById(Guid orderId)
        {
            try
            {
                return await QueryAsNoTracking
                        .Where(o => o.Id == orderId)
                        .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }


        public async Task<Guid> CreateOrderAndGetId(Order entity)
        {
            await EntityRepo.InsertAndGetIdAsync(entity);
            return entity.Id;
        }


        public async Task ChangeStatus(Guid orderId, OrderStatusEnum orderStatus)
        {
            var entity = await EntityRepo.GetAsync(orderId);


            entity.Status = orderStatus;

            await EntityRepo.UpdateAsync(entity);
        }


        public async Task<Order> GetUserFirstOrderByStatus(long? userId, OrderStatusEnum orderStatusEnum)
        {
            try
            {
                return await QueryAsNoTracking
                            .Where(o => o.CreatorUserId == userId.Value && o.Status == orderStatusEnum)
                            .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Order> CreateOrderAndSave(Order entity)
        {
            await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task<int> GetOrderCountByUserIdAndDateRang(long? userId, DateTime startDate, DateTime endDate)
        {
            return await this.QueryAsNoTracking
                 .Where(o => o.UserId == userId.Value
                                && o.CreationTime >= startDate
                                && o.CreationTime < endDate)
                 .CountAsync();
        }

        public async Task<Order> GetOrderByOrderCode(string orderCode)
        {
            return await this.QueryAsNoTracking
                .Where(o => o.OrderCode == orderCode)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> CreateOrder(Order entity)
        {
            return await this.EntityRepo.InsertAsync(entity);
        }

        public async Task<List<Order>> GetListByStatus(OrderStatusEnum orderStatus)
        {
            return await this.QueryAsNoTracking
                  .Where(o => o.Status == orderStatus)
                  .ToListAsync();
        }

        public async Task<Order> UpdateOrderStateByProductType(Order order)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == order.CreatorUserId);
            switch (order.OrderSourceType)
            {
                case OrderSourceType.Product:
                    await this.PresentedCourseWithProductOrder(order);
                    break;
                case OrderSourceType.Course:
                    break;
                default:
                    break;
            }


            // 修改订单状态
            await ChangeStatus(order.Id, OrderStatusEnum.ChangeHands);

            return order;

        }


        public async Task<Order> CreateCourseOrder(Course course)
        {
            // 创建订单
            var order = new Order
            {
                OrderCode = GetOrderMaxCode(OrderSourceType.Course, PayTypeEnum.Alipay),
                ProductCode = course.CourseCode,
                ProductId = null,
                ProductIndate = course.ValidDays,
                ProductCreateProjectCount = 0,
                ProductName = course.Title,
                Price = course.Price,
                Status = OrderStatusEnum.WaitForPayment,
                Discounts = 0m,
                ActualPayment = course.Price,
                UserId = AbpSession.UserId,
                UserName = AbpSession.GetUserName(),
                OrderSourceType = OrderSourceType.Course,
                CourseId = course.Id

            };
            order = await CreateOrder(order);

            return order;
        }


        public async Task<Order> CreateCourseOrderWithPresented(Course course, long userId, string userName)
        {
            // 创建订单
            var order = new Order
            {
                OrderCode = GetOrderMaxCode(OrderSourceType.Course, PayTypeEnum.Alipay),
                ProductCode = course.CourseCode,
                ProductId = null,
                ProductIndate = course.ValidDays,
                ProductCreateProjectCount = 0,
                ProductName = course.Title,
                Price = course.Price,
                Status = OrderStatusEnum.ChangeHands,
                Discounts = course.Price,
                ActualPayment = 0,
                UserId = userId,
                UserName = userName,
                OrderSourceType = OrderSourceType.Course,
                CourseId = course.Id

            };
            order = await CreateOrder(order);

            return order;
        }

        /// <summary>
        /// 如果订单类型为产品,则将课程赠送
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected async Task PresentedCourseWithProductOrder(Order order)
        {
            if (order.OrderSourceType != OrderSourceType.Product)
            {
                return;
            }

            // 创建订单的用户
            var user = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == order.CreatorUserId);

            // 根据订单的产品编码获取产品信息
            var product = await _productRepository.FirstOrDefaultAsync(a => a.Code == order.ProductCode);

            // 设置用户下载配置
            await _userDownloadConfigManager.SetUserDownloadConfig(
                user,
                product.Type,
                order.ProductCode,
                order.ProductCreateProjectCount,
                order.ProductIndate);

            // 获取所有已发布的课程
            var courses = await _courseRepository.GetAll()
                .Where(a => a.CourseState == CourseStateEnum.Publish)
                .ToListAsync();

            // 遍历创建课程订单
            foreach (var course in courses)
            {
                var courseOrder = await CreateCourseOrderWithPresented(course, user.Id, user.UserName);
            }
        }

        #region 私有方法

        /// <summary>
        /// 生成订单编码
        /// 规则: 来源(2位)-支付方式(2位)-时间戳(秒级)-属于用户当日订单数量+1(3位)
        /// </summary>
        /// <param name="sourceType">订单来源</param>
        /// <param name="payType">支付方式</param>
        /// <param name="time">属于用户当日订单数量</param>
        /// <returns></returns>
        public string GetOrderMaxCode(OrderSourceType sourceType, PayTypeEnum payType, DateTime? time = null)
        {
            var precode = RandomHelper.GetRandom(1, 9) + Convert.ToInt32(sourceType).ToString() + Convert.ToInt32(payType);

            var date = time ?? DateTime.Now;
            var codeStateWith = precode + date.ToString("yyyyMMdd"); //截取数据格式：年-月-日 20191021

            var code = codeStateWith;

            var list = this.EntityRepo.GetAll().Where(e => e.OrderCode.StartsWith(codeStateWith)).ToList();
            var model = list.Select(e => new { Number = e.OrderCode.Substring(code.Length).CastTo(0) })
                .OrderByDescending(e => e.Number)
                .FirstOrDefault();//返回订单的最后一位

            if (model != null)
            {
                code += (model.Number + 1).ToString().PadLeft(2, '0');
            }
            else
            {
                code += "01";
            }
            return code;
        }




        #endregion

    }
}
