
using Abp.Domain.Entities;
using Abp.Domain.Services;
using Abp.Runtime.Session;

namespace LTMCompanyName.YoyoCmsTemplate
{
    public abstract class YoyoCmsTemplateDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */
        /*在领域服务中添加你的自定义公共方法*/


        public IAbpSession AbpSession { get; set; }


        protected YoyoCmsTemplateDomainServiceBase()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }
    }

  
}
