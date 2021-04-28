using System;
using System.Collections.Generic;
using System.Text;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Configuration
{
    public static class AppSettingsExtensions
    {
        /// <summary>
        /// 是否开启多租户
        /// </summary>
        static bool? MultiTenancy_IsEnabled { get; set; }
        /// <summary>
        /// 数据库驱动类型
        /// </summary>
        static DatabaseDrivenType? DrivenType { get; set; }


        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string ConnectionStringsDefault(this IConfiguration configuration)
        {
          return   configuration.GetConnectionString(AppSettingNames.System.ConnectionStrings_Default);

         }

        /// <summary>
        /// 根据配置获取驱动类型
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DatabaseDrivenType GetDatabaseDrivenType(this IConfiguration configuration)
        {
            if (DrivenType.HasValue)
            {
                return DrivenType.Value;
            }

            switch (configuration["ConnectionStrings:DatabaseDrivenType"]?.ToLower())
            {
                case "postgresql":
                    DrivenType = DatabaseDrivenType.PostgreSQL;
                    break;
                case "mysql":
                    DrivenType = DatabaseDrivenType.MySql;
                    break;
                case "oracle":
                    DrivenType = DatabaseDrivenType.Oracle;
                    break;
                case "devart-oracle":
                    DrivenType = DatabaseDrivenType.DevartOracle;
                    break;
                default:
                    DrivenType = DatabaseDrivenType.SqlServer;
                    break;
            }


            return DrivenType.Value;
        }

        /// <summary>
        /// 根据配置获取 Devart 的 license
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetDevartLicense(this IConfiguration configuration)
        {
            return configuration["ConnectionStrings:DevartLicense"];
        }

        /// <summary>
        /// 是否开启多租户,默认为true
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool MultiTenancyIsEnabled(this IConfiguration configuration)
        {
            if (MultiTenancy_IsEnabled.HasValue)
            {
                return MultiTenancy_IsEnabled.Value;
            }

            try
            {
                MultiTenancy_IsEnabled = configuration.GetValue<bool?>(AppSettingNames.System.MultiTenancy_IsEnabled);
            }
            catch
            {
                MultiTenancy_IsEnabled = true;
            }

            if (MultiTenancy_IsEnabled.HasValue)
            {
                return MultiTenancy_IsEnabled.Value;
            }

            return true;
        }

        /// <summary>
        /// 是否启用JWT,默认为true
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool AuthenticationJwtBearerIsEnabled(this IConfiguration configuration)
        {
            var val = configuration.GetValue<bool?>(AppSettingNames.System.Authentication_JwtBearer_IsEnabled);
            if (val.HasValue)
            {
                return val.Value;
            }

            return true;
        }

        /// <summary>
        /// jwt SecurityKey
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string AuthenticationJwtBearerSecurityKey(this IConfiguration configuration)
        {
            return configuration[AppSettingNames.System.Authentication_JwtBearer_SecurityKey];
        }

        /// <summary>
        /// jwt Issuer
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string AuthenticationJwtBearerIssuer(this IConfiguration configuration)
        {
            return configuration[AppSettingNames.System.Authentication_JwtBearer_Issuer];
        }

        /// <summary>
        /// jwt Audience
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string AuthenticationJwtBearerAudience(this IConfiguration configuration)
        {
            return configuration[AppSettingNames.System.Authentication_JwtBearer_Audience];
        }
    }

    /// <summary>
    /// EF Core数据库驱动类型
    /// </summary>
    public enum DatabaseDrivenType
    {
        SqlServer = 0,
        PostgreSQL = 1,
        MySql = 2,
        Oracle = 3,
        DevartOracle = 4
    }
}
