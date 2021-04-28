namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class EmailSettingsEditDto
    {
        //没有进行验证，因为可能不想使用邮件系统。

        /// <summary>
        /// 默认发件人邮箱地址
        /// </summary>
        public string DefaultFromAddress { get; set; }

        /// <summary>
        /// 邮箱显示名称
        /// </summary>
        public string DefaultFromDisplayName { get; set; }

        /// <summary>
        /// 发件人邮箱SMTP服务器Host
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// 发件人邮箱SMTP服务器端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// 发件人校验名称
        /// </summary>
        public string SmtpUserName { get; set; }

        /// <summary>
        /// 发件人校验密码
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string SmtpDomain { get; set; }

        /// <summary>
        /// 使用ssl
        /// </summary>
        public bool SmtpEnableSsl { get; set; }

        /// <summary>
        /// 使用默认凭据
        /// </summary>
        public bool SmtpUseDefaultCredentials { get; set; }
    }
}