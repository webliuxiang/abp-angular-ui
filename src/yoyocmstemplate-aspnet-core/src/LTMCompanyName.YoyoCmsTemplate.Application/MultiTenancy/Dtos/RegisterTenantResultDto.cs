namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    public class RegisterTenantResultDto
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 租户是否激活
        /// </summary>
        public bool IsActive { get; set; }


        /// <summary>
        /// 租户启用了用户登陆验证码
        /// </summary>
        public bool UseCaptchaOnUserLogin { get; set; }
    }
}
