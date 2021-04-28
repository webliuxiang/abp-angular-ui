using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Caching;
using GitLabApiClient;
using GitLabApiClient.Models.Projects.Responses;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Documents
{
    public static class GitlabCacheManagerExtensions
    {

        public static ITypedCache<string, GitLabClient> GetGitlabAPICache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, GitLabClient>(GitlabCacheItem.GitlabAPICacheName);
        }


        public static ITypedCache<string, IList<Project>> GetGitlabProjectsCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, IList<Project>>(GitlabCacheItem.GitlabAPICacheName);
        }
    }
}
