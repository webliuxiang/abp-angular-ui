using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Languages.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Languages
{
    /// <summary>
    ///     Language应用层服务的接口方法
    /// </summary>
    public interface ILanguageAppService : IApplicationService
    {
        /// <summary>
        ///     获取所有语言
        /// </summary>
        /// <returns></returns>
        Task<GetLanguagesOutput> GetLanguages();

        /// <summary>
        ///     获取语言编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input);

        /// <summary>
        ///     创建或更新语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input);

        /// <summary>
        ///     删除语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteLanguage(EntityDto input);

        /// <summary>
        ///     设置默认语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetDefaultLanguage(SetDefaultLanguageInput input);

        /// <summary>
        ///     分页获取一个语言的文本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input);

        /// <summary>
        ///     更新一个文本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateLanguageText(UpdateLanguageTextInput input);


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task BatchDelete(List<int> ids);
    }
}