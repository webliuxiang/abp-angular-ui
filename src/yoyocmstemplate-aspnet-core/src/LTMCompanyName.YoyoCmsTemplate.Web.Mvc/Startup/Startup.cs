using System;
using System.IO;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using Hangfire;
using HealthChecks.UI.Client;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Common;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.HealthCheck;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.Swagger;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "YoyoCmsTemplate";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC MVC
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

                });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddScoped<IWebResourceManager, WebResourceManager>();

            services.AddSignalR();

            //Swagger -启用此行以及Configure方法中的相关行，以启用Swagger UI
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

                    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                    var commentsFileName = typeof(YoyoCmsTemplateApplicationModule).Assembly.GetName().Name +
                                           ".XML";
                    var xmlPath = Path.Combine(basePath, commentsFileName);
                    //添加注释
                    options.IncludeXmlComments(xmlPath);
                });
            }

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
                    services.AddHealthChecksUI();
                }
            }
            #endregion 配置健康检查服务
            // Configure Abp and Dependency Injection
            return services.AddAbp<YoyoCmsTemplateWebMvcModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // 初始化 ABP 框架.
            app.UseAbp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              //  app.UseBrowserLink();

            }

            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");

            }


            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseAuthorization();
            app.UseAbpRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");

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
                    options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "52ABP-PRO-MVC-Template  API V1");

                    options.InjectBaseUrl(_appConfiguration["App:AdminServerRootAddress"]);
                }); //URL: /swagger
            }
        }
    }
}
