using System;
using Abp;
using JetBrains.Annotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;
using YoYo.ABPCommunity.Docs.Documents.FileSystem.Documents;
using YoYo.ABPCommunity.Docs.Projects;

namespace YoYo.ABPCommunity.Docs.Documents.FileSystem.Projects
{
    public static class ProjectFileSystemExtensions
    {
        public static string GetFileSystemPath([NotNull] this Project project)
        {
            CheckFileSystemProject(project);
            return project.GetExtraProperties()["Path"] as string;


             
        }

        public static void SetFileSystemPath([NotNull] this Project project, string value)
        {
            CheckFileSystemProject(project);
            project.GetExtraProperties()["Path"] = value;
        }

        private static void CheckFileSystemProject(Project project)
        {
            Check.NotNull(project, nameof(project));

            if (project.DocumentStoreType != FileSystemDocumentStore.Type)
            {
                throw new ApplicationException("Given project has not a FileSystem document store!");
            }
        }
    }
}
