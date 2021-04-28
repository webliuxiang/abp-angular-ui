using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Abp.Authorization;
using Abp.Runtime.Session;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization
{
    public class AuthExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
           try
            {
                await next(context);
            }
            catch (AbpAuthorizationException ex)
            {
                  var logger = context.RequestServices.GetRequiredService<ILogger<AuthExceptionHandlingMiddleware>>();
              

                // 发生异常,但是已经响应了
                if (context.Response.HasStarted)
                {
                    logger.LogWarning("中文 The application has an exception, but is already responding!");
                    throw;
                }
                var options = context.RequestServices.GetService<AuthExceptionHandlingOption>();
                var abpSession = context.RequestServices.GetRequiredService<IAbpSession>();

                var oldStatusCode = context.Response.StatusCode;

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.OnStarting(ProcessCacheHeaders, context.Response);
               
                if (abpSession.UserId.HasValue)
                {
                    // 重定向到指定页面 - 已登录无权限
                    context.Response.Redirect(
                       string.IsNullOrWhiteSpace(options.NoPermissionRedirectPath) ?
                            options.DefaultRedirectPath : options.NoPermissionRedirectPath
                        );
                }
                else
                {
                 //   context.Request.Query.TryGetValue("");
                    // 重定向到指定页面 - 未登录
                    context.Response.Redirect(
                       string.IsNullOrWhiteSpace(options.NoLoginRedirectPath) ?
                            options.DefaultRedirectPath : options.NoLoginRedirectPath
                        );
                }
            }
        }


        /// <summary>
        /// 处理请求头中缓存相关的键值
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual Task ProcessCacheHeaders(object state)
        {
            var response = (HttpResponse)state;

            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);

            return Task.CompletedTask;
        }

    }
}
