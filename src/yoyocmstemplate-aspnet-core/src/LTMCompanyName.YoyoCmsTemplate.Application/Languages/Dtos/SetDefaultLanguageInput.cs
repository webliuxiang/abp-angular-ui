using System.ComponentModel.DataAnnotations;
using Abp.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class SetDefaultLanguageInput
    {
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}