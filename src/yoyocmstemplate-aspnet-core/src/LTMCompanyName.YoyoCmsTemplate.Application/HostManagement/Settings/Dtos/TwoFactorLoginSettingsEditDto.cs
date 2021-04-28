namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class TwoFactorLoginSettingsEditDto
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabledForApplication { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 邮箱启用
        /// </summary>
        public bool IsEmailProviderEnabled { get; set; }

        /// <summary>
        /// 短信启用
        /// </summary>
        public bool IsSmsProviderEnabled { get; set; }

        /// <summary>
        /// 浏览器‘记住我’启用
        /// </summary>
        public bool IsRememberBrowserEnabled { get; set; }

        /// <summary>
        /// google校验启用
        /// </summary>
        public bool IsGoogleAuthenticatorEnabled { get; set; }
    }
}