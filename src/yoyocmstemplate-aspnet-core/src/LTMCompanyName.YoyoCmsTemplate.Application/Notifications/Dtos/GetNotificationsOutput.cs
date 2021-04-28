using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Notifications;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.Dtos
{
    public class GetNotificationsOutput : PagedResultDto<UserNotification>
    {
        public GetNotificationsOutput(int totalCount, int unreadCount, List<UserNotification> notifications)
            : base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }

        /// <summary>
        ///     未阅读消息数量
        /// </summary>
        public int UnreadCount { get; set; }
    }
}