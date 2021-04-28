using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Models.Account
{
    public class ForgotPasswordViewModel
    {

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
