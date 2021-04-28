using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }


        public string Code { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public string ReturnUrlHash { get; set; }
    }
}
