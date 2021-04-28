using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        public bool RememberClient { get; set; }

        public string ReturnUrl { get; set; }

    }
}
