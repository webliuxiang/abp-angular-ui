using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages
{
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
