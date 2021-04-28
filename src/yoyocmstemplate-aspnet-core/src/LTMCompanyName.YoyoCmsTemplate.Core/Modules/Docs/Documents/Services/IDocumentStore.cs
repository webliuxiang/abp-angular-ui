using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;
using YoYo.ABPCommunity.Docs.Projects;

namespace YoYo.ABPCommunity.Docs.Documents.Services
{



    public interface IDocumentStore : IDomainService
    {
       
       /// <summary>
       /// 获取文档 的详情，通过工厂类进行中转
       /// </summary>
       /// <param name="project"></param>
       /// <param name="documentName"></param>
       /// <param name="version"></param>
       /// <returns></returns>
        Task<Document> GetDocumentAsync(Project project, string documentName, string version);

       /// <summary>
       /// 远程获取github的release信息，返回所有的版本列表
       /// </summary>
       /// <param name="project"></param>
       /// <returns></returns>
        Task<List<VersionInfo>> GetVersionsAsync(Project project);


        Task<DocumentResource> GetResource(Project project, string resourceName, string version);
    }
}
