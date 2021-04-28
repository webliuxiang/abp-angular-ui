using System;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Extensions;
using Abp.Runtime.Session;
using L._52ABP.Common.Net.MimeTypes;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Controllers
{
    //[AbpMvcAuthorize]
    [DisableAuditing]
    public class ProfileController : UploadFilesControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IDataFileObjectManager _dataFileObjectManager;

        public ProfileController(IAppFolder appFolder, IDataFileObjectManager dataFileObjectManager, UserManager userManager, IDataTempFileCacheManager dataTempFileCacheManager) : base(appFolder, dataFileObjectManager, dataTempFileCacheManager)
        {
            _dataFileObjectManager = dataFileObjectManager;
            _userManager = userManager;
        }

        public async Task<FileResult> GetProfilePicture()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user.ProfilePictureId == null)
            {
                return GetDefaultProfilePicture();
            }

            return await GetProfilePictureById(user.ProfilePictureId.Value);
        }

        public async Task<FileResult> GetProfilePictureById(string id = "")
        {
            if (id.IsNullOrEmpty())
            {
                return GetDefaultProfilePicture();
            }

            return await GetProfilePictureById(Guid.Parse(id));
        }




        private async Task<FileResult> GetProfilePictureById(Guid profilePictureId)
        {
            var file = await _dataFileObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return GetDefaultProfilePicture();
            }

            return File(file.Bytes, MimeTypeNames.ImageJpeg);
        }

    }
}
