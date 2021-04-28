using Abp.Modules;
using Abp.Reflection.Extensions;
using L._52ABP.Web.FrontView;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup.Navigation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup
{
    [DependsOn(typeof(YoyoCmsTemplateWebCoreModule), typeof(L52ABPWebFrontViewModule))]
    public class YoyoCmsTemplateWebPortalModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YoyoCmsTemplateWebPortalModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<PortalNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebPortalModule).GetAssembly());
        }
    }
}
