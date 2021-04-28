using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class YoyoCmsTemplateDbContextFactory : IDesignTimeDbContextFactory<YoyoCmsTemplateDbContext>
    {
        public YoyoCmsTemplateDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<YoyoCmsTemplateDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            var connectionString = configuration.ConnectionStringsDefault();

            System.Console.WriteLine("迁移使用数据库连接字符串：");
            System.Console.WriteLine(connectionString);

            YoyoCmsTemplateDbContextConfigurer.Configure(configuration, builder, connectionString);

            return new YoyoCmsTemplateDbContext(builder.Options);
        }
    }
}
