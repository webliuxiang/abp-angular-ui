using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using L._52ABP.Common.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs
{
    /// <summary>
    /// UserDownloadConfig应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class UserDownloadConfigAppService : YoyoCmsTemplateAppServiceBase, IUserDownloadConfigAppService
    {

        private readonly IUserDownloadConfigManager _entityManager;

        private readonly IProductManager _productManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public UserDownloadConfigAppService(
            IUserDownloadConfigManager entityManager,
            IProductManager productManager
        )
        {
            _entityManager = entityManager;
            _productManager = productManager;
        }


        /// <summary>
        /// 获取UserDownloadConfig的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(UserDownloadConfigPermissions.Query)]
        public async Task<PagedResultDto<UserDownloadConfigListDto>> GetPaged(GetUserDownloadConfigsInput input)
        {

            var query = _entityManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), o => o.UserName.Contains(input.FilterText));
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting)
                    .AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<UserDownloadConfigListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<UserDownloadConfigListDto>>();

            return new PagedResultDto<UserDownloadConfigListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取UserDownloadConfigListDto信息
        /// </summary>
        [AbpAuthorize(UserDownloadConfigPermissions.Query)]
        public async Task<UserDownloadConfigListDto> GetById(EntityDto<long> input)
        {
            var entity = await _entityManager.FindById(input.Id);

            return ObjectMapper.Map<UserDownloadConfigListDto>(entity);

            //return entity.MapTo<UserDownloadConfigListDto>();
        }

        /// <summary>
        /// 获取编辑 UserDownloadConfig
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(UserDownloadConfigPermissions.Create, UserDownloadConfigPermissions.Edit)]
        public async Task<GetUserDownloadConfigForEditOutput> GetForEdit(NullableIdDto<long> input)
        {
            var output = new GetUserDownloadConfigForEditOutput();
            UserDownloadConfigEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityManager.FindById(input.Id.Value);

                editDto = ObjectMapper.Map<UserDownloadConfigEditDto>(entity);
                //editDto = entity.MapTo<UserDownloadConfigEditDto>();

                //userDownloadConfigEditDto = ObjectMapper.Map<List<userDownloadConfigEditDto>>(entity);
            }
            else
            {
                editDto = new UserDownloadConfigEditDto();
            }

            output.UserDownloadConfig = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改UserDownloadConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(UserDownloadConfigPermissions.Create, UserDownloadConfigPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateUserDownloadConfigInput input)
        {

            if (input.UserDownloadConfig.Id.HasValue)
            {
                await Update(input.UserDownloadConfig);
            }
            else
            {
                await Create(input.UserDownloadConfig);
            }
        }


        /// <summary>
        /// 新增UserDownloadConfig
        /// </summary>
        [AbpAuthorize(UserDownloadConfigPermissions.Create)]
        protected virtual async Task<UserDownloadConfigEditDto> Create(UserDownloadConfigEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<UserDownloadConfig>(input);
            //var entity = input.MapTo<UserDownloadConfig>();


            var product = await _productManager.GetProductByCode(input.ProductCode);
            entity.ProductCode = product.Code;
            entity.DownloadType = product.Type;

            await _entityManager.Create(entity);

            return ObjectMapper.Map<UserDownloadConfigEditDto>(entity);
            //return entity.MapTo<UserDownloadConfigEditDto>();
        }

        /// <summary>
        /// 编辑UserDownloadConfig
        /// </summary>
        [AbpAuthorize(UserDownloadConfigPermissions.Edit)]
        protected virtual async Task Update(UserDownloadConfigEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityManager.FindById(input.Id.Value);
            //input.MapTo(entity);

            ObjectMapper.Map(input, entity);

            var product = await _productManager.GetProductByCode(input.ProductCode);
            entity.ProductCode = product.Code;
            entity.DownloadType = product.Type;

            // ObjectMapper.Map(input, entity);
            await _entityManager.Update(entity);
        }



        /// <summary>
        /// 删除UserDownloadConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(UserDownloadConfigPermissions.Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityManager.Delete(input.Id);
        }



        /// <summary>
        /// 批量删除UserDownloadConfig的方法
        /// </summary>
        [AbpAuthorize(UserDownloadConfigPermissions.BatchDelete)]
        public async Task BatchDelete(List<long> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityManager.Delete(s => input.Contains(s.Id));
        }


        ///// <summary>
        ///// 导出UserDownloadConfig为excel表,等待开发。
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


