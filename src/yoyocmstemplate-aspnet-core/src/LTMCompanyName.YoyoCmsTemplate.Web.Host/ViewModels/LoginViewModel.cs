using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        //租户名称
        public string TenancyName { get; set; }
    }
}
