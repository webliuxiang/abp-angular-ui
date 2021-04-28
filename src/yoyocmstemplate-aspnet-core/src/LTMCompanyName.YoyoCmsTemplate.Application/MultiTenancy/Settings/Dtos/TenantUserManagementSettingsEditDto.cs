using L._52ABP.Core.VerificationCodeStore;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos
{
    public class TenantUserManagementSettingsEditDto
    {
        /// <summary>
        /// 是否启用注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// 是否新注册用户默认激活
        /// </summary>
        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        /// <summary>
        /// 是否必须校验邮箱才能登陆
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        /// <summary>
        /// 是否注册使用验证码
        /// </summary>
        public bool UseCaptchaOnUserRegistration { get; set; }

        /// <summary>
        /// 注册验证码类型
        /// </summary>
        public ValidateCodeType CaptchaOnUserRegistrationType { get; set; }

        /// <summary>
        /// 注册验证码长度
        /// </summary>
        public int CaptchaOnUserRegistrationLength { get; set; }

        /// <summary>
        /// 是否登陆使用验证码
        /// </summary>
        public bool UseCaptchaOnUserLogin { get; set; }


        /// <summary>
        /// 登陆验证码类型
        /// </summary>
        public ValidateCodeType CaptchaOnUserLoginType { get; set; }

        /// <summary>
        /// 登陆验证码长度
        /// </summary>
        public int CaptchaOnUserLoginLength { get; set; }
    }
}
