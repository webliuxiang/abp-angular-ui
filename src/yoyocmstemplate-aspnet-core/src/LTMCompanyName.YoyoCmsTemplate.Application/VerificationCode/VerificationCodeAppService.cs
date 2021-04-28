using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LTMCompanyName.YoyoCmsTemplate.Message;
using LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage;
using LTMCompanyName.YoyoCmsTemplate.Notifications.SmsMessage;

namespace LTMCompanyName.YoyoCmsTemplate.VerificationCode
{
    public class VerificationCodeAppService : YoyoCmsTemplateAppServiceBase, IVerificationCodeAppService
    {
        private readonly IMailManager _emailSender;
        private readonly ISmsMessage _smsMessage;
        private readonly IRepository<MessageHistory> _messageHistoryRepository;

        public VerificationCodeAppService(IMailManager emailSender, IRepository<MessageHistory> messageHistoryRepository, ISmsMessage smsMessage)
        {
            _emailSender = emailSender;
            LocalizationSourceName = AppConsts.LocalizationSourceName;
            _messageHistoryRepository = messageHistoryRepository;
            _smsMessage = smsMessage;
        }

        public async Task SendMailVerificationCode(string mailAddress)
        {
            var code = VerificationCodeGenerater.GenerateCode();
            Expression<Func<Task>> expression = () => _emailSender.SendMessage(mailAddress, L("VerificationSubject"), string.Format(L("VerificationBody"), code));
            Hangfire.BackgroundJob.Schedule(expression, TimeSpan.Zero);
            await _messageHistoryRepository.InsertAsync(new MessageHistory { Code = code, Type = MessageType.Mail, Message = string.Format(L("VerificationBody"), code),To = mailAddress });
        }

        public async Task SendSmsVerificationCode(string phoneNumber)
        {
            var code = VerificationCodeGenerater.GenerateCode();
            var smsParams = new Dictionary<string, string>() { { "code", code } };
            Expression<Func<Task>> expression = () => _smsMessage.SendMessage(phoneNumber, "", smsParams);
            Hangfire.BackgroundJob.Schedule(expression, TimeSpan.Zero);
            await _messageHistoryRepository.InsertAsync(new MessageHistory { Code = code, Type = MessageType.Sms, Message = code, To = phoneNumber });
        }

        public Task<bool> Verify(string code)
        {
            return new ValueTask<bool>(VerificationCodeGenerater.Verify(code)).AsTask();
        }

    }
}
