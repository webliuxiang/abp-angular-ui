using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Languages;
using LTMCompanyName.YoyoCmsTemplate.Languages.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Seed.Host;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Localization
{
    // ReSharper disable once InconsistentNaming
    public class LanguageAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly ILanguageAppService _languageAppService;
        private readonly IApplicationLanguageManager _languageManager;
        //private readonly bool _multiTenancyEnabled = AppConsts.MultiTenancyEnabled;

        public LanguageAppService_Tests()
        {
            //if (_multiTenancyEnabled)
            //{
            //    LoginAsHostAdmin();
            //}
            //else
            //{
            //    LoginAsDefaultTenantAdmin();
            //}

            _languageAppService = Resolve<ILanguageAppService>();
            _languageManager = Resolve<IApplicationLanguageManager>();
        }

        [Fact]
        public async Task Test_GetLanguages()
        {
            //Act
            var output = await _languageAppService.GetLanguages();

            //Assert
            output.Items.Count.ShouldBe(DefaultLanguagesCreator.InitialLanguages.Count);
        }

        [MultiTenantFact]
        public async Task Create_Language()
        {
            //Act
            var output = await _languageAppService.GetLanguageForEdit(new NullableIdDto(null));

            //Assert
            output.Language.Id.ShouldBeNull();
            output.LanguageNames.Count.ShouldBeGreaterThan(0);
            output.Flags.Count.ShouldBeGreaterThan(0);

            //Arrange
            var currentLanguages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);
            var nonRegisteredLanguages = output.LanguageNames.Where(l => currentLanguages.All(cl => cl.Name != l.Value)).ToList();

            //Act
            var newLanguageName = nonRegisteredLanguages[RandomHelper.GetRandom(nonRegisteredLanguages.Count)].Value;
            await _languageAppService.CreateOrUpdateLanguage(
                new CreateOrUpdateLanguageInput
                {
                    Language = new LanguageEditDto
                    {
                        Icon = "icon",
                        Name = newLanguageName
                    }
                });

            //Assert
            currentLanguages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);
            currentLanguages.Count(l => l.Name == newLanguageName).ShouldBe(1);
        }

        [MultiTenantFact]
        public async Task Delete_Language()
        {
            //Arrange
            var currentLanguages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);
            var randomLanguage = RandomHelper.GetRandomOf(currentLanguages.ToArray());

            //Act
            await _languageAppService.DeleteLanguage(new EntityDto(randomLanguage.Id));

            //Assert
            currentLanguages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);
            currentLanguages.Any(l => l.Name == randomLanguage.Name).ShouldBeFalse();
        }

        [Fact]
        public async Task SetDefaultLanguage()
        {
            //Arrange
            var currentLanguages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);
            var randomLanguage = RandomHelper.GetRandomOf(currentLanguages.ToArray());

            //Act
            await _languageAppService.SetDefaultLanguage(
                new SetDefaultLanguageInput
                {
                    Name = randomLanguage.Name
                });

            //Assert
            var defaultLanguage = await _languageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);

            randomLanguage.ShouldBe(defaultLanguage);
        }

        [Fact]
        public async Task UpdateLanguageText()
        {
            await _languageAppService.UpdateLanguageText(
                new UpdateLanguageTextInput
                {
                    SourceName = AppConsts.LocalizationSourceName,
                    LanguageName = "en",
                    Key = "Save",
                    Value = "save-new-value"
                });

            var newValue = Resolve<ILocalizationManager>()
                .GetString(
                    AppConsts.LocalizationSourceName,
                    "Save",
                    CultureInfo.GetCultureInfo("en")
                );

            newValue.ShouldBe("save-new-value");
        }

        [Fact]
        public async Task SetLanguageIsDisabled()
        {
            //Arrange
            var currentEnabledLanguages =
                (await _languageManager.GetLanguagesAsync(AbpSession.TenantId)).Where(l => !l.IsDisabled);
            var randomEnabledLanguage = RandomHelper.GetRandomOf(currentEnabledLanguages.ToArray());

            //Act
            var output = await _languageAppService.GetLanguageForEdit(new NullableIdDto(null));

            //Act
            await _languageAppService.CreateOrUpdateLanguage(
                new CreateOrUpdateLanguageInput
                {
                    Language = new LanguageEditDto
                    {
                        Id = randomEnabledLanguage.Id,
                        IsEnabled = false,
                        Name = randomEnabledLanguage.Name,
                        Icon = "icon"
                    }
                });

            //Assert
            var languages = await _languageManager.GetLanguagesAsync(AbpSession.TenantId);

            var language = languages.FirstOrDefault(l => l.Name == randomEnabledLanguage.Name);
            language.ShouldNotBe(null);
            language.IsDisabled.ShouldBeTrue();
        }
    }
}
