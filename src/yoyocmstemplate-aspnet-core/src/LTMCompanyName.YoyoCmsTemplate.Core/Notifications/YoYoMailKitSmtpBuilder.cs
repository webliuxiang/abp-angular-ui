using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications
{
    public class YoYoMailKitSmtpBuilder : DefaultMailKitSmtpBuilder
    {
        private readonly ISmtpEmailSenderConfiguration _smtpEmailSenderConfiguration;
        public YoYoMailKitSmtpBuilder(
            ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration,
            IAbpMailKitConfiguration abpMailKitConfiguration
        )
            : base(smtpEmailSenderConfiguration, abpMailKitConfiguration)
        {
            _smtpEmailSenderConfiguration = smtpEmailSenderConfiguration;
        }


        protected override void ConfigureClient(MailKit.Net.Smtp.SmtpClient client)
        {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            client.Connect(
                _smtpEmailSenderConfiguration.Host,
                _smtpEmailSenderConfiguration.Port,
                GetSecureSocketOption()
            );





            if (_smtpEmailSenderConfiguration.UseDefaultCredentials)
            {
                return;
            }

            client.Authenticate(
                _smtpEmailSenderConfiguration.UserName,
                SimpleStringCipher.Instance.Decrypt(_smtpEmailSenderConfiguration.Password)
            );
        }

    }
}
