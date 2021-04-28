using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    public class ChangeUserLanguageDto
    {
        [Required] public string LanguageName { get; set; }
    }
}