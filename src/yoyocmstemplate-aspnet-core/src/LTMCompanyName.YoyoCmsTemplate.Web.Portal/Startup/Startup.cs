using System;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using HealthChecks.UI.Client;
using L._52ABP.Core.Configs;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.HealthCheck;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "YoyoCmsTemplate";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            this._env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            var builder = services.AddMvc(options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }).AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Blog/Details", "{blogshortName}/{Posturl}");
                    //    options.ViewLocationFormats.Add("/{0}.cshtml");
                })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif

                .AddNewtonsoftJson(options =>
                {
                    ////将所有枚举序列化为字符串
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            //services.AddServerSideBlazor();

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            //      builder.AddRazorRuntimeCompilation();

            services.AddHttpClient();

            // services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //记录浏览量会根据session判断用户，默认是1天，所以session时效设置要求在1天以上
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            #region 配置健康检查服务

            //services.AddHealthChecks().AddSqlServer(_appConfiguration["ConnectionStrings:Default"]);
            //services.AddHealthChecksUI();

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                services.AddYoyoCmsHealthCheck();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<HealthChecksUISettings>(settings =>
                    {
                        healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true);
                    });
                    services.AddHealthChecksUI();
                }
            }

            #endregion 配置健康检查服务

            // Configure Abp and Dependency Injection
            return services.AddAbp<YoyoCmsTemplateWebPortalModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAbp(); // Initializes ABP framework.

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //    app.UseBrowserLink();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            //初始化数据绑定内容
            app.UseBindAbpConfig();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseAuthorization();

            app.UseSession();

            app.UseAbpRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");

                //endpoints.MapBlazorHub();
                //endpoints.MapFallbackToPage("/_Host");
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
                {
                    endpoints.MapHealthChecks("/healthz", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                    endpoints.MapHealthChecksUI();
                }
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                {
                    app.UseHealthChecksUI();
                }
            }

            //自动设置租户为默认租户, 避免用户手动清除缓存后程序崩坏
            //   app.UseDefaultTenantEnforcer();
        }
    }
}
