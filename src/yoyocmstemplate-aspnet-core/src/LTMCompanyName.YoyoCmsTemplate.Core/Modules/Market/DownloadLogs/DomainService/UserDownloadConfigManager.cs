using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService
{
    public class UserDownloadConfigManager : AbpDomainService<UserDownloadConfig>, IUserDownloadConfigManager
    {
        public UserDownloadConfigManager(IRepository<UserDownloadConfig, long> entityRepo)
            : base(entityRepo)
        {
        }

        public async Task AddResidueDegreeByUserId(long? userId, long num)
        {
            var entity = await this.QueryAsNoTracking
                .Where(o => o.UserId == userId.Value)
                             .FirstOrDefaultAsync();

            entity.ResidueDegree += num;
            await this.Update(entity);
        }
        /// <summary>
        /// 根据用户id获取用户下载可用次数信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDownloadConfig> GetDownloadConfigByUserId(long? userId)
        {
            try
            {
                return await this.QueryAsNoTracking.Where(o => o.UserId == userId.Value)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task SetDownloadTypeByUserId(long? userId, DownloadTypeEnum downloadType)
        {
            var entity = await this.QueryAsNoTracking.Where(o => o.UserId == userId.Value)
                           .FirstOrDefaultAsync();

            entity.DownloadType = downloadType.ToString();
            await this.Update(entity);
        }

        public async Task ResetDownloadConfigByUserId(long? userId)
        {
            var entity = await this.QueryAsNoTracking.Where(o => o.UserId == userId.Value)
                          .FirstOrDefaultAsync();

            entity.Reset();
            await this.Update(entity);
        }

        public async Task CreateOrUpdate(UserDownloadConfig entity)
        {
            if (entity.UserId <= 0 || entity.UserName.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException("用户信息不能为空");
            }

            // 新增
            if (entity.Id <= 0)
            {
                var existEntity = await this.QueryAsNoTracking
                    .FirstOrDefaultAsync(o => o.UserId == entity.UserId || o.UserName == entity.UserName);

                if (existEntity == null)
                {
                    await this.Create(entity);
                    return;
                }

                // 如果已存在，那么赋值实体Id,然后往下走直接更新
                entity.Id = existEntity.Id;
            }

            // 更新
            await this.Update(entity);
        }

        /// <summary>
        /// 设置用户的下载次数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="productType"></param>
        /// <param name="productCode"></param>
        /// <param name="productCreateProjectCount"></param>
        /// <param name="productIndate"></param>
        /// <returns></returns>
        public async Task SetUserDownloadConfig(User user, string productType, string productCode, int productCreateProjectCount, double productIndate)
        {
            // 用户原有配置获取
            var userDownloadConfig = await this.GetDownloadConfigByUserId(user.Id);
            if (userDownloadConfig == null)
            {
                userDownloadConfig = new UserDownloadConfig()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
            }

            // 如果不能已经失效，那么重置开始时间
            if (!userDownloadConfig.IsEfficient())
            {
                userDownloadConfig.StartTime = DateTime.Now;
            }

            // 下载类型校验绑定
            var downloadType = productType;
            if (!userDownloadConfig.DownloadType.IsNullOrWhiteSpace())
            {
                // 存在旧的下载配置,先把旧的给自己
                downloadType = userDownloadConfig.DownloadType;
                var oldDownloadType = (int)userDownloadConfig.DownloadType.ToEnum<DownloadTypeEnum>();
                var newDownloadType = (int)productType.ToEnum<DownloadTypeEnum>();
                // 如果新的等级比旧等级高,那么替换
                // 注意：这里的等级比较是通过枚举转换成数字的特性完成
                if (newDownloadType > oldDownloadType)
                {
                    downloadType = productType;
                }
            }

            // 更新用户下载配置信息
            userDownloadConfig.ProductCode = productCode;
            userDownloadConfig.DownloadType = downloadType;
            userDownloadConfig.ResidueDegree += productCreateProjectCount;
            //userDownloadConfig.Indate += productIndate;


            // 更新下载配置信息
            await this.CreateOrUpdate(userDownloadConfig);
        }
    }
}
