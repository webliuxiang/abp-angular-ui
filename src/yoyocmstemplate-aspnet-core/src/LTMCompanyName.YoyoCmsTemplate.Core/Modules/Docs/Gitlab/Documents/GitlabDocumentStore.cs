using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.UI;
using GitLabApiClient.Models.Releases.Responses;
using L._52ABP.Common.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;
using Newtonsoft.Json.Linq;
using YoYo.ABPCommunity.Docs.Documents.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Documents
{
    public class GitlabDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "GitLab";
        private readonly IGitlabRepositoryManager _gitlabRepositoryManager;

        public GitlabDocumentStore(IGitlabRepositoryManager gitlabRepositoryManager)
        {
            _gitlabRepositoryManager = gitlabRepositoryManager;
        }

        public async Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            List<VersionInfo> versions;



            try
            {

                var releases = await GetReleasesAsync(project);

                versions = releases
                    .OrderByDescending(r => r.ReleasedAt)
                    .Select(r => new VersionInfo
                    {
                        TagName = r.TagName,
                        Name = r.ReleaseName
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
                versions.Add(new VersionInfo { TagName = project.MinimumVersion ?? "master", Name = project.LatestVersionBranchName });
            }

            return versions;

        }


        public async Task<Document> GetDocumentAsync(Project project, string documentName, string version)
        {
            //throw new NotImplementedException();
            
            var token = project.GetGitLabAccessToken();
            var rootUrl = project.GetGitLabRootUrl(version);  //http://code.52abp.com/52abp/ABPCNDocs/tree/{version}/src/
            var rawRootUrl = CalculateRawRootUrl(rootUrl); 
            var rawDocumentUrl = rawRootUrl + documentName;   //http://code.52abp.com/52abp/ABPCNDocs/raw/master/src/docs-nav.json
            //var commitHistoryUrl = project.GetGitLabUrlForCommitHistory() + documentName; //todo
            var userAgent = project.GetGitLabUserAgent();
            var isNavigationDocument = documentName == project.NavigationDocumentName;
            var editLink = rootUrl.ReplaceFirst("/tree/", "/blob/") + documentName;
            var localDirectory = "";
            var fileName = documentName;

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
                fileName = documentName.Substring(documentName.LastIndexOf('/') + 1);
            }
            var contributors = new List<DocumentContributor>();
            if (!isNavigationDocument)
            {
                //contributors = await GetContributors(commitHistoryUrl, token, userAgent);

            }


            var document =
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


        public async Task<DocumentResource> GetResource(Project project, string resourceName, string version)
        {
            var rawRootUrl = CalculateRawRootUrl(project.GetGitLabRootUrl(version));
            var content = await DownloadWebContentAsByteArrayAsync(
                rawRootUrl + resourceName,
                project.GetGitLabAccessToken(),
                project.GetGitLabUserAgent()
            );

            return new DocumentResource(content);
        }



        private async Task<IReadOnlyList<Release>> GetReleasesAsync(Project project)
        {

            var releases = await _gitlabRepositoryManager.GetReleasesAsync(project.GetGitLabBaseUrl(), project.GetGitLabAccessToken(), project.GetGitLabProjectId());

            return releases;


        }


        private async Task<string> DownloadWebContentAsStringAsync(string rawUrl, string token, string userAgent)
        {
            try
            {
                Logger.Info("从Gitlab下载内容(以字符串异步的形式下载Web内容):" + rawUrl);

                return await _gitlabRepositoryManager.GetFileRawStringContentAsync(rawUrl, token, userAgent);
            }
            catch (Exception ex)
            {
                //TODO: 仅当文档确实不可用时才处理
                Logger.Warn(ex.Message, ex);
                throw new UserFriendlyException("下载文档失败，没有发现该文档" + rawUrl + ex.Message);



            }
        }



        private async Task<byte[]> DownloadWebContentAsByteArrayAsync(string rawUrl, string token, string userAgent)
        {
            try
            {
                Logger.Info("从Gitlab下载内容(以字节数组异步方式下载Web内容): " + rawUrl);

                return await _gitlabRepositoryManager.GetFileRawByteArrayContentAsync(rawUrl, token, userAgent);
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

                contributors = contributors.GroupBy(c => c.Username).OrderByDescending(c => c.Count())
                    .Select(c => c.FirstOrDefault()).ToList();
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
                .ReplaceFirst("/tree/", "/raw/")
                .EnsureEndsWith('/');
        }
    }
}
