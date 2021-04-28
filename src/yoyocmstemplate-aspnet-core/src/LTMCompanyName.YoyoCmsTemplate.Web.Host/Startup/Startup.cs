using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Abp.Hangfire;

using Alipay.AopSdk.AspnetCore;

using Castle.Facilities.Logging;

using Hangfire;

using HealthChecks.UI.Client;

using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Chat.SignalR;
using LTMCompanyName.YoyoCmsTemplate.Common;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.HealthCheck;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.IdentityServer;
using LTMCompanyName.YoyoCmsTemplate.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Senparc.CO2NET;

using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

using Yoyo.Abp;
using Yoyo.Abp.FTF;

using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "YoyoCmsTemplate";

        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 配置MVC与SingalR中间件

            //MVC
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
            })
              
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif

                .AddNewtonsoftJson(options =>
                {
                    ////将所有枚举序列化为字符串
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());

                })
                ;

            // Add SingalR
            services.AddSignalR(options => { options.EnableDetailedErrors = true; });

            #endregion 配置MVC与SingalR中间件

            #region 配置前后端分离跨域

            // 配置前后端分离跨域
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // 在appsettings.json中可以包含多个跨域地址，由逗号隔开。
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()

                )
            );

            #endregion 配置前后端分离跨域

            #region 配置支付宝

            services.AddYoYoAlipay(() =>
            {
                var res = _appConfiguration.GetSection("Pay:Alipay").Get<AlipayOptions>();
                return res;
            }, (fTFConfig) =>
            {
                if (fTFConfig == null)
                {
                    fTFConfig = new FTFConfig();
                }

                fTFConfig.QRCodeGenErrorImageFullPath = System.IO.Path.Combine(_env.WebRootPath, "imgs", "pay", "alipay_error.png");
                fTFConfig.QRCodeIconFullPath = System.IO.Path.Combine(_env.WebRootPath, "imgs", "pay", "alipay.png");
            });

            #endregion 配置支付宝

            services.AddHttpClient();

            // Add Wchat
            SenparcWXConfigurer.AddWechat(services, _appConfiguration);

            IdentityRegistrar.Register(services);

            AuthConfigurer.Configure(services, _appConfiguration);

            // IdentityServer4 配置
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration);
            }

            #region 配置SwaggerUI

            if (WebConsts.SwaggerUiEnabled)
            {
                //Swagger -启用此行以及Configure方法中的相关行，以启用Swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = "52ABP-PRO API",
                            Version = "v1",
                            Description = "52ABP-PRO 的动态WEBAPI管理端，可以进行测试和调用API。",
                            TermsOfService = new Uri("https://gitee.com/ABPCN/52abp-pro"),
                            Contact = new OpenApiContact
                            {
                                Name = "52abp.com",
                                Email = "ltm@ddxc.org",
                                Url = new Uri("https://www.52abp.com/")
                            },
                        });

                    // 使用 camel case 的枚举
                    //options.DescribeStringEnumsInCamelCase();

                    //使用相对路径获取应用程序所在目录
                    options.DocInclusionPredicate((docName, description) => true);
                    // 支持非body内容中的枚举
                    options.ParameterFilter<SwaggerEnumParameterFilter>();
                    // 对应client枚举转为字符串对应值
                    options.SchemaFilter<SwaggerEnumSchemaFilter>();
                    options.OperationFilter<SwaggerOperationIdFilter>();
                    options.OperationFilter<SwaggerOperationFilter>();
                    options.CustomDefaultSchemaIdSelector();

                    options.OrderActionsBy(x => x.RelativePath);
                    options.DescribeAllParametersInCamelCase();
                    ConfigApiDoc(options);
                });

                // 使用 newtonsoft.json 做swagger的序列化工具
                services.AddSwaggerGenNewtonsoftSupport();
            }

            #endregion 配置SwaggerUI

            if (WebConsts.HangfireDashboardEnabled)
            {
                // 启用hangfire
                services.AddHangfire(config =>
                {
                    config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
                    // config.UseRecurringJob(typeof(RecurringJobService)); //注入Hnagfire的测试服务
                });
            }

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
                    services.AddHealthChecksUI().AddInMemoryStorage();
                }
            }

            #endregion 配置健康检查服务

            // 配置abp和依赖注入
            return services.AddAbp<YoyoCmsTemplateWebHostModule>(options =>
            {
                // 配置log4net
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config"));
            }
            );
        }

        public void Configure(IApplicationBuilder app, IOptions<SenparcSetting> senparcSetting)
        {
            // 初始化ABP框架
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false;
            });

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseBrowserLink();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hosting Environment: " + _env.EnvironmentName);
            //});

            // Use Wechat

            SenparcWXConfigurer.UseWechat(_env, senparcSetting.Value);

            // 启用静态文件
            app.UseStaticFiles();

            app.UseRouting();
            // 启用CORS
            app.UseCors(_defaultCorsPolicyName);

            // 启用校验
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseAuthorization();

            app.UseAbpRequestLocalization();

            #region 是否启用Hangfire

            //是否启用Hangfire
            if (WebConsts.HangfireDashboardEnabled)
            {
                //配置服务最大重试次数值
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
                //设置Hangfire服务可选参数
                var jobOptions = new BackgroundJobServerOptions
                {
                    //wait all jobs performed when BackgroundJobServer shutdown.
                    ShutdownTimeout = TimeSpan.FromMinutes(30),
                    Queues = new[] { "default", "jobs" },//队列名称，只能为小写
                    WorkerCount = 3,//Environment.ProcessorCount * 5, //并发任务数 Math.Max(Environment.ProcessorCount, 20)
                    ServerName = "yoyosoft.hangfire",
                };

                //启用Hangfire仪表盘和服务器(支持使用Hangfire而不是默认的后台作业管理器)
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new AbpHangfireAuthorizationFilter(YoyoSoftPermissionNames.Pages_Administration_HangfireDashboard) }
                });
                app.UseHangfireServer(jobOptions);
                // app.UseHangfireServer();
            }

            #endregion 是否启用Hangfire

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapHub<ChatHub>("/signalr-chat");

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

            if (WebConsts.SwaggerUiEnabled)
            {
                // 使中间件能够作为JSON端点提供生成的Swagger
                app.UseSwagger();
                // 使中间件能够提供swagger-ui(HTML、JS、CSS等)
                app.UseSwaggerUI(options =>
                {
                    // SwaggerEndPoint
                    options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "YoyoCmsTemplate API V1");
                    options.EnableDeepLinking();
                    options.DocExpansion(DocExpansion.None);

                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("LTMCompanyName.YoyoCmsTemplate.Web.Host.wwwroot.swagger.monitor.index.html");
                }); // URL: /swagger
            } // Enable middleware to serve generated Swagger as a JSON endpoint
        }

        /// <summary>
        /// 配置类库的注释,开发时使用
        /// </summary>
        /// <param name="options"> </param>
        private void ConfigApiDoc(SwaggerGenOptions options)
        {
#if DEBUG
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            //Application层
            var commentsFileName = typeof(YoyoCmsTemplateApplicationModule).Assembly.GetName().Name + ".XML";
            var xmlPath = Path.Combine(basePath, commentsFileName);
            //添加注释
            options.IncludeXmlComments(xmlPath);
#endif
        }
    }
}
