using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Localization;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Languages.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Languages
{
    /// <summary>
    ///     Language应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Languages)]
    public class LanguageAppService : YoyoCmsTemplateAppServiceBase, ILanguageAppService
    {
        private readonly IApplicationLanguageManager _languageManager;
        private readonly IRepository<ApplicationLanguage> _languageRepository;
        private readonly IApplicationLanguageTextManager _languageTextManager;

        /// <summary>
        ///     构造函数
        /// </summary>
        public LanguageAppService(
            IRepository<ApplicationLanguage> languageRepository,
            IApplicationLanguageManager languageManager,
            IApplicationLanguageTextManager languageTextManager
        )
        {
            _languageRepository = languageRepository;
            _languageManager = languageManager;
            _languageTextManager = languageTextManager;
        }

        public async Task<GetLanguagesOutput> GetLanguages()
        {
            var languages = (await _languageManager.GetLanguagesAsync(AbpSession.TenantId)).OrderBy(l => l.DisplayName);
            var defaultLanguage = await _languageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);

            return new GetLanguagesOutput
            {
                Items = ObjectMapper.Map<List<LanguageListDto>>(languages),
                DefaultLanguageName = defaultLanguage?.Name
            };
        }

        public async Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input)
        {
            if (input.Language.Id.HasValue)
                await UpdateLanguageAsync(input);
            else
                await CreateLanguageAsync(input);
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Languages_Create,
            YoyoSoftPermissionNames.Pages_Administration_Languages_Edit)]
        public async Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input)
        {
            ApplicationLanguage language = null;
            if (input.Id.HasValue) language = await _languageRepository.GetAsync(input.Id.Value);

            var output = new GetLanguageForEditOutput();

            //Language
            output.Language = language != null ? ObjectMapper.Map<LanguageEditDto>(language) : new LanguageEditDto();

            //Language names
            output.LanguageNames = CultureHelper
                .Cultures
                .Select(c => new ComboboxItemDto(c.Name, $"{c.EnglishName} ({c.Name})")
                {
                    IsSelected = output.Language.Name == c.Name
                })
                .ToList();

            //Flags
            output.Flags = FlagsHelper.GetFlags(output.Language?.Icon);


            return output;
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Languages_Delete)]
        public async Task DeleteLanguage(EntityDto input)
        {
            var language = await _languageRepository.GetAsync(input.Id);
            await _languageManager.RemoveAsync(AbpSession.TenantId, language.Name);
        }


        public async Task SetDefaultLanguage(SetDefaultLanguageInput input)
        {
            await _languageManager.SetDefaultLanguageAsync(
                AbpSession.TenantId,
                CultureHelper.GetCultureInfo(input.Name).Name
            );
        }


        public async Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input)
        {
            if (input.BaseLanguageName.IsNullOrEmpty())
            {
                var defaultLanguage = await _languageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);
                if (defaultLanguage == null)
                {
                    defaultLanguage = (await _languageManager.GetLanguagesAsync(AbpSession.TenantId)).FirstOrDefault();
                    if (defaultLanguage == null) throw new Exception("No language found in the application!");
                }

                input.BaseLanguageName = defaultLanguage.Name;
            }

            var source = LocalizationManager.GetSource(input.SourceName);
            var baseCulture = CultureInfo.GetCultureInfo(input.BaseLanguageName);
            var targetCulture = CultureInfo.GetCultureInfo(input.TargetLanguageName);

            var languageTexts = source
                .GetAllStrings()
                .Select(localizedString => new LanguageTextListDto
                {
                    Key = localizedString.Name,
                    BaseValue = _languageTextManager.GetStringOrNull(AbpSession.TenantId, source.Name, baseCulture,
                        localizedString.Name),
                    TargetValue = _languageTextManager.GetStringOrNull(AbpSession.TenantId, source.Name, targetCulture,
                        localizedString.Name, false)
                })
                .AsQueryable();

            //Filters
            if (input.TargetValueFilter == "EMPTY")
                languageTexts = languageTexts.Where(s => s.TargetValue.IsNullOrEmpty());

            if (!input.FilterText.IsNullOrEmpty())
                languageTexts = languageTexts.Where(
                    l => l.Key != null &&
                         l.Key.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                         l.BaseValue != null &&
                         l.BaseValue.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                         l.TargetValue != null &&
                         l.TargetValue.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0
                );

            var totalCount = languageTexts.Count();

            //Ordering
            if (!input.Sorting.IsNullOrEmpty()) languageTexts = languageTexts.OrderBy(input.Sorting);

            //Paging
            if (input.SkipCount > 0) languageTexts = languageTexts.Skip(input.SkipCount);

            if (input.MaxResultCount > 0) languageTexts = languageTexts.Take(input.MaxResultCount);

            return new PagedResultDto<LanguageTextListDto>(
                totalCount,
                languageTexts.ToList()
            );
        }

        /// <summary>
        ///     修改语言的文本内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateLanguageText(UpdateLanguageTextInput input)
        {
            var culture = CultureHelper.GetCultureInfo(input.LanguageName);
            var source = LocalizationManager.GetSource(input.SourceName);
            await _languageTextManager.UpdateStringAsync(AbpSession.TenantId, source.Name, culture, input.Key,
                input.Value);
        }


        /// <summary>
        ///     创建语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Languages_Create)]
        protected virtual async Task CreateLanguageAsync(CreateOrUpdateLanguageInput input)
        {
            var culture = CultureHelper.GetCultureInfo(input.Language.Name);

            if (await LanguageExists(culture.Name)) throw new UserFriendlyException(L("ThisLanguageAlreadyExists"));

            var newLanguage = new ApplicationLanguage
            {
                TenantId = AbpSession.TenantId,
                Name = culture.Name,
                DisplayName = culture.DisplayName,
                Icon = input.Language.Icon,
                IsDisabled = !input.Language.IsEnabled
            };

            await _languageManager.AddAsync(newLanguage);
        }

        /// <summary>
        ///     更新语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Languages_Edit)]
        protected virtual async Task UpdateLanguageAsync(CreateOrUpdateLanguageInput input)
        {
            Debug.Assert(input.Language.Id != null, "input.Language.Id != null");

            var culture = CultureHelper.GetCultureInfo(input.Language.Name);

            if (await LanguageExists(culture.Name, input.Language.Id.Value))
            {
                throw new UserFriendlyException(L("ThisLanguageAlreadyExists"));
            }

            var language = await _languageRepository.GetAsync(input.Language.Id.Value);

            language.Name = culture.Name;
            language.DisplayName = culture.DisplayName;
            language.Icon = input.Language.Icon;
            language.IsDisabled = !input.Language.IsEnabled;

            await _languageManager.UpdateAsync(AbpSession.TenantId, language);
        }

        /// <summary>
        ///     检查语言是否已存在
        /// </summary>
        /// <param name="languageName"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        private async Task<bool> LanguageExists(string languageName, int? languageId = null)
        {
            var languages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);

            var queryResult = languages.FirstOrDefault(l => l.Name == languageName);

            if (queryResult == null
                || languageId != null && queryResult.Id == languageId.Value)
                return false;

            return true;
        }





        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Languages_Delete)]
        public async Task BatchDelete(List<int> ids)
        {
            var languages = await _languageRepository.GetAllListAsync(l => ids.Contains(l.Id));
            foreach (var language in languages)
            {

                await _languageManager.RemoveAsync(AbpSession.TenantId, language.Name);
            }
        }
    }
}