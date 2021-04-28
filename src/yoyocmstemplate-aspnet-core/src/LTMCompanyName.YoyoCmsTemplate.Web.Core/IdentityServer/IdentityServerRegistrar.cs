using Abp.IdentityServer4;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LTMCompanyName.YoyoCmsTemplate.IdentityServer
{
    public static class IdentityServerRegistrar
    {
        public static void Register(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
                .AddAbpPersistedGrants<YoyoCmsTemplateDbContext>()
                .AddAbpIdentityServer<User>();
        }
    }
}
