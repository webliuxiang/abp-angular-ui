using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    public class ResetPasswordOutput
    {
        public bool CanLogin { get; set; }

        public string EmailAddress { get; set; }
    }


    /// <summary>
    /// 重置密码的Dto
    /// </summary>
    public class ResetPasswordInput
    {



        public string ReturnUrl { get; set; }

        public string SingleSignIn { get; set; }



        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 重置密码的code
        /// </summary>
        [Required]
        public string ResetCode { get; set; }

        public string Error { get; set; }


    }
}
