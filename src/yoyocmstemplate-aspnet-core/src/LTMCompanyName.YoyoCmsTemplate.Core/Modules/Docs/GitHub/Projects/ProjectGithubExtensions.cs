using Abp;
using JetBrains.Annotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.GitHub.Projects
{
    public static class ProjectGithubExtensions
    {
        public static string GetGitHubUrl([NotNull] this Project project)
        {
            CheckGitHubProject(project);

            return project.GetExtraProperties()["GitHubRootUrl"] as string;

          
        }

        public static string GetGitHubUrl([NotNull] this Project project, string version)
        {
            return project
                .GetGitHubUrl()
                .Replace("{version}", version);
        }

        public static string GetGitHubUrlForCommitHistory([NotNull] this Project project)
        {
            //获取贡献人：https://api.github.com/repos/abpframework/abp/commits?path=docs/zh-Hans/Index.md
            return project
                .GetGitHubUrl()
                .Replace("github.com", "api.github.com/repos")
                .Replace("tree/{version}/", "commits?path=");
        }

        public static void SetGitHubUrl([NotNull] this Project project, string value)
        {
            CheckGitHubProject(project);
            project.GetExtraProperties()["GitHubRootUrl"] = value;
        }

        public static string GetGitHubAccessTokenOrNull([NotNull] this Project project)
        {
            CheckGitHubProject(project);
            return project.GetExtraProperties()["GitHubAccessToken"] as string;
        }

        public static string GetGithubUserAgentOrNull([NotNull] this Project project)
        {
            CheckGitHubProject(project);
            return project.GetExtraProperties()["GitHubUserAgent"] as string;
        }

        public static void SetGitHubAccessToken([NotNull] this Project project, string value)
        {
            CheckGitHubProject(project);
            project.GetExtraProperties()["GitHubAccessToken"] = value;
        }

        private static void CheckGitHubProject(Project project)
        {
            Check.NotNull(project, nameof(project));

        }
    }
}
