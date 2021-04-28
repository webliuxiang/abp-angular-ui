using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using GitLabApiClient.Models.Releases.Responses;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Documents
{
    public interface IGitlabRepositoryManager: ITransientDependency
    {
        /// <summary>
        /// 获取文件原始字符串内容异步
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="token"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        Task<string> GetFileRawStringContentAsync(string rawUrl, string token, string userAgent);
        /// <summary>
        /// 获取文件原始字节数组内容异步
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="token"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        Task<byte[]> GetFileRawByteArrayContentAsync(string rawUrl, string token, string userAgent);
        /// <summary>
        /// 获取发布版本
        /// </summary>
        /// <param name="name"></param>
        /// <param name="repositoryName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Release>> GetReleasesAsync(string url, string token, string projectId);
    }
}
