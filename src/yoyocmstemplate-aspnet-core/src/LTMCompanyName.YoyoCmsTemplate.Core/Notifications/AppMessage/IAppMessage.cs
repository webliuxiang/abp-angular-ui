using System;
using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.AppMessage
{
    /// <summary>
    /// 通知管理领域服务
    /// </summary>
    public interface IAppMessage
    {
        /// <summary>
        /// 欢迎使用CMS消息通知
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task WelcomeToApplicationAsync(User user);

        /// <summary>
        /// 发送自定义消息通知
        /// </summary>
        /// <param name="user">用户身份</param>
        /// <param name="messager">消息内容</param>
        /// <param name="severity">消息严重等级</param>
        /// <returns></returns>
        Task SendMessageAsync(UserIdentifier user, string messager,
            NotificationSeverity severity = NotificationSeverity.Info);



        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId);



        Task TenantsMovedToEdition(UserIdentifier argsUser, string sourceEditionName, string targetEditionName);

        Task SomeUsersCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName);
    }
}