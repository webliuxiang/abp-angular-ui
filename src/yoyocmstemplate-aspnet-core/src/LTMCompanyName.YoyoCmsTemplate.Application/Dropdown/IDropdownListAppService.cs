using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Dropdown
{
    /// <summary>
    /// 下拉组件应用层服务的接口方法
    ///</summary>
    public interface IDropdownListAppService : IApplicationService
    {

        /// <summary>
        /// 通过DDTypeId获取下拉组件ListDto信息
        /// </summary>
        Task<List<DropdownListListDto>> GetByDDTypeId(string dDTypeId);

        /// <summary>
		/// 获取下拉组件的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DropdownListListDto>> GetPaged(GetDropdownListsInput input);


		/// <summary>
		/// 通过指定id获取下拉组件ListDto信息
		/// </summary>
		Task<DropdownListListDto> GetById(EntityDto<string> input);


        /// <summary>
        /// 返回实体下拉组件的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDropdownListForEditOutput> GetForEdit(string input);


        /// <summary>
        /// 添加或者修改下拉组件的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateDropdownListInput input);


        /// <summary>
        /// 删除下拉组件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<string> input);

		
        /// <summary>
        /// 批量删除下拉组件
        /// </summary>
        Task BatchDelete(List<string> input);


		
    }
}
