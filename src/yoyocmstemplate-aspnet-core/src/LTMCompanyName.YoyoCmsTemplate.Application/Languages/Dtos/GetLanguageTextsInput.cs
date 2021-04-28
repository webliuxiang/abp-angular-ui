using System.ComponentModel.DataAnnotations;
using Abp.Extensions;
using Abp.Localization;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class GetLanguageTextsInput : PagedSortedAndFilteredInputDto
    {
        /// <summary>
        ///     语言名称
        /// </summary>
        [Required]
        [MaxLength(ApplicationLanguageText.MaxSourceNameLength)]
        public string SourceName { get; set; }

        /// <summary>
        ///     原语言名称
        /// </summary>
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public string BaseLanguageName { get; set; }

        /// <summary>
        ///     目标语言名称
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength, MinimumLength = 2)]
        public string TargetLanguageName { get; set; }

        /// <summary>
        ///     目标值过滤
        /// </summary>
        public string TargetValueFilter { get; set; }

        public void Normalize()
        {
            if (TargetValueFilter.IsNullOrEmpty()) TargetValueFilter = "ALL";
        }
    }
}