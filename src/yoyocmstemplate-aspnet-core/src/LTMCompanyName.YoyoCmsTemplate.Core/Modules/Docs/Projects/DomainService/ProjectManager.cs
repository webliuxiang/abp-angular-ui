using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using YoYo.ABPCommunity.Docs.Projects.DomainService;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.DomainService
{
    /// <summary>
    /// Project领域层的业务管理
    ///</summary>
    public class ProjectManager :YoyoCmsTemplateDomainServiceBase, IProjectManager
    {
		
		private readonly IRepository<Project,Guid> _repository;

		/// <summary>
		/// Project的构造方法
		///</summary>
		public ProjectManager(
			IRepository<Project, Guid> repository
		)
		{
			_repository =  repository;
		}

      


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitProject()
		{
			throw new NotImplementedException();
		}

        // TODO:编写领域业务代码

        /// <summary>
        /// 通过项目短名称，获取项目实体信息
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public async Task<Project> FindByShortNameAsync(string shortName)
        {
           var entity= await _repository.FirstOrDefaultAsync(o => o.ShortName == shortName);


           if (entity==null)
           {
               
               throw new UserFriendlyException($"当前项目的简称{shortName}不存在！");
           }

           return entity;
        }




    }
}
