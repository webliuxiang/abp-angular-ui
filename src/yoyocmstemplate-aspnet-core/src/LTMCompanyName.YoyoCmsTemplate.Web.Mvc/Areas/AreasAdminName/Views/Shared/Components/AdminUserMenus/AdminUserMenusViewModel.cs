using Abp.Application.Navigation;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminUserMenus
{
    /// <summary>
    /// 左侧菜单服务
    /// </summary>
    public class AdminUserMenusViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }

    /// <summary>
    /// 菜单子项
    /// </summary>
    public class UserMenuItemViewModel
    {
        public UserMenuItem MenuItem { get; set; }

        public string CurrentPageName { get; set; }

        public int MenuItemIndex { get; set; }

        public int ItemDepth { get; set; }

        public bool RootLevel { get; set; }

        public bool IsTabMenuUsed { get; set; }
    }

}