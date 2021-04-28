using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using YoYo;
using YoYo.Containers;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.DomainService
{
    /// <summary>
    /// WechatAppConfig领域层的业务管理
    ///</summary>
    public class WechatAppConfigManager : YoyoCmsTemplateDomainServiceBase, IWechatAppConfigManager
    {

        private readonly IRepository<WechatAppConfig, int> _repository;

        /// <summary>
        /// WechatAppConfig的构造方法
        ///</summary>
        public WechatAppConfigManager(
            IRepository<WechatAppConfig, int> repository
        )
        {
            _repository = repository;
        }



        /// <summary>
        /// 初始化
        ///</summary>
        public void InitWechatAppConfig()
        {
            throw new NotImplementedException();
        }


        public async Task<WechatAppConfig> GetById(int id, bool equalsUserIdAndTenantId = true)
        {
            var entity = await _repository.GetAll()
                     .WhereIf(equalsUserIdAndTenantId, o =>
                        o.CreatorUserId.Value == AbpSession.UserId.Value
                        && o.TenantId == AbpSession.TenantId)
                     .Where(o => o.Id == id)
                     .AsNoTracking()
                     .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new UserFriendlyException(base.L("GetWechatAppForEditErrorMsg"));
            }

            return entity;
        }

        public async Task<WechatAppConfig> GetByAppId(string appId, bool equalsUserIdAndTenantId = true)
        {
            var entity = await _repository.GetAll()
                    .WhereIf(equalsUserIdAndTenantId, o =>
                       o.CreatorUserId.Value == AbpSession.UserId.Value
                       && o.TenantId == AbpSession.TenantId)
                    .Where(o => o.AppId == appId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new UserFriendlyException(base.L("GetWechatAppForEditErrorMsg"));
            }

            return entity;
        }


        public async Task Update(WechatAppConfig entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task Update(WechatAppConfig entity, long userId, int? tenantId)
        {
            entity.CreatorUserId = userId;
            entity.TenantId = tenantId;
            await Update(entity);
        }


        public async Task Create(WechatAppConfig entity)
        {
            entity.CreatorUserId = AbpSession.UserId.Value;
            entity.TenantId = AbpSession.TenantId;
            await _repository.InsertAsync(entity);
        }

        public async Task Delete(int entityId, bool equalsUserIdAndTenantId = true)
        {
            if (equalsUserIdAndTenantId)
            {
                await _repository.DeleteAsync(o =>
                        o.CreatorUserId.Value == AbpSession.UserId
                        && o.TenantId == AbpSession.TenantId
                        && entityId == o.Id);
            }
            else
            {
                await _repository.DeleteAsync(o => entityId == o.Id);
            }
        }

        public async Task BatchDelete(List<int> entityIdList, bool equalsUserIdAndTenantId = true)
        {
            if (equalsUserIdAndTenantId)
            {
                await _repository.DeleteAsync(o =>
                        o.CreatorUserId.Value == AbpSession.UserId
                        && o.TenantId == AbpSession.TenantId
                        && entityIdList.Contains(o.Id));
            }
            else
            {
                await _repository.DeleteAsync(o => entityIdList.Contains(o.Id));
            }
        }

        public async Task<IQueryable<WechatAppConfig>> Query(string filterText, bool equalsUserIdAndTenantId = true)
        {
            await Task.Yield();

            return _repository.GetAll()
                    .WhereIf(equalsUserIdAndTenantId, o =>
                        o.CreatorUserId.Value == AbpSession.UserId.Value
                        && o.TenantId.Value == AbpSession.TenantId)
                    .WhereIf(!filterText.IsNullOrWhiteSpace(), o => o.Name.Contains(filterText))
                    .AsNoTracking();

        }



        public async Task RegisterWechatApp(string appId, bool haveBeenChanged = false)
        {
            var cacheKey = GetCatcheKey(appId);

            if (!haveBeenChanged && MpInfoContainer<long, int?>.CheckRegistered(cacheKey))
            {
                return;
            }


            // 获取配置
            var wechatAppConfig = await this.GetByAppId(appId);


            // 注入到缓存管理器
            YoYoSenparcWXMP.RegisterMpAccount<long, int?>(
                    cacheKey,
                    wechatAppConfig.AppId,
                    wechatAppConfig.AppSecret,
                    wechatAppConfig.Token,
                    wechatAppConfig.EncodingAESKey,
                    wechatAppConfig.Name,
                    AbpSession.UserId.Value,
                    AbpSession.TenantId
                );
        }


        public bool CheckRegister(string appId)
        {
            var cacheKey = GetCatcheKey(appId);

            return MpInfoContainer<long, int?>.CheckRegistered(cacheKey);
        }


        #region 私有函数

        /// <summary>
        /// 获取缓存Key
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        private string GetCatcheKey(string appId)
        {
            return $"{(AbpSession.TenantId.HasValue ? AbpSession.TenantId.Value : 0)}{AbpSession.UserId.Value}{appId}";
        }

        #endregion
    }
}
