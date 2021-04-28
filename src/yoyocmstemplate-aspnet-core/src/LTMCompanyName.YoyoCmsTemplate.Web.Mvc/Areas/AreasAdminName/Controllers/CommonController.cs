using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common.Modals;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize]
    public class CommonController : YoyoCmsTemplateControllerBase
    {
        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }

        public PartialViewResult EntityTypeHistoryModal(EntityHistoryModalViewModel input)
        {
            return PartialView("Modals/_EntityTypeHistoryModal", ObjectMapper.Map<EntityHistoryModalViewModel>(input));
        }
    }
}