using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [DisableAuditing]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_AuditLogs)]
    public class AuditLogsController : YoyoCmsTemplateControllerBase
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogsController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public ActionResult Index()
        {
            return View();
        }


    }
}