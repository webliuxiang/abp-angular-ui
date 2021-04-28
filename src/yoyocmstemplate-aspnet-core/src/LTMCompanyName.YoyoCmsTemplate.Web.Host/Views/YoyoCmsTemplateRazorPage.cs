using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Views
{
    /// <summary>
    /// 用于服务RazorPage
    /// </summary>
    /// <typeparam name="TModel"> </typeparam>
    public abstract class YoyoCmsTemplateRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected YoyoCmsTemplateRazorPage()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }
    }
}