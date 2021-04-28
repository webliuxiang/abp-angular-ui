using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.Weixin;
using YoYo;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Startup
{
    public static class SenparcWXConfigurer
    {
        /// <summary>
        /// 添加微信
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddWechat(IServiceCollection services, IConfiguration configuration)
        {
            // 盛派微信配置
            services.AddYoYoSenparcCO2NET(configuration);
        }

        /// <summary>
        /// 启用微信
        /// </summary>
        /// <param name="env"></param>
        public static void UseWechat(IWebHostEnvironment env, SenparcSetting senparcSetting)
        {
            var registerService = senparcSetting.UseYoYoSenparcCO2NET(env.ContentRootPath);

            registerService = registerService.UseYoYoSenparcCO2NETGlobalCache(null, null)
                .RegisterTraceLog(ConfigTraceLog)//配置TraceLog
                .UseYoYoSenparcWeixin(null, senparcSetting);
        }




        /// <summary>
        /// 配置微信跟踪日志
        /// </summary>
        private static void ConfigTraceLog()
        {
            //这里设为Debug状态时，/App_Data/WeixinTraceLog/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭

            //如果全局的IsDebug（Senparc.CO2NET.Config.IsDebug）为false，此处可以单独设置true，否则自动为true
            Senparc.CO2NET.Trace.SenparcTrace.SendCustomLog("系统日志", "系统启动");//只在Senparc.Weixin.Config.IsDebug = true的情况下生效

            //全局自定义日志记录回调
            Senparc.CO2NET.Trace.SenparcTrace.OnLogFunc = () =>
            {
                //加入每次触发Log后需要执行的代码
            };

            //当发生基于WeixinException的异常时触发
            WeixinTrace.OnWeixinExceptionFunc = (ex) =>
            {
                //加入每次触发WeixinExceptionLog后需要执行的代码


            };
        }
    }
}

