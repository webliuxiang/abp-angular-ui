using System;
using System.Linq;
using System.Security.Claims;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using L._52ABP.Application.GitlabAPIs.Dtos;
using L._52ABP.Core.Configs;
using LTMCompanyName.YoyoCmsTemplate.Blogging;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.Url;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;


namespace LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers
{
    /// <summary>
    /// 发动发布Markdown的文章
    /// </summary>
    public class AutoMaticallyPublishMarkdownPosts : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IPortalBlogAppService _portalBlogAppService;
        private IAppConfigurationAccessor _appConfiguration;
        public AutoMaticallyPublishMarkdownPosts(AbpTimer timer, IPortalBlogAppService portalBlogAppService, IAppConfigurationAccessor appConfiguration) : base(timer)
        {
            _portalBlogAppService = portalBlogAppService;
            _appConfiguration = appConfiguration;
         Timer.Period = 1000 * 60 * 60;
    //   Timer.Period = 5000;

        }

        [UnitOfWork]
        protected override void DoWork()
        {

            foreach (var config in AbpAppConfig.PostCategoryConfigs.Where(a => a.Enabled == true))
            {
                var input = new GitlabPostsNavInput
                {
                    PathWithNamespace = config.ReposName,
                    FileName = config.FileName,
                    FilePath = config.Filepath
                };
                input.SercertCode = _appConfiguration.GetMarkdownPostSercerCode();
                AsyncHelper.RunSync(() => _portalBlogAppService.AutoMaticallyPublishMarkdownPostsAsync(input));
            }
        }

    }
}
