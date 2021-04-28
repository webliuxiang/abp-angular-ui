using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.Dtos
{
    public class GetNotificationSettingsOutput
    {
        public bool ReceiveNotifications { get; set; }

        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }
}