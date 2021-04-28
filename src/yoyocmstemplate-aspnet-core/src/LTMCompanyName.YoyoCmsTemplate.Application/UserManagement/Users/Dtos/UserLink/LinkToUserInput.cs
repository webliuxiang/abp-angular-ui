using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos.UserLink
{
    public class LinkToUserInput
    {
        public string TenancyName { get; set; }

        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }
    }
}