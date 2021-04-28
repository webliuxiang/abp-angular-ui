using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class CreateOrUpdateLanguageInput
    {
        [Required] public LanguageEditDto Language { get; set; }
    }
}