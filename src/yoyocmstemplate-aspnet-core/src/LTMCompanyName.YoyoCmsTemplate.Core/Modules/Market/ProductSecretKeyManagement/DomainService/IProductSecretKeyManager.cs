using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.DomainService
{
    public interface IProductSecretKeyManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProductSecretKey();


        /// <summary>
        /// 绑定卡密到用户
        /// </summary>
        /// <param name="secretKey">卡密</param>
        /// <param name="userName">用户名</param>
        /// <param name="actualPayment">实际付款</param>
        /// <returns></returns>
        Task BindToUser(string secretKey, string userName, decimal actualPayment);

        /// <summary>
        /// 绑定卡密到用户
        /// </summary>
        /// <param name="secretKey">卡密</param>
        /// <param name="userName">用户名</param>
        /// <param name="Order">订单Id</param>
        /// <returns></returns>
        Task BindToUser(string secretKey, string userName, Order order);


    }
}
