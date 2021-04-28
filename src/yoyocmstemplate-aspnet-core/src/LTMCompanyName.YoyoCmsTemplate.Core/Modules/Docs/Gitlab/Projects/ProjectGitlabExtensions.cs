using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Abp;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Projects
{
    public static class ProjectGitlabExtensions
    {
        //主站点Url
        public static string GetGitLabBaseUrl([NotNull] this Project project)
        {
            return project.GetExtraProperties()["GitLabBaseUrl"] as string;
        }

        //文档根目录Url
        public static string GetGitLabRootUrl([NotNull] this Project project)
        {
            return project.GetExtraProperties()["GitLabRootUrl"] as string;
        }

        public static string GetGitLabRootUrl([NotNull] this Project project, string version)
        {
            return project
                .GetGitLabRootUrl()
                .Replace("{version}", version);
        }

        public static string GetGitLabAccessToken([NotNull] this Project project)
        {
            return project.GetExtraProperties()["GitLabAccessToken"] as string;
        }



        public static string GetGitLabProjectId([NotNull] this Project project)
        {
            var baseUrl = project.GetGitLabBaseUrl();
            var rootUrl = project.GetGitLabRootUrl();

            //http://code.52abp.com/52abp/ABPCNDocs/tree/{version}/src
            var trimStartBaseUrl = rootUrl.Substring(baseUrl.Length, rootUrl.Length - baseUrl.Length).TrimStart('/');
            var projectId = trimStartBaseUrl.Substring(0, trimStartBaseUrl.IndexOf("/tree/"));

            return projectId;
        }



        public static string GetGitLabUserAgent([NotNull] this Project project)
        {
            return project.GetExtraProperties()["GitLabUserAgent"] as string;
        }



        //public static string GetGitLabUrlForCommitHistory([NotNull] this Project project)
        //{
        //    //获取贡献人：https://api.github.com/repos/abpframework/abp/commits?path=docs/zh-Hans/Index.md

        //    //http://code.52abp.com/52abp/ABPCNDocs/commits/master
        //    return project
        //        .GetGitLabRootUrl()
        //        .Replace("github.com", "api.github.com/repos")
        //        .Replace("tree/{version}/", "commits?path=");
        //}


    }
}
