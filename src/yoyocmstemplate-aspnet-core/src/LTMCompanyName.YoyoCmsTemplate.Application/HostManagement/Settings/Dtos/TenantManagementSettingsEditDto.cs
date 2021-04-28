using L._52ABP.Core.VerificationCodeStore;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class TenantManagementSettingsEditDto
    {
        /// <summary>
        /// 允许注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// 注册租户默认激活
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        /// <summary>
        /// 宿主租户注册使用验证码
        /// </summary>
        public bool UseCaptchaOnTenantRegistration { get; set; }

        /// <summary>
        /// 宿主租户注册验证码类型
        /// </summary>
        public ValidateCodeType CaptchaOnTenantRegistrationType { get; set; }

        /// <summary>
        /// 宿主租户注册验证码长度
        /// </summary>
        public int CaptchaOnTenantRegistrationLength { get; set; }

        /// <summary>
        /// 默认版本id
        /// </summary>
        public int? DefaultEditionId { get; set; }
    }
}