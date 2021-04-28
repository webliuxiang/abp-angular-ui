
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.Domain.Repositories;
using L._52ABP.Application.Dtos;
using L._52ABP.Common.Extensions.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using iNeu.Equipment.Dropdown.Exporting;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown
{
    /// <summary>
    /// 下拉组件应用层服务的接口实现方法
    ///</summary>
    [AbpAuthorize]
    public class DropdownListAppService : YoyoCmsTemplateAppServiceBase, IDropdownListAppService
    {
        private readonly IRepository<DropdownList, string> _dropdownListRepository;
        private readonly IEnumExtensionsAppService _enumExtensionsAppService;
        private readonly IDropdownListManager _dropdownListManager;
        /// <summary>
        /// 构造函数
        ///</summary>
        public DropdownListAppService(
        IRepository<DropdownList, string> dropdownListRepository, 
        IDropdownListManager dropdownListManager, 
        IEnumExtensionsAppService enumExtensionsAppService)
        {
            _dropdownListRepository = dropdownListRepository;
            _dropdownListManager = dropdownListManager;
            _enumExtensionsAppService = enumExtensionsAppService;
        }


        /// <summary>
        /// 获取下拉组件的分页列表信息
        ///      </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(DropdownListPermissions.Query)]
        public async Task<PagedResultDto<DropdownListListDto>> GetPaged(GetDropdownListsInput input)
        {

            var query = _dropdownListRepository.GetAll()

                          //模糊搜索DDType_Id
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.DDType_Id.Contains(input.FilterText))
                          //模糊搜索Name_TX
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name_TX.Contains(input.FilterText))
                          //模糊搜索ParentId
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.ParentId.Contains(input.FilterText))
                          //模糊搜索ParentIdList
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.ParentIdList.Contains(input.FilterText))
            ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var dropdownListList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var dropdownListListDtos = ObjectMapper.Map<List<DropdownListListDto>>(dropdownListList);

            return new PagedResultDto<DropdownListListDto>(count, dropdownListListDtos);
        }


        /// <summary>
        /// 通过指定id获取DropdownListListDto信息
        /// </summary>
        [AbpAuthorize(DropdownListPermissions.Query)]
        public async Task<DropdownListListDto> GetById(EntityDto<string> input)
        {
            var entity = await _dropdownListRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<DropdownListListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 下拉组件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(DropdownListPermissions.Create, DropdownListPermissions.Edit)]
        public async Task<GetDropdownListForEditOutput> GetForEdit(string input)
        {
            var output = new GetDropdownListForEditOutput();
            DropdownListEditDto editDto;

            if (string.IsNullOrEmpty(input))
            {
                var entity = await _dropdownListRepository.GetAsync(input);
                editDto = ObjectMapper.Map<DropdownListEditDto>(entity);
            }
            else
            {
                editDto = new DropdownListEditDto();
            }

            output.DropdownTypeTypeEnum = _enumExtensionsAppService.GetEntityDoubleStringKeyValueList<DropdownType>();

            output.DropdownList = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改下拉组件的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(DropdownListPermissions.Create, DropdownListPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateDropdownListInput input)
        {

            if (!string.IsNullOrEmpty(input.DropdownList.Id))
            {
                await Update(input.DropdownList);
            }
            else
            {
                await Create(input.DropdownList);
            }
        }


        /// <summary>
        /// 新增下拉组件
        /// </summary>
        [AbpAuthorize(DropdownListPermissions.Create)]
        protected virtual async Task<DropdownListEditDto> Create(DropdownListEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<DropdownList>(input);
            //调用领域服务
            entity = await _dropdownListManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<DropdownListEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑下拉组件
        /// </summary>
        [AbpAuthorize(DropdownListPermissions.Edit)]
        protected virtual async Task Update(DropdownListEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _dropdownListRepository.GetAsync(input.Id);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _dropdownListManager.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除下拉组件信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(DropdownListPermissions.Delete)]
        public async Task Delete(EntityDto<string> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _dropdownListManager.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除DropdownList的方法
        /// </summary>
        [AbpAuthorize(DropdownListPermissions.BatchDelete)]
        public async Task BatchDelete(List<string> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _dropdownListManager.BatchDelete(input);
        }



        public async Task<List<DropdownListListDto>> GetByDDTypeId(string dDTypeId)
        {
            var entity = await _dropdownListRepository.GetAll().AsNoTracking().Where(x=>x.DDType_Id == dDTypeId).ToListAsync();

            var dto = ObjectMapper.Map<List<DropdownListListDto>>(entity);
            return dto;
        }
    }
}


