namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account
{
    public class LoginFormViewModel
    {


        public string SuccessMessage { get; set; }
        public string UserNameOrEmailAddress { get; set; }
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 是否开启了多租户
        /// </summary>
        public bool IsMultiTenancyEnabled { get; set; }
        /// <summary>
        /// 是否允许用户自行注册
        /// </summary>
        public bool IsSelfRegistrationAllowed { get; set; }

        /// <summary>
        /// 是否允许注册租户
        /// </summary>
        public bool IsTenantSelfRegistrationEnabled { get; set; }


    }
}
