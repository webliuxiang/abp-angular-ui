using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.SmsMessage
{
    public interface ISmsMessage
    {
        Task SendMessage(string phoneNumber, string templateCode, IDictionary<string, string> smsParams);
    }
}
