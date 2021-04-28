using System.ComponentModel.DataAnnotations;
using Abp.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    /// <summary>
    ///     更新语言输入信息
    /// </summary>
    public class UpdateLanguageTextInput
    {
        /// <summary>
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public string LanguageName { get; set; }

        /// <summary>
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguageText.MaxSourceNameLength)]
        public string SourceName { get; set; }

        /// <summary>
        ///     键
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguageText.MaxKeyLength)]
        public string Key { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(ApplicationLanguageText.MaxValueLength)]
        public string Value { get; set; }
    }
}