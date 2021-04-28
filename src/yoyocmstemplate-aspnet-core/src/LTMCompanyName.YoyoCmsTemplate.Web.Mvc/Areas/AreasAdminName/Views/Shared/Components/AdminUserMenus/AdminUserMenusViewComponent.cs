using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Startup;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views;
using Microsoft.AspNetCore.Mvc;


namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminUserMenus
{
    public class AdminUserMenusViewComponent : YoyoCmsTemplateViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly TenantManager _tenantManager;


        public AdminUserMenusViewComponent(IUserNavigationManager userNavigationManager, IAbpSession abpSession, TenantManager tenantManager)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _tenantManager = tenantManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentPageName = null)
        {
            var model = new AdminUserMenusViewModel
            {
                Menu = await _userNavigationManager.GetMenuAsync(AppAdminNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
                CurrentPageName = currentPageName
            };


            if (AbpSession.TenantId == null)
            {
                return View(model);
            }

            var tenant = await _tenantManager.GetByIdAsync(AbpSession.TenantId.Value);
            if (tenant.EditionId.HasValue)
            {
                return View(model);
            }



            return View(model);
        }



    }
}
