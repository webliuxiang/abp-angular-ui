using Abp.AspNetCore.Mvc.RazorPages;
using Abp.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages
{
    public class YoyoCmsTemplatePageModel: AbpPageModel
    { /// <summary>
      /// Gets the root path of the application.
      /// </summary>
        public string ApplicationPath
        {
            get
            {
                var appPath = HttpContext.Request.PathBase.Value;
                if (appPath == null)
                {
                    return "/";
                }

                appPath = appPath.EnsureEndsWith('/');

                return appPath;
            }
        }


        public string HeaderTitle { get; set; }

        protected YoyoCmsTemplatePageModel()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }
    }
}
