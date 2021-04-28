using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Models
{
    public class TopBarViewModel
    {
        public IReadOnlyList<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public bool Logined { get; set; }
    }
}
