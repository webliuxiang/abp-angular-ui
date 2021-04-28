using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.Net.Mail;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage
{
    public class MailManager : YoyoCmsTemplateDomainServiceBase, IMailManager
    {
        private readonly IEmailSender _emailSender;

        public MailManager(
            IEmailSender emailSender
            )
        {
            _emailSender = emailSender;
        }

        public async Task SendMessage(string toMailAddress, string title, string body)
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            var defaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var defaultFromDisplayName = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName);
            //var smtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            //var smtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
            //var smtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            //var smtpPasswordDecrypt = SimpleStringCipher.Instance.Decrypt(smtpPassword);
            //var smtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain);
            //var smtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
            //var smtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);


            var mailMessage = new MailMessage(defaultFromAddress, toMailAddress)
            {
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                From = new MailAddress(defaultFromAddress, defaultFromDisplayName),
                Subject = title,
                Body = body
            };


            await _emailSender.SendAsync(mailMessage);
        }
    }
}
