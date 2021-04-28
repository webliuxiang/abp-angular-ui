using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.UI;
using L._52ABP.Common.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.GitHub.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;
using Newtonsoft.Json.Linq;

using Octokit;
using YoYo.ABPCommunity.Docs.Documents.Services;
using Project = LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Project;

namespace YoYo.ABPCommunity.Docs.GitHub.Documents
{
    //TODO: Needs more refactoring

    public class GithubDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "GitHub";

        private readonly IGithubRepositoryManager _githubRepositoryManager;

        public GithubDocumentStore(IGithubRepositoryManager githubRepositoryManager)
        {
            _githubRepositoryManager = githubRepositoryManager;
        }
        
        public virtual async Task<Document> GetDocumentAsync(Project project, string documentName, string version)
        {
            var token = project.GetGitHubAccessTokenOrNull();
            var rootUrl = project.GetGitHubUrl(version);
            var rawRootUrl = CalculateRawRootUrl(rootUrl);
            var rawDocumentUrl = rawRootUrl + documentName;
            var commitHistoryUrl = project.GetGitHubUrlForCommitHistory() + documentName;
            var userAgent = project.GetGithubUserAgentOrNull();
            var isNavigationDocument = documentName == project.NavigationDocumentName;
            var editLink = rootUrl.ReplaceFirst("/tree/", "/blob/") + documentName;
            var localDirectory = "";
            var fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1);
            }
            var contributors=new List<DocumentContributor>();
            if (!isNavigationDocument)
            {
                 contributors = await GetContributors(commitHistoryUrl, token, userAgent);

            }


            var document=
              new Document
            {
                Title = documentName,
                EditLink = editLink,
                RootUrl = rootUrl,
                RawRootUrl = rawRootUrl,
                Format = project.Format,
                LocalDirectory = localDirectory,
                FileName = fileName,
                //Contributors = new List<DocumentContributor>(),
                Contributors = contributors,
                Version = version,
                Content = await DownloadWebContentAsStringAsync(rawDocumentUrl, token, userAgent)
            };


            return document;
        }

        public async Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            List<VersionInfo> versions;



            try
            {

                var releases = await GetReleasesAsync(project);

                versions = releases
                    .OrderByDescending(r => r.PublishedAt)
                    .Select(r => new VersionInfo
                    {
                        TagName = r.TagName,
                        Name = r.Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                //TODO: 隐藏错误可能不是一个好主意!


                Logger.Error(ex.Message, ex);

                
                versions = new List<VersionInfo>();
            }

            if (!versions.Any() && !string.IsNullOrEmpty(project.LatestVersionBranchName))
            {
                versions.Add(new VersionInfo { TagName = "1.0.0", Name = project.LatestVersionBranchName });
            }

            return versions;
        }

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string version)
        {
            var rawRootUrl = CalculateRawRootUrl(project.GetGitHubUrl(version));
            var content = await DownloadWebContentAsByteArrayAsync(
                rawRootUrl + resourceName,
                project.GetGitHubAccessTokenOrNull(),
                project.GetGithubUserAgentOrNull()
            );

            return new DocumentResource(content);
        }

        private async Task<IReadOnlyList<Release>> GetReleasesAsync(Project project)
        {
            var url = project.GetGitHubUrl();  //https://github.com/52ABP/Documents/tree/{version}/src/mvc/"
            var ownerName = GetOwnerNameFromUrl(url);  //52ABP

            var repositoryName = GetRepositoryNameFromUrl(url);  //Documents

            var releases = await _githubRepositoryManager.GetReleasesAsync(ownerName, repositoryName, project.GetGitHubAccessTokenOrNull());

            return releases;


        }

        /// <summary>
        /// 获取组织或者个人名称
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual string GetOwnerNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash = url.Substring(url.IndexOf("github.com/", StringComparison.OrdinalIgnoreCase) + "github.com/".Length);
                return urlStartingAfterFirstSlash.Substring(0, urlStartingAfterFirstSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }


        /// <summary>
        /// 获取仓库名称
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual string GetRepositoryNameFromUrl(string url)
        {
            try
            {
                var urlStartingAfterFirstSlash = url.Substring(url.IndexOf("github.com/", StringComparison.OrdinalIgnoreCase) + "github.com/".Length);
                var urlStartingAfterSecondSlash = urlStartingAfterFirstSlash.Substring(urlStartingAfterFirstSlash.IndexOf('/') + 1);
                return urlStartingAfterSecondSlash.Substring(0, urlStartingAfterSecondSlash.IndexOf('/'));
            }
            catch (Exception)
            {
                throw new Exception($"Github url is not valid: {url}");
            }
        }

        private async Task<string> DownloadWebContentAsStringAsync(string rawUrl, string token, string userAgent)
        {
            try
            {
                Logger.Info("从Github下载内容(以字符串异步的形式下载Web内容):"+ rawUrl);
                 
               // Logger.LogInformation("Downloading content from Github (DownloadWebContentAsStringAsync): " + rawUrl);

                return await _githubRepositoryManager.GetFileRawStringContentAsync(rawUrl, token, userAgent);
            }
            catch (Exception ex)
            {
                //TODO: 仅当文档确实不可用时才处理
                Logger.Warn(ex.Message, ex);
                throw  new UserFriendlyException("下载文档失败，没有发现该文档"+rawUrl+ex.Message);


               
            }
        }

        private async Task<byte[]> DownloadWebContentAsByteArrayAsync(string rawUrl, string token, string userAgent)
        {
            try
            {
                Logger.Info("从Github下载内容(以字节数组异步方式下载Web内容): " + rawUrl);

                return await _githubRepositoryManager.GetFileRawByteArrayContentAsync(rawUrl, token, userAgent);
            }
            catch (Exception ex)
            {
                //TODO: 只在资源确实不可用时处理
                Logger.Warn(ex.Message, ex);
                throw new UserFriendlyException("该资源不存在");

            }
        }

        private async Task<List<DocumentContributor>> GetContributors(string url, string token, string userAgent)
        {
            var contributors = new List<DocumentContributor>();

            try
            {
                var commitsJsonAsString = await DownloadWebContentAsStringAsync(url, token, userAgent);

                var commits = JArray.Parse(commitsJsonAsString);

                foreach (var commit in commits)
                {
                    var author = commit["author"];

                    contributors.Add(new DocumentContributor
                    {
                        Username = (string)author["login"],
                        UserProfileUrl = (string)author["html_url"],
                        AvatarUrl = (string)author["avatar_url"]
                    });
                }

                contributors = contributors.GroupBy(c => c.Username).OrderByDescending(c=>c.Count())
                    .Select( c => c.FirstOrDefault()).ToList();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
            
            return contributors;
        }

        private static string CalculateRawRootUrl(string rootUrl)
        {
            return rootUrl
                .Replace("github.com", "raw.githubusercontent.com")
                .ReplaceFirst("/tree/", "/")
                .EnsureEndsWith('/');
        }
    }
}
