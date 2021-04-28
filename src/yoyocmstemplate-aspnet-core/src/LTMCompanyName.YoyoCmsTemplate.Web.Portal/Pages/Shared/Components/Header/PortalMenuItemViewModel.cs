using Abp.Application.Navigation;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.Header
{
    /// <summary>
    /// 门户菜单子项视图模型
    /// </summary>
    public class PortalMenuItemViewModel
    {
        public UserMenuItem MenuItem { get; set; }

        public int CurrentLevel { get; set; }

        public string CurrentPageName { get; set; }

        public PortalMenuItemViewModel(UserMenuItem menuItem, int currentLevel, string currentPageName)
        {
            MenuItem = menuItem;
            CurrentLevel = currentLevel;
            CurrentPageName = currentPageName;
        }
    }
}