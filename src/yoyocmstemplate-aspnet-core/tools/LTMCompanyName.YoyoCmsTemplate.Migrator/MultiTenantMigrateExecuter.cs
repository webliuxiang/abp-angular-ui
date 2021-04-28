using System;
using System.Collections.Generic;
using System.Data.Common;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Seed;

namespace LTMCompanyName.YoyoCmsTemplate.Migrator
{
    public class MultiTenantMigrateExecuter : ITransientDependency
    {
        private readonly Log _log;
        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;

        public MultiTenantMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            Log log,
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            _log = log;

            _migrator = migrator;
            _tenantRepository = tenantRepository;
            _connectionStringResolver = connectionStringResolver;
        }

        public bool Run(bool skipConnVerification)
        {
            var hostConnStr = CensorConnectionString(_connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host)));

            Console.WriteLine(hostConnStr);

            if (hostConnStr.IsNullOrWhiteSpace())
            {
                _log.Write("配置文件包含一个默认名称为“Default”的连接字符串");
                return false;
            }

            _log.Write("主机数据库的连接字符串: " + ConnectionStringHelper.GetConnectionString(hostConnStr));
            if (!skipConnVerification)
            {
                _log.Write("是否继续迁移此主机数据库及所有租户数据。同意输入Y键，取消输入N键): ");
                var command = Console.ReadLine();
                if (!command.IsIn("Y", "y"))
                {
                    _log.Write("迁移数据取消。");
                    return false;
                }
            }

            _log.Write("开始为主机数据库迁移数据...");

            try
            {
                _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                _log.Write("迁移主机数据库时发生一个内部错误:");
                _log.Write(ex.ToString());
                _log.Write("取消迁移数据。");


                throw new Exception("迁移执行失败");
            }

            _log.Write("主机数据库迁移完成。");
            _log.Write("--------------------------------------------------------");

            var migratedDatabases = new HashSet<string>();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                _log.Write($"开始为租户数据库迁移数据，迁移状态:当前租户{i + 1} /总租户数量{tenants.Count}... ");
                _log.Write("租户简称              : " + tenant.Name);
                _log.Write("租户名称       : " + tenant.TenancyName);
                _log.Write("租户 Id         : " + tenant.Id);
                _log.Write("连接字符串信息 : " + SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString));

                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        _migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        _log.Write("迁移租户数据库过程中出错:");
                        _log.Write(ex.ToString());
                        _log.Write("跳过此租户, 并将继续为其他租户迁移数据...");
                    }

                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    _log.Write("此数据库以前已经迁移过 (同一数据库中有多个租户)。跳过它..。");
                }

                _log.Write(string.Format("租户数据库迁移已完成. ({0} / {1})", (i + 1), tenants.Count));
                _log.Write("--------------------------------------------------------");
            }

            _log.Write("所有数据库都已迁移。");

            return true;
        }


        private static string CensorConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            var keysToMask = new[] { "password", "pwd", "user id", "uid" };

            foreach (var key in keysToMask)
            {
                if (builder.ContainsKey(key))
                {
                    builder[key] = "*****";
                }
            }

            return builder.ToString();
        }
    }
}
