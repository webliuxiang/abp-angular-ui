using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace LTMCompanyName.YoyoCmsTemplate.Configuration
{
    public class AppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public AppConfigurationAccessor(IWebHostEnvironment env)
        {
            Configuration = env.GetAppConfiguration();
        }
    }
}
