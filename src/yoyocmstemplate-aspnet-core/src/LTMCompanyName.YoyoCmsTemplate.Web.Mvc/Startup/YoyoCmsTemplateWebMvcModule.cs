using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Startup
{
    [DependsOn(typeof(YoyoCmsTemplateWebCoreModule))]
    public class YoyoCmsTemplateWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YoyoCmsTemplateWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //配置菜单导航类
            Configuration.Navigation.Providers.Add<AppAdminNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebMvcModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }

            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //  workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());

        }
    }
}
