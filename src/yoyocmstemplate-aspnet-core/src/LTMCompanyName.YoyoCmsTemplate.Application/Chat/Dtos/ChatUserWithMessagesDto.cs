using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Chat.Dtos
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}
