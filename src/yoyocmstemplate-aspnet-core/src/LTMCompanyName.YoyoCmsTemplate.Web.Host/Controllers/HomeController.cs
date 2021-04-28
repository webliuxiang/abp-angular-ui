using Abp.Auditing;
using Abp.Notifications;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    public class HomeController : YoyoCmsTemplateControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;

        public HomeController(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Monitor");

            //return Redirect("/swagger");
        }

        /// <summary>
        /// 演示如何向默认的租户管理员和主机管理员发送通知，是生产环境中，不推荐使用。所以默认注释掉
        /// </summary>
        /// <param name="message"> </param>
        /// <returns> </returns>
        //public async Task<ActionResult> TestNotification(string message = "")
        //{
        //    if (message.IsNullOrEmpty())
        //    {
        //        message = "This is a test notification, created at " + Clock.Now;
        //    }

        // var defaultTenantAdmin = new UserIdentifier(1, 2); var hostAdmin = new
        // UserIdentifier(null, 1);

        // await _notificationPublisher.PublishAsync( "App.SimpleMessage", new MessageNotificationData(message),
        // severity: NotificationSeverity.Info,
        // userIds: new[] { defaultTenantAdmin, hostAdmin } );

        //    return Content("Sent notification: " + message);
        //}
    }
}