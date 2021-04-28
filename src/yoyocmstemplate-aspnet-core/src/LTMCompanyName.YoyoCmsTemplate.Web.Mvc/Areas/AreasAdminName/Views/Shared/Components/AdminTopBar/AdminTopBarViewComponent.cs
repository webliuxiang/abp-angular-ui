using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminTopBar
{
    /// <summary>
    /// 管理端顶部菜单栏视图组件
    /// </summary>
    public class AdminTopBarViewComponent : YoyoCmsTemplateViewComponent
    {

        private readonly ILanguageManager _languageManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ISessionAppService _sessionAppService;

        private readonly IAbpSession _abpSession;


        public AdminTopBarViewComponent(ILanguageManager languageManager, IMultiTenancyConfig multiTenancyConfig, ISessionAppService sessionAppService, IAbpSession abpSession)
        {
            _languageManager = languageManager;
            _multiTenancyConfig = multiTenancyConfig;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AdminTopBarViewModel()
            {
                LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
                Languages = _languageManager.GetLanguages().Where(l => !l.IsDisabled).ToList(),
                CurrentLanguage = _languageManager.CurrentLanguage,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                IsImpersonatedLogin = _abpSession.ImpersonatorUserId.HasValue,
            };

            return View(model);
        }
    }
}
