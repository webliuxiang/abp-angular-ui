using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;


namespace YoYo.ABPCommunity.Docs.Projects.DomainService
{
    public interface IProjectManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProject();

        Task<Project> FindByShortNameAsync(string shortName);





    }
}
