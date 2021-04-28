using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;


namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.DomainService
{
    public interface IDropdownListManager : IDomainService
    {


        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        IQueryable<DropdownList> QueryDropdownLists();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>

        IQueryable<DropdownList> QueryDropdownListsAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DropdownList> FindByIdAsync(string id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(string id);


        /// <summary>
        /// 添加下拉组件
        /// </summary>
        /// <param name="entity">下拉组件实体</param>
        /// <returns></returns>
        Task<DropdownList> CreateAsync(DropdownList entity);

        /// <summary>
        /// 修改下拉组件
        /// </summary>
        /// <param name="entity">下拉组件实体</param>
        /// <returns></returns>
        Task UpdateAsync(DropdownList entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(string id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<string> input);



        //// custom codes



        //// custom codes end





    }
}
