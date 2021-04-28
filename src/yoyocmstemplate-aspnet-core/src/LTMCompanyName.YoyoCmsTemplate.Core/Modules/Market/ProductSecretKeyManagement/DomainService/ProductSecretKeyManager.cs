using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.DomainService
{
    /// <summary>
    /// ProductSecretKey领域层的业务管理
    ///</summary>
    public class ProductSecretKeyManager : YoyoCmsTemplateDomainServiceBase, IProductSecretKeyManager
    {

        private readonly IRepository<ProductSecretKey, Guid> _repository;
        private readonly AbpUserManager<Role, User> _userManager;
        private readonly IUserDownloadConfigManager _userDownloadConfigManager;
        private readonly IProductManager _productManager;
        private readonly IOrderManager _orderManager;

        /// <summary>
        /// ProductSecretKey的构造方法
        ///</summary>
        public ProductSecretKeyManager(
            IRepository<ProductSecretKey, Guid> repository,
            AbpUserManager<Role, User> userManager,
            IUserDownloadConfigManager userDownloadConfigManager,
            IProductManager productManager,
            IOrderManager orderManager
        )
        {
            _repository = repository;
            _userManager = userManager;
            _userDownloadConfigManager = userDownloadConfigManager;
            _productManager = productManager;
            _orderManager = orderManager;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitProductSecretKey()
        {
            throw new NotImplementedException();
        }



        public async Task BindToUser(string secretKey, string userName, decimal actualPayment)
        {




            await this.Use(secretKey, userName, async (user, product, nowDayOrderCount) =>
            {

                var OrderCode = _orderManager.GetOrderMaxCode(OrderSourceType.Product, PayTypeEnum.Alipay);

                // 这里是后台设置的,这里需要创建一个订单
                var order = await _orderManager.CreateOrderAndSave(new Order()
                {
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    Price = product.Price,
                    Discounts = product.Price - actualPayment,
                    ActualPayment = actualPayment,
                    Status = OrderStatusEnum.ChangeHands,
                    ProductCreateProjectCount = product.CreateProjectCount,
                    ProductIndate = product.Indate,
                    UserId = user.Id,
                    UserName = user.UserName,
                    OrderCode = OrderCode
                });
                return order;
            }, true);
        }



        public async Task BindToUser(string secretKey, string userName, Order order)
        {
            var OrderCode = _orderManager.GetOrderMaxCode(OrderSourceType.Product, PayTypeEnum.Alipay);


            await this.Use(secretKey, userName, async (user, product, nowDayOrderCount) =>
            {
                // 没有订单
                if (order == null)
                {
                    order = await _orderManager.CreateOrderAndSave(new Order()
                    {
                        ProductId = product.Id,
                        ProductCode = product.Code,
                        ProductName = product.Name,
                        Price = product.Price,
                        Discounts = 0m,
                        ActualPayment = product.Price,
                        Status = OrderStatusEnum.ChangeHands,
                        ProductCreateProjectCount = product.CreateProjectCount,
                        ProductIndate = product.Indate,
                        UserId = user.Id,
                        UserName = user.UserName,
                        OrderCode = OrderCode
                    });
                }
                else // 已有订单
                {
                    order.ProductId = product.Id;
                    order.ProductCode = product.Code;
                    order.Price = product.Price;
                    order.Discounts = 0;
                    order.ActualPayment = product.Price;
                    order.Status = OrderStatusEnum.ChangeHands;
                    order.ProductCreateProjectCount = product.CreateProjectCount;
                    order.ProductIndate = product.Indate;
                    order.UserId = user.Id;
                    order.UserName = user.UserName;

                    if (order.OrderCode.IsNullOrWhiteSpace())
                    {
                        order.OrderCode =
                            OrderCode;
                    }

                    await _orderManager.Update(order);
                }

                return order;
            });

        }


        private async Task Use(string secretKey, string userName, Func<User, Product, int, Task<Order>> orderFunc, bool adminCall = false)
        {
            // 获取用户
            var user = await _userManager.Users.Where(o => o.UserName == userName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserFriendlyException($"未找到此用户:{userName}");
            }

            // 获取卡密信息
            var productSecret = await _repository
                .GetAll().Where(o => o.SecretKey == secretKey)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (productSecret == null)
            {
                throw new UserFriendlyException($"未找到此卡密:{secretKey}");
            }
            else if (productSecret.Used || productSecret.OrderId.HasValue)
            {
                throw new UserFriendlyException($"此卡密已使用！卡密:{secretKey}");
            }
            else if (adminCall && productSecret.InSale)
            {
                throw new UserFriendlyException($"此卡密在销售中,禁止手动使用！卡密:{secretKey}");
            }

            // 获取卡密关联的产品信息
            var product = await _productManager.GetProductByCode(productSecret.ProductCode);
            if (product == null)
            {
                throw new UserFriendlyException($"未找到卡密对应产品");
            }
            else if (!product.Published)
            {
                throw new UserFriendlyException($"卡密对应产品 {product.Name} 已暂停激活,请稍后再试");
            }

            // 更新用户下载配置信息
            await _userDownloadConfigManager.SetUserDownloadConfig(user, 
                product.Type, 
                product.Code, 
                product.CreateProjectCount, 
                product.Indate);

            
            // 更新卡密设置为已使用
            productSecret.Used = true;
            productSecret.UserId = user.Id;
            productSecret.UserName = user.UserName;

            // 此用户当日订单数量
            var nowDayOrderCount = await _orderManager.GetOrderCountByUserIdAndDateRang(user.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            // 卡密关联订单号
            var order = await orderFunc.Invoke(user, product, nowDayOrderCount);
            productSecret.OrderId = order.Id;
            productSecret.OrderCode = order.OrderCode;

            // 更新卡密信息
            await _repository.UpdateAsync(productSecret);

        }
    }
}
