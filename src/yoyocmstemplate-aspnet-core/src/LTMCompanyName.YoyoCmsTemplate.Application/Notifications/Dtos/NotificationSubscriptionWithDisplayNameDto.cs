using Abp.AutoMapper;
using Abp.Notifications;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.Dtos
{
    [AutoMapFrom(typeof(NotificationDefinition))]
    public class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}