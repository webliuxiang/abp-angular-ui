using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos.Enqueue;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    public abstract class UsersControllerBase : YoyoCmsTemplateControllerBase
    {
        private readonly IDataFileObjectManager _dataFileObjectManager;

        protected readonly IBackgroundJobManager BackgroundJobManager;

        protected UsersControllerBase(IDataFileObjectManager dataFileObjectManager,

            IBackgroundJobManager backgroundJobManager)
        {
            BackgroundJobManager = backgroundJobManager;
            _dataFileObjectManager = dataFileObjectManager;
        }

        [HttpPost]
        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Create)]
        public async Task<JsonResult> ImportFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new DataFileObject(tenantId, fileBytes);

                await _dataFileObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportUsersToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
