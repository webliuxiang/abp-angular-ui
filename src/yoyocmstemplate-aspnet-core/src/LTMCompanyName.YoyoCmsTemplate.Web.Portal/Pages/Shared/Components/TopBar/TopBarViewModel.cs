using System.Collections.Generic;
using Abp.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.TopBar
{
    public class TopBarViewModel
    {

        public IReadOnlyList<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public bool Logined { get; set; }
    }
}