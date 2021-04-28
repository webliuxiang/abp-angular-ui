using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp;

using Hangfire.Annotations;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization
{
    public static class AuthExceptionHandlingMiddlewareExtension
    {
        /// <summary>
        /// 注册52abp实现的Blazor认证中间件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置选项</param>
        /// <returns></returns>
        public static IServiceCollection Add52AbpBlazorAuthExceptionHandling([NotNull] this IServiceCollection services, Action<AuthExceptionHandlingOption> configuration = null)
        {
            Check.NotNull(services, nameof(services));

            services.TryAddTransient<AuthExceptionHandlingMiddleware>();



            var authExceptionHandlingOption = new AuthExceptionHandlingOption();
            authExceptionHandlingOption.DefaultRedirectPath = "error";
            configuration?.Invoke(authExceptionHandlingOption);
            services.TryAddSingleton(authExceptionHandlingOption);


            return services;
        }


        /// <summary>
        /// 使用52abp实现的Blazor认证中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder Use52AbpBlazorAuthExceptionHandling([NotNull] this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.UseMiddleware<AuthExceptionHandlingMiddleware>();

            return app;
        }
    }
}
