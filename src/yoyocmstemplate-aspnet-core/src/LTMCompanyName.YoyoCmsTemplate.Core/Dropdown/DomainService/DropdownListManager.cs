

using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.DomainService
{
    /// <summary>
    /// 下拉组件领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class DropdownListManager : YoyoCmsTemplateDomainServiceBase, IDropdownListManager
    {

        private readonly IRepository<DropdownList, string> _dropdownListRepository;

        /// <summary>
        /// DropdownList的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public DropdownListManager(IRepository<DropdownList, string> dropdownListRepository)
        {
            _dropdownListRepository = dropdownListRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<DropdownList> QueryDropdownLists()
        {
            return _dropdownListRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<DropdownList> QueryDropdownListsAsNoTracking()
        {
            return _dropdownListRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DropdownList> FindByIdAsync(string id)
        {
            var entity = await _dropdownListRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(string id)
        {
            var result = await _dropdownListRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<DropdownList> CreateAsync(DropdownList entity)
        {
            entity.Id = await _dropdownListRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(DropdownList entity)
        {
            await _dropdownListRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _dropdownListRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<string> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _dropdownListRepository.DeleteAsync(a => input.Contains(a.Id));
        }


        //// custom codes



        //// custom codes end


    }
}
