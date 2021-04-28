using System.ComponentModel.DataAnnotations;
using Abp.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class LanguageEditDto
    {
        public virtual int? Id { get; set; }

        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ApplicationLanguage.MaxIconLength)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}