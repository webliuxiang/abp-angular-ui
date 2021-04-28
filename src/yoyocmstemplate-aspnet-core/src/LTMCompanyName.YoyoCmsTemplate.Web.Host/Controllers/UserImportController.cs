using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    [AbpMvcAuthorize]
    public class UserImportController : UsersControllerBase
    {
        public UserImportController(IDataFileObjectManager dataFileObjectManager, IBackgroundJobManager backgroundJobManager) : base(dataFileObjectManager, backgroundJobManager)
        {
        }
    }
}