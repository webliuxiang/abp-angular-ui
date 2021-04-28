using System.Collections.Generic;
using Abp.AspNetCore.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.API;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Startup
{
    [DependsOn(
       typeof(YoyoCmsTemplateWebCoreModule))]
    public class YoyoCmsTemplateWebHostModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public YoyoCmsTemplateWebHostModule(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpAspNetCore().UseMvcDateTimeFormatForAppServices = true;//就是这句，使用mvc时间格式
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebHostModule).GetAssembly());

           //Configuration.MultiTenancy.IsEnabled = false;

        }

        public override void PostInitialize()
        {



            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            if (IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                // workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            }

            ConfigureExternalAuthProviders();

         //   Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        private void ConfigureExternalAuthProviders()
        {
            var externalAuthConfiguration = IocManager.Resolve<ExternalAuthConfiguration>();

            if (bool.Parse(_appConfiguration["Authentication:OpenId:IsEnabled"]))
            {
                externalAuthConfiguration.Providers.Add(
                    new ExternalLoginProviderInfo(
                        OpenIdConnectAuthProviderApi.Name,
                        _appConfiguration["Authentication:OpenId:ClientId"],
                        _appConfiguration["Authentication:OpenId:ClientSecret"],
                        typeof(OpenIdConnectAuthProviderApi),
                        new Dictionary<string, string>
                        {
                            {"Authority", _appConfiguration["Authentication:OpenId:Authority"]},
                            {"LoginUrl",_appConfiguration["Authentication:OpenId:LoginUrl"]}
                        }
                    )
                );
            }

            //qq login config
            if (bool.Parse(_appConfiguration["Authentication:QQ:IsEnabled"]))
            {
                externalAuthConfiguration.Providers.Add(new ExternalLoginProviderInfo(
                    QQAuthProviderApi.ProviderName,
                    _appConfiguration["Authentication:QQ:AppId"],
                    _appConfiguration["Authentication:QQ:AppKey"],
                    typeof(QQAuthProviderApi)));
            }

            //wechat login config
            //if (bool.Parse(_appConfiguration["Authentication:Wechat:IsEnabled"]))
            //{
            //    externalAuthConfiguration.Providers.Add(new ExternalLoginProviderInfo(
            //        WechatAuthProviderApi.ProviderName,
            //        _appConfiguration["Authentication:Wechat:AppId"],
            //        _appConfiguration["Authentication:Wechat:AppSecret"],
            //        typeof(WechatAuthProviderApi)));
            //}

            //Microsoft login config
            //if (bool.Parse(_appConfiguration["Authentication:Microsoft:IsEnabled"]))
            //{
            //    externalAuthConfiguration.Providers.Add(new ExternalLoginProviderInfo(
            //        MicrosoftAuthProviderApi.ProviderName,
            //        _appConfiguration["Authentication:Microsoft:ConsumerKey"],
            //        _appConfiguration["Authentication:Microsoft:ConsumerSecret"],
            //        typeof(MicrosoftAuthProviderApi)));
            //}

            ////Facebook login config
            //if (bool.Parse(_appConfiguration["Authentication:Facebook:IsEnabled"]))
            //{
            //    externalAuthConfiguration.Providers.Add(new ExternalLoginProviderInfo(
            //        FacebookAuthProviderApi.ProviderName,
            //        _appConfiguration["Authentication:Facebook:AppId"],
            //        _appConfiguration["Authentication:Facebook:AppSecret"],
            //        typeof(FacebookAuthProviderApi)));
            //}

            ////Google login config
            //if (bool.Parse(_appConfiguration["Authentication:Google:IsEnabled"]))
            //{
            //    externalAuthConfiguration.Providers.Add(new ExternalLoginProviderInfo(
            //        GoogleAuthProviderApi.ProviderName,
            //        _appConfiguration["Authentication:Google:ClientId"],
            //        _appConfiguration["Authentication:Google:ClientSecret"],
            //        typeof(GoogleAuthProviderApi)));
            //}
        }
    }
}
