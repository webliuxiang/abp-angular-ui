using Abp.Modules;
using Abp.Reflection.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Startup.Navigation;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Startup
{

    [DependsOn(typeof(YoyoCmsTemplateWebCoreModule))]

    public class YoyoCmsTemplateWebPublicModule : AbpModule
    {
        public YoyoCmsTemplateWebPublicModule()
        {

        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<PublicNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebPublicModule).GetAssembly());
        }
    }
}
