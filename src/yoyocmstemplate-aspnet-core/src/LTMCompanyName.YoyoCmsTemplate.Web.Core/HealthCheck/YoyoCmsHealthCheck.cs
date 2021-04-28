using LTMCompanyName.YoyoCmsTemplate.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace LTMCompanyName.YoyoCmsTemplate.HealthCheck
{
    public static class YoyoCmsHealthCheck
    {
        public static IHealthChecksBuilder AddYoyoCmsHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<YoyoCmsTemplateDbContextHealthCheck>("数据库连接");
            builder.AddCheck<YoyoCmsTemplateDbContextUsersHealthCheck>("数据库连接和用户检查");
            builder.AddCheck<CacheHealthCheck>("缓存");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
