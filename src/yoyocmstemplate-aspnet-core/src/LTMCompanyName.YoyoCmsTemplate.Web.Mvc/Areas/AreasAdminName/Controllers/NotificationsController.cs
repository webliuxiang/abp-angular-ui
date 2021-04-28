using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize]
    public class NotificationsController : YoyoCmsTemplateControllerBase
    {
        private readonly INotificationAppService _notificationApp;

        public NotificationsController(INotificationAppService notificationApp)
        {
            _notificationApp = notificationApp;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> SettingsModal()
        {
            var notificationSettings = await _notificationApp.GetNotificationSettings();
            return PartialView("_SettingsModal", notificationSettings);
        }
    }
}