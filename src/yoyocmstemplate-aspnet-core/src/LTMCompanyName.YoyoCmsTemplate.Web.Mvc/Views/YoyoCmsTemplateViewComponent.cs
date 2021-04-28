using Abp.AspNetCore.Mvc.ViewComponents;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views
{
    public abstract class YoyoCmsTemplateViewComponent : AbpViewComponent
    {
        protected YoyoCmsTemplateViewComponent()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }
    }
}
