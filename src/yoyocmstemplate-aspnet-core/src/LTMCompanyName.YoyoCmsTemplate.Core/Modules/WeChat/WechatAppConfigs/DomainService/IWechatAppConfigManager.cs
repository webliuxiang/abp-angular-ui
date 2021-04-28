using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.DomainService
{
    public interface IWechatAppConfigManager : ISingletonDependency
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitWechatAppConfig();


        /// <summary>
        /// 根据id查找微信公众号配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equalsUserIdAndTenantId">过滤用户和租户,默认启用</param>
        /// <returns></returns>
        Task<WechatAppConfig> GetById(int id, bool equalsUserIdAndTenantId = true);


        /// <summary>
        /// 根据AppId查找微信公众号配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equalsUserIdAndTenantId">过滤用户和租户,默认启用</param>
        /// <returns></returns>
        Task<WechatAppConfig> GetByAppId(string appId, bool equalsUserIdAndTenantId = true);

        /// <summary>
        /// 更新公众号配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(WechatAppConfig entity);

        /// <summary>
        /// 更新公众号配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId">所属用户id</param>
        /// <param name="tenantId">所属租户id</param>
        /// <returns></returns>
        Task Update(WechatAppConfig entity, long userId, int? tenantId);

        /// <summary>
        /// 创建公众号配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Create(WechatAppConfig entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="equalsUserIdAndTenantId">过滤用户和租户,默认启用</param>
        /// <returns></returns>
        Task Delete(int entityId, bool equalsUserIdAndTenantId = true);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="equalsUserIdAndTenantId">过滤用户和租户,默认启用</param>
        /// <returns></returns>
        Task BatchDelete(List<int> entityIdList, bool equalsUserIdAndTenantId = true);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filterText">筛选</param>
        /// <param name="equalsUserIdAndTenantId">过滤用户和租户,默认启用</param>
        /// <returns></returns>
        Task<IQueryable<WechatAppConfig>> Query(string filterText, bool equalsUserIdAndTenantId = true);


        /// <summary>
        /// 注册微信信息到容器
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="haveBeenChanged">是否存在修改,如果为true,那么强制重新注册,默认值为false</param>
        /// <returns></returns>
        Task RegisterWechatApp(string appId, bool haveBeenChanged = false);

        /// <summary>
        /// 检查是否已注册到应用容器中
        /// </summary>
        /// <param name="appId">appId</param>
        /// <returns></returns>
        bool CheckRegister(string appId);
    }
}
