using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Runtime.Caching;
using Abp.UI;
using GitLabApiClient;
using GitLabApiClient.Internal.Paths;
using GitLabApiClient.Models.Files.Responses;
using GitLabApiClient.Models.Projects.Responses;
using GitLabApiClient.Models.Releases.Responses;
using L._52ABP.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Documents
{
    public class GitlabRepositoryManager: IGitlabRepositoryManager
    {
        public const string HttpClientName = "GitlabRepositoryManagerHttpClientName";
        private readonly ICacheManager _cacheManager;
        private readonly IHttpClientFactory _clientFactory;

        public GitlabRepositoryManager(ICacheManager cacheManager, IHttpClientFactory clientFactory)
        {
            _cacheManager = cacheManager;
            _clientFactory = clientFactory;
        }


        /// <summary>
        /// 获取gitlab的客户端的信息
        /// </summary>
        /// <param name="input">获取传入的参数，key和api地址</param>
        /// <returns></returns>
		private async Task<GitLabClient> GetClientInfoAsync(string gitlabUrl, string accessToken)
        {

            var client = await _cacheManager.GetGitlabAPICache().GetOrDefaultAsync(GitlabCacheItem.GitlabClientCache);

            if (client == null)
            {
                client = new GitLabClient(gitlabUrl, accessToken);
                await _cacheManager.GetGitlabAPICache().SetAsync(GitlabCacheItem.GitlabClientCache, client);
            }

            return client;
        }


        public async Task<IReadOnlyList<Release>> GetReleasesAsync(string url, string token, string projectId)
        {
            var client = await GetClientInfoAsync(url, token);


            var query = await client.Releases.GetAsync(projectId);


            return query.ToList();

        }

        public async Task<string> GetFileRawStringContentAsync(string rawUrl, string token, string userAgent)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientName);
            if (!token.IsNullOrWhiteSpace())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            }

            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent ?? "");

            return await httpClient.GetStringAsync(new Uri(rawUrl));
        }



        public async Task<byte[]> GetFileRawByteArrayContentAsync(string rawUrl, string token, string userAgent)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientName);
            if (!token.IsNullOrWhiteSpace())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            }

            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent ?? "");

            return await httpClient.GetByteArrayAsync(new Uri(rawUrl));
        }


    }
}
