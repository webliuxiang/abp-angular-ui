using System;
using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using L._52ABP.Common.Consts;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.AppMessage
{
    public class AppMessage : YoyoCmsTemplateDomainServiceBase, IAppMessage
    {

        private readonly INotificationPublisher _notificationPublisher;

        public AppMessage(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToApplicationAsync(User user)
        {

            await _notificationPublisher.PublishAsync(
                AbpProConsts.AppMessage.WelcomeToCms,
                new MessageNotificationData(L("WelcomeToApplication")), severity: NotificationSeverity.Success, userIds: new[] { user.ToUserIdentifier() }



                );

        }

        public async Task SendMessageAsync(UserIdentifier user, string messager, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                AbpProConsts.AppMessage.SendMessageAsync,
                new MessageNotificationData(messager), severity: severity, userIds: new[] { user });
        }



        public Task NewUserRegisteredAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task NewTenantRegisteredAsync(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId)
        {
            throw new NotImplementedException();
        }

        public Task TenantsMovedToEdition(UserIdentifier argsUser, string sourceEditionName, string targetEditionName)
        {
            throw new NotImplementedException();
        }

        public Task SomeUsersCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}