using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.Threading.Timers;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
 using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;
using Yoyo.Abp;

namespace LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers
{
    /// <summary>
    /// 订单支付检查任务
    /// </summary>
    public class OrderPayCheckWorker : BackgroundJob<object>, ISingletonDependency
    {
        /// <summary>
        /// 等待支付等待时常(60分钟)
        /// </summary>
        public const int WaitForPaymentMinutes = 60;
        /// <summary>
        /// 循环执行的时间(60分钟)
        /// </summary>
        public const int WaitWorkMinutes = 60;

        private readonly IAlipayHelper _alipayHelper;
        private readonly IOrderManager _orderManager;
        public OrderPayCheckWorker(
            AbpTimer timer,
            IAlipayHelper alipayHelper,
            IOrderManager orderManager
        )
        {
            _alipayHelper = alipayHelper;
            _orderManager = orderManager;
        }


        /// <summary>
        /// 
        /// </summary>
        public static void Execute()
        {
            IocManager.Instance.Resolve<OrderPayCheckWorker>().Execute(null);
        }


        [UnitOfWork]
        public override void Execute(object args)
        {
            AsyncHelper.RunSync(async () =>
            {
                await ExecuteAsync();
            });
        }

        private async Task ExecuteAsync()
        {
            var currentTime = DateTime.UtcNow;

            var orderList = await _orderManager.GetListByStatus(OrderStatusEnum.WaitForPayment);
            if (orderList == null || orderList.Count == 0)
            {
                return;
            }

            var productManagerDisposable = IocManager.Instance.ResolveAsDisposable<IProductManager>();
            var userDownloadConfigManagerDisposable = IocManager.Instance.ResolveAsDisposable<IUserDownloadConfigManager>();
            var userManagerDisposable = IocManager.Instance.ResolveAsDisposable<AbpUserManager<Role, User>>();

            foreach (var order in orderList)
            {
                if ((currentTime - order.CreationTime).TotalMinutes > WaitForPaymentMinutes)
                {
                    await CloseOrder(order);
                    continue;
                }


                /* 交易状态：
                 * WAIT_BUYER_PAY（交易创建，等待买家付款）、
                 * TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、
                 * TRADE_SUCCESS（交易支付成功）、
                 * TRADE_FINISHED（交易结束，不可退款）
                 */

                var queryResponse = await _alipayHelper.Query(order.OrderCode, null);
                switch (queryResponse.TradeStatus)
                {
                    case "TRADE_FINISHED":
                    case "TRADE_SUCCESS":
                        await SuccessPay(
                            productManagerDisposable,
                            userDownloadConfigManagerDisposable,
                            userManagerDisposable,
                            order);
                        break;
                    case "TRADE_CLOSED":
                        order.Status = OrderStatusEnum.TradingClosed;
                        await _orderManager.Update(order);
                        break;
                }
            }


            productManagerDisposable.Dispose();
            userDownloadConfigManagerDisposable.Dispose();
            userManagerDisposable.Dispose();
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task CloseOrder(Order order)
        {
            var needUpdate = false;

            var closeResponse = await _alipayHelper.OrderClose(order.OrderCode, null);
            if (!closeResponse.IsError)
            {
                var queryResponse = await _alipayHelper.Query(order.OrderCode, null);
                switch (queryResponse.TradeStatus)
                {
                    case "TRADE_CLOSED":
                        order.Status = OrderStatusEnum.TradingClosed;
                        needUpdate = true;
                        goto end;
                }
            }
            switch (closeResponse.SubCode)
            {
                case "ACQ.TRADE_NOT_EXIST":
                    order.Status = OrderStatusEnum.TradingClosed;
                    needUpdate = true;
                    break;
            }

            end:
            if (needUpdate)
            {
                await _orderManager.Update(order);
            }
        }

        /// <summary>
        /// 订单支付成功
        /// </summary>
        /// <param name="productManagerDisposable"></param>
        /// <param name="userDownloadConfigManagerDisposable"></param>
        /// <param name="userManagerDisposable"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task SuccessPay(
             IDisposableDependencyObjectWrapper<IProductManager> productManagerDisposable,
              IDisposableDependencyObjectWrapper<IUserDownloadConfigManager> userDownloadConfigManagerDisposable,
              IDisposableDependencyObjectWrapper<AbpUserManager<Role, User>> userManagerDisposable,
            Order order
            )
        {
            // 获取产品
            var product = await productManagerDisposable.Object.GetProductByCode(order.ProductCode);

            // 获取用户
            var user = await userManagerDisposable.Object.Users.Where(o => o.Id == order.CreatorUserId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // 设置用户下载配置
            await userDownloadConfigManagerDisposable.Object.SetUserDownloadConfig(
                user,
                product.Type,
                order.ProductCode,
                order.ProductCreateProjectCount,
                order.ProductIndate);

            // 修改订单状态
            order.Status = OrderStatusEnum.ChangeHands;
            await _orderManager.Update(order);
        }


    }
}
