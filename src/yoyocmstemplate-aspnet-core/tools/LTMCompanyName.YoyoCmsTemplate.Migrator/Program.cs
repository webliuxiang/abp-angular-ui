using System;

using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Collections.Extensions;
using Abp.Dependency;

using Castle.Facilities.Logging;

using LTMCompanyName.YoyoCmsTemplate.Migrator.Workers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LTMCompanyName.YoyoCmsTemplate.Migrator
{
    public class Program
    {
        /// <summary>
        /// 自动退出模式
        /// </summary>
        private static bool _quietMode;

        /// <summary>
        /// 监听80端口,docker部署时使用
        /// </summary>
        private static bool _listener;

        public static void Main(string[] args)
        {
            ParseArgs(args);

            using (var bootstrapper = AbpBootstrapper.Create<YoyoCmsTemplateMigratorModule>())
            {
                bootstrapper.IocManager.IocContainer
                    .AddFacility<LoggingFacility>(
                        f => f.UseAbpLog4Net().WithConfig("log4net.config")
                    );

                bootstrapper.Initialize();

                using (var migrateExecuter = bootstrapper.IocManager
                    .ResolveAsDisposable<MultiTenantMigrateExecuter>())
                {
                    var migrationSucceeded = migrateExecuter.Object.Run(_quietMode);
                }
            }

            Console.WriteLine("迁移执行结束...");

            // 释放资源
            GC.Collect();


            // docker时会使用此监听80,标识数据库已完整初始化
            if (_listener)
            {
                Console.WriteLine("启用监听");
                CreateHostBuilder(new string[] { }).Build().Run();
                Console.WriteLine("停止监听");
            }

            if (_quietMode)
            {
                Console.WriteLine("自动退出...");
            }
            else
            {
                Console.WriteLine("按下回车键退出...");
                Console.ReadLine();
            }
        }

        private static void ParseArgs(string[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return;
            }

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "-q":
                        _quietMode = true;
                        break;
                    case "--listener":
                        _listener = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Host 主机
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHostedService<MigratorListenerWorker>();
               });
    }
}
