using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Message
{
    public class MessageHistory : FullAuditedEntity
    {
        public string Message { get; set; }

        public string Code { get; set; }

        public MessageType Type { get; set; }

        public string To { get; set; }
    }
    public enum MessageType
    {
        Sms = 1,
        Mail =2
    }
}
