using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class SecuritySettingsEditDto
    {
        /// <summary>
        /// 使用默认密码校验设置
        /// </summary>
        public bool UseDefaultPasswordComplexitySettings { get; set; }

        /// <summary>
        /// 密码校验规则
        /// </summary>
        public PasswordComplexitySetting PasswordComplexity { get; set; }

        /// <summary>
        /// 默认密码校验规则
        /// </summary>
        public PasswordComplexitySetting DefaultPasswordComplexity { get; set; }

        /// <summary>
        /// 用户锁定设置
        /// </summary>
        public UserLockOutSettingsEditDto UserLockOut { get; set; }

        /// <summary>
        /// 双重校验登陆设置
        /// </summary>
        public TwoFactorLoginSettingsEditDto TwoFactorLogin { get; set; }
    }
}