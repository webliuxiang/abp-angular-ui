using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Notifications.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications
{
    /// <summary>
    ///     消息通知系统服务
    /// </summary>
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        ///     获取用户通知信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetNotificationsOutput> GetPagedUserNotificationsAsync(GetUserNotificationsInput input);

        /// <summary>
        ///     设置所有通知为已阅读状态
        /// </summary>
        /// <returns></returns>
        Task MakeAllUserNotificationsAsRead();

        /// <summary>
        ///     标记某条通知为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MakeNotificationAsRead(EntityDto<Guid> input);

        /// <summary>
        ///     获取消息设置
        /// </summary>
        /// <returns></returns>
        Task<GetNotificationSettingsOutput> GetNotificationSettings();

        /// <summary>
        ///     更新消息设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);


        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteNotification(EntityDto<Guid> input);

#if DEBUG

        /// <summary>
        /// 测试模式发送通知
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task SendNoticeToUser(string msg, int? tenantId, long userId);
#endif
    }
}