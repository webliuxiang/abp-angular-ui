using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using LTMCompanyName.YoyoCmsTemplate.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    public static class YoyoCmsTemplateDbContextConfigurer
    {
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });

        /// <summary>
        /// 是否启用 lazy loading proxies,默认为fasle
        /// </summary>
        static bool _enableLazyLoading = false;

        //todo: 等待文档内容

        public static void Configure(IConfiguration configuration, DbContextOptionsBuilder<YoyoCmsTemplateDbContext> builder, string connectionString)
        {
            switch (configuration.GetDatabaseDrivenType())
            {
                case DatabaseDrivenType.SqlServer:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseSqlServer(connectionString);
                    break;
                case DatabaseDrivenType.PostgreSQL:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseRivenPostgreSQL(connectionString);
                    break;
                case DatabaseDrivenType.MySql:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseMySql(connectionString);
                    break;
                case DatabaseDrivenType.Oracle:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseRivenOracle(connectionString);
                    break;
                case DatabaseDrivenType.DevartOracle:
                    var license = configuration.GetDevartLicense();
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseRivenDevartOracle(connectionString, license);
                    break;
            }

            ConfigureLog(builder);
        }

        public static void Configure(IConfiguration configuration, DbContextOptionsBuilder<YoyoCmsTemplateDbContext> builder, DbConnection connection)
        {
            switch (configuration.GetDatabaseDrivenType())
            {
                case DatabaseDrivenType.SqlServer:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseSqlServer(connection);
                    break;
                case DatabaseDrivenType.PostgreSQL:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseRivenPostgreSQL(connection);
                    break;
                case DatabaseDrivenType.MySql:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseMySql(connection);
                    break;
                case DatabaseDrivenType.Oracle:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseRivenOracle(connection);
                    break;
                case DatabaseDrivenType.DevartOracle:
                    builder
                        .UseLazyLoadingProxies(_enableLazyLoading)
                        .UseRivenDevartOracle(connection);
                    break;
            }


            ConfigureLog(builder);
        }

        /// <summary>
        /// 配置日志,只在Debug模式下生效
        /// </summary>
        /// <param name="builder"></param>
        static void ConfigureLog(DbContextOptionsBuilder<YoyoCmsTemplateDbContext> builder)
        {
#if DEBUG
            builder.UseLoggerFactory(MyLoggerFactory);
#endif
        }

    }


}
