using System;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Chat;


namespace LTMCompanyName.YoyoCmsTemplate.Chat.Dtos
{
    public class ChatMessageDto : EntityDto
    {
        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }


        public int Side { get; set; }

        public ChatMessageReadState ReadState { get; set; }

        public ChatMessageReadState ReceiverReadState { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public string SharedMessageId { get; set; }
    }
}
