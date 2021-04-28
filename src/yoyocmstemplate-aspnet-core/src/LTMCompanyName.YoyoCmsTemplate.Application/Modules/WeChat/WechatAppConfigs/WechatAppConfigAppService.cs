using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs
{
    /// <summary>
    /// WechatAppConfig应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class WechatAppConfigAppService : YoyoCmsTemplateAppServiceBase, IWechatAppConfigAppService
    {
        /// <summary>
        /// 微信应用类型
        /// </summary>
        public static List<KeyValuePair<string, int>> WechatAppTypeKVList;


        private readonly IWechatAppConfigManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public WechatAppConfigAppService(
            IWechatAppConfigManager entityManager
        )
        {
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取WechatAppConfig的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WechatAppConfigPermissions.Query)]
        public async Task<PagedResultDto<WechatAppConfigListDto>> GetPaged(GetWechatAppConfigsInput input)
        {
            var query = await _entityManager.Query(input.FilterText);

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<WechatAppConfigListDto>>(entityList);
            foreach (var item in entityListDtos)
            {
                item.AppTypeStr = WechatAppTypeKVList.Find(o => o.Value == (int)item.AppType).Key;
                item.Registered = _entityManager.CheckRegister(item.AppId);
            }

            return new PagedResultDto<WechatAppConfigListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取WechatAppConfigListDto信息
        /// </summary>
        [AbpAuthorize(WechatAppConfigPermissions.Query)]
        public async Task<WechatAppConfigListDto> GetById(EntityDto<int> input)
        {
            var entity = await _entityManager.GetById(input.Id);


            return ObjectMapper.Map<WechatAppConfigListDto>(entity);


            //	return entity.MapTo<WechatAppConfigListDto>();
        }

        /// <summary>
        /// 获取编辑 WechatAppConfig
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WechatAppConfigPermissions.Create, WechatAppConfigPermissions.Edit)]
        public async Task<GetWechatAppConfigForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            var output = new GetWechatAppConfigForEditOutput()
            {
                WechatAppTypeList = WechatAppTypeKVList
            };

            WechatAppConfigEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityManager.GetById(input.Id.Value);
                editDto = ObjectMapper.Map<WechatAppConfigEditDto>(entity);

                //wechatAppConfigEditDto = ObjectMapper.Map<List<WechatAppConfigEditDto>>(entity);
            }
            else
            {
                editDto = new WechatAppConfigEditDto();
            }

            output.WechatAppConfig = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改WechatAppConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WechatAppConfigPermissions.Create, WechatAppConfigPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateWechatAppConfigInput input)
        {
            if (input.WechatAppConfig.Id.HasValue)
            {
                await Update(input.WechatAppConfig);
            }
            else
            {
                await Create(input.WechatAppConfig);
            }
        }


        /// <summary>
        /// 新增WechatAppConfig
        /// </summary>
        [AbpAuthorize(WechatAppConfigPermissions.Create)]
        protected virtual async Task<WechatAppConfigEditDto> Create(WechatAppConfigEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            // var entity = ObjectMapper.Map <WechatAppConfig>(input);

            var entity = ObjectMapper.Map<WechatAppConfig>(input);
            await _entityManager.Create(entity);
            return ObjectMapper.Map<WechatAppConfigEditDto>(entity);
        }

        /// <summary>
        /// 编辑WechatAppConfig
        /// </summary>
        [AbpAuthorize(WechatAppConfigPermissions.Edit)]
        protected virtual async Task Update(WechatAppConfigEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityManager.GetById(input.Id.Value);

            // 记录旧的
            var oldCreateUserId = entity.CreatorUserId.Value;
            var oldTenantId = entity.TenantId;

            ObjectMapper.Map(input, entity);
            //input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityManager.Update(entity, oldCreateUserId, oldTenantId);
        }



        /// <summary>
        /// 删除WechatAppConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WechatAppConfigPermissions.Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityManager.Delete(input.Id);
        }



        /// <summary>
        /// 批量删除WechatAppConfig的方法
        /// </summary>
        [AbpAuthorize(WechatAppConfigPermissions.BatchDelete)]
        public async Task BatchDelete(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityManager.BatchDelete(input);
        }

        /// <summary>
        /// 将wechat app注入到容器,如果已注入则刷新注入
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task RegisterWechatApp(string appId)
        {
            await _entityManager.RegisterWechatApp(appId, true);
        }

        ///// <summary>
        ///// 导出WechatAppConfig为excel表,等待开发。
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


