using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService
{
    public interface IUserDownloadConfigManager : I52AbpDomainService<UserDownloadConfig>
    {
        /// <summary>
        /// 根据用户id获取下载配置
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDownloadConfig> GetDownloadConfigByUserId(long? userId);


        /// <summary>
        /// 设置用户的下载类型
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="downloadType">下载类型</param>
        /// <returns></returns>
        Task SetDownloadTypeByUserId(long? userId, DownloadTypeEnum downloadType);

        /// <summary>
        /// 添加用户下载次数
        /// (下载次数传入负数则减少下载次数)
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="num">下载次数</param>
        /// <returns></returns>
        Task AddResidueDegreeByUserId(long? userId, long num);

        /// <summary>
        /// 重置下载配置
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task ResetDownloadConfigByUserId(long? userId);

        /// <summary>
        /// 创建或更新用户下载配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateOrUpdate(UserDownloadConfig entity);

        /// <summary>
        /// 设置用户的下载配置信息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="productType">产品类型</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="productCreateProjectCount">项目创建数量</param>
        /// <param name="productIndate">有效期</param>
        /// <returns></returns>
        Task SetUserDownloadConfig(User user, string productType, string productCode, int productCreateProjectCount, double productIndate);
    }
}
