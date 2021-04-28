using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class GetLanguageForEditOutput
    {
        public GetLanguageForEditOutput()
        {
            LanguageNames = new List<ComboboxItemDto>();
            Flags = new List<ComboboxItemDto>();
        }

        /// <summary>
        ///     编辑的语言信息
        /// </summary>
        public LanguageEditDto Language { get; set; }

        /// <summary>
        ///     所有的语言名称
        /// </summary>
        public ICollection<ComboboxItemDto> LanguageNames { get; set; }

        /// <summary>
        ///     所有的国旗
        /// </summary>
        public ICollection<ComboboxItemDto> Flags { get; set; }
    }
}