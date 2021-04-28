using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using L._52ABP.Application;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers;
using LTMCompanyName.YoyoCmsTemplate.CustomDtoAutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.MoocModuleDtoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Mapper;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(

        typeof(L52AbpApplicationModule),
        typeof(YoyoCmsTemplateCoreModule)

        )

        ]
    public class YoyoCmsTemplateApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 权限认证提供者
            Configuration.Authorization.Providers.Add<AppProAuthorizationProvider>();

            // ================== Wechat模块权限 =====================
            Configuration.Authorization.Providers.Add<WechatAppConfigAuthorizationProvider>();

            //博客模块，后面统一到一个类中
            Configuration.Authorization.Providers.Add<BloggingAuthorizationProvider>();

            Configuration.Authorization.Providers.Add<SysFileAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<WebSiteSettingAuthorizationProvider>();

            //市场模块

            Configuration.Authorization.Providers.Add<ProductAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<OrderAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<MarketingAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<ProductSecretKeyAuthorizationProvider>();
            //网易腾讯订单
            Configuration.Authorization.Providers.Add<OtherCourseAuthorizationProvider>();
            //慕课模块
            Configuration.Authorization.Providers.Add<MoocAuthorizationProvider>();

            //文档模块
            Configuration.Authorization.Providers.Add<ProjectAuthorizationProvider>();

            // 自定义类型映射
            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                // 系统通用的Dto映射
                CustomerAppDtoMapper.CreateMappings(configuration);
                //在下方添加你的自定义AutoMapper映射…

                // ================== Wechat =====================
                WechatAppConfigMapper.CreateMappings(configuration);

                //博客模块，后面统一到一个类中
                BlogDtoAutoMapper.CreateMappings(configuration);
                SysFileDtoAutoMapper.CreateMappings(configuration);
                WebSiteModuleDtoAutoMapper.CreateMappings(configuration);

                //市场模块
                ProductMapper.CreateMappings(configuration);
                OrderMapper.CreateMappings(configuration);
                ProductSecretKeyMapper.CreateMappings(configuration);
                DownloadLogMapper.CreateMappings(configuration);
                UserDownloadConfigMapper.CreateMappings(configuration);

                //慕课模块

                MoocDtoAutoMapper.CreateMappings(configuration);

                //课程模块
                NeteaseOrderInfoMapper.CreateMappings(configuration);
                TencentOrderInfoMapper.CreateMappings(configuration);

                DropdownListDtoAutoMapper.CreateMappings(configuration);

                // 动态页面信息
                DynamicViewMapper.CreateMappings(configuration);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //DeleteAuditLogsRegularBasisWorker
            //  workManager.Add(IocManager.Resolve<MakeInactiveUsersPassiveWorker>());
            workManager.Add(IocManager.Resolve<DeleteAuditLogsRegularBasisWorker>());
            workManager.Add(IocManager.Resolve<AutoMaticallyPublishMarkdownPosts>());
        }
    }
}
