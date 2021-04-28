using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members
{
    /// <summary>
    /// Member应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class MemberAppService : YoyoCmsTemplateAppServiceBase, IMemberAppService
    {
        private readonly IRepository<Member, long> _entityRepository;

        private readonly IMemberManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public MemberAppService(
        IRepository<Member, long> entityRepository
        , IMemberManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Member的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(MemberPermissions.Query)]
        public async Task<PagedResultDto<MemberListDto>> GetPaged(GetMembersInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<MemberListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<MemberListDto>>();

            return new PagedResultDto<MemberListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取MemberListDto信息
        /// </summary>
        [AbpAuthorize(MemberPermissions.Query)]
        public async Task<MemberListDto> GetById(EntityDto<long> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return ObjectMapper.Map<MemberListDto>(entity);

            //return entity.MapTo<MemberListDto>();
        }

        /// <summary>
        /// 获取编辑 Member
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(MemberPermissions.Create, MemberPermissions.Edit)]
        public async Task<GetMemberForEditOutput> GetForEdit(NullableIdDto<long> input)
        {
            var output = new GetMemberForEditOutput();
            MemberEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                //editDto = entity.MapTo<MemberEditDto>();

                editDto = ObjectMapper.Map<MemberEditDto>(entity);

                //memberEditDto = ObjectMapper.Map<List<memberEditDto>>(entity);
            }
            else
            {
                editDto = new MemberEditDto();
            }

            output.Member = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Member的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(MemberPermissions.Create, MemberPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateMemberInput input)
        {

            if (input.Member.Id.HasValue)
            {
                await Update(input.Member);
            }
            else
            {
                await Create(input.Member);
            }
        }


        /// <summary>
        /// 新增Member
        /// </summary>
        [AbpAuthorize(MemberPermissions.Create)]
        protected virtual async Task<MemberEditDto> Create(MemberEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Member>(input);
            //var entity = input.MapTo<Member>();


            entity = await _entityRepository.InsertAsync(entity);


            return ObjectMapper.Map<MemberEditDto>(entity);
            //return entity.MapTo<MemberEditDto>();
        }

        /// <summary>
        /// 编辑Member
        /// </summary>
        [AbpAuthorize(MemberPermissions.Edit)]
        protected virtual async Task Update(MemberEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);

            ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Member信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(MemberPermissions.Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Member的方法
        /// </summary>
        [AbpAuthorize(MemberPermissions.BatchDelete)]
        public async Task BatchDelete(List<long> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        ///// <summary>
        ///// 导出Member为excel表,等待开发。
        ///// </summary>
        ///// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

    }
}


