using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Notifications;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.Notifications.AppMessage;
using LTMCompanyName.YoyoCmsTemplate.Notifications.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications
{
    [AbpAuthorize]
    public class NotificationAppService : YoyoCmsTemplateAppServiceBase, INotificationAppService
    {
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly IAppMessage _notificationManager;

        public NotificationAppService(
            INotificationDefinitionManager notificationDefinitionManager,
            IUserNotificationManager userNotificationManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppMessage notificationManager)
        {
            _notificationDefinitionManager = notificationDefinitionManager;
            _userNotificationManager = userNotificationManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _notificationManager = notificationManager;
        }

        [DisableAuditing]
        public async Task<GetNotificationsOutput> GetPagedUserNotificationsAsync(GetUserNotificationsInput input)
        {
            var totalCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), input.State
            );

            var unreadCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread
            );

            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                AbpSession.ToUserIdentifier(), input.State, input.SkipCount, input.MaxResultCount
            );

            return new GetNotificationsOutput(totalCount, unreadCount, notifications);
        }

        public async Task MakeAllUserNotificationsAsRead()
        {
            await _userNotificationManager.UpdateAllUserNotificationStatesAsync(AbpSession.ToUserIdentifier(),
                UserNotificationState.Read);
        }

        public async Task MakeNotificationAsRead(EntityDto<Guid> input)
        {
            var userNotification =
                await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);
            if (userNotification.UserId != AbpSession.GetUserId())
                throw new ApplicationException($"消息Id为{input.Id}的信息，不属于当前的用户，用户id：{AbpSession.UserId}");

            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, input.Id,
                UserNotificationState.Read);
        }

        public async Task<GetNotificationSettingsOutput> GetNotificationSettings()
        {
            var notifications = (await _notificationDefinitionManager
                    .GetAllAvailableAsync(AbpSession.ToUserIdentifier()))
                .Where(nd => nd.EntityType == null); //Get general notifications, not entity related notifications.
            var output = new GetNotificationSettingsOutput
            {
                ReceiveNotifications =
                    await SettingManager.GetSettingValueAsync<bool>(NotificationSettingNames.ReceiveNotifications),
                Notifications = ObjectMapper.Map<List<NotificationSubscriptionWithDisplayNameDto>>(notifications)
            };


            var subscribedNotifications = (await _notificationSubscriptionManager
                    .GetSubscribedNotificationsAsync(AbpSession.ToUserIdentifier()))
                .Select(ns => ns.NotificationName)
                .ToList();

            output.Notifications.ForEach(n => n.IsSubscribed = subscribedNotifications.Contains(n.Name));

            return output;
        }

        /// <summary>
        ///     更新消息设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                NotificationSettingNames.ReceiveNotifications, input.ReceiveNotifications.ToString());

            foreach (var notification in input.Notifications)
                if (notification.IsSubscribed)
                    await _notificationSubscriptionManager.SubscribeAsync(AbpSession.ToUserIdentifier(),
                        notification.Name);
                else
                    await _notificationSubscriptionManager.UnsubscribeAsync(AbpSession.ToUserIdentifier(),
                        notification.Name);
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteNotification(EntityDto<Guid> input)
        {
            var notification = await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);
            if (notification.UserId != AbpSession.GetUserId())
            {
                throw new Abp.UI.UserFriendlyException(L("ThisNotificationDoesntBelongToYou"));
            }

            await _userNotificationManager.DeleteUserNotificationAsync(AbpSession.TenantId, input.Id);
        }

#if DEBUG


        public async Task SendNoticeToUser(string msg, int? tenantId, long userId)
        {
            await _notificationManager.SendMessageAsync(new Abp.UserIdentifier(tenantId, userId), msg, Abp.Notifications.NotificationSeverity.Info);
        }
#endif
    }
}