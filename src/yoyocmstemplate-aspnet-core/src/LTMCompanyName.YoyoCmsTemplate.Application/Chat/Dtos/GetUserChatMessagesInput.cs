using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Chat.Dtos
{
    public class GetUserChatMessagesInput
    {
        public int? TenantId { get; set; }

        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        public long? MinMessageId { get; set; }
    }
}
