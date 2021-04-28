using System;
using System.Net.Http;
using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Startup
{
    public class Startup
    {

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            this._env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {


            IdentityRegistrar.Register(services);

            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSignalR();

            services.AddHeadElementHelper(); // <- Add this.
            services.AddHttpClient();
             services.AddSession(options =>
            {
                //记录浏览量会根据session判断用户，默认是1天，所以session时效设置要求在1天以上
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

             services.AddAuthorizationCore();
        //    services.AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();

          services.AddScoped<IdentityAuthenticationStateProvider>();
          services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());

            services.Add52AbpBlazorAuthExceptionHandling(options =>
            {
                options.DefaultRedirectPath = "/error";
                options.NoLoginRedirectPath = "/no-login";
                options.NoPermissionRedirectPath = "/no-permission";
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<YoyoCmsTemplateWebPublicModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseAbp(); // Initializes ABP framework.

            if (_env.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
              app.UseWebAssemblyDebugging();

            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHeadElementServerPrerendering(); // <- Add this.

            app.UseStaticFiles();

            app.UseRouting();

            app.Use52AbpBlazorAuthExceptionHandling();

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseAuthorization();

            app.UseSession();

            app.UseAbpRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

            });
        }
    }
}
