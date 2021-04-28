using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Controllers
{
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IDataFileObjectManager dataFileObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(dataFileObjectManager, backgroundJobManager)
        {
        }
    }
}