using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.Dtos
{
    public class UpdateNotificationSettingsInput
    {
        public bool ReceiveNotifications { get; set; }

        public List<NotificationSubscriptionDto> Notifications { get; set; }
    }
}