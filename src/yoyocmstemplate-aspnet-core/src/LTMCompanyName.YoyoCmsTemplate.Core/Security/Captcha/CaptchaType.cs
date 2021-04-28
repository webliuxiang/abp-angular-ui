using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Security.Captcha
{
    /// <summary>
    /// 图形验证码类型
    /// </summary>
    public enum CaptchaType
    {
        /// <summary>
        /// 默认,非任何类型
        /// </summary>
        [Description("默认")]
        Defulat = 0,
        /// <summary>
        /// 宿主租户注册
        /// </summary>
        [Description("宿主租户注册")]
        HostTenantRegister = 1,
        /// <summary>
        /// 宿主用户登陆
        /// </summary>
        [Description("宿主用户登陆")]
        HostUserLogin = 2,

        /// <summary>
        /// 租户用户注册
        /// </summary>
        [Description("租户用户注册")]
        TenantUserRegister = 3,
        /// <summary>
        /// 租户用户登陆
        /// </summary>
        [Description("租户用户登陆")]
        TenantUserLogin = 4,

        /// <summary>
        /// 租户用户注册验证邮箱
        /// </summary>
        [Description("租户用户注册验证邮箱")]
        TenantUserRegisterActiveEmail = 5,
        /// <summary>
        /// 租户用户忘记密码
        /// </summary>
        [Description("租户用户忘记密码")]
        TenantUserForotPassword = 6,
        /// <summary>
        /// 租户用户重置密码
        /// </summary>
        [Description("租户用户重置密码")]
        TenantUserResetPassword = 7,











    }
}
