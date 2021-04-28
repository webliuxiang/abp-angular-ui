using System.Collections.Generic;
using L._52ABP.Core.VerificationCodeStore;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{

    /// <summary>
    /// 网站全局管理设置
    /// </summary>
    public class HostUserManagementSettingsEditDto
    {
        /// <summary>
        /// 是否必须验证邮箱才能登陆
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        /// <summary>
        /// 是否启用短信验证
        /// </summary>
        public bool SmsVerificationEnabled { get; set; }
        /// <summary>
        /// 是否启用Cookie内容
        /// </summary>
        public bool IsCookieConsentEnabled { get; set; }
        /// <summary>
        /// 宿主用户登陆使用验证码
        /// </summary>
        public bool UseCaptchaOnUserLogin { get; set; }

        /// <summary>
        /// 宿主用户登陆验证码类型
        /// </summary>
        public ValidateCodeType CaptchaOnUserLoginType { get; set; }

        /// <summary>
        /// 宿主用户登陆验证码长度
        /// </summary>
        public int CaptchaOnUserLoginLength { get; set; }


        public bool IsQuickThemeSelectEnabled { get; set; }
        public List<string> ExternalLoginProviders { get; set; }
    }
}