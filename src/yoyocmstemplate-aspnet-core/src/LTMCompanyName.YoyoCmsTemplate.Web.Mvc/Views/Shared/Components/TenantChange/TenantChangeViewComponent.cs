using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views.Shared.Components.TenantChange
{
    public class TenantChangeViewComponent : YoyoCmsTemplateViewComponent
    {
        private readonly ISessionAppService _sessionAppService;

        public TenantChangeViewComponent(ISessionAppService sessionCache)
        {
            _sessionAppService = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginInfo = await _sessionAppService.GetCurrentLoginInformations();
            var model = ObjectMapper.Map<TenantChangeViewModel>(loginInfo);
            return View(model);
        }
    }
}
