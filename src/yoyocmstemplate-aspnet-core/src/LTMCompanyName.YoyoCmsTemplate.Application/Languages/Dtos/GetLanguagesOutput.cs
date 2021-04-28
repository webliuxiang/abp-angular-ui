using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class GetLanguagesOutput : ListResultDto<LanguageListDto>
    {
        /// <summary>
        ///     默认语言名称
        /// </summary>
        public string DefaultLanguageName { get; set; }
    }
}