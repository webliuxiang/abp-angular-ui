using System.Linq;
using System.Threading.Tasks;
using Abp.Localization;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.TopBar
{
    public class TopBarViewComponent : YoyoCmsTemplateViewComponent
    {
        private readonly ILanguageManager _languageManager;

        public TopBarViewComponent(ILanguageManager languageManager)
        {
            _languageManager = languageManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Yield();

            var topbarLanguages = _languageManager.GetLanguages().Where(l => !l.IsDisabled).ToList();


            var topbarCurrentLanguage = _languageManager.CurrentLanguage;


            var topbarModel = new TopBarViewModel
            {
                Languages = topbarLanguages,
                CurrentLanguage = topbarCurrentLanguage,
                Logined = AbpSession.UserId.HasValue
            };

            return View(topbarModel);

        }

    }
}