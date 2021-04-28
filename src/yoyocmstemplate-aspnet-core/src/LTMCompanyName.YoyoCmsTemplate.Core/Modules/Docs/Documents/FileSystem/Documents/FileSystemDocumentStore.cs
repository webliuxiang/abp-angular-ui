using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Abp.Domain.Services;
using YoYo.ABPCommunity.Docs.Documents.FileSystem.Projects;
 using YoYo.ABPCommunity.Docs.Documents.Services;
using YoYo.ABPCommunity.Docs.Projects;
using L._52ABP.Common.IO;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;

namespace YoYo.ABPCommunity.Docs.Documents.FileSystem.Documents
{
    public class FileSystemDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "FileSystem";

        public async Task<Document> GetDocumentAsync(Project project, string documentName, string version)
        {
            var projectFolder = project.GetFileSystemPath();
            var path = Path.Combine(projectFolder, documentName);

            CheckDirectorySecurity(projectFolder, path);
            var content = await LFileHelper.ReadAllTextAsync(path);
            var localDirectory = "";

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
            }

            return new Document
            {
                Content =  content,
                FileName = Path.GetFileName(path),
                Format = project.Format,
                LocalDirectory = localDirectory,
                Title = documentName,
                RawRootUrl = $"/document-resources?projectId={project.Id.ToString()}&version={version}&name=",
                RootUrl = "/"
            };
        }

        public Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            return Task.FromResult(new List<VersionInfo>());
        }

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string version)
        {
            var projectFolder = project.GetFileSystemPath();
            var path = Path.Combine(projectFolder, resourceName);

            if (!LDirectoryHelper.IsSubDirectoryOf(projectFolder, path))
            {
                throw new SecurityException("Can not get a resource file out of the project folder!");
            }

            return new DocumentResource(await LFileHelper.ReadAllBytesAsync(path));
        }

        private static void CheckDirectorySecurity(string projectFolder, string path)
        {
            if (!LDirectoryHelper.IsSubDirectoryOf(projectFolder, path))
            {
                throw new SecurityException("Can not get a resource file out of the project folder!");
            }
        }
    }
}
