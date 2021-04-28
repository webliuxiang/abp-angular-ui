using System.Threading.Tasks;
using Abp.Authorization;
using Alipay.AopSdk.Core.Domain;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Url;
using Yoyo.Abp;
using Yoyo.Abp.WebPay;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.DemoUiManagement.Purchase
{
    /// <summary>
    /// WechatAppConfig应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PurchaseAppService : IPurchaseAppService
    {
        public const string TimeoutExpress = "30m";

        private readonly IAlipayHelper _alipayHelper;
        private readonly IAppConfigurationAccessor _appConfigurationAccessor;
        public PurchaseAppService(IAlipayHelper alipayHelper,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _alipayHelper = alipayHelper;
            _appConfigurationAccessor = appConfigurationAccessor;
        }

        public async Task<string> CreatePay(PurchasePayInput input)
        {
            //todo:处理订单类型
            //判断哪些是课程的，哪些是框架的。
            //如果是框架的订单，自动给客户开通视频播放功能。
            //有效期为1年。
             var webPayOutput = await _alipayHelper.WebPay(new WebPayInput
            {
                Data = new AlipayTradePagePayModel
                {
                    Body = "支付Demo",
                    Subject = "支付Demo",
                    TotalAmount = "1",
                    OutTradeNo = input.OrderCode,
                    ProductCode = YoYoAlipayConsts.ProductCode_FAST_INSTANT_TRADE_PAY,
                    TimeoutExpress = TimeoutExpress // 30分钟后过期,默认90分钟
                },
                //AsyncNotifyUrl = $"{GetHostUrl()}Purchase/Notify",
                //SynchronizationCallbackUrl = $"{GetHostUrl()}Purchase/Callback"
            });


            return webPayOutput.RedirectUrl;
        }



        #region 私有函数
        /// <summary>
        ///    获取HostUrl 
        /// </summary>
        /// <returns></returns>
        private string GetHostUrl()
        {
            return "https://52abp.cyanstream.com/";

            //string scheme = this._httpContextAccessor.HttpContext.Request.Scheme == "https" ? this._httpContextAccessor.HttpContext.Request.Scheme : "https";


            //if (DebugHelper.IsDebug)
            //{
            //    return $"{scheme}://{this._httpContextAccessor.HttpContext.Request.Host}/";
            //}
            //else
            //{
            //    return this._appConfigurationAccessor.GetWebSiteClientRootAddress();
            //}

        }
        #endregion
    }
}
