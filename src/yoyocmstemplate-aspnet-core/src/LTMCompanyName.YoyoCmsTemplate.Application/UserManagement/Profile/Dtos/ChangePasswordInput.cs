using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    public class ChangePasswordInput
    {
        [Required] [DisableAuditing] public string CurrentPassword { get; set; }

        [Required] [DisableAuditing] public string NewPassword { get; set; }
    }
}